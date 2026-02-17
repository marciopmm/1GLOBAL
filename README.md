# MM - Rest API

| Version | Updated By | Updated At |
|:-------:|:----------:|:----------:|
|v0.1.0|Marcio Martins|14/02/2026|

## What is MM Rest API?
MM Rest API is a solution proposed by me (Marcio Martins) as a resolution for a Recruiting Test subimited by MM to myself.

## About this document
This cover the most features about MM Rest API, including usage, error messages and known isses.

## How to install
1GLOBAL Rest API runs in a Docker Container. It can be started by running `docker compose up -d`.

## How to check for API Documentation
Please refer to `http://localhost:8000/swagger` to check up API Documentation in OpenAPI format.

## How to deploy container to run the API
- Get in the solution root folder
- Run the following commands
```
dotnet publish Api/MM.Api.csproj -c Development -o publish/api
cd Docker
docker compose build --no-cache
docker compose up
```
After these steps, API may be found in `http://localhost:8000`. If you wanna make a quick test, run (in a Linux terminal):
```
curl -X 'GET' 'http://localhost:8080/devices'
```

You will receive something like this:
```
[{"id":"7dadea7f-0512-4907-9f98-ff96d8beabf2","name":"Redmi Note 15 Pro 5G","brand":"Redmi","state":0,"creationTime":"2026-02-15T23:21:17.6006814"},{"id":"2fc0b8bb-f4d1-4471-a2ce-1e962cb10592","name":"Vaio Intel i9 512GB - 32GB RAM","brand":"Sony","state":0,"creationTime":"2026-02-16T11:14:54.1730595"}]
```

