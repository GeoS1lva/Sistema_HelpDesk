import React, { useState, useEffect, useMemo, useCallback } from "react";
import { Plus, Search, ChevronDown, RefreshCw } from "lucide-react";
import apiClient from "../api/apiClient";
import {
  ChamadoApiResponse,
  ChamadoDashboard,
  ChamadoView,
  transformDashboardToView,
  transformPostToView,
} from "../types/chamado.types";
import KanbanColumn from "../components/tickets/KanbanColumn";
import CadastroTicketModal from "../components/tickets/CadastroTicketModal";
import AtendimentoTicketModal, {
  SystemEvent,
  ChatHistoryEvent,
} from "../components/tickets/AtendimentoTicketModal";

const USE_MOCK_DATA_GET = false;

const mockTickets: ChamadoView[] = [
  {
    id: "CH2025-001",
    assunto: "Servidor sem ligar",
    prioridade: "Alta",
    slaStatus: "Vencido",
    clienteNome: "Empresa A (Mock)",
    mesaNome: "Infra N2",
    tecnicoNome: "Geovana Silva",
    status: "Aberto",
  },
  {
    id: "CH2025-002",
    assunto: "Mouse quebrado",
    prioridade: "Baixa",
    slaStatus: "No Prazo",
    clienteNome: "Empresa B (Mock)",
    mesaNome: "Suporte N1",
    tecnicoNome: "Alan M.",
    status: "Em Atendimento",
  },
  {
    id: "CH2025-003",
    assunto: "Aguardando Peça",
    prioridade: "Média",
    slaStatus: "No Prazo",
    clienteNome: "Empresa C (Mock)",
    mesaNome: "Suporte N1",
    tecnicoNome: "Alan M.",
    status: "Pausado",
  },
];
interface GroupedTickets {
  abertos: ChamadoView[];
  emAtendimento: ChamadoView[];
  pausados: ChamadoView[];
  concluidos: ChamadoView[];
}

interface AtendimentoModalState {
  isOpen: boolean;
  ticket: ChamadoView | null;
}

