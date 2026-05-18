/** @type {import('jest').Config} */
const config = {
    preset: 'ts-jest',
    testEnvironment: 'jsdom',
    roots: ['<rootDir>/tests'],
    testMatch: ['**/*.spec.ts'],
    moduleFileExtensions: ['ts', 'tsx', 'js', 'jsx', 'json', 'vue'],
    transform: {
        '^.+\\.tsx?$': ['ts-jest', {
            tsconfig: 'tests/tsconfig.json'
        }],
        '^.+\\.vue$': '@vue/vue3-jest',
        '^.+\\.obs$': '@vue/vue3-jest'
    },
    moduleNameMapper: {
        '^@Obsidian/(.*)$': '<rootDir>/node_modules/@rockrms/obsidian-framework/types/$1',
        '^@/(.*)$': '<rootDir>/src/$1'
    },
    testPathIgnorePatterns: [
        '/node_modules/',
        '/dist/'
    ],
    collectCoverageFrom: [
        'src/**/*.{ts,vue,obs}',
        '!src/**/*.d.ts',
        '!src/**/types.partial.ts'
    ],
    coverageDirectory: 'coverage',
    coverageReporters: ['text', 'lcov', 'html'],
    setupFilesAfterEnv: ['<rootDir>/tests/setup.ts'],
    verbose: true
};

module.exports = config;
