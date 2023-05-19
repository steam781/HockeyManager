if (typeof jQuery === 'undefined') {
    console.error('jQuery is not loaded in popup.js.');
} else {
    console.log('jQuery is loaded successfully in popup.js.');
}
$(document).ready(function () {
    // Get the confirmation popup element
    const confirmationPopup = document.getElementById('confirmationPopup');

    // Get the confirm and cancel buttons
    const confirmBtn = document.getElementById('confirmBtn');
    const cancelBtn = document.getElementById('cancelBtn');

    // Function to show the confirmation popup
    function showConfirmationPopup() {
        confirmationPopup.style.display = 'flex';
    }

    // Function to hide the confirmation popup
    function hideConfirmationPopup() {
        confirmationPopup.style.display = 'none';
    }

    // Event listener for confirm button
    confirmBtn.addEventListener('click', function () {
        // Handle the confirmed action here
        hideConfirmationPopup();
    });

    // Event listener for cancel button
    cancelBtn.addEventListener('click', function () {
        hideConfirmationPopup();
    });
});