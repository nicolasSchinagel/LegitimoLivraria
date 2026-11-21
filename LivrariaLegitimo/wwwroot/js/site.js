// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// site.js
//Máscaras
$(document).ready(function () {
    $('.CEP').mask('00000-000');
    $('.dinheiro').mask('000.000,00', { reverse: true });
    $('.CPF').mask('000.000.000-00', { reverse: true });
    $('.CNPJ').mask('00.000.000/0000-00', { reverse: true });
    $('.NumeroCartao').mask('0000.0000.0000.0000', { reverse: true });
    $('.Telefone').mask('(00) 00000-0000');
    $('.Nascimento').mask("00/00/0000", { placeholder: "__/__/____" });
    $('.ISBN').mask("000-00-0000-000-0", { reverse: true });
})


// API CEP
$(document).ready(function () {
    $(".CEP").mask("00000-000");
});

$(document).ready(function () {

    function limpa_formulario_cep() {
        $("#uf").val("");
        $("#cidade").val("");
        $("#endereco").val("");
        $("#bairro").val("");
        $("#complemento").val("");
    }
    // perder foco CEP
    $("#cep").blur(function () {
        var cep = $(this).val().replace(/\D/g, '');

        // verifica valor cep

        if (cep != "") {
            //regular expression
            var validacep = /^[0-9]{8}$/;

            // valida formato
            if (validacep.test(cep)) {

                $("#uf").val("...");
                $("#cidade").val("...");
                $("#logradouro").val("...");
                $("#bairro").val("...");
                $("#complemento").val("...");

                $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {

                    if (!("erro" in dados)) {
                        // atualiza os campos com valores da consulta
                        $("#uf").val(dados.uf);
                        $("#cidade").val(dados.localidade);
                        $("#logradouro").val(dados.logradouro);
                        $("#bairro").val(dados.bairro);
                        $("#complemento").val(dados.complemento);
                    }
                    else {
                        // cep pesquisado n existe
                        limpa_formulario_cep();
                        alert("CEP não foi encontrado.");
                    }

                })
            }
            else {
                // cep inváliado
                limpa_formulario_cep();
                alert("Formato de CEP inválido.");
            }
        }
        else {
            //cep sem valor
            limpa_formulario_cep();
        }
    })
})