const Tickets: React.FC = () => {
  const [tickets, setTickets] = useState<ChamadoView[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isCadastroModalOpen, setIsCadastroModalOpen] = useState(false);
  const [atendimentoModal, setAtendimentoModal] =
    useState<AtendimentoModalState>({
      isOpen: false,
      ticket: null,
    });

  const [historyCache, setHistoryCache] = useState<
    Record<string, ChatHistoryEvent[]>
  >({});
  const [activeTicketHistory, setActiveTicketHistory] = useState<
    ChatHistoryEvent[]
  >([]);
  const [isLoadingHistory, setIsLoadingHistory] = useState(false);
  const [lastBackendSync, setLastBackendSync] = useState<Date | null>(null);

  const fetchTickets = useCallback(async (forceSync = false) => {
    setLoading(forceSync);
    setError(null);
    try {
      let data: ChamadoView[];
      if (USE_MOCK_DATA_GET) {
        data = mockTickets;
      } else {
        const response = await apiClient.get<ChamadoDashboard[]>(
          "/api/chamados/dashboard"
        );
        data = response.data.map(transformDashboardToView);
      }
      setTickets(data);
      if (forceSync) {
        setLastBackendSync(new Date());
      }
    } catch (err) {
      setError("Falha ao carregar os chamados.");
      console.error("Erro no fetchTickets:", err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchTickets(true);
  }, [fetchTickets]);

  const groupedTickets = useMemo((): GroupedTickets => {
    return tickets.reduce(
      (acc, ticket) => {
        if (ticket.status === "Aberto") acc.abertos.push(ticket);
        else if (ticket.status === "Em Atendimento")
          acc.emAtendimento.push(ticket);
        else if (ticket.status === "Pausado") acc.pausados.push(ticket);
        else if (ticket.status === "Concluído") acc.concluidos.push(ticket);
        return acc;
      },
      {
        abertos: [],
        emAtendimento: [],
        pausados: [],
        concluidos: [],
      } as GroupedTickets
    );
  }, [tickets]);

  const handleCadastroSuccess = (novoChamadoApi: ChamadoApiResponse) => {
    const novoTicketView = transformPostToView(novoChamadoApi);
    setTickets((prev) => [...prev, novoTicketView]);
    setIsCadastroModalOpen(false);
  };

  const updateTicketInList = (ticketAtualizado: ChamadoApiResponse) => {
    setTickets((prevTickets) =>
      prevTickets.map((t) => {
        if (t.id !== ticketAtualizado.id) {
          return t;
        }

        return {
          ...t,

          status: ticketAtualizado.status,
          tecnicoNome: ticketAtualizado.tecnicoNome,
        };
      })
    );
    console.log("Ticket atualizado localmente:", ticketAtualizado);
  };

  const handleTicketAction = async (
    numeroChamado: string,
    acao: "play" | "pause" | "atribuir" | "view"
  ) => {
    const ticketParaAtender = tickets.find((t) => t.id === numeroChamado);
    if (!ticketParaAtender) {
      console.error("Ticket não encontrado:", numeroChamado);
      return;
    }

    setAtendimentoModal({ isOpen: true, ticket: ticketParaAtender });
    setIsLoadingHistory(true);

    if (historyCache[numeroChamado]) {
      setActiveTicketHistory(historyCache[numeroChamado]);
    } else {
      try {
        setActiveTicketHistory([]);
        setHistoryCache((prevCache) => ({ ...prevCache, [numeroChamado]: [] }));
      } catch (err) {
        console.error("Erro ao buscar histórico", err);
        setActiveTicketHistory([]);
      }
    }
    setIsLoadingHistory(false);
  };

  const handleAddHistoryEvent = (ticketId: string, event: SystemEvent) => {
    const currentHistory = historyCache[ticketId] || [];
    const newHistory = [...currentHistory, event];

    setHistoryCache((prevCache) => ({ ...prevCache, [ticketId]: newHistory }));
    setActiveTicketHistory(newHistory);
  };

  if (loading && lastBackendSync === null) {
    return <p className="p-10 text-white">Carregando chamados...</p>;
  }
  if (error) {
    return <p className="p-10 text-red-500">{error}</p>;
  }

  return (
    <div className="p-10 text-white h-full flex flex-col">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Tickets</h1>
        <button
          onClick={() => setIsCadastroModalOpen(true)}
          disabled={loading}
          className="flex items-center space-x-2 bg-purple-600 hover:bg-purple-700 px-4 py-2 rounded-lg font-semibold transition-colors"
        >
          <Plus className="w-4 h-4" />
          <span>Cadastrar Ticket</span>
        </button>
      </div>

      <div className="flex items-center space-x-4 mb-6">
        <div className="relative flex-grow max-w-xs">
          <input
            type="text"
            placeholder="Pesquisar..."
            className="w-full bg-[#262626] border border-gray-700 rounded-lg py-2 pl-10 pr-4 focus:outline-none focus:border-purple-500"
          />
          <Search className="w-5 h-5 text-gray-400 absolute left-3 top-1/2 -translate-y-1/2" />
        </div>
        <div className="flex items-center space-x-2 p-2 bg-[#262626] rounded-lg border border-gray-700">
          <span className="text-gray-400 text-sm">Minha Fila</span>
          <ChevronDown className="w-4 h-4 text-purple-500" />
        </div>
        <div className="flex items-center space-x-2 p-2 bg-[#262626] rounded-lg border border-gray-700">
          <span className="text-gray-400 text-sm">Não Atribuídos</span>
          <ChevronDown className="w-4 h-4 text-purple-500" />
        </div>
      </div>

      <div className="flex-1 flex flex-col lg:flex-row space-y-4 lg:space-y-0 lg:space-x-4 overflow-x-auto pb-4">
        <KanbanColumn
          title="Abertos"
          tickets={groupedTickets.abertos}
          onUpdateTicket={handleTicketAction}
        />
        <KanbanColumn
          title="Em Atendimento"
          tickets={groupedTickets.emAtendimento}
          onUpdateTicket={handleTicketAction}
        />
        <KanbanColumn
          title="Pausados"
          tickets={groupedTickets.pausados}
          onUpdateTicket={handleTicketAction}
        />
        <KanbanColumn
          title="Concluídos"
          tickets={groupedTickets.concluidos}
          onUpdateTicket={handleTicketAction}
        />
      </div>

      <CadastroTicketModal
        isOpen={isCadastroModalOpen}
        onClose={() => setIsCadastroModalOpen(false)}
        onCadastroSuccess={handleCadastroSuccess}
      />

      {atendimentoModal.isOpen && atendimentoModal.ticket && (
        <AtendimentoTicketModal
          ticket={atendimentoModal.ticket}
          history={activeTicketHistory}
          isLoadingHistory={isLoadingHistory}
          onAddHistoryEvent={handleAddHistoryEvent}
          onClose={() => setAtendimentoModal({ isOpen: false, ticket: null })}
          onUpdateTicketInList={updateTicketInList}
        />
      )}
    </div>
  );
};

export default Tickets;
