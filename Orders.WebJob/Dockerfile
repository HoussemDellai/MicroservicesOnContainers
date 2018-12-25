FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["/Orders.WebJob.csproj", "Orders.WebJob/"]
RUN dotnet restore "Orders.WebJob/Orders.WebJob.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Orders.WebJob.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Orders.WebJob.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Orders.WebJob.dll"]