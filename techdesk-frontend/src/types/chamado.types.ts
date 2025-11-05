
export interface ChamadoFromApi {
  Id: number;
  Assunto: string;
  Descricao: string;
  Status: number; 
  Prioridade: number; 
  EmpresaNome: string; 
  TecnicoNome?: string; 
}


export interface ChamadoView {
  id: number;
  assunto: string;
  status: 'Aberto' | 'Em Atendimento' | 'Pausado' | 'Concluído';
  prioridade: "Baixa" | "Média" | "Alta";
  clienteNome: string;
  tecnicoNome: string;
}


const mapStatus = (status: number): ChamadoView['status'] => {
  switch (status) {
    case 1: return 'Aberto';
    case 2: return 'Em Atendimento';
    case 3: return 'Pausado';
    case 4: return 'Concluído';
    default: return 'Aberto'; 
  }
};

const mapPrioridade = (prioridade: number): ChamadoView['prioridade'] => {
  switch (prioridade) {
    case 1: return 'Baixa';
    case 2: return 'Média';
    case 3: return 'Alta';
    default: return 'Baixa';
  }
};


export const transformChamadoApiToView = (apiData: ChamadoFromApi): ChamadoView => ({
  id: apiData.Id,
  assunto: apiData.Assunto,
  status: mapStatus(apiData.Status),
  prioridade: mapPrioridade(apiData.Prioridade),
  clienteNome: apiData.EmpresaNome,
  tecnicoNome: apiData.TecnicoNome || 'Não atribuído', 
});
