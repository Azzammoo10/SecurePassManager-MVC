// Appliquer le thème sauvegardé au chargement de la page
document.addEventListener('DOMContentLoaded', () => {
    const savedTheme = localStorage.getItem('theme') || 'light'; // Par défaut, thème clair
    document.body.setAttribute('data-theme', savedTheme);
    console.log("Thème appliqué au chargement :", savedTheme); // Affiche le thème appliqué
});

// Gestion du bouton de bascule de thème
const themeToggle = document.querySelector('.theme-toggle');
const body = document.body;

if (themeToggle) {
    themeToggle.addEventListener('click', () => {
        const currentTheme = body.getAttribute('data-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        body.setAttribute('data-theme', newTheme);
        localStorage.setItem('theme', newTheme);
        console.log("Thème changé :", newTheme); // Affiche le nouveau thème
    });
} else {
    console.error("Bouton de thème non trouvé !"); // Affiche une erreur si le bouton est manquant
}