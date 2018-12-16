FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["BasketItems.Api/BasketItems.Api.csproj", "BasketItems.Api/"]
RUN dotnet restore "BasketItems.Api/BasketItems.Api.csproj"
COPY . .
WORKDIR "/src/BasketItems.Api"
RUN dotnet build "BasketItems.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BasketItems.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BasketItems.Api.dll"]