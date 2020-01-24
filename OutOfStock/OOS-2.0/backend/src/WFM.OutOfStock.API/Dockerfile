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
#Setting the ARG at build time to the environment we are building this image for. Defaults to Development
ARG ENV=Development
#Setting the ENV2 variable to the ARG ENV concatenated with the "--environment" flag to pass to dotnet so when we call docker run
#it will run the website with the appropriate environment. For example, by default this will result in "--environment=Development"
ENV ENV2=--environment=$ENV
ENTRYPOINT dotnet WFM.OutOfStock.API.dll $ENV2