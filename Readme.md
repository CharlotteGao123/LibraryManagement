# LibraryManagement — ASP.NET Core MVC + SQLite + Batch Insert

A course project built with **ASP.NET Core MVC** and **Entity Framework Core (SQLite)**.  
Implements CRUD for **Books, Authors, Customers, and Library Branches** with JSON-based batch insert support.

---

## Features
- **CRUD** operations for:
  - Books
  - Authors
  - Customers
  - Library Branches
- **Batch Insert**: JSON files (`authors.json`, `books.json`, `customers.json`, `librarybranches.json`) are automatically loaded into the database on application startup.  
- **Service Layer**: Business logic encapsulated in `Services/` (e.g., `AuthorService`, `BookService`).  
- **Consistent Routing**: 
  - `List()` → displays all records  
  - `Details(int id)` → single record details  
  - Views: `DetailsList.cshtml` & `Details.cshtml`  

---

## Directory Structure
LibraryManagement/
├─ Controllers/        # MVC Controllers
├─ Data/               # DbContext & Migration files
│  ├─ AppDbContext.cs
│  └─ Library.db       # SQLite database file
├─ Models/             # Entity models
├─ Services/           # Business logic layer
├─ Views/              # Razor Views (Books, Authors, etc.)
├─ appsettings.json    # Config (DB connection string, logging)
├─ Program.cs          # Startup & batch insert
└─ README.md           # Project documentation

---

## Prerequisites
- .NET 8 SDK  
- SQLite  
- DBeaver (optional, for inspecting `Library.db`)  

---

## Run the Project
1. Clone the repository:  
   ```bash
   git clone git@github.com:CharlotteGao123/LibraryManagement.git
   git clone https://github.com/CharlotteGao123/LibraryManagement.git
   cd LibraryManagement
2. Restore dependencies:
   dotnet restore
   
3. Run the project.
   dotnet run

4. Open in the browser:
   eg. http://localhost:5287 


## Batch Insert (JSON Seed Data)
•Place JSON files in Data/ folder:
•authors.json
•books.json
•customers.json
•librarybranches.json
•On startup, Program.cs will check if the tables are empty:
•If empty → insert from JSON
•If not empty → skip insertion


## Database Access with DBeaver
1.	Open DBeaver → New Database Connection → Choose SQLite.
2.	Select database file: LibraryManagement/Data/Library.db
3.	Test connection → Finish.
4.	Run SQL queries to check data:
SELECT * FROM Authors;
SELECT * FROM Books;
SELECT * FROM Customers;
SELECT * FROM LibraryBranches;

## Notes
If models are modified, regenerate migrations:
dotnet ef migrations add UpdateModels
dotnet ef database update
To reset database, delete Library.db and rerun the project.
