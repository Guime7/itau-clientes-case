import { describe, it, expect, beforeEach, vi } from 'vitest';
import { TestBed } from '@angular/core/testing';
import { HttpRequest, HttpHandlerFn, HttpResponse } from '@angular/common/http';
import { of } from 'rxjs';
import { authInterceptor } from './auth.interceptor';
import { AuthService } from '../services/auth.service';

describe('authInterceptor', () => {
  let authService: AuthService;
  let mockHandler: HttpHandlerFn;

  beforeEach(() => {
    const authServiceMock = {
      getToken: vi.fn()
    };

    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: authServiceMock }
      ]
    });

    authService = TestBed.inject(AuthService);
    
    mockHandler = vi.fn(() => of(new HttpResponse({ status: 200 })));
  });

  it('deve adicionar token de autenticação quando disponível', () => {
    const token = 'test-token-123';
    vi.spyOn(authService, 'getToken').mockReturnValue(token);

    const request = new HttpRequest('GET', '/api/test');

    TestBed.runInInjectionContext(() => {
      authInterceptor(request, mockHandler);
    });

    expect(mockHandler).toHaveBeenCalledWith(
      expect.objectContaining({
        headers: expect.objectContaining({
          lazyUpdate: expect.arrayContaining([
            expect.objectContaining({
              name: 'Authorization',
              value: `Bearer ${token}`
            })
          ])
        })
      })
    );
  });

  it('deve passar requisição sem modificação quando não há token', () => {
    vi.spyOn(authService, 'getToken').mockReturnValue(null);

    const request = new HttpRequest('GET', '/api/test');

    TestBed.runInInjectionContext(() => {
      authInterceptor(request, mockHandler);
    });

    expect(mockHandler).toHaveBeenCalledWith(request);
  });

  it('deve preservar headers existentes', () => {
    const token = 'test-token-123';
    vi.spyOn(authService, 'getToken').mockReturnValue(token);

    const request = new HttpRequest('GET', '/api/test', {
      headers: { 'Content-Type': 'application/json' }
    });

    TestBed.runInInjectionContext(() => {
      authInterceptor(request, mockHandler);
    });

    expect(mockHandler).toHaveBeenCalled();
    const calledRequest = (mockHandler as any).mock.calls[0][0];
    expect(calledRequest.headers.has('Content-Type')).toBe(true);
    expect(calledRequest.headers.has('Authorization')).toBe(true);
  });
});
