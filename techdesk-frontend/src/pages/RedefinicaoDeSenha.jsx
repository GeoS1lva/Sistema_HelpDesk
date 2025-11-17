import React, { useState } from "react";
import Button from "../components/ui/Button";
import Input from "../components/ui/Input";
import apiClient from "../api/apiClient";
import { useNavigate } from "react-router-dom";
import { ChevronLeft } from "lucide-react";

const RedefinicaoDeSenha = () => {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handlePasswordReset = async (e) => {
    e.preventDefault();
    setError("");
    setMessage("");
    setIsLoading(true);

    if (!username || !email) {
      setError("Por favor, preencha todos os campos.");
      setIsLoading(false);
      return;
    }

    const payload = {
      userName: username,
      email: email,
    };

    try {
      await apiClient.post("/api/redefinicoes-senha", payload);

      console.log("Solicitação enviada para:", payload);
      setMessage("Link de redefinição enviado para o seu e-mail!");
      setUsername("");
      setEmail("");
    } catch (err) {
      console.error(
        "Erro ao solicitar redefinição de senha:",
        err?.response ?? err
      );
      const status = err?.response?.status;
      if (status === 404) {
        setError("Usuário ou e-mail não encontrado. Verifique os dados.");
      } else {
        setError("Ocorreu um erro. Tente novamente mais tarde.");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-t from-[#2F0D68] to-[#5D1ACE] flex items-center justify-center p-4 ">
      <div
        className="w-full max-w-md p-1 rounded-2xl shadow-2xl shadow-black/50"
        style={{
          backgroundImage:
            "repeating-conic-gradient(from 0deg, #000 0deg 25deg, transparent  0deg 30deg)",
        }}
      >
        <div className="bg-[#262626] p-8 sm:p-10 rounded-xl shadow-2xl shadow-black/50">
          <div className="text-center mb-8">
            <h1 className="text-white text-3xl font-bold mb-3">
              Redefinir Senha
            </h1>
            <p className="text-gray-400 text-sm">
              Digite seu usuário e e-mail para enviarmos um link de recuperação.
            </p>
          </div>

          <form className="space-y-6" onSubmit={handlePasswordReset}>
            <div className="space-y-4">
              <div>
                <Input
                  type="text"
                  placeholder="Digite seu usuário..."
                  aria-label="Usuário"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  className="bg-[#2F2F2F] border border-gray-600 text-white rounded-lg text-base p-3"
                />
              </div>

              <div>
                <Input
                  type="email"
                  placeholder="Digite seu e-mail..."
                  aria-label="Email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className="bg-[#2F2F2F] border border-gray-600 text-white rounded-lg text-base p-3"
                />
              </div>
            </div>

            {error && (
              <p className="text-red-400 text-sm text-center !mt-4">{error}</p>
            )}
            {message && (
              <p className="text-green-400 text-sm text-center !mt-4">
                {message}
              </p>
            )}

            <div className="flex flex-col items-center pt-4 space-y-4">
              <Button
                type="submit"
                className="w-full text-lg p-3"
                disabled={isLoading}
              >
                {isLoading ? "Enviando..." : "Recuperar Senha"}
              </Button>

              <Button
                type="button"
                variant="secondary"
                onClick={() => navigate("/login")}
                className="w-full text-gray-400 hover:text-white"
              >
                <ChevronLeft className="w-4 h-4 mr-1 inline-block" />
                Voltar para o Login
              </Button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default RedefinicaoDeSenha;
