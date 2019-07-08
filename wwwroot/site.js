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
                    .append(
                        $("<td class='center'></td>").append(
                            $("<input/>", {
                                type: "checkbox",
                                disabled: true,
                                checked: item.isArquivoDB
                            })
                        )
                    )
                    .append(
                        $("<td></td>").append(
                            $("<a>Download</a>").attr("href", uri + "/" + item.id)
                        )
                    );

                tr.appendTo(tBody);
            });

            arquivos = data;
        }
    });
}

function addItem() {
    $("#info").hide();

    const fileInput = $("#add-file")[0];

    const payload = {
        nome: fileInput.value.replace(/.*[\/\\]/, ''),
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
            var divInfo = $("#info")[0];

            var conteudoInfo;

            if (jqXHR.responseJSON.errors) {
                conteudoInfo = $("<ul/>");

                $.each(jqXHR.responseJSON.errors, function (campo, erros) {
                    erros.forEach(function (mensagemErro) {
                        const ul = $("<li/>")
                            .append(
                                $("<strong/>").text(campo + ": ")
                            )
                            .append(mensagemErro);

                        conteudoInfo.append(ul);
                    });
                   
                });
                
            } else {
                conteudoInfo = $("<span/>")
                    .append("Verifique os dados e tente novamente.");
            }

            $(divInfo).empty()
                .text("Erro ocorrido no processamento:")
                .append(conteudoInfo)
                .show();

            console.log(errorThrown + ": " + textStatus);
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

function downloadItem(id) {
    window.open(window.location + uri + "/" + id/*, "_blank"*/); /* Voluntariamente mantido sem o _blank */
}
