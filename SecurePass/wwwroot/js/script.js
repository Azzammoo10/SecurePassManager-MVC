// Gestion du mode sombre/clair
const themeToggle = document.getElementById('themeToggle');
const body = document.body;

// Vérifier le mode stocké dans localStorage
const savedTheme = localStorage.getItem('theme');
if (savedTheme) {
    body.classList.add(savedTheme);
    themeToggle.textContent = savedTheme === 'dark-mode' ? '☀️' : '🌙';
}

// Basculer entre les modes
themeToggle.addEventListener('click', () => {
    body.classList.toggle('dark-mode');
    const isDarkMode = body.classList.contains('dark-mode');
    themeToggle.textContent = isDarkMode ? '☀️' : '🌙';
    localStorage.setItem('theme', isDarkMode ? 'dark-mode' : 'light-mode');
});

// Fonction de recherche
const searchInput = document.getElementById('searchInput');
if (searchInput) {
    searchInput.addEventListener('input', () => {
        const searchTerm = searchInput.value.toLowerCase();
        const cards = document.querySelectorAll('.password-card');
        cards.forEach(card => {
            const accountName = card.querySelector('h3').textContent.toLowerCase();
            card.style.display = accountName.includes(searchTerm) ? 'block' : 'none';
        });
    });
}