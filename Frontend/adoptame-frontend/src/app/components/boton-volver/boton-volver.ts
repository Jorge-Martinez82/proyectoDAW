import { Component, Input } from '@angular/core';
import { Location } from '@angular/common';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-boton-volver',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './boton-volver.html',
  styleUrls: ['./boton-volver.css']
})
export class BotonVolver {
  @Input() texto: string = 'Volver';
  @Input() ruta?: string; 

  constructor(private location: Location, private router: Router) { }

  // navega a la ruta dada o vuelve atras
  volver(): void {
    if (this.ruta) {
      this.router.navigateByUrl(this.ruta);
    } else {
      this.location.back();
    }
  }
}
