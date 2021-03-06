﻿$(document).ready(function () {
    //GetVacancies();
    $('.searchbuttons').css("display", "block");
});
// создание строки для таблицы
var rowHH = function (vacancie, index) {
    var id = vacancie.idHH;
    return "<tr data-rowid='" + id + "'><td>" + index + "</td><td>" + vacancie.name + "</td><td>" + vacancie.salary + "</td><td>" + vacancie.organisationName + "</td><td>" + vacancie.contact + "</td>" +
        "<td>" + vacancie.phoneNumber + "</td><td>" + vacancie.employment + "</td> <td>" + vacancie.description + "</td></tr> ";
}
// создание строки для таблицы
var rowDB = function (vacancie, index) {
    return "<tr data-rowid='" + vacancie.idHH + "'><td>" + index + "</td><td>" + vacancie.name + "</td><td>" + vacancie.salary + "</td><td>" + vacancie.organisationName + "</td><td>" + vacancie.contact + "</td>" +
        "<td>" + vacancie.phoneNumber + "</td><td>" + vacancie.employment + "</td> <td>" + vacancie.description + "</td><td><a class='deleteLink' data-id='" + vacancie.idHH + "'>Удалить вакансию из БД</a></td></tr> ";
}
function BlockPage() {
    $("#content").css("display", "none");
    $("#loading").css("display", "block");
}

function UnBlockPage() {
    $("#content").css("display", "block");
    $("#loading").css("display", "none");
}


function ShowDB() {
    $("#saveBtn").css("display", "none");
    $('.title').text("Последние 50 вакансий из БД");
}

function ShowHH() {
    $("#saveBtn").css("display", "block");
    $('.title').text("Последние 50 неархивных вакансий из hh.ru");
}

$(function () {
    $("body").on("click", ".deleteLink", function () {
       // $(this).parent().parent().remove();
        $.ajax({
            url: '/api/VacanciesDB/' + $(this).data("id"),
            type: 'Delete',
            error: function (jqxhr) {
                alert(jqxhr.responseText);
            },
            complete: function () { GetVacanciesDB(); },
            success: function (data) {              
                alert("Запись удалена");
            }
        });

    });
})

   

$(function () {
    $('#downloadHHBtn').click(function (e) {
        e.preventDefault();
        GetVacanciesHH();
      });
});
$(function () {

    $('#downloadDBBtn').click(function (e) {
        e.preventDefault();
        GetVacanciesDB();
    });
});

$(function () {

    $('#saveBtn').click(function (e) {
        e.preventDefault();
        var idArray = new Array();
        $("table tbody tr").each(function (index, element) {  
           idArray[index] = $(this).data("rowid");            
       }); 

        $.ajax({
            url: '/api/VacanciesHH/',
            type: 'Patch',
            traditional: true,
            contentType: 'application/json',
            beforeSend: function () { BlockPage(); },
            complete: function () { UnBlockPage();},
            dataType: 'json',
            data: JSON.stringify({ ids: idArray }),
            success: function (data) {
                alert('В БД сохранено записей:' + data.ids.length);
                GetVacanciesDB();
            },
            error: function (jqxhr) {
                alert('Ошибка.' + jqxhr.responseText);
            },
           
        });
    });
});

function GetVacanciesHH() {

    
    $("table tbody").find("tr").remove();

    
    $.ajax({
        url: '/api/VacanciesHH/?searchText=' + document.getElementById("searchText").value,
        type: 'Get',
        contentType: "application/json",
        dataType: 'json',
        beforeSend: function () { BlockPage(); },
        complete: function () { UnBlockPage(); },
        error: function (jqxhr) {
            alert(jqxhr.responseText); },
        success: function (data) {
            var rows = "";
            $.each(data, function (index, tr) {

                rows += rowHH(tr,index+1);
            })
            $("table tbody").append(rows);
            ShowHH();
        }


    });
}

function GetVacanciesDB() {


    $("table tbody").find("tr").remove();

    $.ajax({
        url: '/api/VacanciesDB/?searchText=' + document.getElementById("searchText").value,
        type: 'Get',
        contentType: "application/json",
        dataType: 'json',
        beforeSend: function () { BlockPage(); }, 
        complete: function () { UnBlockPage(); },
        error: function (jqxhr) {
            alert(jqxhr.responseText);
        },
        success: function (data) {
            var rows = "";
            $.each(data, function (index, tr) {

                rows += rowDB(tr, index + 1);
            })
            $("table tbody").append(rows);
            ShowDB();
        }


    });
}