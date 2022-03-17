import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { User } from 'src/app/model/user';
import { AccountService } from 'src/app/services/account.service';
import { BoardService } from '../../services/board.service';

@Component({
  selector: 'app-invitation',
  templateUrl: './invitation.component.html',
  styleUrls: ['./invitation.component.scss']
})
export class InvitationComponent implements OnInit {


  token: string;
  public redirectUrl: string;

  constructor(private activatedroute:ActivatedRoute, public accountService: AccountService, private boardService: BoardService, private router: Router) { 
    this.token = this.activatedroute.snapshot.paramMap.get("token");
    this.redirectUrl = 'board/sharing/'+this.token;
    console.log(this.token);
  }

  ngOnInit(): void {
    this.shareBoardWithUser();
  }

  shareBoardWithUser() {
    let foundUser: User = null;
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => foundUser = user);

    if(foundUser) {
      this.boardService.addUserToBoard(this.token).subscribe(
        () => { this.invitationSucceed() }
        , error => {
         this.invitationFailed(error);
        }
      )
    }
  }

  redirectTo(url: string) {
    this.router.navigate([url], { queryParams: { returnUrl: this.redirectUrl } });
  }

  invitationSucceed() {
    this.router.navigate(['/']);
  }

  invitationFailed(error) {
    console.log(error);
  }

}
