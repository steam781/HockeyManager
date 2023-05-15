const burgerElement = document.querySelector('.burger');
const navElement = document.querySelector('nav');

burgerElement.addEventListener('click', () => {
    burgerElement.classList.toggle('active');
    navElement.classList.toggle('active');
});

function sortTable(attribute) {
    var table, rows, switching, i, x, y, shouldSwitch;
    table = document.getElementsByTagName("table")[0];
    switching = true;

    while (switching) {
        switching = false;
        rows = table.rows;

        for (i = 1; i < rows.length - 1; i++) {
            shouldSwitch = false;
            x = rows[i].getElementsByTagName("td")[getAttributeIndex(attribute)].innerHTML;
            y = rows[i + 1].getElementsByTagName("td")[getAttributeIndex(attribute)].innerHTML;


            if (attribute === "number" || attribute === "power" || attribute === "cost") {
                // Convert the values to numbers for numerical comparisons
                x = parseFloat(x);
                y = parseFloat(y);
            }

            if (x > y) {
                shouldSwitch = true;
                break;
            }

            if (i > rowsPerPage * currentPage) {
                console.log("break")
                break;
            }
        }

        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }
}

function getAttributeIndex(attribute) {
    switch (attribute) {
        case "name":
            return 0;
        case "number":
            return 1;
        case "role":
            return 2;
        case "power":
            return 3;
        case "cost":
            return 4;
        default:
            return 0;
    }
}
// Global variables
var currentPage = 1;
var rowsPerPage = 100; // Number of rows to display per page

// Function to display a specific page of rows
function displayRows(page) {
    var table = document.getElementById('playerTable');
    var rows = table.rows;
    var totalRows = rows.length - 1; // Exclude the header row

    // Calculate the start and end index of the rows to display
    var startIndex = (page - 1) * rowsPerPage + 1; // Exclude the header row
    var endIndex = Math.min(startIndex + rowsPerPage, totalRows + 1);

    // Hide all rows initially
    for (var i = 1; i < rows.length; i++) {
        rows[i].style.display = 'none';
    }

    // Display the selected rows for the current page
    for (var j = startIndex; j < endIndex; j++) {
        rows[j].style.display = '';
    }

    // Update pagination buttons
    updatePagination(page, Math.ceil(totalRows / rowsPerPage));
}

// Function to update the pagination buttons
function updatePagination(currentPage, totalPages) {
    var pagination = document.getElementById('pagination');
    pagination.innerHTML = '';

    // Create and append previous button
    var prevButton = document.createElement('button');
    prevButton.innerText = 'Previous';
    prevButton.disabled = currentPage === 1;
    prevButton.addEventListener('click', function() {
        if (currentPage > 1) {
            displayRows(currentPage - 1);
        }
    });
    pagination.appendChild(prevButton);

    // Create and append page buttons
    for (var i = 1; i <= totalPages; i++) {
        var pageButton = document.createElement('button');
        pageButton.innerText = i;
        pageButton.disabled = currentPage === i;
        pageButton.addEventListener('click', function(page) {
            return function() {
                displayRows(page);
            };
        }(i));
        pagination.appendChild(pageButton);
    }

    // Create and append next button
    var nextButton = document.createElement('button');
    nextButton.innerText = 'Next';
    nextButton.disabled = currentPage === totalPages;
    nextButton.addEventListener('click', function() {
        if (currentPage < totalPages) {
            displayRows(currentPage + 1);
        }
    });
    pagination.appendChild(nextButton);
}

// Initial setup
displayRows(currentPage);
