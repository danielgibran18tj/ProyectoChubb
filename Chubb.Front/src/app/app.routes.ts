import { Routes } from '@angular/router';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'seguros',
        pathMatch: 'full'
    },
    {
        path: 'seguros',
        loadComponent: () => import('./features/seguros/seguros.component').then(m => m.SegurosComponent)
    },
    {
        path: 'asegurados',
        loadComponent: () => import('./features/asegurados/asegurados.component').then(m => m.AseguradosComponent)
    },
    {
        path: 'consultas',
        loadComponent: () => import('./features/consultas/consultas.component').then(m => m.ConsultasComponent)
    },
    {
        path: 'carga-masiva',
        loadComponent: () => import('./features/carga-masiva/carga-masiva.component').then(m => m.CargaMasivaComponent)
    },
    {
        path: '**',
        redirectTo: 'seguros'
    }
];
