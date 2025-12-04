// Скрипт сортировки приложений

document.addEventListener('DOMContentLoaded', function() {
    const sortSelect = document.getElementById('sort-options');
    const appsContainer = document.querySelector('.container-apps-list .grid-container');
    
    if (!sortSelect || !appsContainer) {
        return;
    }
    
    sortSelect.addEventListener('change', function() {
        const sortOption = sortSelect.value;
        const apps = Array.from(appsContainer.querySelectorAll('.app-item'));
        
        if (apps.length === 0) {
            return;
        }
        
        apps.sort(function(a, b) {
            switch (sortOption) {
                case 'price-asc':
                    // Сортировка по цене (по возрастанию)
                    const priceA = parseFloat(a.getAttribute('data-price')) || 0;
                    const priceB = parseFloat(b.getAttribute('data-price')) || 0;
                    return priceA - priceB;
                    
                case 'price-desc':
                    // Сортировка по цене (по убыванию)
                    const priceA_desc = parseFloat(a.getAttribute('data-price')) || 0;
                    const priceB_desc = parseFloat(b.getAttribute('data-price')) || 0;
                    return priceB_desc - priceA_desc;
                    
                case 'rating-desc':
                    // Сортировка по рейтингу (по убыванию)
                    const ratingA_desc = parseFloat(a.getAttribute('data-rating')) || 0;
                    const ratingB_desc = parseFloat(b.getAttribute('data-rating')) || 0;
                    return ratingB_desc - ratingA_desc;
                    
                case 'rating-asc':
                    // Сортировка по рейтингу (по возрастанию)
                    const ratingA_asc = parseFloat(a.getAttribute('data-rating')) || 0;
                    const ratingB_asc = parseFloat(b.getAttribute('data-rating')) || 0;
                    return ratingA_asc - ratingB_asc;
                    
                case 'name-asc':
                    // Сортировка по названию (А-Я)
                    const nameA = (a.getAttribute('data-name') || '').toLowerCase();
                    const nameB = (b.getAttribute('data-name') || '').toLowerCase();
                    return nameA.localeCompare(nameB, 'ru');
                    
                case 'name-desc':
                    // Сортировка по названию (Я-А)
                    const nameA_desc = (a.getAttribute('data-name') || '').toLowerCase();
                    const nameB_desc = (b.getAttribute('data-name') || '').toLowerCase();
                    return nameB_desc.localeCompare(nameA_desc, 'ru');
                    
                default:
                    // По умолчанию - перезагрузка страницы
                    location.reload();
                    return 0;
            }
        });
        
        // Перемещаем отсортированные элементы обратно в контейнер
        apps.forEach(function(app) {
            appsContainer.appendChild(app);
        });
    });
});
