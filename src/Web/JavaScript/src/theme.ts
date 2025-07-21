declare global {
    // noinspection JSUnusedGlobalSymbols
    interface Window {
        getTheme(): string;

        setTheme(theme: 'dark' | 'light'): void;

        useSavedTheme(): void;
    }
}

window.getTheme = () => localStorage.getItem('theme');

window.setTheme = (theme: 'dark' | 'light') => {
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

window.useSavedTheme = useSavedTheme;

export function useSavedTheme() {
    const theme = localStorage.getItem('theme');
    if (theme !== 'dark' && theme !== 'light') {
        localStorage.removeItem('theme');
        return;
    }
    window.setTheme(theme !== null ? theme : 'dark');
}

function CheckUrl(url: string) {
    let http;
    if (window.XMLHttpRequest) http = new XMLHttpRequest(); // IE7+, Firefox, Chrome, Opera, Safari
    else http = new ActiveXObject("Microsoft.XMLHTTP"); // IE6, IE5

    http.open('HEAD', url, false);
    http.send();
    return http.status !== 404;
}