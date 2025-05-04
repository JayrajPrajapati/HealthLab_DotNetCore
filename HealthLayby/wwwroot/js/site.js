
const showSuccessMessage = (message) => {
    showNotif(message, 1);
};

const showErrorMessage = (message) => {
    showNotif(message, 2);
};

//1 = success
//2 = error
//3 = warning
//4 = info
const showNotif = (notifMessage, notifType) => {

    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    switch (notifType) {
        case 1:
            toastr.success(notifMessage);
            break;
        case 2:
            toastr.error(notifMessage);
            break;
        case 3:
            toastr.warning(notifMessage);
            break;
        case 4:
            toastr.info(notifMessage);
            break;
        default:
            return;
    }
};

const activeMenu = (menuId) => {
    if (!$(`#${menuId}`).hasClass("active"))
        $(`#${menuId}`).addClass("active");
};

const activeSubMenu = (menuId) => {
    if (!$(`#${menuId}`).hasClass("active"))
        $(`#${menuId}`).addClass("active");
    
    $(`#${menuId}`).parents().find(".submenu").css("display", "block");
    $(`#${menuId}`).parents().find(".sublist").addClass("open");  
};

const showLoader = () => {
    if (!$("body").hasClass("loadingpage"))
        $("body").addClass("loadingpage")
};

const hideLoader = () => {
    if ($("body").hasClass("loadingpage"))
        $("body").removeClass("loadingpage")
};

$.fn.resetValidation = function () {

    var $form = this;

    $form.validate().resetForm();

    $form.find("[data-valmsg-summary=true]")
        .removeClass("validation-summary-errors")
        .addClass("validation-summary-valid")
        .find("ul").empty();

    $form.find("[data-valmsg-replace]")
        .removeClass("field-validation-error")
        .addClass("field-validation-valid")
        .empty();

    return $form;
};

$.fn.formReset = function (resetValidation) {
    var $form = this;

    $form[0].reset();

    if (resetValidation == undefined || resetValidation) {
        $form.resetValidation();
    }

    return $form;
}

var months = {
    "Jan": 0, "Feb": 1, "March": 2, "Apr": 3, "May": 4, "Jun": 5,
    "Jul": 6, "Aug": 7, "Sep": 8, "Oct": 9, "Nov": 10, "Dec": 11
};

function getMontName(month) {
    switch (parseInt(month)) {
        case months.Jan:
            return "Jan";            
        case months.Feb:
            return "Feb";
        case months.March:
            return "March";
        case months.Apr:
            return "Apr";
        case months.May:
            return "May";
        case months.Jun:
            return "Jun";
        case months.Jul:
            return "Jul";
        case months.Aug:
            return "Aug";
        case months.Sep:
            return "Sep";
        case months.Oct:
            return "Oct";
        case months.Nov:
            return "Nov";
        case months.Dec:
            return "Dec";
    }
}