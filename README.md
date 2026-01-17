# Student Grade Analysis Application - DANAOS Technical Test

## Overview
This full-stack application was built as part of the DANAOS Projects technical assessment. It is a **Visual Web GUI Application** that consumes a backend service to display student performance data, export reports to Excel, and visualize course analytics via charts.

## Features implemented
* **Backend API:** A RESTful API built with **.NET 8** using **ADO.NET** for maximum performance.
* **Database:** MS SQL Server with a normalized schema (Composite Keys) as requested.
* **Frontend:** A responsive **React (TypeScript)** interface.
* **Data Visualization:** Interactive bar chart showing average grades per course using **Chart.js**.
* **Reporting:** Automated **Excel (.xlsx) export** functionality.

## Technology Stack
* **Backend:** ASP.NET Core Web API (.NET 8)
* **Database Access:** Raw ADO.NET (System.Data.SqlClient) for optimal space/time complexity.
* **Database:** Microsoft SQL Server
* **Frontend:** React 18, Vite, TypeScript
* **Libraries:** EPPlus (Excel Export), Chart.js (Visualization), Axios (HTTP Client).

---

## Design Decisions (Why this architecture?)

### 1. Efficiency (Time & Space Complexity)
Instead of using a heavy ORM like Entity Framework, I utilized **Raw ADO.NET**.
* **Optimization:** Aggregations (Average calculations) are performed directly in the SQL Database engine using `GROUP BY` and `AVG`.
* **Benefit:** This minimizes network traffic (only results are sent, not raw data) and keeps the C# memory footprint O(1) relative to the dataset size.

### 2. Good Design (Object-Oriented)
The application follows the **Repository Pattern** and **Dependency Injection**:
* **Separation of Concerns:** The `DatabaseService` handles all SQL logic, keeping the API Controllers clean and focused on HTTP responses.
* **Interoperability:** The API exposes data in standard JSON format, making it consumable by any client (web, mobile, or desktop), fulfilling the requirement for universal readability.

### 3. Accuracy
The database schema strictly follows the requirements:
* **Student Table:** `Id` (Primary Key), `Name`.
* **Grades Table:** Composite Primary Key (`Student_Id`, `Course_Id`) to ensure data integrity.

---

## Setup & Installation

### Prerequisites
* .NET 8 SDK
* Node.js (v18+)
* Microsoft SQL Server

### Step 1: Database Setup
1.  Open `SQL Server Management Studio` (SSMS).
2.  Open the file `DatabaseSchema.sql` provided in this solution.
3.  Execute the script to create the `DanaosTestDB` database and seed the test data.

### Step 2: Backend Setup
1.  Navigate to the `DanaosBackend` folder.
2.  Update the `ConnectionStrings` in `appsettings.json` if your SQL Server instance name differs from `localhost`.
3.  Run the application:
    ```bash
    dotnet run
    ```
    *The API will start on http://localhost:5000*

### Step 3: Frontend Setup
1.  Navigate to the `DanaosFrontend` folder.
2.  Install dependencies:
    ```bash
    npm install
    ```
3.  Start the development server:
    ```bash
    npm run dev
    ```
4.  Open your browser to the local URL provided (usually http://localhost:5173).

---

## Author
**Carlo Bactol**
*Candidate for Software Developer Position*