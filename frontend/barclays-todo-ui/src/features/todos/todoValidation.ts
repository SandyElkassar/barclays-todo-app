import type { TodoFormValues, TodoItem } from './types';

export function validateTodo(
  values: TodoFormValues,
  existingTodos: TodoItem[],
  editingTodoId?: string
): string[] {
  const errors: string[] = [];

  if (!values.name.trim()) {
    errors.push('Task name is required.');
  }

  if (!Number.isFinite(values.priority) || values.priority < 0) {
    errors.push('Priority must be a non-negative number.');
  }

  const hasDuplicateName = existingTodos.some(
    todo =>
      todo.id !== editingTodoId &&
      todo.name.trim().toLowerCase() === values.name.trim().toLowerCase()
  );

  if (values.name.trim() && hasDuplicateName) {
    errors.push('A task with the same name already exists.');
  }

  return errors;
}