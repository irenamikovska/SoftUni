const element = document.getElementById('errorBox');
const outputMessage = element.querySelector('span');

export function notify(msg){
    outputMessage.textContent = msg;
    element.style.display = 'block';

    setTimeout(() => element.style.display = 'none', 3000);
}