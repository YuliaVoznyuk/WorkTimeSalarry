function validateForm() {
    var firstName = document.getElementById("firstName").value;
    var lastName = document.getElementById("lastName").value;
    var phoneNumber = document.getElementById("phoneNumber").value;
    var email = document.getElementById("email").value;
    var address = document.getElementById("address").value;
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;

    // Проверка всех полей на заполнение
    if (firstName === "" || lastName === "" || phoneNumber === "" || email === "" || address === "" || username === "" || password === "") {
        alert("Будь ласка, заповніть всі поля.");
        return false; // Отменяем отправку формы
    }

    return true; // Продолжаем отправку формы
}