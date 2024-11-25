document.addEventListener("DOMContentLoaded", function () {
    console.log("JavaScript loaded"); // Debugging line to ensure the script is loaded

    // Delete confirmation logic for all delete buttons
    const deleteButtons = document.querySelectorAll('.delete-button');

    console.log(deleteButtons); // Check if the delete buttons are being selected

    deleteButtons.forEach(function (button) {
        button.addEventListener("click", function (event) {
            console.log("Delete button clicked"); // Debugging line
            // Prevent form submission temporarily
            event.preventDefault();

            // Show confirmation dialog
            const isConfirmed = confirm('Are you sure you want to delete this admin?');

            // If the user confirms, submit the form
            if (isConfirmed) {
                console.log("Form submitted"); // Debugging line
                event.target.closest("form").submit();  // Submit the form manually
            }
        });
    });
});
