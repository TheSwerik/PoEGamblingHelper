const waitInterval = 50;
const delay = (durationMs: number) => {
    return new Promise(resolve => setTimeout(resolve, durationMs));
}

export async function WaitForReady(request: IDBRequest<IDBValidKey>): Promise<void> {
    while (request.readyState === 'pending') await delay(waitInterval);
}