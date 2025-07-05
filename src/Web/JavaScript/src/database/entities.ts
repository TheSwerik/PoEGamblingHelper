export interface Session {
    id: number;
    timestamp: Date;
    results: ResultEntry[];
}

export interface ResultEntry {
    id: number;
    gemId: number;
    result: Result;
    gemCost: number;
    templeCost: number;
    resultPrice: number;
    timestamp: Date;
}

export enum Result {
    RemoveLevel = -1,
    KeepLevel = 0,
    AddLevel = 1
}