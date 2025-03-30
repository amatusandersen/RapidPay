# RapidPay Code Challenge

RapidPay is a fast-growing payment provider. This project implements a payment processing API using ASP.NET Core, leveraging CQRS with MediatR, JWT token-based authentication, Entity Framework Core, and a hosted background service for dynamic fee calculations.

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Setup](#setup)
  - [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)
  - [Auth Controller](#auth-controller)
  - [Card Controller](#card-controller)
- [Running Tests](#running-tests)

## Overview

The RapidPay API is divided into several modules:

- **Card Management Module:**  
  Create, update, authorize, and process payments with cards.

- **Card Authorization Module:**  
  Validate card status, perform fraud checks (e.g., duplicate authorization prevention), and log authorization attempts.

- **Payment Fees Module:**  
  Dynamically calculate transaction fees using a fee service that updates every hour.

- **User Management Module:**  
  Register new users with encrypted passwords and log in using JWT token-based authentication.

## Architecture

- **CQRS Pattern:**  
  Separate commands (for writes) and queries (for reads) are implemented with MediatR to promote separation of concerns.

- **Repository and Unit of Work:**  
  Use repository patterns for data access and a Unit of Work to ensure atomic operations.

- **Domain-Driven Design:**  
  Implement domain entities, value objects, factories, and specifications to encapsulate business logic.

- **JWT Authentication:**  
  Secure endpoints using JSON Web Tokens (JWT).

- **Hosted Services:**  
  Use a background service to periodically update fee rates dynamically.

- **Swagger Documentation:**  
  Swagger (via Swashbuckle) is used to generate comprehensive API documentation.

## Technologies Used

- .NET 8
- ASP.NET Core Web API
- MediatR
- Entity Framework Core
- JWT Bearer Authentication
- Swashbuckle/Swagger
- xUnit and Moq for unit testing

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server
- Visual Studio or Visual Studio Code

### Setup

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/amatusandersen/RapidPay.git
   cd RapidPay
   
2. **Configure the database:**

   Update the connection string in `appsettings.json:`
   ```json
   {
     "ConnectionStrings": {
       "sqlServer": "Server=(local);Initial Catalog=RapidPay;Persist Security Info=False;User ID=user;Password=password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
     }
   }
3. **Configure JWT Settings:**

   In `appsettings.json`, set up your JWT settings:
   ```json
   {
     "Jwt": {
       "SecretKey": "Your_Secret_Key_Here_ReplaceWithAStrongKey",
       "Issuer": "RapidPayIssuer",
       "Audience": "RapidPayAudience",
       "ExpiryHours": "1"
     }
   }
### Running the application
- **Build and Run:**
  ```bash
  dotnet build
  dotnet run
- **Access Swagger UI:**
  Open your browser and navigate to:

  - `https://localhost:7209/swagger/`
  
  to view the interactive API documentation.

## API Endpoints
### Auth Controller

| Endpoint             | HTTP Verb | Description                                               |
| -------------------- | --------- | --------------------------------------------------------- |
| `/api/auth/register` | POST      | Registers a new user with encrypted passwords.          |
| `/api/auth/login`    | POST      | Authenticates a user and returns a JWT token.             |

### Card controller

| Endpoint                         | HTTP Verb | Description                                                                                   |
| -------------------------------- | --------- | --------------------------------------------------------------------------------------------- |
| `/api/card/create`               | POST      | Creates a new card with the specified initial balance and optional credit limit.              |
| `/api/card/authorize`            | POST      | Authorizes a card by validating its status and logging the authorization attempt.             |
| `/api/card/pay`                  | POST      | Processes a payment between cards, applying dynamic fees and updating balances accordingly.   |
| `/api/card/{cardNumber}/balance` | GET       | Retrieves the current balance and credit limit for the given card number.                     |
| `/api/card/manual-update`        | PATCH     | Updates card details (balance, credit limit, or status) and logs the changes for auditing.      |

**Note:** All endpoints under `CardController` require a valid JWT token in the `Authorization` header, while endpoints under `AuthController` allow anonymous access.

## Running Tests
The project includes unit tests for command/query handlers. To run tests, execute:
  ```bash
  dotnet test
