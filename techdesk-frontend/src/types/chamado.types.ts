export interface ChamadoDashboard {
  numeroChamado: string;
  assunto: string;
  prioridade: "alta" | "media" | "baixa";
  sla: "naoIniciado" | "noPrazo" | "vencido";
  nomeEmpresa: string;
  nomeMesaAtendimento: string;
  nomeTecnicoResponsavel: string;

  status: "aberto" | "emAtendimento" | "pausado" | "concluido";
}

export interface ChamadoApiResponse {
  numeroChamado: string;
  assunto: string;
  descricao: string;
  status: "aberto" | "emAtendimento" | "pausado" | "concluido";
  nomeMesaAtendimento: string;
  dataCriacao: string;
  nomeTecnicoAberturaChamado: string;
  empresa: {
    id: number;
    nome: string;
  };
  usuario: {
    nome: string;
    email: string;
    userName: string;
  };
  categoria: {
    id: number;
    nome: string;
    subCategorias: {
      id: number;
      nome: string;
      prioridade: "alta" | "media" | "baixa";
      sla: number;
    }[];
  };
}

export interface ChamadoView {
  id: string;
  assunto: string;
  status: "Aberto" | "Em Atendimento" | "Pausado" | "Concluído";
  prioridade: "Baixa" | "Média" | "Alta";
  slaStatus: string;
  clienteNome: string;
  mesaNome: string;
  tecnicoNome: string;
}

const mapStatus = (status: string): ChamadoView["status"] => {
  if (status === "emAndamento") return "Em Atendimento";
  if (status === "pausado") return "Pausado";
  if (status === "finalizado") return "Concluído";
  return "Aberto";
};

const mapPrioridade = (p: string): ChamadoView["prioridade"] => {
  if (p === "alta") return "Alta";
  if (p === "media") return "Média";
  return "Baixa";
};

const mapSla = (s: string): string => {
  if (s === "naoIniciado") return "Não Iniciado";
  if (s === "noPrazo") return "No Prazo";
  if (s === "vencido") return "Vencido";
  return "N/D";
};

export const transformDashboardToView = (
  apiData: ChamadoDashboard
): ChamadoView => ({
  id: apiData.numeroChamado,
  assunto: apiData.assunto,
  status: mapStatus(apiData.status),
  prioridade: mapPrioridade(apiData.prioridade),
  slaStatus: mapSla(apiData.sla),
  clienteNome: apiData.nomeEmpresa,
  mesaNome: apiData.nomeMesaAtendimento,
  tecnicoNome: apiData.nomeTecnicoResponsavel || "Não atribuído",
});

export const transformPostToView = (
  apiData: ChamadoApiResponse
): ChamadoView => ({
  id: apiData.numeroChamado,
  assunto: apiData.assunto,
  status: mapStatus(apiData.status),
  prioridade: mapPrioridade(
    apiData.categoria.subCategorias[0]?.prioridade || "baixa"
  ),
  slaStatus: "Não Iniciado",
  clienteNome: apiData.empresa.nome,
  mesaNome: apiData.nomeMesaAtendimento,
  tecnicoNome: apiData.nomeTecnicoAberturaChamado || "Não atribuído",
});

export interface EmpresaParaDropdown {
  id: number;
  nome: string;
}

export interface MesaParaDropdown {
  id: number;
  nome: string;
}

export interface CategoriaParaDropdown {
  id: number;
  nome: string;
  subcategorias: SubcategoriaParaDropdown[];
}

export interface SubcategoriaParaDropdown {
  id: number;
  nome: string;
  prioridade: number;
}

export const mapPrioridadeNum = (
  prioridade: number
): "Baixa" | "Média" | "Alta" => {
  switch (prioridade) {
    case 1:
      return "Baixa";
    case 2:
      return "Média";
    case 3:
      return "Alta";
    default:
      return "Baixa";
  }
};
