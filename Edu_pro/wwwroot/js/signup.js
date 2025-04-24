
document.addEventListener("DOMContentLoaded", () => {
    // Get all elements
    const signUpButton = document.getElementById("signUp")
    const signInButton = document.getElementById("signIn")
    const signUpLink = document.getElementById("signUpLink")
    const signInLink = document.getElementById("signInLink")
    const container = document.getElementById("container")

    // Toggle between sign up and sign in panels
    if (signUpButton) {
        signUpButton.addEventListener("click", () => {
            container.classList.add("right-panel-active")
        })
    }

    if (signInButton) {
        signInButton.addEventListener("click", () => {
            container.classList.remove("right-panel-active")
        })
    }

    if (signUpLink) {
        signUpLink.addEventListener("click", (e) => {
            e.preventDefault()
            container.classList.add("right-panel-active")
        })
    }

    if (signInLink) {
        signInLink.addEventListener("click", (e) => {
            e.preventDefault()
            container.classList.remove("right-panel-active")
        })
    }

    // Fix password toggle functionality
    const toggleButtons = document.querySelectorAll(".toggle-password")

    toggleButtons.forEach((toggle) => {
        toggle.addEventListener("click", function () {
            // Find the associated password input
            const passwordInput = this.previousElementSibling

            // Toggle between password and text
            const type = passwordInput.getAttribute("type") === "password" ? "text" : "password"
            passwordInput.setAttribute("type", type)

            // Change the eye icon
            this.innerHTML = type === "password" ? '<i class="fas fa-eye"></i>' : '<i class="fas fa-eye-slash"></i>'
        })
    })
})
