FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5014

ENV ASPNETCORE_URLS=http://+:5014

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["devopsproyect/devopsproyect.csproj", "devopsproyect/"]
RUN dotnet restore "devopsproyect/devopsproyect.csproj"
COPY . .
WORKDIR "/src/devopsproyect"
RUN dotnet build "devopsproyect.csproj" -c Release -o /app/build

FROM --platform=linux/amd64 build AS publish
RUN dotnet publish "devopsproyect.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM --platform=linux/amd64 base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "devopsproyect.dll"]
