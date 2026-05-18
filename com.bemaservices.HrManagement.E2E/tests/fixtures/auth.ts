import { test as base, expect, Page } from '@playwright/test';

/**
 * Rock authentication fixture for E2E tests.
 * Provides logged-in page instances for testing HR Management features.
 */

interface RockAuthFixtures {
    authenticatedPage: Page;
}

/**
 * Extended test object with authentication fixtures.
 */
export const test = base.extend<RockAuthFixtures>({
    authenticatedPage: async ({ page }, use) => {
        // Navigate to login page
        await page.goto('/');
        
        // Check if already logged in by looking for admin menu
        const isLoggedIn = await page.locator('.navbar-admin').isVisible().catch(() => false);
        
        if (!isLoggedIn) {
            // Click login link
            await page.click('text=Login');
            
            // Fill in credentials
            await page.fill('input[name="UserName"], input[id*="tbUserName"]', process.env.ROCK_ADMIN_USERNAME || 'admin');
            await page.fill('input[name="Password"], input[id*="tbPassword"]', process.env.ROCK_ADMIN_PASSWORD || 'admin');
            
            // Submit login form
            await page.click('button[type="submit"], input[type="submit"]');
            
            // Wait for navigation to complete
            await page.waitForLoadState('networkidle');
        }
        
        // Provide the authenticated page to the test
        await use(page);
    },
});

export { expect };

/**
 * Helper function to navigate to HR Management pages.
 */
export async function navigateToHrManagement(page: Page, path: string): Promise<void> {
    const baseUrl = process.env.ROCK_BASE_URL || 'http://localhost:6229';
    await page.goto(`${baseUrl}${path}`);
    await page.waitForLoadState('networkidle');
}

/**
 * Helper function to wait for Obsidian block to load.
 */
export async function waitForObsidianBlock(page: Page): Promise<void> {
    // Wait for Vue app to mount
    await page.waitForSelector('[data-v-app]', { timeout: 10000 }).catch(() => {
        // Fallback: wait for any grid or form element
        return page.waitForSelector('.grid, .panel, form', { timeout: 10000 });
    });
}
