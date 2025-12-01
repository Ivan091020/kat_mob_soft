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

    /* ========= ОБРАБОТКА ФОРМЫ КОНТАКТОВ ========= */
    const contactForm = document.getElementById('contact-form');
    const contactSendBtn = document.getElementById('contact-send');
    const contactClearBtn = document.getElementById('contact-clear');
    const contactFeedback = document.getElementById('contact-feedback');
    const contactValidationSummary = document.getElementById('contact-validation-summary');

    if (contactForm && contactSendBtn) {
        // Очистка формы
        if (contactClearBtn) {
            contactClearBtn.addEventListener('click', function () {
                contactForm.reset();
                if (contactFeedback) contactFeedback.innerHTML = '';
                if (contactValidationSummary) contactValidationSummary.innerHTML = '';
                // Очистка ошибок валидации
                const errorSpans = contactForm.querySelectorAll('.text-danger');
                errorSpans.forEach(span => span.innerHTML = '');
            });
        }

        // Отправка формы
        contactSendBtn.addEventListener('click', async function (e) {
            e.preventDefault();
            
            if (contactFeedback) contactFeedback.innerHTML = '';
            if (contactValidationSummary) contactValidationSummary.innerHTML = '';

            const formData = new FormData(contactForm);
            const data = Object.fromEntries(formData);

            try {
                const response = await fetch('/Home/SendMessage', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'X-Requested-With': 'XMLHttpRequest',
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                    },
                    body: JSON.stringify(data)
                });

                let result;
                const contentType = response.headers.get('content-type');
                if (contentType && contentType.includes('application/json')) {
                    result = await response.json();
                } else {
                    const text = await response.text();
                    try {
                        result = JSON.parse(text);
                    } catch {
                        result = { success: false, errors: [text || 'Неизвестная ошибка'] };
                    }
                }

                if (result.success) {
                    if (contactFeedback) {
                        contactFeedback.innerHTML = '<div class="success">Сообщение успешно отправлено!</div>';
                        contactFeedback.className = 'contact-feedback success';
                    }
                    contactForm.reset();
                } else {
                    const errors = result.errors || ['Ошибка при отправке сообщения'];
                    if (contactValidationSummary) {
                        contactValidationSummary.innerHTML = errors.map(err => `<div class="error">${err}</div>`).join('');
                    }
                    if (contactFeedback) {
                        contactFeedback.innerHTML = '<div class="error">Ошибка: ' + errors[0] + '</div>';
                        contactFeedback.className = 'contact-feedback error';
                    }
                }
            } catch (error) {
                console.error('Ошибка отправки формы:', error);
                if (contactFeedback) {
                    contactFeedback.innerHTML = '<div class="error">Сетевая ошибка. Попробуйте позже.</div>';
                    contactFeedback.className = 'contact-feedback error';
                }
            }
        });
    }
});
