import React, { useState } from "react";
import Button from "../components/ui/Button";
import Input from "../components/ui/Input";
import apiClient from "../api/apiClient,";

const LoginPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleLogin = async (e) => {};

  return (
    <div className="min-h-screen bg-gradient-to-t from-[#2F0D68] to-[#5D1ACE] flex items-center justify-center p-4">
      <div
        className="p-1 rounded-2xl shadow-2xl shadow-black/50"
        style={{
          backgroundImage:
            "repeating-conic-gradient(from 0deg, #000 0deg 25deg, transparent  0deg 30deg)",
        }}
      >
        <div className="bg-[#262626] p-8 rounded-lg w-full max-w-sm sm:max-w-md transition-shadow shadow-2xl shadow-black/50">
          <div>
            <p className="text-[#7C7E8B] mb-1 text-left text-xs font-semibold opacity-65">
              Bem-vindo
            </p>
            <img
              src="src/assets/logo.png"
              alt="Logo"
              className="mx-auto mb-4 w-[100px] h-[100px]"
            />

            <form className="space-y-3" onSubmit={handleLogin}>
              <div>
                <Input
                  type="email"
                  placeholder="Digite seu usuário..."
                  aria-label="Usuário"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                />
              </div>

              <div>
                <Input
                  type="password"
                  placeholder="Digite sua senha..."
                  aria-label="Senha"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />

                <div className="flex justify-end mt-1">
                  <a
                    href="#"
                    className="text-[#9067D7] text-[10px] sm:text-xs font-semibold hover:underline"
                  >
                    Esqueci minha senha
                  </a>
                </div>
              </div>

              {error && (
                <p className="text-red-400 text-sm text-center">{error}</p>
              )}

              <div>
                <Button>Login</Button>

                <p className="text-[#7C7E8B] mt-2 text-[9px] sm:text-[10px] font-semibold text-center opacity-50">
                  TechDesk: Conectando você a um clique
                </p>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
