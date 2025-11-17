import { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { Plus, Search, Eye, Pencil, Trash2, ChevronDown } from "lucide-react";
import apiClient from "../api/apiClient";
import CadastroEmpresaModal from "../components/empresas/CadastroEmpresaModal";
import ConfirmationModal from "../components/ui/ConfirmationModal";

const USE_MOCK_DATA_GET = false;

interface EmpresaFromApi {
  id: number;
  nome: string;
  cnpj: string;
  email: string;
  status: number;
  dataCriacao: string;
}
interface EmpresaView {
  id: number;
  nome: string;
  cnpj: string;
  email: string;
  status: string;
  dataCriacao: string;
}

const mockEmpresas: EmpresaView[] = [
  {
    id: 1,
    nome: "Empresa A (Mock)",
    cnpj: "00.000.000/0001-00",
    email: "user@gmail.com",
    status: "Ativo",
    dataCriacao: "22",
  },
  {
    id: 2,
    nome: "Empresa B (Mock)",
    cnpj: "11.111.111/0001-11",
    email: "user@gmail.com",
    status: "Inativo",
    dataCriacao: "22",
  },
];

const transformApiToView = (apiData: EmpresaFromApi): EmpresaView => ({
  id: apiData.id,
  nome: apiData.nome,
  cnpj: apiData.cnpj,
  email: apiData.email,
  status: apiData.status ? "Ativo" : "Inativo",
  dataCriacao: apiData.dataCriacao,
});

const headers = ["Nome", "CNPJ", "E-mail", "Status", "Ações"];

const StatusBadge = ({ status }: { status: string }) => (
  <span
    className={`
            px-3 py-1 text-base  font-semibold rounded-full 
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

const Empresas = () => {
  const navigate = useNavigate();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [empresas, setEmpresas] = useState(mockEmpresas);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [deleteModal, setDeleteModal] = useState<DeleteModalState>({
    isOpen: false,
    id: null,
  });

  const fetchEmpreas = async () => {
    setLoading(true);
    setError(null);

    if (USE_MOCK_DATA_GET) {
      setEmpresas(mockEmpresas);
      setLoading(false);
      return;
    }

    try {
      const response = await apiClient.get<EmpresaFromApi[]>("/api/empresas");
      const transformedData = response.data.map(transformApiToView);
      setEmpresas(transformedData);
    } catch (err) {
      setError("Falha ao carregar os dados das empresas.");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchEmpreas();
  }, []);

  const location = useLocation();
  useEffect(() => {
    if (location.state && (location.state as any).refresh) {
      fetchEmpreas();
    }
  }, [location.state]);

  const handleCadastroSuccess = (novaEmpresaApi: EmpresaFromApi) => {
    const novaEmpresaView = transformApiToView(novaEmpresaApi);
    setEmpresas((empresasAtuais) => [novaEmpresaView, ...empresasAtuais]);
  };
  const gridColsClass =
    "grid grid-cols-[2.5fr_1.5fr_2fr_1.5fr_0.4fr] items-center gap-6 px-6";

  if (loading) {
    return <div className="p-10 text-white">Carregando empresas...</div>;
  }

  if (error) {
    return <div className="p-10 text-red-500">{error}</div>;
  }

  return (
    <div className="p-7 text-white">
      <div className="mb-8">
        <div className="relative">
          <input
            type="text"
            placeholder="Pesquisar..."
            className="w-[177px] bg-[#262626] border border-gray-700 rounded-lg py-2 pl-10 pr-4 focus:outline-none focus:border-purple-500"
          />
          <Search className="w-5 h-5 text-gray-400 absolute left-3 top-1/2 -translate-y-1/2" />
        </div>
      </div>
      <div className="mt-auto pt-4 border-t mb-10 border-white/40"></div>
      <div className="mt-auto pt-4 border-t mb-10 border-white/40"></div>

      <div className="flex justify-between itemns-center mb-8">
        <h1 className="text-3xl font-bold">Empresas</h1>
        <button
          onClick={() => setIsModalOpen(true)}
          className="flex items-center space-x-2 bg-purple-500 hover:bg-purple-700 px-4 py-2 rounded-lg font-semibold transition-colors"
        >
          <Plus className="w-5 h-5" />
          <span>Cadastrar Empresas</span>
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
        {empresas.map((empresa) => (
          <div
            key={empresa.id}
            className={`${gridColsClass} py-4 text-sm hover:bg-gray-700/50 transition-colors`}
          >
            <div className="font-bold truncate" title={empresa.nome}>
              {empresa.nome}
            </div>
            <div className="regular">{empresa.cnpj}</div>
            <div className="truncate " title={empresa.email}>
              {empresa.email}
            </div>
            <div>
              <StatusBadge status={empresa.status} />
            </div>
            <div>
              <div className="flex space-x-3">
                <button
                  onClick={() => navigate(`/empresas/editar/${empresa.id}`)}
                  className="text-gray-400 hover:text-yellow-500"
                >
                  <Pencil className="w-5 h-5" />
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>

      {isModalOpen && (
        <CadastroEmpresaModal
          onClose={() => setIsModalOpen(false)}
          onSaveSuccess={handleCadastroSuccess}
        />
      )}
    </div>
  );
};

export default Empresas;
