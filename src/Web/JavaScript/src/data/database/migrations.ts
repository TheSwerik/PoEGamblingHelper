import {storeName as sessionStoreName} from "../repositories/sessionRepository";
import {storeName as resultEntryStoreName} from "../repositories/resultEntryRepository";

export function migrateFromVersion(database: IDBDatabase, previousVersion: number): void {
    if (previousVersion <= 0) migration1(database);
    if (previousVersion < 1) migration2(database);
}

function migration1(database: IDBDatabase): void {
    const sessionObjectStore = database.createObjectStore(sessionStoreName, {
        autoIncrement: true,
        keyPath: 'id'
    });
    sessionObjectStore.createIndex('id', 'id', {unique: true});
    sessionObjectStore.createIndex('timestamp', 'timestamp', {unique: false});
    sessionObjectStore.createIndex('results', 'results', {unique: false});

    const resultEntryObjectStore = database.createObjectStore(resultEntryStoreName, {
        autoIncrement: true,
        keyPath: 'id'
    });
    resultEntryObjectStore.createIndex('id', 'id', {unique: true});
    resultEntryObjectStore.createIndex('gemId', 'gemId', {unique: false});
    resultEntryObjectStore.createIndex('timestamp', 'timestamp', {unique: false});
    resultEntryObjectStore.createIndex('result', 'result', {unique: false});
    resultEntryObjectStore.createIndex('gemCost', 'gemCost', {unique: false});
    resultEntryObjectStore.createIndex('templeCost', 'templeCost', {unique: false});
    resultEntryObjectStore.createIndex('resultPrice', 'resultPrice', {unique: false});
    resultEntryObjectStore.createIndex('sessionId', 'sessionId', {unique: false});
}

function migration2(database: IDBDatabase): void {

}