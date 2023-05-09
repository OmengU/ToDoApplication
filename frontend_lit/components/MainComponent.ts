import { LitElement, html, css } from 'lit';
import { customElement, property } from 'lit/decorators.js';
import { ToDo, URL } from './global'

async function getToDos(): Promise<ToDo[]> {
  const response = await fetch(URL);
  const data = await response.json();
  console.log(data);
  return data as ToDo[];
}

async function deleteToDo(id: string){
  await fetch(`${URL}/${id}`, {
    method: 'DELETE'
  });
}

async function setCompletedToDo(id: string){
  fetch(`${URL}/${id}`, {
  method: 'PATCH',
  headers: {
    'Content-Type': 'application/json'
  },
})
  .then(response => response.json())
  .then((todo: ToDo) => {
    console.log('ToDo item completed:', todo);
  })
  .catch(error => console.error(error));
}

@customElement('main-element')
export class MainComponent extends LitElement {
  static styles = css `
  .container{
      display: flex;
      align-items: center;
      flex-direction: column;
      gap: 1rem;
      width: 100%
  }
  .content {
    width: 50%;
    min-width: 300px; 
    margin: 0 auto;
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }
  .header{
    display: grid;
    grid-template-columns: repeat(3, 1fr);
  }
  .switcher-container{
    display: flex;
    justify-content: flex-end;
  }
  .heading-container{
    display: flex;
    justify-content: center;
  }
  h1{
    margin: 0;
  }
    
  @media (max-width: 768px) {
    .content {
      width: 90%; 
    }
  }
  
  @media (max-width: 480px) {
    .content {
      width: 100%; 
      min-width: 0;
    }
  }
  *{
    transition: color 200ms;
  }
  `

  handleAddEvent(event: CustomEvent) {
    const newToDo = event.detail;
    console.log("handling add event");
    this.todos = [newToDo, ...this.todos];
    //this.showAddThing = false;
  }
  async handleDeleteEvent(event: CustomEvent){
    console.log("handling delete event");
    await deleteToDo(event.detail);
    this.todos = this.todos.filter(todo => todo.id != event.detail);
  }
  async handleCheckEvent(event: CustomEvent){
    console.log("handling change event");
    await setCompletedToDo(event.detail);
  }

  @property({ type: Boolean }) isLoadingGet: boolean = true;
  @property({ type: Array }) todos: ToDo[] = [];
  
  showAddThing: boolean = true;

  async connectedCallback(): Promise<void> {
    super.connectedCallback();
    this.todos = await getToDos();
    this.isLoadingGet = false;
  }

  render() {
    if (this.isLoadingGet) {
      return html`<p>Loading...</p>`
    }

    return html`
      <div class="container">
      <div class="content">
      <div class="header">
      <div class="empty"></div>
      <div class="heading-container">
      <h1>ToDos</h1>
      </div>
      <div class="switcher-container">
      <theme-switcher></theme-switcher>
      </div>
      </div>
      <add-todo @add-event=${this.handleAddEvent}></add-todo>
      <todo-list @delete-event=${this.handleDeleteEvent} @check-event=${this.handleCheckEvent} .todos=${this.todos}></todo-list>
      </div>
      </div>
    `;
    //${this.showAddThing ? html`<add-todo @add-event=${this.handleAddEvent}></add-todo>` : ''}
  }
}
