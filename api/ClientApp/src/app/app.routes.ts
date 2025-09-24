import { Routes } from '@angular/router';
import { AstronautDutyViewerComponent } from './components/astronaut-page/astronaut-page';

export const routes: Routes = [
  { path: '', component: AstronautDutyViewerComponent, pathMatch: 'full', title: 'Astronaut Duties' },
  { path: '**', redirectTo: '' }
];
