// import { describe, it, expect, beforeEach, vi, afterEach } from 'vitest';
// import { TestBed } from '@angular/core/testing';
// import { HttpClientTestingModule } from '@angular/common/http/testing';
// import { AuthService } from './auth.service';
// import { LoginRequest } from '../models/auth.model';

// describe('AuthService', () => {
//   let service: AuthService;
//   let localStorageMock: { [key: string]: string } = {};

//   beforeEach(() => {
//     // Mock localStorage
//     localStorageMock = {};
    
//     global.Storage.prototype.getItem = vi.fn((key: string) => localStorageMock[key] || null);
//     global.Storage.prototype.setItem = vi.fn((key: string, value: string) => {
//       localStorageMock[key] = value;
//     });
//     global.Storage.prototype.removeItem = vi.fn((key: string) => {
//       delete localStorageMock[key];
//     });

//     TestBed.configureTestingModule({
//       imports: [HttpClientTestingModule],
//       providers: [AuthService]
//     });
//     service = TestBed.inject(AuthService);
//   });

//   afterEach(() => {
//     vi.clearAllMocks();
//   });

//   describe('login', () => {
//     it('deve fazer login com sucesso', (done) => {
//       const credentials: LoginRequest = {
//         email: 'test@example.com',
//         password: 'password123'
//       };

//       service.login(credentials).subscribe(response => {
//         expect(response.email).toBe(credentials.email);
//         expect(response.token).toContain('mock-jwt-token-');
//         expect(localStorageMock['auth_token']).toBeTruthy();
//         expect(localStorageMock['user_email']).toBe(credentials.email);
//         done();
//       });
//     });

//     it('deve atualizar currentUser$ após login', (done) => {
//       const credentials: LoginRequest = {
//         email: 'test@example.com',
//         password: 'password123'
//       };

//       service.login(credentials).subscribe(() => {
//         service.currentUser$.subscribe(user => {
//           expect(user?.email).toBe(credentials.email);
//           done();
//         });
//       });
//     });
//   });

//   describe('logout', () => {
//     it('deve limpar token e user do localStorage', () => {
//       localStorageMock['auth_token'] = 'test-token';
//       localStorageMock['user_email'] = 'test@example.com';

//       service.logout();

//       expect(localStorageMock['auth_token']).toBeUndefined();
//       expect(localStorageMock['user_email']).toBeUndefined();
//     });

//     it('deve limpar currentUser$ após logout', (done) => {
//       localStorageMock['auth_token'] = 'test-token';
//       localStorageMock['user_email'] = 'test@example.com';

//       service.logout();

//       service.currentUser$.subscribe(user => {
//         expect(user).toBeNull();
//         done();
//       });
//     });
//   });

//   describe('getToken', () => {
//     it('deve retornar token do localStorage', () => {
//       localStorageMock['auth_token'] = 'test-token';
//       const token = service.getToken();
//       expect(token).toBe('test-token');
//     });

//     it('deve retornar null quando não há token', () => {
//       const token = service.getToken();
//       expect(token).toBeNull();
//     });
//   });

//   describe('isAuthenticated', () => {
//     it('deve retornar true quando há token', () => {
//       localStorageMock['auth_token'] = 'test-token';
//       expect(service.isAuthenticated()).toBe(true);
//     });

//     it('deve retornar false quando não há token', () => {
//       expect(service.isAuthenticated()).toBe(false);
//     });
//   });

//   describe('getCurrentUser', () => {
//     it('deve retornar usuário do localStorage ao iniciar', () => {
//       localStorageMock['user_email'] = 'existing@example.com';
      
//       // Reiniciar service para pegar o usuário do localStorage
//       const newService = TestBed.inject(AuthService);
      
//       newService.currentUser$.subscribe(user => {
//         expect(user?.email).toBe('existing@example.com');
//       });
//     });

//     it('deve retornar null quando não há usuário', () => {
//       service.currentUser$.subscribe(user => {
//         expect(user).toBeNull();
//       });
//     });
//   });
// });
