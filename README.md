Dotnet API with Clean Architecture and JWT Authentication
Description
This project is a .NET 6/7 Web API built using Clean Architecture principles and implements JWT (JSON Web Token) Authentication. It provides a simple API for user registration, authentication, and balance retrieval.

The architecture ensures a maintainable and scalable structure with separation of concerns between different layers.

Key Features
User Registration: Register new users and assign a starting balance of 5 GBP.
User Authentication: Authenticate users and generate a JWT token for secure API access.
Balance Retrieval: Retrieve the current balance of the authenticated user.
Design Patterns Used
Clean Architecture
The project is structured into four layers:

Domain: Core business logic and entities.
Application: Service layer handling business use cases.
Infrastructure: Data access logic and database interaction.
API: Exposes endpoints and handles HTTP requests.
Repository Pattern
The repository pattern abstracts database access, ensuring separation of concerns between business logic and data handling.

Unit of Work Pattern
The Unit of Work pattern manages database transactions, ensuring that operations are executed consistently.

JWT Authentication
Uses JWT for user authentication. After successful login, a JWT token is generated and must be used to access protected endpoints.

Dependency Injection
The project uses dependency injection to promote loose coupling between different layers and services.

Technologies Used
.NET 8 Web API
Entity Framework Core / Dapper / ADO.NET for data access
JWT Authentication for secure API access
Swagger for API documentation
SQL Server for database storage
Setup Instructions
Prerequisites
.NET 8  SDK
SQL Server (or any compatible database)
Postman (for API testing, optional)
