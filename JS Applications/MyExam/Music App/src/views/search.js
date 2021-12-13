import { html } from '../lib.js';
import { search } from '../api/data.js';

const searchTemplate = (articles, onSearch, searchName = '') => html`
<section id="searchPage">
    <h1>Search by Name</h1>
    <div class="search">
        <input id="search-input" type="text" name="search" placeholder="Enter desired albums's name" .value=${searchName}>
        <button @click=${onSearch} class="button-list">Search</button>
    </div>
    <h2>Results:</h2>
    <!--Show after click Search button-->
    <div class="search-result">
        ${articles.length == 0 
            ? html`<p class="no-result">No result.</p>`             
            : articles.map(itemTemplate)}     
    </div>
</section>`;

const itemTemplate = (article) => html`
<div class="card-box">
    <img src=${article.imgUrl}>
    <div>
        <div class="text-center">
            <p class="name">Name: ${article.name}</p>
            <p class="artist">Artist: ${article.artist}</p>
            <p class="genre">Genre: ${article.genre}</p>
            <p class="price">Price: $${article.price}</p>
            <p class="date">Release Date: ${article.releaseDate}</p>
        </div>
        <div class="btn-group">
            <a href="/details/${article._id}" id="details">Details</a>
        </div>
    </div>
</div>`;

export async function searchPage(ctx){
    const searchName = ctx.querystring.split('=')[1];

    let articles = [];

    if(searchName){
        articles = await search(searchName);
    }
    
    ctx.render(searchTemplate(articles, onSearch, searchName));

    function onSearch(){
        const queryName = document.getElementById('search-input').value;        
        
        if (queryName){
            ctx.page.redirect('/search?query=' + queryName); 
        } else {
            alert('Name must be at least one symbol');
        }               
    }
}
