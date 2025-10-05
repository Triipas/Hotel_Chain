document.addEventListener('DOMContentLoaded', function() {
            const menuToggle = document.querySelector('.menu-toggle');
            const navDerecha = document.querySelector('.nav-derecha');
            const body = document.body;
            
            if(menuToggle) {
                menuToggle.addEventListener('click', function(e) {
                    e.stopPropagation();
                    menuToggle.classList.toggle('active');
                    navDerecha.classList.toggle('active');
                    body.classList.toggle('menu-open');
                });
                
                const navLinks = document.querySelectorAll('.nav-links a');
                navLinks.forEach(link => {
                    link.addEventListener('click', function() {
                        menuToggle.classList.remove('active');
                        navDerecha.classList.remove('active');
                        body.classList.remove('menu-open');
                    });
                });
                
                document.addEventListener('click', function(event) {
                    const isClickInside = navDerecha.contains(event.target) || menuToggle.contains(event.target);
                    if (!isClickInside && navDerecha.classList.contains('active')) {
                        menuToggle.classList.remove('active');
                        navDerecha.classList.remove('active');
                        body.classList.remove('menu-open');
                    }
                });
            }
        });