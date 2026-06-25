import { describe, expect, it } from 'vitest';

import { TodoStatus } from '../types';
import type { TodoFormValues, TodoItem } from '../types';
import { validateTodo } from '../todoValidation';

const existingTodos: TodoItem[] = [
    {
        id: '1',
        name: 'Buy milk',
        priority: 1,
        status: TodoStatus.NotStarted,
        createdAtUtc: '2026-01-01T00:00:00Z',
        updatedAtUtc: '2026-01-01T00:00:00Z',
    },
];

function createValues(overrides?: Partial<TodoFormValues>): TodoFormValues {
    return {
        name: 'Clean room',
        priority: 2,
        status: TodoStatus.InProgress,
        ...overrides,
    };
}

describe('validateTodo', () => {
    it('returns no errors for valid todo values', () => {
        const errors = validateTodo(createValues(), existingTodos);

        expect(errors).toEqual([]);
    });

    it('returns an error when name is empty', () => {
        const errors = validateTodo(createValues({ name: '   ' }), existingTodos);

        expect(errors).toContain('Task name is required.');
    });

    it('returns an error when priority is negative', () => {
        const errors = validateTodo(createValues({ priority: -1 }), existingTodos);

        expect(errors).toContain('Priority must be a non-negative number.');
    });

    it('returns an error when name already exists case-insensitively', () => {
        const errors = validateTodo(createValues({ name: 'buy milk' }), existingTodos);

        expect(errors).toContain('A task with the same name already exists.');
    });

    it('allows the same name when editing the same todo', () => {
        const errors = validateTodo(
            createValues({ name: 'Buy milk' }),
            existingTodos,
            '1',
        );

        expect(errors).toEqual([]);
    });
});