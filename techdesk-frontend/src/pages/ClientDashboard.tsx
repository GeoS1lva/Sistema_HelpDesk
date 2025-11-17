import React, { useState, useEffect, useCallback } from "react";
import { Plus, Clock, CheckCircle, PauseCircle, Loader2, ChevronRight } from "lucide-react";
import apiClient from "../api/apiClient";
import CadastroTicketClienteModal from "../components/tickets/CadastroTicketClienteModal";
import { useNavigate } from "react-router-dom";

import { ChamadoDashboard } from "../types/chamado.types"; 


const StatCard: React.FC<{
  title: string;
  value: number;
  icon: React.ElementType;
}> = ({ title, value, icon: Icon }) => (
  <div className="bg-[#3B3B3B] p-6 rounded-lg shadow-lg border border-gray-700 flex items-center space-x-4">
    <div className="p-3 bg-purple-600/20 rounded-full">
      <Icon className="w-6 h-6 text-purple-400" />
    </div>
    <div>
      <p className="text-3xl font-bold text-white">{value}</p>
      <p className="text-sm text-gray-400">{title}</p>
    </div>
  </div>
);


interface StatsData {
  chamadosAbertos: number;
  chamadosPausados: number;
  chamadosFinalizados: number;
}



const ClientDashboard: React.FC = () => {
  const [stats, setStats] = useState<StatsData>({
    chamadosAbertos: 0,
    chamadosPausados: 0,
    chamadosFinalizados: 0,
  });
  
  const [meusChamados, setMeusChamados] = useState<ChamadoDashboard[]>([]);
  const [loadingStats, setLoadingStats] = useState(true);
  const [loadingTickets, setLoadingTickets] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const navigate = useNavigate(); 

  
  const fetchData = useCallback(async () => {
    setLoadingStats(true);
    setLoadingTickets(true);
    try {
      const [statsResponse, chamadosResponse] = await Promise.all([
        apiClient.get<StatsData>("/api/chamados/quantidade"),
        apiClient.get<ChamadoDashboard[]>("/api/chamados/painel-cliente/informativo")
      ]);

      setStats(statsResponse.data);
      setMeusChamados(chamadosResponse.data); 

    } catch (err) {
      console.error("Erro ao buscar dados do dashboard:", err);
    } finally {
      setLoadingStats(false);
      setLoadingTickets(false);
    }
  }, []); 

  useEffect(() => {
    fetchData(); 
  }, [fetchData]);

  
  const handleCadastroSuccess = (novoTicket: any) => {
    setIsModalOpen(false);
    fetchData(); 
  };

  return (
    <div className="space-y-8">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold">Meu Dashboard</h1>
        <button
          onClick={() => setIsModalOpen(true)}
          className="flex items-center space-x-2 bg-purple-600 hover:bg-purple-700 px-4 py-2 rounded-lg font-semibold transition-colors"
        >
          <Plus className="w-5 h-5" />
          <span>Abrir Novo Chamado</span>
        </button>
      </div>

      {loadingStats ? (
        <div className="flex justify-center items-center p-10">
          <Loader2 className="w-8 h-8 animate-spin text-purple-500" />
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          <StatCard title="Chamados Abertos" value={stats.chamadosAbertos} icon={Clock} />
          <StatCard title="Chamados Pausados" value={stats.chamadosPausados} icon={PauseCircle} />
          <StatCard title="Chamados Concluídos" value={stats.chamadosFinalizados} icon={CheckCircle} />
        </div>
      )}


      <div className="bg-[#1E1E1E] p-6 rounded-lg shadow-lg border border-gray-800">
        <h2 className="text-2xl font-bold mb-4">Meus Chamados Recentes</h2>
        <div className="divide-y divide-gray-700">
          {loadingTickets ? (
            <div className="flex justify-center items-center p-10">
              <Loader2 className="w-8 h-8 animate-spin text-purple-500" />
            </div>
          ) : meusChamados.length > 0 ? (
            meusChamados.map((ticket) => (
              <div
                key={ticket.numeroChamado}
                onClick={() => navigate(`/portal/chamado/${ticket.numeroChamado}`)}
                className="flex justify-between items-center py-4 px-2 cursor-pointer hover:bg-gray-800/50 rounded-lg"
              >
                <div>
                  <p className="text-base font-semibold text-white">{ticket.assunto}</p>
                  <p className="text-sm text-gray-400">
                    {ticket.numeroChamado} • Status: <span className="capitalize">{ticket.status}</span>
                  </p>

                </div>
                <ChevronRight className="w-5 h-5 text-gray-500" />
              </div>
            ))
          ) : (
            <p className="text-gray-400 text-center py-5">Você ainda não abriu nenhum chamado.</p>
          )}
        </div>
      </div>

      {isModalOpen && (
        <CadastroTicketClienteModal
          onClose={() => setIsModalOpen(false)}
          onCadastroSuccess={handleCadastroSuccess}
        />
      )}
    </div>
  );
};

export default ClientDashboard;