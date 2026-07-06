# HR Management Testing

This document describes the testing infrastructure for the HR Management plugin.

## Test Projects Overview

The HR Management plugin currently has two testing layers:

1. **C# Unit Tests** (`com.bemaservices.HrManagement.Tests`)
   - Tests for models, services, and business logic
   - Uses MSTest framework with Moq for mocking

2. **Obsidian/Vue Unit Tests** (`com.bemaservices.HrManagement.Obsidian/tests`)
   - Tests for Vue components and TypeScript utilities
   - Uses Jest with Vue Test Utils

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

## E2E Tests

E2E tests are currently disabled and the `com.bemaservices.HrManagement.E2E` project has been removed for now.

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

```

---

## Best Practices

1. **Test Naming**: Use descriptive names that explain what is being tested
   - `PtoType_DefaultValues_ShouldBeCorrect`
   - `should display the PTO Types grid`

2. **Test Isolation**: Each test should be independent and not rely on other tests

3. **Mock External Dependencies**: Use Moq for C# and Jest mocks for TypeScript

4. **Coverage Goals**: Aim for at least 80% code coverage on business logic

5. **Future E2E Coverage**: When E2E tests are reintroduced, focus on critical user workflows, not implementation details
