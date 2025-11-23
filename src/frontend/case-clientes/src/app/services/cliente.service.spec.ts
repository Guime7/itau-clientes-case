// import { describe, it, expect, beforeEach, vi } from 'vitest';
// import { TestBed } from '@angular/core/testing';
// import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
// import { ClienteService } from './cliente.service';
// import { Cliente, Result, ResultVoid, CriarClienteRequest, AtualizarClienteRequest, TransacaoRequest } from '../models/cliente.model';
// import { environment } from '../../environments/environment';

// describe('ClienteService', () => {
//   let service: ClienteService;
//   let httpMock: HttpTestingController;
//   const apiUrl = `${environment.apiUrl}/api/clientes`;

//   const mockCliente: Cliente = {
//     id: 1,
//     nome: 'João Silva',
//     email: 'joao@example.com',
//     saldo: 1000,
//     dataCriacao: '2024-01-01T00:00:00Z',
//     dataAtualizacao: '2024-01-02T00:00:00Z'
//   };

//   beforeEach(() => {
//     TestBed.configureTestingModule({
//       imports: [HttpClientTestingModule],
//       providers: [ClienteService]
//     });
//     service = TestBed.inject(ClienteService);
//     httpMock = TestBed.inject(HttpTestingController);
//   });

//   afterEach(() => {
//     httpMock.verify();
//   });

//   describe('obterTodos', () => {
//     it('deve retornar lista de clientes', () => {
//       const mockClientes: Cliente[] = [mockCliente];

//       service.obterTodos().subscribe(clientes => {
//         expect(clientes).toEqual(mockClientes);
//         expect(clientes.length).toBe(1);
//       });

//       const req = httpMock.expectOne(apiUrl);
//       expect(req.request.method).toBe('GET');
//       req.flush(mockClientes);
//     });

//     it('deve tratar erro ao obter todos os clientes', () => {
//       const errorMessage = 'Erro ao carregar clientes';

//       service.obterTodos().subscribe({
//         next: () => fail('deveria ter falhado'),
//         error: (error) => {
//           expect(error.message).toBe(errorMessage);
//           expect(error.status).toBe(500);
//         }
//       });

//       const req = httpMock.expectOne(apiUrl);
//       req.flush({ errorDescription: errorMessage }, { status: 500, statusText: 'Server Error' });
//     });
//   });

//   describe('obterPorId', () => {
//     it('deve retornar um cliente por ID', () => {
//       const mockResult: Result<Cliente> = {
//         isSuccess: true,
//         data: mockCliente,
//         message: 'Cliente encontrado'
//       };

//       service.obterPorId(1).subscribe(result => {
//         expect(result.isSuccess).toBe(true);
//         expect(result.data).toEqual(mockCliente);
//       });

//       const req = httpMock.expectOne(`${apiUrl}/1`);
//       expect(req.request.method).toBe('GET');
//       req.flush(mockResult);
//     });

//     it('deve retornar erro quando cliente não existe', () => {
//       const mockResult: Result<Cliente> = {
//         isSuccess: false,
//         errorCode: 'NotFound',
//         errorDescription: 'Cliente não encontrado'
//       };

//       service.obterPorId(999).subscribe(result => {
//         expect(result.isSuccess).toBe(false);
//         expect(result.errorCode).toBe('NotFound');
//       });

//       const req = httpMock.expectOne(`${apiUrl}/999`);
//       req.flush(mockResult, { status: 404, statusText: 'Not Found' });
//     });
//   });

//   describe('criar', () => {
//     it('deve criar um novo cliente', () => {
//       const request: CriarClienteRequest = {
//         nome: 'João Silva',
//         email: 'joao@example.com'
//       };

//       const mockResult: Result<Cliente> = {
//         isSuccess: true,
//         data: mockCliente,
//         message: 'Cliente criado com sucesso'
//       };

//       service.criar(request).subscribe(result => {
//         expect(result.isSuccess).toBe(true);
//         expect(result.data).toEqual(mockCliente);
//       });

//       const req = httpMock.expectOne(apiUrl);
//       expect(req.request.method).toBe('POST');
//       expect(req.request.body).toEqual(request);
//       req.flush(mockResult);
//     });

//     it('deve retornar erro de validação', () => {
//       const request: CriarClienteRequest = {
//         nome: '',
//         email: 'invalid'
//       };

