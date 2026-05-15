// This code was auto-generated, any manual changes made will be lost.

import { PtoAllocationSourceType, PtoAllocationStatus } from "./enums.partial";
import { ListItemBag } from "@Obsidian/ViewModels/Utility/listItemBag";
import { PublicAttributeBag } from "@Obsidian/ViewModels/Utility/publicAttributeBag";

export type PtoAllocationBag = {
    attributes?: Record<string, PublicAttributeBag> | null;

    attributeValues?: Record<string, string> | null;

    endDate?: string | null;

    hours: number;

    idKey?: string | null;

    note?: string | null;

    personAlias?: ListItemBag | null;

    ptoAllocationSourceType: PtoAllocationSourceType;

    ptoAllocationStatus: PtoAllocationStatus;

    ptoType?: ListItemBag | null;

    startDate?: string | null;
};

export type PtoAllocationDetailOptionsBag = {
    ptoTypes?: ListItemBag[] | null;
};

export type PtoAllocationListOptionsBag = {
};

export type PtoBracketBag = {
    attributes?: Record<string, PublicAttributeBag> | null;

    attributeValues?: Record<string, string> | null;

    idKey?: string | null;

    isActive: boolean;

    maximumYear?: number | null;

    minimumYear: number;
};

export type PtoBracketDetailOptionsBag = {
};

export type PtoBracketListOptionsBag = {

    /** Gets or sets a value indicating whether the block should be displayed to the user. */
    isBlockVisible: boolean;
};

export type PtoRequestListOptionsBag = {
    hasPersonContext: boolean;

    contextPersonIdKey?: string | null;
};

export type PtoTierBag = {
    attributes?: Record<string, PublicAttributeBag> | null;

    attributeValues?: Record<string, string> | null;

    color?: string | null;

    daysOfWeek?: string[] | null;

    description?: string | null;

    idKey?: string | null;

    isActive: boolean;

    name?: string | null;
};

export type PtoTierDetailOptionsBag = {
};

export type PtoTierListOptionsBag = {
};

export type PtoTypeBag = {
    attributes?: Record<string, PublicAttributeBag> | null;

    attributeValues?: Record<string, string> | null;

    color?: string | null;

    description?: string | null;

    id: number;

    idKey?: string | null;

    isActive: boolean;

    isNegativeTimeBalanceAllowed: boolean;

    name?: string | null;

    workflowType?: ListItemBag | null;
};

export type PtoTypeListOptionsBag = {
    ptoCalendarFeedUrl?: string | null;
};

export type HrEmployeeListOptionsBag = {
    ptoTypes?: ListItemBag[] | null;

    fiscalYears?: ListItemBag[] | null;

    showSupervisorFilter: boolean;

    showMinistryAreaFilter: boolean;
};
