const burgerElement = document.querySelector('.burger');
const navElement = document.querySelector('nav');

burgerElement.addEventListener('click', () => {
    burgerElement.classList.toggle('active');
    navElement.classList.toggle('active');
});

// Pagination
function paginateTable(pageNumber) {
    var table, rowsPerPage, rows, totalPages, i;

    table = document.getElementById("playerTable");
    rowsPerPage = 30;
    rows = table.getElementsByTagName("tr");
    totalPages = Math.ceil((rows.length - 1) / rowsPerPage);

    for (i = 1; i < rows.length; i++) {
        if (i > (pageNumber * rowsPerPage) || i <= ((pageNumber - 1) * rowsPerPage)) {
            rows[i].style.display = "none";
        } else {
            rows[i].style.display = "";
        }
    }

    generatePaginationButtons(pageNumber, totalPages);
}

function generatePaginationButtons(currentPage, totalPages) {
    var paginationDiv = document.getElementById("pagination");
    paginationDiv.innerHTML = "";

    for (var i = 1; i <= totalPages; i++) {
        var button = document.createElement("button");
        button.innerHTML = i;
        button.addEventListener("click", function () {
            paginateTable(parseInt(this.innerHTML));
        });

        if (i === currentPage) {
            button.classList.add("active");
        }

        paginationDiv.appendChild(button);
    }
}

// Initial pagination setup
paginateTable(1);
