// Ждём полной загрузки документа
document.addEventListener('DOMContentLoaded', function () {
    const header = document.querySelector('.site-header');

    window.addEventListener('scroll', function () {
        if (window.scrollY > 50) { // когда прокрутка > 50px
            header.style.backgroundColor = '#ff9900'; // оранжевый цвет
        } else {
            header.style.backgroundColor = 'transparent'; // прозрачный
        }
    });

    // Меню пользователя
    const userMenuTrigger = document.getElementById('user-menu-trigger');
    const userMenuDropdown = document.getElementById('user-menu-dropdown');
    const userMenuContainer = document.querySelector('.user-menu-container');

    if (userMenuTrigger && userMenuDropdown) {
        // Открытие/закрытие меню
        userMenuTrigger.addEventListener('click', function(e) {
            e.stopPropagation();
            userMenuContainer.classList.toggle('active');
        });

        // Закрытие меню при клике вне его
        document.addEventListener('click', function(e) {
            if (userMenuContainer && !userMenuContainer.contains(e.target)) {
                userMenuContainer.classList.remove('active');
            }
        });

        // Закрытие меню при нажатии Escape
        document.addEventListener('keydown', function(e) {
            if (e.key === 'Escape' && userMenuContainer) {
                userMenuContainer.classList.remove('active');
            }
        });
    }

    // Обработка выхода
    const btnLogout = document.getElementById('btn-logout');
    if (btnLogout) {
        btnLogout.addEventListener('click', async function(e) {
            e.preventDefault();
            const form = btnLogout.closest('form');
            if (form) {
                const formData = new FormData(form);
                try {
                    const response = await fetch(form.action, {
                        method: 'POST',
                        body: formData,
                        headers: {
                            'X-Requested-With': 'XMLHttpRequest'
                        }
                    });
                    const result = await response.json();
                    if (result.success) {
                        window.location.href = '/Home/Index';
                    } else {
                        // Если не AJAX, просто отправляем форму
                        form.submit();
                    }
                } catch (error) {
                    // Если ошибка, просто отправляем форму
                    form.submit();
                }
            }
        });
    }
});
