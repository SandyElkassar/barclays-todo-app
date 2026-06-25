import type { TodoFormValues, TodoItem } from './types';

const API_BASE_URL = 'http://localhost:5115/api/todos';

async function handleResponse<T>(response: Response): Promise<T> {
  if (response.ok) {
    if (response.status === 204) {
      return undefined as T;
    }

    return response.json() as Promise<T>;
  }

  const errorBody = await response.json().catch(() => ['Unexpected server error.']);
  throw Array.isArray(errorBody) ? errorBody : ['Unexpected server error.'];
}

export async function getTodos(): Promise<TodoItem[]> {
  const response = await fetch(API_BASE_URL);
  return handleResponse<TodoItem[]>(response);
}

export async function createTodo(values: TodoFormValues): Promise<TodoItem> {
  const response = await fetch(API_BASE_URL, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(values),
  });

  return handleResponse<TodoItem>(response);
}

export async function updateTodo(id: string, values: TodoFormValues): Promise<TodoItem> {
  const response = await fetch(`${API_BASE_URL}/${id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(values),
  });

  return handleResponse<TodoItem>(response);
}

export async function deleteTodo(id: string): Promise<void> {
  const response = await fetch(`${API_BASE_URL}/${id}`, {
    method: 'DELETE',
  });

  return handleResponse<void>(response);
}