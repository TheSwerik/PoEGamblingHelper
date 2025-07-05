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
        const session: Session = {
            id: undefined,
            timestamp: new Date(),
            results: [],
        }

        const transaction = this.database.transaction([storeName], "readwrite");
        const objectStore = transaction.objectStore(storeName);

        const request: IDBRequest<IDBValidKey> = objectStore.add(session);
        await WaitForReady(request);

        console.log(request)
        console.log(request.result)

        // @ts-ignore
        return request.result as Session;
    }
}