import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FormularioAdopcion } from './formulario-adopcion';

describe('FormularioAdopcion', () => {
  let component: FormularioAdopcion;
  let fixture: ComponentFixture<FormularioAdopcion>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormularioAdopcion]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormularioAdopcion);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
