# Book Store Sales Management

**Book Store Sales Management** is an application designed for a bookstore to manage the buying and selling processes and store information about books, customers, and orders. This project is developed using Visual Studio, primarily using C#, and utilizes .NET ASP and SQL Server.

## Installation Guide

1. **Prerequisites**
   - Visual Studio: Make sure you have Visual Studio installed for compiling and running the project.
   - SQL Server: Ensure you have SQL Server installed for data storage.

2. **Installation**
   - Clone the project from our GitHub repository at [https://github.com/an-dora/Book-store-sales-management](https://github.com/an-dora/Book-store-sales-management).
   - Open the project using Visual Studio.

3. **Migrate and Update the Database**
   - Open the NuGet Package Manager Console in Visual Studio.
   - Run the following command to create a migration for the database: `add-migration first-db`
   - Run the following command to apply the migration to the database: `update-database`

4. **Run the Application**
   - You can now run the project by pressing F5 or selecting Debug > Start Debugging in Visual Studio.

## Basic Usage Guide

1. **Login and Account Management**
   - Log in with an admin or staff account.
   - Administrators have access to and can manage all features of the application.
   - Staff members can manage orders, products, and customers.

2. **Product Management**
   - Add, edit, and delete information about books in the store.

3. **Order Management**
   - Create new orders, view a list of orders, and update the order status.

## Author
This project is developed by An Dora.
- GitHub: [https://github.com/an-dora](https://github.com/an-dora)

## Note
- The project is still in development and may not be complete. If you encounter any issues or wish to contribute, please contact the author.

## License
This project is licensed under the [MIT License](LICENSE).

---
Please refer to the [LICENSE](LICENSE) file for details regarding the license.
