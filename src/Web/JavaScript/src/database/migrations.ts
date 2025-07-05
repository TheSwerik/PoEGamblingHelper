export function migrateFromVersion(db: IDBDatabase, previousVersion: number): void {
    switch (previousVersion) {
        case 0:
            migrateFromScratch(db);
            break;
    }
}

function migrateFromScratch(db: IDBDatabase): void {

}