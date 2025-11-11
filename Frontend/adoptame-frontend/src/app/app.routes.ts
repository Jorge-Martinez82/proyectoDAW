import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Animales } from './pages/animales/animales';
import { Protectoras } from './pages/protectoras/protectoras';
import { About } from './pages/about/about';
import { DetalleAnimal } from './pages/detalle-animal/detalle-animal';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'animales', component: Animales },
  { path: 'detalle-animal', component: DetalleAnimal },
  { path: 'protectoras', component: Protectoras },
  { path: 'about', component: About },
  { path: '**', redirectTo: '' }
];
