import { describe, it, expect, beforeEach, vi } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { ClienteFormModalComponent } from './cliente-form-modal.component';
import { Cliente } from '../../models/cliente.model';

describe('ClienteFormModalComponent', () => {
  let component: ClienteFormModalComponent;
  let fixture: ComponentFixture<ClienteFormModalComponent>;

  const mockCliente: Cliente = {
    id: 1,
    nome: 'João Silva',
    email: 'joao@example.com',
    saldo: 1000,
    dataCriacao: '2024-01-01T00:00:00Z'
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClienteFormModalComponent, FormsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(ClienteFormModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('deve criar o componente', () => {
    expect(component).toBeTruthy();
  });

  describe('title', () => {
    it('deve retornar "Novo Cliente" quando não há cliente', () => {
      component.cliente = undefined;
      expect(component.title).toBe('Novo Cliente');
    });

    it('deve retornar "Editar Cliente" quando há cliente', () => {
      component.cliente = mockCliente;
      expect(component.title).toBe('Editar Cliente');
    });
  });

  describe('isEdit', () => {
    it('deve retornar false quando não há cliente', () => {
      component.cliente = undefined;
      expect(component.isEdit).toBe(false);
    });

    it('deve retornar true quando há cliente', () => {
      component.cliente = mockCliente;
      expect(component.isEdit).toBe(true);
    });
  });

  describe('ngOnChanges', () => {
    it('deve preencher form quando cliente é definido', () => {
      component.cliente = mockCliente;
      component.ngOnChanges();
      
      expect(component.nome).toBe(mockCliente.nome);
      expect(component.email).toBe(mockCliente.email);
    });

    it('deve resetar form quando cliente não é definido', () => {
      component.nome = 'teste';
      component.email = 'teste@test.com';
      component.cliente = undefined;
      
      component.ngOnChanges();
      
      expect(component.nome).toBe('');
      expect(component.email).toBe('');
    });
  });

  describe('onClose', () => {
    it('deve emitir evento close e resetar form', () => {
      const closeSpy = vi.spyOn(component.close, 'emit');
      component.nome = 'teste';
      component.email = 'teste@test.com';
      
      component.onClose();
      
      expect(closeSpy).toHaveBeenCalled();
      expect(component.nome).toBe('');
      expect(component.email).toBe('');
    });
  });

  describe('onSubmit', () => {
    it('não deve submeter quando nome está vazio', () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const saveSpy = vi.spyOn(component.save, 'emit');
      
      component.nome = '';
      component.email = 'test@example.com';
      component.onSubmit();
      
      expect(alertSpy).toHaveBeenCalledWith('Por favor, preencha todos os campos');
      expect(saveSpy).not.toHaveBeenCalled();
    });

    it('não deve submeter quando email está vazio', () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const saveSpy = vi.spyOn(component.save, 'emit');
      
      component.nome = 'João Silva';
      component.email = '';
      component.onSubmit();
      
      expect(alertSpy).toHaveBeenCalledWith('Por favor, preencha todos os campos');
      expect(saveSpy).not.toHaveBeenCalled();
    });

    it('deve criar novo cliente quando não está em modo edição', () => {
      const saveSpy = vi.spyOn(component.save, 'emit');
      component.cliente = undefined;
      component.nome = 'João Silva';
      component.email = 'joao@example.com';
      
      component.onSubmit();
      
      expect(saveSpy).toHaveBeenCalledWith({
        nome: 'João Silva',
        email: 'joao@example.com'
      });
      expect(component.loading).toBe(true);
    });

    it('deve atualizar cliente quando está em modo edição', () => {
      const saveSpy = vi.spyOn(component.save, 'emit');
      component.cliente = mockCliente;
      component.nome = 'João Silva Updated';
      component.email = 'joao.updated@example.com';
      
      component.onSubmit();
      
      expect(saveSpy).toHaveBeenCalledWith({
        id: mockCliente.id,
        data: {
          nome: 'João Silva Updated',
          email: 'joao.updated@example.com'
        }
      });
      expect(component.loading).toBe(true);
    });

    it('deve ignorar espaços em branco', () => {
      const alertSpy = vi.spyOn(window, 'alert').mockImplementation(() => {});
      const saveSpy = vi.spyOn(component.save, 'emit');
      
      component.nome = '   ';
      component.email = '   ';
      component.onSubmit();
      
      expect(alertSpy).toHaveBeenCalled();
      expect(saveSpy).not.toHaveBeenCalled();
    });
  });

  describe('error display', () => {
    it('deve exibir mensagem de erro quando error prop é definida', () => {
      component.error = 'Erro ao salvar cliente';
      fixture.detectChanges();
      
      const compiled = fixture.nativeElement as HTMLElement;
      const errorElement = compiled.querySelector('.error-alert');
      
      expect(errorElement?.textContent).toContain('Erro ao salvar cliente');
    });
  });
});