//       const errorMessage = 'Nome e email são obrigatórios';

//       service.criar(request).subscribe({
//         next: () => fail('deveria ter falhado'),
//         error: (error) => {
//           expect(error.message).toBe(errorMessage);
//           expect(error.status).toBe(400);
//         }
//       });

//       const req = httpMock.expectOne(apiUrl);
//       req.flush({ errorDescription: errorMessage }, { status: 400, statusText: 'Bad Request' });
//     });

//     it('deve retornar erro de conflito quando email já existe', () => {
//       const request: CriarClienteRequest = {
//         nome: 'João Silva',
//         email: 'joao@example.com'
//       };

//       const errorMessage = 'Email já cadastrado';

//       service.criar(request).subscribe({
//         next: () => fail('deveria ter falhado'),
//         error: (error) => {
//           expect(error.message).toBe(errorMessage);
//           expect(error.status).toBe(409);
//         }
//       });

//       const req = httpMock.expectOne(apiUrl);
//       req.flush({ errorDescription: errorMessage }, { status: 409, statusText: 'Conflict' });
//     });
//   });

//   describe('atualizar', () => {
//     it('deve atualizar um cliente existente', () => {
//       const request: AtualizarClienteRequest = {
//         nome: 'João Silva Updated',
//         email: 'joao.updated@example.com'
//       };

//       const updatedCliente = { ...mockCliente, ...request };
//       const mockResult: Result<Cliente> = {
//         isSuccess: true,
//         data: updatedCliente,
//         message: 'Cliente atualizado com sucesso'
//       };

//       service.atualizar(1, request).subscribe(result => {
//         expect(result.isSuccess).toBe(true);
//         expect(result.data?.nome).toBe(request.nome);
//       });

//       const req = httpMock.expectOne(`${apiUrl}/1`);
//       expect(req.request.method).toBe('PUT');
//       expect(req.request.body).toEqual(request);
//       req.flush(mockResult);
//     });

//     it('deve retornar erro quando cliente não existe', () => {
//       const request: AtualizarClienteRequest = {
//         nome: 'João Silva',
//         email: 'joao@example.com'
//       };

//       const errorMessage = 'Cliente não encontrado';

//       service.atualizar(999, request).subscribe({
//         next: () => fail('deveria ter falhado'),
//         error: (error) => {
//           expect(error.message).toBe(errorMessage);
//           expect(error.status).toBe(404);
//         }
//       });

//       const req = httpMock.expectOne(`${apiUrl}/999`);
//       req.flush({ errorDescription: errorMessage }, { status: 404, statusText: 'Not Found' });
//     });
//   });

//   describe('deletar', () => {
//     it('deve deletar um cliente', () => {
//       const mockResult: ResultVoid = {
//         isSuccess: true,
//         message: 'Cliente deletado com sucesso'
//       };

//       service.deletar(1).subscribe(result => {
//         expect(result.isSuccess).toBe(true);
//       });

//       const req = httpMock.expectOne(`${apiUrl}/1`);
//       expect(req.request.method).toBe('DELETE');
//       req.flush(mockResult);
//     });

//     it('deve retornar erro quando cliente não existe', () => {
//       const errorMessage = 'Cliente não encontrado';

//       service.deletar(999).subscribe({
//         next: () => fail('deveria ter falhado'),
//         error: (error) => {
//           expect(error.message).toBe(errorMessage);
//           expect(error.status).toBe(404);
//         }
//       });

//       const req = httpMock.expectOne(`${apiUrl}/999`);
//       req.flush({ errorDescription: errorMessage }, { status: 404, statusText: 'Not Found' });
//     });
//   });

//   describe('depositar', () => {
//     it('deve depositar valor na conta do cliente', () => {
//       const request: TransacaoRequest = {
//         valor: 500,
//         descricao: 'Depósito de salário'
//       };

//       const updatedCliente = { ...mockCliente, saldo: 1500 };
//       const mockResult: Result<Cliente> = {
//         isSuccess: true,
//         data: updatedCliente,
//         message: 'Depósito realizado com sucesso'
//       };

//       service.depositar(1, request).subscribe(result => {
//         expect(result.isSuccess).toBe(true);
//         expect(result.data?.saldo).toBe(1500);
//       });

