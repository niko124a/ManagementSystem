FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AutoMechanicManagementSystem.sln", "./"]
COPY ["Website/Website.csproj", "Website/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
COPY ["DatabaseAccess/DatabaseAccess.csproj", "DatabaseAccess/"]
RUN dotnet restore "Website/Website.csproj"
COPY . .
WORKDIR "/src/Website"
RUN dotnet build "Website.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Website.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Website.dll"]
