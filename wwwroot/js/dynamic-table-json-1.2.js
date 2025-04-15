
var DynamicTable = {
    renderTable: function (options) {
        var jsonData = options.jsonData;
        var targetElementId = options.targetElementId;
        var tableClasses = options.tableClasses || [];
        var columnModifiers = options.columnModifiers || [];
         
        function Rendering(columnModifiers,value) {
        return columnModifiers[value] || value;
        }

        var targetElement = document.getElementById(targetElementId);

        var table = document.createElement("table");
        var divId = targetElementId.replace("Div", "");
        table.id = divId;

        var existingTable = document.getElementById(divId);
        if (existingTable) {
            existingTable.parentNode.removeChild(existingTable);
        }

        if (!jsonData) {
            console.error("❌Dynamic Table: There is no data available to render the table 🫣🙄");
            return;
        }

        if (!targetElement) {
            console.error("❌Dynamic Table: Target element not found. 🧐");
            return;
        }

        if (!tableClasses.length) {
            console.warn("⚠️Dynamic Table: No class attribute to be added to the table 😏");
        }

        if (Array.isArray(tableClasses)) {
            tableClasses.forEach(function (className) {
                table.classList.add(className);
            });
        }

        var thead = document.createElement("thead");
        var headerRow = document.createElement("tr");
        for (var key in jsonData[0]) {
            var th = document.createElement("th");
            th.textContent = key;
            headerRow.appendChild(th);
        }
        thead.appendChild(headerRow);
        table.appendChild(thead);

        var tbody = document.createElement("tbody");
        for (var i = 0; i < jsonData.length; i++) {
            var rowData = jsonData[i];
            var row = document.createElement("tr");
            var columnIndex = 0;  
            for (var key in rowData) {
                var cell = document.createElement("td");
                cell.setAttribute("nowrap", "");

                var modifier = columnModifiers.find(function (modifier) {
                    return parseInt(modifier[0]) === columnIndex + 1;
                });

                if (modifier) {
   
                    if (typeof modifier[1] === 'string') {
                        var allText =   modifier[1];
                      } else  {
                          var allText = Rendering(modifier[1],rowData[key]);
                      }
                      

                    cell.innerHTML = allText.replace(/{{td}}/g, rowData[key]);
                } else {
                    cell.textContent = rowData[key] !== null ? rowData[key] : '';
                }

                row.appendChild(cell);
                columnIndex++;
            }
            tbody.appendChild(row);
        }
        table.appendChild(tbody);
        targetElement.appendChild(table);

        console.info("✅Dynamic Table: Created Successfully! 🤩");
    },
};