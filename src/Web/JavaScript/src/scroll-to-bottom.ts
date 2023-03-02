declare global {
    // noinspection JSUnusedGlobalSymbols
    interface Window {
        scrollInfoService: ScrollInfoService;

        RegisterScrollInfoService(scrollInfoService: ScrollInfoService): void;

        UnRegisterScrollInfoService(): void;
    }
}


window.onscroll = () => {
    if (window.scrollInfoService !== undefined)
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight)
            window.scrollInfoService.invokeMethodAsync('OnScrollToBottom', window.scrollY);
}


window.RegisterScrollInfoService = (scrollInfoService: any) => window.scrollInfoService = scrollInfoService;

window.UnRegisterScrollInfoService = UnRegisterScrollInfoService;
export function UnRegisterScrollInfoService() {
    window.scrollInfoService = undefined;
}

interface ScrollInfoService {
    invokeMethodAsync: Function
}
