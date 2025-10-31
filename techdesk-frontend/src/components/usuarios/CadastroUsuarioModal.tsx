import React, { useState } from "react";
import Modal from "../ui/Modal";
import Input from "../ui/Input";
import Button from "../ui/Button";
import apiClient from "../../api/apiClient,";

const USE_MOCK_DATA_POST = true

interface UsuarioFromApi {
  id: number
  nome: string
  usuario: string;
  email: string
  dataCadastro: string
  status: string
  password: string
}

interface CadastroUsuarioModalProps {
  onClose: () => void;
  empresaId: number
  onSaveSucess: (novoUsuario: UsuarioFromApi) => void
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

const CadastroUsuarioModal: React.FC<CadastroUsuarioModalProps> = ({
  onClose, empresaId, onSaveSucess,
}) => {
  const [nome, setNome] = useState("");
  const [usuario, setUsuario] = useState("");
  const [email, setEmail] = useState("");
  const [status, setStatus] = useState("Ativo");
  const [password, setPassword] = useState("")

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    
    const payload = {
      Nome: nome,
      UserName: usuario, 
      Email: email,
      Status: status === "Ativo" ? 1 : 0, 
      EmpresaId: empresaId,
      Password: password 
    };

    try {
      let novoUsuario: UsuarioFromApi;

      if (USE_MOCK_DATA_POST) {
        console.log("Modo Mock: Simulando POST de Usuário", payload);
        await new Promise((res) => setTimeout(res, 1000));
        novoUsuario = {
          id: Math.floor(Math.random() * 10000),
          nome: payload.Nome,
          usuario: payload.UserName,
          email: payload.Email,
          status: status, 
          password: payload.Password,
          dataCadastro: new Date().toLocaleDateString("pt-BR"),
        };
      } else {
        const response = await apiClient.post<UsuarioFromApi>(
          "/api/usuarios", 
          payload
        );
        novoUsuario = response.data;
      }

      
      onSaveSucess(novoUsuario);
      onClose(); 
    } catch (err) {
      console.error("Erro ao cadastrar usuário:", err);
      setError("Falha ao cadastrar. Tente novamente.");
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
          className="bg-[#606060] h-[42px] border border-[#CAC9CF] focus:outline-none focus:border-purple-500"
          label="Nome"
          type="text"
          value={nome}
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            setNome(e.target.value)
          }
        />
        <LabeledInput
          className="bg-[#606060] h-[42px] border border-[#CAC9CF] focus:outline-none focus:border-purple-500"
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
            className="bg-[#606060] h-[42px] border border-[#CAC9CF] focus:outline-none focus:border-purple-500"
            label="Usuário"
            type="text"
            value={usuario}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setUsuario(e.target.value)
            }
          />
            <LabeledInput
              className="bg-[#606060]  border border-[#CAC9CF] focus:outline-none focus:border-purple-500 w-[278px] h-[42px] "
              label="Senha"
              type="password"
              value={password}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                setPassword(e.target.value)
              }
            />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-300 mb-1">
            Status
          </label>
          <select
            value={status}
            onChange={(e) => setStatus(e.target.value)}
            className="bg-[#606060] border border-[#CAC9CF] text-white rounded-lg p-2 focus:outline-none focus:border-purple-500 w-[288px] h-[42px]"
          >
            <option value="Ativo">Ativo</option>
            <option value="Inativo">Inativo</option>
          </select>
        </div>

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
