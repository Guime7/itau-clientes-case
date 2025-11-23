import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LoginRequest, LoginResponse, User } from '../models/auth.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'auth_token';
  private readonly USER_KEY = 'user_email';
  private currentUserSubject = new BehaviorSubject<User | null>(this.getCurrentUser());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  // Login simulado - ajuste quando tiver endpoint real no backend
  login(credentials: LoginRequest): Observable<LoginResponse> {
    // TODO: Substituir por endpoint real quando disponível
    // return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, credentials);
    
    // Simulação temporária
    return new Observable<LoginResponse>(observer => {
      setTimeout(() => {
        const mockResponse: LoginResponse = {
          token: 'mock-jwt-token-' + Date.now(),
          email: credentials.email
        };
        observer.next(mockResponse);
        observer.complete();
      }, 500);
    }).pipe(
      tap((response: LoginResponse) => {
        this.saveToken(response.token);
        this.saveUser(response.email);
        this.currentUserSubject.next({ email: response.email });
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.USER_KEY);
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  private saveToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  private saveUser(email: string): void {
    localStorage.setItem(this.USER_KEY, email);
  }

  private getCurrentUser(): User | null {
    const email = localStorage.getItem(this.USER_KEY);
    return email ? { email } : null;
  }
}
