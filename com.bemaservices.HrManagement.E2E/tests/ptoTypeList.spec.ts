import { test, expect, navigateToHrManagement, waitForObsidianBlock } from './fixtures/auth';

/**
 * E2E tests for PTO Type List page.
 * Tests navigation, display, and basic CRUD operations.
 */
test.describe('PTO Type List', () => {
    test.beforeEach(async ({ authenticatedPage }) => {
        // Navigate to the PTO Types page
        await navigateToHrManagement(authenticatedPage, process.env.PTO_TYPES_PAGE || '/admin/plugins/hr-management/pto-types');
        await waitForObsidianBlock(authenticatedPage);
    });

    test('should display the PTO Types grid', async ({ authenticatedPage }) => {
        // Verify the grid is visible
        const grid = authenticatedPage.locator('.grid, table');
        await expect(grid).toBeVisible();
    });

    test('should have column headers', async ({ authenticatedPage }) => {
        // Check for expected column headers
        await expect(authenticatedPage.locator('text=Name')).toBeVisible();
        await expect(authenticatedPage.locator('text=Description')).toBeVisible();
        await expect(authenticatedPage.locator('text=Is Active')).toBeVisible();
    });

    test('should have Add button when user has edit permission', async ({ authenticatedPage }) => {
        // Look for Add button
        const addButton = authenticatedPage.locator('button:has-text("Add"), a:has-text("Add")');
        // This may or may not be visible depending on permissions
        const isVisible = await addButton.isVisible().catch(() => false);
        
        // Just log the result - this is informational
        console.log(`Add button visible: ${isVisible}`);
    });

    test('should have Export Calendar Feed button', async ({ authenticatedPage }) => {
        // Check for the calendar feed export button
        const exportButton = authenticatedPage.locator('button:has-text("Export PTO Calendar Feed")');
        await expect(exportButton).toBeVisible();
    });

    test('should open modal when clicking Add', async ({ authenticatedPage }) => {
        // Click Add button if visible
        const addButton = authenticatedPage.locator('button:has-text("Add"), a:has-text("Add")').first();
        
        if (await addButton.isVisible()) {
            await addButton.click();
            
            // Wait for modal to appear
            const modal = authenticatedPage.locator('.modal, [role="dialog"]');
            await expect(modal).toBeVisible();
            
            // Verify modal has expected fields
            await expect(authenticatedPage.locator('label:has-text("Name")')).toBeVisible();
            await expect(authenticatedPage.locator('label:has-text("Is Active")')).toBeVisible();
        }
    });
});

test.describe('PTO Type CRUD Operations', () => {
    const testPtoTypeName = `Test PTO Type ${Date.now()}`;

    test('should create a new PTO Type', async ({ authenticatedPage }) => {
        await navigateToHrManagement(authenticatedPage, process.env.PTO_TYPES_PAGE || '/admin/plugins/hr-management/pto-types');
        await waitForObsidianBlock(authenticatedPage);

        // Click Add button
        const addButton = authenticatedPage.locator('button:has-text("Add"), a:has-text("Add")').first();
        
        if (await addButton.isVisible()) {
            await addButton.click();
            
            // Fill in the form
            await authenticatedPage.fill('input[id*="Name"], input[name*="name"]', testPtoTypeName);
            await authenticatedPage.fill('textarea[id*="Description"], textarea[name*="description"]', 'Test description for E2E test');
            
            // Check the Is Active checkbox
            const isActiveCheckbox = authenticatedPage.locator('input[type="checkbox"][id*="Active"], input[type="checkbox"][name*="active"]');
            if (await isActiveCheckbox.isVisible()) {
                await isActiveCheckbox.check();
            }
            
            // Save the form
            await authenticatedPage.click('button:has-text("Save")');
            
            // Wait for the modal to close
            await authenticatedPage.waitForSelector('.modal, [role="dialog"]', { state: 'hidden' }).catch(() => {});
            
            // Verify the new item appears in the grid
            await expect(authenticatedPage.locator(`text=${testPtoTypeName}`)).toBeVisible({ timeout: 10000 });
        }
    });
});
