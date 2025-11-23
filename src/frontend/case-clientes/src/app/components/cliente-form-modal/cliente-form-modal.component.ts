import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModalComponent } from '../modal/modal.component';
import { Cliente, CriarClienteRequest, AtualizarClienteRequest } from '../../models/cliente.model';

@Component({
  selector: 'app-cliente-form-modal',
  standalone: true,
  imports: [CommonModule, FormsModule, ModalComponent],
  templateUrl: './cliente-form-modal.component.html',
  styleUrls: ['./cliente-form-modal.component.css']
})
export class ClienteFormModalComponent implements OnChanges {
  @Input() isOpen = false;
  @Input() cliente?: Cliente;
  @Input() error = '';
  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<CriarClienteRequest | { id: number; data: AtualizarClienteRequest }>();

  nome = '';
  email = '';
  loading = false;

  get isEdit(): boolean {
    return !!this.cliente;
  }

  get title(): string {
    return this.isEdit ? 'Editar Cliente' : 'Novo Cliente';
  }

  ngOnChanges(): void {
    if (this.cliente) {
      this.nome = this.cliente.nome;
      this.email = this.cliente.email;
    } else {
      this.resetForm();
    }
  }

  onClose(): void {
    this.resetForm();
    this.close.emit();
  }

  onSubmit(): void {
    if (!this.nome.trim() || !this.email.trim()) {
      alert('Por favor, preencha todos os campos');
      return;
    }

    this.loading = true;

    if (this.isEdit && this.cliente) {
      const data: AtualizarClienteRequest = {
        nome: this.nome,
        email: this.email
      };
      this.save.emit({ id: this.cliente.id, data });
    } else {
      const data: CriarClienteRequest = {
        nome: this.nome,
        email: this.email
      };
      this.save.emit(data);
    }
  }

  private resetForm(): void {
    this.nome = '';
    this.email = '';
    this.loading = false;
  }
}
