import { getAll } from '../api/data.js';
import { html } from '../lib.js';

const catalogTemplate = (articles) => html`
<section id="catalogPage">
    <h1>All Albums</h1>    
    ${articles.length == 0 
        ? html`<p>No Albums in Catalog!</p>`             
        : articles.map(itemTemplate)}   
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

export async function catalogPage(ctx) {
    const articles = await getAll();   
    //const userData = ctx.userData;
  
    ctx.render(catalogTemplate(articles));
}
