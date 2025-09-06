$(document).ready(function () {
    // Enable client-side validation
    $.validator.unobtrusive.parse($("form"));

    // Custom password strength validation
    $("#Password").on("input", function () {
        var password = $(this).val();
        var strength = 0;
        var requirements = [];

        if (password.length >= 6) {
            strength++;
        } else {
            requirements.push("Ít nhất 6 ký tự");
        }

        if (password.match(/[a-z]/)) { 
            strength++;
        } else {
            requirements.push("1 chữ thường"); 
        }

        if (password.match(/[A-Z]/)) {
            strength++;
        } else {
            requirements.push("1 chữ hoa");
        }

        if (password.match(/[0-9]/)) {
            strength++;
        } else {
            requirements.push("1 số");
        }

        if (password.match(/[^a-zA-Z0-9]/)) { // ký tự đặc biệt
            strength++;
        } else {
            requirements.push("1 ký tự đặc biệt");
        }

        var strengthText = "";
        var strengthClass = "";

        switch (strength) {
            case 0:
            case 1:
                strengthText = "Rất yếu";
                strengthClass = "text-danger";
                break;
            case 2:
                strengthText = "Yếu";
                strengthClass = "text-warning";
                break;
            case 3:
                strengthText = "Trung bình";
                strengthClass = "text-info";
                break;
            case 4:
                strengthText = "Mạnh";
                strengthClass = "text-primary";
                break;
            case 5:
                strengthText = "Rất mạnh";
                strengthClass = "text-success";
                break;
        }

        $(".password-strength").remove();
        $(".password-requirements").remove();

        if (password.length > 0) {
            $(this).after('<small class="password-strength ' + strengthClass + '">Độ mạnh: ' + strengthText + '</small>');

            if (requirements.length > 0) {
                $(this).after('<small class="password-requirements text-muted d-block mt-1">Yêu cầu: ' + requirements.join(', ') + '</small>');
            }
        }
    }).on("blur", function () {
        console.log("Password value:", $(this).val());
        console.log("Valid:", $(this).valid());
    });


    // Phone number formatting
    $("#Phone").on("input", function () {
        var phone = $(this).val().replace(/\D/g, '');
        
        $(this).val(phone);
    });

    // Confirm password validation
    $("#confirmPassword").on("input", function () {
        var password = $("#Password").val();
        var confirmPassword = $(this).val();
        $(".confirm-password-error").remove();

        if (confirmPassword.length > 0 && password !== confirmPassword) {
            $(this).after('<small class="confirm-password-error text-danger">Mật khẩu xác nhận không khớp</small>');
        }
    });

    // Form submission handling
    $("form").on("submit", function (e) {
        console.log("Form submit triggered");
        console.log("Form valid:", $(this).valid());
        
        if (!$(this).valid()) {
            e.preventDefault();
            console.log("Form validation failed");
            
            // Show all validation errors
            var errors = [];
            $(this).find('.field-validation-error').each(function() {
                errors.push($(this).text());
            });
            console.log("Validation errors:", errors);
            
            // Scroll to first error
            if ($(".field-validation-error:first").length > 0) {
                $("html, body").animate({
                    scrollTop: $(".field-validation-error:first").offset().top - 100
                }, 500);
            }
        } else {
            console.log("Form is valid, proceeding with submission");
        }
    });
    
    // Debug: Log validation state on input
    $("form input, form select").on("blur", function() {
        var fieldName = $(this).attr('name');
        var fieldValue = $(this).val();
        var isValid = $(this).valid();
        console.log("Field:", fieldName, "Value:", fieldValue, "Valid:", isValid);
    });
});