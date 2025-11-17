import React, { useState, useEffect } from "react";
import {
  ChamadoView,
  ChamadoApiResponse,
  mapPrioridadeNum,
} from "../../types/chamado.types";
import Modal from "../ui/Modal";
import Button from "../ui/Button";
import { X, User, Play, Loader2, Pause, CheckCircle } from "lucide-react";
import apiClient from "../../api/apiClient";

export interface SystemEvent {
  type: "system";
  id: number | string;
  usuarioNome: string;
  horaAtendimento: Date;
  mensagemInicioAtendimento: string;
  comentario: string;
}
export type ChatHistoryEvent = SystemEvent;

interface EventoApiResponse {
  usuarioNome: string;
  horaAtendimento: string;
  mensagem: string;
}
interface PauseApiResponse {
  numeroChamado: string;
  status: string;
  usuarioNome: string;
  horaAtendimento: string;
  comentario: string;
  mensagem: string;
}

const SystemEventBubble: React.FC<{ event: SystemEvent }> = ({ event }) => (
  <div className="flex items-start space-x-4 my-4">
    <div className="flex-shrink-0 w-10 h-10 rounded-full bg-gradient-to-br from-gray-700 to-gray-800 flex items-center justify-center shadow-lg">
      <User className="w-5 h-5 text-purple-300" />
    </div>

    <div
      className="flex-1 bg-gray-800 rounded-lg shadow-xl"
      style={{
        background:
          "linear-gradient(145deg, rgba(41, 41, 50, 0.8), rgba(30, 30, 35, 0.8))",
        borderLeft: "4px solid #8B5CF6",
      }}
    >
      <div className="flex justify-between items-center p-4 pb-2 border-b border-gray-700/50">
        <span className="text-base font-semibold text-white">
          {event.usuarioNome}
        </span>
        <span className="text-xs text-gray-400">
          {event.horaAtendimento.toLocaleTimeString("pt-BR", {
            hour: "2-digit",
            minute: "2-digit",
          })}
        </span>
      </div>

      <div className="p-4 pt-3">
        <p className="text-sm text-gray-200">
          {event.mensagemInicioAtendimento}
        </p>
      </div>
    </div>
  </div>
);

interface AtendimentoProps {
  ticket: ChamadoView;
  onClose: () => void;
  onUpdateTicketInList: (ticket: ChamadoApiResponse) => void;
  history: ChatHistoryEvent[];
  isLoadingHistory: boolean;
  onAddHistoryEvent: (ticketId: string, event: SystemEvent) => void;
}

