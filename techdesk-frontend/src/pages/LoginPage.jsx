import React, { useState } from "react";
import Button from "../components/ui/Button";
import Input from "../components/ui/Input";
import { useNavigate } from "react-router-dom";
import apiClient from "../api/apiClient";
import { useAuth } from "../contexts/AuthContext";

const UserTypeToggle = ({ userType, setUserType }) => {
  const baseStyle =
    "py-2 px-6 rounded-md text-sm font-semibold transition-colors";
  const activeStyle = "bg-purple-600 text-white";
  const inactiveStyle = "bg-gray-700 text-gray-400 hover:bg-gray-600";

  return (
    <div className="flex w-full bg-gray-700 rounded-md p-1 mb-6">
      <button
        type="button"
        onClick={() => setUserType("tecnico")}
        className={`w-1/2 ${baseStyle} ${
          userType === "tecnico" ? activeStyle : inactiveStyle
        }`}
      >
        Sou Técnico
      </button>
      <button
        type="button"
        onClick={() => setUserType("cliente")}
        className={`w-1/2 ${baseStyle} ${
          userType === "cliente" ? activeStyle : inactiveStyle
        }`}
      >
        Sou Cliente
      </button>
    </div>
  );
};

const LoginPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const { login } = useAuth();
  const [isLoading, setIsLoading] = useState(false);
  const [userType, setUserType] = useState("tecnico");

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");
    setIsLoading(true);

    const isTecnico = userType === "tecnico";

    const endpoint = isTecnico
      ? "/api/autenticacoes"
      : "/api/autenticacoes/painel-chamado";

    const payload = {
      userName: username,
      password: password,
    };

    try {
      const res = await apiClient.post(endpoint, payload);

      if (res.data && res.data.userName) {
        const userData = {
          id: res.data.id,
          userName: res.data.userName,
          role: res.data.role,
          email: res.data.email,
        };

        login(userData);

        const role = userData.role.toLowerCase();

        if (role === "administrador" || role === "tecnico") {
          navigate("/tickets");
        } else {
          navigate("/portal/dashboard");
        }
      } else {
        setError("API não retornou os dados do usuário.");
        console.log("Resposta inesperada da API:", res.data);
      }
    } catch (err) {
      setError("Credenciais inválidas. Verifique seu usuário e senha.");
      console.log("Erro no login:", err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-t from-[#2F0D68] to-[#5D1ACE] flex items-center justify-center p-4">
      <div
        className="w-full max-w-md p-1 rounded-2xl shadow-2xl shadow-black/50"
        style={{
          backgroundImage:
            "repeating-conic-gradient(from 0deg, #000 0deg 25deg, transparent  0deg 30deg)",
        }}
      >
        <div className="bg-[#262626] p-8 sm:p-10 rounded-xl shadow-2xl shadow-black/50">
          <div className="text-center mb-6">
            <p className="text-[#7C7E8B] mb-2 text-sm font-semibold opacity-65">
              Bem-vindo ao TechDesk
            </p>
            <img
              src="src/assets/logo2.png"
              alt="Logo TechDesk"
              className="mx-auto w-40 h-40"
            />
          </div>

          <UserTypeToggle userType={userType} setUserType={setUserType} />

          <form className="space-y-6" onSubmit={handleLogin}>
            <div className="space-y-4">
              <div>
                <Input
                  placeholder="Digite seu usuário..."
                  ariaLabel="Usuário"
                  value={username}
                  onChange={(e) => setUsername(e.target.value)}
                  className="bg-[#2F2F2F] border border-gray-600 text-white rounded-lg text-base p-3"
                />
              </div>
              <div>
                <Input
                  type="password"
                  placeholder="Digite sua senha..."
                  ariaLabel="Senha"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  className="bg-[#2F2F2F] border border-gray-600 text-white rounded-lg text-base p-3"
                />
                <div className="flex justify-end mt-2">
                  <a
                    onClick={() => navigate("/redefinir-senha")}
                    className="text-[#5D1ACE] text-sm font-semibold hover:underline cursor-pointer"
                  >
                    Esqueci minha senha
                  </a>
                </div>
              </div>
            </div>

            {error && (
              <p className="text-red-400 text-sm text-center !mt-4">{error}</p>
            )}

            <div className="flex flex-col items-center pt-4">
              <Button
                type="submit"
                variant="primary"
                className="w-full text-lg p-3"
                disabled={isLoading}
              >
                {isLoading ? "Entrando..." : "Entrar"}
              </Button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
