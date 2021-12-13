import * as api from './api.js';

export const login = api.login;
export const register = api.register;
export const logout = api.logout;

const endpoints = {
    all: '/data/books?sortBy=_createdOn%20desc',
    byId: '/data/books/',
    //myItems: (userId) => `/data/books?where=_ownerId%3D%22${userId}%22&sortBy=_createdOn%20desc`,
    create: '/data/books',
    edit: '/data/books/',
    delete: '/data/books/'
}

export async function getAll(){
    return api.get(endpoints.all);
}

export async function getById(id){
    return api.get(endpoints.byId + id);
}

export async function getMyList(userId){
    //return api.get(endpoints.myItems(userId));
    return api.get(`/data/books?where=_ownerId%3D%22${userId}%22&sortBy=_createdOn%20desc`);
}

export async function createItem(car){
    return api.post(endpoints.create, car)
}

export async function editItem(id, car){
    return api.put(endpoints.edit + id, car)
}

export async function deleteItem(id){
    return api.del(endpoints.delete + id)
}

export async function addLike(bookId){
    return api.post('/data/likes', {
        bookId
    });
}

export async function getLikes(bookId){
    return api.get(`/data/likes?where=bookId%3D%22${bookId}%22&distinct=_ownerId&count`);
}

export async function getMyLikes(bookId, userId){
    return api.get(`/data/likes?where=bookId%3D%22${bookId}%22%20and%20_ownerId%3D%22${userId}%22&count`);
}

export async function searchBooks(query){
    return api.get('/data/books?where=' + encodeURIComponent(`title LIKE "${query}"`));
}
