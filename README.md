# 📚 Library Management System (ASP.NET Core MVC)

A full-featured web application for managing a library system, developed using ASP.NET Core MVC. This project provides functionalities for managing books, members, and reviews with user authentication and an admin panel.

## ✨ Features

- 🔐 **User Authentication** – Sign up, login, and session management.
- 📖 **Book Management** – List of books with search, filter, and sort capabilities.
- 🧑‍🤝‍🧑 **Member Management** – View and manage member details (admin only).
- 💬 **Review System** – Members can add reviews; admins can delete inappropriate reviews.
- 📊 **Admin Panel** – A secure section for managing library content and members.
- 🎯 **Role-based Access Control** – Access restriction based on admin/user roles.

## 🛠 Technologies Used

- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQLite** (local development database)
- **Bootstrap 5** (UI styling)
- **C#**

## 🚀 Getting Started

### Prerequisites

- [.NET SDK 7.0+](https://dotnet.microsoft.com/)
- Visual Studio or VS Code with C# and .NET support

### Running the Project Locally

1. Clone the repository:
   ```bash
   git clone https://github.com/Sonia-hhl/LibraryManagementMVC.git
   cd LibraryManagementMVC
2. Restore dependencies:
   ```bash
   dotnet restore
3. Run the application:
   ```bash
   dotnet run
4. Open your browser and navigate to:
   ```bash
   https://localhost:7021
You may need to enable session support or migrations depending on your setup.


## 🧪 Sample Admin Login

You can log in as an admin using credentials from the seeded or test database:

- **Username**: `admin`
- **Password**: `admin123`

> Make sure the user in the database has `IsAdmin = true`.

## 📁 Project Structure
```vbnet
LibraryManagementMVC/
│
├── Controllers/
│   ├── BooksController.cs
│   ├── MembersController.cs
│   └── ReviewController.cs
│
├── Models/
│   ├── Book.cs
│   ├── Member.cs
│   └── Review.cs
│
├── Views/
│   ├── Shared/
│   ├── Books/
│   ├── Members/
│   └── Review/
│
├── Data/
│   └── LibraryContext.cs
│
├── wwwroot/
│   └── (CSS, JS, images)
│
└── appsettings.json
```

## 🛡 Security Notes

- Passwords are stored in plain text in this version — only for demo purposes.
- For production, use hashing (e.g., `BCrypt`) and HTTPS.

## 📜 License

This project is for educational purposes and currently has no license.

---

Feel free to fork, star ⭐, or contribute via issues and pull requests!

