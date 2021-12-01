#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Payment.csproj", "."]
RUN dotnet restore "./Payment.csproj"
COPY . .
WORKDIR "/src/."
ENV ASPNETCORE_ENVIRONMENT=Production
ENV PATH="${PATH}:/root/.dotnet/tools"
CMD DATABASE_URL= $DATABASE_URL
RUN dotnet build "Payment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .



#ENTRYPOINT ["dotnet", "Payment.dll"]

CMD ASPNETCORE_URLS=http://*:$PORT dotnet Payment.dll