# 🎬 Movie Price Comparison App

This is a full-stack web application built with **React** (frontend) and **ASP.NET Core (.NET 8)** (backend) that 
allows users to compare movie ticket prices from two providers: **Cinemaworld** and **Filmworld**. 
The app retrieves data via external APIs, handles partial failures with retry logic, and displays the lowest price for each movie.

---

## Technologies Used

- Frontend: [React](https://reactjs.org/) + [Vite](https://vitejs.dev/)
- Backend: [ASP.NET Core 8](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core)
- Language: JavaScript (Frontend), C# (Backend)
- Tooling: Visual Studio 2022, VS Code

---

## Features

- Fetches movies from two providers via external APIs
- Handles provider API failure with retry and fallback
- Displays the lowest price for each movie
---

## How to Run the App Locally

### Backend (ASP.NET Core)

1. Navigate to the backend project:
   ```
   cd backend/MovieCompareApi
2. Launch the API with Visual Studio or CLI:
   ```
   dotnet run
3. It will start on a dynamic port like:
   ```
    https://localhost:7236/
4. Make sure this endpoint is available (example):
   ```
    https://localhost:7236/api/Movie/compare-prices
Note: The port may vary. 
Please copy the exact https://localhost:xxxx URL shown in your console and use it in the frontend configuration.

### Frontend (React + Vite)
1. Navigate to the frontend folder:
   ```
    cd frontend/movie-price-app
2. Install dependencies:
   ```
    npm install
3. Start the frontend dev server:
   ```
    nnpm run dev
4. Open in browser:
   ```
    http://localhost:5173
5. The app will call the backend API at https://localhost:7236 (or your actual backend URL).
  If different, you can update the URL in the frontend code:
    Edit this file:
    ```
       frontend/movie-price-app/src/components/MovieList.jsx
   ```
    Replace the fetch URL with your actual backend URL if needed：
      ```
      fetch('https://localhost:7236/api/Movie/compare-prices')
      ```
