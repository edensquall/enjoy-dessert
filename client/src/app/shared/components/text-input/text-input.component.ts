import { formatDate } from '@angular/common';
import {
  Component,
  ElementRef,
  Input,
  OnInit,
  Self,
  ViewChild,
} from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.scss'],
})
export class TextInputComponent implements OnInit, ControlValueAccessor {
  @ViewChild('input', { static: true })
  input!: ElementRef;
  @Input() type = 'text';
  @Input()
  label!: string;
  showPassword: boolean = false;
  isPassword: boolean = false;

  constructor(@Self() public controlDir: NgControl) {
    this.controlDir.valueAccessor = this;
  }

  ngOnInit(): void {
    const control = this.controlDir.control;
    const validators = control?.validator ? [control.validator] : [];
    const asyncValidators = control?.asyncValidator
      ? [control.asyncValidator]
      : [];

    control?.setValidators(validators);
    control?.setAsyncValidators(asyncValidators);
    control?.updateValueAndValidity();

    if (this.type === 'password') {
      this.isPassword = true;
    }
  }

  onChange(event: any) {}

  onTouched() {}

  writeValue(obj: any): void {
    if (this.type === 'date') {
      if (obj) {
        this.input.nativeElement.value = formatDate(obj, 'yyyy-MM-dd', 'en');
      } else {
        this.input.nativeElement.value = '';
      }
    } else {
      this.input.nativeElement.value = obj || '';
    }
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  swtichPasswordVisiable() {
    this.showPassword = !this.showPassword;
    if (this.showPassword) {
      this.type = 'text';
    } else {
      this.type = 'password';
    }
  }
}
