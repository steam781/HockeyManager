const encryptedMail = localStorage.getItem('Email');
const encryptedPass = localStorage.getItem('Password');
console.log(encryptedMail + ", " + encryptedPass)

// Define the secret key used for encryption/decryption
const secretKey = 'mysecretkey';

// Function to decrypt data using the secret key
function decryptData(encryptedData) {
    const bytes = CryptoJS.AES.decrypt(encryptedData, secretKey);
    const decryptedData = bytes.toString(CryptoJS.enc.Utf8);
    return decryptedData;
}

if (encryptedMail && encryptedPass) {   
    const mail = decryptData(encryptedMail);
    const pass = decryptData(encryptedPass);

    document.getElementById('Email').value = mail;
    document.getElementById('Password').value = pass;
    document.getElementById('submit').click();
}

// Function to encrypt data using the secret key
function encryptData(data) {
    const encryptedData = CryptoJS.AES.encrypt(data, secretKey).toString();
    return encryptedData;
}

const rememberMeCheckbox = document.getElementById('rememberme');
console.log(rememberMeCheckbox)

// Add a submit event listener to the form
document.addEventListener("submit", (event) => {
    event.preventDefault();

    // If the "remember me" checkbox is checked, encrypt the data before storing it
    if (rememberMeCheckbox.checked) {
        const email = document.getElementById('Email').value;
        const password = document.getElementById('Password').value;
        const encryptedEmail = encryptData(email);
        const encryptedPassword = encryptData(password);
        localStorage.setItem('Email', encryptedEmail);
        localStorage.setItem('Password', encryptedPassword);
    }

    const email = document.getElementById('Email').value;
    const password = document.getElementById('Password').value;

    const user = {
        Email: email,
        Password: password
    };

    const xhr = new XMLHttpRequest();
    xhr.open('POST', '/ControllerName/Login', true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.onload = function () {
        if (xhr.status === 200) {
            // Handle success response
        } else {
            // Handle error response
        }
    };
    xhr.send(JSON.stringify(user));
}); send(formData);