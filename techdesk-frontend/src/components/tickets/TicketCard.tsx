import React from "react";
import { ChamadoView } from "../../types/chamado.types";
import {
  User,
  UserCheck,
  Play,
  Pause,
  CheckCircle,
  Pencil,
  Clock,
} from "lucide-react";
import Button from "../ui/Button";
import { useNavigate } from "react-router-dom";

const TicketActions: React.FC<{
  status: ChamadoView["status"];
  numeroChamado: string;
  onUpdate: (
    numeroChamado: string,
    acao: "play" | "pause" | "atribuir" | "view"
  ) => void;
}> = ({ status, numeroChamado, onUpdate }) => {
  switch (status) {
    case "Aberto":
      return (
        <Button
          variant="BotãoAtribuirChamado"
          onClick={() => onUpdate(numeroChamado, "play")}
          className="w-full bg-blue-600 hover:bg-blue-700 text-white text-sm py-1.5 px-3"
          title="Atribuir chamado a mim e iniciar atendimento"
        >
          <UserCheck className="w-4 h-4" />
        </Button>
      );
    case "Em Atendimento":
      return (
        <div className="flex space-x-2">
          <Button
            variant="BotãoPausarChamado"
            onClick={() => onUpdate(numeroChamado, "pause")}
            className="w-full bg-yellow-600 hover:bg-yellow-700 text-white text-xs py-1.5 px-3"
            title="Pausar Atendimento (Abre o modal)"
          >
            <Pause className="w-4 h-4" />
          </Button>
          <Button
            variant="BotãoFinalizarChamado"
            onClick={() => onUpdate(numeroChamado, "view")}
            className="w-full bg-red-600 hover:bg-red-700 text-white text-xs py-1.5 px-3"
            title="Finalizar Chamado (Abre o modal)"
          >
            <CheckCircle className="w-4 h-4" />
          </Button>
        </div>
      );
    case "Pausado":
      return (
        <Button
          variant="BotãoRetomarChamado"
          onClick={() => onUpdate(numeroChamado, "play")}
          className="w-full bg-green-600 hover:bg-green-700 text-white text-sm py-1.5 px-3"
        >
          <Play className="w-4 h-4" />
        </Button>
      );
    case "Concluído":
    default:
      return null;
  }
};

const PriorityDisplay: React.FC<{ prioridade: ChamadoView["prioridade"] }> = ({
  prioridade,
}) => {
  const colors = {
    Alta: "bg-red-500",
    Média: "bg-yellow-500",
    Baixa: "bg-blue-500",
  };
  return (
    <div
      className="flex items-center space-x-2"
      title={`Prioridade: ${prioridade}`}
    >
      <span className={`w-3 h-3 rounded-full ${colors[prioridade]}`} />
      <span className="text-sm font-medium text-gray-300 capitalize">
        {prioridade}
      </span>
    </div>
  );
};

const SlaDisplay: React.FC<{ sla: ChamadoView["slaStatus"] }> = ({ sla }) => {
  const styles = {
    Vencido: "bg-red-600/20 text-red-400 border-red-500/30",
    "Não Iniciado": "bg-gray-600/20 text-gray-400 border-gray-500/30",
    "No Prazo": "bg-green-600/20 text-green-400 border-green-500/30",
  };
  const style = styles[sla] || styles["Não Iniciado"];

  return (
    <div
      className={`flex items-center space-x-1.5 text-xs font-semibold px-2 py-1 rounded-md border ${style}`}
    >
      <Clock className="w-3 h-3" />
      <span>{sla}</span>
    </div>
  );
};

interface TicketCardProps {
  ticket: ChamadoView;
  onUpdate: (
    numeroChamado: string,
    acao: "play" | "pause" | "atribuir" | "view"
  ) => void;
}

const TicketCard: React.FC<TicketCardProps> = ({ ticket, onUpdate }) => {
  const navigate = useNavigate();

  const handleEditClick = (e: React.MouseEvent) => {
    e.stopPropagation();
    navigate(`/chamados/detalhes/${ticket.id}`);
  };

  return (
    <div
      onClick={() => onUpdate(ticket.id, "view")}
      className="bg-[#3B3B3B] rounded-lg shadow-lg border border-gray-700/50 flex flex-col justify-between min-h-[220px] cursor-pointer hover:border-purple-500/70 transition-all duration-200"
    >
      <div className="p-4 pb-2">
        <div className="flex justify-between items-start">
          <div>
            <p
              className="text-base font-semibold text-white hover:underline"
              title={ticket.assunto}
            >
              {ticket.assunto}
            </p>
            <p className="text-xs text-gray-400">#{ticket.id}</p>
          </div>

          <button
            onClick={handleEditClick}
            className="p-1.5 rounded-md hover:bg-gray-700 text-gray-500 hover:text-yellow-500"
            title="Ver e Editar Detalhes do Chamado"
          >
            <Pencil className="w-4 h-4" />
          </button>
        </div>
      </div>

      <div className="px-4 py-2 space-y-3 flex-grow">
        <div className="text-sm text-gray-400 space-y-1">
          <p>
            <span className="font-medium text-gray-500 w-16 inline-block">
              Cliente:
            </span>
            <span className="text-gray-300">{ticket.clienteNome}</span>
          </p>
          <p>
            <span className="font-medium text-gray-500 w-16 inline-block">
              Mesa:
            </span>
            <span className="text-gray-300">{ticket.mesaNome}</span>
          </p>
        </div>

        <div className="flex justify-between items-center pt-3 border-t border-gray-700/50">
          <SlaDisplay sla={ticket.slaStatus} />
          <PriorityDisplay prioridade={ticket.prioridade} />
        </div>
      </div>

      <div className="px-4 py-3 bg-gray-900/30 rounded-b-lg mt-auto">
        <div className="flex items-center justify-between">
          <div
            className="flex items-center space-x-2 text-sm text-gray-400"
            title={`Técnico: ${ticket.tecnicoNome || "Não atribuído"}`}
          >
            <User className="w-4 h-4" />
            <span className="truncate max-w-[100px]">
              {ticket.tecnicoNome || "Não atribuído"}
            </span>
          </div>

          <div onClick={(e) => e.stopPropagation()}>
            <TicketActions
              status={ticket.status}
              numeroChamado={ticket.id}
              onUpdate={onUpdate}
            />
          </div>
        </div>
      </div>
    </div>
  );
};

export default TicketCard;
