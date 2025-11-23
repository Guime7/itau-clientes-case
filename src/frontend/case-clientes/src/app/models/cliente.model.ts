export interface Cliente {
  id: number;
  nome: string;
  email: string;
  saldo: number;
  dataCriacao: string;
  dataAtualizacao?: string;
}

export interface CriarClienteRequest {
  nome: string;
  email: string;
}

export interface AtualizarClienteRequest {
  nome: string;
  email: string;
}

export interface TransacaoRequest {
  valor: number;
  descricao: string;
}

export interface ApiResponse<T> {
  data?: T;
  mensagem?: string;
  erro?: string;
}
