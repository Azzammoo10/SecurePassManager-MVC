// Validation du formulaire
document.addEventListener('DOMContentLoaded', function() {
    const form = document.querySelector('form');
    const inputs = form.querySelectorAll('input');

    // Ajouter une classe pour l'animation des champs de saisie
    inputs.forEach(input => {
        input.addEventListener('focus', function() {
            this.parentElement.classList.add('focused');
        });

        input.addEventListener('blur', function() {
            if (!this.value) {
                this.parentElement.classList.remove('focused');
            }
        });
    });

    // Validation basique avant soumission
    form.addEventListener('submit', function(e) {
        let isValid = true;
        inputs.forEach(input => {
            if (!input.value.trim()) {
                isValid = false;
                const errorSpan = input.nextElementSibling;
                errorSpan.textContent = 'Ce champ est requis';
            }
        });

        if (!isValid) {
            e.preventDefault();
        }
    });
});