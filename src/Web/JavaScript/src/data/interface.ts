import {Session} from "./entities";
import {instance as sessionRepository} from "./repositories/sessionRepository";

declare global {
    // noinspection JSUnusedGlobalSymbols
    interface Window {
        createSession(): Promise<Session>;
    }
}

window.createSession = async function (): Promise<Session> {
    return await sessionRepository.create();
}
window.createSession = async function (): Promise<Session> {
    return await sessionRepository.create();
}