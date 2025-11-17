import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { ChevronLeft, Loader2, User, Send } from "lucide-react";
import apiClient from "../api/apiClient";
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

interface AcoesApiResponse {
  numeroChamado: string;
  retornoAcao: string;
  usuarioNome: string;
  horaAtendimento: string;
  mensagem: string;
}

interface ComentarioApiResponse {
  id: number;
  usuarioNome: string;
  horaAtendimento: string;
  mensagem: string;
}

const transformAcaoToHistoryEvent = (
  acao: AcoesApiResponse
): ChatHistoryEvent => {
  let mensagem = acao.mensagem;
  if (acao.retornoAcao === "pausarAtendimento") {
    mensagem = `Atendimento Pausado: ${acao.mensagem}`;
  } else if (acao.retornoAcao === "finalizarAtendimento") {
    mensagem = `Atendimento Finalizado: ${acao.mensagem}`;
  } else if (acao.retornoAcao === "iniciarAtendimento") {
    mensagem = "Atendimento Iniciado";
  } else if (acao.retornoAcao === "aberturaChamado") {
    mensagem = `Ticket Aberto: ${acao.mensagem}`;
  }
  return {
    type: "system",
    id: acao.horaAtendimento,
    nomeTecnico: acao.usuarioNome,
    horaInicioAtendimento: new Date(acao.horaAtendimento),
    mensagemInicioAtendimento: mensagem,
  };
};

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
            : "Enviando..."}
        </span>
      </div>
      <p className="text-sm text-gray-300">{event.comentario}</p>
    </div>
  </div>
);

interface TicketDetalhesData {
  numeroChamado: string;
  categoriaNome: string;
  subCategoriaNome: string;
  mesaAtendimento: string;
  tecnicoResponsavelNome: string;
  tempoAtendimentoTotal: string;
  statusSla: string;
  status: string;
}

const ClientTicketDetalhes: React.FC = () => {
  const { numeroChamado } = useParams<{ numeroChamado: string }>();
  const navigate = useNavigate();

  const [ticketDetalhes, setTicketDetalhes] =
    useState<TicketDetalhesData | null>(null);
  const [history, setHistory] = useState<ChatHistoryEvent[]>([]);
  const [loading, setLoading] = useState(true);
  const [novoComentario, setNovoComentario] = useState("");
  const [isSending, setIsSending] = useState(false);

  useEffect(() => {
    if (!numeroChamado) return;

    const fetchData = async () => {
      setLoading(true);
      try {
        const [detalhesResponse, historicoResponse] = await Promise.all([
          apiClient.get<TicketDetalhesData>(
            `/api/chamados/painel-cliente/${numeroChamado}`
          ),
          apiClient.get<AcoesApiResponse[]>(
            `/api/chamados/acoes/${numeroChamado}`
          ),
        ]);

        setTicketDetalhes(detalhesResponse.data);

        setHistory(historicoResponse.data.map(transformAcaoToHistoryEvent));
      } catch (err) {
        console.error("Erro ao buscar dados do chamado:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
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

  return (
    <div className="p-10 text-white min-h-screen max-w-4xl mx-auto">
      <button
        onClick={() => navigate("/portal/dashboard")}
        className="flex items-center space-x-2 bg-purple-500 hover:bg-purple-700 px-2 py-2 rounded-lg font-semibold transition-colors mb-4"
      >
        <ChevronLeft className="w-5 h-5" />
        <span>Voltar para o Dashboard</span>
      </button>
      <div className="pt-1 border-t mb-4 border-white/40"></div>

      {ticketDetalhes && (
        <div className="bg-[#3B3B3B] p-6 rounded-lg shadow-lg border border-gray-700 mb-6">
          <h2 className="text-xl font-bold text-white mb-4">
            Detalhes do Chamado: {ticketDetalhes.numeroChamado}
          </h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4 text-sm">
            <div>
              <span className="text-gray-400 block">Técnico Responsável:</span>
              <span className="text-white font-medium">
                {ticketDetalhes.tecnicoResponsavelNome}
              </span>
            </div>
            <div>
              <span className="text-gray-400 block">Status:</span>
              <span className="text-white font-medium capitalize">
                {ticketDetalhes.status}
              </span>
            </div>
            <div>
              <span className="text-gray-400 block">SLA:</span>
              <span className="text-white font-medium">
                {ticketDetalhes.statusSla === "emConformidade"
                  ? "Em Conformidade"
                  : "Vencido"}
              </span>
            </div>
            <div>
              <span className="text-gray-400 block">Categoria:</span>
              <span className="text-white font-medium">
                {ticketDetalhes.categoriaNome}
              </span>
            </div>
            <div>
              <span className="text-gray-400 block">Subcategoria:</span>
              <span className="text-white font-medium">
                {ticketDetalhes.subCategoriaNome}
              </span>
            </div>
            <div>
              <span className="text-gray-400 block">Mesa de Atendimento:</span>
              <span className="text-white font-medium">
                {ticketDetalhes.mesaAtendimento}
              </span>
            </div>
          </div>
        </div>
      )}

      <div className="bg-[#262626] p-6 rounded-lg flex flex-col">
        <h2 className="text-2xl font-bold mb-4">Andamento do Chamado</h2>

        <div className="flex-1 overflow-y-auto pr-2 min-h-[400px]">
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
            placeholder="Adicionar um comentário ao chamado..."
          />
          <div className="flex justify-end">
            <Button
              onClick={handleAdicionarComentario}
              isLoading={isSending}
              disabled={novoComentario.trim() === ""}
              className="bg-purple-600 hover:bg-purple-700"
            >
              <Send className="w-4 h-4 mr-2" />
              Enviar Comentário
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ClientTicketDetalhes;
