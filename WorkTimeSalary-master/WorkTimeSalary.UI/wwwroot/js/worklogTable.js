function addRow() {
    var table = document.getElementById('myTable');
    var row = table.insertRow(-1); // Вставляем новую строку в конец таблицы
    var cell1 = row.insertCell(0);
    var cell2 = row.insertCell(1);
    var cell3 = row.insertCell(2);
    var cell4 = row.insertCell(3);
    cell1.innerHTML = '<input type="datetime-local" class="start-date">';
    cell2.innerHTML = '<input type="datetime-local" class="end-date">';
    cell3.contentEditable = true;
    cell4.innerHTML = '<button class="delete-row-button" onclick="deleteRow(this)">-</button>';

    // Добавляем обработчики событий для новых полей начала и конца
    var startDateInput = cell1.querySelector('.start-date');
    var endDateInput = cell2.querySelector('.end-date');

    startDateInput.addEventListener('input', function () {
        var rowIndex = this.parentNode.parentNode.rowIndex - 1; // -1 для коррекции смещения
        var value = this.value;
        localStorage.setItem('start-date-' + rowIndex, value);
    });

    endDateInput.addEventListener('input', function () {
        var rowIndex = this.parentNode.parentNode.rowIndex - 1; // -1 для коррекции смещения
        var value = this.value;
        localStorage.setItem('end-date-' + rowIndex, value);
    });
}

function deleteRow(btn) {
    var row = btn.parentNode.parentNode;
    row.parentNode.removeChild(row);
}

// При загрузке страницы восстанавливаем даты из localStorage
var startDateInputs = document.querySelectorAll('.start-date');
var endDateInputs = document.querySelectorAll('.end-date');

startDateInputs.forEach(function (input) {
    var rowIndex = input.parentNode.parentNode.rowIndex - 1; // -1 для коррекции смещения
    var savedValue = localStorage.getItem('start-date-' + rowIndex);
    if (savedValue !== null) {
        input.value = savedValue;
    }
});

endDateInputs.forEach(function (input) {
    var rowIndex = input.parentNode.parentNode.rowIndex - 1; // -1 для коррекции смещения
    var savedValue = localStorage.getItem('end-date-' + rowIndex);
    if (savedValue !== null) {
        input.value = savedValue;
    }
});

function saveData() {
    var table = document.getElementById('myTable');
    var lastRow = table.rows[table.rows.length - 1]; // Получаем последнюю строку таблицы
    var startDate = lastRow.querySelector('.start-date').value;
    var endDate = lastRow.querySelector('.end-date').value;
    var description = lastRow.cells[2].innerText; // Получаем текст из ячейки с описанием


    var workLogDTO = {
        id: 0,
        description: description,
        startTime: new Date(startDate), 
        endTime: new Date(endDate), 
        employeeId: parseInt(lastRow.dataset.employeeId) 
    };

    // Отправляем данные на сервер
    fetch('/WorkLog/Index', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(workLogDTO)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return;
        })
        .catch(error => {
            console.error('There was a problem with your fetch operation:', error);
        });
}