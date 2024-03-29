
# MalirosoBlog API

A Role-Based Access Control (RBAC) Blog Post Web API using ASP.NETCore Web API. 




## Authors

- [@Alexa](https://github.com/king-Alex-d-great)


## Environment Variables

To run this project, please add a .env file at the root of the `MalirosoBlog.API` project.

You will need to add the following environment variables to your .env file:

```
# DB Config
DatabaseConfiguration__Host=<DB-Server>
DatabaseConfiguration__Name=<DB-Name>

#jwt Config
JWTConfiguration__Secret=<some-secret-key>
JWTConfiguration__Issuer=<JWT-Issuer>
JWTConfiguration__Audience=<JWT-Audience>
```

Sample .env:

```
# DB Config
DatabaseConfiguration__Host=(localdb)\MSSQLLocalDB
DatabaseConfiguration__Name=Maliroso

#jwt Config
JWTConfiguration__Secret=xgrrdupqizxpwvkitiiehvdcwfwqdhqvbgrpzjeoel
JWTConfiguration__Issuer=http://localhost:44356/swagger/
JWTConfiguration__Audience=http://localhost:44356
```



## Tech Stack

**Server:** 
- .Net 8.0
- C# 12
- ASP.Net Web API
- EFCore 8.0
- MSSQL Server


## Run Locally

> **_NOTE:_**  Please check out Environment Variable Section to add .env file before running.

Clone the project

```bash
  git clone https://github.com/king-Alex-d-great/MailirosoBlog-API.git
```

Go to the project directory

```bash
  cd MailirosoBlog-API
```

****
Install dependencies (if necessary):

```bash
  dotnet restore
```

Start the server

```bash
  dotnet run
```


## Documentation

If you're looking for a comprehensive document, [please visit our documentation project](https://github.com/king-Alex-d-great/MailirosoBlog-API/tree/main/MalirosoBlog.Docs). 

**Otherwise, here is a concise API reference for the application:**


### **Blogpost API**

- **Get Blog Post by Id**
  
Retrieves a blog post by its unique ID.

```http
  GET /api/blogpost/{id}
```

| Parameter | Type     | Description             |
| --------- | -------- | ----------------------- |
| `id`      | `long`   | **Required**. Blog post ID to fetch |


---

- **Get All Blog Posts**
  
Retrieves a paginated list of blog posts.

```http
  GET /api/blogpost
```

| Parameter | Type     | Description                           |
| --------- | -------- | ------------------------------------- |
| `page`    | `int`    | Page number for pagination (optional) |
| `size`    | `int`    | Number of items per page (optional)   |
| `SearchTerm`    | `string`    | parameter to search by (optional)   |

---

-  **Update Blog Post**
  
Updates the title and content of a blog post.

```http
  PUT /api/blogpost/{id}
```

| Parameter | Type         | Description                         |
| --------- | ------------ | ----------------------------------- |
| `id`      | `long`       | **Required**. Blog post ID to update |
| `request` | `UpdateBlogPostDTO` | **Required**. Updated blog post details |


---

-  **Create Blog Post**

  
Creates a new blog post.

```http
  POST /api/blogpost
```

| Parameter | Type             | Description                 |
| --------- | ---------------- | --------------------------- |
| `request` | `CreateBlogPostDTO` | **Required**. Blog post details to create |

---

-  **Soft Delete Blog Post**

  
Soft deletes a blog post by setting its active status to false.


```http
  PUT /api/blogpost/delete/{id}
```

| Parameter | Type     | Description                     |
| --------- | -------- | ------------------------------- |
| `id`      | `long`   | **Required**. Blog post ID to soft delete |

---

### **Author API**

-  **Get Author by Id

Retrieves an author by their unique ID.

```http
  GET /api/author/{id}
```

| Parameter | Type     | Description             |
| --------- | -------- | ----------------------- |
| `id`      | `string` | **Required**. Author ID to fetch |

---

-  **Get All Authors**

Retrieves a paginated list of authors.

```http
  GET /api/author
```

| Parameter | Type     | Description                           |
| --------- | -------- | ------------------------------------- |
| `page`    | `int`    | Page number for pagination (optional) |
| `size`    | `int`    | Number of items per page (optional)   |

---

-  **Soft Delete Author**
  
Soft deletes an author by setting their status to deleted.

```http
  PUT /api/author/{id}
```

| Parameter | Type     | Description                     |
| --------- | -------- | ------------------------------- |
| `id`      | `string` | **Required**. Author ID to soft delete |


---

### **Auth API**

-  **Login User**

Logs in a user with the provided credentials.

```http
  POST /api/auth
```

| Parameter | Type            | Description             |
| --------- | --------------- | ----------------------- |
| `request` | `LoginUserDTO` | **Required**. User login details |


---

-  **Sign Up Author**

  Registers a new Author on the application.

```http
  POST /api/auth/signup
```

| Parameter | Type            | Description                 |
| --------- | --------------- | --------------------------- |
| `request` | `CreateUserDTO` | **Required**. User sign-up details |



## Demo

Insert gif or link to demo


## Features

The system is role based and so the features available to a user depends on what role they are logged in as.
### **Admin Based Functionalities**
- View all Authors
- View an Author
- Delete an Author
### **Author Based Functionalities**
- Create a Blogpost
- Update a Blogpost
- Delete a Blogpost
### **Features That Do Not Require A Role**
- View All Blogposts
- View a Blogpost


