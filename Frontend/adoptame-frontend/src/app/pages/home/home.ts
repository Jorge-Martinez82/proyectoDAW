import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  imports: [CommonModule, FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  searchForm = {
    tipo: 'todos',
    provincia: 'todos'
  };

  onContactSubmit(event: any) {
    event.preventDefault();
    alert('Mensaje enviado');
    event.target.reset();
  }

  onSearchSubmit() {
    console.log('Buscando:', this.searchForm);
    // busqueda
  }

  toggleMenu() {
    const menu = document.getElementById('menu');
    if (menu) {
      menu.classList.toggle('hidden');
    }
  }
}
