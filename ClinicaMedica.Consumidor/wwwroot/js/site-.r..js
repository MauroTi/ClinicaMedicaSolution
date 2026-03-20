document.addEventListener("DOMContentLoaded", function () {
    ocultarColunasDeId();
});

function ocultarColunasDeId() {
    const tabelas = document.querySelectorAll("table");

    tabelas.forEach(tabela => {
        const cabecalhos = tabela.querySelectorAll("thead th");
        const indicesParaOcultar = [];

        cabecalhos.forEach((th, index) => {
            const texto = th.textContent.trim().toLowerCase().replace(/\s+/g, "");

            if (texto === "id" || texto.endsWith("id")) {
                indicesParaOcultar.push(index);
                th.style.display = "none";
            }
        });

        if (indicesParaOcultar.length > 0) {
            const linhas = tabela.querySelectorAll("tbody tr");

            linhas.forEach(linha => {
                const colunas = linha.querySelectorAll("td");

                indicesParaOcultar.forEach(indice => {
                    if (colunas.length > indice) {
                        colunas[indice].style.display = "none";
                    }
                });
            });
        }
    });
}