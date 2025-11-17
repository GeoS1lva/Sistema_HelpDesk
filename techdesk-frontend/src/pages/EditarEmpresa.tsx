import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { ChevronLeft } from "lucide-react";
import FormularioEditarEmpresa from "../components/empresas/FormularioEditarEmpresa";
import ListarUsuariosEmpresa from "../components/usuarios/ListarUsuariosEmpresa";
import apiClient from "../api/apiClient";

const USE_MOCK_DATA_GET_EMPRESA = false;
const USE_MOCK_DATA_GET_USUARIOS = false;
interface EmpresaApiData {
  id: number;
  nome: string;
  cnpj: string;
  email: string;
  status: number;
  dataCriacao: string;
}

interface EmpresaViewData {
  id: number;
  nome: string;
  cnpj: string;
  email: string;
  status: string;
  dataCriacao: string;
}
interface UsuarioApiData {
  id: number;
  nome: string;
  userName: string;
  email: string;
  status: number;
  dataCriacao: string;
}

interface UsuarioViewData {
  id: number;
  nome: string;
  usuario: string;
  email: string;
  dataCadastro: string;
  status: string;
}

const mockEmpresaApiData: EmpresaApiData = {
  id: 1,
  nome: "Empresa A (Mock)",
  cnpj: "00.000.000/0001-00",
  email: "contato@empresaa.com",
  status: 1,
  dataCriacao: "2024-05-10T10:00:00Z",
};
const mockUsuariosApiData: UsuarioApiData[] = [
  {
    id: 1,
    nome: "Alan M. (Mock)",
    userName: "alan.m",
    email: "alan@empresa.com",
    dataCriacao: "2024-10-20T10:00:00Z",
    status: 1,
  },
  {
    id: 2,
    nome: "Beatriz S. (Mock)",
    userName: "beatriz.s",
    email: "bia@empresa.com",
    dataCriacao: "2024-10-21T10:00:00Z",
    status: 0,
  },
];

const transformEmpresaApiToView = (
  apiData: EmpresaApiData
): EmpresaViewData => ({
  id: apiData.id,
  nome: apiData.nome,
  cnpj: apiData.cnpj,
  email: apiData.email,
  status: apiData.status ? "Ativo" : "Inativo",
  dataCriacao: apiData.dataCriacao.split("T")[0],
});

const transformUsuarioApiToView = (
  apiData: UsuarioApiData
): UsuarioViewData => ({
  id: apiData.id,
  nome: apiData.nome,
  usuario: apiData.userName,
  email: apiData.email,
  status: apiData.status ? "Ativo" : "Inativo",
  dataCadastro: new Date(apiData.dataCriacao).toLocaleDateString("pt-BR"),
});

const EditarEmpresa: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const empresaId = parseInt(id || "0");
  const navigate = useNavigate();

  const [empresa, setEmpresa] = useState<EmpresaViewData | null>(null);
  const [usuarios, setUsuarios] = useState<UsuarioViewData[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchDados = async () => {
      setLoading(true);
      try {
        let empresaData: EmpresaApiData;

        if (USE_MOCK_DATA_GET_EMPRESA) {
          await new Promise((res) => setTimeout(res, 300));
          empresaData = { ...mockEmpresaApiData, id: empresaId };
        } else {
          const response = await apiClient.get<EmpresaApiData>(
            `/api/empresas/${id}`
          );
          empresaData = response.data;
        }

        setEmpresa(transformEmpresaApiToView(empresaData));
      } catch (error) {
        console.error("Erro CRÍTICO ao buscar dados da empresa:", error);
        setLoading(false);
        return;
      }

      try {
        let usuariosData: UsuarioApiData[];
        if (USE_MOCK_DATA_GET_USUARIOS) {
          await new Promise((res) => setTimeout(res, 300));

          if (empresaId === 2) {
            usuariosData = [];
          } else {
            usuariosData = mockUsuariosApiData;
          }
        } else {
          const response = await apiClient.get<UsuarioApiData[]>(
            `/api/empresas/${id}/users`
          );
          usuariosData = response.data;
        }
        setUsuarios(usuariosData.map(transformUsuarioApiToView));
      } catch (error) {
        console.warn(
          "Não foi possível buscar usuários (ou a empresa não tem usuários):",
          error
        );

        setUsuarios([]);
      }

      setLoading(false);
    };

    fetchDados();
  }, [id]);

  const handleUsuarioCadastrado = (novoUsuarioApi: UsuarioApiData) => {
    const novoUsuarioView = transformUsuarioApiToView(novoUsuarioApi);
    setUsuarios((usuariosAtuais) => [novoUsuarioView, ...usuariosAtuais]);
  };

  const handleUsuarioDeletado = (idParaDeletar: number) => {
    setUsuarios((usuariosAtuais) =>
      usuariosAtuais.filter((usuario) => usuario.id !== idParaDeletar)
    );
  };

  if (loading) {
    return <div className="p-10 text-white">Carregando...</div>;
  }

  if (!empresa) {
    return <div className="p-10 text-white">Empresa não encontrada.</div>;
  }

  return (
    <div className="p-10 text-white space-y-3">
      <div className="pt-1 border-t mb-1 border-white/40"></div>
      <button
        onClick={() => navigate("/empresas", { state: { refresh: true } })}
        className="flex items-center space-x-2 bg-purple-500 hover:bg-purple-700 px-2 py-2 rounded-lg font-semibold transition-colors"
      >
        <ChevronLeft className="w-5 h-5" />
        <span> Voltar</span>
      </button>
      <div className="pt-1 border-t mb-1 border-white/40"></div>

      <FormularioEditarEmpresa
        initialData={empresa}
        onDataChange={setEmpresa}
      />

      <ListarUsuariosEmpresa
        usuarios={usuarios}
        empresaId={empresa.id}
        onUsuarioCadastrado={handleUsuarioCadastrado}
        onUsuarioDeletado={handleUsuarioDeletado}
      />
    </div>
  );
};

export default EditarEmpresa;
