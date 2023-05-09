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
      #check{
        height: 1.5rem;
        width: 1.5rem;
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

    handlecheck(){
        this.dispatchEvent(new CustomEvent("check-event", {detail: this.id, bubbles: true, composed: true}));
        this.isdisabled = true;
    }

    render() {
        if(this.iscompleted == true) this.isdisabled = true;
        return html `
        <div class="container">
            <div class="todo-contents">
            <div class="title-container">
            <p class="title">${this.title}</p>
            </div>
            <p class="content">${this.content}</p>
            </div>
            <div class="todo-actions">
            <button id="delete" @click="${() => {this.dispatchEvent(new CustomEvent('delete-event', { detail: this.id , bubbles: true, composed: true}))}}">❌</button>
            <input id="check" type="checkbox" .checked=${this.iscompleted} ?disabled=${this.isdisabled} @change=${this.handlecheck}>
            </div>
        </div>
        `;
    }
}
