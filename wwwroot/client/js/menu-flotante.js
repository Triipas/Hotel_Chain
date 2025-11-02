// Menu Flotante - Control de apertura/cierre
document.addEventListener('DOMContentLoaded', function() {
    const menuFlotante = document.getElementById('menuFlotante');
    const btnMenuPrincipal = document.getElementById('btnMenuPrincipal');
    
    if (!menuFlotante || !btnMenuPrincipal) return;
    
    // Toggle del menú al hacer click en el botón principal
    btnMenuPrincipal.addEventListener('click', function(e) {
        e.stopPropagation();
        menuFlotante.classList.toggle('active');
    });
    
    // Cerrar el menú al hacer click fuera
    document.addEventListener('click', function(e) {
        if (!menuFlotante.contains(e.target)) {
            menuFlotante.classList.remove('active');
        }
    });
    
    // Prevenir que los clicks en las opciones cierren el menú antes de navegar
    const opciones = menuFlotante.querySelectorAll('.menu-option');
    opciones.forEach(opcion => {
        opcion.addEventListener('click', function(e) {
            e.stopPropagation();
        });
    });
});