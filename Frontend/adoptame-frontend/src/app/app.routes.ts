import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Animales } from './pages/animales/animales';
import { Protectoras } from './pages/protectoras/protectoras';
import { About } from './pages/about/about';
import { DetalleAnimal } from './pages/detalle-animal/detalle-animal';
import { FormularioAdopcion } from './pages/formulario-adopcion/formulario-adopcion';
import { Login } from './pages/login/login';
import { Registro } from './pages/registro/registro';
import { authGuard } from './guards/auth-guard';


export const routes: Routes = [
  { path: '', component: Home },
  { path: 'home', redirectTo: '', pathMatch: 'full' },
  { path: 'animales', component: Animales },
  { path: 'detalle-animal/:uuid', component: DetalleAnimal },
  { path: 'protectoras', component: Protectoras },
  { path: 'about', component: About },
  { path: 'login', component: Login },
  { path: 'registro', component: Registro },
  
  { 
    path: 'formulario-adopcion', 
    component: FormularioAdopcion,
    canActivate: [authGuard]
  },
  
  { path: '**', redirectTo: '' }
];
