import React, { useState } from "react";
import Button from "../components/ui/Button";
import Input from "../components/ui/Input";
import apiClient from "../api/apiClient,";

const RedefinicaoDeSenha = () => {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [error, setError] = useState("");
  const [message, SetMessage] = useState

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
        <div className="bg-[#262626] p-8 rounded-lg w-[500px] transition-shadow shadow-2xl shadow-black/50">
          <div>
            <h1 className="text-[#FFFFFF] mb-10 text-center text-[24px] font-bold opacity-65">
              Redefinição de Senha
            </h1>

            <form className="space-y-3" onSubmit={handleLogin}>
              <div>
                <Input
                  className="w-44 h-44"
                  type="email"
                  placeholder="Digite seu usuário..."
                  aria-label="Usuário"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                />
              </div>

              <div>
                <Input
                  type="email"
                  placeholder="Digite seu email..."
                  aria-label="Email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
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
                <Button>Recuperar Senha</Button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default RedefinicaoDeSenha;
