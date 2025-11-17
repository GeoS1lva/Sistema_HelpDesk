import { ChamadoView } from "../../types/chamado.types";
import TicketCard from "./TicketCard";

interface KanbanColumnProps {
  title: string;
  tickets: ChamadoView[];
  onUpdateTicket: (
    numeroChamado: string,
    acao: "play" | "pause" | "atribuir" | "view"
  ) => void;
}

const KanbanColumn: React.FC<KanbanColumnProps> = ({
  title,
  tickets,
  onUpdateTicket,
}) => {
  return (
    <div className="flex-1 min-w-[300px] max-w-full lg:max-w-1/4 p-1">
      <div className="bg-[#262626] rounded-lg h-full flex flex-col">
        <h2 className="text-lg font-bold text-white mb-4 px-4 pt-4">
          {title} ({tickets.length})
        </h2>
        <div className="space-y-4 h-[calc(100vh-250px)] overflow-y-auto px-4 pb-4">
          {tickets.length > 0 ? (
            tickets.map((ticket) => (
              <TicketCard
                key={ticket.id}
                ticket={ticket}
                onUpdate={onUpdateTicket}
              />
            ))
          ) : (
            <p className="text-gray-500 text-sm text-center pt-10">
              Nenhum chamado nesta coluna.
            </p>
          )}
        </div>
      </div>
    </div>
  );
};

export default KanbanColumn;
