import React, { useState, useMemo, useEffect } from "react";
import Modal from "../ui/Modal";
import Input from "../ui/Input";
import Button from "../ui/Button";
import apiClient from "../../api/apiClient";
import {
  CategoriaParaDropdown,
  SubcategoriaParaDropdown,
} from "../../types/chamado.types";

interface CadastroModalProps {
  onClose: () => void;
  onCadastroSuccess: (novoTicket: any) => void;
}

interface LabeledInputProps {
  label: string;
  [key: string]: any;
}
interface LabeledSelectProps {
  label: string;
  children: React.ReactNode;
  [key: string]: any;
}

const LabeledInput: React.FC<LabeledInputProps> = ({ label, ...props }) => (
  <div className="space-y-1">
    <label className="block text-sm font-medium text-gray-300 mb-1">
      {label}
    </label>
    <Input
      {...props}
      className="w-full bg-[#606060] border border-[#CAC9CF] text-base text-white rounded-lg p-2.5"
    />
  </div>
);

const LabeledSelect: React.FC<LabeledSelectProps> = ({
  label,
  children,
  ...props
}) => (
  <div className="space-y-1">
    <label className="block text-sm font-medium text-gray-300 mb-1">
      {label}
    </label>
    <select
      {...props}
      className="w-full bg-[#606060] border border-[#CAC9CF] text-base text-white rounded-lg p-2.5"
    >
      {children}
    </select>
  </div>
);

const CadastroTicketClienteModal: React.FC<CadastroModalProps> = ({
  onClose,
  onCadastroSuccess,
}) => {
  const [submitLoading, setSubmitLoading] = useState(false);
  const [erros, setErros] = useState<{ [key: string]: string }>({});

  const [assunto, setAssunto] = useState("");
  const [descricao, setDescricao] = useState("");
  const [categoriaId, setCategoriaId] = useState<number | null>(null);
  const [subcategoriaId, setSubcategoriaId] = useState<number | null>(null);

  const [categorias, setCategorias] = useState<CategoriaParaDropdown[]>([]);

  useEffect(() => {
    const fetchModalData = async () => {
      try {
        const catResponse = await apiClient.get<CategoriaParaDropdown[]>(
          "/api/categorias"
        );
        setCategorias(catResponse.data);
      } catch (err) {
        console.error("Erro fatal ao carregar dados do modal:", err);
        setErros((prev) => ({
          ...prev,
          geral: "Erro ao carregar dados. Feche e abra o modal.",
        }));
      }
    };
    fetchModalData();
  }, []);

  useEffect(() => {
    setAssunto("");
    setDescricao("");
    setCategoriaId(null);
    setSubcategoriaId(null);
    setErros({});
  }, []);

  const subcategoriasDisponiveis = useMemo((): SubcategoriaParaDropdown[] => {
    if (!categoriaId) return [];
    const categoria = categorias.find((c) => c.id === categoriaId);

    return (
      (categoria as any)?.subCategorias ||
      (categoria as any)?.subcategorias ||
      []
    );
  }, [categoriaId, categorias]);

  const isFormValido = useMemo(() => {
    if (!assunto.trim()) return false;
    if (!descricao.trim()) return false;
    if (!categoriaId) return false;
    if (!subcategoriaId) return false;
    return true;
  }, [assunto, descricao, categoriaId, subcategoriaId]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!isFormValido) {
      setErros({ geral: "Preencha todos os campos obrigatórios." });
      return;
    }
    setSubmitLoading(true);
    setErros({});

    try {
      const payload = {
        assunto: assunto.trim(),
        descricao: descricao.trim(),
        categoriaId: Number(categoriaId),
        subCategoriaId: Number(subcategoriaId),
      };

      console.log("Enviando (Painel Cliente):", payload);

      await apiClient.post("/api/chamados/painelChamados", payload);

      onCadastroSuccess(payload);
      onClose();
    } catch (error: any) {
      console.error("Erro:", error);
      if (error.response?.data) {
        setErros({ geral: error.response.data });
      } else {
        setErros({ geral: "Erro ao criar chamado. Verifique os dados." });
      }
    } finally {
      setSubmitLoading(false);
    }
  };

  return (
    <Modal onClose={onClose} title="Abrir Novo Chamado" className="max-w-2xl">
      <form onSubmit={handleSubmit} className="space-y-4">
        {erros.geral && (
          <div className="p-3 bg-red-900/20 border border-red-500/30 rounded text-red-100 text-sm">
            {erros.geral}
          </div>
        )}

        <LabeledInput
          label="Assunto *"
          type="text"
          value={assunto}
          onChange={(e) => setAssunto(e.target.value)}
          placeholder="Descreva o problema"
          required
          disabled={submitLoading}
        />

        <div className="space-y-1">
          <label className="block text-sm font-medium text-gray-300 mb-1">
            Descrição *
          </label>
          <textarea
            value={descricao}
            onChange={(e) => setDescricao(e.target.value)}
            placeholder="Detalhes do problema..."
            rows={3}
            className="w-full bg-[#606060] border border-[#CAC9CF] text-base text-white rounded-lg p-2.5"
            required
            disabled={submitLoading}
          />
        </div>

        <LabeledSelect
          label="Categoria *"
          value={categoriaId ?? ""}
          onChange={(e) => {
            const id = Number(e.target.value) || null;
            setCategoriaId(id);
            setSubcategoriaId(null);
          }}
          required
          disabled={submitLoading}
        >
          <option value="">Selecione categoria</option>
          {categorias.map((cat) => (
            <option key={cat.id} value={cat.id}>
              {cat.nome}
            </option>
          ))}
        </LabeledSelect>

        <LabeledSelect
          label="Subcategoria *"
          value={subcategoriaId ?? ""}
          onChange={(e) => setSubcategoriaId(Number(e.target.value) || null)}
          disabled={
            submitLoading ||
            !categoriaId ||
            subcategoriasDisponiveis.length === 0
          }
          required
        >
          <option value="" disabled>
            {categoriaId
              ? subcategoriasDisponiveis.length > 0
                ? "Selecione"
                : "Sem opções"
              : "Selecione categoria primeiro"}
          </option>
          {subcategoriasDisponiveis.map((sub) => (
            <option key={sub.id} value={sub.id}>
              {sub.nome}
            </option>
          ))}
        </LabeledSelect>

        <div className="flex justify-end space-x-3 pt-4 border-t border-gray-600">
          <Button
            variant="secondary"
            type="button"
            onClick={onClose}
            disabled={submitLoading}
            className="bg-gray-700 text-white px-4 py-2 rounded"
          >
            Cancelar
          </Button>
          <Button
            type="submit"
            disabled={submitLoading || !isFormValido}
            className="bg-purple-600 text-white px-6 py-2 rounded"
          >
            {submitLoading ? "Enviando..." : "Abrir Chamado"}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default CadastroTicketClienteModal;
