import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { describe, expect, it, vi } from 'vitest';

import { TodoForm } from '../TodoForm';
import { TodoStatus } from '../types';
import type { TodoItem } from '../types';

const todos: TodoItem[] = [
    {
        id: '1',
        name: 'Buy milk',
        priority: 1,
        status: TodoStatus.NotStarted,
        createdAtUtc: '2026-01-01T00:00:00Z',
        updatedAtUtc: '2026-01-01T00:00:00Z',
    },
];

describe('TodoForm', () => {
    it('shows client validation error when name is empty', async () => {
        const user = userEvent.setup();
        const onSubmit = vi.fn();

        render(
            <TodoForm
                todos={[]}
                serverErrors={[]}
                onSubmit={onSubmit}
                onCancelEdit={vi.fn()}
            />,
        );

        await user.click(screen.getByRole('button', { name: 'Add task' }));

        expect(screen.getByText('Task name is required.')).toBeInTheDocument();
        expect(onSubmit).not.toHaveBeenCalled();
    });

    it('shows duplicate name validation error', async () => {
        const user = userEvent.setup();

        render(
            <TodoForm
                todos={todos}
                serverErrors={[]}
                onSubmit={vi.fn()}
                onCancelEdit={vi.fn()}
            />,
        );

        await user.type(screen.getByLabelText('Name'), 'buy milk');
        await user.click(screen.getByRole('button', { name: 'Add task' }));

        expect(screen.getByText('A task with the same name already exists.')).toBeInTheDocument();
    });

    it('submits valid values', async () => {
        const user = userEvent.setup();
        const onSubmit = vi.fn().mockResolvedValue(undefined);

        render(
            <TodoForm
                todos={[]}
                serverErrors={[]}
                onSubmit={onSubmit}
                onCancelEdit={vi.fn()}
            />,
        );

        await user.type(screen.getByLabelText('Name'), 'Clean room');
        await user.clear(screen.getByLabelText('Priority'));
        await user.type(screen.getByLabelText('Priority'), '3');
        await user.selectOptions(screen.getByLabelText('Status'), String(TodoStatus.InProgress));

        await user.click(screen.getByRole('button', { name: 'Add task' }));

        expect(onSubmit).toHaveBeenCalledWith({
            name: 'Clean room',
            priority: 3,
            status: TodoStatus.InProgress,
        });
    });

    it('populates fields when editing a todo', () => {
        render(
            <TodoForm
                todos={todos}
                editingTodo={todos[0]}
                serverErrors={[]}
                onSubmit={vi.fn()}
                onCancelEdit={vi.fn()}
            />,
        );

        expect(screen.getByLabelText('Name')).toHaveValue('Buy milk');
        expect(screen.getByLabelText('Priority')).toHaveValue(1);
        expect(screen.getByLabelText('Status')).toHaveValue(String(TodoStatus.NotStarted));
        expect(screen.getByRole('button', { name: 'Save changes' })).toBeInTheDocument();
    });
});