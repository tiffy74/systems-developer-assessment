function showReplyForm(button, commentId) {
    // Find the corresponding reply form for the button that was clicked
    var replyForm = document.getElementById('Reply' + commentId);

    // Toggle the 'd-none' class which controls the visibility of the form
    if (replyForm.classList.contains('d-none')) {
        replyForm.classList.remove('d-none');
        button.textContent = "Cancel"; // Change button text to 'Cancel'
    } else {
        replyForm.classList.add('d-none');
        button.textContent = "Reply"; // Change button text back to 'Reply'
    }
}