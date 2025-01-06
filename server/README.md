

# ToDo API

A minimal API for managing a to-do list, allowing users to perform CRUD operations (Create, Read, Update, Delete) on tasks. The API is secured using **JWT Authentication**, and **CORS** is enabled to allow cross-origin requests.

---

## Features

- **JWT Authentication** for secure access to endpoints.
- CRUD operations for managing tasks.
- **CORS** support for cross-origin requests.
- Uses **MySQL** as the database.

---

## Endpoints

### Task Management

| HTTP Method | Route             | Description               | Auth Required |
|-------------|-------------------|---------------------------|---------------|
| **GET**     | `/tasks`          | Retrieve all tasks        | ✅             |
| **POST**    | `/tasks`          | Add a new task            | ✅             |
| **PUT**     | `/tasks/{id}`     | Update an existing task   | ✅             |
| **DELETE**  | `/tasks/{id}`     | Delete a task by ID       | ✅             |

---

## Getting Started

### Prerequisites

- **.NET 6.0 SDK** or higher
- **MySQL** database
- **Postman** or any API testing tool (optional)

---

### Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/yourusername/todo-api.git
   cd todo-api
   ```

2. **Install Dependencies**:
   ```bash
   dotnet restore
   ```

3. **Set Up the Database**:
   Update the `appsettings.json` file with your MySQL connection string:
   ```json
   {
       "ConnectionStrings": {
           "ToDoDB": "server=localhost;database=todo;user=root;password=yourpassword"
       }
   }
   ```

4. **Run the Application**:
   ```bash
   dotnet watch run
   ```

---

## Request Examples

### Authentication
The API uses **JWT** for authentication. Obtain a token by using the `/register` or `/login` endpoints.

---

### CRUD Operations

#### **GET /tasks**
- Retrieves all tasks from the database.
- **Headers**:
  ```
  Authorization: Bearer <your-jwt-token>
  ```

#### **POST /tasks**
- Adds a new task.
- **Headers**:
  ```
  Authorization: Bearer <your-jwt-token>
  ```
- **Body** (JSON):
  ```json
  {
      "name": "Task Name",
      "isComplete": false
  }
  ```

#### **PUT /tasks/{id}**
- Updates an existing task by ID.
- **Headers**:
  ```
  Authorization: Bearer <your-jwt-token>
  ```
- **Body** (JSON):
  ```json
  {
      "name": "Updated Task Name",
      "isComplete": true
  }
  ```

#### **DELETE /tasks/{id}**
- Deletes a task by ID.
- **Headers**:
  ```
  Authorization: Bearer <your-jwt-token>
  ```

---

## Additional Features

### **CORS Support**
CORS is enabled to allow any origin to access the API. This is useful during development when your frontend and backend are hosted on different ports.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

### Notes

- This project includes Swagger for API documentation. You can access it at `/swagger` once the application is running.
- For the frontend, refer to [ToDo List React Client](https://github.com/malbruk/ToDoListReact) and update the API URL in `service.js`.

