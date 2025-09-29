const mainCard = document.getElementById('mainCard');
const showRegister = document.getElementById('showRegister');
const showLogin = document.getElementById('showLogin');
const returnToLogin = document.getElementById('returnToLogin');
const registerForm = document.getElementById('registerForm');
const registerBtn = document.getElementById('registerBtn');

// Mostrar formulario de registro
showRegister.addEventListener('click', function (e) {
    e.preventDefault();
    mainCard.classList.add('flipped');
    mainCard.classList.remove('success');
});

// Mostrar formulario de login desde registro
showLogin.addEventListener('click', function (e) {
    e.preventDefault();
    mainCard.classList.remove('flipped', 'success');
});

// Manejar submit del formulario de registro
registerForm.addEventListener('submit', function (e) {
    e.preventDefault();

    // Validar formulario
    const inputs = registerForm.querySelectorAll('input[required]');
    let valid = true;

    inputs.forEach(input => {
        if (!input.value.trim()) {
            valid = false;
            input.focus();
            return;
        }
    });

    if (valid) {
        // Enviar el formulario usando fetch
        const formData = new FormData(registerForm);

        fetch('/Login/RegisterUser', {
            method: 'POST',
            body: formData
        })
            .then(response => response.text())
            .then(html => {
                // Si la respuesta contiene "exitosamente" o redirige, mostrar éxito
                if (html.includes('exitosamente') || html.includes('Cuenta creada')) {
                    // Mostrar mensaje de éxito
                    mainCard.classList.add('success');
                } else {
                    // Si hay error, recargar la página para mostrar el mensaje
                    window.location.reload();
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Error al registrar usuario. Por favor intenta nuevamente.');
            });
    }
});

// Regresar al login desde mensaje de éxito
returnToLogin.addEventListener('click', function (e) {
    e.preventDefault();
    // Redirigir a la página de login
    window.location.href = '/Login/Index';
});