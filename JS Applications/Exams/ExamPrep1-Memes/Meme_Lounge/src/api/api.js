import { notify } from "../notify.js";
import { getUserData, setUserData, clearUserData } from "../util.js";

const host = 'http://localhost:3030';

async function request(url, options) {
    try {
        const response = await fetch(host + url, options);

        if (response.ok == false) {
            if(response.status == 403){             
                clearUserData();
            }
            const error = await response.json();
            throw new Error(error.message);            
        } 
        
        try {           
            return await response.json();
        } catch(err) {
            return response;
        }
    } catch (err) {
        //alert(err.message);
        notify(err.message);
        throw err;
    }
}

function createOptions(method = 'get', data){
    const options = {
        method,
        headers: {}
    };

    if (data != undefined){
        options.headers['Content-Type'] = 'application/json';
        options.body = JSON.stringify(data);
    }

    const userData = getUserData();
    if (userData != null){
        options.headers['X-Authorization'] = userData.token;
    }

    return options;
}

export async function get(url) {
    return request(url, createOptions());
}

export async function post(url, data) {
    return request(url, createOptions('post', data));
}

export async function put(url, data) {
    return request(url, createOptions('put', data));
}

export async function del(url) {
    return request(url, createOptions('delete'));
}

export async function login(email, password){
    const response = await post('/users/login', {email, password});
    const userData = {
        username: response.username,
        email: response.email,
        id: response._id,
        gender: response.gender,
        token: response.accessToken
    };
    setUserData(userData);
    return response;
}

export async function register(username, email, password, gender){
    const response = await post('/users/register', {username, email, password, gender});
    const userData = {
        username: response.username,
        email: response.email,
        id: response._id,
        gender: response.gender,
        token: response.accessToken
    };
    setUserData(userData);
    return response;
}

export async function logout(){
    await get('/users/logout');
    clearUserData();
}
