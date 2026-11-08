# Inventory Management System

A Windows Forms desktop application built with C# and SQL Server to manage inventory operations, products, categories, suppliers, and user access.

## Overview

The Inventory Management System helps businesses manage their products efficiently by providing product management, stock monitoring, user authentication, and inventory reporting.

The application uses ADO.NET for database operations and follows a role-based access control approach to ensure that users can only perform actions permitted by their roles.

## Features

### 1. Authentication & User Management
- Secure login system.
- Password hashing and salting using PBKDF2.
- Initial Admin account setup.
- Admin can create new users.
- Role-based access control:
  - **Admin:** Full access to the system.
  - **Viewer:** View-only access.

### 2. Product Management
- Add new products.
- Edit existing products.
- Delete products.
- View all products in a DataGridView.
- Manage product details, including:
  - Product Name
  - Description
  - Quantity in Stock
  - Minimum Stock Level
  - Price
  - Category
  - Supplier

### 3. Search & Filtering
- Search products by name.
- Filter products by category.
- Filter products by stock status:
  - All
  - Low Stock
  - In Stock
- Low-stock status is determined using each product's Minimum Stock Level.

### 4. Inventory Reporting
- Generate inventory reports using Crystal Reports.
- Display all products and their inventory information.
- Highlight products that have reached or fallen below their minimum stock level.

## Technologies Used

- C#
- Windows Forms (.NET Framework 4.8)
- SQL Server
- ADO.NET
- Crystal Reports
- SQL

## Database Design

The application uses SQL Server with the following main tables:

- **Users:** Stores user accounts, password hashes, salts, and roles.
- **Products:** Stores product information and stock levels.
- **Categories:** Stores product categories.
- **Suppliers:** Stores supplier information.

## Database

The application uses a **hosted SQL Server database**, which is already configured and ready to use. No local database setup is required.

## Getting Started

### Prerequisites

- Visual Studio 2022
- .NET Framework 4.8
- SAP Crystal Reports for Visual Studio
- Internet connection

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/kero-emad/InventoryManagementSystem
   ```

2. Open the solution file in Visual Studio 2022.

3. Build and run the application.

4. Create the initial Admin account on the first launch, then log in to explore the system.



## Role Permissions

| Feature                  | Admin | Viewer |
|--------------------------|:-----:|:------:|
| View Products            |   ✓   |   ✓    |
| Search & Filter          |   ✓   |   ✓    |
| Add Products             |   ✓   |   ✗    |
| Edit Products            |   ✓   |   ✗    |
| Delete Products          |   ✓   |   ✗    |
| Manage Users             |   ✓   |   ✗    |
| View Inventory Reports   |   ✓   |   ✓    |

## Future Improvements

- Audit Logging to track product changes.
- Advanced dashboard with inventory statistics.
