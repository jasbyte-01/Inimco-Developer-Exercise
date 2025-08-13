import { Component } from '@angular/core';
import { PersonFormComponent } from './person-form/person-form';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [PersonFormComponent],
  templateUrl: './app.html',
})
export class App {}
