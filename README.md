\section*{Library Management MVC}

A web application built with ASP.NET Core MVC to manage a library system, providing the following features:

\begin{itemize}
    \item \textbf{Book Management:} Add, edit, delete, and search books with filtering options by author, genre, and availability.
    \item \textbf{Member Management:} User registration, login, and profile viewing.
    \item \textbf{Reservation System:} Members can reserve books, with automatic stock adjustment and reservation tracking.
    \item \textbf{Admin Panel:} Separate dashboard for admins to manage users, books, and delete reviews.
    \item \textbf{Authentication and Sessions:} Secure login system for members and admins using sessions to control access.
    \item \textbf{Filtering and Sorting:} Implemented search filters and sorting for books and members.
    \item \textbf{Technologies Used:} ASP.NET Core MVC, Entity Framework Core, SQL Server / SQLite, Bootstrap.
\end{itemize}

\section*{Project Structure}

\begin{itemize}
    \item \textbf{Controllers:} Handle requests for books, members, reservations, and admin panel.
    \item \textbf{Models:} Data models such as Member, Book, Reservation, Review.
    \item \textbf{Views:} Razor pages for user interface and interaction.
    \item \textbf{Data:} Database context and migration classes using EF Core.
    \item \textbf{wwwroot:} Static files like CSS and JavaScript.
\end{itemize}

\section*{Key Features}

\begin{itemize}
    \item Member registration and login with validation.
    \item Admin roles for privileged site management.
    \item Book reservation with stock control and reservation status display.
    \item Admin-only review deletion and management.
    \item Session-based authentication for secure access control.
\end{itemize}

\section*{How to Run}

\begin{enumerate}
    \item \textbf{Clone the repository:}
    \begin{verbatim}
    git clone https://github.com/Sonia-hhl/LibraryManagementMVC.git
    cd LibraryManagementMVC
    \end{verbatim}

    \item \textbf{Configure the connection string in \texttt{appsettings.json}:}

    \begin{verbatim}
    "ConnectionStrings": {
        "LibraryConnection": "Data Source=library.db"
    }
    \end{verbatim}

    Or update it to connect to your SQL Server instance.

    \item \textbf{Apply migrations and update the database:}

    \begin{verbatim}
    dotnet ef database update
    \end{verbatim}

    \item \textbf{Run the application:}

    \begin{verbatim}
    dotnet run
    \end{verbatim}

    \item Open your browser and navigate to \texttt{https://localhost:5001} (or your project's assigned port).
\end{enumerate}

\section*{Using the Admin Panel}

\begin{itemize}
    \item Log in using an admin username and password stored in the database.
    \item After login, you can manage books, members, and reviews.
    \item The delete review button is visible only to admins.
\end{itemize}

\section*{Future Enhancements}

\begin{itemize}
    \item Password recovery feature.
    \item Improved UI responsiveness.
    \item Implementing API endpoints for mobile app integration.
    \item Password hashing and enhanced login security.
\end{itemize}

\section*{Contact}

\begin{itemize}
    \item Email: your.email@example.com
    \item GitHub: \url{https://github.com/Sonia-hhl}
\end{itemize}
