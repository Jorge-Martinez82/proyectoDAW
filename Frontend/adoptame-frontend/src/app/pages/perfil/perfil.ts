import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

interface Usuario {
  nombre: string;
  apellidos: string;
  ciudad: string;
  provincia: string;
  correo: string;
  tipo: 'adoptante' | 'protectora';
}

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.html',
  imports: [CommonModule]
})
export class Perfil implements OnInit {

  pestanaSeleccionada: string = 'datosPersonales';
  usuario: Usuario | null = null;

  constructor(private router: Router) { }

  ngOnInit(): void {

    this.usuario = {
      nombre: 'Juan',
      apellidos: 'Martínez',
      ciudad: 'Bilbao',
      provincia: 'Bizkaia',
      correo: 'juanmartinez@example.com',
      tipo: 'adoptante'  
    };
  }

  seleccionarPestana(pestana: string): void {

    this.pestanaSeleccionada = pestana;
  }

  actualizarDatos(): void {
    alert('Datos actualizados (simulado)');
  }
}
