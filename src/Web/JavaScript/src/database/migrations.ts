import {instance as sessionRepository} from "./sessionRepository";

export function migrateFromVersion(database: IDBDatabase, previousVersion: number): void {
    if (previousVersion <= 0) migration1(database);
    if (previousVersion < 1) migration2(database);
}

function migration1(database: IDBDatabase): void {
    sessionRepository.createSessionStore(database);
}

function migration2(database: IDBDatabase): void {

}