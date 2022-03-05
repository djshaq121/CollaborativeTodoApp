import { Todo } from "./todo"

export interface Board {
    id: number,
    title: string,
    dateCreated: Date,
    todos: Todo[],
}