import { Component, OnInit } from '@angular/core';
import { IUsuarioInterface } from '../../Interface/iusuario.interface';
import { LoginService } from '../../service/login.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard.component',
  imports: [FormsModule, ReactiveFormsModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export class DashboardComponent implements OnInit {
  lista_usaurio: IUsuarioInterface[] = [];

  constructor(private usuariosServicio: LoginService) {}

  ngOnInit(): void {
    this.usuariosServicio.todos_usuarios().subscribe((lista) => {
      console.table(lista);
      this.lista_usaurio = lista;
    });
  }
  imprimir() {
    const html = document.getElementById('area_imprimir')?.innerHTML;
    const ventana = window.open('', '', 'height=600, width=900');
    ventana?.document.open();
    ventana?.document.write(
      `<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <title>Nuevo</title>
  <base href="/">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="icon" type="image/x-icon" href="favicon.ico">
  <style>
@media print{
  button{
    display: none;
  }
}
@page{
  size: A4 portrait;
  margin: 12mm
}
  </style>
</head>
<body onload="window.print(); window.close();">
${html}
  
</body>
</html>`
    );
  }
}
