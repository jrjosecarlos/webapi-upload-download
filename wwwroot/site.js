const uri = "api/arquivo";
let arquivos = null;

$(document).ready(function () {
    getData();
});

function getData() {
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        success: function (data) {
            const tBody = $("#arquivos");

            $(tBody).empty();

            $.each(data, function (key, item) {
                const tr = $("<tr></tr>") 
                    .append($("<td></td>").text(item.nome))
                    .append($("<td></td>").text(item.caminho != null ? item.caminho : "N/A"))
                    .append(
                        $("<td></td>").append(
                            $("<input/>", {
                                type: "checkbox",
                                disabled: true,
                                checked: item.isArquivoDB
                            })
                        )
                    )
                    .append(
                        $("<td></td>").append(
                            $("<button>Download</button>").on("click", function () {
                                editItem(item.id);
                            })
                        )
                    )
                    .append(
                        $("<td></td>").append(
                            $("<button>Excluir</button>").on("click", function () {
                                deleteItem(item.id);
                            })
                        )
                    );

                tr.appendTo(tBody);
            });

            arquivos = data;
        }
    });
}

function addItem() {
    const fileInput = $("#add-file")[0];

    const payload = {
        nome: fileInput.value.replace(/.*[\/\\]/, ''),
        caminho: "fixo",
        isArquivoDB: $("#add-DB").is(":checked")
    };

    let formData = new FormData();

    formData.append("payload", JSON.stringify(payload));
    formData.append("arquivo", fileInput.files[0]);

    const tBody = $("#arquivos");

    tBody.hide();

    $.ajax({
        type: "POST",
        url: uri,
        contentType: false,
        enctype: 'multipart/form-data',
        processData: false,
        data: formData,
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
            console.log(errorThrown);
        },
        success: function (result) {
            getData();
            fileInput.value = "";
        },
        complete: function (jqXHR) {
            tBody.show();
        }
    });
}

/*
function deleteItem(id) {
    $.ajax({
        url: uri + "/" + id,
        type: "DELETE",
        success: function (result) {
            getData();
        }
    });
}

function editItem(id) {
    $.each(todos, function (key, item) {
        if (item.id === id) {
            $("#edit-name").val(item.name);
            $("#edit-id").val(item.id);
            $("#edit-isComplete")[0].checked = item.isComplete;
        }
    });
    $("#spoiler").css({ display: "block" });
}

$(".my-form").on("submit", function () {
    const item = {
        name: $("#edit-name").val(),
        isComplete: $("#edit-isComplete").is(":checked"),
        id: $("#edit-id").val()
    };

    $.ajax({
        url: uri + "/" + $("#edit-id").val(),
        type: "PUT",
        accepts: "application/json",
        contentType: "application/json",
        data: JSON.stringify(item),
        success: function (result) {
            getData();
        }
    });

    closeInput();
    return false;
});
*/
function closeInput() {
    $("#spoiler").css({ display: "none" });
}