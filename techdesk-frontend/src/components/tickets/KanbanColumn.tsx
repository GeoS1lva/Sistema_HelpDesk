import { ChamadoView } from "../../types/chamado.types";
import TicketCard from "./TicketCard";

interface KanbanColumnProps {
    title: string
    tickets: ChamadoView[]
}

const KanbanColumn: React.FC<KanbanColumnProps> = ({ title, tickets }) => {
    return (
        <div className="flex-1 min-w-[300px] max-w-full lg:max-w-1/4 max-h-52 p-1">
            <div className="bg-[#262626] p-2 rounded-lg h-full">
                <h2 className="text-lg font-bold text-white mb-4">{title} ({tickets.length})</h2>

                <div className="space-y-4-[calc(100vh-250px)] overflow-y-auto">
                    {tickets.length > 0 ? (
                        tickets.map((ticket) => (
                            <TicketCard key={ticket.id} ticket={ticket} />
                        ))
                    ) : (
                        <p className="text-gray-500 text-sm text-center pt-10">
                            Nenhum chamado nesta coluna.
                        </p>
                    )}
                </div>
            </div>
        </div>
    )
}

export default KanbanColumn