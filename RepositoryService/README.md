# Game Progress Microservice (Cloud Save)

A specialized microservice for handling cloud saves in a microservice-based game backend. It allows users to sync their game progress across devices using **AWS DynamoDB** for high-performance, low-latency storage.

## 🚀 Features

- **Cloud Save System:** Stores complex game state (JSON) linked to User ID.
- **AWS DynamoDB Integration:** Uses NoSQL for scalable, fast read/write operations suitable for gaming.
- **Security:**
  - **JWT Validation:** Validates tokens issued by the Auth Service to ensure data ownership.
  - **AWS IAM Roles:** Uses `InstanceProfileAWSCredentials` for secure, keyless access to AWS resources.
- **Conflict Resolution:** Tracks `AppVersion` and `LastUpdated` timestamps to manage save versions.
- **Docker Ready:** Containerized for deployment in ECS or Kubernetes.

## 🛠 Tech Stack

- **Framework:** ASP.NET Core 8 Web API
- **Database:** AWS DynamoDB
- **Cloud Provider:** Amazon Web Services (AWS)
- **Authentication:** JWT (Validation only)

## ⚙️ Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- AWS Account (with DynamoDB access)
- [Docker](https://www.docker.com/) (optional)

### Environment Variables

**AWS Configuration:**
- `AWS_REGION`: The AWS region where your DynamoDB table resides (e.g., `us-east-1`).
- *Note: AWS Credentials are handled via Instance Profiles when running in the cloud, or via `~/.aws/credentials` locally.*

**JWT Configuration:**
- `JWT_KEY`: Must match the key used in the Auth Service.
- `JWT_ISSUER`: Must match the Auth Service issuer.
- `JWT_AUDIENCE`: Must match the Auth Service audience.

**Database Configuration:**
- `DynamoDB:TableName`: Name of the table in DynamoDB (default: `UserProgress`).

### AWS DynamoDB Setup

Ensure you have a DynamoDB table created with the following schema:
- **Partition Key (HASH):** `UserId` (Number)
- **Sort Key (RANGE):** `DeviceId` (String)

### Running Locally

1. **Clone the repository:**
```
git clone https://github.com/your-username/progress-service.git
cd progress-service
```

2. **Run the application:**
```
dotnet run
```

### Running with Docker

```
docker build -t progress-service .
docker run -d -p 8080:8080
-e AWS_REGION=us-east-1
-e JWT_KEY=super_secret_key_12345
-e JWT_ISSUER=AuthService
-e JWT_AUDIENCE=GameClient
-v ~/.aws:/root/.aws \ # Maps local AWS creds to container
progress-service
```

## 📡 API Endpoints

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/progress/save` | Saves current game progress (JSON payload). Requires valid Bearer Token. |
| `POST` | `/api/progress/load` | Retrieves the last saved progress for the user/device. |

### Example Payload (Save)
```
{
	"progress": {
	"level": 10,
	"coins": 500,
	"inventory": ["sword", "shield"]
},
	"deviceId": "device_unique_id_123",
	"appVersion": "1.0.2"
}
```


## 📝 License

This project is open-source and available under the [MIT License](LICENSE).

