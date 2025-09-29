// Auto-focus en el primer input
document.getElementById('email').focus();

// Enter en password field envía el form
document.getElementById('password').addEventListener('keypress', function (e) {
    if (e.key === 'Enter') {
        document.querySelector('form').submit();
    }
});

particlesJS("particles-js", {
    "particles": {
        "number": {
            "value": 80
        },
        "size": {
            "value": 3
        },
        "move": {
            "speed": 2
        },
        "line_linked": {
            "enable": true
        }
    }
});