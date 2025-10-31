import React, { useState } from "react";
import Modal from "../ui/Modal";
import Input from "../ui/Input";
import Button from "../ui/Button";
import apiClient from "../../api/apiClient,";

const USE_MOCK_DATA_POST = true;

interface EmpresaFromApi {
  Id: number;
  Nome: string;
  Cnpj: string;
  Email: string;
  Status: number;
  DataCriacao: string;
}

interface CadastroEmpresaModalProps {
  onClose: () => void;
  onSaveSucess: (novaEmpresa: EmpresaFromApi) => void;
}

const LabeledInput: React.FC<{ label: string; [key: string]: any }> = ({
  label,
  ...props
}) => (
  <div className="flex-1">
    <label className="block text-sm font-medium text-gray-300 mb-1 ">
      {label}
    </label>
    <Input {...props} />
  </div>
);

const CadastroEmpresaModal: React.FC<CadastroEmpresaModalProps> = ({
  onClose,
  onSaveSucess,
}) => {
  const [nome, setNome] = useState("");
  const [cnpj, setCnpj] = useState("");
  const [email, setEmail] = useState("");
  const [status, setStatus] = useState("Ativo");
  const [dataCadastro, setDataCadastro] = useState(
    new Date().toISOString().split("T")[0]
  );

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    const payload = {
      Nome: nome,
      Cnpj: cnpj,
      Email: email,
      DataCriacao: dataCadastro,
      Status: status === "Ativo" ? 1 : 0,
    };

    try {
      let novaEmpresa: EmpresaFromApi;

      if (USE_MOCK_DATA_POST) {
        console.log("Modo Mock: Simulando POST", payload);
        await new Promise((res) => setTimeout(res, 1000));
        novaEmpresa = {
          Id: Math.floor(Math.random() * 1000),
          ...payload,
        };
      } else {
        const res = await apiClient.post<EmpresaFromApi>(
          "/api/empresas",
          payload
        );
        novaEmpresa = res.data;
      }
      onSaveSucess(novaEmpresa);
      onClose();
      console.log("Dados a serem enviados:", payload);
    } catch (err) {
      console.log("Erro ao cadastrar empresa:", err);
      setError("Falha ao cadastrar. Tente novamente.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal title="Cadastrar Empresa" onClose={onClose}>
      <div className="border-t mb-10 border-white/40"></div>
      <form onSubmit={handleSubmit} className="space-y-4">
        <LabeledInput
          className="bg-[#606060] h-[42px] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-base"
          label="Nome"
          type="text"
          value={nome}
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            setNome(e.target.value)
          }
        />

        <div className="flex flex-col md:flex-row gap-4">
          <LabeledInput
            className="bg-[#606060] h-[42px] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-base"
            label="CNPJ"
            type="text"
            value={cnpj}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setCnpj(e.target.value)
            }
          />
          <LabeledInput
            label="Data de Cadastro"
            type="date"
            value={dataCadastro}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setDataCadastro(e.target.value)
            }
            className="bg-[#606060] h-[42px] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-base"
          />
        </div>

        <div className="flex flex-col md:flex-row gap-4">
          <LabeledInput
            label="E-mail"
            type="email"
            value={email}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setEmail(e.target.value)
            }
            className="bg-[#606060] h-[42px] border border-[#CAC9CF] focus:outline-none focus:border-purple-500 text-base"
          />
          <div className="flex-1">
            <label className="block text-sm font-medium text-gray-300 mb-1">
              Status
            </label>
            <select
              value={status}
              onChange={(e) => setStatus(e.target.value)}
              className="bg-[#606060] border border-[#CAC9CF] text-white rounded-lg p-2 focus:outline-none focus:border-purple-500 w-full h-[42px] text-base"
            >
              <option value="Ativo">Ativo</option>
              <option value="Inativo">Inativo</option>
            </select>
          </div>
        </div>

        {error && <p className="text-red-400 text-sm text-center">{error}</p>}


        <div className="flex justify-end gap-4 pt-4">
          <Button
            type="submit"
            className="bg-purple-600 hover:bg-purple-700 text-white px-6 py-2"
            disabled={isLoading}
          >
            {isLoading ? "Cadastrando..." : "Cadastrar"}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default CadastroEmpresaModal;
