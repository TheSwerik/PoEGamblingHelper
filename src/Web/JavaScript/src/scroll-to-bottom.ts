declare global {
    // noinspection JSUnusedGlobalSymbols
    interface Window {
        scrollInfoService: ScrollInfoService;
    }
}


window.onscroll = () => {
    console.log(window.scrollInfoService)
    console.log(window.scrollY)
    if (window.scrollInfoService !== undefined)
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight)
            window.scrollInfoService.invokeMethodAsync('OnScrollToBottom', window.scrollY);
}


export function RegisterScrollInfoService(scrollInfoService: any) {
    window.scrollInfoService = scrollInfoService;
}

export function UnRegisterScrollInfoService() {
    window.scrollInfoService = undefined;
}

interface ScrollInfoService {
    invokeMethodAsync: Function
}
