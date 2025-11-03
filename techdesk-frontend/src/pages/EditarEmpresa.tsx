import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { ChevronLeft } from "lucide-react";
import FormularioEditarEmpresa from "../components/empresas/FormularioEditarEmpresa";
import ListarUsuariosEmpresa from "../components/usuarios/ListarUsuariosEmpresa";
import apiClient from "../api/apiClient,";

interface EmpresaData {
  id: number;
  nome: string;
  cnpj: string;
  dataCadastro: string;
  email: string;
  status: string;
}

interface UsuarioData {
  id: number;
  nome: string;
  usuario: string;
  email: string;
  dataCadastro: string;
  status: string;
}

const mockEmpresasData: EmpresaData = {
  id: 1,
  nome: "Empresa A",
  cnpj: "00.000.000/0001-00",
  dataCadastro: "2024-05-10",
  email: "contato@empresaa.com",
  status: "Ativo",
};

const mockUsuariosData: UsuarioData[] = [
  {
    id: 1,
    nome: "Alan M.",
    usuario: "alan.m",
    email: "alan@empresa.com",
    dataCadastro: "10/05/2024",
    status: "Ativo",
  },
  {
    id: 2,
    nome: "Beatriz S.",
    usuario: "beatriz.s",
    email: "bia@empresa.com",
    dataCadastro: "11/05/2024",
    status: "Inativo",
  },
];

const EditarEmpresa: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const empresaId = parseInt(id || "0");
  const [empresa, setEmpresa] = useState<EmpresaData>(mockEmpresasData);
  const [usuarios, setUsuarios] = useState<UsuarioData[]>(mockUsuariosData);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    // const fetchDados = async () => {
    //   setLoading(true);
    //   try {
    //     const [empresaRes, usuariosRes] = await Promise.all([
    //       apiClient.get(`/api/empresas/${id}`),
    //       apiClient.get(`/api/usuarios?empresaId=${id}`)
    //     ]);
    //     setEmpresa(empresaRes.data);
    //     setUsuarios(usuariosRes.data);
    //   } catch (error) {
    //     console.error("Erro ao buscar dados:", error);
    //   }
    //   setLoading(false);
    // };
    // fetchDados();
    // setLoading(false)
  }, [id]);

  const handleUsuarioCadastrado = (novoUsuario: UsuarioData) => {
    setUsuarios((usuariosAtuais) => [novoUsuario, ...usuariosAtuais]);
  };

  const handleUsuarioDeletado = (id: number) => {
    setUsuarios((usuariosAtuais) =>
      usuariosAtuais.filter((usuario) => usuario.id !== id)
    );
  };

  if (loading) {
    return <div className="p-10 text-white">Carregando...</div>;
  }

  return (
    <div className="p-10 text-white space-y-3">
      <div className="pt-1 border-t mb-1 border-white/40"></div>
      <button
        onClick={() => navigate("/empresas")}
        className="flex items-center space-x-2 bg-purple-500 hover:bg-purple-700 px-2 py-2 rounded-lg font-semibold transition-colors"
      >
        <ChevronLeft className="w-5 h-5" />
        <span> Voltar</span>
      </button>
      <div className="pt-4 border-t mb-1 border-white/40"></div>
      <FormularioEditarEmpresa initialData={empresa} />

      <ListarUsuariosEmpresa
        usuarios={usuarios}
        empresaId={empresaId}
        onUsuarioCadastrado={handleUsuarioCadastrado}
        onUsuarioDeletado={handleUsuarioDeletado}
      />
    </div>
  );
};

export default EditarEmpresa;
