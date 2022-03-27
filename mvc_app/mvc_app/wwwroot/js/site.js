const form = document.getElementById('form');
const name = document.getElementById('name');
const email = document.getElementById('email');
const message = document.getElementById('message');

//VALIDERAR LÄNGD MED REGULAR EXPRESSION
function check_length(element) {

    const regEx_length = /^([A-Za-z]).{1,}$/;
    const error_text = document.getElementById(`${element.id}-error`);
    if (!regEx_length.test(element.value)) {
        error_text.innerHTML = `You need at least 2 letters in your ${element.id}.`;
    } else {
        error_text.innerHTML = ``;
    }
}


//VALIDERAR EMAIL ADRESSER MED REGULAR EXPRESSION. MIN KRAV EX: a@a.se
function check_email(element) {
    const regEx_email = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    const error_text = document.getElementById('email-error');
    if (!regEx_email.test(element.value)) {
        error_text.innerHTML = `You need a valid email address.`;
    } else {
        error_text.innerHTML = ``;
    }
}

email.addEventListener('keyup', function (e) {
    check_email(e.target)
});

name.addEventListener('keyup', function (e) {
    check_length(e.target)
})

message.addEventListener('keyup', function (e) {
    check_length(e.target)
})