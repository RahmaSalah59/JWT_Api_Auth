
# JWT API Auth

A secure and modular ASP.NET Core Web API implementing JSON Web Token (JWT) authentication. This project provides a foundational structure for building scalable and maintainable APIs with role-based access control.

## Features

- **JWT Authentication**: Secure token-based authentication for stateless API access.
- **Modular Architecture**: Organized into Controllers, Services, Models, DTOs, and Properties for clarity and scalability.
- **Entity Framework Core**: Utilizes EF Core for database interactions and migrations.
- **Role-Based Access Control**: Implement roles and permissions to manage user access.
- **Configuration Management**: Centralized settings via `appsettings.json` and environment-specific configurations.

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or any other supported database
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/RahmaSalah59/JWT_Api_Auth.git
   cd JWT_Api_Auth
   ```

2. **Restore dependencies**:

   ```bash
   dotnet restore
   ```

3. **Apply migrations and update the database**:

   ```bash
   dotnet ef database update
   ```

4. **Run the application**:

   ```bash
   dotnet run
   ```

   The API will be available at `https://localhost:5001` or `http://localhost:5000`.

## Project Structure

- **Controllers**: Handle HTTP requests and responses.
- **Services**: Contain business logic and interact with repositories.
- **Models**: Define the data structures and entities.
- **DTOs (Data Transfer Objects)**: Define the shape of data sent over the network.
- **Properties**: Contain configuration files like `launchSettings.json`.
- **Migrations**: Manage database schema changes using EF Core migrations.
- **wwwroot**: Serve static files if needed.

## Configuration

Update the `appsettings.json` file with your specific settings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YourDatabaseConnectionString"
  },
  "Jwt": {
    "Key": "YourSecretKey",
    "Issuer": "YourIssuer",
    "Audience": "YourAudience",
    "ExpireMinutes": 60
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

- **Key**: A secret key used to sign the JWT tokens.
- **Issuer**: Identifies the principal that issued the JWT.
- **Audience**: Identifies the recipients that the JWT is intended for.
- **ExpireMinutes**: Token expiration time in minutes.

## API Endpoints

Here are some of the primary endpoints exposed by the API:

- **POST** `/api/auth/register`: Register a new user.
- **POST** `/api/auth/login`: Authenticate a user and receive a JWT.
- **GET** `/api/users`: Retrieve a list of users (requires authentication).
- **GET** `/api/users/{id}`: Retrieve user details by ID (requires authentication).

> **Note**: Replace `/api/` with your actual route prefixes if they differ.

## Usage

1. **Register a new user**:

   Send a POST request to `/api/auth/register` with the following JSON body:

   ```json
   {
     "username": "yourusername",
     "email": "youremail@example.com",
     "password": "YourSecurePassword"
   }
   ```

2. **Login**:

   Send a POST request to `/api/auth/login` with the following JSON body:

   ```json
   {
     "email": "youremail@example.com",
     "password": "YourSecurePassword"
   }
   ```

   On successful authentication, you'll receive a JWT token.

3. **Access protected routes**:

   Include the JWT token in the `Authorization` header of your requests:

   ```http
   Authorization: Bearer your_jwt_token
   ```

## Contributing

Contributions are welcome! Please follow these steps:

1. **Fork the repository**.
2. **Create a new branch**:

   ```bash
   git checkout -b feature/YourFeature
   ```

3. **Commit your changes**:

   ```bash
   git commit -m "Add your feature"
   ```

4. **Push to the branch**:

   ```bash
   git push origin feature/YourFeature
   ```

5. **Open a pull request**.

## License

This project is licensed under the [MIT License](LICENSE).

---

For more details and to explore the codebase, visit the [JWT_Api_Auth GitHub Repository](https://github.com/RahmaSalah59/JWT_Api_Auth).
