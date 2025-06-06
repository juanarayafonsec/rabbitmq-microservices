# .NET 9 Microservices with RabbitMQ

This project is built using **.NET 9** microservices architecture, using **RabbitMQ** as the message broker for asynchronous communication between services.

---

## 🐇 RabbitMQ

RabbitMQ is included in the project via Docker Compose.

### ▶️ Run RabbitMQ

Start the RabbitMQ container:

```bash
docker compose up -d
```

This will start RabbitMQ with the following ports:

- `5672` – RabbitMQ message broker (used by services)
- `15672` – RabbitMQ Management Dashboard (web UI)

---

## 🌐 Access RabbitMQ Dashboard

Once RabbitMQ is running, open the management dashboard in your browser:

```
http://localhost:15672
```

**Login credentials:**

- **Username:** `user`
- **Password:** `password`

From the dashboard, you can:

- View queues, exchanges, bindings, and virtual hosts
- Monitor message flow
- Create/delete resources
- Manage users and permissions

---

## 🧩 Microservices Overview

| Microservice      | Description                       |
|-------------------|-----------------------------------|
| ServiceName1      | Description 1                     |
| ServiceName2      | Description 2                     |
| ServiceName3      | Description 3                     |

---

## 📬 Message Contracts

### `OrderCreated` Event

```json
{
  
}
```

---

## ▶️ Running the Microservices

```bash
cd src/ServiceName
dotnet run
```

Replace `ServiceName` with the actual service folder (e.g. `OrderService`).

---

## ⚙️ RabbitMQ Configuration

- **Exchange type:** `direct`
- **Durable queues:** `true`
- **Auto-delete:** `false`
- **Retry mechanism:** Dead-letter exchange (DLX) or retry queues
- **Connection string:**  
  `amqp://user:password@rabbitmq:5672/`

---

## ✅ Testing

Run all tests with:

```bash
dotnet test
```

---

## 🐞 Troubleshooting

### Dashboard not loading?

- Make sure Docker is running
- Ensure ports `5672` and `15672` are available
- Restart RabbitMQ:

```bash
docker compose down
docker compose up -d
```

### Services can't connect?

- RabbitMQ may not be ready immediately — add retry logic
- Use the correct connection string:
  ```
  amqp://user:password@rabbitmq:5672/
  ```

---

## 🧰 Development Tools

- RabbitMQ Dashboard: [http://localhost:15672](http://localhost:15672)
- Docker Desktop
- Postman or REST Client
- Visual Studio 2022+ or VS Code

---

## 📄 License

_To be added_

---

## 🗺️ To-Do / Roadmap

- [ ] Define message contract schemas
- [ ] Add retry & DLQ strategy
- [ ] Add health checks
- [ ] Add logging & monitoring
- [ ] Write integration tests
- [ ] Add CI/CD pipeline
