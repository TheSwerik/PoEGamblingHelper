import {createSessionStore} from "./session";

export let database: IDBDatabase;
const DBOpenRequest = window.indexedDB.open('GemCorruptionResults', 1);
DBOpenRequest.onerror = event => {
    throw event;
}
DBOpenRequest.onsuccess = _ => database = DBOpenRequest.result;


DBOpenRequest.onupgradeneeded = _ => {
    DBOpenRequest.onerror = event => {
        throw event;
    }

    createSessionStore(database);
};