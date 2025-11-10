import { Component } from '@angular/core';

@Component({
  selector: 'app-about',
  imports: [],
  templateUrl: './about.html',
  styleUrl: './about.css',
})
export class About {

  onContactSubmit(event: any) {
    event.preventDefault();
    alert('Mensaje enviado');
    event.target.reset();
  }

}
