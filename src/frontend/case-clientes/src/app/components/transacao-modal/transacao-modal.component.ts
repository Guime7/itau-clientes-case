import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModalComponent } from '../modal/modal.component';
import { Cliente, TransacaoRequest } from '../../models/cliente.model';

export type TipoTransacao = 'deposito' | 'saque';

@Component({
  selector: 'app-transacao-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ModalComponent],
  templateUrl: './transacao-modal.component.html',
  styleUrls: ['./transacao-modal.component.css']
})
export class TransacaoModalComponent {
  @Input() isOpen = false;
  @Input() cliente?: Cliente;
  @Input() tipo: TipoTransacao = 'deposito';
  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<{ clienteId: number; tipo: TipoTransacao; data: TransacaoRequest }>();

  valor = 0;
  descricao = '';
  loading = false;

  get title(): string {
    return this.tipo === 'deposito' ? 'Depositar Saldo' : 'Sacar Saldo';
  }

  get buttonText(): string {
    return this.tipo === 'deposito' ? 'Depositar' : 'Sacar';
  }

  get icon(): string {
    return this.tipo === 'deposito' ? '💰' : '💸';
  }

  onClose(): void {
    this.resetForm();
    this.close.emit();
  }

  onSubmit(): void {
    if (!this.cliente) return;

    if (this.valor <= 0) {
      alert('Por favor, informe um valor maior que zero');
      return;
    }

    if (!this.descricao.trim()) {
      alert('Por favor, informe uma descrição');
      return;
    }

    this.loading = true;

    const data: TransacaoRequest = {
      valor: this.valor,
      descricao: this.descricao
    };

    this.save.emit({
      clienteId: this.cliente.id,
      tipo: this.tipo,
      data
    });
  }

  formatarMoeda(valor: number): string {
    return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
  }

  private resetForm(): void {
    this.valor = 0;
    this.descricao = '';
    this.loading = false;
  }
}
