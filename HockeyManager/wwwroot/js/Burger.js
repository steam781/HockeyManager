const burgerElement = document.querySelector('.burger');
const navElement = document.querySelector('nav');

burgerElement.addEventListener('click', () => {
    burgerElement.classList.toggle('active');
    navElement.classList.toggle('active');
});