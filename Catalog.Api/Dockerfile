FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Catalog.Api/Catalog.Api.csproj", "Catalog.Api/"]
RUN dotnet restore "Catalog.Api/Catalog.Api.csproj"
COPY . .
WORKDIR "/src/Catalog.Api"
RUN dotnet build "Catalog.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Catalog.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]