//       const req = httpMock.expectOne(`${apiUrl}/1/depositar`);
//       expect(req.request.method).toBe('POST');
//       expect(req.request.body).toEqual(request);
//       req.flush(mockResult);
//     });

//     it('deve retornar erro ao depositar valor negativo', () => {
//       const request: TransacaoRequest = {
//         valor: -100,
//         descricao: 'Depósito inválido'
//       };

//       const errorMessage = 'Valor deve ser maior que zero';

//       service.depositar(1, request).subscribe({
//         next: () => fail('deveria ter falhado'),
//         error: (error) => {
//           expect(error.message).toBe(errorMessage);
//           expect(error.status).toBe(400);
//         }
//       });

//       const req = httpMock.expectOne(`${apiUrl}/1/depositar`);
//       req.flush({ errorDescription: errorMessage }, { status: 400, statusText: 'Bad Request' });
//     });
//   });

//   describe('sacar', () => {
//     it('deve sacar valor da conta do cliente', () => {
//       const request: TransacaoRequest = {
//         valor: 300,
//         descricao: 'Saque no caixa'
//       };

//       const updatedCliente = { ...mockCliente, saldo: 700 };
//       const mockResult: Result<Cliente> = {
//         isSuccess: true,
//         data: updatedCliente,
//         message: 'Saque realizado com sucesso'
//       };

//       service.sacar(1, request).subscribe(result => {
//         expect(result.isSuccess).toBe(true);
//         expect(result.data?.saldo).toBe(700);
//       });

//       const req = httpMock.expectOne(`${apiUrl}/1/sacar`);
//       expect(req.request.method).toBe('POST');
//       expect(req.request.body).toEqual(request);
//       req.flush(mockResult);
//     });

//     it('deve retornar erro ao sacar valor maior que saldo', () => {
//       const request: TransacaoRequest = {
//         valor: 2000,
//         descricao: 'Saque além do saldo'
//       };

//       const errorMessage = 'Saldo insuficiente';

//       service.sacar(1, request).subscribe({
//         next: () => fail('deveria ter falhado'),
//         error: (error) => {
//           expect(error.message).toBe(errorMessage);
//           expect(error.status).toBe(400);
//         }
//       });

//       const req = httpMock.expectOne(`${apiUrl}/1/sacar`);
//       req.flush({ errorDescription: errorMessage }, { status: 400, statusText: 'Bad Request' });
//     });

//     it('deve retornar erro quando cliente não existe', () => {
//       const request: TransacaoRequest = {
//         valor: 100,
//         descricao: 'Saque'
//       };

//       const errorMessage = 'Cliente não encontrado';

//       service.sacar(999, request).subscribe({
//         next: () => fail('deveria ter falhado'),
//         error: (error) => {
//           expect(error.message).toBe(errorMessage);
//           expect(error.status).toBe(404);
//         }
//       });

//       const req = httpMock.expectOne(`${apiUrl}/999/sacar`);
//       req.flush({ errorDescription: errorMessage }, { status: 404, statusText: 'Not Found' });
//     });
//   });

//   describe('handleError', () => {
//     it('deve tratar erro de cliente (ErrorEvent)', () => {
//       const errorEvent = new ErrorEvent('Network error', {
//         message: 'Falha na conexão'
//       });

//       service.obterTodos().subscribe({
//         next: () => fail('deveria ter falhado'),
//         error: (error) => {
//           expect(error.message).toBe('Erro: Falha na conexão');
//         }
//       });

//       const req = httpMock.expectOne(apiUrl);
//       req.error(errorEvent);
//     });

//     it('deve tratar erro com mensagem string', () => {
//       service.obterTodos().subscribe({
//         next: () => fail('deveria ter falhado'),
//         error: (error) => {
//           expect(error.message).toBe('Erro interno do servidor');
//         }
//       });

//       const req = httpMock.expectOne(apiUrl);
//       req.flush('Erro interno do servidor', { status: 500, statusText: 'Server Error' });
//     });

//     it('deve usar mensagem padrão quando erro não tem detalhes', () => {
//       service.obterTodos().subscribe({
//         next: () => fail('deveria ter falhado'),
//         error: (error) => {
//           expect(error.message).toBe('Ocorreu um erro na comunicação com o servidor');
//         }
//       });

//       const req = httpMock.expectOne(apiUrl);
//       req.flush({}, { status: 500, statusText: 'Server Error' });
//     });
//   });
// });
