import {migrateFromVersion} from "./migrations";
import {initSessionRepository} from "./sessionRepository";

export const DatabaseVersion = 1;
export const DatabaseName = 'GemCorruptionResults';
export let database: IDBDatabase;

const DBOpenRequest = window.indexedDB.open(DatabaseName, DatabaseVersion);
DBOpenRequest.onerror = event => {
    throw event;
}

// if no migration is needed:
DBOpenRequest.onsuccess = _ => {
    database = DBOpenRequest.result;
    initRepositories(database);
}

// if migration is needed:
DBOpenRequest.onupgradeneeded = migrateDatabase;

function migrateDatabase(event: IDBVersionChangeEvent) {
    // @ts-ignore
    database = event.target.result;
    initRepositories(database);
    migrateFromVersion(database, event.oldVersion);
}

function initRepositories(database: IDBDatabase) {
    initSessionRepository(database);
}