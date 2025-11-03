import React, { createContext, useContext, useState, useEffect } from "react";
import apiClient from "../api/apiClient,";

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(false);
  const [isSidebarExpanded, setIsSidebarExpanded] = useState(false);

  const checkAuthStatus = async () => {
    try {
      const res = await apiClient.get("/api/Autenticacao/Profile");

      if (res.data && res.data.UserName) {
        setUser({
          name: res.data.UserName,
          role: res.data.Role,
          email: res.data.Email,
        });
      } else {
        setUser(null);
      }
    } catch (error) {
      setUser(null);
      console.log("Nenhum usuário autenticado.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    checkAuthStatus();
  }, []);

  const logout = async () => {
    setLoading(true);
    // await apiClient.post("/api/Autenticacao/Logout");
    setUser(null);
    setLoading(false);
    window.location.href = "/login";
  };

  const expandSidebar = () => setIsSidebarExpanded(true);
  const collapseSidebar = () => setIsSidebarExpanded(false);

  return (
    <AuthContext.Provider
      value={{
        user,
        loading,
        logout,
        isSidebarExpanded,
        expandSidebar,
        collapseSidebar,
      }}
    >
      {!loading && children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};
