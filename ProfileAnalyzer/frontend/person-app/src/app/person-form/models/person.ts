import { SocialMediaType } from './social-media-type';

export class Person {
  constructor(
    public readonly FirstName: string,
    public readonly LastName: string,
    public readonly SocialSkills: string[],
    public readonly SocialAccounts: SocialMedia[]
  ) {}
}

export class SocialMedia {
  constructor(public readonly Type: SocialMediaType, public readonly Address: string) {}
}
