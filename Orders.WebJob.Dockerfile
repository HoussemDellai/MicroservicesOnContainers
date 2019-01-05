FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src

# build Catalog.Api
ARG path="Orders.WebJob"
ARG projectPath="Orders.WebJob/Orders.WebJob.csproj"
COPY ${projectPath} ${projectPath}
RUN dotnet restore ${projectPath}
COPY ${path} ${path}
RUN dotnet build ${projectPath} -c Release -o /app

FROM build AS publish
RUN dotnet publish "Orders.WebJob/Orders.WebJob.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Orders.WebJob.dll"]