import React, { useState } from "react";
import Button from "../components/ui/Button";
import Input from "../components/ui/Input";
import apiClient from "../api/apiClient,";

const RedefinicaoDeSenha = () => {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [error, setError] = useState("");
  const [message, SetMessage] = useState("");

  const handlePasswordReset = async (e) => {
    e.preventDefault();
    setError("");
    SetMessage("");
  };

  return (
    <div className="min-h-screen bg-gradient-to-t from-[#2F0D68] to-[#5D1ACE] flex items-center justify-center p-4 ">
      <div
        className="p-1 rounded-2xl shadow-2xl shadow-black/50"
        style={{
          backgroundImage:
            "repeating-conic-gradient(from 0deg, #000 0deg 25deg, transparent  0deg 30deg)",
        }}
      >
        <div className="bg-[#262626]  p-8 rounded-xl w-full max-w-md shadow-2xl shadow-black/50">
          <div className="text-center mb-8">
            <h1 className="text-white text-2xl font-bold mb-6">
              Redefinição de Senha
            </h1>

            <p className="text-[#7C7E8B] mt-2 text-[9px] sm:text-[11px] font-semibold text-center opacity-50">
              Digite seu usuário e endereço de e-mail e lhe enviaremos um link
              para redefinição de senha
            </p>
          </div>

          <form className="space-y-6" onSubmit={handlePasswordReset}>
            <div>
              <Input
                type="text"
                placeholder="Digite seu usuário..."
                aria-label="Usuário"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
            </div>

            <div>
              <Input
                type="email"
                placeholder="Digite seu e-mail..."
                aria-label="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>

            {error && (
              <p className="text-red-400 text-sm text-center">{error}</p>
            )}

            {/* Exibição de Mensagem de Sucesso */}
            {message && (
              <p className="text-green-400 text-sm text-center">{message}</p>
            )}

            {/* Botão de Ação */}
            <div className="ml-14">
              <Button type="submit">Recuperar senha</Button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default RedefinicaoDeSenha;
