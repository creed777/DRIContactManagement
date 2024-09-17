// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

var checkedList;

$(document).ready(function () {

    $('#tblDRIList').DataTable(
            {
            "order": [],
            "columnDefs": [{
                "targets": 0,
                "orderable": true,
                "autoWidth": true
            }],
            lengthMenu: [10, 25, 50, { label: 'All', value: -1 }],
            pagingType: 'simple_numbers',
            "fnDrawCallback": function () {
                $('#tblDRIList tbody tr').each(function () {
                    $this = $(this);
                    var driName = $this.find("span.dri-name").text()
                    var driEmail = $this.find("span.dri-email").text();
                    var delegateName = $this.find("span.delegate-name").text();
                    var delegateEmail = $this.find("span.delegate-name").text();

                    if (driName == "" || driEmail == "" || delegateEmail == "" || delegateName == "") {
                        $this.find("span.flag").removeClass("flag-hidden")
                    }
                });
                FilterRowsNoDri();
            }
        });

    $(LoadData())

    $('.table').on('change', function (e) {
        checkboxList = [];
        checkbox = $('[type=checkbox]:checked');
        checkboxList.push($(checkbox).closest('tr'));

        if (checkboxList.length != 0) {
            $('#btnEdit').prop('disabled', false);
            $('#btnSaveChecked').prop('disabled', false);
        }
        else {
            $('#btnEdit').prop('disabled', true);
        }
    });

    var json = $('.table');
    console.writeline(json.data.length + ' row(s) were loaded');

});

$('#tblDRIList').on('length.dt', function (e, settings, len) {
    console.log('New page length: ' + len);
    LoadData();
})

//------------ "filter by" controls --------------------
$('input[type=radio][name=flaggedrow]').on("change", function () {

    $("#tblDRIList tbody tr").show()

    if (this.value == "true") {
        $("#tblDRIList tbody tr").filter(function () {
            return $(this).find("span.flag-hidden").length > 0
        }).hide();
    }
    else if (this.value == "false") {
        $("#tblDRIList tbody tr").filter(function () {
            return $(this).find("span.flag-hidden").length == 0
        }).hide();
    }

    if (this.value != "") {
        $(DisableDDLs(""))
    }
    else {
        $(EnableDDLs())
    }
});

$("#organization").on("change", function () {
    let selected = this.value;

    $("#tblDRIList tr").show().filter(function () {
        return $(this).text().toLowerCase().indexOf(selected.toLowerCase()) == -1 && $(this).html().toLowerCase().indexOf("<th>") == -1;
    }).hide();

    if (this.value != "") {
        $(DisableDDLs("organization"))
    }
    else {
        $(EnableDDLs())
    }
});

$("#serviceGroup").on("change", function () {
    let selected = this.value;

    $("#tblDRIList tr").show().filter(function () {
        return $(this).text().toLowerCase().indexOf(selected.toLowerCase()) == -1 && $(this).html().toLowerCase().indexOf("<th>") == -1;
    }).hide();

    if (this.value != "") {
        $(DisableDDLs("serviceGroup"))
    }
    else {
        $(EnableDDLs())
    }
});

$("#serviceTeam").on("change", function () {
    let selected = this.value;

    $("#tblDRIList tr").show().filter(function () {
        return $(this).text().toLowerCase().indexOf(selected.toLowerCase()) == -1 && $(this).html().toLowerCase().indexOf("<th>") == -1;
    }).hide();

    if (this.value != "") {
        $(DisableDDLs("serviceTeam"))
    }
    else {
        $(EnableDDLs())
    }
});

$("#driName").on("change", function () {
    let selected = this.value;

    $("#tblDRIList tr").show().filter(function () {
        return $(this).text().toLowerCase().indexOf(selected.toLowerCase()) == -1 && $(this).html().toLowerCase().indexOf("<th>") == -1;
    }).hide();

    if (this.value != "") {
        $(DisableDDLs("driName"))
    }
    else {
        $(EnableDDLs())
    }
});

$("#delegateName").on("change", function () {
    let selected = this.value;

    $("#tblDRIList tr").show().filter(function () {
        return $(this).text().toLowerCase().indexOf(selected.toLowerCase()) == -1 && $(this).html().toLowerCase().indexOf("<th>") == -1;
    }).hide();

    if (this.value != "") {
        $(DisableDDLs("delegateName"))
    }
    else {
        $(EnableDDLs())
    }
});

//------------ end of "filter by" controls --------------------

//the requirement is to "suggest" a DRI if it is blank.  Trying to find a comparable org level and populate the blank DRI
function FilterRowsNoDri() {
    let test;
    driRows = $("#tblDRIList tr").filter(function () {
        return ($(this).find("span.dri-name").text() == "" || $(this).find("span.dri-email").text() == "" || $(this).find("span.delegate-name").text() == "" || $(this).find("span.dri-email").text() == "");
    });

    driRows.each(function () {
        serviceIds = $(this).find("span.serviceId").text();
        orgName = $(this).find("td.org-name").text();
        driName = $(this).find("span.dri-name").text();
        driEmail = $(this).find("span.dri-email").text();
        delegateName = $(this).find("span.delegate-name").text();
        delegateEmail = $(this).find("span.delegate-name").text();

        dri = ""
        if (orgName && driName == "") {
            result = $("#tblDRIList tr").filter(function () {
                return $(this).find("td.org-name").text() == orgName && $(this).find("span.dri-name").text() != "" ? $(this) : null;
            })

            propDriName = result.first("span.dri-name").find("span.dri-name").text();
            propDriEmail = result.first("span.dri-name").find("span.dri-email").text();
            $(this).find("span.dri-name").text(propDriName);
            $(this).find("span.dri-name").css("color", "red")
            $(this).find("span.dri-email").text(propDriEmail);
            $(this).find("span.dri-email").css("color", "red")

            propDelegateName = result.first("span.delegate-name").find("span.delegate-name").text();
            propDelegateEmail = result.first("span.delegate-email").find("span.delegate-email").text();
            $(this).find("span.delegate-name").text(propDelegateName);
            $(this).find("span.delegate-name").css("color", "red")
            $(this).find("span.delegate-email").text(propDelegateEmail);
            $(this).find("span.delegate-email").css("color", "red")
        }        
    });
}

