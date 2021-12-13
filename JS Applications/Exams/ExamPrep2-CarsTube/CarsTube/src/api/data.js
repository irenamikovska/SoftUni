import * as api from './api.js';

export const login = api.login;
export const register = api.register;
export const logout = api.logout;

const endpoints = {
    all: '/data/cars?sortBy=_createdOn%20desc',
    byId: '/data/cars/',
    //myItems: (userId) => `/data/cars?where=_ownerId%3D%22${userId}%22&sortBy=_createdOn%20desc`,
    create: '/data/cars',
    edit: '/data/cars/',
    delete: '/data/cars/'
}
//pagination
export async function getAll(page = 1){
    return api.get(`/data/cars?sortBy=_createdOn%20desc&offset=${(page - 1) * 3}&pageSize=3`);
}

export async function getById(id){
    return api.get(endpoints.byId + id);
}

export async function getMyList(userId){
    //return api.get(endpoints.myItems(userId));
    return api.get(`/data/cars?where=_ownerId%3D%22${userId}%22&sortBy=_createdOn%20desc`);
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

export async function search(query){
    return api.get(`/data/cars?where=year%3D${query}`);
}

//pagination
export async function getCollectionSize(){
    return api.get('/data/cars?count');
}