function useTheme(theme) {
    const themePath = `css/${theme}.theme.css`;
    if (!CheckUrl(themePath)) return;

    const previousElement = document.getElementById('theme');
    document.getElementsByTagName('head')[0].removeChild(previousElement);

    const element = document.createElement('link');
    element.id = 'theme';
    element.rel = 'stylesheet';
    element.type = 'text/css';
    element.href = themePath;
    // element.setAttribute('id', 'theme');
    // element.setAttribute('rel', 'stylesheet');
    // element.setAttribute('type', 'text/css');
    // element.setAttribute('href', 'css/' + theme.toLowerCase() + '.theme.css');
    // element.setAttribute('href', themePath);
    document.getElementsByTagName('head')[0].appendChild(element);

    localStorage.setItem('theme', theme);
}

function useSavedTheme() {
    const theme = localStorage.getItem('theme');
    if (theme !== null) useTheme(theme); else useTheme('dark');
}

function CheckUrl(url) {
    let http;
    if (window.XMLHttpRequest) http = new XMLHttpRequest(); // IE7+, Firefox, Chrome, Opera, Safari
    else http = new ActiveXObject("Microsoft.XMLHTTP"); // IE6, IE5

    http.open('HEAD', url, false);
    http.send();
    return http.status !== 404;
}