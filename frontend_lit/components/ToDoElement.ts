import {LitElement, html, css, CSSResultGroup} from 'lit';
import {customElement, property} from 'lit/decorators.js';
import { CSS } from './global';

@customElement("todo-element")
export class ToDoElement extends LitElement{
    static styles = css`
    .container{
        display: grid;
        grid-template-columns: 1fr auto;
        padding: 0;
        border: inherit;
    }
    .todo-actions {
        display: flex;
        align-items: center;
        justify-content: center;
        flex-direction: column;
        border-left: 1px solid var(--secondary-accent-color);
        padding: .5rem;
        gap: .6rem;
        background-color: var(--secondary-background-color);
      }
      .todo-content {
        flex: 1;
      }
      .title-container{
        width: 100%;
        border-bottom: 1px solid var(--accent-color);
        display: flex;
        justify-content: space-between;
        align-items: center;
      }
      button {
        border: none;
        background: transparent;
        outline: none;
        font-size: 1.5rem;
      }
      .title{
        margin-bottom: .3rem;
        margin-top: .5rem;
        font-size: 2rem;
        font-weight: bold;
      }
      p{
        margin-left: 1rem;
      }
      input, textarea{
        color: var(--accent-color);
        background-color: var(--background-color);
        padding: 0.3rem 1rem;
        border: none;
        font-family: 'Segoe UI';
        outline: none;
      }
      input{
        margin-bottom: 0;
        margin-top: 0;
        font-size: 2rem;
        font-weight: bold;
      }
      textarea{
        font-size: 1rem;
        width: 100%;
      }
      #check{
        height: 1.5rem;
        width: 1.5rem;
        padding: 0;
        -webkit-appearance: none;
        -moz-appearance: none;
        -o-appearance: none;
        appearance: none;
        background-color: var(--background-color);
        border: 2px solid var(--accent-color);
        border-radius: 50%;
      }
      #check:checked{
        background-color: #2196f3;
      }
      #check:hover{
        background-color: #2196f3;
        border: 2px solid #2196f3;
        transition: background-color 400ms, border 400ms;
      }
      #check:disabled{
        background-color: grey;
      }
      #delete:hover{
        background-color: #f92f60;
        border-radius: 15px;
        color: transparent;
        text-shadow: 0 0 0 white;
        transition: background-color 400ms, color 600ms, text-shadow 600ms, border-radius 400ms;
      }
      #delete{
        transition: background-color 400ms, color 600ms, text-shadow 600ms, border-radius 400ms;
      }
    @media (max-width: 480px) {
        .container {
            grid-template-columns: 1fr;
            grid-template-rows: 1fr auto;
        }
        .todo-actions {
            flex-direction: row-reverse;
            justify-content: center;
            gap: 3rem;
            border-left: none;
            border-top: 1px solid var(--secondary-accent-color);
            width: 100%
          }
          input{
            width: 50%;
          }
          textarea{
            width: 100%;
          }
      }
      *{
        transition: background 200ms, color 200ms, border 200ms;
      }
    `
    @property({type: String}) title: string = "";
    @property({type: String}) content: string = "";
    @property({type: String}) id: string = "";
    @property({type: Boolean}) iscompleted?: boolean;
    @property({type: Boolean}) isdisabled?: boolean;
    @property({type: Boolean}) isediting?: boolean = false;
    
    private originalTitle: string = "";
    private originalContent: string = "";

    handleToggleEdit() {
      if (this.isediting) {
        this.title = this.originalTitle;
        this.content = this.originalContent;
      } else {
        this.originalTitle = this.title;
        this.originalContent = this.content;
      }
      this.isediting = !this.isediting;
    }


    handlecheck(){
        this.dispatchEvent(new CustomEvent("check-event", {detail: this.id, bubbles: true, composed: true}));
        this.isdisabled = true;
    }
    handleEdit(event: SubmitEvent) {
      event.preventDefault();
      this.dispatchEvent(new CustomEvent('add-event', { detail: {id: this.id, title: this.title, content: this.content} }));
      this.isediting = false;
  }

    render() {
      if(this.iscompleted == true) this.isdisabled = true;
        return html `
        <div class="container">
          ${this.isediting == false ? html`
          <div class="todo-contents">
          <div class="title-container">
          <p class="title">${this.title}</p>
          <div class="editButtons">
          <button class="sendEdit">✅</button>
          <button class="edit" @click="${() => {this.handleToggleEdit()}}">${this.isediting === false ? '✏️' : '❌'}</button>
          </div>
          </div>
          <p class="content">${this.content}</p>
          </div>
          `: html`
          <form @submit="${this.handleEdit}">
          <div class="todo-contents">
          <div class="title-container">
          <input type="text" placeholder="title" .value="${this.title}" @input="${(event: Event) => {
          this.title = (event.target as HTMLInputElement).value;
          }}">
          <div class="editButtons">
          <button class="sendEdit" type="submit">✅</button>
          <button class="edit" @click="${() => {this.handleToggleEdit()}}">❌</button>
          </div>
          </div>
          <textarea type="text" placeholder="content" .value="${this.content}" @input="${(event: Event) => {
          this.content = (event.target as HTMLInputElement).value;
          }}" @keyup="${(e: KeyboardEvent) => { if (e.key === 'Enter') { this.handleEdit } }}"></textarea>
          </div>
          </form>
          `}
            <div class="todo-actions">
            <button id="delete" @click="${() => {this.dispatchEvent(new CustomEvent('delete-event', { detail: this.id , bubbles: true, composed: true}))}}">❌</button>
            <input id="check" type="checkbox" .checked=${this.iscompleted} ?disabled=${this.isdisabled} @change=${this.handlecheck}>
            </div>
        </div>
        `;
    }
}
