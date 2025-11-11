// home.component.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class Home {
  searchForm = {
    tipo: 'todos',
    provincia: 'todos'
  };

  constructor(private router: Router) { }

  onSearchSubmit(): void {
    this.router.navigate(['/animales'], {
      queryParams: {
        tipo: this.searchForm.tipo,
        provincia: this.searchForm.provincia
      }
    });
  }
}
