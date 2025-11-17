import React, { createContext, useContext, useState } from "react";
import apiClient from "../api/apiClient";

const defaultContextValue = {
  user: null,
  loading: false,
  isSidebarExpanded: true,
  login: () => {},
  logout: () => {},
  expandSidebar: () => {},
  collapseSidebar: () => {},
};

const AuthContext = createContext(defaultContextValue);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(() => {
    try {
      const storedUser = localStorage.getItem("techdeskUser");
      return storedUser ? JSON.parse(storedUser) : null;
    } catch (e) {
      return null;
    }
  });

  const [loading, setLoading] = useState(false);
  const [isSidebarExpanded, setIsSidebarExpanded] = useState(true);

  const login = (userData) => {
    setUser(userData);
    localStorage.setItem("techdeskUser", JSON.stringify(userData));
  };

  const logout = async () => {
    setLoading(true);
    try {
      await apiClient.delete("/api/autenticacoes");
    } catch (error) {
      console.error("Erro no logout:", error);
    }
    setUser(null);
    localStorage.removeItem("techdeskUser");
    setLoading(false);
    window.location.href = "/login";
  };

  const expandSidebar = () => setIsSidebarExpanded(true);
  const collapseSidebar = () => setIsSidebarExpanded(false);

  const value = {
    user,
    loading,
    login,
    logout,
    isSidebarExpanded,
    expandSidebar,
    collapseSidebar,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined || context === null) {
    throw new Error("useAuth deve ser usado dentro de um AuthProvider");
  }
  return context;
};
