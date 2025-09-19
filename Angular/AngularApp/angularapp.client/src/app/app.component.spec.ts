import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { of } from 'rxjs';
import { AppComponent } from './app.component';
import { EventManagementService } from './event-management.service';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let serviceSpy: jasmine.SpyObj<EventManagementService>;

  beforeEach(async () => {
    serviceSpy = jasmine.createSpyObj<EventManagementService>('EventManagementService', [
      'getEvents',
      'getParticipants',
      'getRegistrations',
      'createEvent',
      'createParticipant',
      'createRegistration',
      'deleteEvent',
      'deleteParticipant',
      'deleteRegistration'
    ]);

    serviceSpy.getEvents.and.returnValue(of([]));
    serviceSpy.getParticipants.and.returnValue(of([]));
    serviceSpy.getRegistrations.and.returnValue(of([]));

    await TestBed.configureTestingModule({
      declarations: [AppComponent],
      imports: [FormsModule, ReactiveFormsModule],
      providers: [{ provide: EventManagementService, useValue: serviceSpy }]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should load initial data on init', () => {
    expect(serviceSpy.getEvents).toHaveBeenCalled();
    expect(serviceSpy.getParticipants).toHaveBeenCalled();
    expect(serviceSpy.getRegistrations).toHaveBeenCalled();
  });

  it('should not submit an invalid event form', () => {
    component.eventForm.setValue({ nombre: '', fecha: '', ubicacion: '', descripcion: '' });
    component.submitEvent();
    expect(serviceSpy.createEvent).not.toHaveBeenCalled();
  });

  it('should submit a valid event form', () => {
    const mockEvent = { eventoId: 1, nombre: 'Maratón', fecha: new Date().toISOString(), ubicacion: 'Parque', descripcion: '' };
    serviceSpy.createEvent.and.returnValue(of(mockEvent));

    component.eventForm.setValue({ nombre: 'Maratón', fecha: '2025-01-01', ubicacion: 'Parque', descripcion: '' });
    component.submitEvent();

    expect(serviceSpy.createEvent).toHaveBeenCalled();
  });
});
