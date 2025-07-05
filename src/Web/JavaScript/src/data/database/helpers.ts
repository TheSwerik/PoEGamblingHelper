const waitInterval = 50;

export async function WaitForReady(request: IDBRequest<IDBValidKey>): Promise<void> {
    while (request.readyState === 'pending') {
        await new Promise(() => setTimeout(() => {
            console.log("WaitTING")
        }, waitInterval))
    }
}