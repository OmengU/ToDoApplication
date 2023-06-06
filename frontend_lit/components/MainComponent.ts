import { LitElement, html, css } from 'lit';
import { customElement, property } from 'lit/decorators.js';
import { ToDo, URL} from './global'

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
  fetch(`${URL}/${id}/complete`, {
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

async function updateToDoData(id: string, body : object): Promise<any> {
  try {
    const response = await fetch(`${URL}/${id}`, {
      method: 'PATCH',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(body),
    });
    const data = await response.json();

    return data;
  } catch (error) {
    console.error(error);
  }
}

@customElement('main-element')
export class MainComponent extends LitElement {
  static styles = css `
  .container{
      display: flex;
      align-items: center;
      flex-direction: column;
      gap: 1rem;
      width: 100%;
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
  dialog{
    position: absolute;
    top: 50%;
    left: .5vw;
    background: var(--secondary-background-color);
    border: 2px solid var(--accent-color);
    border-radius: 15px;
    color: var(--text-color);
    padding: 1rem;
    z-index: 100;
  }
  .overlay {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    backdrop-filter: blur(10px);
    z-index: 99;
  }
  .closeDialogContainer{
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
  .closeDialogContainer > p {
    margin: 0;
    font-size: 2rem;
  }
  .closeDialogButton{
    border: none;
    background: transparent;
    outline: none;
    font-size: 1rem;
    height: 1.5rem;
  }
  .closeDialogButton:hover{
    background-color: #f92f60;
    border-radius: 15px;
    color: transparent;
    text-shadow: 0 0 0 white;
    transition: background-color 400ms, color 400ms, text-shadow 600ms, border-radius 400ms;
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
  async handleEditEvent(event: CustomEvent){
    console.log("handling edit event");
    await updateToDoData(event.detail.id, {title: event.detail.title, content: event.detail.content})
  }
  handleErrorEvent(event: CustomEvent){
    console.log("handling error");
    this.hasError = true;
  }

  @property({ type: Boolean }) isLoadingGet: boolean = true;
  @property({ type: Boolean }) hasError: boolean = false;
  @property({ type: Array }) todos: ToDo[] = [];

  async connectedCallback(): Promise<void> {
    super.connectedCallback();
    try{
      this.todos = await getToDos();
    }catch(e){
      console.log("ToDos could not be loaded");
      this.hasError = true;
    }
    if(this.todos.length > 0) this.isLoadingGet = false;
  }

  render() {
    if (this.isLoadingGet) return html`
      <p>Loading...</p>
      ${this.hasError ? html`<div class="overlay"></div>` : ''}
        <dialog ?open=${this.hasError}>
          <div class="closeDialogContainer">
            <p>⚠️</p>
            <button class="closeDialogButton" @click="${() => {this.hasError = false}}">❌</button>
          </div>
          <p>ToDos could not be loaded<p>
        </dialog>
      `
    return html`
    ${this.hasError ? html`<div class="overlay"></div>` : ''}
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
            <add-todo @add-event=${this.handleAddEvent} @error-event=${this.handleErrorEvent}></add-todo>
            <dialog ?open=${this.hasError}>
              <div class="closeDialogContainer">
                <p>⚠️</p>
                <button class="closeDialogButton" @click="${() => {this.hasError = false}}">❌</button>
              </div>
              <p>ToDos must contain both a title and content!!<p>
            </dialog>
            <todo-list @delete-event=${this.handleDeleteEvent} @check-event=${this.handleCheckEvent} @edit-event=${this.handleEditEvent} @error-event=${this.handleErrorEvent} .todos=${this.todos}></todo-list>
          </div>
      </div>
    `;
  }
}