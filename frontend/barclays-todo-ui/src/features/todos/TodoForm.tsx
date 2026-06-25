import { useEffect, useState } from 'react';
import type { ComponentPropsWithoutRef } from 'react';

import { TodoStatus, statusLabels } from './types';
import type { TodoFormValues, TodoItem } from './types';
import { validateTodo } from './todoValidation';

type TodoFormProps = {
  todos: TodoItem[];
  editingTodo?: TodoItem | null;
  serverErrors: string[];
  onSubmit: (values: TodoFormValues) => Promise<void>;
  onCancelEdit: () => void;
};

const initialValues: TodoFormValues = {
  name: '',
  priority: 0,
  status: TodoStatus.NotStarted,
};

export function TodoForm({
  todos,
  editingTodo,
  serverErrors,
  onSubmit,
  onCancelEdit,
}: TodoFormProps) {
  const [values, setValues] = useState<TodoFormValues>(initialValues);
  const [clientErrors, setClientErrors] = useState<string[]>([]);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    if (editingTodo) {
      setValues({
        name: editingTodo.name,
        priority: editingTodo.priority,
        status: editingTodo.status,
      });
      setClientErrors([]);
      return;
    }

    setValues(initialValues);
    setClientErrors([]);
  }, [editingTodo]);

  const handleSubmit: ComponentPropsWithoutRef<'form'>['onSubmit'] = async event => {
    event.preventDefault();

    const errors = validateTodo(values, todos, editingTodo?.id);
    setClientErrors(errors);

    if (errors.length > 0) {
      return;
    }

    setIsSubmitting(true);

    try {
      await onSubmit({
        ...values,
        name: values.name.trim(),
      });

      if (!editingTodo) {
        setValues(initialValues);
      }

      setClientErrors([]);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <form className="todo-form" onSubmit={handleSubmit} noValidate>
      <h2>{editingTodo ? 'Edit task' : 'Add task'}</h2>

      {[...clientErrors, ...serverErrors].length > 0 && (
        <div role="alert" className="error-summary">
          <ul>
            {[...clientErrors, ...serverErrors].map(error => (
              <li key={error}>{error}</li>
            ))}
          </ul>
        </div>
      )}

      <div className="form-field form-field-name">
        <label htmlFor="todo-name">Name</label>
        <input
          id="todo-name"
          value={values.name}
          onChange={event =>
            setValues(current => ({
              ...current,
              name: event.target.value,
            }))
          }
        />
      </div>

      <div className="form-field form-field-priority">
        <label htmlFor="todo-priority">Priority</label>
        <input
          id="todo-priority"
          type="number"
          value={values.priority}
          onChange={event =>
            setValues(current => ({
              ...current,
              priority: Number(event.target.value),
            }))
          }
        />
      </div>

      <div className="form-field form-field-status">
        <label htmlFor="todo-status">Status</label>
        <select
          id="todo-status"
          value={values.status}
          onChange={event =>
            setValues(current => ({
              ...current,
              status: Number(event.target.value) as TodoStatus,
            }))
          }
        >
          <option value={TodoStatus.NotStarted}>{statusLabels[TodoStatus.NotStarted]}</option>
          <option value={TodoStatus.InProgress}>{statusLabels[TodoStatus.InProgress]}</option>
          <option value={TodoStatus.Completed}>{statusLabels[TodoStatus.Completed]}</option>
        </select>
      </div>

      <div className="form-actions">
        <button type="submit" disabled={isSubmitting}>
          {editingTodo ? 'Save changes' : 'Add task'}
        </button>

        {editingTodo && (
          <button type="button" onClick={onCancelEdit}>
            Cancel
          </button>
        )}
      </div>
    </form>
  );
}