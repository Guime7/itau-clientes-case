import { describe, it, expect, beforeEach, vi } from 'vitest';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ModalComponent } from './modal.component';

describe('ModalComponent', () => {
  let component: ModalComponent;
  let fixture: ComponentFixture<ModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ModalComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('deve criar o componente', () => {
    expect(component).toBeTruthy();
  });

  describe('onClose', () => {
    it('deve emitir evento close', () => {
      const closeSpy = vi.spyOn(component.close, 'emit');
      
      component.onClose();
      
      expect(closeSpy).toHaveBeenCalled();
    });
  });

  describe('onBackdropClick', () => {
    it('deve fechar quando clica no backdrop', () => {
      const closeSpy = vi.spyOn(component, 'onClose');
      const event = {
        target: document.createElement('div'),
        currentTarget: document.createElement('div')
      } as any;
      
      // Simula clique no backdrop (target === currentTarget)
      event.target = event.currentTarget;
      
      component.onBackdropClick(event);
      
      expect(closeSpy).toHaveBeenCalled();
    });

    it('não deve fechar quando clica no conteúdo do modal', () => {
      const closeSpy = vi.spyOn(component, 'onClose');
      const backdrop = document.createElement('div');
      const content = document.createElement('div');
      
      const event = {
        target: content,
        currentTarget: backdrop
      } as any;
      
      component.onBackdropClick(event);
      
      expect(closeSpy).not.toHaveBeenCalled();
    });
  });

  describe('inputs', () => {
    it('deve receber isOpen como false por padrão', () => {
      expect(component.isOpen).toBe(false);
    });

    it('deve receber title vazio por padrão', () => {
      expect(component.title).toBe('');
    });

    it('deve permitir definir isOpen', () => {
      component.isOpen = true;
      fixture.detectChanges();
      
      expect(component.isOpen).toBe(true);
    });

    it('deve permitir definir title', () => {
      component.title = 'Título do Modal';
      fixture.detectChanges();
      
      expect(component.title).toBe('Título do Modal');
    });
  });
});
