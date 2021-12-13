import { getMyList } from '../api/data.js';
import { html } from '../lib.js';

const profileTemplate = (books) => html`
<section id="my-books-page" class="my-books">
    <h1>My Books</h1>
    <!-- Display ul: with list-items for every user's books (if any) -->
    <ul class="my-books-list">
    ${books.length == 0 
            ? html`<p class="no-books">No books in database!</p>`             
            : books.map(itemTemplate)}    
    </ul>    
</section>`;

const itemTemplate = (book) => html`
<li class="otherBooks">
    <h3>${book.title}</h3>
    <p>Type: ${book.type}</p>
    <p class="img"><img src=${book.imageUrl}></p>
    <a class="button" href="/details/${book._id}">Details</a>
</li>`;


export async function myListPage(ctx) {
    const userId = ctx.userData._id;
    const books = await getMyList(userId);
    ctx.render(profileTemplate(books));
}
