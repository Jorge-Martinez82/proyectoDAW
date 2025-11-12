import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormularioBusqueda } from './formulario-busqueda';

describe('FormularioBusqueda', () => {
  let component: FormularioBusqueda;
  let fixture: ComponentFixture<FormularioBusqueda>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormularioBusqueda]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormularioBusqueda);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
