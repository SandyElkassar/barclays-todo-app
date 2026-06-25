export const TodoStatus = {
  NotStarted: 0,
  InProgress: 1,
  Completed: 2,
} as const;

export type TodoStatus = (typeof TodoStatus)[keyof typeof TodoStatus];

export type TodoItem = {
  id: string;
  name: string;
  priority: number;
  status: TodoStatus;
  createdAtUtc: string;
  updatedAtUtc: string;
};

export type TodoFormValues = {
  name: string;
  priority: number;
  status: TodoStatus;
};

export const statusLabels: Record<TodoStatus, string> = {
  [TodoStatus.NotStarted]: 'Not started',
  [TodoStatus.InProgress]: 'In progress',
  [TodoStatus.Completed]: 'Completed',
};