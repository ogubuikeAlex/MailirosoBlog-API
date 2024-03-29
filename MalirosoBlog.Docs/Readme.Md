# Official Documentation For MailirosoBlog API

Hello There! This is the official documentation for the blog API.

This application is Role-Based Access Control (RBAC) Blog Post Web API built using ASP.NET Core Web API.

## Database Configuration

This application by default uses an in-memory database, so you do not need to configure anything to use it.

**PLEASE NOTE**

**Using an in-memroy Db means everytime you restart the server all previous saved data is lost.**

Nevertheless, the application has been configured to also work with an external database if you choose.

All you have to do is:
- Go to `MailirosoBlog.API`
- Create a .env variable (Check [here](https://github.com/king-Alex-d-great/MailirosoBlog-API/blob/main/MalirosoBlog.Docs/Readme.Md#environment-variables) for how to do this)
- Go to `MailirosoBlog.API/Program.cs` file
- Simply comment the in-memory db configuration
- Then uncomment the external db configuration

**Note: I have also added a very helpful comment in the Program.cs file to show exactly what to do**

## Understanding The Application Roles
Within the application, there are two roles => 
- Author 
- Admin

Having the Author role will allow you  =>
- create a blogpost
- update a blogpost
- delete a blogpost

Having the Admin role will allow you =>
- view all authors
- delete an author's profile
- get a particular author


You do not need to be logged in to
- view all blogposts
- view a blog post

## Data Seeding
To enable you start testing immediately, when you run the application some information will be seeded into the DB.

These include
- Information for the application Admin
- A demo author
- Some demo blogposts

## Application Authentication 

### Login As An Admin
By default, when you run this application for the first, the admin will be created. 

The application must have just one admin and so a new admin cannot be created.

To get admin privileges, all you have to do is login with these credentials:

```
{
   "Email": "Admin@MalirosoBlog",
   "Password": "Admin@MalirosoBlog1"
}

```

### Login As An Author
If you would like to use the seeded Author to run the app, login with these credentials:

```
{
   "Email": "johndoe@yopmail.com",
   "Password": "Admin@MalirosoBlog1"
}

```

To get your own credentials, please create an Author profile using the signup endpoint (/auth/signup).
After your profile has been created, you can login with your own credentials.


## Application Authorization

The application makes use of bearer tokens.
After a successful logon, you will be given a token which you can use to access protected routes.

Here is a sample login Response:

```
{
  "success": true,
  "data": {
    "jwtToken": {
      "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjUwMmVjYjFlLTQ3MmMtNDYyYy04OTdhLTZlMWVkNzMwYzlhZSIsImVtYWlsIjoidXNlckBleGFtcGxlLmNvbSIsImp0aSI6ImQ1OTE2YTMyLTNkNGUtNDVjYi05OTc1LTg5NTI0OWFhOWU3ZiIsIm5hbWVpZCI6IjUwMmVjYjFlLTQ3MmMtNDYyYy04OTdhLTZlMWVkNzMwYzlhZSIsInJvbGUiOiJBdXRob3IiLCJuYmYiOjE3MTE3MDk2OTQsImV4cCI6MTcxMTcxNjg5NCwiaWF0IjoxNzExNzA5Njk0LCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjQ0MzU2L3N3YWdnZXIvIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo0NDM1NiJ9.GS9125ejQnaz0IwPPw0tFCygazVWZVK5hm1knJHCdL0",
      "issued": "2024-03-29T11:54:54.9567308+01:00",
      "expires": "2024-03-29T13:54:54.9293301+01:00"
    },
    "lastName": "string",
    "firstName": "string",
    "email": "user@example.com",
    "userId": "502ecb1e-472c-462c-897a-6e1ed730c9ae"
  }
}
```

To access a protected route, simply pass the token as a bearer token to the request header:

```
header
{
    Authorization: `bearer ${token}`
}
```

On Swagger, you can pass it via the authorize modal.

### **Note that if you use login details associated with an Author, you will only be able to access resources for the Author role and this is for credentials for the Admin role.**

---

## Specifics Documentation
- [Author Docs](https://github.com/king-Alex-d-great/MailirosoBlog-API/blob/main/MalirosoBlog.Docs/Author.md)
- [Blogpost Docs](https://github.com/king-Alex-d-great/MailirosoBlog-API/blob/main/MalirosoBlog.Docs/BlogSpot.md)
- [Auth Docs](https://github.com/king-Alex-d-great/MailirosoBlog-API/blob/main/MalirosoBlog.Docs/Authentcation.md)

## Environment Variables

You will need to add the following environment variables to your .env file at the root of the `MalirosoBlog.API` project.:

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
