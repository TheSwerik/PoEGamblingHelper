import {Result, ResultEntry} from "../entities";
import {WaitForReady} from "../database/helpers";

export let instance: IResultRepository;
export const storeName: string = 'Results';

export function init(database: IDBDatabase): void {
    instance = new ResultRepository(database);
}

export interface IResultRepository {
    create(data: CreateResultEntry): Promise<ResultEntry>
}

export interface CreateResultEntry {
    sessionId: number | undefined;
    gemId: number;
    result: Result;
    gemCost: number;
    resultPrice: number;
    templeCost: number;
}

class ResultRepository implements IResultRepository {
    constructor(private readonly database: IDBDatabase) {
    }

    async create(data: CreateResultEntry): Promise<ResultEntry> {
        const entry: Omit<ResultEntry, 'id'> = {
            timestamp: new Date(),
            sessionId: data.sessionId,
            gemId: data.gemId,
            result: data.result,
            gemCost: data.gemCost,
            resultPrice: data.resultPrice,
            templeCost: data.templeCost,
        }

        const transaction = this.database.transaction([storeName], "readwrite");
        const objectStore = transaction.objectStore(storeName);

        const request = objectStore.add(entry);
        await WaitForReady(request);

        return {
            id: request.result as number,
            ...entry
        };
    }
}