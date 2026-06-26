document.addEventListener("DOMContentLoaded", function () {
    console.log("Open Library JavaScript Layer Loaded Successfully.");

    // ==========================================================================
    // 1. BOOTSTRAP 5 TOOLTIPS INITIALIZATION
    // ==========================================================================
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // ==========================================================================
    // 2. GLOBAL DELETE CONFIRMATION HANDLER
    // ==========================================================================
    const deleteButtons = document.querySelectorAll('.delete-button');
    deleteButtons.forEach(function (button) {
        button.addEventListener("click", function (event) {
            console.log("Delete action intercepted.");

            // Prevent accidental submission
            event.preventDefault();

            // Native browser dialog pop
            const isConfirmed = confirm('Are you sure you want to delete this item?');

            if (isConfirmed) {
                console.log("Delete confirmed. Processing form routing.");
                event.target.closest("form").submit();
            }
        });
    });

    // ==========================================================================
    // 3. USER SIGNUP / REGISTRATION RUNTIME VALIDATION
    // ==========================================================================
    const form = document.getElementById("signupForm");
    const password = document.getElementById("password");
    const passwordConfirm = document.getElementById("password-confirm");
    const passwordError = document.getElementById("password-error");

    // Null check verification block upang maiwasan ang operational errors sa ibang pages
    if (form && password && passwordConfirm && passwordError) {
        form.addEventListener("submit", function (event) {
            // Reset structural layout flags gamit ang Bootstrap helper utilities
            passwordError.classList.add("d-none");

            // Structural matching verification architecture
            if (password.value !== passwordConfirm.value) {
                event.preventDefault();
                passwordError.classList.remove("d-none");
                passwordConfirm.focus();
                return;
            }

            // Safe trim state validation rules
            if (password.value.trim() === "" || passwordConfirm.value.trim() === "") {
                event.preventDefault();
                alert("Both password fields must be filled out cleanly.");
            }
        });
    }

    // ==========================================================================
    // 4. CENTRALIZED PASSWORD VISIBILITY TOGGLE HANDLER
    // ==========================================================================
    const toggleButtons = document.querySelectorAll('.password-toggle-btn');
    
    toggleButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            // Hanapin ang katabing input element sa loob ng parehong input-group block
            const inputContainer = btn.closest('.input-group');
            const passwordInput = inputContainer.querySelector('.toggle-password-input, input[type="password"], input[type="text"]');
            const icon = btn.querySelector('i');

            if (passwordInput && icon) {
                if (passwordInput.type === 'password') {
                    // Ipakita ang password text runtime state
                    passwordInput.type = 'text';
                    icon.classList.remove('fa-eye-slash');
                    icon.classList.add('fa-eye');
                } else {
                    // Itago pabalik ang password layer security framework
                    passwordInput.type = 'password';
                    icon.classList.remove('fa-eye');
                    icon.classList.add('fa-eye-slash');
                }
            }
        });
    });

});