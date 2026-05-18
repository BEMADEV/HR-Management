import { PtoAccrualScheduleDescription, PtoAllocationSourceTypeDescription } from "../src/enums.partial";

describe("HR Management Enum Descriptions", () => {
    describe("PtoAccrualScheduleDescription", () => {
        it("should have correct description for None", () => {
            expect(PtoAccrualScheduleDescription[0]).toBe("None");
        });

        it("should have correct description for Yearly", () => {
            expect(PtoAccrualScheduleDescription[1]).toBe("Yearly");
        });

        it("should have correct description for Quarterly", () => {
            expect(PtoAccrualScheduleDescription[2]).toBe("Quarterly");
        });

        it("should have correct description for Monthly", () => {
            expect(PtoAccrualScheduleDescription[3]).toBe("Monthly");
        });

        it("should have correct description for Weekly", () => {
            expect(PtoAccrualScheduleDescription[4]).toBe("Weekly");
        });
    });

    describe("PtoAllocationSourceTypeDescription", () => {
        it("should have correct description for Automatic", () => {
            expect(PtoAllocationSourceTypeDescription[1]).toBe("Automatic");
        });

        it("should have correct description for Manual", () => {
            expect(PtoAllocationSourceTypeDescription[2]).toBe("Manual");
        });

        it("should have correct description for Request", () => {
            expect(PtoAllocationSourceTypeDescription[3]).toBe("Request");
        });
    });
});
