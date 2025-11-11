(() => {
    const DBG = true;
    const log = (...args) => { if (DBG) console.log('[LR]', ...args); };

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
            errorSignin.innerHTML = '';
            const result = await postForm(formSignin, formSignin.action || '/Account/Login', errorSignin);
            if (result.success) {
                location.reload();
            } else {
                displayErrors(errorSignin, result.errors);
            }
        });

        // ---- Submit Register ----
        btnSignUpSubmit?.addEventListener('click', async (ev) => {
            ev.preventDefault();
            if (!formSignup) return;
            errorSignup.innerHTML = '';
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
