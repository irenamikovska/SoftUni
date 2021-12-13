import * as api from './api.js';

export const login = api.login;
export const register = api.register;
export const logout = api.logout;

const endpoints = {
    all: '/data/albums?sortBy=_createdOn%20desc&distinct=name',
    byId: '/data/albums/',
    create: '/data/albums',
    edit: '/data/albums/',
    delete: '/data/albums/'
}

export async function getAll(){
    return api.get(endpoints.all);
}

export async function getById(id){
    return api.get(endpoints.byId + id);
}

export async function createItem(article){
    return api.post(endpoints.create, article)
}

export async function editItem(id, article){
    return api.put(endpoints.edit + id, article)
}

export async function deleteItem(id){
    return api.del(endpoints.delete + id)
}

export async function search(query){
    return api.get(`/data/albums?where=name%20LIKE%20%22${query}%22`);
}

