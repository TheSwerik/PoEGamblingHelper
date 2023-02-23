window.onscroll = () => {
    if (window.scrollInfoService != null)
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight)
            window.scrollInfoService.invokeMethodAsync('OnScrollToBottom', window.scrollY);
}
window.RegisterScrollInfoService = scrollInfoService => window.scrollInfoService = scrollInfoService;
window.UnRegisterScrollInfoService = () => window.scrollInfoService = undefined;