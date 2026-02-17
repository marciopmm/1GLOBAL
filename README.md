# MM - Rest API

| Version | Updated By | Updated At |
|:-------:|:----------:|:----------:|
|v0.1.0|Marcio Martins|14/02/2026|
|v0.1.1|Marcio Martins|16/02/2026|
|v0.1.2|Marcio Martins|17/02/2026|

## What is MM Rest API?
MM REST API is a solution developed by me (Marcio Martins) as part of a recruiting test submitted to a tech company.

## About this document
This document covers the main features of the MM REST API, including usage, error messages, and known issues.

## How to deploy container to run the API
- Go to the solution root folder
- Run the following commands
```
dotnet publish Api/MM.Api.csproj -c Development -o publish/api
cd Docker
docker compose build --no-cache
docker compose up
```
After these steps, the API will be available at `http://localhost:8080`. If you want to run a quick test, call (in a Linux terminal):
```
curl -X 'GET' 'http://localhost:8080/devices'
```

You will receive something like this:
```
[
  {
    "id": "7dadea7f-0512-4907-9f98-ff96d8beabf2",
    "name": "Redmi Note 15 Pro 5G",
    "brand": "Redmi",
    "state": "Available",
    "creationTime": "2026-02-15T23:21:17.6006814"
  },
  {
    "id": "2fc0b8bb-f4d1-4471-a2ce-1e962cb10592",
    "name": "Vaio Intel i9 512GB - 32GB RAM",
    "brand": "Sony",
    "state": "Available",
    "creationTime": "2026-02-16T11:14:54.1730595"
  },
  {
    "id": "aa4f59d3-7689-425f-8000-d7cedbf896be",
    "name": "MacBook Air M1",
    "brand": "Apple",
    "state": "InUse",
    "creationTime": "2026-02-17T16:19:50.3753063"
  },
  {
    "id": "3ecf0011-89ea-4748-864e-a5db36f8f0e6",
    "name": "XPS 12",
    "brand": "Dell",
    "state": "Available",
    "creationTime": "2026-02-17T16:20:45.9539552"
  },
  {
    "id": "e1735fbe-7e6d-47b2-b4fb-55d5ba72d41f",
    "name": "ThinkPad X1 Carbon Gen 11",
    "brand": "Lenovo",
    "state": "Inactive",
    "creationTime": "2026-02-17T16:22:03.6945601"
  }
]
```
If you received this response from the request above... **CONGRATULATIONS**! Your API is up and running, and you can test it with any REST Tool (like Postman, Insomnia, RapiAPI, Hoppscotch, etc.)

## How to test with API Documentation
Please refer to http://localhost:8080/swagger to check the API documentation in OpenAPI format.

## Features and Resources
Let's get a little deep into the API Resources.

### Devices (http:localhost:8080/devices)
Here you can access the following HTTP methods:
- GET
- POST
- PUT
- PATCH
- DELETE

Each of these methods expose one or many functions for you to manage Devices in your API.

#### ***GET http://localhost:8080/devices***
By calling this method you will get a list of devices in the API.

#### ***GET http://localhost:8080/devices/{id}***
By calling this method followed by the Device ID, you will get a single device with the respective ID (replace `{id}` with the correct UUID code).

#### ***GET http://localhost:8080/devices/search***
By calling this method you will get a list of devices filtered by name, brand or state. You can do a call with none or multiple filter keys.

Example: 
- `http://localhost:8080/devices/search?name=XPS` - You'll get a list of devices containing "XPS" in their "name" property;
- `http://localhost:8080/devices/search?brand=Sony`- You'll get a list of devices containing "Sony" in their "brand" property;
- `http://localhost:8080/devices/search?state=Available`- You'll get a list of devices which "state" property is "Available";
- `http://localhost:8080/devices/search?brand=Apple&state=Available`- You'll get a list of devices containing "Apple" in their "brand" property which "state" property are "Available";
- `http://localhost:8080/devices/search` - You'll get a list of all devices (same as `http://localhost:8080/devices`).

#### ***POST http://localhost:8080/devices***
Calling this method with a valid body allows you to add a Device in the API.

#### ***PUT http://localhost:8080/devices/{id}***
Calling this method with a valid body allows you to update the full content of a Device in the API (replace `{id}` with the correct UUID code).

#### ***PATCH http://localhost:8080/devices/{id}***
Calling this method with a valid body allows you to partially update a device in the API (replace `{id}` with the correct UUID code).

#### ***DELETE http://localhost:8080/devices/{id}***
Calling this method allows you to delete a Device in the API (replace `{id}` with the correct UUID code).

## OpenAPI Docs
For further documentation and information related to all resources and methods visit `http://localhost:8080/swagger`.