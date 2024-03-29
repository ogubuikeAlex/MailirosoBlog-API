# Blog Post Controller API Documentation

## Overview

The Blog Post Controller provides endpoints for managing blog posts.

## Base Route

All endpoints in this controller are relative to the base route: `/api/blogpost`

## Authorization

Endpoints in this controller require authorization with the `AuthorsOnly` policy.

## Endpoints

### 1. Get Blog Post By ID

- **HTTP Method**: GET
- **Route**: `/{id}`
- **Summary**: This method gets a blog post by its ID. It does not require authentication.
- **Parameters**:
  - `id` (long): The ID of the blog post to retrieve
- **Responses**:
  - **200 OK**: Blog post retrieved. Returns [BlogPostResponse](#blogpostresponse)
  - **404 Not Found**: No record found. Returns [ErrorResponse](#errorresponse)
  - **500 Internal Server Error**: It's not you, it's us. Returns [ErrorResponse](#errorresponse)

### 2. Get All Blog Posts

- **HTTP Method**: GET
- **Route**: `/`
- **Summary**: This method gets a paginated list of blog posts. It does not require authentication.
- **Query Parameters**:
  - `request` ([BlogPostRequestParameter](#blogpostrequestparameter)): Request parameters for pagination and filtering
- **Responses**:
  - **200 OK**: Blog posts retrieved. Returns [PagedResponse<BlogPostResponse>](#pagedresponseblogpostresponse)
  - **500 Internal Server Error**: It's not you, it's us. Returns [ErrorResponse](#errorresponse)

### 3. Update Blog Post

- **HTTP Method**: PUT
- **Route**: `/{id}`
- **Summary**: This endpoint updates the title and content of a blog post. It requires that the caller be an Author and also the creator of the blog post to be updated
- **Parameters**:
  - `id` (long): The ID of the blog post to update
  - `request` ([UpdateBlogPostDTO](#updateblogpostdto)): The updated title and content
- **Responses**:
  - **200 OK**: Blog post updated. Returns [BlogPostResponse](#blogpostresponse)
  - **404 Not Found**: No record found. Returns [ErrorResponse](#errorresponse)
  - **500 Internal Server Error**: It's not you, it's us. Returns [ErrorResponse](#errorresponse)

### 4. Create Blog Post

- **HTTP Method**: POST
- **Route**: `/`
- **Summary**: This endpoint creates a new blog post. It requires that the caller be an Author.
- **Request Body**: [CreateBlogPostDTO](#createblogpostdto)
- **Responses**:
  - **200 OK**: Blog post created. Returns [BlogPostResponse](#blogpostresponse)
  - **400 Bad Request**: Blog post already exists. Returns [ErrorResponse](#errorresponse)
  - **500 Internal Server Error**: It's not you, it's us. Returns [ErrorResponse](#errorresponse)

### 5. Soft Delete A Blog Post

- **HTTP Method**: PUT
- **Route**: `/delete/{id}`
- **Summary**: Soft delete a blog post
- **Parameters**:
  - `id` (long): The ID of the blog post to delete
- **Responses**:
  - **200 OK**: Blog post soft deleted. Returns [BlogPostResponse](#blogpostresponse)
  - **404 Not Found**: No record found. Returns [ErrorResponse](#errorresponse)
  - **500 Internal Server Error**: It's not you, it's us. Returns [ErrorResponse](#errorresponse)


## Additional Notes

- All endpoints in this controller are secured and require author-level access. Ensure you are authenticated and authorized as an Author to be able to access these endpoints.
- For better data retrieval control, you can pass in custom pagination and filtering query parameters to the `Get All BlogSpot` endpoint 
- For data integrity, the `Delete-Blog` endpoint follows a soft delete approach.

## Base Controller

This controller inherits from `BaseController`, which provides common functionalities and behaviors for all controllers in the application.

## Dependencies

This controller relies on the `IBlogPostService` interface for blog post-related business logic and data access.