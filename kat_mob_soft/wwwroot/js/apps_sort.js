// Скрипт сортировки приложений

// Функция сортировки, которую можно вызвать извне
function applySorting(sortOption) {
    const appsContainer = document.querySelector('.container-apps-list .grid-container');
    
    if (!appsContainer || !sortOption) {
        return;
    }
    
    // Выбираем родительские элементы-ссылки, а не сами карточки
    const allAppLinks = Array.from(appsContainer.querySelectorAll('.app-item-link'));
    
    if (allAppLinks.length === 0) {
        return;
    }
    
    // Разделяем видимые и скрытые элементы
    // Элемент считается видимым, если у родительской ссылки нет класса hide
    const visibleLinks = allAppLinks.filter(function(link) {
        return !link.classList.contains('hide');
    });
    
    const hiddenLinks = allAppLinks.filter(function(link) {
        return link.classList.contains('hide');
    });
    
    // Сортируем только видимые элементы
    visibleLinks.sort(function(linkA, linkB) {
        // Получаем дочерний элемент .app-item для получения data-атрибутов
        const appA = linkA.querySelector('.app-item');
        const appB = linkB.querySelector('.app-item');
        
        if (!appA || !appB) {
            return 0;
        }
        
        switch (sortOption) {
            case 'price-asc':
                // Сортировка по цене (по возрастанию)
                const priceA = parseFloat(appA.getAttribute('data-price')) || 0;
                const priceB = parseFloat(appB.getAttribute('data-price')) || 0;
                return priceA - priceB;
                
            case 'price-desc':
                // Сортировка по цене (по убыванию)
                const priceA_desc = parseFloat(appA.getAttribute('data-price')) || 0;
                const priceB_desc = parseFloat(appB.getAttribute('data-price')) || 0;
                return priceB_desc - priceA_desc;
                
            case 'rating-desc':
                // Сортировка по рейтингу (по убыванию)
                const ratingA_desc = parseFloat(appA.getAttribute('data-rating')) || 0;
                const ratingB_desc = parseFloat(appB.getAttribute('data-rating')) || 0;
                return ratingB_desc - ratingA_desc;
                
            case 'rating-asc':
                // Сортировка по рейтингу (по возрастанию)
                const ratingA_asc = parseFloat(appA.getAttribute('data-rating')) || 0;
                const ratingB_asc = parseFloat(appB.getAttribute('data-rating')) || 0;
                return ratingA_asc - ratingB_asc;
                
            case 'name-asc':
                // Сортировка по названию (А-Я)
                const nameA = (appA.getAttribute('data-name') || '').toLowerCase();
                const nameB = (appB.getAttribute('data-name') || '').toLowerCase();
                return nameA.localeCompare(nameB, 'ru');
                
            case 'name-desc':
                // Сортировка по названию (Я-А)
                const nameA_desc = (appA.getAttribute('data-name') || '').toLowerCase();
                const nameB_desc = (appB.getAttribute('data-name') || '').toLowerCase();
                return nameB_desc.localeCompare(nameA_desc, 'ru');
                
            default:
                return 0;
        }
    });
    
    // Создаем новый контейнер для отсортированных элементов
    // Используем DocumentFragment для эффективного добавления
    const fragment = document.createDocumentFragment();
    
    // Добавляем отсортированные видимые элементы
    visibleLinks.forEach(function(link) {
        fragment.appendChild(link);
    });
    
    // Добавляем скрытые элементы в конец
    hiddenLinks.forEach(function(link) {
        fragment.appendChild(link);
    });
    
    // Полностью очищаем контейнер перед добавлением отсортированных элементов
    // Сохраняем текущие элементы перед очисткой, чтобы они не потерялись
    appsContainer.innerHTML = '';
    
    // Добавляем все элементы из фрагмента в правильном порядке
    appsContainer.appendChild(fragment);
}

document.addEventListener('DOMContentLoaded', function() {
    const sortSelect = document.getElementById('sort-options');
    const appsContainer = document.querySelector('.container-apps-list .grid-container');
    
    if (!sortSelect || !appsContainer) {
        return;
    }
    
    sortSelect.addEventListener('change', function() {
        const sortOption = sortSelect.value;
        if (!sortOption) {
            // Если выбор сброшен, перезагружаем страницу
            location.reload();
            return;
        }
        
        applySorting(sortOption);
    });
});
