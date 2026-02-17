# Runtime only (runs already-published binaries)
# NOTE: If you are using a different .NET runtime version, adjust the tag below.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

# Keep the same folder layout as the repo root:
# /app
#   ├─ publish/api (binaries)
#   └─ SQLite (db file)
WORKDIR /app

# Copy published output and SQLite folder into the image
# Build context must be the repo root (see docker-compose.yml).
COPY publish/ ./publish/
COPY SQLite/ ./SQLite/

# Expose the port that Kestrel will listen on inside the container
EXPOSE 8080

# Bind to all interfaces so the port mapping works
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the API from the published binaries
ENTRYPOINT ["dotnet", "publish/api/MM.Api.dll"]