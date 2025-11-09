// wwwroot/js/login_and_registration_script.js
(() => {
    // Удобная обёртка для логов — можно выключить, установив false
    const DBG = true;
    const log = (...args) => { if (DBG) console.log('[LR]', ...args); };

    // Подготовка: безопасно ищем элементы и привязываем слушатели после загрузки DOM
    function init() {
        log('Инициализация login_and_registration_script.js');

        const modal = document.getElementById('login-registration-modal');
        const overlay = document.getElementById('login-registration-overlay');

        // Кнопки, которые открывают модал
        const showSignInBtn = document.getElementById('click-to-show-signin');
        const showSignUpBtn = document.getElementById('click-to-show-signup');

        // Формы и кнопки отправки (могут быть null, если partial не вставлен)
        const formSignin = document.getElementById('form_signin');
        const formSignup = document.getElementById('form_signup');
        const btnSignInSubmit = document.getElementById('btn-signin-submit');
        const btnSignUpSubmit = document.getElementById('btn-signup-submit');

        const btnCloseSign = document.getElementById('btn-close-sign');
        const btnCloseSignUp = document.getElementById('btn-close-signup');

        const errorSignin = document.getElementById('error-messages-singin');
        const errorSignup = document.getElementById('error-messages-signup');

        // Функции управления модальным окном
        function openModal() {
            if (modal) { modal.classList.remove('hidden'); modal.setAttribute('aria-hidden', 'false'); }
            if (overlay) overlay.classList.remove('hidden');
        }
        function closeModal() {
            if (modal) { modal.classList.add('hidden'); modal.setAttribute('aria-hidden', 'true'); }
            if (overlay) overlay.classList.add('hidden');
            clearForms();
        }
        function clearForms() {
            formSignin?.reset?.();
            formSignup?.reset?.();
            if (errorSignin) errorSignin.innerHTML = '';
            if (errorSignup) errorSignup.innerHTML = '';
        }

        // Убедимся, что кнопки открытия есть — если нет, покажем предупреждение в консоль
        if (!showSignInBtn && !showSignUpBtn) {
            log('Кнопки открытия модала не найдены. Проверь id (click-to-show-signin / click-to-show-signup) и место подключения partial.');
        }

        // Привязываем обработчики (защищённо)
        showSignInBtn?.addEventListener('click', (e) => {
            e.preventDefault();
            log('Нажата кнопка: Войти (open modal signin)');
            if (formSignin) {
                formSignin.classList.remove('hidden');
                formSignup?.classList.add('hidden');
            }
            showSignInBtn.classList.add('active');
            showSignUpBtn?.classList.remove('active');
            openModal();
        });

        showSignUpBtn?.addEventListener('click', (e) => {
            e.preventDefault();
            log('Нажата кнопка: Зарегистрироваться (open modal signup)');
            if (formSignup) {
                formSignup.classList.remove('hidden');
                formSignin?.classList.add('hidden');
            }
            showSignUpBtn.classList.add('active');
            showSignInBtn?.classList.remove('active');
            openModal();
        });

        overlay?.addEventListener('click', closeModal);
        btnCloseSign?.addEventListener('click', closeModal);
        btnCloseSignUp?.addEventListener('click', closeModal);

        // Функция отображения ошибок
        function displayErrors(container, errors) {
            if (!container) return;
            container.innerHTML = '';
            if (!errors || errors.length === 0) return;
            errors.forEach(e => {
                const div = document.createElement('div');
                div.className = 'error';
                div.textContent = e;
                container.appendChild(div);
            });
        }

        // Функция отправки запроса к серверу
        async function sendRequest(url, body) {
            try {
                const resp = await fetch(url, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(body)
                });
                if (!resp.ok) {
                    // попробуем прочитать тело ошибки
                    let text = await resp.text().catch(() => null);
                    log('Fetch returned not ok', resp.status, text);
                    return { success: false, errors: [`HTTP ${resp.status}`] };
                }
                const data = await resp.json().catch(() => null);
                return data ?? { success: false, errors: ['Неправильный ответ сервера'] };
            } catch (err) {
                log('sendRequest error', err);
                return { success: false, errors: ['Сетевая ошибка'] };
            }
        }

        // Обработчики отправки
        btnSignInSubmit?.addEventListener('click', async (ev) => {
            ev.preventDefault();
            log('Попытка входа...');
            if (!formSignin) { log('form_signin отсутствует'); return; }
            if (errorSignin) errorSignin.innerHTML = '';
            const body = {
                Email: document.getElementById('signin-email')?.value?.trim(),
                Password: document.getElementById('signin-password')?.value
            };
            log('body', body);
            const result = await sendRequest('/Home/Login', body);
            log('result', result);
            if (result?.success) {
                location.reload();
            } else {
                displayErrors(errorSignin, result?.errors || [result?.message || 'Ошибка']);
            }
        });

        btnSignUpSubmit?.addEventListener('click', async (ev) => {
            ev.preventDefault();
            log('Попытка регистрации...');
            if (!formSignup) { log('form_signup отсутствует'); return; }
            if (errorSignup) errorSignup.innerHTML = '';
            const body = {
                Name: document.getElementById('signup-name')?.value?.trim(),
                Email: document.getElementById('signup-email')?.value?.trim(),
                Password: document.getElementById('signup-password')?.value,
                ConfirmPassword: document.getElementById('signup-confirm')?.value
            };
            log('body', body);
            const result = await sendRequest('/Home/Register', body);
            log('result', result);
            if (result?.success) {
                location.reload();
            } else {
                displayErrors(errorSignup, result?.errors || [result?.message || 'Ошибка']);
            }
        });
    } // init

    // Инициализируем после загрузки DOM
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
