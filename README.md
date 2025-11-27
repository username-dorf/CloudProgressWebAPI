# Educational Cloud Game Backend

**Note:** This is an educational project created to practice building a **Microservice Architecture** from scratch. It demonstrates a real-world scenario of a backend for a mobile game with Cloud Save functionality.

## 🎓 Learning Goals
This project was built to master the following concepts:
- Breaking down a monolith into **Microservices**.
- Implementing **Cloud-Native** patterns (Stateless Auth, External Configuration).
- Working with **Docker** and containerization.
- Mixing relational (PostgreSQL) and NoSQL (DynamoDB) databases based on use-cases.

## 📂 Project Structure

The system consists of three lightweight services:

1.  **[Auth Service](./AuthService)**
    *   *Purpose:* Handles User Login & Security.
    *   *Key Tech:* JWT (Access/Refresh Tokens), BCrypt.
    
2.  **[Registration Service](./RegistrationService)**
    *   *Purpose:* Manages new user sign-ups.
    *   *Key Tech:* Repository Pattern, Data Validation.

3.  **[Progress Service](./RepositoryService)**
    *   *Purpose:* Handles requests to **save** (upload) and **load** (download) game progress.
    *   *Key Tech:* **AWS DynamoDB**, Cloud Storage.

## 🚀 How to Run (Simple Way)

You will need [Docker](https://www.docker.com/) installed.

1.  **Clone the repo**
2.  **Navigate to each folder** and follow the instructions in their respective README files to run them individually.
    *   *Tip:* Ensure you have a local PostgreSQL instance running for the Auth/Registration services.

## 🛠 Tech Stack Overview
- **.NET 8** (Web API)
- **PostgreSQL** (User Data)
- **AWS DynamoDB** (Game Saves)
- **Docker** (Containerization)

---
*Created for educational purposes to demonstrate backend development skills.*
