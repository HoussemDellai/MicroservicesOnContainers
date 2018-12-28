FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["/Basket.Api.csproj", "Basket.Api/"]
RUN dotnet restore "Basket.Api/Basket.Api.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Basket.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Basket.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Basket.Api.dll"]