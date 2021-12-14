using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Payment.Configurations;
using Payment.Data;
using Payment.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly JwtConfig jwtConfig;
        private TokenValidationParameters tokenValidationParams;
        private readonly PaymentDbContext paymentDbContext;



        public AuthService(UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor,
            TokenValidationParameters tokenValidationParams,
            PaymentDbContext apiDbContext)
        {
            this.userManager = userManager;
            this.jwtConfig = optionsMonitor.CurrentValue;
            this.tokenValidationParams = tokenValidationParams;
            this.paymentDbContext = apiDbContext;
        }

        public UserManager<IdentityUser> UserManager()
        {
            return userManager;
        }
        public async Task<AuthResponse> GenerateJwToken(IdentityUser user)
        {


            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddSeconds(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);


            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6),
                Token = RandomString(10) + Guid.NewGuid()
            };


            await paymentDbContext.RefreshTokens.AddAsync(refreshToken);
            await paymentDbContext.SaveChangesAsync();

            return new AuthResponse()
            {
                Message = "Get jwt token and refresh token",
                Success = true,
                RefreshToken = refreshToken.Token,
                Token = jwtToken
            };
        }


        public async void RevokeRefreshToken(TokenRequest tokenRequest)
        {
            var refreshToken = await paymentDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);
            refreshToken.IsRevoked = true;
            paymentDbContext.RefreshTokens.Update(refreshToken);
            await paymentDbContext.SaveChangesAsync();
        }

        public async Task<AuthResponse> CheckJwtToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenInverification = jwtTokenHandler.ValidateToken(tokenRequest.Token, tokenValidationParams, out var validatedToken);


                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false) return null;
                }

                var utcExpiryDate = long.Parse(tokenInverification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                // 
                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthResponse()
                    {
                        Success = true,
                        Message = "JWT Token is not expired"
                    };
                }
                return new AuthResponse()
                {
                    Success = false,
                    Message = "Something wrong"
                };
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Lifetime validation failed. The token is expired"))
                {
                    return new AuthResponse()
                    {
                        Success = true,
                        Message = "JWT Token is expired"
                    };
                }
                return new AuthResponse()
                {
                    Success = false,
                    Message = "Something wrong"
                };
            }
        }


        public async Task<AuthResponse> RefreshJwtToken(TokenRequest tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // validate token lifetime
                // if expired it will be exception
                var tokenInverification = jwtTokenHandler.ValidateToken(tokenRequest.Token, tokenValidationParams, out var validatedToken);


                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (result == false) return null;
                }

                var utcExpiryDate = long.Parse(tokenInverification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                // 
                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthResponse()
                    {
                        Success = true,
                        Message = "JWT Token is not expired"
                    };
                }

                return new AuthResponse()
                {
                    Success = false,
                    Message = "Something wrong"
                };

            }
            catch (Exception e)
            {

                if (e.Message.Contains("Lifetime validation failed. The token is expired"))
                {
                    var refreshToken = await paymentDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

                    // refreshtoken not found in database
                    if (refreshToken == null)
                    {
                        return new AuthResponse()
                        {
                            Success = false,
                            Message = "RefreshToken not found in database"
                        };
                    }

                    // refreshtoken has been used, so use other refreshtoken
                    if (refreshToken.IsUsed)
                    {
                        return new AuthResponse()
                        {
                            Success = false,
                            Message = "RefreshToken has been used"
                        };
                    }

                    // refreshtoken has been revoke, so use other refreshtoken
                    if (refreshToken.IsRevoked)
                    {
                        return new AuthResponse()
                        {
                            Success = false,
                            Message = "Token has been revoked"
                        };
                    }

                    tokenValidationParams.ValidateLifetime = false;
                    var tokenInverification = jwtTokenHandler.ValidateToken(tokenRequest.Token, tokenValidationParams, out var validatedToken);
                    tokenValidationParams.ValidateLifetime = true;


                    var jti = tokenInverification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                    // refreshtoken and jwt token doesn't match, 
                    // make sure jwttoken and refreshtoken match
                    if (refreshToken.JwtId != jti)
                    {
                        return new AuthResponse()
                        {
                            Success = false,
                            Message = "JwtToken and Refreshtoken Doesn't match"
                        };
                    }

                    refreshToken.IsUsed = true;
                    paymentDbContext.RefreshTokens.Update(refreshToken);
                    await paymentDbContext.SaveChangesAsync();

                    var dbUser = await UserManager().FindByIdAsync(refreshToken.UserId);
                    return await GenerateJwToken(dbUser);
                }

                return new()
                {
                    Success = false,
                    Message = e.Message
                };

            }
        }

        public static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static DateTime UnixTimeStampToDateTime(long unixTimeStampt)
        {
            var dateTimeVal = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeVal = dateTimeVal.AddSeconds(unixTimeStampt).ToUniversalTime();
            return dateTimeVal;
        }


    }
}
