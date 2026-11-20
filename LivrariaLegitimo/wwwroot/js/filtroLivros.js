document.addEventListener("DOMContentLoaded", () => {
    const cards = document.querySelectorAll(".div-mainList-section-lista .card1-2");

    const filtrosGeneros = document.querySelectorAll(
        ".div-mainList-section-filtros-generos input[type='checkbox']"
    );

    const filtrosTipo = document.querySelectorAll(
        ".div-mainList-section-filtros-tipoLivro input[type='checkbox']"
    );

    const filtrosPreco = document.querySelectorAll(
        ".div-mainList-section-filtros-faixapreco input[type='number']"
    );

    const filtrosAvaliacao = document.querySelectorAll(
        ".div-mainList-section-filtros-avaliacao-selector input[type='number']"
    );

    function NormalizarTexto(texto) {
        return texto
            .toLowerCase()
            .normalize("NFD")
            .replace(/[\u0300-\u036f]/g, "")
            .trim();
    }

    function ConverterPreco(preco) {
        return Number(
            preco.replace(/\./g, "").replace(",", ".")
        );
    }

    function FiltrarLivros() {
        const generosSelecionados = Array.from(filtrosGeneros)
            .filter(input => input.checked)
            .map(input => NormalizarTexto(input.nextElementSibling.textContent));

        const tiposSelecionados = Array.from(filtrosTipo)
            .filter(input => input.checked)
            .map(input => NormalizarTexto(input.nextElementSibling.textContent));

        const precoMinimo = filtrosPreco[0].value !== ""
            ? ConverterPreco(filtrosPreco[0].value)
            : null;

        const precoMaximo = filtrosPreco[1].value !== ""
            ? ConverterPreco(filtrosPreco[1].value)
            : null;

        const avaliacaoMinima = filtrosAvaliacao[0].value !== ""
            ? Number(filtrosAvaliacao[0].value)
            : null;

        const avaliacaoMaxima = filtrosAvaliacao[1].value !== ""
            ? Number(filtrosAvaliacao[1].value)
            : null;

        cards.forEach(card => {
            const generos = card.dataset.generos
                .split("|")
                .map(genero => NormalizarTexto(genero));

            const preco = ConverterPreco(card.dataset.preco);
            const avaliacao = Number(card.dataset.avaliacao);
            const tipo = NormalizarTexto(card.dataset.tipo);
            const ebook = card.dataset.ebook === "True" || card.dataset.ebook === "true";

            const correspondeGenero =
                generosSelecionados.length === 0 ||
                generosSelecionados.some(genero => generos.includes(genero));

            const correspondeTipo =
                tiposSelecionados.length === 0 ||
                tiposSelecionados.some(tipoSelecionado => {
                    if (tipoSelecionado === "ebook") {
                        return ebook;
                    }

                    return tipo === tipoSelecionado;
                });

            const correspondePreco =
                (precoMinimo === null || preco >= precoMinimo) &&
                (precoMaximo === null || preco <= precoMaximo);

            const correspondeAvaliacao =
                (avaliacaoMinima === null || avaliacao >= avaliacaoMinima) &&
                (avaliacaoMaxima === null || avaliacao <= avaliacaoMaxima);

            const mostrarLivro =
                correspondeGenero &&
                correspondeTipo &&
                correspondePreco &&
                correspondeAvaliacao;

            card.style.display = mostrarLivro ? "" : "none";
        });
    }

    filtrosGeneros.forEach(input => {
        input.addEventListener("change", FiltrarLivros);
    });

    filtrosTipo.forEach(input => {
        input.addEventListener("change", FiltrarLivros);
    });

    filtrosPreco.forEach(input => {
        input.addEventListener("input", FiltrarLivros);
    });

    filtrosAvaliacao.forEach(input => {
        input.addEventListener("input", FiltrarLivros);
    });
    FiltrarLivros();
});

