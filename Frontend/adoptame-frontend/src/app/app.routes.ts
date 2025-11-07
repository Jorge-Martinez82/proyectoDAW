import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Animales } from './pages/animales/animales';
import { Protectoras } from './pages/protectoras/protectoras';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'animales', component: Animales },
  { path: 'protectoras', component: Protectoras },
  { path: '**', redirectTo: '' }
];
