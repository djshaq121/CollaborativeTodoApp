import { Todo } from "./todo"

export interface Board {
    id: number,
    title: string,
    createdDate: Date,
    todos: Todo[],
}