import { Component, signal } from '@angular/core';
import {
  Form,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { LoginServices } from './Services/login.service';
import { IloginInterface } from './Interfaces/ilogin.interface';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, FormsModule, ReactiveFormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('frontend');

  loginform: FormGroup = new FormGroup({});

  constructor(private loginServicio: LoginServices) {
    this.loginform = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      contrasenia: new FormControl('', [Validators.required]),
    });
  }

  login() {
    var ilog: IloginInterface = {
      email: this.loginform.value.email,
      contrasenia: this.loginform.value.contrasenia,
    };
    this.loginServicio.login(ilog).subscribe((usuario) => {
      console.log(usuario);
    });
  }
}
