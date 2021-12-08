const pathname = window.location.pathname;

if (pathname === '/') {
    document.querySelectorAll('a[href="' + pathname + '"]')[1].classList.add('active');
} else {
    document.querySelector('a[href="' + pathname + '"]').classList.add('active');
}
