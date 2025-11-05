import React from "react";
import { ChamadoView } from "../../types/chamado.types";
import { User } from "lucide-react";

interface TicketCardProps {
  ticket: ChamadoView;
}

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
      <div className="flex items-center space-x-2 mb-3">
        <span
          className={`w-3 h-3 rounded-full ${priorityColor}`}
          title={`Prioridade: ${ticket.prioridade}`}
        />
        <span className="text-sm font-semibold text-gray-300">
          Prioridade {ticket.prioridade}
        </span>
      </div>

      <p className="text-base font-semibold text-white mb-3 truncate">
        {ticket.assunto}
      </p>

      <p className="text-sm text-gray-400  mb-4">
        Cliente: <span className="text-gray-200">{ticket.clienteNome}</span>
      </p>

      <div className="flex items-center space-x-2 text-sm text-gray-400">
        <User className="w-4 h-4" />
        <span>{ticket.tecnicoNome}</span>
      </div>
    </div>
  );
};

export default TicketCard;
