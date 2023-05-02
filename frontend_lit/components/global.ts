import {css} from 'lit';
export interface ToDo{
    title: string;
    content: string;
    isCompleted: boolean;
    id: string;
}
export const URL = 'http://localhost:5161/api/todo';

export const CSS = css`
html {
  --background-color: #0d1117;
  --text-color: white;
  --accent-color: #7d8590;
  --secondary-background-color: #161b22;
  --secondary-accent-color: #30363d;
}

html[data-theme="light"] {
  --background-color: white;
  --text-color: black;
  --accent-color: #d0d7de;
  --secondary-background-color: #f6f8fa;
  --secondary-accent-color: #d0d7de;
}
`;