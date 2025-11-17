import React, { useState, useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import Button from "../components/ui/Button";
import Input from "../components/ui/Input";
import apiClient from "../api/apiClient";

const AlterarSenhaPage = () => {
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const [token, setToken] = useState<string | null>(null);
  const [userName, setUserName] = useState<string | null>(null);

  useEffect(() => {
    const tokenFromUrl = searchParams.get("token");
    const userFromUrl = searchParams.get("userName");

    if (!tokenFromUrl || !userFromUrl) {
      setError("Link de redefinição inválido ou expirado.");
    } else {
      setToken(tokenFromUrl);
      setUserName(userFromUrl);
    }
  }, [searchParams]);

  const handlePasswordReset = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setMessage("");

    if (newPassword !== confirmPassword) {
      setError("As senhas não coincidem.");
      return;
    }
    if (newPassword.length < 6) {
      setError("A senha deve ter no mínimo 6 caracteres.");
      return;
    }

    if (!token || !userName) {
      setError("Não foi possível verificar seu link. Tente novamente.");
      return;
    }

    setIsLoading(true);

    const payload = {
      userName: userName,
      token: token,
      password: newPassword,
    };

    try {
      await apiClient.post("/api/redefinicoes-senha/confirmar", payload);

      setMessage(
        "Senha alterada com sucesso! Você será redirecionado para o login."
      );

      setTimeout(() => {
        navigate("/login");
      }, 3000);
    } catch (err: any) {
      const errorMsg =
        err.response?.data?.message ||
        "Erro ao redefinir a senha. Tente novamente.";
      setError(errorMsg);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-t from-[#2F0D68] to-[#5D1ACE] flex items-center justify-center p-4">
      <div
        className="p-1 rounded-2xl shadow-2xl shadow-black/50"
        style={{
          backgroundImage:
            "repeating-conic-gradient(from 0deg, #000 0deg 25deg, transparent  0deg 30deg)",
        }}
      >
        <div className="bg-[#1E1E1E] p-10 rounded-lg shadow-2xl w-full max-w-lg">
          <h1 className="text-white text-2xl font-semibold text-center mb-10">
            Alterar senha de Acesso
          </h1>

          <form onSubmit={handlePasswordReset}>
            <div className="space-y-3 mb-8">
              <Input
                type="password"
                placeholder="Digite sua senha..."
                aria-label="Nova Senha"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                className="bg-[#2F2F2F] border border-gray-600 text-white rounded"
                disabled={isLoading || !!message}
              />
              <Input
                type="password"
                placeholder="Confirme sua senha..."
                aria-label="Confirme a Nova Senha"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                className="bg-[#2F2F2F] border border-gray-600 text-white rounded"
                disabled={isLoading || !!message}
              />
            </div>

            {error && (
              <p className="text-red-400 text-sm text-center mb-4">{error}</p>
            )}
            {message && (
              <p className="text-green-400 text-sm text-center mb-4">
                {message}
              </p>
            )}

            <div className="space-y-3 items-center justify-center flex flex-col">
              <Button
                type="submit"
                variant="primary"
                className="w-[190px] text-white font-bold py-3 hover:opacity-90 rounded-3xl"
                isLoading={isLoading}
                disabled={isLoading || !!message}
              >
                Salvar
              </Button>

              <Button
                type="button"
                variant="secondary"
                className="w-[190px] items-center flex bg-gradient-to-t from-[#383838] to-[#848383] text-white font-bold py-3 rounded-3xl"
                onClick={() => navigate("/login")}
                disabled={isLoading}
              >
                Cancelar
              </Button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default AlterarSenhaPage;
