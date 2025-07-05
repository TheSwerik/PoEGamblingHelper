import {Session} from "./entities";
import {database} from "./database";

export interface ISessionRepository {
    createSessionStore(): void,

    createSession(): Promise<Session>
}

class SessionRepository implements ISessionRepository {
    readonly storeName: string = 'Sessions';

    constructor(private database: IDBDatabase) {
    }

    createSessionStore(): void {
        const objectStore = database.createObjectStore(this.storeName, {autoIncrement: true, keyPath: 'id'});
        objectStore.createIndex('timestamp', 'timestamp', {unique: false});
        objectStore.createIndex('results', 'results', {unique: false});
    }

    async createSession(): Promise<Session> {
        const session: Session = {
            results: [],
            timestamp: new Date(),
            id: 187187
        }

        // open a read/write db transaction, ready for adding the data
        const transaction = database.transaction([this.storeName], "readwrite");

        // create an object store on the transaction
        const objectStore = transaction.objectStore(this.storeName);

        // add our newItem object to the object store
        const request = objectStore.add(session);

        await WaitForReady();

        async function WaitForReady() {
            while (request.readyState === 'pending') {
                await new Promise(() => setTimeout(() => {
                    console.log("WaitTING")
                }, 2000))
            }
        }

        console.log(request)
        console.log(request.result)

        // @ts-ignore
        return request.result as Session;
    }
}

export let instance: ISessionRepository;

export function initSessionRepository(database: IDBDatabase): void {
    instance = new SessionRepository(database);
}