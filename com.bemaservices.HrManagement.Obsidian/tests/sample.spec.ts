import { PtoAllocationSourceType, PtoAllocationStatus, PtoAccrualSchedule } from "../src/enums.partial";

describe("HR Management Enums", () => {
    describe("PtoAllocationSourceType", () => {
        it("should have Automatic value", () => {
            expect(PtoAllocationSourceType.Automatic).toBe(1);
        });

        it("should have Manual value", () => {
            expect(PtoAllocationSourceType.Manual).toBe(2);
        });

        it("should have Request value", () => {
            expect(PtoAllocationSourceType.Request).toBe(3);
        });
    });

    describe("PtoAllocationStatus", () => {
        it("should have Inactive value", () => {
            expect(PtoAllocationStatus.Inactive).toBe(0);
        });

        it("should have Active value", () => {
            expect(PtoAllocationStatus.Active).toBe(1);
        });
    });

    describe("PtoAccrualSchedule", () => {
        it("should have None value", () => {
            expect(PtoAccrualSchedule.None).toBe(0);
        });

        it("should have Yearly value", () => {
            expect(PtoAccrualSchedule.Yearly).toBe(1);
        });

        it("should have Quarterly value", () => {
            expect(PtoAccrualSchedule.Quarterly).toBe(2);
        });

        it("should have Monthly value", () => {
            expect(PtoAccrualSchedule.Monthly).toBe(3);
        });

        it("should have Weekly value", () => {
            expect(PtoAccrualSchedule.Weekly).toBe(4);
        });
    });
});
