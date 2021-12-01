using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.Models;
using Payment.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Payment.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentDetailController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }


        // GET: api/<PaymentController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var payments = await paymentService.Get();
                return Ok(new BasicResponse
                {
                    Message = "Get all paayment",
                    Success = true,
                    Data = payments

                });

            }
            catch (System.Exception e)
            {
                return new JsonResult(
                    new BasicResponse
                    {
                        Message = e.InnerException.Message,
                        Success = false
                    })
                { StatusCode = 500 };
            }

        }

        // GET api/<PaymentController>/5
        [HttpGet("{id}", Name = "Get By Id")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var payments = await paymentService.Get(id);
                if (payments != null)
                    return Ok(
                        new BasicResponse
                        {
                            Message = "payment found",
                            Success = true,
                            Data = payments
                        });
                return NotFound(
                        new BasicResponse
                        {
                            Message = "payment not found",
                            Success = false,
                            Data = payments
                        });
            }
            catch (System.Exception e)
            {

                return new JsonResult(
                     new BasicResponse
                     {
                         Message = e.InnerException.Message,
                         Success = false
                     })
                { StatusCode = 500 };
            }


        }

        // POST api/<PaymentController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.Payment data)
        {
            try
            {
                if (data.CardNumber == null)
                    return BadRequest(
                        new BasicResponse
                        {
                            Message = "CardNumber cannot be null",
                            Success = false
                        });
                if (data.CardOwnerName == null)
                    return BadRequest(
                        new BasicResponse
                        {
                            Message = "CardOwnerName cannot be null",
                            Success = false
                        });
                if (data.SecurityCode == null)
                    return BadRequest(
                        new BasicResponse
                        {
                            Message = "SecurityCode cannot be null",
                            Success = false
                        });
                if (ModelState.IsValid)
                {
                    var id = await paymentService.Create(data);
                    var payment = await paymentService.Get(id);
                    return new JsonResult(
                        new BasicResponse
                        {
                            Message = "Create payment success",
                            Success = true,
                            Data = payment
                        })
                    { StatusCode = 201 };
                }

                return BadRequest(
                    new BasicResponse
                    {
                        Message = "Payload invalid",
                        Success = false
                    });
            }
            catch (System.Exception e)
            {
                return new JsonResult(
                    new BasicResponse
                    {
                        Message = e.InnerException.Message,
                        Success = false
                    })
                { StatusCode = 500 };
            }
        }

        // PUT api/<PaymentController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Models.Payment data)
        {
            try
            {
                if (data.CardNumber == null)
                    return BadRequest(
                        new BasicResponse
                        {
                            Message = "CardNumber cannot be null",
                            Success = false
                        });
                if (data.CardOwnerName == null)
                    return BadRequest(
                        new BasicResponse
                        {
                            Message = "CardOwnerName cannot be null",
                            Success = false
                        });
                if (data.SecurityCode == null)
                    return BadRequest(
                        new BasicResponse
                        {
                            Message = "SecurityCode cannot be null",
                            Success = false
                        });
                if (ModelState.IsValid)
                {
                    var oldPayment = await paymentService.Get(id);
                    if (oldPayment == null)
                        return NotFound(
                            new BasicResponse
                            {
                                Message = "payment not found",
                                Success = false
                            });
                    data.PaymentDetailId = id;
                    var paymentDetailId = await paymentService.Update(oldPayment, data);
                    var payment = await paymentService.Get(id);
                    return Ok(
                        new BasicResponse
                        {
                            Message = "Update payment success",
                            Success = true,
                            Data = payment
                        });
                }

                return BadRequest(
                   new BasicResponse
                   {
                       Message = "Payload invalid",
                       Success = false
                   });
            }
            catch (System.Exception e)
            {
                return new JsonResult(
                    new BasicResponse
                    {
                        Message = e.InnerException.Message,
                        Success = false
                    })
                { StatusCode = 500 };
            }
        }

        // DELETE api/<PaymentController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var payment = await paymentService.Get(id);
                if (payment == null)
                    return NotFound(
                       new BasicResponse
                       {
                           Message = "payment not found",
                           Success = false
                       });
                await paymentService.Delete(payment);
                return Ok(
                  new BasicResponse
                  {
                      Message = "Delete payment success",
                      Success = true
                  });
            }
            catch (System.Exception e)
            {
                return new JsonResult(
                       new BasicResponse
                       {
                           Message = e.InnerException.Message,
                           Success = false
                       })
                { StatusCode = 500 };
            }

        }
    }
}