function LoadData() {

    var indexToLoad = [4, 5, 6, 7, 9]

    for (let i of indexToLoad) {

        LoadDDL(i);
    }
}

function LoadDDL(ddlIndex) {

    var name = [];
    var distinctName = [];

    $('#tblDRIList tbody tr').each(function () {
        name.push($(this).find(':nth-child(' + ddlIndex + ')').text());
    });

    for (var n of name) {
        if (n != "" && n != undefined && !distinctName.includes(n))
            distinctName.push(n);
    }
    distinctName.sort();

    switch (ddlIndex) {
        case 4:
            for (var index = 0; index <= distinctName.length; index++) {
                if (distinctName[index] != undefined) {
                    $('#organization').append('<option value="' + distinctName[index] + '">' + distinctName[index] + '</option>');
                }
            }
            break;
        case 5:
            for (var index = 0; index <= distinctName.length; index++) {
                if (distinctName[index] != undefined) {
                    $('#serviceGroup').append('<option value="' + distinctName[index] + '">' + distinctName[index] + '</option>');
                }
            }
            break;
        case 6:
            for (var index = 0; index <= distinctName.length; index++) {
                if (distinctName[index] != undefined) {
                    $('#serviceTeam').append('<option value="' + distinctName[index] + '">' + distinctName[index] + '</option>');
                }
            }
            break;
        case 7:
            for (var index = 0; index <= distinctName.length; index++) {
                if (distinctName[index] != undefined) {
                    $('#driName').append('<option value="' + distinctName[index] + '">' + distinctName[index] + '</option>');
                }
            }
            break;
        case 9:
            for (var index = 0; index <= distinctName.length; index++) {
                if (distinctName[index] != undefined) {
                    $('#delegateName').append('<option value="' + distinctName[index] + '">' + distinctName[index] + '</option>');
                }
            }
            break;
    }
}

function EnableDDLs() {
    $("select").each(function () {
        $(this).prop("disabled", false)
    });

}

function DisableDDLs(ddlName) {
    $("select").each(function () {
        if (this.id != ddlName) {
            $(this).prop("disabled", true)
        }
    });
}

$(btnEdit).on('click', function () {
    checkedList = $('[type=checkbox]:checked');
})

$(btnSave).on('click', function () {

    var contactList = [];

    for (var item of checkedList) {
        var serviceId = $(item).closest('tr').find(".serviceId").text().replace(/\s+/g, " ").trim()

        var temp = {
            ServiceId: serviceId,
            DRIName: $('#newDriName').val(),
            DRIEmail: $('#newDriEmail').val(),
            DelegateName: $('#newDelegateName').val(),
            DelegateEmail: $('#newDelegateEmail').val()
        }

        contactList.push(temp)
    }

    $.ajax({
        type: "POST",
        data: JSON.stringify(contactList),
        headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() },
        contentType: "application/json",
        success: function () {
            checkedList = $('[type=checkbox]:checked');
            for (item in checkedList){
                $(item).prop("checked", "")
            }
            alert("Data was saved successfully")
            $('#editDialog').modal('hide');
        },
        error: function () {
            alert("There was an error saving the data")
        }
    });
})

$(btnCheckAll).on('click', function () {
 
    checkboxList = $("#tblDRIList tr").filter(function () {
        return $(this).is(":not(':hidden')");
    })

    for (item in checkboxList) {
        flag = $(item).find('.checkbox')
        flag.prop("checked", "checked")
        $(btnSaveChecked).prop('disabled', '')
    }
})

$(btnSaveChecked).on('click', function () {
    if (checkboxList.length == 0) {
        checkboxList = $('[type=checkbox]:checked');
    }

    SaveUpdates(checkboxList);
})

function SaveUpdates(saveList) {
    var contactList = [];

    $('#spinnerDialog').modal('show');

    for (var item of saveList) {
        var serviceId = $(item).find(".serviceId").text().replace(/\s+/g, " ").trim()

        var temp = {
            ServiceId: serviceId,
            DRIName: $('#newDriName').val() != '' ? $('#newDriName').val() : $(item).find("span.dri-name").text(),
            DRIEmail: $('#newDriEmail').val() != '' ? $('#newDriEmail').val() : $(item).find("span.dri-email").text(),
            DelegateName: $('#newDelegateName').val() != '' ? $('#newDelegateName').val() : $(item).find("span.delegate-name").text(),
            DelegateEmail: $('#newDelegateEmail').val() != '' ? $('#newDelegateEmail').val() : $(item).find("span.delegate-email").text()
        }

        if (serviceId != '') {
            contactList.push(temp)
        }
    }

    $.ajax({
        type: "POST",
        data: JSON.stringify(contactList),
        headers: { "RequestVerificationToken": $('input:hidden[name="__RequestVerificationToken"]').val() },
        contentType: "application/json",
        success: function () {
            for (item in saveList) {
                $(item).prop("checked", "")
            }
            alert("Data was saved successfully")
        },
        error: function () {
            alert("There was an error saving the data")
        }
    });

    $('#spinnerDialog').modal('hide');
}