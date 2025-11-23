import { describe, it, expect, beforeEach, vi } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of, throwError, BehaviorSubject } from 'rxjs';
import { DashboardComponent } from './dashboard.component';
import { ClienteService } from '../../services/cliente.service';
import { AuthService } from '../../services/auth.service';
import { Cliente, Result, ResultVoid } from '../../models/cliente.model';

describe('DashboardComponent', () => {
  let component: DashboardComponent;
  let fixture: ComponentFixture<DashboardComponent>;
  let clienteService: ClienteService;
  let authService: AuthService;
  let router: Router;

  const mockCliente: Cliente = {
    id: 1,
    nome: 'João Silva',
    email: 'joao@example.com',
    saldo: 1000,
    dataCriacao: '2024-01-01T00:00:00Z'
  };

  const currentUserSubject = new BehaviorSubject({ email: 'test@example.com' });

  beforeEach(async () => {
    const clienteServiceMock = {
      obterTodos: vi.fn(),
      obterPorId: vi.fn(),
      criar: vi.fn(),
      atualizar: vi.fn(),
      deletar: vi.fn(),
      depositar: vi.fn(),
      sacar: vi.fn()
    };

    const authServiceMock = {
      currentUser$: currentUserSubject.asObservable(),
      logout: vi.fn()
    };

    const routerMock = {
      navigate: vi.fn()
    };

    await TestBed.configureTestingModule({
      imports: [DashboardComponent],
      providers: [
        { provide: ClienteService, useValue: clienteServiceMock },
        { provide: AuthService, useValue: authServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(DashboardComponent);
    component = fixture.componentInstance;
    clienteService = TestBed.inject(ClienteService);
    authService = TestBed.inject(AuthService);
    router = TestBed.inject(Router);
  });

  it('deve criar o componente', () => {
    expect(component).toBeTruthy();
  });

  describe('ngOnInit', () => {
    it('deve carregar clientes ao inicializar', () => {
      vi.spyOn(clienteService, 'obterTodos').mockReturnValue(of([mockCliente]));
      
      fixture.detectChanges();
      
      expect(clienteService.obterTodos).toHaveBeenCalled();
      expect(component.clientes).toEqual([mockCliente]);
    });

    it('deve definir userEmail do currentUser', () => {
      vi.spyOn(clienteService, 'obterTodos').mockReturnValue(of([]));
      
      fixture.detectChanges();
      
      expect(component.userEmail).toBe('test@example.com');
    });
  });

  describe('carregarClientes', () => {
    it('deve carregar lista de clientes com sucesso', () => {
      vi.spyOn(clienteService, 'obterTodos').mockReturnValue(of([mockCliente]));
      
      component.carregarClientes();
      
      expect(component.loading).toBe(false);
      expect(component.clientes).toEqual([mockCliente]);
      expect(component.error).toBe('');
    });

    it('deve tratar erro ao carregar clientes', () => {
      const errorMessage = 'Erro ao buscar clientes';
      vi.spyOn(clienteService, 'obterTodos').mockReturnValue(
        throwError(() => ({ message: errorMessage }))
      );
      
      component.carregarClientes();
      
      expect(component.loading).toBe(false);
      expect(component.error).toBe(errorMessage);
    });

    it('deve limpar mensagens ao carregar', () => {
      component.error = 'erro anterior';
      component.successMessage = 'sucesso anterior';
      vi.spyOn(clienteService, 'obterTodos').mockReturnValue(of([]));
      
      component.carregarClientes();
      
      expect(component.error).toBe('');
      expect(component.successMessage).toBe('');
    });
  });

  describe('onNovoCliente', () => {
    it('deve abrir modal de cliente sem cliente selecionado', () => {
      component.onNovoCliente();
      
      expect(component.isClienteModalOpen).toBe(true);
      expect(component.selectedCliente).toBeUndefined();
    });
  });

  describe('onEditar', () => {
    it('deve abrir modal de cliente com cliente selecionado', () => {
      component.onEditar(mockCliente);
      
      expect(component.isClienteModalOpen).toBe(true);
      expect(component.selectedCliente).toEqual(mockCliente);
    });
  });

  describe('onDeletar', () => {
    it('deve deletar cliente após confirmação', () => {
      const mockResult: ResultVoid = { isSuccess: true, message: 'Deletado' };
      vi.spyOn(clienteService, 'deletar').mockReturnValue(of(mockResult));
      vi.spyOn(clienteService, 'obterTodos').mockReturnValue(of([]));
      vi.spyOn(window, 'confirm').mockReturnValue(true);
      
      component.onDeletar(mockCliente);
      
      expect(clienteService.deletar).toHaveBeenCalledWith(mockCliente.id);
      expect(component.successMessage).toContain('sucesso');
    });

    it('não deve deletar se usuário cancelar confirmação', () => {
      vi.spyOn(clienteService, 'deletar');
      vi.spyOn(window, 'confirm').mockReturnValue(false);
      
      component.onDeletar(mockCliente);
      
      expect(clienteService.deletar).not.toHaveBeenCalled();
    });

    it('deve mostrar erro quando falha ao deletar', () => {
      const mockResult: ResultVoid = { 
        isSuccess: false, 
        errorDescription: 'Erro ao deletar' 
      };
      vi.spyOn(clienteService, 'deletar').mockReturnValue(of(mockResult));
      vi.spyOn(window, 'confirm').mockReturnValue(true);
      
      component.onDeletar(mockCliente);
      
      expect(component.error).toBe('Erro ao deletar');
    });
  });

  describe('onSaveCliente', () => {
    it('deve criar novo cliente', () => {
      const request = { nome: 'Novo Cliente', email: 'novo@example.com' };
      const mockResult: Result<Cliente> = { 
        isSuccess: true, 
        data: mockCliente,
        message: 'Cliente criado'
      };
      vi.spyOn(clienteService, 'criar').mockReturnValue(of(mockResult));
      vi.spyOn(clienteService, 'obterTodos').mockReturnValue(of([mockCliente]));
      
      component.onSaveCliente(request);
      
      expect(clienteService.criar).toHaveBeenCalledWith(request);
      expect(component.isClienteModalOpen).toBe(false);
      expect(component.successMessage).toContain('sucesso');
    });

    it('deve atualizar cliente existente', () => {
      const request = { 
        id: 1, 
        data: { nome: 'Nome Atualizado', email: 'atualizado@example.com' }
      };
      const mockResult: Result<Cliente> = { 
        isSuccess: true, 
        data: mockCliente,
        message: 'Cliente atualizado'
      };
      vi.spyOn(clienteService, 'atualizar').mockReturnValue(of(mockResult));
      vi.spyOn(clienteService, 'obterTodos').mockReturnValue(of([mockCliente]));
      
      component.onSaveCliente(request);
      
      expect(clienteService.atualizar).toHaveBeenCalledWith(request.id, request.data);
      expect(component.isClienteModalOpen).toBe(false);
    });

    it('deve mostrar erro no modal quando falha ao criar', () => {
      const request = { nome: 'Novo Cliente', email: 'novo@example.com' };
      const mockResult: Result<Cliente> = { 
        isSuccess: false, 
        errorDescription: 'Email já existe'
      };
      vi.spyOn(clienteService, 'criar').mockReturnValue(of(mockResult));
      
      component.onSaveCliente(request);
      
      expect(component.modalError).toBe('Email já existe');
      expect(component.isClienteModalOpen).toBe(true);
    });
  });

  describe('onSaveTransacao', () => {
    it('deve realizar depósito', () => {
      const event = { 
        clienteId: 1, 
        tipo: 'deposito' as const, 
        data: { valor: 500, descricao: 'Depósito' }
      };
      const mockResult: Result<Cliente> = { 
        isSuccess: true, 
        data: { ...mockCliente, saldo: 1500 }
      };
      vi.spyOn(clienteService, 'depositar').mockReturnValue(of(mockResult));
      vi.spyOn(clienteService, 'obterTodos').mockReturnValue(of([]));
      
      component.onSaveTransacao(event);
      
      expect(clienteService.depositar).toHaveBeenCalledWith(event.clienteId, event.data);
      expect(component.isTransacaoModalOpen).toBe(false);
    });

    it('deve realizar saque', () => {
      const event = { 
        clienteId: 1, 
        tipo: 'saque' as const, 
        data: { valor: 300, descricao: 'Saque' }
      };
      const mockResult: Result<Cliente> = { 
        isSuccess: true, 
        data: { ...mockCliente, saldo: 700 }
      };
      vi.spyOn(clienteService, 'sacar').mockReturnValue(of(mockResult));
      vi.spyOn(clienteService, 'obterTodos').mockReturnValue(of([]));
      
      component.onSaveTransacao(event);
      
      expect(clienteService.sacar).toHaveBeenCalledWith(event.clienteId, event.data);
      expect(component.isTransacaoModalOpen).toBe(false);
    });

    it('deve mostrar erro quando transação falha', () => {
      const event = { 
        clienteId: 1, 
        tipo: 'saque' as const, 
        data: { valor: 2000, descricao: 'Saque' }
      };
      const mockResult: Result<Cliente> = { 
        isSuccess: false, 
        errorDescription: 'Saldo insuficiente'
      };
      vi.spyOn(clienteService, 'sacar').mockReturnValue(of(mockResult));
      
      component.onSaveTransacao(event);
      
      expect(component.modalError).toBe('Saldo insuficiente');
    });
  });

  describe('onLogout', () => {
    it('deve fazer logout e redirecionar para login', () => {
      component.onLogout();
      
      expect(authService.logout).toHaveBeenCalled();
      expect(router.navigate).toHaveBeenCalledWith(['/login']);
    });
  });

  describe('formatarData', () => {
    it('deve formatar data corretamente', () => {
      const formatted = component.formatarData('2024-01-15T10:30:00Z');
      expect(formatted).toMatch(/\d{1,2}\/\d{1,2}\/\d{4}/);
    });
  });

  describe('formatarMoeda', () => {
    it('deve formatar moeda corretamente', () => {
      const formatted = component.formatarMoeda(1500.50);
      expect(formatted).toContain('1.500,50');
      expect(formatted).toContain('R$');
    });
  });

  describe('modal controls', () => {
    it('deve fechar modal de cliente e limpar dados', () => {
      component.selectedCliente = mockCliente;
      component.modalError = 'erro';
      
      component.onCloseClienteModal();
      
      expect(component.isClienteModalOpen).toBe(false);
      expect(component.selectedCliente).toBeUndefined();
      expect(component.modalError).toBe('');
    });

    it('deve fechar modal de transação e limpar dados', () => {
      component.selectedCliente = mockCliente;
      component.modalError = 'erro';
      
      component.onCloseTransacaoModal();
      
      expect(component.isTransacaoModalOpen).toBe(false);
      expect(component.selectedCliente).toBeUndefined();
      expect(component.modalError).toBe('');
    });
  });
});
