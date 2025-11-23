import { describe, it, expect, beforeEach, vi } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { TransacaoModalComponent } from './transacao-modal.component';
import { Cliente } from '../../models/cliente.model';

describe('TransacaoModalComponent', () => {
  let component: TransacaoModalComponent;
  let fixture: ComponentFixture<TransacaoModalComponent>;

  const mockCliente: Cliente = {
    id: 1,
    nome: 'João Silva',
    email: 'joao@example.com',
    saldo: 1000,
    dataCriacao: '2024-01-01T00:00:00Z'
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TransacaoModalComponent, FormsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(TransacaoModalComponent);
    component = fixture.componentInstance;
    component.cliente = mockCliente;
    fixture.detectChanges();
  });

  it('deve criar o componente', () => {
    expect(component).toBeTruthy();
  });

  describe('title', () => {
    it('deve retornar "Depositar Saldo" para depósito', () => {
      component.tipo = 'deposito';
      expect(component.title).toBe('Depositar Saldo');
    });

    it('deve retornar "Sacar Saldo" para saque', () => {
      component.tipo = 'saque';
      expect(component.title).toBe('Sacar Saldo');
    });
  });

  describe('buttonText', () => {
    it('deve retornar "Depositar" para depósito', () => {
      component.tipo = 'deposito';
      expect(component.buttonText).toBe('Depositar');
    });

    it('deve retornar "Sacar" para saque', () => {
      component.tipo = 'saque';
      expect(component.buttonText).toBe('Sacar');
    });
  });

  describe('icon', () => {
    it('deve retornar 💰 para depósito', () => {
      component.tipo = 'deposito';
      expect(component.icon).toBe('💰');
    });

    it('deve retornar 💸 para saque', () => {
      component.tipo = 'saque';
      expect(component.icon).toBe('💸');
    });
  });

  describe('onClose', () => {
    it('deve emitir evento close e resetar form', () => {
      const closeSpy = vi.spyOn(component.close, 'emit');
      component.valor = 500;
      component.descricao = 'teste';
      
      component.onClose();
      
      expect(closeSpy).toHaveBeenCalled();
      expect(component.valor).toBe(0);
      expect(component.descricao).toBe('');
    });
  });

  describe('onSubmit', () => {
    it('não deve submeter quando cliente não está definido', () => {
      const saveSpy = vi.spyOn(component.save, 'emit');
      component.cliente = undefined;
      
      component.onSubmit();
      
      expect(saveSpy).not.toHaveBeenCalled();
    });

    it('não deve submeter quando valor é zero', () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const saveSpy = vi.spyOn(component.save, 'emit');
      
      component.valor = 0;
      component.descricao = 'teste';
      component.onSubmit();
      
      expect(alertSpy).toHaveBeenCalledWith('Por favor, informe um valor maior que zero');
      expect(saveSpy).not.toHaveBeenCalled();
    });

    it('não deve submeter quando valor é negativo', () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const saveSpy = vi.spyOn(component.save, 'emit');
      
      component.valor = -100;
      component.descricao = 'teste';
      component.onSubmit();
      
      expect(alertSpy).toHaveBeenCalledWith('Por favor, informe um valor maior que zero');
      expect(saveSpy).not.toHaveBeenCalled();
    });

    it('não deve submeter quando descrição está vazia', () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const saveSpy = vi.spyOn(component.save, 'emit');
      
      component.valor = 100;
      component.descricao = '';
      component.onSubmit();
      
      expect(alertSpy).toHaveBeenCalledWith('Por favor, informe uma descrição');
      expect(saveSpy).not.toHaveBeenCalled();
    });

    it('deve submeter transação válida', () => {
      const saveSpy = vi.spyOn(component.save, 'emit');
      component.tipo = 'deposito';
      component.valor = 500;
      component.descricao = 'Depósito de salário';
      
      component.onSubmit();
      
      expect(saveSpy).toHaveBeenCalledWith({
        clienteId: mockCliente.id,
        tipo: 'deposito',
        data: {
          valor: 500,
          descricao: 'Depósito de salário'
        }
      });
      expect(component.loading).toBe(true);
    });

    it('deve ignorar espaços em branco na descrição', () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const saveSpy = vi.spyOn(component.save, 'emit');
      
      component.valor = 100;
      component.descricao = '   ';
      component.onSubmit();
      
      expect(alertSpy).toHaveBeenCalledWith('Por favor, informe uma descrição');
      expect(saveSpy).not.toHaveBeenCalled();
    });
  });

  describe('formatarMoeda', () => {
    it('deve formatar valor corretamente', () => {
      const formatted = component.formatarMoeda(1500.50);
      expect(formatted).toContain('1.500,50');
      expect(formatted).toContain('R$');
    });

    it('deve formatar zero corretamente', () => {
      const formatted = component.formatarMoeda(0);
      expect(formatted).toContain('0,00');
    });
  });

  describe('error display', () => {
    it('deve exibir mensagem de erro quando error prop é definida', () => {
      component.error = 'Saldo insuficiente';
      fixture.detectChanges();
      
      const compiled = fixture.nativeElement as HTMLElement;
      const errorElement = compiled.querySelector('.error-alert');
      
      expect(errorElement?.textContent).toContain('Saldo insuficiente');
    });
  });
});
