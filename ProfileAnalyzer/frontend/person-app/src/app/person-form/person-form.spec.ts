import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { SocialMediaType } from './models/social-media-type';
import { PersonFormComponent } from './person-form';
import { PersonService } from './services/person.service';
import { provideZonelessChangeDetection } from '@angular/core';
import { of } from 'rxjs';

describe('PersonFormComponent', () => {
  let component: PersonFormComponent;
  let fixture: ComponentFixture<PersonFormComponent>;
  let mockPersonService: jasmine.SpyObj<PersonService>;

  beforeEach(async () => {
    mockPersonService = jasmine.createSpyObj('PersonService', ['addPerson$']);

    await TestBed.configureTestingModule({
      imports: [
        FormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatButtonModule,
        MatListModule,
        MatChipsModule,
        MatIconModule,
        ReactiveFormsModule,
        MatSelectModule,
      ],
      providers: [
        FormBuilder,
        { provide: PersonService, useValue: mockPersonService },
        provideZonelessChangeDetection(),
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(PersonFormComponent);
    await fixture.whenStable();
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not save when form invalid', () => {
    // Arrange
    component.formGroup.setValue({
      firstName: '',
      lastName: '',
      socialSkills: [],
      socialMedias: [],
    });

    // Act
    component.submitForm();

    // Assert
    expect(mockPersonService.addPerson$).not.toHaveBeenCalled();
  });

  it('should save when form valid', () => {
    // Arrange
    component.formGroup.setValue({
      firstName: 'John',
      lastName: 'Doe',
      socialSkills: [],
      socialMedias: [],
    });
    component.addSkill();
    component.addSocialMedia();
    component.socialSkillsArray!.controls[0]!.setValue('test');
    component.socialMediasArray!.controls[0]!.get('type')!.setValue(SocialMediaType.Facebook);
    component.socialMediasArray!.controls[0]!.get('url')!.setValue('https://facebook.com/johndoe');

    mockPersonService.addPerson$.and.returnValue(of({}));

    // Act
    component.submitForm();

    // Assert
    expect(mockPersonService.addPerson$).toHaveBeenCalled();
  });
});
