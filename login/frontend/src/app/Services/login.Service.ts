import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IloginInterface } from '../Interfaces/ilogin.interface';
import { IusuarioInterface } from '../Interfaces/iusuario.interface';

@Injectable({
  providedIn: 'root',
})
export class LoginServices {
  private readonly rutaAPI = 'https://localhost:7194/api/usuarios';
  constructor(private http: HttpClient) {}

  login(login: IloginInterface): Observable<IusuarioInterface> {
    return this.http.post<IusuarioInterface>(this.rutaAPI + '/login', login);
  }
}
