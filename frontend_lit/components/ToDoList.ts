import {LitElement, html, css} from 'lit';
import {customElement, property} from 'lit/decorators.js';
import {ToDo} from './global'

@customElement("todo-list")
export class ToDoList extends LitElement{
    static styles = css`
        .todos{
            border: 2px solid var(--accent-color);
            border-radius: 15px;
            overflow: hidden;
        }
        .todos-empty{
            border: none;
        }
        todo-element{
            border-bottom: 2px solid var(--accent-color);
        }
        todo-element:last-child{
            border-bottom: none;
        }
        *{
            transition: border 200ms;
        }
        `
    @property() todos : ToDo[] = [];

    render(){
    return html`
    <div class=${this.todos.length > 0 ? "todos" : "todos-empty"}>
        ${this.todos.map((todo) => html`<todo-element .title=${todo.title} .content=${todo.content} .id=${todo.id} .iscompleted=${todo.isCompleted}></todo-element>`)}
    </div>
    `;
    }
}