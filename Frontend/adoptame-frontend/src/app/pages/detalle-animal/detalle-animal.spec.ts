import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleAnimal } from './detalle-animal';

describe('DetalleAnimal', () => {
  let component: DetalleAnimal;
  let fixture: ComponentFixture<DetalleAnimal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetalleAnimal]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetalleAnimal);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
