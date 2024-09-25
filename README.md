# ASP.NET API Project for Product Management

This project is a  web application built using ASP.NET API (C#) that allows users to register, log in, and perform CRUD operations on products with pagination and filter support.

## Features

1. **User Registration API**
   - **POST Endpoint**: `/api/register`
   - **Request Body**: 
     ```json
     {
       "email": "user@example.com",
       "password": "yourPassword",
       "firstName": "Olayinka",
       "lastName": "Uche"
     }
     ```
   - **Validation**: The API includes backend validation to ensure:
     - Valid email format
     - Strong password criteria
   - **Confirmation Email**: Users receive a confirmation email upon successful registration.
   - **Data Storage**: User registration details are saved in the database.

2. **User Login API**
   - **POST Endpoint**: `/api/login`
   - **Request Body**:
     ```json
     {
       "email": "user@example.com",
       "password": "yourPassword"
     }
     ```
   - **Authentication Logic**: The API verifies user credentials against stored records, including password hashing for security.
   - **JWT Token Generation**: Upon successful login, a JWT token is generated and returned in the response.
   - **Authorization**: Subsequent authenticated requests must include the token in the `Authorization` header:
     ```
     Authorization: Bearer <your-token>
     ```

3. **Product CRUD Operations**
   - **Endpoints**:
     - **GET All Products**: `/api/products`
     - **GET Product by ID**: `/api/products/{id}`
     - **POST New Product**: `/api/products`
       - **Request Body**:
         ```json
         {
           "name": "Product Name",
           "description": "Product Description",
           "price": 99.99,
          
         }
         ```
     - **PUT Update Product**: `/api/products/{id}`
       - **Request Body**: Similar to POST.
     - **DELETE Product**: `/api/products/{id}`

## Requirements

- .NET SDK (version X.X)
- A database server (e.g., SQL Server, PostgreSQL)

## Setup and Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/dlaw-code/ProductsAuth.git
   cd ProductsAuth
