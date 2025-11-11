import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleProtectora } from './detalle-protectora';

describe('DetalleProtectora', () => {
  let component: DetalleProtectora;
  let fixture: ComponentFixture<DetalleProtectora>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetalleProtectora]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetalleProtectora);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
