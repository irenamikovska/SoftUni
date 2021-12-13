import { html } from '../lib.js';
import { searchBooks} from '../api/data.js';

const searchTemplate = (books, onSearch, searchText = '') => html`
<section id="search-page" class="dashboard">
<h1>Search</h1>
<ul class="other-books-list">

<form @submit=${onSearch}>
    <input type="text" name ="search" .value=${searchText}>
    <input type ="submit" value="Search">
</form>

${books.length == 0 
       ? html`<p class="no-books">No results!</p>`             
       : books.map(itemTemplate)}  
</ul>     
</section>`;

const itemTemplate = (book) => html`
<li class="otherBooks">
    <h3>${book.title}</h3>
    <p>Type: ${book.type}</p>
    <p class="img"><img src=${book.img}></p>
    <a class="button" href="/details/${book._id}">Details</a>
</li>`;

export async function searchPage(ctx){
    // take by url
    //search?query=2016
    const searchText = ctx.querystring.split('=')[1];

    let books = [];

    if(searchText){
        books = await searchBooks(decodeURIComponent(searchText));
    }

    ctx.render(searchTemplate(books, onSearch, searchText));

    function onSearch(event){
        // go to url
        event.preventDefault();
        const formData = new FormData(event.target);
        const search = formData.get('search');
       
        if(search){
            ctx.page.redirect('/search?query=' + encodeURIComponent(search)); 
        }           
    }
}
