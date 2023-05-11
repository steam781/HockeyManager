const encryptedMail = localStorage.getItem('Email');
const encryptedPass = localStorage.getItem('Password');

// Define the secret key used for encryption/decryption
let secretKey = '<%= HockeyManager.Models.Other.GetKey().SecurityKey %>';

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
}

// Function to encrypt data using the secret key
function encryptData(data) {
    const encryptedData = CryptoJS.AES.encrypt(data, secretKey).toString();
    return encryptedData;
}

const rememberMeCheckbox = document.getElementById('rememberme');

// Add a submit event listener to the form
document.addEventListener("submit", (event) => {
    event.preventDefault();

    const email = document.getElementById('Email').value;
    const password = document.getElementById('Password').value;

    // If the "remember me" checkbox is checked, encrypt the data before storing it
    if (rememberMeCheckbox.checked) {
        const encryptedEmail = encryptData(email);
        const encryptedPassword = encryptData(password);
        localStorage.setItem('Email', encryptedEmail);
        localStorage.setItem('Password', encryptedPassword);
    }

    // Create a new XMLHttpRequest object
    const xhr = new XMLHttpRequest();
    xhr.open('POST', '/Home/Login', true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.onload = function () {
        if (xhr.status === 200) {
            // Handle success response
            const response = JSON.parse(xhr.responseText);
            if (response.success) {
                // User is logged in, redirect to home page
                window.location.href = "/Game/Home";
            } else {
                // Login failed, display error message
                const errorDiv = document.getElementById('error-message');
                errorDiv.innerHTML = response.message;
                errorDiv.style.display = "block";
            }
        } else {
            // Handle error response
            console.log("Error: " + xhr.statusText);
        }
    };

    // Create a JSON object containing the email and password values
    const data = {
        Email: email,
        Password: password
    };

    // Send the data to the server
    xhr.send(JSON.stringify(data));
});
