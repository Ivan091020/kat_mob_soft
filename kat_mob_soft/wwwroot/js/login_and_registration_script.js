(() => {
    const DBG = true;
    const log = (...args) => { if (DBG) console.log('[LR]', ...args); };

    // false = реальная отправка (то, что нужно!)
    const MOCK_REGISTRATION = false;

    function init() {
        log('login_and_registration_script.js — финальная рабочая версия');

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

        // === Модальное окно ===
        function openModal() {
            modal?.classList.remove('hidden');
            overlay?.classList.remove('hidden');
        }
        function closeModal() {
            modal?.classList.add('hidden');
            overlay?.classList.add('hidden');
            errorSignin && (errorSignin.innerHTML = '');
            errorSignup && (errorSignup.innerHTML = '');
        }

        showSignInBtn?.addEventListener('click', e => { e.preventDefault(); formSignin?.classList.remove('hidden'); formSignup?.classList.add('hidden'); openModal(); });
        showSignUpBtn?.addEventListener('click', e => { e.preventDefault(); formSignup?.classList.remove('hidden'); formSignin?.classList.add('hidden'); openModal(); });
        overlay?.addEventListener('click', closeModal);
        btnCloseSign?.addEventListener('click', closeModal);
        btnCloseSignUp?.addEventListener('click', closeModal);

        // === Антифрод токен ===
        function getToken(form) {
            const input = form.querySelector('input[name="__RequestVerificationToken"]');
            return input ? input.value : null;
        }

        // === Универсальная отправка формы ===
        async function postForm(form, url, errorContainer) {
            const token = getToken(form);
            const body = new URLSearchParams(new FormData(form));

            const headers = { 'Content-Type': 'application/x-www-form-urlencoded' };
            if (token) headers['RequestVerificationToken'] = token;

            try {
                const resp = await fetch(url, { method: 'POST', headers, body, credentials: 'same-origin' });
                const data = await resp.json();
                return { success: resp.ok, data };
            } catch (err) {
                console.error('Fetch error:', err);
                return { success: false, data: { errors: ['Сетевая ошибка'] } };
            }
        }

        // === Вход ===
        btnSignInSubmit?.addEventListener('click', async e => {
            e.preventDefault();
            errorSignin && (errorSignin.innerHTML = '');
            const result = await postForm(formSignin, '/Account/Login', errorSignin);
            if (result.success) location.reload();
            else errorSignin && (errorSignin.innerHTML = result.data.errors?.map(e => `<div class="error">${e}</div>`).join('') || 'Ошибка входа');
        });

        // === РЕГИСТРАЦИЯ (ГЛАВНОЕ) ===
        btnSignUpSubmit?.addEventListener('click', async e => {
            e.preventDefault();
            errorSignup && (errorSignup.innerHTML = '');

            if (MOCK_REGISTRATION) {
                console.log('%c[MOCK] Регистрация успешна', 'color: cyan; font-size: 16px;');
                setTimeout(() => location.reload(), 500);
                return;
            }

            console.log('%c[REG] Отправка регистрации на /Account/Register', 'color: orange; font-size: 16px; font-weight: bold;');
            console.log('Токен антифрод:', getToken(formSignup) ? 'Есть' : 'НЕТ ТОКЕНА!');

            const result = await postForm(formSignup, '/Account/Register', errorSignup);

            if (result.success) {
                console.log('%cРЕГИСТРАЦИЯ УСПЕШНА! Перезагружаем...', 'color: lime; font-size: 18px;');
                location.reload();
            } else {
                const errors = result.data.errors || ['Неизвестная ошибка'];
                errorSignup && (errorSignup.innerHTML = errors.map(e => `<div class="error">${e}</div>`).join(''));
            }
        });
    }

    document.readyState === 'loading'
        ? document.addEventListener('DOMContentLoaded', init)
        : init();
})();