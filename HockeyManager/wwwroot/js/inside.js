// JavaScript Code (inside.js)
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

function getPlayerIdFromDataTransfer(dataTransfer) {
    let playerId = dataTransfer.getData('text/plain');

    if (!playerId) {
        playerId = dataTransfer.getData('text/html');
        if (playerId) {
            const tempElement = document.createElement('div');
            tempElement.innerHTML = playerId;
            const playerElement = tempElement.querySelector('[value]');
            playerId = playerElement ? playerElement.getAttribute('value') : null;
        }
    }
    console.log(playerId)
    return playerId;
}

// Get all team player boxes
const playerBoxes = document.querySelectorAll('.player-box');
const teamPlayerBoxes = document.querySelectorAll('.team-player-box');

// Add event listeners for drag and drop events
playerBoxes.forEach((box) => {
    // Allow the box to accept a draggable player
    box.addEventListener('dragover', (event) => {
        event.preventDefault();
    });

    // Handle the dragstart event
    box.addEventListener('dragstart', (event) => {
        // Add the "dragged" class to the dragged element
        lastDraggedElement = event.target;
        lastDraggedElement.classList.add('dragged');
    });

    // Handle the drop event
    box.addEventListener('drop', handleDropEvent);
});

teamPlayerBoxes.forEach((box) => {
    // Allow the box to accept a draggable player
    box.addEventListener('dragover', (event) => {
        event.preventDefault();
    });

    // Handle the drop event
    box.addEventListener('drop', handleDropEvent);
});

function handleDropEvent(event) {
    // Get the dragged player's ID from the last dragged element
    const playerElement = document.querySelector('.dragged');
    const playerId = playerElement.getAttribute('id');

    // Remove the "dragged" class from the last dragged element
    playerElement.classList.remove('dragged');

    if (playerId) {
        // Get the dragged player's role
        const playerElementById = document.getElementById(playerId);
        const playerClasses = playerElementById.classList;
        const playerRole = playerClasses[0];

        // Assign the player to the position (box)
        const positionId = this.id; // Use "this" to refer to the current team-player-box

        // Update the input value for the respective position
        const inputElement = document.querySelector(`input[name="${positionId}"]`);

        console.log(inputElement);

        if (inputElement) {
            inputElement.value = playerId;
            console.log(`Player ${playerId} assigned to position ${positionId}`);
        } else {
            console.log('Input element not found for position', positionId);
        }

        // Update the player name in the current number box
        const currentNumberBox = document.getElementById(`${positionId}-Current`);
        if (currentNumberBox) {
            currentNumberBox.innerText = playerElementById.innerText;
        }
    }

    // Prevent the default drop behavior
    event.preventDefault();
}

//// Save Team Positions
//function saveTeamPositions(playerId, positionId) {
//    const data = { playerId: playerId, positionId: positionId };

//    $.ajax({
//        url: '/Game/SaveTeamPositions',
//        type: 'POST',
//        data: data,
//        success: function () {
//            console.log('Team positions saved successfully.');
//        },
//        error: function () {
//            console.log('Error occurred while saving team positions.');
//        }
//    });
//}

$(document).ready(function () {
    $(".player").click(function () {
        var playerId = $(this).attr("id");
        $.ajax({
            url: "/Market/GetPlayerPartial",
            data: { playerId: playerId },
            type: "GET",
            success: function (data) {
                $("#selectedPlayerPartial").html(data);
            },
            error: function () {
                alert("An error occurred while retrieving the player information.");
            }
        });
    }