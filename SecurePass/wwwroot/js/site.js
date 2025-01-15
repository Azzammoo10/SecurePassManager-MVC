// Confirmation de suppression
document.addEventListener("DOMContentLoaded", function () {
    const deleteButtons = document.querySelectorAll(".btn-danger");

    deleteButtons.forEach((button) => {
        button.addEventListener("click", function (event) {
            const confirmation = confirm("Êtes-vous sûr de vouloir supprimer cet élément ?");
            if (!confirmation) {
                event.preventDefault(); // Empêche l'action si l'utilisateur annule
            }
        });
    });
});

// Basculer la visibilité des mots de passe
const togglePasswordVisibility = () => {
    const passwordFields = document.querySelectorAll('input[type="password"]');
    passwordFields.forEach((field) => {
        const toggleButton = document.createElement("button");
        toggleButton.type = "button";
        toggleButton.textContent = "Afficher";
        toggleButton.className = "btn btn-sm btn-secondary toggle-password";

        // Ajouter le bouton après le champ de mot de passe
   

        toggleButton.addEventListener("click", () => {
            if (field.type === "password") {
                field.type = "text";
                toggleButton.textContent = "Masquer";
            } else {
                field.type = "password";
                toggleButton.textContent = "Afficher";
            }
        });
    });
};

// Appeler la fonction lors du chargement
document.addEventListener("DOMContentLoaded", togglePasswordVisibility);

// Animation des alertes (disparaître automatiquement)
const dismissibleAlerts = () => {
    const alerts = document.querySelectorAll(".alert");
    alerts.forEach((alert) => {
        setTimeout(() => {
            alert.style.opacity = "0";
            setTimeout(() => {
                alert.remove();
            }, 500); // Temps pour la transition CSS
        }, 5000); // Durée avant disparition
    });
};

// Appeler la fonction lors du chargement
document.addEventListener("DOMContentLoaded", dismissibleAlerts);
