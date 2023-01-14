import { AbstractControl, FormGroup } from '@angular/forms';
export function ConfirmValidator(
  controlName: string,
  matchingControlName: string
) {
  return (group: AbstractControl) => {
    const formGroup = <FormGroup>group;
    const control = formGroup.controls[controlName];
    const matchingControl = formGroup.controls[matchingControlName];

    if (matchingControl.errors && !matchingControl.errors['confirmValid']) {
      // return if another validator has already found an error on the matchingControl
      return null;
    }

    // set error on matchingControl if validation fails
    if (control.value !== matchingControl.value) {
      matchingControl.setErrors({ confirmValid: true });
    } else {
      matchingControl.setErrors(null);
    }

    return null;
  };
}
