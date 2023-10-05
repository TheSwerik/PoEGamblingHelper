// noinspection JSUnusedGlobalSymbols
function addTooltips() {
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    // noinspection JSUnusedLocalSymbols
    const tooltipList = [...tooltipTriggerList].map(tooltipTriggerElement => new bootstrap.Tooltip(tooltipTriggerElement));
}