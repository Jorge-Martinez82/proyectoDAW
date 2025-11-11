import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BotonVolver } from './boton-volver';

describe('BotonVolver', () => {
  let component: BotonVolver;
  let fixture: ComponentFixture<BotonVolver>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BotonVolver]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BotonVolver);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
