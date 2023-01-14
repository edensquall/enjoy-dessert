import { ValidatorFn } from '@angular/forms';
import { of } from 'rxjs';

export function PasswordValidator(): ValidatorFn {
  return (control) => {
    if (!control.value) {
      return of(null);
    }

    const PASSWORD_REGEXP = new RegExp('(?=^.{6,}$)(?=.*[a-z])(?!.*\\s).*$');
    return !PASSWORD_REGEXP.test(control.value)
      ? { passwordValid: true }
      : null;
  };
}
