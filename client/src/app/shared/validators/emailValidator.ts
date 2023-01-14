import { ValidatorFn } from '@angular/forms';
import { of } from 'rxjs';

export function emailValidator(): ValidatorFn {
  return (control) => {
    if (!control.value) {
      return of(null);
    }

    const EMAIL_REGEXP = new RegExp('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$');
    return !EMAIL_REGEXP.test(control.value) ? { emailValid: true } : null;
  };
}
