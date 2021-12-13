export function getUserData() {
    const user = sessionStorage.getItem('userData');
    if(user) {
        return JSON.parse(user);
    } else {
        return undefined;
    }
    //return JSON.parse(sessionStorage.getItem('userData'));
}

export function setUserData(user) {
    sessionStorage.setItem('userData', JSON.stringify(user));
}

export function clearUserData() {
    sessionStorage.removeItem('userData');
}
