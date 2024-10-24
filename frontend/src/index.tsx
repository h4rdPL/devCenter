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
import Home from "./pages/home/home";
import { GoogleOAuthProvider } from "@react-oauth/google";
import { AuthProvider } from "./context/authContext";
import ProtectedRoute from "./pages/protectedRoutes/protectedRoutes";
import { Layout } from "./layout/layout";
import Contractors from "./pages/contracts";
import Archive from "./pages/archive/archive";
import Calendar from "./pages/calendar/calendar";
import Stats from "./pages/stats/stats";
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
      path: "/home",
      element: (
        <ProtectedRoute>
          <Layout>
            <Home />
          </Layout>
        </ProtectedRoute>
      ),
    },

    {
      path: "/contractors",
      element: (
        <ProtectedRoute>
          <Layout>
            <Contractors />
          </Layout>
        </ProtectedRoute>
      ),
    },
    {
      path: "/archive",
      element: (
        <ProtectedRoute>
          <Layout>
            <Archive />
          </Layout>
        </ProtectedRoute>
      ),
    },
    {
      path: "/calendar",
      element: (
        <ProtectedRoute>
          <Layout>
            <Calendar />
          </Layout>
        </ProtectedRoute>
      ),
    },
    {
      path: "/stats",
      element: (
        <ProtectedRoute>
          <Layout>
            <Stats />
          </Layout>
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
