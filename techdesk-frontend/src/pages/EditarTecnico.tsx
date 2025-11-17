import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { ChevronLeft } from "lucide-react";
import apiClient from "../api/apiClient";
import FormularioEditarTecnico, {
  FormularioPayload,
} from "../components/tecnicos/FormularioEditarTecnico";

export interface TecnicoViewData {
  nome: string;
  userName: string;
  email: string;
  tipoPerfil: string;
}

interface TecnicoApiData {
  id: number;
  nome: string;
  userName: string;
  email: string;
  tipoPerfil: string;
}

const EditarTecnico: React.FC = () => {
  const { userName } = useParams<{ userName: string }>();
  const navigate = useNavigate();

  const [tecnico, setTecnico] = useState<TecnicoViewData | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchTecnico = async () => {
      setLoading(true);
      setError(null);
      try {
        const response = await apiClient.get<TecnicoApiData>(
          `/api/tecnicos/${userName}`
        );
        setTecnico({
          nome: response.data.nome,
          userName: response.data.userName,
          email: response.data.email,
          tipoPerfil: response.data.tipoPerfil,
        });
      } catch (err) {
        console.error("Erro ao buscar técnico:", err);
        setError("Não foi possível carregar os dados do técnico.");
      } finally {
        setLoading(false);
      }
    };

    fetchTecnico();
  }, [userName]);

  const handleSave = async (formData: FormularioPayload) => {
    if (!userName || !tecnico) {
      return "Erro: Dados originais do técnico não carregados.";
    }

    try {
      const payload: any = {
        antigoUserName: userName,
      };

      if (formData.nome !== tecnico.nome) {
        payload.nome = formData.nome;
      }
      if (formData.novoUserName !== tecnico.userName) {
        payload.novoUserName = formData.novoUserName;
      }
      if (formData.email !== tecnico.email) {
        payload.email = formData.email;
      }
      if (formData.password && formData.password.trim() !== "") {
        payload.password = formData.password;
      }

      if (Object.keys(payload).length === 1) {
        console.log("Nenhuma alteração detectada. Nada a salvar.");
        navigate("/tecnicos");
        return;
      }

      await apiClient.patch(`/api/tecnicos/${userName}`, payload);

      navigate("/tecnicos", { state: { refresh: true } });
    } catch (err) {
      console.error("Erro ao salvar técnico:", err);
      return "Falha ao salvar. Verifique os dados e tente novamente.";
    }
  };

  if (loading) {
    return <div className="p-10 text-white">Carregando dados...</div>;
  }
  if (error) {
    return <div className="p-10 text-red-500">{error}</div>;
  }
  if (!tecnico) {
    return <div className="p-10 text-white">Técnico não encontrado.</div>;
  }

  return (
    <div className="p-10 text-white">
      <button
        onClick={() => navigate("/tecnicos", { state: { refresh: true } })}
        className="flex items-center space-x-2 bg-purple-500 hover:bg-purple-700 px-2 py-2 rounded-lg font-semibold transition-colors"
      >
        <ChevronLeft className="w-5 h-5" />
        <span>Voltar para Técnicos</span>
      </button>

      <div className="pt-4 border-t mb-1 mt-3 border-white/40"></div>

      <div className="bg-[#1E1E1E] p-6 rounded-lg shadow-lg border border-gray-800">
        <h1 className="text-3xl font-bold text-white mb-6">
          Editar Técnico: {tecnico.nome}
        </h1>
        <FormularioEditarTecnico initialData={tecnico} onSave={handleSave} />
      </div>
    </div>
  );
};

export default EditarTecnico;
