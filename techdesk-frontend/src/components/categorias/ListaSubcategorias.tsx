import React, { useState } from "react";
import {
  Plus,
  Pencil,
  Trash2,
  CheckCircle,
  XCircle,
  Loader2,
  ChevronDown,
} from "lucide-react";
import { SubcategoriaApiData } from "../../pages/EditarCategoria";
import CadastroSubcategoriaModal from "./CadastroSubcategoriaModal";
import apiClient from "../../api/apiClient";

interface ListaProps {
  categoriaId: number;
  subcategorias: SubcategoriaApiData[];
  onSubcategoriaAdicionada: (novaSub: SubcategoriaApiData) => void;
  onSubcategoriaChange: (updatedSub: SubcategoriaApiData) => void;
}

const StatusBadge = ({
  status,
}: {
  status: string | boolean | undefined | null;
}) => {
  const isAtivo = status === "Ativo" || status === true;
  return (
    <span
      className={`
        px-3 py-1 text-base font-semibold rounded-full 
        ${
          isAtivo
            ? "bg-green-600/20 text-green-400"
            : "bg-red-600/20 text-red-400"
        }
      `}
    >
      {isAtivo ? "Ativo" : "Inativo"}
    </span>
  );
};

const headers = [
  "Nome da Subcategoria",
  "Prioridade",
  "SLA (Horas)",
  "Status",
  "Ações",
];
const gridColsClass =
  "grid grid-cols-[2fr_1fr_1fr_1fr_0.5fr] items-center gap-6 px-6";

const ListaSubcategorias: React.FC<ListaProps> = ({
  categoriaId,
  subcategorias,
  onSubcategoriaAdicionada,
  onSubcategoriaChange,
}) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [loadingId, setLoadingId] = useState<number | null>(null);

  const handleToggleStatus = async (sub: SubcategoriaApiData) => {
    setLoadingId(sub.id);

    const isAtivo = sub.status === "Ativo" || sub.status === true;
    const novoStatus = isAtivo ? "Inativo" : "Ativo";
    const endpoint = isAtivo
      ? `/api/categorias/subCategoria/${sub.id}/inativar`
      : `/api/categorias/subCategoria/${sub.id}/ativar`;

    try {
      await apiClient.patch(
        endpoint,
        {},
        {
          headers: { "Content-Type": "application/json" },
        }
      );

      const updatedSub = { ...sub, status: novoStatus === "Ativo" };

      onSubcategoriaChange(updatedSub);
    } catch (err) {
      console.error(`Falha ao ${novoStatus.toLowerCase()} subcategoria.`, err);
    } finally {
      setLoadingId(null);
    }
  };

  return (
    <div>
      <div className="flex justify-between items-center mb-8">
        <h2 className="text-2xl font-bold">Subcategorias</h2>
        <button
          onClick={() => setIsModalOpen(true)}
          className="flex items-center space-x-2 bg-purple-600 hover:bg-purple-700 px-4 py-2 rounded-lg font-semibold transition-colors"
        >
          <Plus className="w-5 h-5" />
          <span>Cadastrar Subcategoria</span>
        </button>
      </div>
      <div
        className={`${gridColsClass} bg-[#3B3B3B] py-3 rounded-t-lg text-base font-semibold text-white shadow-gray-100`}
      >
        {headers.map((header) => (
          <div
            key={header}
            className="flex items-center space-x-1 font-extrabold"
          >
            <span>{header}</span>
            {header !== "Ações" && (
              <ChevronDown className="w-4 h-4 text-purple-500" />
            )}
          </div>
        ))}
      </div>

      <div className="bg-[#3B3B3B] mt-3 rounded-b-lg shadow-md divide-y divide-gray-800 text-gray-400">
        {subcategorias.length > 0 ? (
          subcategorias.map((sub) => (
            <div
              key={sub.id}
              className={`${gridColsClass} py-4 text-base  hover:bg-gray-700/50`}
            >
              <div className="font-bold truncate">{sub.nome}</div>
              <div className="capitalize">{sub.prioridade}</div>
              <div className="truncate">{sub.sla / 3600}h</div>

              <div>
                <StatusBadge status={sub.status} />
              </div>

              <div className="flex space-x-3">
                <button
                  onClick={() => handleToggleStatus(sub)}
                  disabled={loadingId === sub.id}
                  className={`text-gray-400 ${
                    sub.status === "Ativo" || sub.status === true
                      ? "hover:text-red-500"
                      : "hover:text-green-500"
                  }`}
                  title={sub.status ? "Inativar" : "Ativar"}
                >
                  {loadingId === sub.id ? (
                    <Loader2 className="w-5 h-5 animate-spin" />
                  ) : sub.status === "Ativo" || sub.status === true ? (
                    <XCircle className="w-5 h-5" />
                  ) : (
                    <CheckCircle className="w-5 h-5" />
                  )}
                </button>
              </div>
            </div>
          ))
        ) : (
          <div className="p-4 text-center text-gray-400">
            Nenhuma subcategoria cadastrada.
          </div>
        )}
      </div>

      {isModalOpen && (
        <CadastroSubcategoriaModal
          categoriaId={categoriaId}
          onClose={() => setIsModalOpen(false)}
          onSaveSuccess={onSubcategoriaAdicionada}
        />
      )}
    </div>
  );
};

export default ListaSubcategorias;
