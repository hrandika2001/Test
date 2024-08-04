# RestXUnit.Tests
This repository contains automated tests for the Restful API using xUnit and .NET. The tests are designed to verify various operations like getting, adding, updating, and deleting objects from the API.

## Project Structure
- Apitest.cs: Contains the xUnit test class Apitest which includes test methods for interacting with the API.
- ApiObject.cs: Defines the data model classes ApiObject and DataObject used for serialization and deserialization of the API responses.

## Prerequisites
- .NET SDK 6.0 or later
- xUnit testing framework

## Running the Tests

Commands to run using CLI --

dotnet clean
dotnet build
dotnet test


Commands to run using IDE .
From the Menu Bar Test - > Run All Tests.

```
