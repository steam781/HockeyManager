const encryptedMail = localStorage.getItem('Email');
const encryptedPass = localStorage.getItem('Password');
console.log(encryptedMail + ", " + encryptedPass)

// Define the secret key used for encryption/decryption
let secretKey = '<%= HockeyManager.Models.Other.GetKey().SecurityKey %>';
console.log(secretKey)
// Function to decrypt data using the secret key
function decryptData(encryptedData) {
    console.log("decrypting")
    const bytes = CryptoJS.AES.decrypt(encryptedData, secretKey);
    const decryptedData = bytes.toString(CryptoJS.enc.Utf8);
    return decryptedData;
}

if (encryptedMail && encryptedPass) {
    console.log("autologin");
    const mail = decryptData(encryptedMail);
    const pass = decryptData(encryptedPass);
    console.log(mail + " and " + pass)
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
    console.log("prevented")
    const email = document.getElementById('Email').value;
    const password = document.getElementById('Password').value;
    // If the "remember me" checkbox is checked, encrypt the data before storing it
    if (rememberMeCheckbox.checked) {
        const encryptedEmail = encryptData(email);
        const encryptedPassword = encryptData(password);
        localStorage.setItem('Email', encryptedEmail);
        localStorage.setItem('Password', encryptedPassword);
    }


    const user = {
        Email: email,
        Password: password
    };


    const xhr = new XMLHttpRequest();
    xhr.open('POST', '/Home/Login', true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.onload = function () {
        if (xhr.status === 200) {
            // Handle success response
            console.log("Handle success response")
        } else {
            // Handle error response
            console.log("Handle error response")
        }
    };
    // Create a new FormData object
    const formData = new FormData();

    // Append the email and password values to the formData object
    formData.append('Email', email);
    formData.append('Password', password);

    console.log(formData);

    // Send the data to the server
    xhr.send(formData);
});

// Call the GetKey() method in your C# code to retrieve the SecurityKey value
