// noinspection JSUnusedGlobalSymbols
function getTheme() {
    return localStorage.getItem('theme');
}

function setTheme(theme) {
    const themePath = `css/theme.${theme}.css`;
    if (!CheckUrl(themePath)) return;

    const previousElements = document.head.querySelectorAll('[data-theme]');

    const element = document.createElement('link');
    element.dataset.theme = '';
    element.rel = 'stylesheet';
    element.type = 'text/css';
    element.href = themePath;
    element.onload = () => previousElements.forEach(e => e.parentNode?.removeChild(e));
    document.head.appendChild(element);

    localStorage.setItem('theme', theme);
}

// noinspection JSUnusedGlobalSymbols
function useSavedTheme() {
    const theme = localStorage.getItem('theme');
    setTheme(theme !== null ? theme : 'dark');
}

function CheckUrl(url) {
    let http;
    if (window.XMLHttpRequest) http = new XMLHttpRequest(); // IE7+, Firefox, Chrome, Opera, Safari
    else http = new ActiveXObject("Microsoft.XMLHTTP"); // IE6, IE5

    http.open('HEAD', url, false);
    http.send();
    return http.status !== 404;
}