# Author Controller API Documentation

## Overview

The Author Controller provides endpoints for author-related operations.

## Base Route

All endpoints in this controller are relative to the base route: `/api/author`

## Authorization

Endpoints in this controller require admin-level authorization (`AdminOnly` policy).

## Endpoints

### 1. Get Author By ID

- **HTTP Method**: GET
- **Route**: `/{id}`
- **Summary**: Get an author by their ID
- **Parameters**:
  - `id` (string): The ID of the author to retrieve
- **Responses**:
  - **200 OK**: Author retrieved. Returns [AuthorResponse](#authorresponse)
  - **404 Not Found**: No record found. Returns [ErrorResponse](#errorresponse)
  - **500 Internal Server Error**: It's not you, it's us. Returns [ErrorResponse](#errorresponse)

### 2. Get All Authors

- **HTTP Method**: GET
- **Route**: `/`
- **Summary**: Get all authors
- **Query Parameters**:
  - `request` ([AuthorRequestParameter](#authorrequestparameter)): Request parameters for pagination and filtering
- **Responses**:
  - **200 OK**: Authors retrieved. Returns [PagedList<AuthorResponse>](#pagedlistauthorresponse)
  - **500 Internal Server Error**: It's not you, it's us. Returns [ErrorResponse](#errorresponse)

### 3. Soft Delete An Author

- **HTTP Method**: PUT
- **Route**: `/{id}`
- **Summary**: Soft delete an author
- **Parameters**:
  - `id` (string): The ID of the author to delete
- **Responses**:
  - **200 OK**: Author soft deleted. Returns [AuthorResponse](#authorresponse)
  - **404 Not Found**: No record found. Returns [ErrorResponse](#errorresponse)
  - **500 Internal Server Error**: It's not you, it's us. Returns [ErrorResponse](#errorresponse)


## Additional Notes

- All endpoints in this controller are secured and require admin-level access. Ensure you are authenticated and authroized as an admin to be able to access these endpoints.
- For better data retrieval control, you can pass in custom pagination and filtering query parameters to the `Get All Authors` endpoint 

## Base Controller

This controller inherits from `BaseController`, which provides common functionalities and behaviors for all controllers in the application.

## Dependencies

This controller relies on the `IAuthorService` interface for author-related business logic and data access.

## Error Handling

Errors are handled consistently across endpoints. When an error occurs, an appropriate HTTP status code is returned along with an error response object (`ErrorResponse`).

## Response Formats

- **AuthorResponse**: Represents the format of an author object returned by the API.
- **PagedList<AuthorResponse>**: Represents a paged list of author objects returned by the API.
- **ErrorResponse**: Represents the format of an error response returned by the API.
