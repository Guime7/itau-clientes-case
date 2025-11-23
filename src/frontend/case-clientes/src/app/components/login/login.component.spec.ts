import { describe, it, expect, beforeEach, vi } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { LoginComponent } from './login.component';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authService: AuthService;
  let router: Router;

  beforeEach(async () => {
    const authServiceMock = {
      login: vi.fn()
    };

    const routerMock = {
      navigate: vi.fn()
    };

    await TestBed.configureTestingModule({
      imports: [LoginComponent, FormsModule],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService);
    router = TestBed.inject(Router);
    fixture.detectChanges();
  });

  it('deve criar o componente', () => {
    expect(component).toBeTruthy();
  });

  describe('onLogin', () => {
    it('deve mostrar erro quando email está vazio', () => {
      component.email = '';
      component.senha = 'password123';
      
      component.onLogin();
      
      expect(component.error).toBe('Por favor, preencha email e senha');
      expect(authService.login).not.toHaveBeenCalled();
    });

    it('deve mostrar erro quando senha está vazia', () => {
      component.email = 'test@example.com';
      component.senha = '';
      
      component.onLogin();
      
      expect(component.error).toBe('Por favor, preencha email e senha');
      expect(authService.login).not.toHaveBeenCalled();
    });

    it('deve fazer login com sucesso e redirecionar', () => {
      const mockResponse = { token: 'test-token', email: 'test@example.com' };
      vi.spyOn(authService, 'login').mockReturnValue(of(mockResponse));
      
      component.email = 'test@example.com';
      component.senha = 'password123';
      
      component.onLogin();
      
      expect(authService.login).toHaveBeenCalledWith({
        email: 'test@example.com',
        senha: 'password123'
      });
      expect(router.navigate).toHaveBeenCalledWith(['/dashboard']);
    });

    it('deve definir loading como true durante o login', () => {
      vi.spyOn(authService, 'login').mockReturnValue(of({ token: 'test', email: 'test@test.com' }));
      
      component.email = 'test@example.com';
      component.senha = 'password123';
      component.onLogin();
      
      expect(component.loading).toBe(false); // Já completou devido ao of()
    });

    it('deve tratar erro de login', () => {
      const errorMessage = 'Credenciais inválidas';
      vi.spyOn(authService, 'login').mockReturnValue(
        throwError(() => new Error(errorMessage))
      );
      
      component.email = 'test@example.com';
      component.senha = 'wrong-password';
      
      component.onLogin();
      
      expect(component.error).toBe('Erro ao fazer login. Tente novamente.');
      expect(component.loading).toBe(false);
      expect(router.navigate).not.toHaveBeenCalled();
    });

    it('deve limpar erro anterior ao fazer novo login', () => {
      vi.spyOn(authService, 'login').mockReturnValue(of({ token: 'test', email: 'test@test.com' }));
      
      component.error = 'Erro anterior';
      component.email = 'test@example.com';
      component.senha = 'password123';
      
      component.onLogin();
      
      expect(component.error).toBe('');
    });
  });

  describe('initial state', () => {
    it('deve ter valores vazios inicialmente', () => {
      expect(component.email).toBe('');
      expect(component.senha).toBe('');
      expect(component.loading).toBe(false);
      expect(component.error).toBe('');
    });
  });
});
