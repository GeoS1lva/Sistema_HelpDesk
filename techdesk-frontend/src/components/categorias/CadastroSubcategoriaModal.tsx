import React, { useState } from "react";
import Modal from "../ui/Modal";
import Input from "../ui/Input";
import Button from "../ui/Button";
import apiClient from "../../api/apiClient";
import { AxiosError } from "axios";
import { SubcategoriaApiData } from "../../pages/EditarCategoria"; 
import { useEffect } from "react";

const USE_MOCK_DATA_POST = false;

interface MesaParaDropdown {
  id: number;
  nome: string;
}

interface CadastroModalProps {
  categoriaId: number;
  onClose: () => void;
  onSaveSuccess: (novaSub: SubcategoriaApiData) => void;
}

const LabeledInput: React.FC<{ label: string; [key: string]: any }> = ({
  label,
  ...props
}) => (
  <div className="flex-1">
    <label className="block text-sm font-medium text-gray-300 mb-1">
      {label}
    </label>
    <Input {...props} />
  </div>
);

const LabeledSelect: React.FC<{
  label: string;
  children: React.ReactNode;
  [key: string]: any;
}> = ({ label, children, ...props }) => (
  <div className="flex-1">
    <label className="block text-sm font-medium text-gray-300 mb-1">
      {label}
    </label>
    <select
      {...props}
      className="bg-[#606060] border border-[#CAC9CF] text-white rounded-lg p-2 focus:outline-none focus:border-purple-500 w-full h-[42px]"
    >
      {children}
    </select>
  </div>
);

const CadastroSubcategoriaModal: React.FC<CadastroModalProps> = ({
  categoriaId,
  onClose,
  onSaveSuccess,
}) => {
  const [nome, setNome] = useState("");
  const [prioridade, setPrioridade] = useState<"baixa" | "media" | "alta">("baixa");
  const [slaHoras, setSlaHoras] = useState(0);

  const [mesas, setMesas] = useState<MesaParaDropdown[]>([]);
  const [mesaId, setMesaId] = useState<number | null>(null);

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchMesas = async () => {
      try {
        const response = await apiClient.get<MesaParaDropdown[]>(
          "/api/mesasatendimento" 
        );
        setMesas(response.data);
      } catch (err) {
        console.error("Erro ao buscar mesas:", err);
        setError("Não foi possível carregar as mesas de atendimento.");
      }
    };
    fetchMesas();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!mesaId) {
      setError("Por favor, selecione uma mesa de atendimento.");
      return;
    }

    setIsLoading(true);
    setError(null);

    const payload = [
      {
        nome: nome,
        prioridade: prioridade,
        sla: slaHoras * 3600,
        mesaAtendimentoId: mesaId,
      },
    ];

    try {
      let novaSubcategoria: SubcategoriaApiData;
      
      if (USE_MOCK_DATA_POST) {
        console.log("Modo Mock: Cadastrando Subcategoria", payload);
        await new Promise((res) => setTimeout(res, 1000));
        novaSubcategoria = {
          id: Math.floor(Math.random() * 1000),
          ...payload[0],
        };
      } else {
        const response = await apiClient.post<SubcategoriaApiData[]>(
          `/api/categorias/subCategoria?categoriaId=${categoriaId}`,
          payload
        );
        novaSubcategoria = response.data[0]; 
      }
      
      onSaveSuccess(novaSubcategoria);
      onClose();

    } catch (err) {
      const error = err as AxiosError<any>;
      console.error("Erro ao cadastrar subcategoria:", error.response);
      if (error.response?.data) {
        setError(error.response.data);
      } else {
        setError("Falha ao cadastrar. Tente novamente.");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal title="Cadastrar Subcategoria" onClose={onClose}>
      <form onSubmit={handleSubmit} className="space-y-4">
        
        <LabeledInput
          label="Nome da Subcategoria"
          type="text"
          value={nome}
          onChange={(e: React.ChangeEvent<HTMLInputElement>) => setNome(e.target.value)}
          placeholder="Ex: Mouse, Teclado, Monitor"
          className="bg-[#606060] h-[42px] border border-[#CAC9CF] text-base"
        />

        <LabeledSelect
          label="Mesa de Atendimento Padrão"
          value={mesaId || ""}
          onChange={(e: React.ChangeEvent<HTMLSelectElement>) =>
            setMesaId(Number(e.target.value))
          }
          required
        >
          <option value="" disabled>
            Selecione uma mesa
          </option>
          {mesas.map((mesa) => (
            <option key={mesa.id} value={mesa.id}>
              {mesa.nome}
            </option>
          ))}
        </LabeledSelect>

        <div className="flex flex-col md:flex-row gap-4">
          <LabeledSelect
            label="Prioridade"
            value={prioridade}
            onChange={(e: React.ChangeEvent<HTMLSelectElement>) =>
              setPrioridade(e.target.value as any)
            }
          >
            <option value="baixa">Baixa</option>
            <option value="media">Média</option>
            <option value="alta">Alta</option>
          </LabeledSelect>

          <LabeledInput
            label="SLA (em horas)"
            type="number"
            value={slaHoras}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setSlaHoras(Number(e.target.value))
            }
            className="bg-[#606060] h-[42px] border border-[#CAC9CF] text-base"
            min="0"
          />
        </div>

        {error && (
          <p className="text-red-400 text-sm text-center bg-red-900/20 p-2 rounded-lg">
            {error}
          </p>
        )}

        <div className="flex justify-end gap-4 pt-4">
          <Button 
          variant="secondary"
          type="button" onClick={onClose}  >
            Cancelar
          </Button>
          <Button type="submit" disabled={isLoading} >
            {isLoading ? "Cadastrando..." : "Cadastrar"}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default CadastroSubcategoriaModal;