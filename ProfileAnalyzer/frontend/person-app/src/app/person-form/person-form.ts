import { ChangeDetectionStrategy, Component } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { SocialMediaType } from './models/social-media-type';
import { Person, SocialMedia } from './models/person';
import { PersonService } from './services/person.service';
import { take, tap } from 'rxjs';

@Component({
  selector: 'app-person-form',
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
  templateUrl: './person-form.html',
  styleUrl: './person-form.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PersonFormComponent {
  public readonly formGroup: FormGroup<UserForm>;
  protected readonly socialMediaTypes: SocialMediaType[];

  constructor(private fb: FormBuilder, private personService: PersonService) {
    this.socialMediaTypes = Object.values(SocialMediaType);
    this.formGroup = fb.group<UserForm>({
      firstName: fb.control(null, Validators.required),
      lastName: fb.control(null, Validators.required),
      socialSkills: fb.array<string | null>([], Validators.required),
      socialMedias: fb.array<FormGroup<SocialMediaForm>>([], Validators.required),
    });
  }

  public get socialSkillsArray(): FormArray<FormControl<string | null>> {
    return this.formGroup.get('socialSkills') as FormArray<FormControl<string | null>>;
  }

  public get socialMediasArray(): FormArray<FormGroup<SocialMediaForm>> {
    return this.formGroup.get('socialMedias') as FormArray<FormGroup<SocialMediaForm>>;
  }

  public addSkill() {
    this.socialSkillsArray.push(this.fb.control(null, Validators.required));
  }

  public removeSkill(index: number) {
    this.socialSkillsArray.removeAt(index);
  }

  public addSocialMedia() {
    this.socialMediasArray.push(
      this.fb.group<SocialMediaForm>({
        type: this.fb.control(null, Validators.required),
        url: this.fb.control(null, Validators.required),
      })
    );
  }

  public removeSocialMedia(index: number) {
    this.socialMediasArray.removeAt(index);
  }

  public submitForm() {
    this.formGroup.updateValueAndValidity();

    if (this.formGroup.valid) {
      const formGroupValue = this.formGroup.getRawValue();
      const person = new Person(
        formGroupValue.firstName!,
        formGroupValue.lastName!,
        formGroupValue.socialSkills.map((skill) => skill!),
        formGroupValue.socialMedias.map((media) => new SocialMedia(media.type!, media.url!))
      );
      this.personService
        .addPerson$(person)
        .pipe(
          take(1),
          tap(() => {
            this.formGroup.reset();
          })
        )
        .subscribe();
    } else {
      this.formGroup.markAllAsTouched();
    }
  }
}

interface UserForm {
  firstName: FormControl<string | null>;
  lastName: FormControl<string | null>;
  socialSkills: FormArray<FormControl<string | null>>;
  socialMedias: FormArray<FormGroup<SocialMediaForm>>;
}

interface SocialMediaForm {
  type: FormControl<SocialMediaType | null>;
  url: FormControl<string | null>;
}
