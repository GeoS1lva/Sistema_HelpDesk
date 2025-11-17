import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { ChevronLeft, Loader2, User, Send } from "lucide-react";
import apiClient from "../api/apiClient";
import TicketDetalhesPainel, {
  FullTicketData,
} from "../components/tickets/TicketDetalhesPainel";
import Button from "../components/ui/Button";

interface SystemEvent {
  type: "system";
  id: string | number;
  nomeTecnico: string;
  horaInicioAtendimento: Date;
  mensagemInicioAtendimento: string;
}

interface CommentEvent {
  type: "comment";
  id: string | number;
  tecnicoNome: string;
  horaComentario: Date;
  comentario: string;
}

type ChatHistoryEvent = SystemEvent | CommentEvent;

interface ComentarioApiResponse {
  id: number;
  usuarioNome: string;
  horaAtendimento: string;
  mensagem: string;
}

const SystemEventBubble: React.FC<{ event: SystemEvent }> = ({ event }) => (
  <div className="text-center my-4">
    <span className="text-xs text-gray-400 bg-gray-700 px-3 py-1 rounded-full">
      {event.nomeTecnico} | {event.mensagemInicioAtendimento} |{" "}
      {event.horaInicioAtendimento.toLocaleTimeString("pt-BR", {
        hour: "2-digit",
        minute: "2-digit",
      })}
    </span>
  </div>
);

const CommentEventBubble: React.FC<{ event: CommentEvent }> = ({ event }) => (
  <div className="flex items-start space-x-3 my-4">
    <div className="flex-shrink-0 w-10 h-10 rounded-full bg-gray-700 flex items-center justify-center">
      <User className="w-5 h-5 text-gray-400" />
    </div>
    <div className="flex-1 bg-gray-800/50 rounded-lg p-4 border-l-4 border-purple-500 shadow-md">
      <div className="flex justify-between items-center mb-1">
        <span className="text-sm font-semibold text-white">
          {event.tecnicoNome}
        </span>
        <span className="text-xs text-gray-400">
          {event.horaComentario && !isNaN(event.horaComentario.getTime())
            ? event.horaComentario.toLocaleTimeString("pt-BR", {
                hour: "2-digit",
                minute: "2-digit",
              })
            : "..."}
        </span>
      </div>
      <p className="text-sm text-gray-300">{event.comentario}</p>
    </div>
  </div>
);

