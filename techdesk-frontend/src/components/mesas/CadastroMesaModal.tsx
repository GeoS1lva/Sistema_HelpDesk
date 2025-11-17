import React, { useState } from "react";
import Modal from "../ui/Modal";
import Input from "../ui/Input";
import Button from "../ui/Button";
import apiClient from "../../api/apiClient";
import { AxiosError } from "axios";

const USE_MOCK_DATA_POST = false;

interface MesaApiData {
  id: number;
  nome: string;
}

interface CadastroMesaModalProps {
  onClose: () => void;
  onSaveSuccess: (novaMesa: MesaApiData) => void;
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

const CadastroMesaModal: React.FC<CadastroMesaModalProps> = ({
  onClose,
  onSaveSuccess,
}) => {
  const [nome, setNome] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    const payload = {
      nome: nome,
    };

    try {
      let novaMesa: MesaApiData;
      if (USE_MOCK_DATA_POST) {
        console.log("Modo Mock: Cadastrando Mesa", payload);
        await new Promise((res) => setTimeout(res, 1000));
        novaMesa = {
          id: Math.floor(Math.random() * 1000),
          nome: payload.nome,
        };
      } else {
        const response = await apiClient.post<MesaApiData>(
          "/api/mesasatendimento",
          payload
        );
        novaMesa = response.data;
      }
      onSaveSuccess(novaMesa);
      onClose();
    } catch (err) {
      const error = err as AxiosError<any>;
      console.error("Erro ao cadastrar mesa:", error.response);

      if (error.response && error.response.status === 400) {
        if (typeof error.response.data === "string") {
          setError(error.response.data);
        } else {
          setError("Erro de validação desconhecido.");
        }
      } else {
        setError("Falha ao cadastrar. Tente novamente.");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal title="Cadastrar Mesa de Atendimento" onClose={onClose}>
      <form onSubmit={handleSubmit} className="space-y-4">
        <LabeledInput
          label="Nome da Mesa"
          name="nome"
          type="text"
          value={nome}
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            setNome(e.target.value)
          }
          placeholder="Ex: Suporte Nível 1"
          className="bg-[#606060] h-[42px] border border-[#CAC9CF] text-base"
        />

        {error && (
          <p className="text-red-400 text-sm text-center bg-red-900/20 p-2 rounded-lg">
            {error}
          </p>
        )}

        <div className="flex justify-end gap-4 pt-4">
          <Button
            type="button"
            variant="secondary"
            onClick={onClose}
            className="bg-transparent text-red-500 hover:bg-red-500/10 px-4 py-2"
            disabled={isLoading}
          >
            Cancelar
          </Button>
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

export default CadastroMesaModal;
