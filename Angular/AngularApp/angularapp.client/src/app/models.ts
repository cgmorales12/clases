export interface Event {
  eventoId: number;
  nombre: string;
  fecha: string;
  ubicacion: string;
  descripcion?: string | null;
  inscripciones?: RegistrationDetail[];
}

export interface Participant {
  participanteId: number;
  nombre: string;
  apellido: string;
  email: string;
  telefono?: string | null;
  inscripciones?: RegistrationDetail[];
}

export interface RegistrationDetail {
  inscripcionId: number;
  eventoId: number;
  eventoNombre: string;
  participanteId: number;
  participanteNombre: string;
  participanteEmail: string;
  fechaInscripcion: string;
}

export interface RegistrationRequest {
  eventoId: number;
  participanteId: number;
}
