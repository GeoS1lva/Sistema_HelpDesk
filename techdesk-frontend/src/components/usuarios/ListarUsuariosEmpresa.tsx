import { useState } from "react";
import { Plus, Search, Eye, Pencil, Trash2, ChevronDown } from "lucide-react";
import { useNavigate } from "react-router-dom";
import CadastroUsuarioModal from "./CadastroUsuarioModal";
import ConfirmationModal from "../ui/ConfirmationModal";
import apiClient from "../../api/apiClient,";

const USE_MOCK_DATA_DELETE = true;
interface UsuarioData {
  id: number;
  nome: string;
  usuario: string;
  email: string;
  dataCadastro: string;
  status: string;
}

interface ListaUsuariosProps {
  usuarios: UsuarioData[];
  empresaId: number;
  onUsuarioCadastrado: (novoUsuario: any) => void;
  onUsuarioDeletado: (id: number) => void
}

const headers = ["Nome", "E-mail", "Status", "Ações"];

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

interface DeleteModalState {
  isOpen: boolean;
  id: number | null;
}

const ListarUsuariosEmpresa: React.FC<ListaUsuariosProps> = ({
  usuarios,
  empresaId,
  onUsuarioCadastrado,
  onUsuarioDeletado,
}) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const navigate = useNavigate();
  const [deleteModal, setDeleteModal] = useState<DeleteModalState>({
    isOpen: false,
    id: null,
  });
  const [isDeleting, setIsDeleting] = useState(false);

  const handleDeleteClick = (id: number) => {
    setDeleteModal({ isOpen: true, id: id });
  };

  const handleConfirmDelete = async () => {
    const idToDelete = deleteModal.id;
    if (!idToDelete) return;
    setIsDeleting(true);

    try {
      if (USE_MOCK_DATA_DELETE) {
        console.log(`Modo Mock: Deletando empresa com ID: ${idToDelete}`);
        await new Promise((res) => setTimeout(res, 1000));
      } else {
        await apiClient.delete(`/api/usuarios/${idToDelete}`);
      }

      onUsuarioDeletado(idToDelete)

    } catch (err) {
      console.error("Erro ao deletar usuário:", err);
    } finally {
      setDeleteModal({ isOpen: false, id: null });
      setIsDeleting(false);
    }
  };

  const gridColsClass =
    "grid grid-cols-[1.9fr_1.9fr_1.9fr_0.4fr] items-center gap-6 px-5";

  return (
    <div>
      <div className="flex justify-between items-center mb-8 mt-20">
        <h2 className="text-2xl font-bold">Usuários Cadastrados</h2>
        <button
          onClick={() => setIsModalOpen(true)}
          className="flex items-center space-x-2 bg-purple-600 hover:bg-purple-700 px-4 py-2 rounded-lg font-semibold transition-colors"
        >
          <Plus className="w-5 h-5" />
          <span>Cadastrar Usuário</span>
        </button>
      </div>
      <div className="mb-5">
        <div className="relative">
          <input
            type="text"
            placeholder="Pesquisar usuários..."
            className=" text-sm w-[177px] bg-[#262626] border border-gray-700 rounded-lg py-2 pl-10 pr-4 focus:outline-none focus:border-purple-500"
          />
          <Search className="w-5 h-5 text-gray-400 absolute left-3 top-1/2 -translate-y-1/2" />
        </div>
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
        {usuarios.map((usuario) => (
          <div
            key={usuario.id}
            className={`${gridColsClass} py-4 text-sm hover:bg-gray-700/50 transition-colors`}
          >
            <div className="font-medium truncate" title={usuario.nome}>
              {usuario.nome}
            </div>
            <div className="truncate" title={usuario.email}>
              {usuario.email}
            </div>
            <div>
              <StatusBadge status={usuario.status} />
            </div>
            <div>
              <div className="flex space-x-3">
                {/* <button className="text-gray-400 hover:text-blue-500">
                  <Eye className="w-5 h-5" />
                </button> */}
                <button
                  onClick={() => navigate(`/usuarios/editar/${usuario.id}`)}
                  className="text-gray-400 hover:text-yellow-500"
                >
                  <Pencil className="w-5 h-5" />
                </button>
                <button
                  onClick={() => handleDeleteClick(usuario.id)}
                  className="text-gray-400 hover:text-red-500"
                >
                  <Trash2 className="w-5 h-5" />
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>

      {isModalOpen && (
        <CadastroUsuarioModal
          onClose={() => setIsModalOpen(false)}
          empresaId={empresaId}
          onSaveSucess={onUsuarioCadastrado}
        />
      )}

      {deleteModal.isOpen && (
        <ConfirmationModal
          title="Confirmar Exclusão"
          message={`Tem certeza que deseja excluir a empresa? Esta ação não pode ser desfeita.`}
          onClose={() => setDeleteModal({ isOpen: false, id: null })}
          onConfirm={handleConfirmDelete}
          isLoading={isDeleting}
        />
      )}
    </div>
  );
};

export default ListarUsuariosEmpresa;
