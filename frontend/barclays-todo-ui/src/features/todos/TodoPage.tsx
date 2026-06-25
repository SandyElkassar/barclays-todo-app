import { useEffect, useState } from 'react';
import { createTodo, deleteTodo, getTodos, updateTodo } from './todoApi';
import { TodoForm } from './TodoForm';
import { TodoList } from './TodoList';
import type { TodoFormValues, TodoItem } from './types';

export function TodoPage() {
    const [todos, setTodos] = useState<TodoItem[]>([]);
    const [editingTodo, setEditingTodo] = useState<TodoItem | null>(null);
    const [serverErrors, setServerErrors] = useState<string[]>([]);
    const [pageError, setPageError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    async function loadTodos() {
        setIsLoading(true);
        setPageError(null);

        try {
            const loadedTodos = await getTodos();
            setTodos(loadedTodos);
        } catch {
            setPageError('Failed to load tasks.');
        } finally {
            setIsLoading(false);
        }
    }

    useEffect(() => {
        void loadTodos();
    }, []);

    async function handleSubmit(values: TodoFormValues) {
        setServerErrors([]);

        try {
            if (editingTodo) {
                const updatedTodo = await updateTodo(editingTodo.id, values);

                setTodos(currentTodos =>
                    currentTodos.map(todo => (todo.id === updatedTodo.id ? updatedTodo : todo))
                );

                setEditingTodo(null);
                return;
            }

            const createdTodo = await createTodo(values);
            setTodos(currentTodos => [...currentTodos, createdTodo]);
        } catch (error) {
            if (Array.isArray(error)) {
                setServerErrors(error.map(String));
                return;
            }

            setServerErrors(['Unexpected server error.']);
        }
    }

    async function handleDelete(todo: TodoItem) {
        setServerErrors([]);
        setPageError(null);

        try {
            await deleteTodo(todo.id);
            setTodos(currentTodos => currentTodos.filter(item => item.id !== todo.id));

            if (editingTodo?.id === todo.id) {
                setEditingTodo(null);
            }
        } catch (error) {
            if (Array.isArray(error)) {
                setPageError(error.map(String).join(' '));
                return;
            }

            setPageError('Failed to delete task.');
        }
    }

    return (
        <main className="todo-page">
            <h1>TODO application</h1>

            <TodoForm
                todos={todos}
                editingTodo={editingTodo}
                serverErrors={serverErrors}
                onSubmit={handleSubmit}
                onCancelEdit={() => {
                    setEditingTodo(null);
                    setServerErrors([]);
                }}
            />

            {isLoading && <p>Loading tasks...</p>}

            {pageError && (
                <div role="alert" className="error-summary">
                    {pageError}
                </div>
            )}

            {!isLoading && (
                <TodoList
                    todos={todos}
                    onEdit={todo => {
                        setEditingTodo(todo);
                        setServerErrors([]);
                    }}
                    onDelete={handleDelete}
                />
            )}
        </main>
    );
}