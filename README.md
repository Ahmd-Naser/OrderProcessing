# 🛒 Order Processing System

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-brightgreen?style=for-the-badge)](#)
[![CQRS](https://img.shields.io/badge/Pattern-CQRS-blue?style=for-the-badge)](#)
[![Stripe](https://img.shields.io/badge/Stripe-Payment-6772E5?style=for-the-badge&logo=stripe&logoColor=white)](#)

A robust, modern backend API for an e-commerce **Order Processing System**, built with C# and the latest **.NET 10**. The project demonstrates enterprise-level architecture using **Clean Architecture**, **CQRS**, and the **Result Pattern**, ensuring high maintainability, performance, and clear separation of concerns.

---

## ✨ Key Features

- **Clean Architecture & CQRS:** Strict separation across Domain, Application, Infrastructure, and WebApi layers orchestrated via MediatR.
- **Result Pattern:** Explicit and functional handling of domain failures and HTTP responses without relying on exceptions for control flow.
- **Stripe Integration:** Production-ready payment flow utilizing Stripe Checkout Sessions and verified Webhook event processing.
- **Idempotent Operations:** Prevents duplicate orders and charges using X-Idempotency-Key request headers.
- **Role-Based Authorization:** Granular role segregation between Customers (browsing and checkout) and Vendors (catalog management).
- **Comprehensive Unit Testing:** High test coverage and isolated mocks using xUnit, NSubstitute, MockQueryable, and FluentAssertions.
- **Global Exception Handling:** Standardized error responses compliant with RFC 7807 Problem Details.
- **Interactive API Documentation:** Rich Swagger and Scalar integration powered by XML doc comments.

---

## 🛠️ Tech Stack

- **Framework:** .NET 10 Web API
- **Language:** C# 14
- **Database:** SQL Server & Entity Framework Core
- **Architecture:** Clean Architecture
- **Patterns:** CQRS (MediatR), Result Pattern, Repository Pattern (via EF Core DbContext)
- **Security:** JWT Authentication & Role-Based Access Control
- **Payments:** Stripe.net
- **Testing:** xUnit, NSubstitute, MockQueryable, FluentAssertions

---

## 📂 Project Structure

    src/
    ├── OrderProcessing.Domain/         # Domain entities, value objects, and domain errors
    ├── OrderProcessing.Application/    # Commands, Queries, Handlers, validators, and interfaces
    ├── OrderProcessing.Infrastructure/ # EF Core DbContext, persistence, Stripe services, Auth
    ├── OrderProcessing.Infrastructure/ # Database persistence, EF Core DbContext, and data migrations
    └── OrderProcessing.WebApi/         # Controllers, Middleware, Scalar/Swagger configurations

    tests/
    └── OrderProcessing.Application.Tests/ # Unit tests for commands, queries, and business logic

---

## 🚀 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, SQL Express, or Docker container)
- A [Stripe Developer Account](https://dashboard.stripe.com/register) (for test API keys)

### 1. Clone the Repository

    git clone https://github.com/Ahmd-Naser/OrderProcessing.git
    cd order-processing-system

### 2. Configure Settings
Update `src/OrderProcessing.WebApi/appsettings.json` with your database connection string and secret keys:

    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OrderProcessingDb;Trusted_Connection=True;MultipleActiveResultSets=true"
      },
      "Jwt": {
        "Issuer": "YourIssuer",
        "Audience": "YourAudience",
        "SecretKey": "YOUR_SUPER_SECRET_KEY_MUST_BE_LONG_ENOUGH"
      },
      "Stripe": {
        "SecretKey": "sk_test_...",
        "WebhookSecret": "whsec_..."
      }
    }

### 3. Apply Database Migrations

    cd src/OrderProcessing.WebApi
    dotnet ef database update

### 4. Run the Project

    dotnet run

Once started, explore the API endpoints via Scalar / Swagger at:
https://localhost:xxxx/scalar or https://localhost:xxxx/swagger

---

## 🧪 Running Tests

Execute the automated test suite with:

    dotnet test

---

## 📖 API Documentation Example (Order Placement)

Orders require an Idempotency Key in the header to ensure safe retries:

    POST /api/orders
    Authorization: Bearer <token>
    X-Idempotency-Key: a4c8916e-69f8-4b77-a8b4-4b5258e7cb44

---

## 👨‍💻 Author

**Ahmed Nasser**  
Backend Software Engineer specializing in C#, .NET, and Scalable Backend Architecture.  
- [GitHub](https://github.com/Ahmd-Naser)

*(Feel free to reach out for feedback, discussions, or opportunities!)*
