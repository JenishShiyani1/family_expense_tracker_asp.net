// Include SweetAlert2 CDN dynamically if not already included
if (typeof Swal === "undefined") {
    var script = document.createElement("script");
    script.src = "https://cdn.jsdelivr.net/npm/sweetalert2@11";
    script.async = true;
    document.head.appendChild(script);
}

function showSuccessMessage(message) {
    Swal.fire({
            text: message,
            icon: "success"
    });
}

function showErrorMessage(message) {
    Swal.fire({
        text: message,
        icon: "error"
    });
}

function confirmDelete(deleteUrl) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = deleteUrl;
        }
    });
}
