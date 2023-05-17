import { LitElement, html, css } from 'lit';
import { customElement, property } from 'lit/decorators.js';

function loadTheme(): string {
  const savedTheme = localStorage.getItem('theme');
  document.body.setAttribute('data-theme', savedTheme ? JSON.parse(savedTheme) : 'dark');
  return savedTheme ? JSON.parse(savedTheme) : 'dark';
}

@customElement('theme-switcher')
export class ThemeSwitcher extends LitElement {
  static styles = css`
  button{
    border: none;
    background: transparent;
    outline: none;
    font-size: 2rem;
  }
  `;

  @property({ type: Object }) private currentTheme: string = loadTheme();

  private switchTheme() {
    const newTheme = this.currentTheme === 'dark' ? 'light' : 'dark';
    document.documentElement.setAttribute('data-theme', newTheme);
    this.currentTheme = newTheme;
    console.log("theme changed to " + this.currentTheme);
    localStorage.setItem('theme', JSON.stringify(newTheme));
    document.body.setAttribute('data-theme', newTheme);
  }

  render() {
    return html`
      <button @click="${this.switchTheme}">
        ${this.currentTheme === 'light' ? '🌚' : '🌞'}
      </button>
    `;
  }
}