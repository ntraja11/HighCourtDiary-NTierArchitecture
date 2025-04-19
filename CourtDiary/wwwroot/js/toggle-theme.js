(function () {
    let savedTheme = localStorage.getItem("theme") || "light"; // Default to light theme
    document.documentElement.setAttribute("data-bs-theme", savedTheme);

    // Update button text/icon based on theme
    updateThemeButton(savedTheme);
})();

document.getElementById("theme-switch").addEventListener('click', function () {
    let currentTheme = document.documentElement.getAttribute("data-bs-theme");
    let newTheme = currentTheme === "dark" ? "light" : "dark";

    document.documentElement.setAttribute("data-bs-theme", newTheme);
    localStorage.setItem("theme", newTheme);

    // Sync button state with applied theme
    updateThemeButton(newTheme);
});

function updateThemeButton(theme) {
    let themeButton = document.getElementById("theme-switch");
    themeButton.innerHTML = theme === "light"
        ? 'Light Mode <i class="bi bi-sun"></i>'
        : 'Dark Mode <i class="bi bi-moon-stars-fill"></i>';
}