FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["WFM.OutOfStock.API/WFM.OutOfStock.API.csproj", "WFM.OutOfStock.API/"]
RUN dotnet restore "WFM.OutOfStock.API/WFM.OutOfStock.API.csproj"
COPY . .
WORKDIR "/src/WFM.OutOfStock.API"
RUN dotnet build "WFM.OutOfStock.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WFM.OutOfStock.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WFM.OutOfStock.API.dll"]