document.addEventListener('DOMContentLoaded', function () {

    /* ========= ГАМБУРГЕР ========= */
    const hamburger = document.querySelector('.hamburger');
    const nav = document.querySelector('.nav-list');

    if (hamburger && nav) {
        hamburger.addEventListener('click', function (e) {
            e.stopPropagation();
            nav.classList.toggle('open');
        });

        // закрытие при клике вне меню
        document.addEventListener('click', function (e) {
            if (!nav.contains(e.target) && !hamburger.contains(e.target)) {
                nav.classList.remove('open');
            }
        });
    }

    /* ========= СВАЙП ДЛЯ services-track ========= */
    const track = document.querySelector('.services-track');

    if (track) {
        let startX = 0;
        let scrollLeft = 0;
        let isDown = false;

        track.addEventListener('touchstart', (e) => {
            isDown = true;
            startX = e.touches[0].pageX;
            scrollLeft = track.scrollLeft;
        }, { passive: true });

        track.addEventListener('touchmove', (e) => {
            if (!isDown) return;
            const x = e.touches[0].pageX;
            const dx = startX - x;
            track.scrollLeft = scrollLeft + dx;
        }, { passive: true });

        track.addEventListener('touchend', () => {
            isDown = false;
        });

        // на мобильных должен быть scroll-x auto
        track.style.overflowX = "auto";
        track.style.webkitOverflowScrolling = "touch";
        track.style.touchAction = "pan-x";
    }
});
