function dashboard() {
    $.ajax({
        type: 'POST',
        url: window.location.origin+"/Admin/Dashboard",
        data: '',
        success: function (data) {
            // $("#viewlist")[0].innerHtml = data;
            //or
            $("#manageDiseaseView").html(data);
            $('#default').hide();
        }
    });
    return false; //prevent default action(submit) for a button
}
function manageDisease() {
    $.ajax({
        type: 'POST',
        url: window.location.origin+"/Disease/ListDisease",
        data: '',
        success: function (data) {
            // $("#viewlist")[0].innerHtml = data;
            //or
            $("#manageDiseaseView").html(data);
            $('#default').hide();
        }
    });
    return false; //prevent default action(submit) for a button

}
function addDisease() {
    $.ajax({
        type: 'POST',
        url: window.location.origin+"/Disease/AddDisease",
        data: '',
        success: function (data) {
            // $("#viewlist")[0].innerHtml = data;
            //or
            $("#manageDiseaseView").html(data);
            $('#default').hide();
        }
    });
    return false; //prevent default action(submit) for a button
}

function addUser() {
    $.ajax({
        type: 'POST',
        url: window.location.origin+"/Admin/addUser",
        data: '',
        success: function (data) {
            // $("#viewlist")[0].innerHtml = data;
            //or
            $("#manageDiseaseView").html(data);
            $('#default').hide();
        }
    });
    return false; //prevent default action(submit) for a button
}

function manageUser() {
    $.ajax({
        type: 'POST',
        url: window.location.origin+"/Admin/manageUser",
        data: '',
        success: function (data) {
            // $("#viewlist")[0].innerHtml = data;
            //or
            $("#manageDiseaseView").html(data);
            $('#default').hide();
        }
    });
    return false; //prevent default action(submit) for a button
}

function changePassword() {
    $.ajax({
        type: 'POST',
        url: window.location.origin+"/Admin/changePassword",
        data: '',
        success: function (data) {
            // $("#viewlist")[0].innerHtml = data;
            //or
            $("#manageDiseaseView").html(data);
            $('#default').hide();
        }
    });
    return false; //prevent default action(submit) for a button
}

function addDoctor() {
    $.ajax({
        type: 'POST',
        url: window.location.origin+"/Doctor/AddDoctor",
        data: '',
        success: function (data) {
            // $("#viewlist")[0].innerHtml = data;
            //or
            $("#manageDiseaseView").html(data);
            $('#default').hide();
        }
    });
    return false; //prevent default action(submit) for a button
}

function manageDoctor() {
    $.ajax({
        type: 'POST',
        url: window.location.origin+"/Doctor/ListDoctors",
        data: '',
        success: function (data) {
            // $("#viewlist")[0].innerHtml = data;
            //or
            $("#manageDiseaseView").html(data);
            $('#default').hide();
        }
    });
    return false; //prevent default action(submit) for a button
}
function addHospital() {
    $.ajax({
        type: 'POST',
        url: window.location.origin+"/Hospital/AddHospital",
        data: '',
        success: function (data) {
            // $("#viewlist")[0].innerHtml = data;
            //or
            $("#manageDiseaseView").html(data);
            $('#default').hide();
        }
    });
    return false; //prevent default action(submit) for a button
}
function manageHospital() {
    $.ajax({
        type: 'POST',
        url: window.location.origin+"/Hospital/ListHospital",
        data: '',
        success: function (data) {
            // $("#viewlist")[0].innerHtml = data;
            //or
            $("#manageDiseaseView").html(data);
            $('#default').hide();
        }
    });
    return false; //prevent default action(submit) for a button
}

