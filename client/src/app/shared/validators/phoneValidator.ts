import { ValidatorFn } from '@angular/forms';
import { of } from 'rxjs';

export function phoneValidator(): ValidatorFn {
  return (control) => {
    if (!control.value) {
      return of(null);
    }

    const PHONE_REGEXP = new RegExp(
      '(\\d{2,3}-?|\\(\\d{2,3}\\))\\d{2,3}-?\\d{4}|09\\d{2}(\\d{6}|-\\d{3}-\\d{3})'
    );
    return !PHONE_REGEXP.test(control.value) ? { phoneValid: true } : null;
  };
}
