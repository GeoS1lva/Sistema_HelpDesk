import React, { useState, useEffect } from "react";
import Modal from "../ui/Modal";
import Input from "../ui/Input";
import Button from "../ui/Button";
import apiClient from "../../api/apiClient";
import { AxiosError } from "axios";

const USE_MOCK_DATA = false;

interface MesaData {
  id: number;
  nome: string;
}

interface TecnicoApiData {
  id: number;
  nome: string;
  userName: string;
  email: string;
  tipoPerfil: string;
}
interface CadastroTecnicoModalProps {
  onClose: () => void;
  onSaveSuccess: (novoTecnico: TecnicoApiData) => void;
}

const mockMesas: MesaData[] = [
  { id: 1, nome: "Nível 1 - Suporte Geral" },
  { id: 2, nome: "Nível 2 - Infraestrutura" },
  { id: 3, nome: "Nível 3 - Sistemas" },
];

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

const CadastroTecnicoModal: React.FC<CadastroTecnicoModalProps> = ({
  onClose,
  onSaveSuccess,
}) => {
  const [nome, setNome] = useState("");
  const [userName, setUserName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [tipoPerfil, setTipoPerfil] = useState("tecnico");

  const [mesasDisponiveis, setMesasDisponiveis] = useState<MesaData[]>([]);
  const [mesasSelecionadas, setMesasSelecionadas] = useState<number[]>([]);
  const [loadingMesas, setLoadingMesas] = useState(true);

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchMesas = async () => {
      setLoadingMesas(true);
      try {
        if (USE_MOCK_DATA) {
          setMesasDisponiveis(mockMesas);
        } else {
          const response = await apiClient.get<MesaData[]>(
            "/api/mesasatendimento"
          );
          setMesasDisponiveis(response.data);
        }
      } catch (err) {
        console.error("Erro ao buscar mesas:", err);
        setError("Não foi possível carregar as mesas de atendimento.");
      }
      setLoadingMesas(false);
    };
    fetchMesas();
  }, []);

  const handleMesaToggle = (mesaId: number) => {
    setMesasSelecionadas((prev) =>
      prev.includes(mesaId)
        ? prev.filter((id) => id !== mesaId)
        : [...prev, mesaId]
    );
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError(null);

    const payload = {
      nome: nome,
      userName: userName,
      email: email,
      password: password,
      tipoPerfil: tipoPerfil,
      mesasAtendimentoId: mesasSelecionadas,
    };

    try {
      let novoTecnico: TecnicoApiData;
      if (USE_MOCK_DATA) {
        console.log("Modo Mock: Cadastrando Técnico", payload);
        await new Promise((res) => setTimeout(res, 1000));
        novoTecnico = {
          id: Math.floor(Math.random() * 1000),
          ...payload,
        };
      } else {
        const response = await apiClient.post<TecnicoApiData>(
          "/api/tecnicos",
          payload
        );
        novoTecnico = response.data;
      }
      onSaveSuccess(novoTecnico);
      onClose();
    } catch (err) {
      const error = err as AxiosError<any>;
      console.error("Erro ao cadastrar técnico:", error.response);

      if (error.response && error.response.status === 400) {
        if (typeof error.response.data === "string") {
          setError(error.response.data);
        } else if (error.response.data && error.response.data.errors) {
          const validationErrors = error.response.data.errors as {
            [key: string]: string[];
          };
          const errorMessages = Object.keys(validationErrors)
            .map((field) => validationErrors[field].join(" "))
            .join(" | ");
          setError(errorMessages);
        } else {
          setError("Erro de validação desconhecido.");
        }
      } else {
        setError("Falha ao cadastrar. Tente novamente.");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal title="Cadastrar Técnico" onClose={onClose}>
      <form onSubmit={handleSubmit} className="space-y-4">
        <LabeledInput
          label="Nome Completo"
          name="nome"
          type="text"
          value={nome}
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            setNome(e.target.value)
          }
          className="bg-[#606060] h-[42px] border border-[#CAC9CF] text-base"
        />

        <div className="flex flex-col md:flex-row gap-4">
          <LabeledInput
            label="Usuário (Login)"
            name="userName"
            type="text"
            value={userName}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setUserName(e.target.value)
            }
            className="bg-[#606060] h-[42px] border border-[#CAC9CF] text-base"
          />
          <LabeledInput
            label="E-mail"
            name="email"
            type="email"
            value={email}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setEmail(e.target.value)
            }
            className="bg-[#606060] h-[42px] border border-[#CAC9CF] text-base"
          />
        </div>

        <div className="flex flex-col md:flex-row gap-4">
          <LabeledInput
            label="Senha"
            name="password"
            type="password"
            value={password}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
              setPassword(e.target.value)
            }
            className="bg-[#606060] h-[42px] border border-[#CAC9CF] text-base"
          />
          <div className="flex-1">
            <label className="block text-sm font-medium text-gray-300 mb-1">
              Tipo de Perfil
            </label>
            <select
              value={tipoPerfil}
              onChange={(e) => setTipoPerfil(e.target.value)}
              className="bg-[#606060] border border-[#CAC9CF] text-white rounded-lg p-2 focus:outline-none focus:border-purple-500 w-full h-[42px] text-base"
            >
              <option value="tecnico">Técnico</option>
              <option value="administrador">Administrador</option>
            </select>
          </div>
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-300 mb-2">
            Mesas de Atendimento
          </label>
          {loadingMesas ? (
            <p className="text-gray-400">Carregando mesas...</p>
          ) : (
            <div className="max-h-32 overflow-y-auto bg-[#3B3B3B] p-2 rounded-lg border border-gray-600">
              {mesasDisponiveis.map((mesa) => (
                <div key={mesa.id} className="flex items-center gap-2">
                  <input
                    type="checkbox"
                    id={`mesa-${mesa.id}`}
                    checked={mesasSelecionadas.includes(mesa.id)}
                    onChange={() => handleMesaToggle(mesa.id)}
                    className="form-checkbox h-4 w-4 text-purple-600 bg-gray-700 border-gray-500 rounded focus:ring-purple-500"
                  />
                  <label htmlFor={`mesa-${mesa.id}`}>{mesa.nome}</label>
                </div>
              ))}
            </div>
          )}
        </div>

        {error && (
          <p className="text-red-400 text-sm text-center bg-red-900/20 p-2 rounded-lg">
            {error}
          </p>
        )}

        <div className="flex justify-end gap-4 pt-4">
          <Button variant="secondary" type="button" onClick={onClose}>
            Cancelar
          </Button>
          <Button type="submit" disabled={isLoading}>
            {isLoading ? "Cadastrando..." : "Cadastrar"}
          </Button>
        </div>
      </form>
    </Modal>
  );
};

export default CadastroTecnicoModal;
