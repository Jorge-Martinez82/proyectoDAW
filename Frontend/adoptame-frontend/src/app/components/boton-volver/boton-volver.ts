import { Component, Input } from '@angular/core';
import { Location } from '@angular/common';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-boton-volver',
  standalone: true,
  imports: [CommonModule],
  template: `
    <button 
      (click)="volver()"
      class="flex items-center gap-2 rounded-full bg-white px-4 py-2 shadow-lg hover:shadow-xl transition hover:bg-gray-50"
    >
      <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
        <path fill-rule="evenodd" d="M9.707 16.707a1 1 0 01-1.414 0l-6-6a1 1 0 010-1.414l6-6a1 1 0 011.414 1.414L5.414 9H17a1 1 0 110 2H5.414l4.293 4.293a1 1 0 010 1.414z" clip-rule="evenodd" />
      </svg>
      <span class="font-semibold">{{ texto }}</span>
    </button>
  `,
  styles: []
})
export class BotonVolver {
  @Input() texto: string = 'Volver';

  constructor(private location: Location) { }

  volver(): void {
    this.location.back();
  }
}
