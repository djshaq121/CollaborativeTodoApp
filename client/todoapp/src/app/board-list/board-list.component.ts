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
  
  constructor(public boardService: BoardService, private formBuilder: FormBuilder) { }

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

}
