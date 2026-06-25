import { expect, test } from '@playwright/test';

test.describe('TODO application', () => {
    test('adds a task, edits it to completed, and deletes it', async ({ page }) => {
        await page.goto('/');

        const taskName = `Playwright task ${Date.now()}`;

        await page.getByLabel('Name').fill(taskName);
        await page.getByLabel('Priority').fill('3');
        await page.getByLabel('Status').selectOption('1');

        await page.getByRole('button', { name: 'Add task' }).click();

        const row = page.getByRole('row', { name: new RegExp(taskName) });

        await expect(row).toBeVisible();
        await expect(row).toContainText('In progress');

        await expect(row.getByRole('button', { name: 'Delete' })).toBeDisabled();

        await row.getByRole('button', { name: 'Edit' }).click();

        await expect(page.getByRole('button', { name: 'Save changes' })).toBeVisible();

        await page.getByLabel('Status').selectOption('2');
        await page.getByRole('button', { name: 'Save changes' }).click();

        await expect(page.getByRole('row', { name: new RegExp(taskName) })).toContainText('Completed');

        const completedRow = page.getByRole('row', { name: new RegExp(taskName) });

        await expect(completedRow.getByRole('button', { name: 'Delete' })).not.toBeDisabled();

        await completedRow.getByRole('button', { name: 'Delete' }).click();

        await expect(page.getByText(taskName)).not.toBeVisible();
    });

    test('shows validation error for duplicate task names', async ({ page }) => {
        await page.goto('/');

        const taskName = `Duplicate task ${Date.now()}`;

        await page.getByLabel('Name').fill(taskName);
        await page.getByLabel('Priority').fill('1');
        await page.getByRole('button', { name: 'Add task' }).click();

        await expect(page.getByText(taskName)).toBeVisible();

        await page.getByLabel('Name').fill(taskName.toLowerCase());
        await page.getByLabel('Priority').fill('2');
        await page.getByRole('button', { name: 'Add task' }).click();

        await expect(page.getByText('A task with the same name already exists.')).toBeVisible();
    });
});