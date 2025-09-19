import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Event, Participant, RegistrationDetail, RegistrationRequest } from './models';

@Injectable({
  providedIn: 'root'
})
export class EventManagementService {
  private readonly baseUrl = '/api';

  constructor(private http: HttpClient) {}

  getEvents(): Observable<Event[]> {
    return this.http.get<Event[]>(`${this.baseUrl}/eventos`);
  }

  createEvent(event: Omit<Event, 'eventoId' | 'inscripciones'>): Observable<Event> {
    return this.http.post<Event>(`${this.baseUrl}/eventos`, event);
  }

  updateEvent(event: Event): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/eventos/${event.eventoId}`, event);
  }

  deleteEvent(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/eventos/${id}`);
  }

  getParticipants(): Observable<Participant[]> {
    return this.http.get<Participant[]>(`${this.baseUrl}/participantes`);
  }

  createParticipant(participant: Omit<Participant, 'participanteId' | 'inscripciones'>): Observable<Participant> {
    return this.http.post<Participant>(`${this.baseUrl}/participantes`, participant);
  }

  deleteParticipant(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/participantes/${id}`);
  }

  getRegistrations(filters?: { eventoId?: number | null; participanteId?: number | null }): Observable<RegistrationDetail[]> {
    let params = new HttpParams();
    if (filters?.eventoId) {
      params = params.set('eventoId', filters.eventoId);
    }
    if (filters?.participanteId) {
      params = params.set('participanteId', filters.participanteId);
    }

    return this.http.get<RegistrationDetail[]>(`${this.baseUrl}/inscripciones`, { params });
  }

  createRegistration(request: RegistrationRequest): Observable<RegistrationDetail> {
    return this.http.post<RegistrationDetail>(`${this.baseUrl}/inscripciones`, request);
  }

  deleteRegistration(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/inscripciones/${id}`);
  }
}
