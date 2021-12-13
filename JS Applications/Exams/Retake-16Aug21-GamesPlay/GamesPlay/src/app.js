import { logout } from './api/data.js';
import { page, render } from './lib.js';
import { getUserData } from './util.js';
import { homePage } from './views/home.js';
import { catalogPage } from './views/catalog.js';
import { createPage } from './views/create.js';
import { detailsPage } from './views/details.js';
import { editPage } from './views/edit.js';
import { loginPage } from './views/login.js';
import { registerPage } from './views/register.js';


const container = document.getElementById('main-content'); 
document.getElementById('logoutBtn').addEventListener('click', onLogout);

page(decorateContext);
page('/', homePage);
page('/games', catalogPage);
page('/details/:id', detailsPage);
page('/login', loginPage);
page('/register', registerPage);
page('/create', createPage);
page('/edit/:id', editPage);

updateUserNav();
page.start();

function decorateContext(ctx, next){
    ctx.render = (content) => render(content, container);
    ctx.updateUserNav = updateUserNav;
    ctx.userData = getUserData();
    next();
}

function updateUserNav(){
    const userData = getUserData();
    
    if(userData) {
        document.getElementById('user').style.display = 'block';
        document.getElementById('guest').style.display = 'none';
        
    } else {
        document.getElementById('user').style.display = 'none';
        document.getElementById('guest').style.display = 'block';
    }
}

function onLogout(){
    logout();
    updateUserNav();
    page.redirect('/');
}
