const pathname = window.location.pathname;

document.querySelector('a[href="' + pathname + '"]').classList.add('active');
