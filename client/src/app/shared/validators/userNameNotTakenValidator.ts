import { AsyncValidatorFn } from '@angular/forms';
import { map, of, switchMap, timer } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';

export function userNameNotTakenValidator(
  accountService: AccountService
): AsyncValidatorFn {
  return (control) => {
    return timer(500).pipe(
      switchMap(() => {
        if (!control.value) {
          return of(null);
        }
        return accountService.checkUserNameExists(control.value).pipe(
          map((res) => {
            return res ? { userNameExists: true } : null;
          })
        );
      })
    );
  };
}
