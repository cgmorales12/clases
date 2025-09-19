import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EventManagementService } from './event-management.service';
import { Event, Participant, RegistrationDetail } from './models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Gestión de Eventos Deportivos';

  events: Event[] = [];
  participants: Participant[] = [];
  registrations: RegistrationDetail[] = [];

  eventForm: FormGroup;
  participantForm: FormGroup;
  registrationForm: FormGroup;

  selectedEventFilter: number | null = null;
  feedbackMessage = '';
  feedbackType: 'success' | 'error' | '' = '';

  constructor(
    private readonly fb: FormBuilder,
    private readonly eventService: EventManagementService
  ) {
    this.eventForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(150)]],
      fecha: ['', Validators.required],
      ubicacion: ['', [Validators.required, Validators.maxLength(200)]],
      descripcion: ['', Validators.maxLength(500)]
    });

    this.participantForm = this.fb.group({
      nombre: ['', [Validators.required, Validators.maxLength(100)]],
      apellido: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      telefono: ['', Validators.maxLength(30)]
    });

    this.registrationForm = this.fb.group({
      eventoId: [null, Validators.required],
      participanteId: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadEvents();
    this.loadParticipants();
    this.loadRegistrations();
  }

  private loadEvents(): void {
    this.eventService.getEvents().subscribe({
      next: events => {
        this.events = events;
        if (!this.registrationForm.value.eventoId && this.events.length > 0) {
          this.registrationForm.patchValue({ eventoId: this.events[0].eventoId });
        }
      },
      error: () => this.showError('No se pudieron cargar los eventos.')
    });
  }

  private loadParticipants(): void {
    this.eventService.getParticipants().subscribe({
      next: participants => {
        this.participants = participants;
        if (!this.registrationForm.value.participanteId && this.participants.length > 0) {
          this.registrationForm.patchValue({ participanteId: this.participants[0].participanteId });
        }
      },
      error: () => this.showError('No se pudieron cargar los participantes.')
    });
  }

  loadRegistrations(): void {
    this.eventService.getRegistrations({ eventoId: this.selectedEventFilter }).subscribe({
      next: registrations => {
        this.registrations = registrations;
      },
      error: () => this.showError('No se pudieron cargar las inscripciones.')
    });
  }

  submitEvent(): void {
    if (this.eventForm.invalid) {
      this.eventForm.markAllAsTouched();
      return;
    }

    const newEvent = {
      ...this.eventForm.value,
      fecha: new Date(this.eventForm.value.fecha).toISOString()
    };

    this.eventService.createEvent(newEvent).subscribe({
      next: event => {
        this.events = [...this.events, event].sort((a, b) => new Date(a.fecha).getTime() - new Date(b.fecha).getTime());
        this.eventForm.reset();
        this.showSuccess('Evento creado correctamente.');
      },
      error: () => this.showError('No se pudo crear el evento.')
    });
  }

  submitParticipant(): void {
    if (this.participantForm.invalid) {
      this.participantForm.markAllAsTouched();
      return;
    }

    this.eventService.createParticipant(this.participantForm.value).subscribe({
      next: participant => {
        this.participants = [...this.participants, participant].sort((a, b) => a.apellido.localeCompare(b.apellido));
        this.participantForm.reset();
        this.showSuccess('Participante registrado correctamente.');
      },
      error: () => this.showError('No se pudo registrar el participante.')
    });
  }

  submitRegistration(): void {
    if (this.registrationForm.invalid) {
      this.registrationForm.markAllAsTouched();
      return;
    }

    this.eventService.createRegistration(this.registrationForm.value).subscribe({
      next: registration => {
        this.registrations = [registration, ...this.registrations];
        this.showSuccess('Inscripción realizada con éxito.');
      },
      error: error => {
        if (error.status === 409) {
          this.showError('El participante ya está inscrito en este evento.');
        } else {
          this.showError('No se pudo registrar la inscripción.');
        }
      }
    });
  }

  onEventFilterChange(eventId: string): void {
    const parsedId = eventId ? Number(eventId) : null;
    this.selectedEventFilter = parsedId;
    this.loadRegistrations();
  }

  deleteEvent(id: number): void {
    this.eventService.deleteEvent(id).subscribe({
      next: () => {
        this.events = this.events.filter(event => event.eventoId !== id);
        this.registrations = this.registrations.filter(reg => reg.eventoId !== id);
        if (this.registrationForm.value.eventoId === id) {
          this.registrationForm.patchValue({ eventoId: this.events[0]?.eventoId ?? null });
        }
        if (this.selectedEventFilter === id) {
          this.selectedEventFilter = null;
          this.loadRegistrations();
        }
        this.showSuccess('Evento eliminado.');
      },
      error: () => this.showError('No se pudo eliminar el evento.')
    });
  }

  deleteParticipant(id: number): void {
    this.eventService.deleteParticipant(id).subscribe({
      next: () => {
        this.participants = this.participants.filter(participant => participant.participanteId !== id);
        this.registrations = this.registrations.filter(reg => reg.participanteId !== id);
        if (this.registrationForm.value.participanteId === id) {
          this.registrationForm.patchValue({ participanteId: this.participants[0]?.participanteId ?? null });
        }
        this.showSuccess('Participante eliminado.');
      },
      error: () => this.showError('No se pudo eliminar el participante.')
    });
  }

  deleteRegistration(id: number): void {
    this.eventService.deleteRegistration(id).subscribe({
      next: () => {
        this.registrations = this.registrations.filter(registration => registration.inscripcionId !== id);
        this.showSuccess('Inscripción eliminada.');
      },
      error: () => this.showError('No se pudo eliminar la inscripción.')
    });
  }

  registrationsCountByEvent(eventId: number): number {
    return this.registrations.filter(registration => registration.eventoId === eventId).length;
  }

  registrationsCountByParticipant(participantId: number): number {
    return this.registrations.filter(registration => registration.participanteId === participantId).length;
  }

  private showSuccess(message: string): void {
    this.feedbackType = 'success';
    this.feedbackMessage = message;
    this.dismissFeedbackLater();
  }

  private showError(message: string): void {
    this.feedbackType = 'error';
    this.feedbackMessage = message;
    this.dismissFeedbackLater();
  }

  private dismissFeedbackLater(): void {
    if (!this.feedbackMessage) {
      return;
    }

    setTimeout(() => {
      this.feedbackMessage = '';
      this.feedbackType = '';
    }, 4000);
  }
}
