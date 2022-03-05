import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BoardService } from '../services/board.service';

@Component({
  selector: 'app-board-list',
  templateUrl: './board-list.component.html',
  styleUrls: ['./board-list.component.scss']
})
export class BoardListComponent implements OnInit {

  createBoardForm!: FormGroup;
  
  constructor(private boardservice: BoardService, private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.createBoardForm = this.formBuilder.group({
      name: ['', Validators.required],
    })
  }

  createBoard(){
    const name = this.createBoardForm.get('name')?.value;
    this.boardservice.createBoard(name).subscribe();
  }

}
