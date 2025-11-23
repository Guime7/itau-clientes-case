import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ClienteService } from '../../services/cliente.service';
import { AuthService } from '../../services/auth.service';
import { Cliente, CriarClienteRequest, AtualizarClienteRequest, TransacaoRequest } from '../../models/cliente.model';
import { ClienteFormModalComponent } from '../cliente-form-modal/cliente-form-modal.component';
import { TransacaoModalComponent, TipoTransacao } from '../transacao-modal/transacao-modal.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ClienteFormModalComponent, TransacaoModalComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  clientes: Cliente[] = [];
  loading = false;
  error = '';
  successMessage = '';
  userEmail = '';

  // Modal states
  isClienteModalOpen = false;
  isTransacaoModalOpen = false;
  selectedCliente?: Cliente;
  tipoTransacao: TipoTransacao = 'deposito';
  modalError = '';

  constructor(
    private clienteService: ClienteService,
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {
    this.authService.currentUser$.subscribe(user => {
      this.userEmail = user?.email || '';
    });
  }

  ngOnInit(): void {
    console.log('DashboardComponent initialized');
    this.carregarClientes();
  }

  carregarClientes(): void {
    console.log('Carregando clientes...');
    this.loading = true;
    this.error = '';
    this.clearMessages();

    this.clienteService.obterTodos().subscribe({
      next: (clientes) => {
        console.log('Clientes recebidos:', clientes);
        console.log('É array?', Array.isArray(clientes));
        // Garante que sempre seja um array
        this.clientes = Array.isArray(clientes) ? clientes : [];
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading clientes:', err);
        this.error = err.message || 'Erro ao carregar clientes';
        this.loading = false;
        this.clientes = []; // Garante array vazio em caso de erro
        this.cdr.detectChanges();
      }
    });
  }

  private clearMessages(): void {
    this.error = '';
    this.successMessage = '';
    this.modalError = '';
  }

  private showSuccess(message: string): void {
    this.successMessage = message;
    setTimeout(() => this.successMessage = '', 5000);
  }

  private showError(message: string): void {
    this.error = message;
    setTimeout(() => this.error = '', 5000);
  }

  onNovoCliente(): void {
    console.log('Abrindo modal de novo cliente');
    this.selectedCliente = undefined;
    this.isClienteModalOpen = true;
    console.log('Modal aberto:', this.isClienteModalOpen);
    this.cdr.detectChanges();
  }

  onEditar(cliente: Cliente): void {
    this.selectedCliente = cliente;
    this.isClienteModalOpen = true;
  }

  onDeletar(cliente: Cliente): void {
    if (confirm(`Tem certeza que deseja deletar o cliente ${cliente.nome}?`)) {
      this.loading = true;
      this.clienteService.deletar(cliente.id).subscribe({
        next: (result) => {
          this.loading = false;
          if (result.isSuccess) {
            this.showSuccess('Cliente deletado com sucesso!');
            this.carregarClientes();
          } else {
            this.showError(result.errorDescription || 'Erro ao deletar cliente');
          }
        },
        error: (err) => {
          this.loading = false;
          this.showError(err.message || 'Erro ao deletar cliente');
          console.error('Error deleting cliente:', err);
        }
      });
    }
  }

  onDepositar(cliente: Cliente): void {
    this.selectedCliente = cliente;
    this.tipoTransacao = 'deposito';
    this.isTransacaoModalOpen = true;
  }

  onSacar(cliente: Cliente): void {
    this.selectedCliente = cliente;
    this.tipoTransacao = 'saque';
    this.isTransacaoModalOpen = true;
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  formatarData(data: string): string {
    return new Date(data).toLocaleDateString('pt-BR');
  }

  formatarMoeda(valor: number): string {
    return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }

  onSaveCliente(data: CriarClienteRequest | { id: number; data: AtualizarClienteRequest }): void {
    this.modalError = '';
    
    if ('id' in data) {
      // Atualizar
      this.clienteService.atualizar(data.id, data.data).subscribe({
        next: (result) => {
          if (result.isSuccess) {
            this.isClienteModalOpen = false;
            this.showSuccess(result.message || 'Cliente atualizado com sucesso!');
            this.carregarClientes();
          } else {
            this.modalError = result.errorDescription || 'Erro ao atualizar cliente';
          }
        },
        error: (err) => {
          this.modalError = err.message || 'Erro ao atualizar cliente';
          console.error('Error updating cliente:', err);
        }
      });
    } else {
      // Criar
      this.clienteService.criar(data).subscribe({
        next: (result) => {
          if (result.isSuccess) {
            this.isClienteModalOpen = false;
            this.showSuccess(result.message || 'Cliente criado com sucesso!');
            this.carregarClientes();
          } else {
            this.modalError = result.errorDescription || 'Erro ao criar cliente';
          }
        },
        error: (err) => {
          this.modalError = err.message || 'Erro ao criar cliente';
          console.error('Error creating cliente:', err);
        }
      });
    }
  }

  onSaveTransacao(event: { clienteId: number; tipo: TipoTransacao; data: TransacaoRequest }): void {
    this.modalError = '';
    
    const observable = event.tipo === 'deposito'
      ? this.clienteService.depositar(event.clienteId, event.data)
      : this.clienteService.sacar(event.clienteId, event.data);

    observable.subscribe({
      next: (result) => {
        if (result.isSuccess) {
          this.isTransacaoModalOpen = false;
          const operacao = event.tipo === 'deposito' ? 'Depósito' : 'Saque';
          this.showSuccess(result.message || `${operacao} realizado com sucesso!`);
          this.carregarClientes();
        } else {
          this.modalError = result.errorDescription || `Erro ao realizar ${event.tipo}`;
        }
      },
      error: (err) => {
        this.modalError = err.message || `Erro ao realizar ${event.tipo}`;
        console.error(`Error ${event.tipo}:`, err);
      }
    });
  }

  onCloseClienteModal(): void {
    this.isClienteModalOpen = false;
    this.selectedCliente = undefined;
    this.modalError = '';
  }

  onCloseTransacaoModal(): void {
    this.isTransacaoModalOpen = false;
    this.selectedCliente = undefined;
    this.modalError = '';
  }
}
