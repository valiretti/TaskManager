export enum Status {
    NotStarted = 0,
    InProgress = 1,
    Completed = 2,
    Postponed = 3
};

export class Task {
    id: number;
    name: string;
    workHours: number;
    startDate: string;
    finishDate: string;
    status: Status;    
    projectId: number;
    projectAbbreviation: string;
    fullNames: string[];
    employeeIds: number[];

    get statusName(): string {
        return Status[this.status];
    }
}