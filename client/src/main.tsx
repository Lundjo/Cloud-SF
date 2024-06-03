import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./index.css";
import { AuthProvider } from "./contexts/auth/AuthContext.tsx";

ReactDOM.createRoot(document.getElementById("root")!).render(
  // dodaj ovde auth context vrv
  <React.StrictMode>
    <AuthProvider>
      <App />
    </AuthProvider>
  </React.StrictMode>
);
