// Pagination and Filtering
function filterAndPaginateTable(pageNumber) {
    var selectedRole = document.getElementById('role-filter');
    if (!selectedRole) return; // Check if the element exists

    selectedRole = selectedRole.value;
    var table, rowsPerPage, rows, filteredRows, totalPages, i;

    table = document.getElementById("playerTable");
    if (!table) return; // Check if the table exists

    rowsPerPage = 30;
    rows = table.querySelectorAll("tr.player-box");
    filteredRows = Array.from(rows).filter(function (row) {
        var playerRole = row.getAttribute('data-role');
        return selectedRole === "" || selectedRole === "All Roles" || playerRole === selectedRole;
    });

    totalPages = Math.ceil(filteredRows.length / rowsPerPage);

    var startIndex = (pageNumber - 1) * rowsPerPage;
    var endIndex = startIndex + rowsPerPage;

    for (i = 0; i < rows.length; i++) {
        var playerBox = rows[i];
        if (filteredRows.includes(playerBox)) {
            if (filteredRows.indexOf(playerBox) >= startIndex && filteredRows.indexOf(playerBox) < endIndex) {
                playerBox.style.display = "flex";
            } else {
                playerBox.style.display = "none";
            }
        } else {
            playerBox.style.display = "none";
        }
    }

    generatePaginationButtons(pageNumber, totalPages);
}




function generatePaginationButtons(currentPage, totalPages) {
    var paginationDiv = document.getElementById("pagination");
    if (!paginationDiv) return; // Check if the pagination div exists

    paginationDiv.innerHTML = "";

    for (var i = 1; i <= totalPages; i++) {
        var button = document.createElement("button");
        button.innerHTML = i;
        button.addEventListener("click", function () {
            filterAndPaginateTable(parseInt(this.innerHTML));
        });

        if (i === currentPage) {
            button.classList.add("active");
        }

        paginationDiv.appendChild(button);
    }
}

// Initial setup
filterAndPaginateTable(1);

if (document.getElementById('role-filter') != null) {
    document.getElementById('role-filter').addEventListener('change', function () {
        filterAndPaginateTable(1);
    });
}

//PlayerDetails

$(document).ready(function () {
    $('.player-click').click(function () {
        console.log("you tried");
        $('.player-click').removeClass('selected');
        $(this).closest('.player-click').addClass('selected');

        var playerId = $(this).closest('.player-click').data('player-id');

        console.log(playerId);

        // Make an AJAX request to retrieve the player details
        $.ajax({
            url: '/Game/PlayerDetail/' + playerId,
            type: 'GET',
            success: function (data) {
                $('#player-detail').html(data);
            },
            error: function () {
                console.log('Error occurred while retrieving player details.');
            }
        });
        
    });
});


