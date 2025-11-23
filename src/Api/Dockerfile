# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["ContactAgenda.Api.csproj", "./"]
COPY ["../Application/ContactAgenda.Application.csproj", "../Application/"]
COPY ["../Domain/ContactAgenda.Domain.csproj", "../Domain/"]
COPY ["../Infrastructure/ContactAgenda.Infrastructure.csproj", "../Infrastructure/"]

# Restore dependencies
RUN dotnet restore "ContactAgenda.Api.csproj"

# Copy all source files
COPY . .
COPY ../Application ../Application
COPY ../Domain ../Domain
COPY ../Infrastructure ../Infrastructure

# Build
WORKDIR "/src"
RUN dotnet build "ContactAgenda.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ContactAgenda.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContactAgenda.Api.dll"]
