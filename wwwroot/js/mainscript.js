document.addEventListener("DOMContentLoaded", function () {
    const password = document.getElementById("password");
    const passwordConfirm = document.getElementById("password-confirm");
    const passwordError = document.getElementById("password-error");
    const form = document.getElementById("signUpForm");

    form.addEventListener("submit", function (event) {
        // Reset error messages
        passwordError.style.display = "none";

        // Check if passwords match
        if (password.value !== passwordConfirm.value) {
            event.preventDefault(); // Prevent form submission
            passwordError.style.display = "inline"; // error message
        }

        // if password and confirmPassword are empty, it will not submit
        if (password.value === "" || passwordConfirm.value === "") {
            event.preventDefault(); // Prevent form submission
            alert("Both password fields must be filled out.");
        }
    });
});
