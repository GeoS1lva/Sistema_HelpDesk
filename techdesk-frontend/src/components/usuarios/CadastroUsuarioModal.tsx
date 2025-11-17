import React, { useState } from "react";
import Modal from "../ui/Modal";
import Input from "../ui/Input";
import Button from "../ui/Button";
import apiClient from "../../api/apiClient";
import { AxiosError } from "axios";

const USE_MOCK_DATA_POST = false;

interface UsuarioFromApi {
  id: number;
  nome: string;
  userName: string;
  email: string;
  dataCriacao: string;
  status: number;
}

interface CadastroUsuarioModalProps {
  onClose: () => void;
  empresaId: number;
  onSaveSucess: (novoUsuario: UsuarioFromApi) => void;
}

const LabeledInput: React.FC<{ label: string; [key: string]: any }> = ({
  label,
  ...props
}) => (
  <div className="flex-1">
    <label className="block text-sm font-medium text-gray-300 mb-1">
      {label}
    </label>
    <Input {...props} />
  </div>
);

interface ValidationErrors {
  title: string;
  status: number;
  errors: {
    [key: string]: string[];
  };
}

const CadastroUsuarioModal: React.FC<CadastroUsuarioModalProps> = ({
  onClose,
  empresaId,
  onSaveSucess,
}) => {
  const [nome, setNome] = useState("");
  const [usuario, setUsuario] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    const payload = {
      nome: nome,
      userName: usuario,
      email: email,
      empresaId: empresaId,
      password: password,
    };

    try {
      let novoUsuario: UsuarioFromApi;

      if (USE_MOCK_DATA_POST) {
        console.log("Modo Mock: Simulando POST de Usuário", payload);
        await new Promise((res) => setTimeout(res, 1000));
        novoUsuario = {
          id: Math.floor(Math.random() * 10000),
          nome: payload.nome,
          userName: payload.userName,
          email: payload.email,
          status: 2,
          dataCriacao: new Date().toLocaleDateString("pt-BR"),
        };
      } else {
        const response = await apiClient.post<UsuarioFromApi>(
          "/api/usuariosempresas",
          payload
        );
        novoUsuario = response.data;
      }

      onSaveSucess(novoUsuario);
      onClose();
    } catch (err) {
      const error = err as AxiosError<any>;
      console.error("Erro ao cadastrar usuário:", error.response);

      if (error.response && error.response.status === 400) {
        if (typeof error.response.data === "string") {
          setError(error.response.data);
        } else if (error.response.data && error.response.data.errors) {
          const validationErrors = error.response.data.errors as {
            [key: string]: string[];
          };
          const errorMessages = Object.keys(validationErrors)
            .map((field) => {
              const messages = validationErrors[field].join(" ");
              const fieldName = field.charAt(0).toUpperCase() + field.slice(1);
              return `${fieldName}: ${messages}`;
            })
            .join(" | ");
          setError(errorMessages);
        } else {
          setError("Erro de validação desconhecido.");
        }
      } else {
        setError("Falha ao cadastrar. Verifique os dados e tente novamente.");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal title="Cadastrar Usuário" onClose={onClose}>
      <div className="border-t mb-10 border-white/40"></div>
      <form onSubmit={handleSubmit} className="space-y-4">
        <div className="flex flex-col md:flex-row gap-4">
          <LabeledInput
            className="bg-[#606060] h-[42px] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-base"
            label="Nome"
            type="text"
            value={nome}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setNome(e.target.value)
            }
          />
          <LabeledInput
            className="bg-[#606060] h-[42px] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-base"
            label="E-mail"
            type="email"
            value={email}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setEmail(e.target.value)
            }
          />
        </div>

        <div className="flex flex-col md:flex-row gap-4">
          <LabeledInput
            className="bg-[#606060] h-[42px] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-base"
            label="Usuário"
            type="text"
            value={usuario}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setUsuario(e.target.value)
            }
          />
          <LabeledInput
            className="bg-[#606060]  border border-[#CAC9CF] focus:outline-none focus:border-purple-500 w-[278px] h-[42px] text-base"
            label="Senha"
            type="password"
            value={password}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setPassword(e.target.value)
            }
          />
        </div>

        {error && (
          <p className="text-red-400 text-sm text-center p-2 rounded-lg">
            {error}
          </p>
        )}

        <div className="flex justify-end gap-4 pt-4">
          <Button
            type="submit"
            className="bg-purple-600 hover:bg-purple-700 text-white px-6 py-2"
          >
            Cadastrar
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default CadastroUsuarioModal;
