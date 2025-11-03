import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import apiClient from "../api/apiClient,";
import FormularioEditarUsuario from "../components/usuarios//FormularioEditarUsuario";
import { ChevronLeft } from "lucide-react";

const USE_MOCK_DATA_GET = true;
const USE_MOCK_DATA_PUT = true;

interface UsuarioApiData {
  Id: number;
  Nome: string;
  UserName: string;
  Email: string;
  Status: number;
}

interface UsuarioViewData {
  id: number;
  nome: string;
  userName: string;
  email: string;
  status: string;
}

const mockUsuarioApi: UsuarioApiData = {
  Id: 7,
  Nome: "Alan M. (Mockado)",
  UserName: "alan.m",
  Email: "alan@empresa.com",
  Status: 1,
};

const transformApiToView = (apiData: UsuarioApiData): UsuarioViewData => ({
  id: apiData.Id,
  nome: apiData.Nome,
  userName: apiData.UserName,
  email: apiData.Email,
  status: apiData.Status === 1 ? "Ativo" : "Inativo",
});

const EditarUsuario: React.FC = () => {
  const { userId } = useParams<{ userId: string }>();
  const navigate = useNavigate();

  const [usuario, setUsuario] = useState<UsuarioViewData | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchUsuario = async () => {
      setLoading(true);
      setError(null);
      try {
        let dadosApi: UsuarioApiData;
        if (USE_MOCK_DATA_GET) {
          await new Promise((res) => setTimeout(res, 500));
          dadosApi = mockUsuarioApi;
        } else {
          const response = await apiClient.get<UsuarioApiData>(
            `/api/Usuarios/${userId}`
          );
          dadosApi = response.data;
        }
        setUsuario(transformApiToView(dadosApi));
      } catch (err) {
        console.error("Erro ao buscar usuário:", err);
        setError("Não foi possível carregar os dados do usuário.");
      } finally {
        setLoading(false);
      }
    };

    fetchUsuario();
  }, [userId]);

  const handleSave = async (formData: UsuarioViewData) => {
    const payload = {
      Nome: formData.nome,
      UserName: formData.userName,
      Email: formData.email,
      Status: formData.status === "Ativo" ? 1 : 0,
    };

    try {
      if (USE_MOCK_DATA_PUT) {
        console.log("Modo Mock: Simulando PUT", payload);
        await new Promise((res) => setTimeout(res, 1000));
      } else {
        await apiClient.put(`/api/Usuarios/${userId}`, payload);
      }
      navigate(-1);
    } catch (err) {
      console.error("Erro ao salvar usuário:", err);
      return "Falha ao salvar. Tente novamente.";
    }
  };

  if (loading) {
    return (
      <div className="p-10 text-white">Carregando dados do usuário...</div>
    );
  }
  if (error) {
    return <div className="p-10 text-red-500">{error}</div>;
  }
  if (!usuario) {
    return <div className="p-10 text-white">Usuário não encontrado.</div>;
  }

  return (
    <div className="p-10 text-white">
      <div className="pt-1 border-t mb-1 border-white/40"></div>
      <button
        onClick={() => navigate(`/empresas/editar/`)}
        className="flex items-center space-x-2 bg-purple-500 hover:bg-purple-700 px-2 py-2 rounded-lg font-semibold transition-colors"
      >
        <ChevronLeft className="w-5 h-5" />
        <span> Voltar</span>
      </button>
      <div className="pt-4 border-t mb-1 mt-3 border-white/40"></div>
      <div className="bg-[#1E1E1E] p-6 rounded-lg shadow-lg border border-gray-800">
        <h1 className="text-3xl font-bold text-white mb-6">
          Editar Usuário: {usuario.nome}
        </h1>
        <FormularioEditarUsuario initialData={usuario} onSave={handleSave} />
      </div>
    </div>
  );
};

export default EditarUsuario;
