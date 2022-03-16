import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  redirectUrl: string = null;
  
  constructor(private accountService: AccountService, private formBuilder: FormBuilder, private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.redirectUrl = navigation?.extras?.queryParams?.returnUrl;
  }

  ngOnInit(): void {
    this.initForm();
  }

  initForm () {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    })
  }

  login() {
      this.accountService.login(this.loginForm.value).subscribe(
        () => {
          this.router.navigateByUrl(this.redirectUrl || '/');
        });
  }

}
