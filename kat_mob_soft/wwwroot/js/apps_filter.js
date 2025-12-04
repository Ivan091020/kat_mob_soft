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

// Функция применения фильтров (отправка fetch запроса)
function applyFilters() {
    // Сбор данных из ползунков
    const priceMin = parseFloat(document.getElementById('price-min').value) || 0;
    const priceMax = parseFloat(document.getElementById('price-max').value) || 0;
    
    // Сбор данных из чекбоксов
    const selectedCategories = [];
    const categoryCheckboxes = document.querySelectorAll('.app-categories .custom-checkbox:checked');
    categoryCheckboxes.forEach(function(checkbox) {
        selectedCategories.push(checkbox.value);
    });
    
    // Формирование данных для отправки
    const filterData = {
        priceMin: priceMin,
        priceMax: priceMax,
        categories: selectedCategories
    };
    
    console.log('Отправляемые данные:', filterData);
    
    // Отправка данных через fetch запрос
    fetch('/Apps/Filter', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(filterData)
    })
    .then((response) => {
        if (!response.ok) {
            throw new Error('Ошибка при фильтрации данных');
        }
        return response.json(); // Преобразуем ответ в JSON
    })
    .then((data) => {
        console.log('Результаты фильтрации:', data);
        dataDisplay(data); // Отображаем отфильтрованные данные
    })
    .catch((error) => {
        console.error('Ошибка:', error);
    });
}

// Функция для отображения приложений при успешной фильтрации
function dataDisplay(data) {
    // Найти контейнер для списка приложений
    const appsList = document.querySelector('.container-apps-list .grid-container');
    
    if (!appsList) {
        console.error('Контейнер для приложений не найден');
        return;
    }
    
    appsList.innerHTML = ''; // Очистить старые данные
    
    if (!data || data.length === 0) {
        // Если нет данных, отображаем сообщение
        const noAppsMessage = '<p class="no-apps-message">По данному фильтру нет приложений</p>';
        appsList.innerHTML = noAppsMessage;
        return;
    }
    
    // Если данные есть, создаем элементы для приложений
    data.forEach((app) => {
        // Формируем HTML для звезд рейтинга
        let starsHtml = '';
        const rating = Math.round(app.averageRating || 0);
        for (let i = 1; i <= 5; i++) {
            if (i <= rating) {
                starsHtml += '<span class="star filled">★</span>';
            } else {
                starsHtml += '<span class="star">★</span>';
            }
        }
        
        // Формируем HTML для цены
        let priceHtml = '';
        if (app.price > 0) {
            priceHtml = `<span class="card-price">${app.price.toFixed(2)} ${app.currency || 'USD'}</span>`;
        }
        
        const appItem = `
            <div class="app-item card-app" 
                 data-price="${app.price}" 
                 data-rating="${app.averageRating}" 
                 data-name="${app.name}"
                 data-category="${app.categoryName}">
                <div class="card-image-wrapper">
                    <img src="${app.pathImg || '/images/default-app.png'}" alt="${app.name}" class="card-image" />
                </div>
                <div class="card-content">
                    <h3 class="card-title">${app.name}</h3>
                    <div class="card-category-price">
                        <span class="card-category">${app.categoryName || 'Без категории'}</span>
                        ${priceHtml}
                    </div>
                    <div class="card-rating">
                        ${starsHtml}
                        <span class="rating-value">${(app.averageRating || 0).toFixed(1)}</span>
                    </div>
                </div>
            </div>
        `;
        
        appsList.innerHTML += appItem; // Добавить приложение в список
    });
}
