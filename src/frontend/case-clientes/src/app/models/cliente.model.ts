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

// Result Pattern types matching backend API
export interface Result<T> {
  isSuccess: boolean;
  data?: T;
  message?: string;
  errorCode?: string;
  errorDescription?: string;
}

export interface ResultVoid {
  isSuccess: boolean;
  message?: string;
  errorCode?: string;
  errorDescription?: string;
}
