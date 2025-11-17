import React, { useState, useMemo, useEffect } from "react";
import Modal from "../ui/Modal";
import Input from "../ui/Input";
import Button from "../ui/Button";
import apiClient from "../../api/apiClient";
import {
  EmpresaParaDropdown,
  CategoriaParaDropdown,
  SubcategoriaParaDropdown,
  ChamadoApiResponse,
} from "../../types/chamado.types";
import { useAuth } from "../../contexts/AuthContext";

interface CadastroTicketModalProps {
  isOpen: boolean;
  onClose: () => void;
  onCadastroSuccess: (novoTicket: ChamadoApiResponse) => void;
}

const LabeledInput: React.FC<{ label: string; [key: string]: any }> = ({
  label,
  ...props
}) => (
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

const LabeledSelect: React.FC<{
  label: string;
  children: React.ReactNode;
  [key: string]: any;
}> = ({ label, children, ...props }) => (
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

const CadastroTicketModal: React.FC<CadastroTicketModalProps> = ({
  isOpen,
  onClose,
  onCadastroSuccess,
}) => {
  const [submitLoading, setSubmitLoading] = useState(false);
  const [erros, setErros] = useState<{ [key: string]: string }>({});

  const [assunto, setAssunto] = useState("");
  const [descricao, setDescricao] = useState("");
  const [empresaId, setEmpresaId] = useState<number | null>(null);
  const [categoriaId, setCategoriaId] = useState<number | null>(null);
  const [subcategoriaId, setSubcategoriaId] = useState<number | null>(null);

  const [empresas, setEmpresas] = useState<EmpresaParaDropdown[]>([]);
  const [categorias, setCategorias] = useState<CategoriaParaDropdown[]>([]);

  const { user } = useAuth();

  useEffect(() => {
    const fetchModalData = async () => {
      try {
        const [empResponse, catResponse] = await Promise.all([
          apiClient.get<EmpresaParaDropdown[]>("/api/empresas"),
          apiClient.get<CategoriaParaDropdown[]>("/api/categorias"),
        ]);
        setEmpresas(empResponse.data);
        setCategorias(catResponse.data);
      } catch (err) {
        console.error("Erro ao carregar dados do modal:", err);
        setErros((prev) => ({
          ...prev,
          geral: "Erro ao carregar dados. Feche e abra o modal.",
        }));
      }
    };

    fetchModalData();
  }, []);

  useEffect(() => {
    if (isOpen) {
      setAssunto("");
      setDescricao("");
      setEmpresaId(null);
      setCategoriaId(null);
      setSubcategoriaId(null);
      setErros({});
    }
  }, [isOpen]);

  const subcategoriasDisponiveis = useMemo((): SubcategoriaParaDropdown[] => {
    if (!categoriaId) return [];
    const categoria = categorias.find((c) => c.id === categoriaId);
    return categoria?.subCategorias ?? [];
  }, [categoriaId, categorias]);

  const isFormValido = useMemo(() => {
    if (!assunto.trim()) return false;
    if (!descricao.trim()) return false;
    if (!empresaId) return false;
    if (!categoriaId) return false;
    if (!subcategoriaId) return false;
    if (!user) return false;
    return true;
  }, [assunto, descricao, empresaId, categoriaId, subcategoriaId, user]);

  const validarFormulario = () => {
    const novosErros: { [key: string]: string } = {};
    if (!assunto.trim()) novosErros.assunto = "Assunto obrigatório";
    if (!descricao.trim()) novosErros.descricao = "Descrição obrigatória";
    if (!empresaId) novosErros.empresa = "Selecione empresa";
    if (!categoriaId) novosErros.categoria = "Selecione categoria";
    if (!subcategoriaId) novosErros.subcategoria = "Selecione subcategoria";
    if (!user) novosErros.geral = "Você não está autenticado.";

    setErros(novosErros);
    return Object.keys(novosErros).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitLoading(true);
    setErros({});

    if (!user) {
      setErros({ geral: "Usuário não autenticado." });
      setSubmitLoading(false);
      return;
    }

    const usuarioEmpresaId =
      user.empresaId || user.companyId || user.company?.id || 3;

    console.log("ID do usuário:", usuarioEmpresaId);

    if (!usuarioEmpresaId) {
      setErros({ geral: "Erro: ID do usuário não encontrado" });
      setSubmitLoading(false);
      return;
    }

    const novosErros: { [key: string]: string } = {};
    if (!assunto.trim()) novosErros.assunto = "Assunto obrigatório";
    if (!descricao.trim()) novosErros.descricao = "Descrição obrigatória";
    if (!empresaId) novosErros.empresa = "Selecione empresa";
    if (!categoriaId) novosErros.categoria = "Selecione categoria";
    if (!subcategoriaId) novosErros.subcategoria = "Selecione subcategoria";

    if (Object.keys(novosErros).length > 0) {
      setErros(novosErros);
      setSubmitLoading(false);
      return;
    }

    try {
      const payload = {
        assunto: assunto.trim(),
        descricao: descricao.trim(),
        empresaId: empresaId!,
        categoriaId: categoriaId!,
        subcategoriaId: subcategoriaId!,
        usuarioEmpresaId: usuarioEmpresaId,
        prioridadeNum: 2,
      };

      console.log("Enviando payload:", payload);

      const response = await apiClient.post("api/chamados", payload);
      onCadastroSuccess(response.data);

      setAssunto("");
      setDescricao("");
      setEmpresaId(null);
      setCategoriaId(null);
      setSubcategoriaId(null);

      onClose();
    } catch (error: any) {
      setErros({
        geral:
          error.response?.data?.message ||
          "Erro ao cadastrar ticket. Tente novamente.",
      });
    } finally {
      setSubmitLoading(false);
    }
  };

  if (!isOpen) return null;

  return (
    <Modal onClose={onClose} title="Novo Chamado" className="max-w-2xl">
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
          onChange={(e) => {
            setAssunto(e.target.value);
            setErros((prev) => ({ ...prev, assunto: "" }));
          }}
          placeholder="Descreva o problema"
          required
          disabled={submitLoading}
        />
        {erros.assunto && (
          <p className="text-red-400 text-xs ml-1">{erros.assunto}</p>
        )}

        <div className="space-y-1">
          <label className="block text-sm font-medium text-gray-300 mb-1">
            Descrição *
          </label>
          <textarea
            value={descricao}
            onChange={(e) => {
              setDescricao(e.target.value);
              setErros((prev) => ({ ...prev, descricao: "" }));
            }}
            placeholder="Detalhes do problema..."
            rows={3}
            className="w-full bg-[#606060] border border-[#CAC9CF] text-base text-white rounded-lg p-2.5"
            required
            disabled={submitLoading}
          />
        </div>
        {erros.descricao && (
          <p className="text-red-400 text-xs ml-1">{erros.descricao}</p>
        )}

        <LabeledSelect
          label="Empresa *"
          value={empresaId ?? ""}
          onChange={(e) => {
            const id = Number(e.target.value) || null;
            setEmpresaId(id);
            setErros((prev) => ({ ...prev, empresa: "" }));
          }}
          required
          disabled={submitLoading}
        >
          <option value="">Selecione empresa</option>
          {empresas.map((emp) => (
            <option key={emp.id} value={emp.id}>
              {emp.nome}
            </option>
          ))}
        </LabeledSelect>
        {erros.empresa && (
          <p className="text-red-400 text-xs ml-1">{erros.empresa}</p>
        )}

        <LabeledSelect
          label="Categoria *"
          value={categoriaId ?? ""}
          onChange={(e) => {
            const id = Number(e.target.value) || null;
            setCategoriaId(id);
            setSubcategoriaId(null);
            setErros((prev) => ({ ...prev, categoria: "" }));
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
        {erros.categoria && (
          <p className="text-red-400 text-xs ml-1">{erros.categoria}</p>
        )}

        <LabeledSelect
          label="Subcategoria *"
          value={subcategoriaId ?? ""}
          onChange={(e) => {
            const id = Number(e.target.value) || null;
            setSubcategoriaId(id);
            setErros((prev) => ({ ...prev, subcategoria: "" }));
          }}
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
        {erros.subcategoria && (
          <p className="text-red-400 text-xs ml-1">{erros.subcategoria}</p>
        )}

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
            {submitLoading ? "Criando..." : "Criar Chamado"}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default CadastroTicketModal;
