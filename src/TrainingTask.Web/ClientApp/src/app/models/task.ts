import { StatusTask } from './statusTaskEnum';

export class Task {
    id: number;
    name: string;
    // workHours: number;
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

    private _workHours: number;

    get workHours(): any {
        return this._workHours;
    }

    set workHours(newValue: any) {
        if (typeof newValue === 'number') {
            debugger;
            this._workHours = newValue;
        }
        else if (typeof newValue === 'string') {
            debugger;
            let partStr = newValue.split(":");
            this._workHours = parseInt(partStr[0], 10) + parseInt(partStr[1], 10) / 60.0;
        }
    }
}