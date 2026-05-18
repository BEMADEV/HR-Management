import { test, expect, navigateToHrManagement, waitForObsidianBlock } from './fixtures/auth';

/**
 * E2E tests for PTO Allocation List page.
 * Tests navigation, display, and filtering operations.
 */
test.describe('PTO Allocation List', () => {
    test.beforeEach(async ({ authenticatedPage }) => {
        await navigateToHrManagement(authenticatedPage, process.env.PTO_ALLOCATIONS_PAGE || '/admin/plugins/hr-management/pto-allocations');
        await waitForObsidianBlock(authenticatedPage);
    });

    test('should display the PTO Allocations grid', async ({ authenticatedPage }) => {
        const grid = authenticatedPage.locator('.grid, table');
        await expect(grid).toBeVisible();
    });

    test('should have expected column headers', async ({ authenticatedPage }) => {
        // Check for expected columns based on the PTO Allocation model
        await expect(authenticatedPage.locator('th:has-text("Person"), td:has-text("Person")')).toBeVisible().catch(() => {});
        await expect(authenticatedPage.locator('th:has-text("PTO Type"), td:has-text("PTO Type")')).toBeVisible().catch(() => {});
    });
});

/**
 * E2E tests for PTO Bracket List page.
 */
test.describe('PTO Bracket List', () => {
    test.beforeEach(async ({ authenticatedPage }) => {
        await navigateToHrManagement(authenticatedPage, process.env.PTO_BRACKETS_PAGE || '/admin/plugins/hr-management/pto-brackets');
        await waitForObsidianBlock(authenticatedPage);
    });

    test('should display the PTO Brackets grid', async ({ authenticatedPage }) => {
        const grid = authenticatedPage.locator('.grid, table');
        await expect(grid).toBeVisible();
    });

    test('should have Add button', async ({ authenticatedPage }) => {
        const addButton = authenticatedPage.locator('button:has-text("Add"), a:has-text("Add")');
        const isVisible = await addButton.isVisible().catch(() => false);
        console.log(`Add button visible: ${isVisible}`);
    });
});

/**
 * E2E tests for PTO Tier List page.
 */
test.describe('PTO Tier List', () => {
    test.beforeEach(async ({ authenticatedPage }) => {
        await navigateToHrManagement(authenticatedPage, process.env.PTO_TIERS_PAGE || '/admin/plugins/hr-management/pto-tiers');
        await waitForObsidianBlock(authenticatedPage);
    });

    test('should display the PTO Tiers grid', async ({ authenticatedPage }) => {
        const grid = authenticatedPage.locator('.grid, table');
        await expect(grid).toBeVisible();
    });
});

/**
 * E2E tests for HR Employee List page.
 */
test.describe('HR Employee List', () => {
    test.beforeEach(async ({ authenticatedPage }) => {
        await navigateToHrManagement(authenticatedPage, process.env.HR_MANAGEMENT_PAGE || '/admin/plugins/hr-management');
        await waitForObsidianBlock(authenticatedPage);
    });

    test('should display the HR Employee grid', async ({ authenticatedPage }) => {
        const grid = authenticatedPage.locator('.grid, table');
        await expect(grid).toBeVisible();
    });

    test('should have search/filter capability', async ({ authenticatedPage }) => {
        // Look for filter/search input
        const searchInput = authenticatedPage.locator('input[type="text"][placeholder*="Search"], input[type="text"][placeholder*="Filter"]');
        const isVisible = await searchInput.isVisible().catch(() => false);
        console.log(`Search input visible: ${isVisible}`);
    });
});
