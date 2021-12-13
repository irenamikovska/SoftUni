import * as api from './api.js';

export const login = api.login;
export const register = api.register;
export const logout = api.logout;

const endpoints = {
    all: '/data/games?sortBy=_createdOn%20desc',
    last: '/data/games?sortBy=_createdOn%20desc&distinct=category',
    byId: '/data/games/',    
    create: '/data/games',
    edit: '/data/games/',
    delete: '/data/games/'
}

export async function getAll(){
    return api.get(endpoints.all);
}

export async function getLast(){
    return api.get(endpoints.last);
}

export async function getById(id){
    return api.get(endpoints.byId + id);
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

export async function addComment(gameId, comment){
    return api.post('/data/comments', {
        gameId,
        comment
    });
}

export async function getComments(gameId){
    return api.get(`/data/comments?where=gameId%3D%22${gameId}%22`);
}
