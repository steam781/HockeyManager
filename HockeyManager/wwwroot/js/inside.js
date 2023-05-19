 //JavaScript Code (inside.js)
 //Har andävnt ChatGTP för komentarer då det underlättar med orginisation
 //Pagination and Filtering


if (typeof jQuery === 'undefined') {
    console.error('jQuery is not loaded in inside.js.');
} else {
    console.log('jQuery is loaded successfully in inside.js.');
}

document.addEventListener("DOMContentLoaded", function () {
    function filterAndPaginateTable(pageNumber) {
        var selectedRole = document.getElementById('role-filter');
        if (!selectedRole) return; // Check if the element exists

        console.log('filterAndPaginateTable function called.');

        selectedRole = selectedRole.value;
        var table, rowsPerPage, rows, filteredRows, totalPages, i;

        table = document.getElementById("Player-table");

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

    //Initial setup
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

            //Make an AJAX request to retrieve the player details
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

        console.log("PlayerID:" + playerId)

        return playerId;
    }

    //Get all team player boxes
    const playerBoxes = document.querySelectorAll('.player-box');
    const teamPlayerBoxes = document.querySelectorAll('.team-player-box');

    console.log(playerBoxes);
    console.log(teamPlayerBoxes);

    //Add event listeners for drag and drop events
    playerBoxes.forEach((box) => {
        //Allow the box to accept a draggable player
        box.addEventListener('dragover', (event) => {
            console.log("dragover");
            event.preventDefault();
        });

        //Handle the dragstart event
        box.addEventListener('dragstart', (event) => {
            //Add the "dragged" class to the dragged element
            console.log("dragstart");
            lastDraggedElement = event.target;
            lastDraggedElement.classList.add('dragged');

            // Get the value of the dragged element
            const playerId = event.target.getAttribute('value');

            // Set the playerId as data in the DataTransfer object
            event.dataTransfer.setData('text/plain', playerId);
        });


        //Handle the drop event
        console.log("droped");
        box.addEventListener('drop', handleDropEvent);
    });

    teamPlayerBoxes.forEach((box) => {
        //Allow the box to accept a draggable player
        box.addEventListener('dragover', (event) => {
            console.log("dragover accept");
            event.preventDefault();
        });

        //Handle the drop event
        console.log("drop accept");
        box.addEventListener('drop', handleDropEvent);
    });

    function handleDropEvent(event) {
        event.preventDefault();
        const playerId = event.dataTransfer.getData("text");
        const positionId = event.target.parentElement.getAttribute("data-position-id");

        console.log("Player", playerId, "assigned to position", positionId);

        const playerElement = document.querySelector(`tr[data-id="${playerId}"]`);
        console.log("PlayerElement:", playerElement);

        if (playerElement) {
            const playerRole = playerElement.getAttribute("data-role");
            if (playerRole === positionId) {
                // Update input element value
                const inputElement = document.getElementById(positionId + "Input");
                console.log("check check: " + inputElement + " and  " + inputElement.getAttribute("value"))
                if (inputElement) {
                    inputElement.setAttribute("value", playerId);
                    console.log("Input value set:", inputElement.getAttribute("value"));
                }

                // Update current-number element text
                const currentNumberElement = document.getElementById(positionId + "Current");
                if (currentNumberElement) {
                    currentNumberElement.textContent = playerElement.querySelector(".name").textContent;
                    console.log("Current-number text updated:", currentNumberElement.textContent);
                }
            } else {
                console.log("Player role does not match position");
            }
        }
    }

        event.preventDefault();
});

// Save Team Positions
function saveTeamPositions(playerId, positionId) {
    const data = { playerId: playerId, positionId: positionId };

    $.ajax({
        url: '/Game/SaveTeamPositions',
        type: 'POST',
        data: data,
        success: function () {
            console.log('Team positions saved successfully.');
        },
        error: function () {
            console.log('Error occurred while saving team positions.');
        }
    });
}

$(document).ready(function () { 
    $(".player").click(function () {
        var playerId = $(this).attr("id");
        $.ajax({
            url: "/Game/PlayerDetail",
            data: { playerId: playerId },
            type: "GET",
            success: function (data) {
                $("#selectedPlayerDetail").html(data);
            },
            error: function () {
                alert("An error occurred while retrieving the player information.");
            }
        });
    });
});
