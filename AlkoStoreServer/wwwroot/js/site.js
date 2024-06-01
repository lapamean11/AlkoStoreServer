// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {

    var checkboxes = document.querySelectorAll('input[type="checkbox"]');

    document.querySelectorAll('input[type="checkbox"]').forEach(function (checkbox) {
        /*checkbox.addEventListener('click', function () {
            var hidden = document.querySelector('input[type="hidden"][id="' + checkbox.getAttribute('id') + '"]');

            if (checkbox.checked) {
                checkbox.setAttribute('value', '1');
                *//*var hidden = document.querySelector('input[type="hidden"][id="' + checkbox.getAttribute('id') + '"]');*//*
                hidden.setAttribute('value', '1');
            } else {
                checkbox.setAttribute('value', '0');
                hidden.setAttribute('value', '0');
            }
        });*/
    });
});