const DetalhesTicket: React.FC = () => {
  const { numeroChamado } = useParams<{ numeroChamado: string }>();
  const navigate = useNavigate();

  const [fullTicket, setFullTicket] = useState<FullTicketData | null>(null);
  const [history, setHistory] = useState<ChatHistoryEvent[]>([]);
  const [loading, setLoading] = useState(true);

  const [novoComentario, setNovoComentario] = useState("");
  const [isSending, setIsSending] = useState(false);

  const handlePanelUpdate = (newData: Partial<FullTicketData>) => {
    setFullTicket((prevTicket) => {
      if (!prevTicket) return null;
      return { ...prevTicket, ...newData };
    });
  };

  useEffect(() => {
    if (!numeroChamado) return;

    const fetchDetalhesEHistorico = async () => {
      setLoading(true);
      try {
        const detalhesResponse = await apiClient.get<
          Omit<FullTicketData, "numeroChamado">
        >(`/api/chamados/acoes/${numeroChamado}`);
        setFullTicket({
          ...detalhesResponse.data,
          numeroChamado: numeroChamado,
        });

        const historicoResponse = await apiClient.get(
          `/api/chamados/acoes/${numeroChamado}`
        );

        const formattedHistory: ChatHistoryEvent[] = historicoResponse.data.map(
          (item: any) => {
            const isSystemEvent = [
              "aberturaChamado",
              "iniciarAtendimento",
              "pausarAtendimento",
              "finalizarAtendimento",
            ].includes(item.retornoAcao);

            if (isSystemEvent) {
              let mensagem = item.mensagem;
              if (item.retornoAcao === "iniciarAtendimento")
                mensagem = "Atendimento Iniciado";
              if (item.retornoAcao === "pausarAtendimento")
                mensagem = `Atendimento Pausado: ${item.mensagem}`;
              if (item.retornoAcao === "finalizarAtendimento")
                mensagem = `Atendimento Finalizado: ${item.mensagem}`;
              if (item.retornoAcao === "aberturaChamado")
                mensagem = `Ticket Aberto: ${item.mensagem}`;

              return {
                type: "system",
                id: item.horaAtendimento,
                nomeTecnico: item.usuarioNome,
                horaInicioAtendimento: new Date(item.horaAtendimento),
                mensagemInicioAtendimento: mensagem,
              };
            } else {
              return {
                type: "comment",
                id: item.horaAtendimento,
                tecnicoNome: item.usuarioNome,
                horaComentario: new Date(item.horaAtendimento),
                comentario: item.mensagem,
              };
            }
          }
        );

        setHistory(formattedHistory);
      } catch (err) {
        console.error("Erro ao buscar dados do chamado:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchDetalhesEHistorico();
  }, [numeroChamado]);

  const handleAdicionarComentario = async () => {
    if (novoComentario.trim() === "" || !numeroChamado) return;

    setIsSending(true);
    const payload = {
      numeroChamado: numeroChamado,
      comentario: novoComentario,
    };

    try {
      const response = await apiClient.post<ComentarioApiResponse>(
        `/api/chamados/adicionar-comentario/${numeroChamado}`,
        payload,
        { headers: { "Content-Type": "application/json" } }
      );

      const novoEvento: CommentEvent = {
        type: "comment",
        id: response.data.id,
        tecnicoNome: response.data.usuarioNome,
        horaComentario: new Date(response.data.horaAtendimento),
        comentario: response.data.mensagem,
      };

      setHistory((prev) => [...prev, novoEvento]);
      setNovoComentario("");
    } catch (err) {
      console.error("Erro ao adicionar comentário:", err);
    } finally {
      setIsSending(false);
    }
  };

  if (loading) {
    return (
      <div className="p-10 text-white flex justify-center items-center h-screen">
        <Loader2 className="w-10 h-10 animate-spin text-purple-500" />
      </div>
    );
  }

  if (!fullTicket) {
    return <div className="p-10 text-white">Erro: Chamado não encontrado.</div>;
  }

  return (
    <div className="p-10 text-white min-h-screen">
      <button
        onClick={() => navigate("/tickets")}
        className="flex items-center space-x-2 bg-purple-500 hover:bg-purple-700 px-2 py-2 rounded-lg font-semibold transition-colors mb-4"
      >
        <ChevronLeft className="w-5 h-5" />
        <span>Voltar</span>
      </button>
      <div className="pt-1 border-t mb-4 border-white/40"></div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="md:col-span-2 bg-[#262626] p-6 rounded-lg flex flex-col">
          <h2 className="text-2xl font-bold mb-4">Andamento do Chamado</h2>

          <div className="flex-1 overflow-y-auto pr-2">
            {history.length > 0 ? (
              history.map((event) => {
                if (event.type === "system") {
                  return <SystemEventBubble key={event.id} event={event} />;
                }
                if (event.type === "comment") {
                  return <CommentEventBubble key={event.id} event={event} />;
                }
                return null;
              })
            ) : (
              <p className="text-gray-500 text-sm text-center pt-10">
                Nenhum andamento registrado.
              </p>
            )}
          </div>

          <div className="mt-4 pt-4 border-t border-gray-700">
            <textarea
              value={novoComentario}
              onChange={(e) => setNovoComentario(e.target.value)}
              rows={4}
              className="w-full bg-[#3B3B3B] border border-gray-600 rounded-lg p-2.5 mb-2"
              placeholder="Adicionar um comentário ao histórico..."
            />
            <div className="flex justify-end">
              <Button
                variant="BotãoRetomarChamado"
                onClick={handleAdicionarComentario}
                isLoading={isSending}
                disabled={novoComentario.trim() === ""}
              >
                <Send className="w-4 h-4 mr-2" />
                Enviar Comentário
              </Button>
            </div>
          </div>
        </div>

        <div className="md:col-span-1">
          <TicketDetalhesPainel
            ticket={fullTicket}
            onUpdate={handlePanelUpdate}
          />
        </div>
      </div>
    </div>
  );
};

export default DetalhesTicket;
