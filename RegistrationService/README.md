# Registration Microservice

A specialized microservice responsible for new user registration within the microservices architecture. Built with **.NET 8**, ensuring secure password handling and data validation.

## 🚀 Features

- **User Registration:** Handles new user sign-ups with data validation.
- **Architecture Pattern:** Implements **Repository Pattern** (`IUserRepository`) for clean data access abstraction.
- **Security:**
  - Passwords are securely hashed using **BCrypt** before storage.
  - Prevents duplicate registrations (email uniqueness check).
- **Validation:** Strong request validation (Email format, Password length) using Data Annotations.
- **Database:** Integrated with **PostgreSQL** via EF Core.
- **Docker Ready:** Fully containerized for easy deployment.

## 🛠 Tech Stack

- **Framework:** ASP.NET Core 8 Web API
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core
- **Security:** BCrypt.Net-Next
- **Containerization:** Docker

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/)
- [Docker](https://www.docker.com/) (optional)

### Environment Variables

Configure the following variables in your environment, `launchSettings.json`, or Docker Compose file.

**Database Configuration:**
- `DB_HOST`: Database host
- `DB_PORT`: Database port (default: `5432`)
- `DB_NAME`: Database name (e.g., `authdb`)
- `DB_USER`: Database username
- `DB_PASSWORD`: Database password

### Running Locally

1. **Clone the repository:**
```
git clone https://github.com/your-username/registration-service.git
cd registration-service
```
2. **Apply Migrations:**
```
dotnet ef database update
```
3. **Run the application:**
```
dotnet run
```

The service will start on `http://localhost:5267` (check console for actual port).

### Running with Docker
```
docker build -t registration-service .
docker run -d -p 8080:8080
-e DB_HOST=host.docker.internal
-e DB_PORT=5432
-e DB_NAME=authdb
-e DB_USER=postgres
-e DB_PASSWORD=yourpassword
registration-service
```

## 📡 API Endpoints

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/user/register` | Registers a new user. Accepts JSON with `email` and `password`. |

### Example Request
```
{
"email": "user@example.com",
"password": "strongPassword123"
}
```

## 📝 License

This project is open-source and available under the [MIT License](LICENSE).

