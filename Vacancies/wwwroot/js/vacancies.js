$(document).ready(function () {
    //GetVacancies();
});
// создание строки для таблицы
var row = function (vacancie,index) {
    return "<tr><td>" + index + "</td><td>" + vacancie.name + "</td><td>" + vacancie.salary + "</td><td>" + vacancie.organisationName + "</td><td>" + vacancie.contact + "</td>" +
        "<td>" + vacancie.phoneNumber + "</td><td>" + vacancie.employment + "</td> <td>" + vacancie.description + "</td></tr> ";
}
function BlockPage() {
    $("#content").css("display", "none");
    $("#loading").css("display", "block");
}

function UnBlockPage() {
    $("#content").css("display", "block");
    $("#loading").css("display", "none");
}

$(function () {
   
    $('#downloadBtn').click(function (e) {
        e.preventDefault();
       
    
        GetVacancies();
      
       
    });
});
function GetVacancies() {

    
    $("table tbody").find("tr").remove();

    BlockPage();
    $.ajax({
        url: '/api/vacancies',
        type: 'POST',
        contentType: "application/json",
        dataType: 'json',
        data: JSON.stringify({
            grant_type: 'searchText',
             searchText: document.getElementById("searchText").value
        }),

        success: function (data) {
            var rows = "";
            $.each(data, function (index, tr) {

                rows += row(tr,index+1);
            })
            $("table tbody").append(rows);
            UnBlockPage();
        }


    });
}