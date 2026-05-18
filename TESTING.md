# HR Management Testing

This document describes the testing infrastructure for the HR Management plugin.

## Test Projects Overview

The HR Management plugin has three testing layers:

1. **C# Unit Tests** (`com.bemaservices.HrManagement.Tests`)
   - Tests for models, services, and business logic
   - Uses MSTest framework with Moq for mocking

2. **Obsidian/Vue Unit Tests** (`com.bemaservices.HrManagement.Obsidian/tests`)
   - Tests for Vue components and TypeScript utilities
   - Uses Jest with Vue Test Utils

3. **E2E Tests** (`com.bemaservices.HrManagement.E2E`)
   - End-to-end browser tests for full user workflows
   - Uses Playwright for cross-browser testing

---

## C# Unit Tests

### Setup

The test project is located at `com.bemaservices.HrManagement.Tests/`.

### Running Tests

```bash
# From the HrManagement directory
dotnet test com.bemaservices.HrManagement.Tests

# With verbose output
dotnet test com.bemaservices.HrManagement.Tests --verbosity normal

# Run specific test
dotnet test com.bemaservices.HrManagement.Tests --filter "FullyQualifiedName~PtoTypeTests"
```

### Test Structure

```
com.bemaservices.HrManagement.Tests/
├── Model/
│   ├── PtoTypeTests.cs
│   └── PtoAllocationTests.cs
├── Enums/
│   └── EnumTests.cs
└── Services/
    └── (future service tests)
```

---

## Obsidian/Vue Unit Tests

### Setup

```bash
cd com.bemaservices.HrManagement.Obsidian
npm install
```

### Running Tests

```bash
# Run all tests
npm test

# Run tests in watch mode
npm test -- --watch

# Run tests with coverage
npm test -- --coverage

# Run specific test file
npm test -- tests/sample.spec.ts
```

### Test Structure

```
com.bemaservices.HrManagement.Obsidian/
├── tests/
│   ├── setup.ts              # Jest setup and mocks
│   ├── sample.spec.ts        # Enum tests
│   └── enumDescriptions.spec.ts
├── jest.config.js            # Jest configuration
└── package.json
```

### Writing New Tests

```typescript
import { mount } from '@vue/test-utils';
import MyComponent from '../src/MyComponent.obs';

describe('MyComponent', () => {
    it('renders correctly', () => {
        const wrapper = mount(MyComponent, {
            props: {
                // component props
            }
        });
        expect(wrapper.exists()).toBe(true);
    });
});
```

---

## E2E Tests (Playwright)

### Setup

```bash
cd com.bemaservices.HrManagement.E2E
npm install
npx playwright install
```

### Configuration

1. Copy `.env.example` to `.env`
2. Update the environment variables with your Rock instance details:

```env
ROCK_BASE_URL=http://localhost:6229
ROCK_ADMIN_USERNAME=admin
ROCK_ADMIN_PASSWORD=admin
PTO_TYPES_PAGE=/admin/plugins/hr-management/pto-types
```

### Running Tests

```bash
# Run all tests
npm test

# Run tests in headed mode (see the browser)
npm run test:headed

# Run tests with UI mode
npm run test:ui

# Debug tests
npm run test:debug

# View test report
npm run report
```

### Test Structure

```
com.bemaservices.HrManagement.E2E/
├── tests/
│   ├── fixtures/
│   │   └── auth.ts           # Authentication helpers
│   ├── ptoTypeList.spec.ts   # PTO Type page tests
│   └── allPages.spec.ts      # General page tests
├── playwright.config.ts      # Playwright configuration
└── package.json
```

### Writing New E2E Tests

```typescript
import { test, expect, navigateToHrManagement } from './fixtures/auth';

test.describe('My Feature', () => {
    test('should work correctly', async ({ authenticatedPage }) => {
        await navigateToHrManagement(authenticatedPage, '/my-page');
        
        // Your test assertions
        await expect(authenticatedPage.locator('.my-element')).toBeVisible();
    });
});
```

---

## CI/CD Integration

### GitHub Actions Example

```yaml
name: HR Management Tests

on: [push, pull_request]

jobs:
  unit-tests:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '4.7.2'
      
      - name: Run C# Tests
        run: dotnet test HrManagement/com.bemaservices.HrManagement.Tests

  frontend-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup Node
        uses: actions/setup-node@v3
        with:
          node-version: '18'
      
      - name: Install dependencies
        run: |
          cd HrManagement/com.bemaservices.HrManagement.Obsidian
          npm install
      
      - name: Run Jest tests
        run: |
          cd HrManagement/com.bemaservices.HrManagement.Obsidian
          npm test

  e2e-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup Node
        uses: actions/setup-node@v3
        with:
          node-version: '18'
      
      - name: Install Playwright
        run: |
          cd HrManagement/com.bemaservices.HrManagement.E2E
          npm install
          npx playwright install --with-deps
      
      - name: Run E2E tests
        run: |
          cd HrManagement/com.bemaservices.HrManagement.E2E
          npm test
        env:
          ROCK_BASE_URL: ${{ secrets.ROCK_BASE_URL }}
          ROCK_ADMIN_USERNAME: ${{ secrets.ROCK_ADMIN_USERNAME }}
          ROCK_ADMIN_PASSWORD: ${{ secrets.ROCK_ADMIN_PASSWORD }}
```

---

## Best Practices

1. **Test Naming**: Use descriptive names that explain what is being tested
   - `PtoType_DefaultValues_ShouldBeCorrect`
   - `should display the PTO Types grid`

2. **Test Isolation**: Each test should be independent and not rely on other tests

3. **Mock External Dependencies**: Use Moq for C# and Jest mocks for TypeScript

4. **Coverage Goals**: Aim for at least 80% code coverage on business logic

5. **E2E Tests**: Focus on critical user workflows, not implementation details
