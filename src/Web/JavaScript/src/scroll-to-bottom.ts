declare global {
    // noinspection JSUnusedGlobalSymbols
    interface Window {
        scrollInfoService: ScrollInfoService;

        RegisterScrollInfoService(scrollInfoService: ScrollInfoService): void;

        UnRegisterScrollInfoService(): void;
    }
}

function getMain() {
    return document.getElementsByTagName('main')[0];
}

function onScroll() {
    const main = getMain();
    if (window.scrollInfoService !== undefined)
        if ((window.innerHeight + main.scrollTop) >= main.scrollHeight)
            window.scrollInfoService.invokeMethodAsync('OnScrollToBottom', main.scrollTop);
}


window.RegisterScrollInfoService = function (scrollInfoService: any) {
    window.scrollInfoService = scrollInfoService;
    getMain().addEventListener("scroll", onScroll);
}

window.UnRegisterScrollInfoService = UnRegisterScrollInfoService;

export function UnRegisterScrollInfoService() {
    getMain().removeEventListener("scroll", onScroll);
    window.scrollInfoService = undefined;
}

interface ScrollInfoService {
    invokeMethodAsync: Function
}
