import React, { createContext, useContext, useState, useEffect } from "react";
import apiClient from "../api/apiClient,";


const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [isSidebarExpanded, setIsSidebarExpanded] = useState(false);

  const login = (userData) => {
    setUser(userData)
  }

  const logout = async () => {
    try {
     
      await apiClient.post("/api/autenticacao/logout"); 
    } catch (error) {
      console.error("Erro ao fazer logout:", error);
    }
    
    setUser(null);
    window.location.href = '/login';
  };

  const expandSidebar = () => setIsSidebarExpanded(true)
  const collapseSidebar = () => setIsSidebarExpanded(false)

  return (
    <AuthContext.Provider
      value={{ user, login, logout, isSidebarExpanded, expandSidebar, collapseSidebar }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};
