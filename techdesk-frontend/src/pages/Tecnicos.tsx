import React, { useState, useEffect } from "react";
import { Plus, Search, Pencil, Trash2, ChevronDown } from "lucide-react";
import apiClient from "../api/apiClient";
import ConfirmationModal from "../components/ui/ConfirmationModal";
import CadastroTecnicoModal from "../components/tecnicos/CadastroTecnicoModal";
import { useNavigate } from "react-router-dom";

const USE_MOCK_DATA_GET = false;
const USE_MOCK_DATA_DELETE = false;

interface TecnicoApiData {
  id: number;
  nome: string;
  userName: string;
  email: string;
  tipoPerfil: string;
}

const mockTecnicos: TecnicoApiData[] = [
  {
    id: 1,
    nome: "Alan M. (Mock)",
    userName: "alan.m",
    email: "alan@techdesk.com",
    tipoPerfil: "administrador",
  },
  {
    id: 2,
    nome: "Beatriz S. (Mock)",
    userName: "beatriz.s",
    email: "bia@techdesk.com",
    tipoPerfil: "tecnico",
  },
];

const headers = ["Nome", "Usuário (Login)", "E-mail", "Perfil", "Ações"];

const Tecnicos: React.FC = () => {
  const [tecnicos, setTecnicos] = useState<TecnicoApiData[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [deleteModal, setDeleteModal] = useState({
    isOpen: false,
    userName: "",
  });
  const [isDeleting, setIsDeleting] = useState(false);

  const navigate = useNavigate();

  useEffect(() => {
    const fetchTecnicos = async () => {
      setLoading(true);
      setError(null);
      try {
        if (USE_MOCK_DATA_GET) {
          setTecnicos(mockTecnicos);
        } else {
          const response = await apiClient.get<TecnicoApiData[]>(
            "/api/tecnicos"
          );
          setTecnicos(response.data);
        }
      } catch (err) {
        setError("Falha ao carregar os dados dos técnicos.");
        console.error(err);
      }
      setLoading(false);
    };
    fetchTecnicos();
  }, []);

  const handleCadastroSuccess = (novoTecnico: TecnicoApiData) => {
    setTecnicos((tecnicosAtuais) => [novoTecnico, ...tecnicosAtuais]);
  };

  const handleDeleteClick = (userName: string) => {
    setDeleteModal({ isOpen: true, userName: userName });
  };

  const handleConfirmDelete = async () => {
    const userNameToDelete = deleteModal.userName;
    if (!userNameToDelete) return;

    setIsDeleting(true);
    try {
      if (USE_MOCK_DATA_DELETE) {
        console.log(`Mock: Deletando técnico: ${userNameToDelete}`);
        await new Promise((res) => setTimeout(res, 1000));
      } else {
        await apiClient.delete(`/api/tecnicos/${userNameToDelete}`);
      }
      setTecnicos((tecnicosAtuais) =>
        tecnicosAtuais.filter((t) => t.userName !== userNameToDelete)
      );
    } catch (err) {
      console.error("Erro ao deletar técnico:", err);
    } finally {
      setDeleteModal({ isOpen: false, userName: "" });
      setIsDeleting(false);
    }
  };

  const gridColsClass =
    "grid grid-cols-[2fr_2fr_2fr_1fr_1fr] items-center gap-6 px-6";

  if (loading) return <div className="p-10 text-white">Carregando...</div>;
  if (error) return <div className="p-10 text-red-500">{error}</div>;

  return (
    <div className="p-10 text-white">
      <div className="flex justify-between items-center mb-8">
        <h1 className="text-3xl font-bold">Técnicos e Administradores</h1>
        <button
          onClick={() => setIsModalOpen(true)}
          className="flex items-center space-x-2 bg-purple-600 hover:bg-purple-700 px-4 py-2 rounded-lg font-semibold transition-colors"
        >
          <Plus className="w-5 h-5" />
          <span>Cadastrar Técnico</span>
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
        {tecnicos.map((tecnico) => (
          <div
            key={tecnico.id}
            className={`${gridColsClass} py-4 text-sm hover:bg-gray-700/50`}
          >
            <div className="font-medium truncate">{tecnico.nome}</div>
            <div className="truncate">{tecnico.userName}</div>
            <div className="truncate">{tecnico.email}</div>
            <div className="capitalize">{tecnico.tipoPerfil}</div>
            <div className="flex space-x-3">
              <button
                onClick={() =>
                  navigate(`/tecnicos/editar/${tecnico.userName}`, {
                    state: { refresh: true },
                  })
                }
                className="text-gray-400 hover:text-yellow-500"
              >
                <Pencil className="w-5 h-5" />
              </button>
              <button
                onClick={() => handleDeleteClick(tecnico.userName)}
                className="text-gray-400 hover:text-red-500"
              >
                <Trash2 className="w-5 h-5" />
              </button>
            </div>
          </div>
        ))}
      </div>

      {isModalOpen && (
        <CadastroTecnicoModal
          onClose={() => setIsModalOpen(false)}
          onSaveSuccess={handleCadastroSuccess}
        />
      )}
      {deleteModal.isOpen && (
        <ConfirmationModal
          title="Confirmar Exclusão"
          message={`Tem certeza que deseja excluir o técnico "${deleteModal.userName}"?`}
          onClose={() => setDeleteModal({ isOpen: false, userName: "" })}
          onConfirm={handleConfirmDelete}
          isLoading={isDeleting}
        />
      )}
    </div>
  );
};

export default Tecnicos;
