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
  email = '';
  senha = '';
  loading = false;
  error = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onLogin(): void {
    if (!this.email || !this.senha) {
      this.error = 'Por favor, preencha email e senha';
      return;
    }

    this.loading = true;
    this.error = '';

    this.authService.login({ email: this.email, senha: this.senha })
      .subscribe({
        next: () => {
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          this.error = 'Erro ao fazer login. Tente novamente.';
          this.loading = false;
          console.error('Login error:', err);
        },
        complete: () => {
          this.loading = false;
        }
      });
  }
}
