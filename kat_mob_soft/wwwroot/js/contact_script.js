document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("contact-form");
    const feedback = document.getElementById("contact-feedback");
    const clearBtn = document.getElementById("contact-clear");

    function showMessage(text, ok = true) {
        feedback.textContent = text;
        feedback.style.color = ok ? "#1f8a3e" : "#d04545";
    }

    clearBtn?.addEventListener("click", () => {
        form.reset();
        feedback.textContent = "";
    });

    form.addEventListener("submit", async function (e) {
        e.preventDefault();
        feedback.textContent = "";

        const name = form.Name.value.trim();
        const email = form.Email.value.trim();
        const message = form.Message.value.trim();

        // простая валидация
        if (!name || !email || !message) {
            showMessage("Пожалуйста, заполните все обязательные поля.", false);
            return;
        }

        const data = {
            Name: name,
            Email: email,
            Subject: form.Subject.value.trim(),
            Message: message
        };

        try {
            const resp = await fetch("/Home/SendMessage", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(data),
            });

            if (!resp.ok) {
                const text = await resp.text();
                showMessage("Ошибка сервера: " + (text || resp.status), false);
                return;
            }

            const json = await resp.json();
            if (json?.success) {
                showMessage("Сообщение отправлено. Спасибо!");
                form.reset();
            } else {
                showMessage(json?.message || "Не удалось отправить сообщение.", false);
            }
        } catch (err) {
            showMessage("Сетевая ошибка: " + err.message, false);
        }
    });
});
