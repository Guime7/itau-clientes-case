import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { 
  Cliente, 
  CriarClienteRequest, 
  AtualizarClienteRequest, 
  TransacaoRequest,
  Result,
  ResultVoid
} from '../models/cliente.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {
  private readonly apiUrl = `${environment.apiUrl}/api/clientes`;

  constructor(private http: HttpClient) {}

  obterTodos(): Observable<Cliente[]> {
    return this.http.get<any>(this.apiUrl).pipe(
      map(response => {
        // Se a API retornar um array diretamente
        if (Array.isArray(response)) {
          return response;
        }
        // Se a API retornar Result<Cliente[]>
        if (response && response.data && Array.isArray(response.data)) {
          return response.data;
        }
        // Se a API retornar apenas o data como array
        if (response && Array.isArray(response.data)) {
          return response.data;
        }
        // Fallback: retorna array vazio
        console.warn('Formato de resposta inesperado:', response);
        return [];
      }),
      catchError(this.handleError)
    );
  }

  obterPorId(id: number): Observable<Result<Cliente>> {
    return this.http.get<Result<Cliente>>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  criar(request: CriarClienteRequest): Observable<Result<Cliente>> {
    return this.http.post<Result<Cliente>>(this.apiUrl, request).pipe(
      catchError(this.handleError)
    );
  }

  atualizar(id: number, request: AtualizarClienteRequest): Observable<Result<Cliente>> {
    return this.http.put<Result<Cliente>>(`${this.apiUrl}/${id}`, request).pipe(
      catchError(this.handleError)
    );
  }

  deletar(id: number): Observable<ResultVoid> {
    return this.http.delete<ResultVoid>(`${this.apiUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  depositar(id: number, request: TransacaoRequest): Observable<Result<Cliente>> {
    return this.http.post<Result<Cliente>>(`${this.apiUrl}/${id}/depositar`, request).pipe(
      catchError(this.handleError)
    );
  }

  sacar(id: number, request: TransacaoRequest): Observable<Result<Cliente>> {
    return this.http.post<Result<Cliente>>(`${this.apiUrl}/${id}/sacar`, request).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Ocorreu um erro na comunicação com o servidor';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Erro: ${error.error.message}`;
    } else if (error.error) {
      // Backend returned Result with error
      if (error.error.errorDescription) {
        errorMessage = error.error.errorDescription;
      } else if (error.error.message) {
        errorMessage = error.error.message;
      } else if (typeof error.error === 'string') {
        errorMessage = error.error;
      }
    }
    
    return throwError(() => ({
      status: error.status,
      message: errorMessage,
      error: error.error
    }));
  }
}
