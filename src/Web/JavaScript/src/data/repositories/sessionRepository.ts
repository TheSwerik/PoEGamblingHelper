import {Session} from "../entities";
import {WaitForReady} from "../database/helpers";


export let instance: ISessionRepository;
export const storeName: string = 'Sessions';

export function init(database: IDBDatabase): void {
    instance = new SessionRepository(database);
}

export interface ISessionRepository {
    create(): Promise<Session>
}

class SessionRepository implements ISessionRepository {
    constructor(private readonly database: IDBDatabase) {
    }

    async create(): Promise<Session> {
        const session: Omit<Session, 'id'> = {
            timestamp: new Date(),
            results: [],
        }

        const transaction = this.database.transaction([storeName], "readwrite");
        const objectStore = transaction.objectStore(storeName);

        const request = objectStore.add(session);
        await WaitForReady(request);

        return {
            id: request.result as number,
            ...session
        };
    }
}