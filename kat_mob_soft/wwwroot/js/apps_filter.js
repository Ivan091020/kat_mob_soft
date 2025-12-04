// Скрипт для обновления отображаемых значений фильтров

function updatePriceValues() {
    const priceMin = document.getElementById('price-min').value;
    const priceMax = document.getElementById('price-max').value;
    
    document.getElementById('price-min-label').innerText = priceMin;
    document.getElementById('price-max-label').innerText = priceMax;
    document.getElementById('price-values').innerText = `${priceMin} - ${priceMax}`;
}

// Добавляем обработчики событий для ползунков
document.addEventListener('DOMContentLoaded', function() {
    const priceMinInput = document.getElementById('price-min');
    const priceMaxInput = document.getElementById('price-max');
    
    if (priceMinInput && priceMaxInput) {
        priceMinInput.addEventListener('input', updatePriceValues);
        priceMaxInput.addEventListener('input', updatePriceValues);
        
        // Инициализация значений при загрузке страницы
        updatePriceValues();
    }
    
    // Обработчик кнопки применения фильтра
    const applyFilterButton = document.getElementById('apply-filter');
    if (applyFilterButton) {
        applyFilterButton.addEventListener('click', function() {
            applyFilters();
        });
    }
});

// Функция применения фильтров
function applyFilters() {
    const priceMin = parseFloat(document.getElementById('price-min').value);
    const priceMax = parseFloat(document.getElementById('price-max').value);
    
    // Получаем выбранные категории
    const selectedCategories = [];
    const categoryCheckboxes = document.querySelectorAll('.app-categories .custom-checkbox:checked');
    categoryCheckboxes.forEach(function(checkbox) {
        selectedCategories.push(checkbox.value);
    });
    
    // Получаем все карточки приложений
    const appItems = document.querySelectorAll('.app-item');
    
    appItems.forEach(function(item) {
        const itemPrice = parseFloat(item.getAttribute('data-price')) || 0;
        const itemCategory = item.getAttribute('data-category') || '';
        
        // Проверка цены
        const priceMatch = itemPrice >= priceMin && itemPrice <= priceMax;
        
        // Проверка категории
        const categoryMatch = selectedCategories.length === 0 || selectedCategories.includes(itemCategory);
        
        // Показываем или скрываем элемент
        if (priceMatch && categoryMatch) {
            item.style.display = '';
        } else {
            item.style.display = 'none';
        }
    });
    
    // Проверяем, есть ли видимые элементы
    let visibleCount = 0;
    appItems.forEach(function(item) {
        if (item.style.display !== 'none') {
            visibleCount++;
        }
    });
    
    const noAppsMessage = document.querySelector('.no-apps-message');
    
    if (visibleCount === 0) {
        if (!noAppsMessage) {
            const gridContainer = document.querySelector('.grid-container');
            const message = document.createElement('p');
            message.className = 'no-apps-message';
            message.textContent = 'Приложения не найдены';
            gridContainer.appendChild(message);
        }
    } else {
        if (noAppsMessage) {
            noAppsMessage.remove();
        }
    }
}
