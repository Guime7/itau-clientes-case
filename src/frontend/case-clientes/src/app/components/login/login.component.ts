import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  tipoUsuario = 'Admin'; // 'Admin' ou 'Cliente'
  clienteId: number | null = null;
  email = '';
  senha = '';
  loading = false;
  error = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onLogin(): void {
    // Validação: se Cliente, precisa informar o ClienteId
    if (this.tipoUsuario === 'Cliente' && !this.clienteId) {
      this.error = 'Por favor, informe o ID do cliente';
      return;
    }

    this.loading = true;
    this.error = '';

    this.authService.login({
      tipoUsuario: this.tipoUsuario,
      clienteId: this.clienteId ?? undefined,
      email: this.email || undefined,
      senha: this.senha || undefined
    }).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Erro ao fazer login. Tente novamente.';
        this.loading = false;
        console.error('Login error:', err);
      },
      complete: () => {
        this.loading = false;
      }
    });
  }
}
