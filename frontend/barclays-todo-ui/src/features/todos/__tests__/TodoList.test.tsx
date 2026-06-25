import { render, screen } from '@testing-library/react';
import { describe, expect, it, vi } from 'vitest';

import { TodoList } from '../TodoList';
import { TodoStatus } from '../types';
import type { TodoItem } from '../types';

const todos: TodoItem[] = [
    {
        id: '1',
        name: 'Buy milk',
        priority: 1,
        status: TodoStatus.InProgress,
        createdAtUtc: '2026-01-01T00:00:00Z',
        updatedAtUtc: '2026-01-01T00:00:00Z',
    },
    {
        id: '2',
        name: 'Clean room',
        priority: 2,
        status: TodoStatus.Completed,
        createdAtUtc: '2026-01-01T00:00:00Z',
        updatedAtUtc: '2026-01-01T00:00:00Z',
    },
];

describe('TodoList', () => {
    it('renders todo rows', () => {
        render(
            <TodoList
                todos={todos}
                onEdit={vi.fn()}
                onDelete={vi.fn()}
            />,
        );

        expect(screen.getByText('Buy milk')).toBeInTheDocument();
        expect(screen.getByText('Clean room')).toBeInTheDocument();
    });

    it('disables delete button for non-completed tasks and enables it for completed tasks', () => {
        render(
            <TodoList
                todos={todos}
                onEdit={vi.fn()}
                onDelete={vi.fn()}
            />,
        );

        const deleteButtons = screen.getAllByRole('button', { name: 'Delete' });

        expect(deleteButtons[0]).toBeDisabled();
        expect(deleteButtons[1]).not.toBeDisabled();
    });
});