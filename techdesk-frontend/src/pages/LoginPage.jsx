import React, { useState } from "react";

import Button from "../components/ui/Button";
import Input from "../components/ui/Input";


import apiClient from "../api/apiClient,"; 
import { useAuth } from "../contexts/AuthContext";

const LoginPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  
  
  const { login } = useAuth(); 
  const [isLoading, setIsLoading] = useState(false);

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");
    setIsLoading(true);

    try {
      const res = await apiClient.post("/api/autenticacao/login", {
        userName: username, 
        password: password,
      });

      if (res.data && redirect.data.userName) {
        const userData = {
          name: res.data.userName,
          role: res.data.Role
        } 

        login(userData)
        
        window.location.href = "/empresas";
      } else {
        setError("API não retornou os dados do usuário.");
        console.log("Resposta inesperada da API:", res.data)
      }
    } catch (err) {
      setError("Credenciais inválidas. Verifique seu usuário e senha.");
      console.log("Erro no login:", err.response?.data || err.message);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-t from-[#2F0D68] to-[#5D1ACE] flex items-center justify-center p-4">
      <div className="bg-[#262626] p-8 rounded-xl w-full max-w-md shadow-2xl shadow-black/50">
        <div>
          <p className="text-[#7C7E8B] mb-4 text-left text-xs font-semibold opacity-65">
            Bem-vindo
          </p>
          <img
            src="src/assets/logo.png"
            alt="Logo"
            className="mx-auto mb-4 w-[150px] h-[150px]"
          />

          <form className="space-y-5" onSubmit={handleLogin}>
            <div>
              <Input
                placeholder="Digite seu usuário..."
                ariaLabel="Usuário"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                className="bg-[#2F2F2F] border border-gray-600 text-white rounded text-[16px]"
              />
            </div>

            <div>
              <Input
                type="password"
                placeholder="Digite sua senha..."
                ariaLabel="Senha"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="bg-[#2F2F2F] border border-gray-600 text-white rounded text-[16px]"
              />
              <div className="flex justify-end mt-2">
                <a
                  href="#"
                  className="text-[#5D1ACE] text-[10px] sm:text-xs font-semibold hover:underline"
                >
                  Esqueci minha senha
                </a>
              </div>
            </div>

            {error && (
              <p className="text-red-400 text-sm text-center">{error}</p>
            )}

            <div className="flex flex-col items-center pt-4">
              <Button
                type="submit"
                variant="primary"
                className="w-[298px] text-36"
                disabled={isLoading}
              >
                {isLoading ? "Entrando..." : "Entrar"}
              </Button>

              <p className="text-[#7C7E8B] mt-4 text-[9px] sm:text-[10px] font-semibold text-center opacity-50">
                TechDesk: Conectando você a um clique
              </p>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
