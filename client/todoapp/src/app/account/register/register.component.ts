import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm: FormGroup;
  redirectUrl: string = null;
  constructor(private accountService: AccountService, private formBuilder: FormBuilder, private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.redirectUrl = navigation?.extras?.queryParams?.returnUrl;
   }

  ngOnInit(): void {
    this.initForm();
  }

  initForm() {
    this.registerForm = this.formBuilder.group({
      userName: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required],
    })
  }

  register() {
    this.accountService.register(this.registerForm.value).subscribe(
      response => {
        this.router.navigateByUrl(this.redirectUrl || '/');
      }, 
      error => {
      
      }
    )
  }

}
