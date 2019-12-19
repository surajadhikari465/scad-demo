#Create base image using ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#Create build image for the backend using ASP.NET Core SDK image
FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["IrmaMobile/IrmaMobile.csproj", "IrmaMobile/"]
RUN dotnet restore "IrmaMobile/IrmaMobile.csproj"
COPY . .
WORKDIR "/src/IrmaMobile"
RUN dotnet build "IrmaMobile.csproj" -c Release -o /app/build

#Publish backend with release configuration
FROM build AS publish
RUN dotnet publish "IrmaMobile.csproj" -c Release -o /app/publish

#Create final image using base image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish /app
ENTRYPOINT ["dotnet", "IrmaMobile.dll"]