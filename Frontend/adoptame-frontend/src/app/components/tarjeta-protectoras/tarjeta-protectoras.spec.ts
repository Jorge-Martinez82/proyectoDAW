import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TarjetaProtectoras } from './tarjeta-protectoras';

describe('TarjetaProtectoras', () => {
  let component: TarjetaProtectoras;
  let fixture: ComponentFixture<TarjetaProtectoras>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TarjetaProtectoras]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TarjetaProtectoras);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
