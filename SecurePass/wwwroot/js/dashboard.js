document.addEventListener("DOMContentLoaded", function () {
    const dashboardDeleteButtons = document.querySelectorAll(".service-grid .btn-danger");
    const deleteForm = document.querySelector("form[asp-action='Delete']");

    // Confirmation pour les boutons dans le tableau de bord
    dashboardDeleteButtons.forEach(button => {
        button.addEventListener("click", function (event) {
            if (!confirm("Êtes-vous sûr de vouloir supprimer ce compte ?")) {
                event.preventDefault();
            }
        });
    });

    // Confirmation pour le formulaire de suppression (si nécessaire)
    if (deleteForm) {
        deleteForm.addEventListener("submit", function (event) {
            if (!confirm("Êtes-vous sûr de vouloir supprimer ce compte ? Cette action est irréversible.")) {
                event.preventDefault();
            }
        });
    }
});
