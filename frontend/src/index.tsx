import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import App from "./App";
import reportWebVitals from "./reportWebVitals";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import Homepage from "./pages/homepage/homepage";
import { ThemeProvider } from "styled-components";
import { theme } from "./styles/theme/theme";
import Login from "./pages/login/login";
import Dashboard from "./pages/dashboard/dashboard";
import Home from "./pages/home/home";
import { GoogleOAuthProvider } from "@react-oauth/google";
import { AuthProvider } from "./context/authContext";
import ProtectedRoute from "./pages/protectedRoutes/protectedRoutes";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

const clientId = process.env.REACT_APP_GOOGLE_CLIENT_ID;

if (clientId) {
  const router = createBrowserRouter([
    {
      path: "/",
      element: <Homepage />,
    },
    {
      path: "/login",
      element: <Login />,
    },
    {
      path: "/dashboard",
      element: <Dashboard />,
    },
    {
      path: "/home",
      element: (
        <ProtectedRoute>
          <Home />
        </ProtectedRoute>
      ),
    },
  ]);

  root.render(
    <GoogleOAuthProvider clientId={clientId}>
      <React.StrictMode>
        <ThemeProvider theme={theme}>
          <AuthProvider>
            <RouterProvider router={router}></RouterProvider>
            <App />
          </AuthProvider>
        </ThemeProvider>
      </React.StrictMode>
    </GoogleOAuthProvider>
  );
} else {
  console.error("Client ID is not defined");
}

reportWebVitals();
