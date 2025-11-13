document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("contact-form");
    const feedback = document.getElementById("contact-feedback");
    const clearBtn = document.getElementById("contact-clear");

    // поля
    const fldName = form.querySelector("input[name='Name']");
    const fldEmail = form.querySelector("input[name='Email']");
    const fldSubject = form.querySelector("input[name='Subject']");
    const fldMessage = form.querySelector("textarea[name='Message']");

    // helper: показать сообщение под полем
    function setFieldError(field, message) {
        const row = field.closest(".form-row");
        row.classList.add("error");
        field.classList.add("input-error");
        let err = row.querySelector(".field-error");
        if (!err) {
            err = document.createElement("div");
            err.className = "field-error";
            row.appendChild(err);
        }
        err.textContent = message;
    }

    function clearFieldError(field) {
        const row = field.closest(".form-row");
        row.classList.remove("error");
        field.classList.remove("input-error");
        const err = row.querySelector(".field-error");
        if (err) err.textContent = "";
    }

    // простая проверка email
    function isValidEmail(email) {
        // разумная валидирующая регулярка (не идеал, но хороша для фронтенда)
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    }

    // валидация одного поля (на blur)
    function validateField(field) {
        const val = field.value.trim();
        if (field === fldName) {
            if (!val) { setFieldError(field, "Введите имя"); return false; }
            clearFieldError(field);
            return true;
        }
        if (field === fldEmail) {
            if (!val) { setFieldError(field, "Введите email"); return false; }
            if (!isValidEmail(val)) { setFieldError(field, "Некорректный email"); return false; }
            clearFieldError(field);
            return true;
        }
        if (field === fldSubject) {
            // тема необязательна — но можно требовать минимум символов, если преподаватель того хочет
            clearFieldError(field);
            return true;
        }
        if (field === fldMessage) {
            if (!val) { setFieldError(field, "Введите текст сообщения"); return false; }
            if (val.length < 5) { setFieldError(field, "Сообщение слишком короткое"); return false; }
            clearFieldError(field);
            return true;
        }
        return true;
    }

    // события фокуса/ввода/blur
    [fldName, fldEmail, fldSubject, fldMessage].forEach(field => {
        if (!field) return;
        field.addEventListener("focus", () => {
            // пометка фокуса: красный border делаем через :focus CSS, но можно убрать ошибку при фокусе
            // не трогаем ошибку автоматически — пользователь видит подсказку
        });
        field.addEventListener("input", () => {
            // при вводе убираем ошибку (динамично)
            clearFieldError(field);
            feedback.textContent = "";
        });
        field.addEventListener("blur", () => {
            validateField(field);
        });
    });

    clearBtn?.addEventListener("click", () => {
        form.reset();
        feedback.textContent = "";
        [fldName, fldEmail, fldSubject, fldMessage].forEach(f => { if (f) clearFieldError(f); });
    });

    form.addEventListener("submit", async function (e) {
        e.preventDefault();
        feedback.textContent = "";

        // валидируем все поля
        const v1 = validateField(fldName);
        const v2 = validateField(fldEmail);
        const v3 = validateField(fldSubject);
        const v4 = validateField(fldMessage);

        if (!v1 || !v2 || !v3 || !v4) {
            feedback.textContent = "Пожалуйста, исправьте ошибки в форме.";
            feedback.style.color = "#c02020";
            return;
        }

        // собираем данные
        const data = {
            Name: fldName.value.trim(),
            Email: fldEmail.value.trim(),
            Subject: fldSubject.value.trim(),
            Message: fldMessage.value.trim()
        };

        try {
            const resp = await fetch("/Home/SendMessage", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data),
            });

            if (!resp.ok) {
                const text = await resp.text();
                feedback.textContent = "Ошибка сервера: " + (text || resp.status);
                feedback.style.color = "#c02020";
                return;
            }
            const json = await resp.json();
            if (json?.success) {
                feedback.textContent = "Сообщение отправлено. Спасибо!";
                feedback.style.color = "#1f8a3e";
                form.reset();
            } else {
                feedback.textContent = json?.message || "Не удалось отправить сообщение.";
                feedback.style.color = "#c02020";
            }
        } catch (err) {
            feedback.textContent = "Сетевая ошибка: " + err.message;
            feedback.style.color = "#c02020";
        }
    });
});