const AtendimentoTicketModal: React.FC<AtendimentoProps> = ({
  ticket,
  onClose,
  onUpdateTicketInList,
  history,
  isLoadingHistory,
  onAddHistoryEvent,
}) => {
  const [localTicket, setLocalTicket] = useState(ticket);
  const [novoComentario, setNovoComentario] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    setLocalTicket(ticket);
  }, [ticket]);

  const handleStartAtendimento = async () => {
    setIsLoading(true);
    try {
      const response = await apiClient.post<EventoApiResponse>(
        `/api/chamados/play/${localTicket.id}`,
        {},
        { headers: { "Content-Type": "application/json" } }
      );
      const apiData = response.data;
      const novoEvento: SystemEvent = {
        type: "system",
        id: Math.random(),
        usuarioNome: apiData.usuarioNome,
        horaAtendimento: new Date(apiData.horaAtendimento),
        mensagemInicioAtendimento: apiData.mensagem,
      };
      onAddHistoryEvent(localTicket.id, novoEvento);
      onUpdateTicketInList({
        id: localTicket.id,
        assunto: localTicket.assunto,
        status: "Em Atendimento",
        prioridade: mapPrioridadeNum(localTicket.prioridade),
        clienteNome: localTicket.clienteNome,
        mesaNome: localTicket.mesaNome,
        tecnicoNome: apiData.usuarioNome,
      });
      setLocalTicket((prev) => ({ ...prev, status: "Em Atendimento" }));
    } catch (err) {
      console.error("Erro ao iniciar atendimento", err);
    } finally {
      setIsLoading(false);
    }
  };

  const handlePauseAtendimento = async () => {
    if (novoComentario.trim() === "") {
      alert("Para pausar, o comentário é obrigatório.");
      return;
    }
    setIsLoading(true);
    const payload = {
      numeroChamado: localTicket.id,
      comentario: novoComentario,
    };
    try {
      const response = await apiClient.post<PauseApiResponse>(
        `/api/chamados/pausar/${localTicket.id}`,
        payload,
        { headers: { "Content-Type": "application/json" } }
      );
      const apiData = response.data;
      const novoEvento: SystemEvent = {
        type: "system",
        id: Math.random(),
        usuarioNome: apiData.usuarioNome,
        horaAtendimento: new Date(apiData.horaAtendimento),
        mensagemInicioAtendimento: `Atendimento Pausado: ${apiData.mensagem}`,
        comentario: apiData.mensagem, // <-- CORRIGIDO
      };
      onAddHistoryEvent(localTicket.id, novoEvento);
      onUpdateTicketInList({
        id: localTicket.id,
        assunto: localTicket.assunto,
        status: "Pausado",
        prioridade: mapPrioridadeNum(localTicket.prioridade),
        clienteNome: localTicket.clienteNome,
        mesaNome: localTicket.mesaNome,
        tecnicoNome: apiData.usuarioNome,
      });
      setLocalTicket((prev) => ({ ...prev, status: "Pausado" }));
      setNovoComentario("");
    } catch (err) {
      console.error("Erro ao pausar atendimento", err);
    } finally {
      setIsLoading(false);
    }
  };

  const handleFinalizarAtendimento = async () => {
    if (novoComentario.trim() === "") {
      alert("Para finalizar, o comentário é obrigatório.");
      return;
    }
    setIsLoading(true);
    const payload = {
      numeroChamado: localTicket.id,
      comentarioFinalizacao: novoComentario,
    };
    try {
      const response = await apiClient.post<EventoApiResponse>(
        `/api/chamados/finalizar/${localTicket.id}`,
        payload,
        { headers: { "Content-Type": "application/json" } }
      );
      const apiData = response.data;
      const novoEvento: SystemEvent = {
        type: "system",
        id: Math.random(),
        usuarioNome: apiData.usuarioNome,
        horaAtendimento: new Date(apiData.horaAtendimento),
        mensagemInicioAtendimento: `Atendimento Finalizado: ${apiData.mensagem}`,
      };
      onAddHistoryEvent(localTicket.id, novoEvento);
      onUpdateTicketInList({
        id: localTicket.id,
        assunto: localTicket.assunto,
        status: "Concluído",
        prioridade: mapPrioridadeNum(localTicket.prioridade),
        clienteNome: localTicket.clienteNome,
        mesaNome: localTicket.mesaNome,
        tecnicoNome: apiData.usuarioNome,
      });
      setLocalTicket((prev) => ({ ...prev, status: "Concluído" }));
      setNovoComentario("");
    } catch (err) {
      console.error("Erro ao finalizar atendimento", err);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Modal onClose={onClose} className="max-w-6xl  max-h-6xl">
      <div className="flex flex-col h-full">
        <div className="flex justify-between items-center pb-4 mb-4 border-b border-gray-700">
          <div>
            <h2 className="text-3xl font-bold">{localTicket.assunto}</h2>
            <p className="text-sm text-gray-400">
              {localTicket.id} • {localTicket.clienteNome}
            </p>
          </div>
        </div>

        <div className="flex-1 overflow-y-auto pr-2">
          {isLoadingHistory ? (
            <div className="flex justify-center items-center h-full">
              <Loader2 className="w-8 h-8 animate-spin text-purple-500" />
            </div>
          ) : (
            history.map((event) => (
              <SystemEventBubble key={event.id} event={event} />
            ))
          )}
          {!isLoadingHistory && history.length === 0 && (
            <p className="text-gray-500 text-sm text-center pt-10">
              Nenhum andamento registrado para este chamado.
            </p>
          )}
        </div>

        <div className="mt-4 pt-4 border-t border-gray-700">
          {localTicket.status === "Em Atendimento" && (
            <textarea
              value={novoComentario}
              onChange={(e) => setNovoComentario(e.target.value)}
              rows={3}
              className="w-full bg-[#3B3B3B] border border-gray-600 rounded-lg p-2.5 mb-2"
              placeholder="Digite o comentário obrigatório para Pausar ou Finalizar..."
            />
          )}

          <div className="flex justify-end items-center mt-2 space-x-3">
            {(localTicket.status === "Aberto" ||
              localTicket.status === "Pausado") && (
              <Button
                variant="BotãoAtribuirChamado"
                onClick={handleStartAtendimento}
                isLoading={isLoading}
                className="bg-blue-600 hover:bg-blue-700"
              >
                <Play className="w-4 h-4 mr-2" />
                {localTicket.status === "Aberto"
                  ? "Iniciar Atendimento"
                  : "Retomar Atendimento"}
              </Button>
            )}

            {localTicket.status === "Em Atendimento" && (
              <>
                <Button
                  variant="BotãoAtribuirChamado"
                  onClick={handlePauseAtendimento}
                  isLoading={isLoading}
                  disabled={novoComentario.trim() === ""}
                  className="bg-yellow-600 hover:bg-yellow-700 text-white disabled:bg-gray-500"
                >
                  <Pause className="w-4 h-4 mr-2" />
                  Pausar Atendimento
                </Button>

                <Button
                  variant="BotãoFinalizarChamado"
                  onClick={handleFinalizarAtendimento}
                  isLoading={isLoading}
                  disabled={novoComentario.trim() === ""}
                  className="text-white disabled:bg-gray-500"
                >
                  <CheckCircle className="w-4 h-4 mr-2" />
                  Finalizar Chamado
                </Button>
              </>
            )}
          </div>
        </div>
      </div>
    </Modal>
  );
};

export default AtendimentoTicketModal;
