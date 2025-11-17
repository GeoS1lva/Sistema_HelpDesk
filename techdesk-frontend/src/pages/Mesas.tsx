import React, { useState, useEffect } from "react";
import { Plus, Search, Pencil, Trash2, ChevronDown } from "lucide-react";
import apiClient from "../api/apiClient";
import ConfirmationModal from "../components/ui/ConfirmationModal";
import CadastroMesaModal from "../components/mesas/CadastroMesaModal";

const USE_MOCK_DATA_GET = false;
const USE_MOCK_DATA_DELETE = false;

interface MesaApiData {
  id: number;
  nome: string;
}

const mockMesas: MesaApiData[] = [
  { id: 1, nome: "Nível 1 - Suporte Geral (Mock)" },
  { id: 2, nome: "Nível 2 - Infraestrutura (Mock)" },
];

const headers = ["Nome da Mesa", "Ações"];

const Mesas: React.FC = () => {
  const [mesas, setMesas] = useState<MesaApiData[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [deleteModal, setDeleteModal] = useState({ isOpen: false, id: 0 });
  const [isDeleting, setIsDeleting] = useState(false);

  useEffect(() => {
    const fetchMesas = async () => {
      setLoading(true);
      setError(null);
      try {
        if (USE_MOCK_DATA_GET) {
          setMesas(mockMesas);
        } else {
          const response = await apiClient.get<MesaApiData[]>(
            "/api/mesasatendimento"
          );
          setMesas(response.data);
        }
      } catch (err) {
        setError("Falha ao carregar as mesas.");
        console.error(err);
      }
      setLoading(false);
    };
    fetchMesas();
  }, []);

  const handleCadastroSuccess = (novaMesa: MesaApiData) => {
    setMesas((mesasAtuais) => [novaMesa, ...mesasAtuais]);
  };

  const handleDeleteClick = (id: number) => {
    setDeleteModal({ isOpen: true, id: id });
  };

  const handleConfirmDelete = async () => {
    const idToDelete = deleteModal.id;
    if (!idToDelete) return;

    setIsDeleting(true);
    try {
      if (USE_MOCK_DATA_DELETE) {
        console.log(`Mock: Deletando mesa: ${idToDelete}`);
        await new Promise((res) => setTimeout(res, 1000));
      } else {
        await apiClient.delete(`/api/mesasatendimento/${idToDelete}`);
      }
      setMesas((mesasAtuais) => mesasAtuais.filter((m) => m.id !== idToDelete));
    } catch (err) {
      console.error("Erro ao deletar mesa:", err);
    } finally {
      setDeleteModal({ isOpen: false, id: 0 });
      setIsDeleting(false);
    }
  };

  const gridColsClass = "grid grid-cols-[3fr_1fr] items-center gap-6 px-6";

  if (loading) return <div className="p-10 text-white">Carregando...</div>;
  if (error) return <div className="p-10 text-red-500">{error}</div>;

  return (
    <div className="p-10 text-white">
      <div className="flex justify-between items-center mb-8">
        <h1 className="text-3xl font-bold">Mesas de Atendimento</h1>
        <button
          onClick={() => setIsModalOpen(true)}
          className="flex items-center space-x-2 bg-purple-600 hover:bg-purple-700 px-4 py-2 rounded-lg font-semibold transition-colors"
        >
          <Plus className="w-5 h-5" />
          <span>Cadastrar Mesa</span>
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
        {mesas.map((mesa) => (
          <div
            key={mesa.id}
            className={`${gridColsClass} py-4 text-sm hover:bg-gray-700/50`}
          >
            <div className="font-medium truncate">{mesa.nome}</div>
            <div className="flex space-x-3">
              <button
                onClick={() => handleDeleteClick(mesa.id)}
                className="text-gray-400 hover:text-red-500"
              >
                <Trash2 className="w-5 h-5" />
              </button>
            </div>
          </div>
        ))}
      </div>

      {isModalOpen && (
        <CadastroMesaModal
          onClose={() => setIsModalOpen(false)}
          onSaveSuccess={handleCadastroSuccess}
        />
      )}
      {deleteModal.isOpen && (
        <ConfirmationModal
          title="Confirmar Exclusão"
          message={`Tem certeza que deseja excluir esta mesa? Técnicos associados podem precisar ser reatribuídos.`}
          onClose={() => setDeleteModal({ isOpen: false, id: 0 })}
          onConfirm={handleConfirmDelete}
          isLoading={isDeleting}
        />
      )}
    </div>
  );
};

export default Mesas;
