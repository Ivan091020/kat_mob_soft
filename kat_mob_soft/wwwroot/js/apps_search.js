// Функция для вставки HTML-тега <mark> вокруг найденного текста
function insertMark(str, pos, len) {
    return str.slice(0, pos) + '<mark>' + str.slice(pos, pos + len) + '</mark>' + str.slice(pos + len);
}

// Функция поиска приложений
function triggerSearch() {
    const mySearch = document.querySelector("#mySearch");
    if (!mySearch) return;
    
    let val = mySearch.value.trim().toLowerCase();
    // Работаем с родительскими ссылками, чтобы скрывать весь элемент целиком
    const appsContainer = document.querySelector('.container-apps-list .grid-container');
    if (!appsContainer) return;
    
    let appLinks = appsContainer.querySelectorAll('.app-item-link');
    
    appLinks.forEach(function (appLink) {
        const app_item = appLink.querySelector('.app-item');
        if (!app_item) return;
        
        let appName = app_item.querySelector('.card-title') ? app_item.querySelector('.card-title').innerText.toLowerCase() : '';
        let appDescription = app_item.querySelector('.card-description') ? app_item.querySelector('.card-description').innerText.toLowerCase() : '';
        let appCategory = app_item.querySelector('.card-category') ? app_item.querySelector('.card-category').innerText.toLowerCase() : '';
        
        // Проверяем, содержит ли название, описание или категория введенный текст
        if (val === '' || appName.search(val) !== -1 || appDescription.search(val) !== -1 || appCategory.search(val) !== -1) {
            // Показываем элемент - убираем класс hide с родительской ссылки
            appLink.classList.remove('hide');
            app_item.classList.remove('hide');
        } else {
            // Скрываем элемент - добавляем класс hide к родительской ссылке
            appLink.classList.add('hide');
            app_item.classList.add('hide');
        }
        
        // Выделение совпадений в названии приложения (только для видимых элементов)
        if (!appLink.classList.contains('hide')) {
            const titleElement = app_item.querySelector('.card-title');
            if (titleElement) {
                if (val !== '' && appName.search(val) !== -1) {
                    let str = titleElement.innerText;
                    titleElement.innerHTML = insertMark(str, appName.search(val), val.length);
                } else {
                    // Очищаем mark при пустом поиске или отсутствии совпадений
                    titleElement.innerHTML = titleElement.innerText;
                }
            }
            
            // Выделение совпадений в описании приложения
            const descElement = app_item.querySelector('.card-description');
            if (descElement) {
                if (val !== '' && appDescription.search(val) !== -1) {
                    let str = descElement.innerText;
                    descElement.innerHTML = insertMark(str, appDescription.search(val), val.length);
                } else {
                    // Очищаем mark при пустом поиске или отсутствии совпадений
                    descElement.innerHTML = descElement.innerText;
                }
            }
            
            // Выделение совпадений в категории приложения
            const catElement = app_item.querySelector('.card-category');
            if (catElement) {
                if (val !== '' && appCategory.search(val) !== -1) {
                    let str = catElement.innerText;
                    catElement.innerHTML = insertMark(str, appCategory.search(val), val.length);
                } else {
                    // Очищаем mark при пустом поиске или отсутствии совпадений
                    catElement.innerHTML = catElement.innerText;
                }
            }
        }
    });
}

// Обработка событий после загрузки DOM
document.addEventListener('DOMContentLoaded', function () {
    const mySearch = document.querySelector("#mySearch");
    const headerSearch = document.querySelector("#header-search");
    const headerSearchForm = document.querySelector(".search-form");
    
    // Если мы на странице каталога и есть форма поиска в шапке - перехватываем отправку
    if (headerSearchForm && mySearch) {
        headerSearchForm.addEventListener('submit', function(e) {
            e.preventDefault();
            if (headerSearch && headerSearch.value.trim()) {
                mySearch.value = headerSearch.value;
                triggerSearch();
                // Обновляем URL без перезагрузки страницы
                const newUrl = new URL(window.location);
                newUrl.searchParams.set('q', headerSearch.value);
                window.history.pushState({}, '', newUrl);
            }
        });
    }
    
    // Синхронизация поля поиска в шапке с полем на странице каталога
    if (mySearch && headerSearch) {
        // При изменении поля на странице каталога - обновляем поле в шапке
        mySearch.addEventListener('input', function() {
            headerSearch.value = mySearch.value;
        });
        
        // При изменении поля в шапке - обновляем поле на странице каталога
        headerSearch.addEventListener('input', function() {
            mySearch.value = headerSearch.value;
            triggerSearch();
        });
    }
    
    if (mySearch) {
        // Читаем параметр q из URL и заполняем поле поиска
        const urlParams = new URLSearchParams(window.location.search);
        const searchQuery = urlParams.get('q');
        if (searchQuery) {
            mySearch.value = searchQuery;
            if (headerSearch) {
                headerSearch.value = searchQuery;
            }
            // Выполняем поиск с задержкой, чтобы DOM был полностью загружен
            setTimeout(function() {
                triggerSearch();
            }, 100);
        }
        
        const clear = document.querySelector('.clear');
        
        if (clear) {
            clear.addEventListener("click", function () {
                mySearch.value = '';
                if (headerSearch) {
                    headerSearch.value = '';
                }
                triggerSearch();
            });
        }
        
        mySearch.oninput = triggerSearch;
    }
});
