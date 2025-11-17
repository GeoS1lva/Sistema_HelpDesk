import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import apiClient from "../../api/apiClient";

import Input from "../ui/Input";
import Button from "../ui/Button";

const USE_MOCK_DATA_PUT = false;
const USE_MOCK_DATA_PATCH = false;

interface EmpresaViewData {
  id: number;
  nome: string;
  cnpj: string;
  email: string;
  status: string;
  dataCriacao: string;
}

interface FormularioProps {
  initialData: EmpresaViewData;
  onDataChange: (newData: EmpresaViewData) => void;
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

const FormularioEditarEmpresa: React.FC<FormularioProps> = ({
  initialData,
  onDataChange,
}) => {
  const [formData, setFormData] = useState(initialData);
  const [isDirty, setIsDirty] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [isStatusLoading, setIsStatusLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  useEffect(() => {
    setFormData(initialData);
    setIsDirty(false);
  }, [initialData]);

  useEffect(() => {
    const hasChanged =
      formData.nome !== initialData.nome ||
      formData.email !== initialData.email;
    setIsDirty(hasChanged);
  }, [formData, initialData]);

  const handleStatusChange = async (novoStatus: string) => {
    setIsStatusLoading(true);
    setError(null);

    let novoEstadoCompleto: EmpresaViewData | undefined;

    setFormData((prev) => {
      novoEstadoCompleto = { ...prev, status: novoStatus };
      return novoEstadoCompleto;
    });

    const endpoint = novoStatus === "Ativo" ? "ativar" : "inativar";

    try {
      if (USE_MOCK_DATA_PATCH) {
        console.log(`Mock: PATCH /api/empresas/${initialData.id}/${endpoint}`);
        await new Promise((res) => setTimeout(res, 1000));
      } else {
        await apiClient.patch(`/api/empresas/${initialData.id}/${endpoint}`);
      }

      if (novoEstadoCompleto) {
        onDataChange(novoEstadoCompleto);
      } else {
        console.warn("Atualização de estado do pai usou fallback.");
        onDataChange((prev) => ({ ...prev, status: novoStatus }));
      }
    } catch (err) {
      console.error(`Erro ao ${endpoint} empresa:`, err);
      setError("Falha ao alterar status. Tente novamente.");
      setFormData(initialData);
    } finally {
      setIsStatusLoading(false);
    }
  };

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    if (name === "status") {
      handleStatusChange(value);
    } else {
      setFormData((prev) => ({ ...prev, [name]: value }));
    }
  };

  const handleCancel = () => {
    setFormData((prev) => ({
      ...prev,
      nome: initialData.nome,
      email: initialData.email,
    }));
    setError(null);
  };

  const handleSave = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    const updatePromises: Promise<any>[] = [];

    if (formData.nome !== initialData.nome) {
      updatePromises.push(
        apiClient.patch(`/api/empresas/${formData.id}/nome`, {
          nome: formData.nome,
        })
      );
    }

    if (formData.email !== initialData.email) {
      updatePromises.push(
        apiClient.patch(`/api/empresas/${formData.id}/email`, {
          email: formData.email,
        })
      );
    }

    if (updatePromises.length === 0) {
      setIsLoading(false);
      return;
    }

    try {
      if (USE_MOCK_DATA_PUT) {
        console.log(
          `Modo Mock: Simulando ${updatePromises.length} chamadas PUT...`
        );
        await new Promise((res) => setTimeout(res, 1000));
      } else {
        await Promise.all(updatePromises);
      }
      onDataChange({
        ...initialData,
        nome: formData.nome,
        email: formData.email,
      });
      setIsLoading(false);
      navigate("/empresas", { state: { refresh: true } });
    } catch (err) {
      console.error("Erro ao salvar alterações:", err);
      setError("Falha ao salvar uma ou mais alterações. Tente novamente.");
      setIsLoading(false);
    }
  };

  return (
    <div className="bg-[#1E1E1E] p-6 rounded-lg shadow-lg border border-gray-800">
      <h2 className="text-2xl font-bold mb-6 text-white">
        Empresa: {initialData.nome}
      </h2>

      <form onSubmit={handleSave} className="space-y-4">
        <LabeledInput
          label="Nome Fantasia"
          name="nome"
          type="text"
          value={formData.nome}
          onChange={handleChange}
          disabled={isLoading}
          className="h-[42px] bg-[#606060] text-base border border-[#CAC9CF] focus:outline-none focus:border-purple-500"
        />

        <div className="flex flex-col md:flex-row gap-4">
          <LabeledInput
            label="CNPJ"
            name="cnpj"
            type="text"
            value={formData.cnpj}
            disabled
            className="h-[42px] bg-[#3B3B3B] border border-gray-600 cursor-not-allowed text-base"
          />
          <LabeledInput
            label="Data de Cadastro"
            name="dataCadastro"
            type="date"
            value={formData.dataCriacao}
            disabled
            onChange={handleChange}
            className="h-[42px] bg-[#3B3B3B] text-base border border-gray-600 cursor-not-allowed"
          />
        </div>

        <div className="flex flex-col md:flex-row gap-4 ">
          <LabeledInput
            label="E-mail"
            name="email"
            type="email"
            value={formData.email}
            onChange={handleChange}
            disabled={isLoading}
            className="h-[42px] bg-[#606060] text-base border border-[#CAC9CF] focus:outline-none focus:border-purple-500"
          />
          <div className="flex-1">
            <label className="block text-base font-medium text-gray-300 mb-1">
              Status
            </label>
            <select
              name="status"
              value={formData.status}
              onChange={handleChange}
              disabled={isStatusLoading || isLoading}
              className={`h-[42px] border border-[#CAC9CF] text-white rounded-lg p-2 focus:outline-none w-full
                ${
                  isStatusLoading
                    ? "bg-gray-700 animate-pulse cursor-not-allowed"
                    : "bg-[#606060]"
                }
              `}
            >
              <option value="Ativo">Ativo</option>
              <option value="Inativo">Inativo</option>
            </select>
          </div>
        </div>

        {error && <p className="text-red-400 text-sm text-center">{error}</p>}

        {isDirty && (
          <div className="flex justify-end gap-4 pt-4">
            <Button
              type="button"
              variant="secondary"
              onClick={handleCancel}
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
              {isLoading ? "Salvando..." : "Salvar Alterações"}
            </Button>
          </div>
        )}
      </form>
    </div>
  );
};

export default FormularioEditarEmpresa;
