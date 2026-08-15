export interface ResultQuery {
    name?: string;
    dateFrom?: Date;
    dateTo?: Date;
    valueFrom?: number;
    valueTo?: number;
    executionTimeFrom?:number;
    executionTimeTo?:number;
}