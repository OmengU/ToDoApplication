import {LitElement, html, css, CSSResultGroup} from 'lit';
import {customElement, property} from 'lit/decorators.js';
import {ToDo, URL} from './global'

async function createToDo(body : object) : Promise<any>{
    const headers = { 'Content-Type': 'application/json' };
    console.log(body);

    try {
        const response = await fetch(URL, {
        method: 'POST',
        headers,
        body: JSON.stringify(body),
      });

      const data = await response.json();

      return data;
    } catch (error) {
      console.error(error);
    }
}

@customElement("add-todo")
export class AddToDo extends LitElement{
    static styles = css `
    input, textarea{
      color: var(--text-color);
      background-color: var(--background-color);
      border: 1px solid var(--secondary-accent-color);
      border-radius: 15px;
      padding: 0.3rem .5rem;
      font-size: 1rem;
    }
    .container{
      display: grid;
      grid-template-columns: 1fr auto;
      gap: 1rem;
      padding-bottom: 2rem;
      border: 2px solid var(--accent-color);
      border-radius: 15px;
      padding: 1.5rem .5rem;
      background-color: var(--secondary-background-color);
    }
    .text-fields{
      display: flex;
      flex-direction: column;
      gap: .5rem;
    }
    button {
      border: none;
      background: transparent;
      outline: none;
      font-size: 1.8rem;
    }
    .button{
      display: flex;
      flex-direction: column;
      justify-content: flex-end;
    }
    button:hover{
      filter: invert(var(--color-invert)) hue-rotate(var(--color-rotate));  
    }
    @media (max-width: 480px) {
      .container{
        grid-template-columns: 1fr;
        grid-template-rows: 1fr auto;
        padding: .75rem .5rem;
        gap: .5rem;
      }
      .button{
        flex-direction: row;
        font-size: 1.5rem;
      }
    }
    *{
      transition: background 200ms, color 200ms, border 200ms, filter 150ms;
    }
    `;

    @property() title: string = "";
    @property() content: string = "";

    async handleSubmit(event: SubmitEvent) {
        event.preventDefault();
        if(this.title.length > 0 && this.content.length > 0){
          const data = await createToDo({title: this.title, content: this.content});
          this.dispatchEvent(new CustomEvent('add-event', { detail: data }));
          this.title = "";
          this.content = "";
        }
    }

render() {
    return html`
    <form @submit="${this.handleSubmit}">
      <div class="container">
        <div class="text-fields">
          <input type="text" placeholder="title" .value="${this.title}" @input="${(event: Event) => {this.title = (event.target as HTMLInputElement).value;}}">
          <textarea type="text" placeholder="content" .value="${this.content}" @input="${(event: Event) => {this.content = (event.target as HTMLInputElement).value;}}" @keyup="${(e: KeyboardEvent) => { if (e.key === 'Enter') { this.handleSubmit } }}"></textarea>
        </div>
        <div class="button">
          <button type="submit">✅</button>
        </div>
      </div>
    </form>
    `
}
}