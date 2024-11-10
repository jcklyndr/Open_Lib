document.getElementById('adminForm').addEventListener('submit', function (event) {
    var password = document.getElementById('password').value;
    var pattern = new RegExp('^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\\W).{8,}$');

    if (!pattern.test(password)) {
        event.preventDefault();
        alert('Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one number, and one special character.');
    }
});

