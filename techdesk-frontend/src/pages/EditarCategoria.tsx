import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { ChevronLeft } from "lucide-react";
import apiClient from "../api/apiClient";
import FormularioEditarCategoria from "../components/categorias/FormularioEditarCategoria";
import ListaSubcategorias from "../components/categorias/ListaSubcategorias";

const USE_MOCK_DATA_GET = false;

interface CategoriaApiData {
  id: number;
  nome: string;
  status: number | boolean;
  subCategorias: SubcategoriaApiData[];
}

export interface CategoriaViewData {
  id: number;
  nome: string;
  status: string;
  subCategorias: SubcategoriaApiData[];
}

export interface SubcategoriaApiData {
  id: number;
  nome: string;
  prioridade: "baixa" | "media" | "alta";
  sla: number;
  status: boolean | string;
  mesaAtendimentoId?: number;
}

const mockCategoriaApiData: CategoriaApiData = {
  id: 1,
  nome: "Servidor (Mock)",
  status: 1,
  subCategorias: [
    // @ts-ignore (Ignorando o 'status' faltante no mock)
    { id: 1, nome: "Offline", prioridade: "alta", sla: 3600 },
    // @ts-ignore
    { id: 2, nome: "Sem Sistema", prioridade: "alta", sla: 3600 },
    // @ts-ignore
    { id: 3, nome: "Levantamento Hardware", prioridade: "baixa", sla: 86400 },
  ],
};

const transformApiToView = (apiData: CategoriaApiData): CategoriaViewData => ({
  id: apiData.id,
  nome: apiData.nome,
  status: apiData.status ? "Ativo" : "Inativo",
  subCategorias: apiData.subCategorias,
});

const EditarCategoria: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const categoriaId = parseInt(id || "0");
  const navigate = useNavigate();

  const [categoria, setCategoria] = useState<CategoriaViewData | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchDados = async () => {
      setLoading(true);
      try {
        let dadosApi: CategoriaApiData;
        if (USE_MOCK_DATA_GET) {
          await new Promise((res) => setTimeout(res, 500));
          dadosApi = { ...mockCategoriaApiData, id: categoriaId };
        } else {
          const response = await apiClient.get<CategoriaApiData>(
            `/api/categorias/${id}`
          );
          dadosApi = response.data;
        }
        setCategoria(transformApiToView(dadosApi));
      } catch (error) {
        console.error("Erro ao buscar dados da categoria:", error);
      }
      setLoading(false);
    };
    fetchDados();
  }, [id, categoriaId]);

  const handleSubcategoriaAdicionada = (novaSubApi: SubcategoriaApiData) => {
    if (categoria) {
      const novaSubComStatus = {
        ...novaSubApi,
        status: true,
      };

      setCategoria({
        ...categoria,
        subCategorias: [...categoria.subCategorias, novaSubComStatus],
      });
    }
  };

  const handleDataChange = (novaData: CategoriaViewData) => {
    setCategoria(novaData);
  };

  const handleSubcategoriaStatusChange = (updatedSub: SubcategoriaApiData) => {
    if (categoria) {
      const updatedSubCategorias = categoria.subCategorias.map((sub) =>
        sub.id === updatedSub.id ? updatedSub : sub
      );
      setCategoria({ ...categoria, subCategorias: updatedSubCategorias });
    }
  };

  if (loading) {
    return <div className="p-10 text-white">Carregando...</div>;
  }

  if (!categoria) {
    return <div className="p-10 text-white">Categoria não encontrada.</div>;
  }

  return (
    <div className="p-10 text-white space-y-10">
      <button
        onClick={() => navigate("/categorias", { state: { refresh: true } })}
        className="flex items-center space-x-2 bg-purple-500 hover:bg-purple-700 px-2 py-2 rounded-lg font-semibold transition-colors"
      >
        <ChevronLeft className="w-5 h-5" />
        <span>Voltar</span>
      </button>
      <div className="pt-1 border-t mb-1 border-white/40"></div>

      <FormularioEditarCategoria
        initialData={categoria}
        onDataChange={handleDataChange}
      />

      <ListaSubcategorias
        categoriaId={categoria.id}
        subcategorias={categoria.subCategorias}
        onSubcategoriaAdicionada={handleSubcategoriaAdicionada}
        onSubcategoriaChange={handleSubcategoriaStatusChange}
      />
    </div>
  );
};

export default EditarCategoria;
