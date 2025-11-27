import { Component } from '@angular/core';
import { BotonPrincipal } from '../../components/boton-principal/boton-principal';


@Component({
  selector: 'app-about',
  imports: [BotonPrincipal],
  templateUrl: './about.html',
  styleUrl: './about.css',
})
export class About {

  // gestiona envio del formulario de contacto simple
  onContactSubmit(event: any) {
    event.preventDefault();
    alert('Mensaje enviado');
    event.target.reset();
  }

}
