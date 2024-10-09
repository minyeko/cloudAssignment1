// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showError(errorMsg) {
	$("#errorMessage").text(errorMsg);
	var errorToastEl = document.getElementById('errorToast');
	var errorToast = new bootstrap.Toast(errorToastEl, {
		delay: 3000 // 3 seconds
	});
	errorToast.show();
}


function showSuccess(successMsg) {
	$("#successMsg").text(successMsg);
	var errorToastEl = document.getElementById('errorToast');
	var errorToast = new bootstrap.Toast(errorToastEl, {
		delay: 3000 // 3 seconds
	});
	errorToast.show();
}