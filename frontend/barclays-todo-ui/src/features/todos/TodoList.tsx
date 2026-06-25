import type { TodoItem } from './types';
import { TodoStatus, statusLabels } from './types';

type TodoListProps = {
  todos: TodoItem[];
  onEdit: (todo: TodoItem) => void;
  onDelete: (todo: TodoItem) => Promise<void>;
};

export function TodoList({ todos, onEdit, onDelete }: TodoListProps) {
  if (todos.length === 0) {
    return <p>No tasks yet.</p>;
  }

  return (
    <section>
      <h2>Tasks</h2>

      <table className="todo-table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Priority</th>
            <th>Status</th>
            <th aria-label="Actions" />
          </tr>
        </thead>

        <tbody>
          {todos.map(todo => {
            const canDelete = todo.status === TodoStatus.Completed;

            return (
              <tr key={todo.id}>
                <td>{todo.name}</td>
                <td>{todo.priority}</td>
                <td>{statusLabels[todo.status]}</td>
                <td>
                  <button type="button" onClick={() => onEdit(todo)}>
                    Edit
                  </button>

                  <button
                    type="button"
                    onClick={() => onDelete(todo)}
                    disabled={!canDelete}
                    title={canDelete ? 'Delete completed task' : 'Only completed tasks can be deleted'}
                  >
                    Delete
                  </button>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </section>
  );
}