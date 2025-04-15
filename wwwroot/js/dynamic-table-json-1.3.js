
if (typeof jQuery === 'undefined') {

    var DynamicTable = {
        renderTable: function (options) {
            console.error("❌ Dynamic Table Vrs 1.3 : jQuery is not loaded. Please include jQuery library. 🛑");
        }
    };

} else {

    var DynamicTable = {
        renderTable: function (options) {
            var StaticHeader = options.StaticHeader || true;
            var jsonData = options.jsonData;
            var targetElementId = options.targetElementId;
            var tableClasses = options.tableClasses || [];
            var columnModifiers = options.columnModifiers || [];
            var rowsPerPage = 5;
            var TableCover = false;

            function Rendering(columnModifiers, value) {

                return columnModifiers[value] || value;
            }

            var $targetElement = $('#' + targetElementId);

            var $table = $('<table></table>');
            var TableDivId = targetElementId;
            var TableId = targetElementId.replace("Div", "");
            $table.attr('id', TableId);

            var $existingTable = $('#' + TableId);
            if ($existingTable.length) {
                $existingTable.remove();
            }

            if (!jsonData) {
                console.error("❌Dynamic Table: There is no data available to render the table 🫣🙄");
                return;
            }

            if (!$targetElement.length) {
                console.error("❌Dynamic Table: Target element not found. 🧐");
                return;
            }

            if (!tableClasses.length) {
                console.warn("⚠️Dynamic Table: No class attribute to be added to the table 😏");
            }

            if (Array.isArray(tableClasses)) {
                $table.addClass(tableClasses.join(' '));
            }

            var $thead = $('<thead></thead>');
            var $headerRow = $('<tr style="background-color:white"></tr>');
            for (var key in jsonData[0]) {
                var $th = $('<th NOWRAP data-sortable></th>');
                $th.text(key);
                $headerRow.append($th);
            }
            $thead.append($headerRow);
            $table.append($thead);

            var $tbody = $('<tbody></tbody>');
            for (var i = 0; i < jsonData.length; i++) {
                var rowData = jsonData[i];
                var $row = $('<tr></tr>');
                var columnIndex = 0; // Track the current column index
                for (var key in rowData) {
                    var $cell = $('<td></td>');
                    $cell.attr("nowrap", "");

                    var modifier = columnModifiers.find(function (modifier) {
                        return parseInt(modifier[0]) === columnIndex + 1;
                    });

                    if (modifier) {
                        var allText = '';
                        if (typeof modifier[1] === 'string') {
                            allText = modifier[1];
                        } else {
                            allText = Rendering(modifier[1], rowData[key]);
                        }

                        $cell.html(allText.replace(/{{td}}/g, rowData[key]));
                    } else {
                        $cell.text(rowData[key] !== null ? rowData[key] : '');
                    }

                    $row.append($cell);
                    columnIndex++;
                }
                $tbody.append($row);
            }
            $table.append($tbody);
            $targetElement.append($table);


            if (StaticHeader == true) {
 
 

                //--------
                $(`#${TableDivId}`).css({
                    'overflow': 'auto',
                    'max-height': '400px',
                    'width': '100%'
                });

                $(`#${TableId}`).css({
                    'border-collapse': 'collapse',
                    'width': '100%'
                });

                $(`#${TableId} th, #${TableId} td`).css({
                    'padding': '8px',
                    'border': '1px solid #ddd'
                });

                $(`#${TableId} th`).css({
                    'cursor': 'pointer'
                });

                $(`#${TableId} td`).css({
                    'white-space': 'nowrap',
                    'overflow': 'hidden',
                    'text-overflow': 'ellipsis'
                });

                $(`#${TableDivId}`).on('scroll', function () {
                    var scroll = $(this).scrollTop();
                    $('#TempTable1 thead').css('transform', 'translateY(' + scroll + 'px)');
                });


               //console.info("✅Dynamic Table: Static Headers Added");
            }


            if (TableCover == true) {

                $(`#${TableId} th`).each(function (index) {

                    var SortIcon = `<span class="sortIcon" title="Click to SORT" >
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="23" height="23">
              <g id="iconUp" style="display: block;">
                <path d="M7 14l5-5 5 5z"></path>
              </g>
              <g id="iconDown" style="display: none;">
                <path d="M7 10l5 5 5-5z"></path>
              </g>
            </svg>
          </span>`;
                    $(this).append(SortIcon);
                });


                //console.info("✅Dynamic Table: Created ");

    

                var DivBottomRow = `<div id="${TableId}DivBottomRow" style="border: 1px solid #dddddd;background: #f2f2f2;display:flex;justify-content: space-between;flex-wrap: wrap;flex-direction: row;">    <label style="padding: 4px;font-weight:bold;" id="${TableId}RowsCount" >Rows Count : </label>`;


                if ($(`#${TableId}DivBottomRow`).length === 0) {
                    $(`#${TableDivId}`).after(DivBottomRow);
                }

                //$(`#${TableDivId}`).after(DivBottomRow);

                var tableRows = $(`#${TableId} tbody tr`);
                var totalRows = tableRows.length;
                console.log(totalRows)

                $(`#${TableId}RowsCount`).text('Rows Count : ' + totalRows);


                //Pagination Functionality

                var DivPagination = $(`<div id="${TableId}pagination" style="display: flex;justify-content: flex-end;background: #f2f2f2;padding: 4px;border: 1px solid #dddddd;""> </div>`);

                if (!($(`#${TableId}pagination`).is('*'))) {
                    $(`#${TableId}DivBottomRow`).append(DivPagination);
                }

                $(`#${TableId}pagination .page-link`).css({
                    'padding': '4px',
                    'margin': '1px',
                    'border': '1px solid gray',
                    'border-radius': '3px',
                });


                var table = $(`#${TableDivId}`);
                var tbody = table.find('tbody');
                var rows = tbody.find('tr');
                var currentPage = 1;
                var sortColumn = null;
                var sortOrder = 1;

                function showPage(page) {




                    var start = (page - 1) * rowsPerPage;
                    var end = start + rowsPerPage;
                    rows.hide().slice(start, end).show();
                }

                showPage(currentPage);

                function setupPagination() {
                    var totalPages = Math.ceil(rows.length / rowsPerPage);
                    var pagination = $(`#${TableId}pagination`);
                    pagination.empty();

                    for (var i = 1; i <= totalPages; i++) {
                        $(`<span class="${TableId}page-link" style="padding: 1px 5px 1px 5px;margin: 1px;border: 1px solid gray; border-radius: 2px; cursor: pointer;">${i} </span>`).appendTo(pagination);
                    }

                    pagination.on('click', `.${TableId}page-link`, function () {
                        currentPage = parseInt($(this).text());
                        showPage(currentPage);
                    });
                }

                setupPagination();

                $(`#${TableId}pagination .${TableId}page-link`).on('click', function (e) {
                    $(`#${TableId}pagination .${TableId}page-link`).css('background-color', '#9a9c9a').css('font-weight', '');
                    $(this).css('background-color', '').css('font-weight', 'bold');
                    console.log(this);
                });


                //Sorting functionality



                var initialDisplayState = {};
                $(`#${TableId} tbody tr`).each(function (index, row) {
                    var rowDisplay = $(row).css('display');
                    initialDisplayState[index] = rowDisplay;
                });

                function sortTable(columnIndex, order) {



                    var sortRows = $(`#${TableId} tbody tr`).toArray();

                    sortRows.sort(function (a, b) {
                        var aValue = $(a).find('td').eq(columnIndex).text();
                        var bValue = $(b).find('td').eq(columnIndex).text();

                        if (isNaN(aValue) || isNaN(bValue)) {
                            return aValue.localeCompare(bValue) * order;
                        } else {
                            return (parseInt(aValue) - parseInt(bValue)) * order;
                        }
                    });

                    $(`#${TableId} tbody`).empty();
                    $.each(sortRows, function (index, row) {
                        $(`#${TableId} tbody`).append(row);
                    });


                    $(`#${TableId} tbody tr`).each(function (index, row) {
                        $(row).css('display', initialDisplayState[index]);
                    });
                }
                table.on('click', 'th[data-sortable]', function () {

                    $(this).find('#iconUp, #iconDown').toggle();
                    var columnIndex = $(this).index();

                    if (columnIndex === sortColumn) {
                        sortOrder *= -1;
                    } else {
                        sortColumn = columnIndex;
                        sortOrder = 1;
                    }

                    sortTable(columnIndex, sortOrder);
                });


                $('#rowsPerPage').on('change', function () {
                    rowsPerPage = parseInt($(this).val());
                    currentPage = 1;
                    showPage(currentPage);
                    setupPagination();
                });



                // TOP Row functionality

                var DivTopRow = $(`<div id="${TableDivId}DivTopRow" style="display: flex;justify-content: space-between;background: #f2f2f2;padding: 4px;border: 1px solid #dddddd;""> </div>`);


                if (!($(`#${TableDivId}DivTopRow`).is('*'))) {
                    $(`#${TableDivId}`).before(DivTopRow);
                }

                var tableRows = $(`#${TableId} tbody tr`);



                // Rows Filtering Functionality

                var rowsPerPageOptions = [5, 10, 25, 50, 100];
                var $rowsPerPageSelect = $(`<select id="${TableId}rowsPerPageSelect" style="border: none;padding: 5px;margin: 3px;width: 5%;"></select>`);

                $.each(rowsPerPageOptions, function (index, value) {
                    $rowsPerPageSelect.append('<option value="' + value + '">' + value + '</option>');
                });

                $rowsPerPageSelect.change(function () {
                    var selectedRowsPerPage = parseInt($(this).val());
                    rowsPerPage = selectedRowsPerPage;
                    currentPage = 1;
                    showPage(currentPage);
                    setupPagination();
                });


                if (!($(`#${TableId}rowsPerPageSelect`).is('*'))) {
                    $(`#${TableDivId}DivTopRow`).prepend($rowsPerPageSelect);
                }



                // Search functionality

                var SearchInput = `<input id="${TableId}searchInput" type="text"  style="border: none;padding: 5px;margin: 3px;width: 20%;" placeholder="Search...">`;

                if (!($(`#${TableId}searchInput`).is('*'))) {
                    $(`#${TableDivId}DivTopRow`).prepend(SearchInput);
                }
                $(`#${TableId}searchInput`).on('keyup', function () {
                    var searchText = $(this).val().toLowerCase();

                    $(`#${TableId} tbody tr`).each(function () {
                        var rowText = $(this).text().toLowerCase();
                        if (rowText.indexOf(searchText) === -1) {
                            $(this).hide();
                        } else {
                            $(this).show();
                        }
                    });

                    // If the search input is empty, show all rows and reapply pagination
                    if (searchText === '') {
                        showPage(currentPage);
                        setupPagination();
                    }
                });

                // console.info("✅Dynamic Table: Search functionality Added ");

            }
            console.info("✅Dynamic Table: Created Successfully! 🤩");

        },
    };

}