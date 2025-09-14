import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterOutlet } from '@angular/router';
import Swal from 'sweetalert2';
import { LoginService } from '../../service/login.service';
import { ILoginInterface } from '../../Interface/ilogin.interface';

@Component({
  selector: 'app-login.component',
  imports: [RouterOutlet, ReactiveFormsModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loginform: FormGroup = new FormGroup({});

  constructor(private loginServicio: LoginService, private rutas: Router) {
    this.loginform = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      contrasenia: new FormControl('', [Validators.required]),
    });
  }

  login() {
    const login: ILoginInterface = {
      email: this.loginform.value.email,
      contrasenia: this.loginform.value.contrasenia,
    };
    this.loginServicio.login(login).subscribe({
      next: (usuario) => {
        Swal.fire('Sistemas de XXX', `Bienvenido ${usuario.nombre} ${usuario.apellido}`, 'success');
        localStorage.setItem('usuario', JSON.stringify(usuario));
        this.rutas.navigate(['/dashboard']);
      },
      error: (error) => {
        console.log(error);
        if (error.status === 401) {
          Swal.fire('Sistema de XXX', error.error.mensaje, 'question');
        } else {
          Swal.fire('Sistemas de XXX', 'Ocurrio un error inesperado', 'error');
        }
      },
    });
  }
}
