(() => {
    const DBG = true;
    const log = (...args) => { if (DBG) console.log('[LR]', ...args); };

    // ---------- DEMO FLAG ----------
    // true  = не отправляем реальный запрос регистрации, только логируем в консоль (demo mode)
    // false = выполняем реальную отправку на сервер (postForm -> /Account/Register)
    const MOCK_REGISTRATION = true;
    // --------------------------------

    function init() {
        log('Инициализация login_and_registration_script.js');

        const modal = document.getElementById('login-registration-modal');
        const overlay = document.getElementById('login-registration-overlay');

        const showSignInBtn = document.getElementById('click-to-show-signin');
        const showSignUpBtn = document.getElementById('click-to-show-signup');

        const formSignin = document.getElementById('form_signin');
        const formSignup = document.getElementById('form_signup');
        const btnSignInSubmit = document.getElementById('btn-signin-submit');
        const btnSignUpSubmit = document.getElementById('btn-signup-submit');

        const btnCloseSign = document.getElementById('btn-close-sign');
        const btnCloseSignUp = document.getElementById('btn-close-signup');

        const errorSignin = document.getElementById('error-messages-singin');
        const errorSignup = document.getElementById('error-messages-signup');

        // ---- Модальное окно ----
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

        showSignInBtn?.addEventListener('click', (e) => {
            e.preventDefault();
            formSignin?.classList.remove('hidden');
            formSignup?.classList.add('hidden');
            showSignInBtn.classList.add('active');
            showSignUpBtn?.classList.remove('active');
            openModal();
        });

        showSignUpBtn?.addEventListener('click', (e) => {
            e.preventDefault();
            formSignup?.classList.remove('hidden');
            formSignin?.classList.add('hidden');
            showSignUpBtn.classList.add('active');
            showSignInBtn?.classList.remove('active');
            openModal();
        });

        overlay?.addEventListener('click', closeModal);
        btnCloseSign?.addEventListener('click', closeModal);
        btnCloseSignUp?.addEventListener('click', closeModal);

        // ---- Отображение ошибок ----
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

        // ---- Получение токена AntiForgery ----
        function getToken(form) {
            const tokenInput = form.querySelector('input[name="__RequestVerificationToken"]');
            return tokenInput ? tokenInput.value : null;
        }

        // ---- Отправка формы через fetch ----
        async function postForm(form, url, errorContainer) {
            const token = getToken(form);
            const formData = new FormData(form);
            const body = new URLSearchParams();
            for (const [key, value] of formData.entries()) body.append(key, value);

            const headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
            if (token) headers['RequestVerificationToken'] = token;

            try {
                const resp = await fetch(url, {
                    method: 'POST',
                    credentials: 'same-origin',
                    headers,
                    body
                });
                if (resp.ok) {
                    const data = await resp.json().catch(() => null);
                    return { success: true, data };
                } else {
                    const errData = await resp.json().catch(() => null);
                    return { success: false, errors: errData?.errors || [errData?.message || 'Ошибка'] };
                }
            } catch (err) {
                log('postForm error', err);
                return { success: false, errors: ['Сетевая ошибка'] };
            }
        }

        // ---- Submit Login ----
        btnSignInSubmit?.addEventListener('click', async (ev) => {
            ev.preventDefault();
            if (!formSignin) return;
            if (errorSignin) errorSignin.innerHTML = '';
            const result = await postForm(formSignin, formSignin.action || '/Account/Login', errorSignin);
            if (result.success) {
                location.reload();
            } else {
                displayErrors(errorSignin, result.errors);
            }
        });

        // ---- Client-side validation for Register (used in MOCK mode) ----
        function validateRegisterFields(values) {
            const errors = [];

            // Required checks
            if (!values.UserName && !values.Name) { // allow both field names: UserName or Name
                errors.push('Имя пользователя обязательно');
            }
            const email = values.Email || '';
            if (!email.trim()) {
                errors.push('Email обязателен');
            } else {
                // simple email regex
                const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!re.test(email)) errors.push('Неверный формат email');
            }

            const pwd = values.Password || '';
            const confirm = values.ConfirmPassword || values.Confirm || '';

            if (!pwd) errors.push('Пароль обязателен');
            else if (pwd.length < 6) errors.push('Пароль минимум 6 символов');

            if (!confirm) errors.push('Подтверждение пароля обязательно');
            else if (pwd && pwd !== confirm) errors.push('Пароли не совпадают');

            // Optionally: other checks, e.g. username length
            return errors;
        }

        // ---- Submit Register (с поддержкой demo-mode и клиентской валидацией) ----
        btnSignUpSubmit?.addEventListener('click', async (ev) => {
            ev.preventDefault();
            if (!formSignup) return;
            if (errorSignup) errorSignup.innerHTML = '';

            // собираем поля из формы
            const formData = new FormData(formSignup);
            const bodyObj = {};
            for (const [k, v] of formData.entries()) bodyObj[k] = v;

            log('Попытка регистрации, body:', bodyObj);

            // Если в MOCK режиме — сначала валидируем на клиенте и выводим ошибки, если есть
            if (MOCK_REGISTRATION) {
                const clientErrors = validateRegisterFields(bodyObj);
                if (clientErrors.length > 0) {
                    // показываем ошибки в контейнере
                    displayErrors(errorSignup, clientErrors);
                    // и логируем в консоль для преподавателя
                    console.warn('[MOCK REGISTER] Клиентская валидация не пройдена:', clientErrors);
                    return; // не отправляем запрос
                }

                // Если клиентская валидация успешна — логируем (симуляция отправки)
                console.log('%c[MOCK REGISTER] Отправка данных (симуляция):', 'color: teal; font-weight: bold;', bodyObj);

                // имитируем небольшой delay и успешный ответ (по желанию можно снять комментарий)
                // setTimeout(() => {
                //     // чистим форму и закрываем модал (если нужно)
                //     formSignup.reset();
                //     displayErrors(errorSignup, []); // очистить контейнер
                //     // можно закрыть модал: closeModal();
                // }, 200);

                return; // выходим, не делаем реальную отправку
            }

            // Если mock выключен — делаем реальную отправку через postForm
            const result = await postForm(formSignup, formSignup.action || '/Account/Register', errorSignup);
            if (result.success) {
                location.reload();
            } else {
                displayErrors(errorSignup, result.errors);
            }
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', init);
    } else {
        init();
    }
})();
