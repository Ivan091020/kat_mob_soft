// site.js — простой скрипт для мобильного гамбургера и корректной работы swipe/card UX
document.addEventListener('DOMContentLoaded', function () {
    const hamburger = document.querySelector('.hamburger');
    const navList = document.querySelector('.nav-list');
    const navLinks = document.querySelectorAll('.nav-list a');

    if (hamburger && navList) {
        // Toggle menu open/close
        hamburger.addEventListener('click', function (e) {
            navList.classList.toggle('open');
            hamburger.classList.toggle('is-open');
        });

        // Close menu when a nav link is clicked (mobile UX)
        navLinks.forEach(link => {
            link.addEventListener('click', function () {
                navList.classList.remove('open');
                hamburger.classList.remove('is-open');
            });
        });

        // Close menu when clicking outside (optional, improves UX)
        document.addEventListener('click', function (e) {
            if (!navList.contains(e.target) && !hamburger.contains(e.target)) {
                navList.classList.remove('open');
                hamburger.classList.remove('is-open');
            }
        });
    }

    // Дополнительно: предотвращаем конфликт JS-стрелок с touch swipe.
    // Если у тебя есть обработчики стрелок, не перехватывающие touch события — всё ок.
    // Если есть код, который ставит preventDefault на touchmove, надо убрать — ниже простой guard:
    function allowNativeTouchOnServices() {
        const servicesTrack = document.querySelector('.services-track');
        if (!servicesTrack) return;
        // Убедимся, что на контейнер не навешаны обработчики, блокирующие touch.
        // (Если у тебя есть кастомный скрипт, удаляй там preventDefault для тач-событий)
        // Ничего не делаем здесь — просто рекомендация.
    }
    allowNativeTouchOnServices();
});
// site.js — hamburger + robust touch/mouse drag for .services-track
document.addEventListener('DOMContentLoaded', function () {
    // ======== hamburger toggle (оставляем как раньше, с небольшой защитой) ========
    const hamburger = document.querySelector('.hamburger');
    const navList = document.querySelector('.nav-list');

    if (hamburger && navList) {
        hamburger.addEventListener('click', function (e) {
            e.stopPropagation();
            navList.classList.toggle('open');
            hamburger.classList.toggle('is-open');
        });

        // закрываем при клике вне меню
        document.addEventListener('click', function (e) {
            if (!navList.contains(e.target) && !hamburger.contains(e.target)) {
                navList.classList.remove('open');
                hamburger.classList.remove('is-open');
            }
        });

        // закрываем при клике на ссылку меню (для мобильного)
        navList.querySelectorAll('a').forEach(a => {
            a.addEventListener('click', () => {
                navList.classList.remove('open');
                hamburger.classList.remove('is-open');
            });
        });
    }

    // ======== touch / mouse drag для .services-track (fallback + улучшение UX) ========
    (function enableServicesTouchDrag() {
        const track = document.querySelector('.services-track');
        if (!track) return;

        // Если уже подключен обработчик — не дублировать
        if (track.__touchDragAttached) return;
        track.__touchDragAttached = true;

        // настройки
        let isDown = false;
        let startX = 0;
        let scrollStart = 0;
        let isTouch = false; // distinguish touch vs mouse

        // Touch start
        track.addEventListener('touchstart', function (e) {
            isTouch = true;
            isDown = true;
            startX = e.touches[0].pageX;
            scrollStart = track.scrollLeft;
            // если нужно — убрать any pointer capture from other scripts
        }, { passive: true });

        // Touch move
        track.addEventListener('touchmove', function (e) {
            if (!isDown) return;
            const x = e.touches[0].pageX;
            const dx = startX - x;
            track.scrollLeft = scrollStart + dx;
        }, { passive: true });

        // Touch end / cancel
        track.addEventListener('touchend', function () {
            isDown = false;
        });
        track.addEventListener('touchcancel', function () {
            isDown = false;
        });

        // Mouse drag (desktop optional)
        track.addEventListener('mousedown', function (e) {
            // ignore if touch already active
            if (isTouch) return;
            isDown = true;
            startX = e.pageX;
            scrollStart = track.scrollLeft;
            track.classList.add('is-dragging');
            e.preventDefault();
        });

        track.addEventListener('mousemove', function (e) {
            if (!isDown || isTouch) return;
            const x = e.pageX;
            const dx = startX - x;
            track.scrollLeft = scrollStart + dx;
        });

        ['mouseup', 'mouseleave'].forEach(evt => {
            track.addEventListener(evt, function () {
                isDown = false;
                track.classList.remove('is-dragging');
            });
        });

        // Дополнительно: позволим нативному свайпу работать (touch-action)
        track.style.touchAction = 'pan-x';
        // Убедимся, что overflow-x = auto (чтобы браузер позволял scrolling)
        track.style.overflowX = 'auto';
        // iOS smooth
        track.style.webkitOverflowScrolling = 'touch';
    })();
});
