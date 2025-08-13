import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Person } from '../models/person';
import { environment } from '../../../environment/environment';

@Injectable({ providedIn: 'root' })
export class PersonService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  addPerson$(person: Person) {
    return this.http.post(`${this.apiUrl}/users`, person);
  }
}
