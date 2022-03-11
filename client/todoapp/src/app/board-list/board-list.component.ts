import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { take } from 'rxjs/operators';
import { Board } from '../model/board';
import { User } from '../model/user';
import { AccountService } from '../services/account.service';
import { BoardService } from '../services/board.service';

@Component({
  selector: 'app-board-list',
  templateUrl: './board-list.component.html',
  styleUrls: ['./board-list.component.scss']
})
export class BoardListComponent implements OnInit, OnDestroy {

  createBoardForm!: FormGroup;
  user!: User;
  
  constructor(public boardService: BoardService, private formBuilder: FormBuilder, private accountService: AccountService) { 
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }
 

  ngOnInit(): void { 
    this.getAllBoards();
    this.createBoardForm = this.formBuilder.group({
      name: ['', Validators.required],
    })
  }

  createBoard(){
    const name = this.createBoardForm.get('name')?.value;
    this.boardService.createBoard(name).subscribe();
  }

  getAllBoards() {
    this.boardService.getBoards().subscribe();
  }

  selectBoard(board: Board) {
   this.boardService.createHubConnection(this.user, board.id);
  }

  ngOnDestroy(): void {
   this.boardService.stopHubConnection();
  }

}
