import { StatusTask } from './statusTaskEnum';

export class Task {
    id: number;
    name: string;
    workHours: number;
    startDate: string;
    finishDate: string;
    status: StatusTask;
    projectId: number;
    projectAbbreviation: string;
    fullNames: string[];
    employees: number[];

    get statusName(): string {
        return StatusTask[this.status];
    }
}