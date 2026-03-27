// This code was auto-generated, any manual changes made will be lost.

export const PtoAccrualSchedule = {
    None: 0,

    Yearly: 1,

    Quarterly: 2,

    Monthly: 3,

    Weekly: 4
} as const;

export const PtoAccrualScheduleDescription: Record<number, string> = {
    0: "None",

    1: "Yearly",

    2: "Quarterly",

    3: "Monthly",

    4: "Weekly"
};

export type PtoAccrualSchedule = typeof PtoAccrualSchedule[keyof typeof PtoAccrualSchedule];

export const PtoAllocationSourceType = {
    Automatic: 1,

    Manual: 2,

    Request: 3
} as const;

export const PtoAllocationSourceTypeDescription: Record<number, string> = {
    1: "Automatic",

    2: "Manual",

    3: "Request"
};

export type PtoAllocationSourceType = typeof PtoAllocationSourceType[keyof typeof PtoAllocationSourceType];

export const PtoAllocationStatus = {
    Inactive: 0,

    Active: 1,

    Pending: 2,

    Denied: 3
} as const;

export const PtoAllocationStatusDescription: Record<number, string> = {
    0: "Inactive",

    1: "Active",

    2: "Pending",

    3: "Denied"
};

export type PtoAllocationStatus = typeof PtoAllocationStatus[keyof typeof PtoAllocationStatus];
