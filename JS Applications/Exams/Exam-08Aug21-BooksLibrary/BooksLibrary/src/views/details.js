import { addLike, deleteItem, getById, getLikes, getMyLikes } from '../api/data.js';
import { html } from '../lib.js';

const detailsTemplate = (book, isOwner, onDelete, likes, showLikeBtn, onLike) => html`
<section id="details-page" class="details">
    <div class="book-information">
        <h3>${book.title}</h3>
        <p class="type">Type: ${book.type}</p>
        <p class="img"><img src=${book.imageUrl}></p>
        <div class="actions">
            ${isOwner ? html`
            <!-- Edit/Delete buttons ( Only for creator of this book )  -->
            <a class="button" href="/edit/${book._id}">Edit</a>
            <a @click=${onDelete} class="button" href="javascript:void(0)">Delete</a>` : null}

            <!-- Bonus -->
            <!-- Like button ( Only for logged-in users, which is not creators of the current book ) -->
            ${showLikeBtn ? html`
            <a @click=${onLike} class="button" href="javascript:void(0)">Like</a>` : null}

            <!-- ( for Guests and Users )  -->
            <div class="likes">
                <img class="hearts" src="/images/heart.png">
                <span id="total-likes">Likes: ${likes}</span>
            </div>
            <!-- Bonus -->
        </div>
    </div>
    <div class="book-description">
        <h3>Description:</h3>
        <p>${book.description}</p>
    </div>
</section>`;

export async function detailsPage(ctx) {
    const bookId = ctx.params.id;
        
    const [book, likes, hasLike] = await Promise.all([
        getById(bookId),
        getLikes(bookId),
        ctx.userData ? getMyLikes(bookId, ctx.userData._id) : 0
    ]);
   
    const isOwner = ctx.userData && ctx.userData._id == book._ownerId;
    const showLikeBtn = ctx.userData && isOwner == false && hasLike == false;
    
    ctx.render(detailsTemplate(book, isOwner, onDelete, likes, showLikeBtn, onLike));

    async function onDelete() {
        const confirmed = confirm('Are you sure you want to delete it?');

        if (confirmed) {
            await deleteItem(bookId);
            ctx.page.redirect('/');
        }
    }

    async function onLike(){
        await addLike(bookId);
        //redirect to the same page will increase counter of likes
        ctx.page.redirect('/details/' + bookId);
    }
}
