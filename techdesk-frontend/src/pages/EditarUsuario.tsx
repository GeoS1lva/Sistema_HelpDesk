import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import apiClient from "../api/apiClient";
import FormularioEditarUsuario, {
  FormularioPayload,
} from "../components/usuarios/FormularioEditarUsuario";
import { ChevronLeft } from "lucide-react";

const USE_MOCK_DATA_GET = false;

interface UsuarioApiData {
  id: number;
  nome: string;
  userName: string;
  email: string;
  status: number;
  dataCriacao: string;
}

export interface UsuarioViewData {
  id: number;
  nome: string;
  userName: string;
  email: string;
  status: string;
}

const mockUsuarioApi: UsuarioApiData = {
  id: 7,
  nome: "Alan M. (Mockado)",
  userName: "alan.m",
  email: "alan@empresa.com",
  status: 1,
  dataCriacao: "2025-01-01T00:00:00Z",
};

const transformApiToView = (apiData: UsuarioApiData): UsuarioViewData => ({
  id: apiData.id,
  nome: apiData.nome,
  userName: apiData.userName,
  email: apiData.email,
  status: apiData.status ? "Ativo" : "Inativo",
});

const EditarUsuario: React.FC = () => {
  const { userName } = useParams<{ userName: string }>();
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
            `/api/usuariosempresas/${userName}`
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
  }, [userName]);

  const handleSave = async (formData: FormularioPayload) => {
    if (!usuario) {
      return "Erro: Dados originais do usuário não carregados.";
    }

    const antigoUserName = usuario.userName;

    try {
      const payload: any = {
        antigoUserName: antigoUserName,
      };

      if (formData.nome !== usuario.nome) {
        payload.nome = formData.nome;
      }
      if (formData.novoUserName !== usuario.userName) {
        payload.novoUserName = formData.novoUserName;
      }
      if (formData.email !== usuario.email) {
        payload.email = formData.email;
      }
      if (formData.password && formData.password.trim() !== "") {
        payload.password = formData.password;
      }

      if (Object.keys(payload).length === 1) {
        console.log("Nenhuma alteração detectada. Nada a salvar.");
        navigate(-1);
        return;
      }

      await apiClient.patch(`/api/usuariosempresas/${antigoUserName}`, payload);

      navigate(-1);
    } catch (err) {
      console.error("Erro ao salvar usuário:", err);
      return "Falha ao salvar. Verifique os dados e tente novamente.";
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
        onClick={() => navigate(-1, { state: { refresh: true } })}
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

        <FormularioEditarUsuario
          initialData={usuario}
          onSave={handleSave}
          onDataChange={setUsuario}
        />
      </div>
    </div>
  );
};

export default EditarUsuario;
