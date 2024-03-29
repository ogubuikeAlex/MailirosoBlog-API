# Auth Controller API Documentation

## Overview

The Auth Controller provides endpoints for user authentication and authorization-related operations.

## Base Route

All endpoints in this controller are relative to the base route: `/api/auth`

## Authentication

Endpoints in this controller require authentication.

## Endpoints

### 1. Login User

- **HTTP Method**: POST
- **Route**: `/`
- **Summary**: Log in a user
- **Request Body**: [LoginUserDTO](#loginuserdto)
- **Responses**:
  - **200 OK**: Logon successful. Returns [LoginResponse](#loginresponse)
  - **404 Not Found**: No record found. Returns [ErrorResponse](#errorresponse)
  - **500 Internal Server Error**: It's not you, it's us. Returns [ErrorResponse](#errorresponse)

### 2. Sign Up Author

- **HTTP Method**: POST
- **Route**: `/signup`
- **Summary**: Sign up to the app
- **Request Body**: [CreateUserDTO](#createuserdto)
- **Responses**:
  - **200 OK**: User signed up. Returns [UserResponseDTO](#userresponsedto)
  - **400 Bad Request**: User already exists. Returns [ErrorResponse](#errorresponse)
  - **500 Internal Server Error**: It's not you, it's us. Returns [ErrorResponse](#errorresponse)

## Data Transfer Objects (DTOs)

### LoginUserDTO

```csharp
public class LoginUserDTO
{
    // Define properties here
}
