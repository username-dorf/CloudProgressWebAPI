# Authentication Microservice

A robust authentication microservice built with **.NET 8**, **Entity Framework Core**, and **PostgreSQL**. This service handles user authentication using JWT (Access + Refresh tokens) and follows modern REST API standards.

## 🚀 Features

- **User Authentication:** Secure login processing.
- **JWT Implementation:**
  - **Access Tokens:** Short-lived tokens (60 min) for API access.
  - **Refresh Tokens:** Long-lived tokens (30 days) stored securely in the database to renew access sessions without re-login.
- **Security:**
  - Password verification using **BCrypt** (via `BCrypt.Net-Next`).
  - Secure configuration using environment variables.
- **Database:** Utilizes **PostgreSQL** with EF Core for ORM.
- **Docker Ready:** Includes `Dockerfile` for containerized deployment.

## 🛠 Tech Stack

- **Framework:** ASP.NET Core 8 Web API
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core
- **Containerization:** Docker

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/) (or a Docker container running Postgres)
- [Docker](https://www.docker.com/) (optional, for running the app in a container)

### Environment Variables

To run this project, you need to set up the following environment variables. You can set them in your OS, your IDE (launchSettings.json), or a `.env` file if using Docker Compose.

**Database Configuration:**
- `DB_HOST`: Database host (e.g., `localhost`)
- `DB_PORT`: Database port (e.g., `5432`)
- `DB_NAME`: Database name
- `DB_USER`: Database username
- `DB_PASSWORD`: Database password

**JWT Configuration:**
- `JWT_KEY`: A long secure string for signing tokens (at least 32 chars).
- `JWT_ISSUER`: The token issuer (e.g., `AuthService`).
- `JWT_AUDIENCE`: The token audience (e.g., `MyFrontendApp`).

### Running Locally

1. **Clone the repository:**
```
git clone https://github.com/your-username/auth-service.git
cd auth-service
```

2. **Apply Migrations:**
Ensure your Postgres database is running and connection strings are set, then run:
```
dotnet ef database update
```

3. **Run the application:**
```
dotnet run
```

The service will start on `http://localhost:5267` (or the port defined in `launchSettings.json`).

### Running with Docker

1. **Build the image:**
```
docker build -t auth-service .
```

2. **Run the container:**
```
docker run -d -p 8080:8080
-e DB_HOST=host.docker.internal
-e DB_PORT=5432
-e DB_NAME=authdb
-e DB_USER=postgres
-e DB_PASSWORD=yourpassword
-e JWT_KEY=super_secret_key_12345
-e JWT_ISSUER=AuthService
-e JWT_AUDIENCE=ClientApp
auth-service
```


## 📡 API Endpoints

### Auth

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/auth/login` | Authenticates a user and returns an Access & Refresh token pair. |
| `POST` | `/api/auth/refresh` | Refreshes the Access Token using a valid Refresh Token. |

## 📝 License

This project is open-source and available under the [MIT License](LICENSE).





