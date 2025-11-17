import React, { useState, useEffect } from "react";

import { useNavigate, useLocation } from "react-router-dom";
import { Plus, Search, Pencil, Trash2, ChevronDown } from "lucide-react";
import apiClient from "../api/apiClient";
import ConfirmationModal from "../components/ui/ConfirmationModal";
import CadastroCategoriaModal from "../components/categorias/CadastroCategoriaModal";

const USE_MOCK_DATA_GET = false;
const USE_MOCK_DATA_DELETE = false;

interface CategoriaApiData {
  id: number;
  nome: string;
  status: boolean | number;
}

interface CategoriaViewData {
  id: number;
  nome: string;
  status: string;
}

const mockCategorias: CategoriaViewData[] = [
  { id: 1, nome: "Hardware (Mock)", status: "Ativo" },
  { id: 2, nome: "Software (Mock)", status: "Inativo" },
  { id: 3, nome: "Rede (Mock)", status: "Ativo" },
];

const StatusBadge = ({ status }: { status: string }) => (
  <span
    className={`
            px-3 py-1 text-base font-semibold rounded-full 
            ${
              status === "Ativo"
                ? "bg-green-600/20 text-green-400"
                : "bg-red-600/20 text-red-400"
            }
        `}
  >
    {status}
  </span>
);

const transformApiToView = (apiData: CategoriaApiData): CategoriaViewData => ({
  id: apiData.id,
  nome: apiData.nome,
  status: apiData.status ? "Ativo" : "Inativo",
});

const headers = ["Nome da Categoria", "Status", "Ações"];

const Categorias: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const [categorias, setCategorias] = useState<CategoriaViewData[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [deleteModal, setDeleteModal] = useState({ isOpen: false, id: 0 });
  const [isDeleting, setIsDeleting] = useState(false);

  const fetchCategorias = async () => {
    setLoading(true);
    setError(null);
    try {
      if (USE_MOCK_DATA_GET) {
        setCategorias(mockCategorias);
      } else {
        const response = await apiClient.get<CategoriaApiData[]>(
          "/api/categorias",
          {
            headers: {
              "Cache-Control": "no-cache",
              Pragma: "no-cache",
              Expires: "0",
            },
          }
        );

        setCategorias(response.data.map(transformApiToView));
      }
    } catch (err) {
      setError("Falha ao carregar as categorias.");
      console.error(err);
    }
    setLoading(false);
  };

  useEffect(() => {
    fetchCategorias();
  }, []);

  useEffect(() => {
    if (location.state && (location.state as any).refresh) {
      fetchCategorias();
    }
  }, [location.state]);

  const handleCadastroSuccess = (novaCategoriaApi: CategoriaApiData) => {
    const novaCategoriaView = transformApiToView(novaCategoriaApi);
    setCategorias((categoriasAtuais) => [
      novaCategoriaView,
      ...categoriasAtuais,
    ]);
  };

  const gridColsClass =
    "grid grid-cols-[2.5fr_1fr_0.5fr] items-center gap-6 px-6";

  if (loading) return <div className="p-10 text-white">Carregando...</div>;
  if (error) return <div className="p-10 text-red-500">{error}</div>;

  return (
    <div className="p-10 text-white">
      <div className="flex justify-between items-center mb-8">
        <h1 className="text-3xl font-bold">Categorias</h1>
        <button
          onClick={() => setIsModalOpen(true)}
          className="flex items-center space-x-2 bg-purple-600 hover:bg-purple-700 px-4 py-2 rounded-lg font-semibold transition-colors"
        >
          <Plus className="w-5 h-5" />
          <span>Cadastrar Categoria</span>
        </button>
      </div>

      <div className="mb-6"></div>

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
        {categorias.map((categoria) => (
          <div
            key={categoria.id}
            className={`${gridColsClass} py-4 text-sm hover:bg-gray-700/50`}
          >
            <div className="font-bold text-base truncate">{categoria.nome}</div>

            <div>
              <StatusBadge status={categoria.status} />
            </div>

            <div className="flex space-x-3">
              <button
                onClick={() =>
                  navigate(`/categorias/editar/${categoria.id}`, {
                    state: { refresh: true },
                  })
                }
                className="text-gray-400 hover:text-yellow-500"
              >
                <Pencil className="w-5 h-5" />
              </button>
            </div>
          </div>
        ))}
      </div>

      {isModalOpen && (
        <CadastroCategoriaModal
          onClose={() => setIsModalOpen(false)}
          onSaveSuccess={handleCadastroSuccess}
        />
      )}
      {deleteModal.isOpen && (
        <ConfirmationModal
          title="Confirmar Exclusão"
          message={`Tem certeza que deseja excluir esta categoria? Todas as subcategorias associadas também serão afetadas.`}
          onClose={() => setDeleteModal({ isOpen: false, id: 0 })}
          onConfirm={handleConfirmDelete}
          isLoading={isDeleting}
        />
      )}
    </div>
  );
};

export default Categorias;
