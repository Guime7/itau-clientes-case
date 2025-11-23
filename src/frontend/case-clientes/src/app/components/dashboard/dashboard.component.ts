import { Component, OnInit } from '@angular/core';
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
  userEmail = '';

  // Modal states
  isClienteModalOpen = false;
  isTransacaoModalOpen = false;
  selectedCliente?: Cliente;
  tipoTransacao: TipoTransacao = 'deposito';

  constructor(
    private clienteService: ClienteService,
    private authService: AuthService,
    private router: Router
  ) {
    this.authService.currentUser$.subscribe(user => {
      this.userEmail = user?.email || '';
    });
  }

  ngOnInit(): void {
    this.carregarClientes();
  }

  carregarClientes(): void {
    this.loading = true;
    this.error = '';

    this.clienteService.obterTodos().subscribe({
      next: (clientes) => {
        this.clientes = clientes;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Erro ao carregar clientes';
        this.loading = false;
        console.error('Error loading clientes:', err);
      }
    });
  }

  onNovoCliente(): void {
    this.selectedCliente = undefined;
    this.isClienteModalOpen = true;
  }

  onEditar(cliente: Cliente): void {
    this.selectedCliente = cliente;
    this.isClienteModalOpen = true;
  }

  onDeletar(cliente: Cliente): void {
    if (confirm(`Tem certeza que deseja deletar o cliente ${cliente.nome}?`)) {
      this.clienteService.deletar(cliente.id).subscribe({
        next: () => {
          this.carregarClientes();
        },
        error: (err) => {
          alert('Erro ao deletar cliente');
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
    if ('id' in data) {
      // Atualizar
      this.clienteService.atualizar(data.id, data.data).subscribe({
        next: () => {
          this.isClienteModalOpen = false;
          this.carregarClientes();
        },
        error: (err) => {
          alert('Erro ao atualizar cliente');
          console.error('Error updating cliente:', err);
        }
      });
    } else {
      // Criar
      this.clienteService.criar(data).subscribe({
        next: () => {
          this.isClienteModalOpen = false;
          this.carregarClientes();
        },
        error: (err) => {
          alert('Erro ao criar cliente');
          console.error('Error creating cliente:', err);
        }
      });
    }
  }

  onSaveTransacao(event: { clienteId: number; tipo: TipoTransacao; data: TransacaoRequest }): void {
    const observable = event.tipo === 'deposito'
      ? this.clienteService.depositar(event.clienteId, event.data)
      : this.clienteService.sacar(event.clienteId, event.data);

    observable.subscribe({
      next: () => {
        this.isTransacaoModalOpen = false;
        this.carregarClientes();
      },
      error: (err) => {
        alert(`Erro ao realizar ${event.tipo}`);
        console.error(`Error ${event.tipo}:`, err);
      }
    });
  }

  onCloseClienteModal(): void {
    this.isClienteModalOpen = false;
    this.selectedCliente = undefined;
  }

  onCloseTransacaoModal(): void {
    this.isTransacaoModalOpen = false;
    this.selectedCliente = undefined;
  }
}
