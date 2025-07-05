import {Session} from "./entities";
import {database} from "./database";

export function createSessionStore(db: IDBDatabase): void {
    const objectStore = db.createObjectStore('Sessions', {autoIncrement: true, keyPath: 'id'});

    // Define what data items the objectStore will contain
    objectStore.createIndex('timestamp', 'timestamp', {unique: false});
    objectStore.createIndex('results', 'results', {unique: false});
}

function createSession(): Session {
    const session: Session = {}

    // open a read/write db transaction, ready for adding the data
    const transaction = database.transaction(["toDoList"], "readwrite");

    // report on the success of opening the transaction
    transaction.oncomplete = (event) => {
        note.appendChild(document.createElement("li")).textContent =
            "Transaction opened for task addition.";
    };

    transaction.onerror = (event) => {
        note.appendChild(document.createElement("li")).textContent =
            "Transaction not opened due to error. Duplicate items not allowed.";
    };

    // create an object store on the transaction
    const objectStore = transaction.objectStore("toDoList");

    // add our newItem object to the object store
    const request = objectStore.add(newItem[0]);
}