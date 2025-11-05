import React from "react";
import { ChamadoView } from "../../types/chamado.types";
import { Pause, Play, TicketIcon, User, UserCheck } from "lucide-react";
import Button from "../ui/Button";

interface TicketCardProps {
  ticket: ChamadoView;
}

const TicketActions: React.FC<{ status: ChamadoView["status"] }> = ({
  status,
}) => {
  switch (status) {
    case "Aberto":
      return (
        <Button className="w-[210px] bg-blue-600 hover:bg-blue-700 text-white text-sm py-1.5">
          <UserCheck className="w-4 h-4 mr-2" />
          Atribuir Chamado
        </Button>
      );
    case "Em Atendimento":
      return (
        <Button className="w-[210px] bg-yellow-600 hover:bg-yellow-700 text-white text-sm py-1.5">
          <Pause className="w-4 h-4 mr-2" />
          Pausar Chamado
        </Button>
      );
    case "Pausado":
      return (
        <Button className="w-[210px] bg-green-600 hover:bg-green-700 text-white text-sm py-1.5">
          <Play className="w-4 h-4 mr-2" />
          Retomar Atendimento
        </Button>
      );

    case "Concluído":
    default:
      return null;
  }
};

const getPriorityColor = (prioridade: ChamadoView["prioridade"]) => {
  switch (prioridade) {
    case "Alta":
      return "bg-red-500";
    case "Média":
      return "bg-yellow-500";
    case "Baixa":
      return "bg-green-500";
  }
};

const TicketCard: React.FC<TicketCardProps> = ({ ticket }) => {
  const priorityColor = getPriorityColor(ticket.prioridade);

  return (
    <div className="bg-[#3B3B3B] p-4 rounded-lg shadow-md border border-gray-700 hover:border-purple-500 transition-colors cursor-pointer mb-3">
      <div>
        {/* LINHA 1: SLA e Prioridade */}
        <div className="flex justify-between items-center mb-5">
          <div
            className="flex items-center space-x-2"
            title={`Prioridade: ${ticket.prioridade}`}
          >
            <span className={`w-3 h-3 rounded-full ${priorityColor}`} />
            <span className="text-xs font-medium text-gray-300">
              {ticket.prioridade}
            </span>
          </div>
          <span
            className={`text-xs font-semibold px-2 py-0.5 rounded
              ${
                ticket.slaStatus === "No Prazo"
                  ? "bg-green-600/20 text-green-400"
                  : "bg-red-600/20 text-red-400"
              }
            `}
          >
            SLA: {ticket.slaStatus}
          </span>
        </div>

        <div className="mb-8">
          <p className="text-xs text-gray-500">Ticket #{ticket.id}</p>
          <p
            className="text-base font-semibold text-white truncate"
            title={ticket.assunto}
          >
            {ticket.assunto}
          </p>
        </div>

        <div className="text-sm text-gray-400 space-y-1 mb-4">
          <p>
            <span className="font-medium">Cliente:</span> {ticket.clienteNome}
          </p>
          <p>
            <span className="font-medium">Mesa:</span> {ticket.mesaNome}
          </p>
        </div>
      </div>

      <div className="mt-auto">
        <div className="flex items-center justify-between text-sm text-gray-400 mb-3">
          <div className="flex items-center space-x-2">
            <User className="w-4 h-4" />
            <span>{ticket.tecnicoNome}</span>
          </div>
        </div>

        <TicketActions status={ticket.status} />
      </div>
    </div>
  );
};

export default TicketCard;
