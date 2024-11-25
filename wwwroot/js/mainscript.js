document.addEventListener("DOMContentLoaded", function () {
    console.log("JavaScript loaded");

    // Select all delete buttons
    const deleteButtons = document.querySelectorAll('.delete-button');

    // Add event listeners to each delete button
    deleteButtons.forEach(function (button) {
        button.addEventListener("click", function (event) {
            console.log("Delete button clicked");

            // Prevent form submission
            event.preventDefault();

            // Show confirmation dialog
            const isConfirmed = confirm('Are you sure you want to delete this item?');

            // If the user confirms, submit the form
            if (isConfirmed) {
                console.log("Form submitted");
                event.target.closest("form").submit();
            }
        });
    });
});
