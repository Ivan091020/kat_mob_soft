(() => {
    const DBG = true;
    const log = (...args) => { if (DBG) console.log('[LR]', ...args); };

    // false = реальная отправка (то, что нужно!)
    const MOCK_REGISTRATION = false;

    // === TOAST УВЕДОМЛЕНИЯ ===
    function showToast(message, type = 'success') {
        log('showToast вызвана:', message, type);
        const container = document.getElementById('toast-container');
        if (!container) {
            log('ОШИБКА: toast-container не найден!');
            console.error('toast-container не найден в DOM');
            return;
        }
        log('Контейнер найден, создаем toast...');

        const toast = document.createElement('div');
        toast.className = `toast ${type}`;
        
        const icons = {
            success: '✓',
            error: '✕',
            info: 'ℹ'
        };
        
        toast.innerHTML = `
            <span class="toast-icon">${icons[type] || icons.success}</span>
            <span class="toast-message">${message}</span>
            <button class="toast-close" aria-label="Закрыть">×</button>
        `;
        
        container.appendChild(toast);
        log('Toast добавлен в DOM');
        
        const closeBtn = toast.querySelector('.toast-close');
        const closeToast = () => {
            toast.classList.add('hiding');
            setTimeout(() => {
                if (toast.parentNode) {
                    toast.parentNode.removeChild(toast);
                }
            }, 300);
        };
        
        closeBtn.addEventListener('click', closeToast);
        
        // Автоматическое закрытие через 3 секунды
        setTimeout(closeToast, 3000);
    }

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

            const headers = { 
                'Content-Type': 'application/x-www-form-urlencoded',
                'X-Requested-With': 'XMLHttpRequest'
            };
            if (token) {
                headers['RequestVerificationToken'] = token;
            }

            try {
                const resp = await fetch(url, { method: 'POST', headers, body, credentials: 'same-origin' });
                log('Ответ получен, статус:', resp.status, 'ok:', resp.ok);
                let data;
                const contentType = resp.headers.get('content-type');
                log('Content-Type:', contentType);
                if (contentType && contentType.includes('application/json')) {
                    data = await resp.json();
                    log('JSON данные:', data);
                } else {
                    // Если ответ не JSON, пытаемся прочитать как текст
                    const text = await resp.text();
                    log('Текстовый ответ:', text);
                    try {
                        data = JSON.parse(text);
                    } catch {
                        data = { errors: [text || 'Неизвестная ошибка'] };
                    }
                }
                // Проверяем и success в данных, и статус ответа
                const isSuccess = resp.ok && resp.status >= 200 && resp.status < 300 && (data.success !== false);
                log('Итоговый success:', isSuccess);
                return { success: isSuccess, data };
            } catch (err) {
                console.error('Fetch error:', err);
                log('Ошибка fetch:', err);
                return { success: false, data: { errors: ['Сетевая ошибка'] } };
            }
        }

        // === Вход ===
        btnSignInSubmit?.addEventListener('click', async e => {
            e.preventDefault();
            errorSignin && (errorSignin.innerHTML = '');
            log('Отправка запроса на вход...');
            const result = await postForm(formSignin, '/Account/Login', errorSignin);
            log('Результат входа:', result);
            if (result.success && result.data && result.data.success !== false) {
                const message = result.data?.message || 'Вход выполнен';
                log('Показываем toast:', message);
                showToast(message, 'success');
                closeModal();
                // Увеличиваем задержку, чтобы toast успел показаться
                setTimeout(() => {
                    log('Перезагружаем страницу...');
                    location.reload();
                }, 1500);
            } else {
                const errors = result.data?.errors || result.data?.error || ['Ошибка входа'];
                log('Ошибка входа:', errors);
                errorSignin && (errorSignin.innerHTML = errors.map(e => `<div class="error">${e}</div>`).join(''));
            }
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

        // === Выход ===
        const btnLogout = document.getElementById('btn-logout');
        const logoutForm = document.getElementById('logout-form');
        
        btnLogout?.addEventListener('click', async e => {
            e.preventDefault();
            if (!logoutForm) return;
            
            const token = getToken(logoutForm);
            const body = new URLSearchParams(new FormData(logoutForm));
            
            const headers = {
                'Content-Type': 'application/x-www-form-urlencoded',
                'X-Requested-With': 'XMLHttpRequest'
            };
            if (token) {
                headers['RequestVerificationToken'] = token;
            }
            
            try {
                const resp = await fetch('/Account/Logout', {
                    method: 'POST',
                    headers,
                    body,
                    credentials: 'same-origin'
                });
                
                let data;
                const contentType = resp.headers.get('content-type');
                if (contentType && contentType.includes('application/json')) {
                    data = await resp.json();
                } else {
                    const text = await resp.text();
                    try {
                        data = JSON.parse(text);
                    } catch {
                        data = { success: resp.ok, message: 'Вы вышли' };
                    }
                }
                
                if (data.success) {
                    showToast(data.message || 'Вы вышли', 'info');
                    setTimeout(() => location.reload(), 500);
                }
            } catch (err) {
                console.error('Logout error:', err);
                showToast('Ошибка при выходе', 'error');
            }
        });
    }

    document.readyState === 'loading'
        ? document.addEventListener('DOMContentLoaded', init)
        : init();
})();