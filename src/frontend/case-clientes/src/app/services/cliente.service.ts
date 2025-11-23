import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { 
  Cliente, 
  CriarClienteRequest, 
  AtualizarClienteRequest, 
  TransacaoRequest 
} from '../models/cliente.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {
  private readonly apiUrl = `${environment.apiUrl}/api/clientes`;

  constructor(private http: HttpClient) {}

  obterTodos(): Observable<Cliente[]> {
    return this.http.get<Cliente[]>(this.apiUrl);
  }

  obterPorId(id: number): Observable<Cliente> {
    return this.http.get<Cliente>(`${this.apiUrl}/${id}`);
  }

  criar(request: CriarClienteRequest): Observable<Cliente> {
    return this.http.post<Cliente>(this.apiUrl, request);
  }

  atualizar(id: number, request: AtualizarClienteRequest): Observable<Cliente> {
    return this.http.put<Cliente>(`${this.apiUrl}/${id}`, request);
  }

  deletar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  depositar(id: number, request: TransacaoRequest): Observable<Cliente> {
    return this.http.post<Cliente>(`${this.apiUrl}/${id}/depositar`, request);
  }

  sacar(id: number, request: TransacaoRequest): Observable<Cliente> {
    return this.http.post<Cliente>(`${this.apiUrl}/${id}/sacar`, request);
  }
}
