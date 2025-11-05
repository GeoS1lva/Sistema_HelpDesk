import React, { useState, useEffect, useMemo } from 'react';
import { Plus, Search, ChevronDown } from 'lucide-react';
import apiClient from '../api/apiClient,';
import {
  ChamadoFromApi,
  ChamadoView,
  transformChamadoApiToView,
} from '../types/chamado.types';
import KanbanColumn from '../components/tickets/KanbanColumn';

const USE_MOCK_DATA_GET = true;


const mockTickets: ChamadoView[] = [
  { id: 1, assunto: 'Problema de conexão com servidor de arquivos', status: 'Aberto', prioridade: 'Alta', clienteNome: 'Empresa A', tecnicoNome: 'Não atribuído' },
  { id: 2, assunto: 'Aguardando liberação de acesso', status: 'Pausado', prioridade: 'Média', clienteNome: 'Empresa B', tecnicoNome: 'Alan M.' },
  { id: 3, assunto: 'Mouse sem fio não está conectando', status: 'Em Atendimento', prioridade: 'Baixa', clienteNome: 'Empresa A', tecnicoNome: 'Alan M.' },
  { id: 4, assunto: 'Instalação de software finalizada', status: 'Concluído', prioridade: 'Baixa', clienteNome: 'Empresa C', tecnicoNome: 'Beatriz S.' },
  { id: 5, assunto: 'Impressora não funciona na rede', status: 'Aberto', prioridade: 'Média', clienteNome: 'Empresa B', tecnicoNome: 'Não atribuído' },
];

interface GroupedTickets {
  abertos: ChamadoView[];
  emAtendimento: ChamadoView[];
  pausados: ChamadoView[];
  concluidos: ChamadoView[];
}

const Tickets: React.FC = () => {
  const [tickets, setTickets] = useState<ChamadoView[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

 
  useEffect(() => {
    const fetchTickets = async () => {
      setLoading(true);
      setError(null);
      if (USE_MOCK_DATA_GET) {
        setTickets(mockTickets);
        setLoading(false);
        return;
      }

      try {
        const response = await apiClient.get<ChamadoFromApi[]>('/api/chamados');
        const transformedData = response.data.map(transformChamadoApiToView);
        setTickets(transformedData);
      } catch (err) {
        setError('Falha ao carregar os chamados.');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };
    fetchTickets();
  }, []);


  const groupedTickets = useMemo((): GroupedTickets => {
    return {
      abertos: tickets.filter((t) => t.status === 'Aberto'),
      emAtendimento: tickets.filter((t) => t.status === 'Em Atendimento'),
      pausados: tickets.filter((t) => t.status === 'Pausado'),
      concluidos: tickets.filter((t) => t.status === 'Concluído'),
    };
  }, [tickets]);

  if (loading) {
    return <div className="p-10 text-white">Carregando chamados...</div>;
  }
  if (error) {
    return <div className="p-10 text-red-500">{error}</div>;
  }

  return (
    <div className="p-10 text-white h-full flex flex-col">
      <div className="mt-auto pt-4 border-t mb-10 border-white/40"></div>
      <div className="mt-auto pt-4 border-t mb-10 border-white/40"></div>
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Tickets</h1>
        <button className="flex items-center space-x-2 bg-purple-600 hover:bg-purple-700 px-4 py-2 rounded-lg font-semibold transition-colors">
          <Plus className="w-5 h-5" />
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


      <div className="flex-1 flex flex-col lg:flex-row lg:space-y-1 lg:space-x-4 overflow-x-auto">
        <KanbanColumn title="Abertos" tickets={groupedTickets.abertos} />
        <KanbanColumn title="Em Atendimento" tickets={groupedTickets.emAtendimento} />
        <KanbanColumn title="Pausados" tickets={groupedTickets.pausados} />
        <KanbanColumn title="Concluídos" tickets={groupedTickets.concluidos} />
      </div>
    </div>
  );
};

export default Tickets;
