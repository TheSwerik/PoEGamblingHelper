import {Result, ResultEntry} from "../entities";
import {WaitForReady} from "../database/helpers";

export let instance: IResultRepository;
export const storeName: string = 'Results';

export function init(database: IDBDatabase): void {
    instance = new ResultRepository(database);
}

export interface IResultRepository {
    createResultEntry(data: CreateResultEntry): Promise<ResultEntry>
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

    async createResultEntry(data: CreateResultEntry): Promise<ResultEntry> {
        const entry: ResultEntry = {
            id: undefined,
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

        console.log(request)
        console.log(request.result)

        // @ts-ignore
        return request.result as Result;
    }
}