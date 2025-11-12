import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BotonPrincipal } from './boton-principal';

describe('BotonPrincipal', () => {
  let component: BotonPrincipal;
  let fixture: ComponentFixture<BotonPrincipal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BotonPrincipal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BotonPrincipal);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
