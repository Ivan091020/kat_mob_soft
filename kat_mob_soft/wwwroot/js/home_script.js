document.addEventListener('DOMContentLoaded', () => {
    const track = document.getElementById('services-track');
    const left = document.getElementById('services-left');
    const right = document.getElementById('services-right');

    if (!track || !left || !right) return;

    // Вычисляем шаг прокрутки = ширина карточки + gap
    function getStep() {
        const card = track.querySelector('.service-card');
        if (!card) return 300;
        const style = getComputedStyle(card);
        const gap = parseFloat(style.marginRight) || 16;
        return card.offsetWidth + gap;
    }

    left.addEventListener('click', () => {
        track.scrollBy({ left: -getStep(), behavior: 'smooth' });
    });

    right.addEventListener('click', () => {
        track.scrollBy({ left: getStep(), behavior: 'smooth' });
    });
});
