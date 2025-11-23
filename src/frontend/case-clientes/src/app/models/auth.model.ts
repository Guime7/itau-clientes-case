export interface LoginRequest {
  tipoUsuario: string;  // "Admin" ou "Cliente"
  clienteId?: number;   // Obrigatório se tipoUsuario === "Cliente"
  email?: string;
  senha?: string;       // Aceito mas ignorado pela API
}

export interface LoginResponse {
  token: string;
  tipoUsuario: string;
  clienteId?: number;
  email?: string;
  expiresAt: string;
}

export interface User {
  email?: string;
  tipoUsuario: string;
  clienteId?: number;
}
