FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["NPO-Client/NPO-Client.csproj", "NPO-Client/"]
RUN dotnet restore "NPO-Client/NPO-Client.csproj"
COPY . .
WORKDIR "/src/NPO-Client"
RUN dotnet build "NPO-Client.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NPO-Client.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NPO-Client.dll"]
