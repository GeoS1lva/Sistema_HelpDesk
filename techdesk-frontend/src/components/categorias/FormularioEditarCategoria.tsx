import React, { useState, useEffect } from "react";
import apiClient from "../../api/apiClient";
import Input from "../ui/Input";
import Button from "../ui/Button";
import { CategoriaViewData } from "../../pages/EditarCategoria";

interface FormularioProps {
  initialData: CategoriaViewData;
  onDataChange: (newData: CategoriaViewData) => void;
}

const FormularioEditarCategoria: React.FC<FormularioProps> = ({
  initialData,
  onDataChange,
}) => {
  const [formData, setFormData] = useState(initialData);
  const [isStatusLoading, setIsStatusLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    setFormData(initialData);
  }, [initialData]);

  const handleStatusChange = async (novoStatus: string) => {
    setIsStatusLoading(true);
    setError(null);

    let novoEstadoCompleto: CategoriaViewData | undefined;
    setFormData((prev) => {
      novoEstadoCompleto = { ...prev, status: novoStatus };
      return novoEstadoCompleto;
    });

    const endpoint = novoStatus === "Ativo" ? "ativar" : "inativar";

    try {
      await apiClient.patch(`/api/categorias/${initialData.id}/${endpoint}`);

      if (novoEstadoCompleto) {
        onDataChange(novoEstadoCompleto);
      }
    } catch (err) {
      console.error(`Erro ao ${endpoint} categoria:`, err);
      setError("Falha ao alterar status. Tente novamente.");

      setFormData(initialData);
    } finally {
      setIsStatusLoading(false);
    }
  };

  return (
    <div className="bg-[#1E1E1E] p-6 rounded-lg shadow-lg border border-gray-800">
      <h2 className="text-2xl font-bold mb-6 text-white">
        Gerenciar Categoria
      </h2>

      <form className="space-y-4">
        <div>
          <label className="block text-base font-medium text-gray-300 mb-1">
            Nome da Categoria
          </label>
          <Input
            type="text"
            value={formData.nome}
            disabled
            className="h-[42px] bg-[#3B3B3B] border border-gray-600 cursor-not-allowed text-sm"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-300 mb-1">
            Status
          </label>
          <select
            name="status"
            value={formData.status}
            onChange={(e) => handleStatusChange(e.target.value)}
            disabled={isStatusLoading}
            className={`h-[42px] border border-[#CAC9CF] text-white rounded-lg p-2 focus:outline-none focus:border-purple-500 w-full
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

        {error && <p className="text-red-400 text-sm text-center">{error}</p>}
      </form>
    </div>
  );
};

export default FormularioEditarCategoria;
