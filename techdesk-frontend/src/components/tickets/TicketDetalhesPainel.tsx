import React, { useState, useEffect, useMemo } from "react";
import apiClient from "../../api/apiClient";
import Button from "../ui/Button";
import { Loader2 } from "lucide-react";

interface Categoria {
  id: number;
  nome: string;
  subCategorias: SubCategoria[];
}
interface SubCategoria {
  id: number;
  nome: string;
}
interface Mesa {
  id: number;
  nome: string;
}
interface Tecnico {
  userName: string;
  nome: string;
}

export interface FullTicketData {
  numeroChamado: string;
  categoriaId: number;
  subCategoriaId: number;
  mesaAtendimentoId: number;
  tecnicoResponsavelUserName: string;
}

interface DetalhesProps {
  ticket: FullTicketData;
  onUpdate: (newData: Partial<FullTicketData>) => void;
}

const LabeledSelect: React.FC<any> = ({ label, children, ...props }) => (
  <div className="space-y-1">
    <label className="block text-sm font-medium text-gray-400">{label}</label>
    <select
      {...props}
      className="w-full bg-[#3B3B3B] border border-gray-600 rounded-lg p-2.5"
    >
      {children}
    </select>
  </div>
);

const TicketDetalhesPainel: React.FC<DetalhesProps> = ({
  ticket,
  onUpdate,
}) => {
  const [formData, setFormData] = useState(ticket);
  const [categorias, setCategorias] = useState<Categoria[]>([]);
  const [mesas, setMesas] = useState<Mesa[]>([]);
  const [tecnicos, setTecnicos] = useState<Tecnico[]>([]);

  const [isLoading, setIsLoading] = useState(false);
  const [isDataLoading, setIsDataLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  useEffect(() => {
    setIsDataLoading(true);
    const fetchData = async () => {
      try {
        const [catRes, mesaRes, tecRes] = await Promise.all([
          apiClient.get<Categoria[]>("/api/categorias"),
          apiClient.get<Mesa[]>("/api/mesasatendimento"),
          apiClient.get<Tecnico[]>("/api/tecnicos"),
        ]);
        setCategorias(catRes.data);
        setMesas(mesaRes.data);
        setTecnicos(tecRes.data);
      } catch (err) {
        console.error("Erro ao buscar dados de suporte:", err);
        setError("Falha ao carregar opções.");
      } finally {
        setIsDataLoading(false);
      }
    };
    fetchData();
  }, []);

  useEffect(() => {
    setFormData(ticket);
  }, [ticket]);

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const { name, value } = e.target;
    const newValue = name.includes("Id") ? Number(value) : value;

    setFormData((prev) => ({ ...prev, [name]: newValue }));

    if (name === "categoriaId") {
      const categoriaSelecionada = categorias.find((c) => c.id === newValue);
      const primeiraSub = categoriaSelecionada?.subCategorias[0]?.id || null;
      setFormData((prev) => ({
        ...prev,
        categoriaId: newValue,
        subCategoriaId: primeiraSub,
      }));
    }
  };

  const handleSave = async () => {
    setIsLoading(true);
    setError(null);
    setSuccessMessage(null);

    const payload: any = { numeroChamado: ticket.numeroChamado };

    if (
      formData.categoriaId !== ticket.categoriaId ||
      formData.subCategoriaId !== ticket.subCategoriaId
    ) {
      payload.categoriaId = formData.categoriaId;
      payload.subCategoriaId = formData.subCategoriaId;
    }
    if (formData.mesaAtendimentoId !== ticket.mesaAtendimentoId) {
      payload.mesaAtendimentoId = formData.mesaAtendimentoId;
    }
    if (
      formData.tecnicoResponsavelUserName !== ticket.tecnicoResponsavelUserName
    ) {
      payload.tecnicoResponsavelUserName = formData.tecnicoResponsavelUserName;
    }
    if (Object.keys(payload).length === 1) {
      setIsLoading(false);
      return;
    }

    try {
      await apiClient.patch(
        `/api/chamados/alterar/dados/${ticket.numeroChamado}`,
        payload,
        { headers: { "Content-Type": "application/json" } }
      );

      onUpdate(payload);

      setSuccessMessage("Alterações salvas com sucesso!");

      setTimeout(() => setSuccessMessage(null), 3000);
    } catch (err: any) {
      console.error("Erro ao salvar detalhes:", err);

      let errorMessage = "Falha ao salvar.";

      if (err.response && err.response.data) {
        const backendMessage =
          err.response.data.message ||
          err.response.data.title ||
          err.response.data.error;

        if (typeof backendMessage === "string") {
          errorMessage = backendMessage;
        }
      }

      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  const subcategoriasAtuais = useMemo(() => {
    return (
      categorias.find((c) => c.id === formData.categoriaId)?.subCategorias || []
    );
  }, [formData.categoriaId, categorias]);

  if (isDataLoading) {
    return (
      <div className="flex justify-center items-center h-full">
        <Loader2 className="w-8 h-8 animate-spin text-purple-500" />
      </div>
    );
  }

  return (
    <div className="bg-[#1E1E1E] p-4 rounded-lg space-y-4">
      <h3 className="text-lg font-semibold border-b border-gray-600 pb-2">
        Detalhes
      </h3>

      {error && <p className="text-red-400 text-sm">{error}</p>}

      {successMessage && (
        <p className="text-green-400 text-sm">{successMessage}</p>
      )}

      <LabeledSelect
        label="Categoria"
        name="categoriaId"
        value={formData.categoriaId}
        onChange={handleChange}
      >
        {categorias.map((c) => (
          <option key={c.id} value={c.id}>
            {c.nome}
          </option>
        ))}
      </LabeledSelect>

      <LabeledSelect
        label="Subcategoria"
        name="subCategoriaId"
        value={formData.subCategoriaId}
        onChange={handleChange}
        disabled={subcategoriasAtuais.length === 0}
      >
        {subcategoriasAtuais.map((s) => (
          <option key={s.id} value={s.id}>
            {s.nome}
          </option>
        ))}
      </LabeledSelect>

      <LabeledSelect
        label="Mesa de Atendimento"
        name="mesaAtendimentoId"
        value={formData.mesaAtendimentoId}
        onChange={handleChange}
      >
        {mesas.map((m) => (
          <option key={m.id} value={m.id}>
            {m.nome}
          </option>
        ))}
      </LabeledSelect>

      <LabeledSelect
        label="Técnico Responsável"
        name="tecnicoResponsavelUserName"
        value={formData.tecnicoResponsavelUserName || ""}
        onChange={handleChange}
      >
        <option value="">Não atribuído</option>
        {tecnicos.map((t) => (
          <option key={t.userName} value={t.userName}>
            {t.nome}
          </option>
        ))}
      </LabeledSelect>

      <Button
        onClick={handleSave}
        isLoading={isLoading}
        disabled={
          JSON.stringify(formData) === JSON.stringify(ticket) ||
          !!successMessage
        } // Desabilita enquanto a msg de sucesso está visível
        className="bg-green-600 hover:bg-green-700"
      >
        Salvar Alterações
      </Button>
    </div>
  );
};

export default TicketDetalhesPainel;
