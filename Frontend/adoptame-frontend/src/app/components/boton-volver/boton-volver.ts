import { Component, Input } from '@angular/core';
import { Location } from '@angular/common';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-boton-volver',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './boton-volver.html',
  styleUrls: ['./boton-volver.css']
})
export class BotonVolver {
  @Input() texto: string = 'Volver';

  constructor(private location: Location) { }

  volver(): void {
    this.location.back();
  }
}
