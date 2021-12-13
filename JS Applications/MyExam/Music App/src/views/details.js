
import { deleteItem, getById } from '../api/data.js';
import { html } from '../lib.js';
//import { getUserData } from '../util.js';

const detailsTemplate = (article, isOwner, onDelete) => html`
<section id="detailsPage">
    <div class="wrapper">
        <div class="albumCover">
            <img src=${article.imgUrl}>
        </div>
        <div class="albumInfo">
            <div class="albumText">

                <h1>Name: ${article.name}</h1>
                <h3>Artist: ${article.artist}</h3>
                <h4>Genre: ${article.genre}</h4>
                <h4>Price: $${article.price}</h4>
                <h4>Date: ${article.releaseDate}</h4>
                <p>Description: ${article.description}</p>
            </div>

            <!-- Only for registered user and creator of the album-->
            ${isOwner ? html`
            <div class="actionBtn">
                <a href="/edit/${article._id}" class="edit">Edit</a>
                <a @click=${onDelete} href="javascript:void(0)" class="remove">Delete</a>
            </div>` : null}
        </div>
    </div>
</section>`;

export async function detailsPage(ctx) {
    const articleId = ctx.params.id;
    const article = await getById(articleId);

    const isOwner = ctx.userData && ctx.userData._id == article._ownerId;
   
    ctx.render(detailsTemplate(article, isOwner, onDelete));

    async function onDelete() {
        const confirmed = confirm('Are you sure you want to delete it?');

        if (confirmed) {
            await deleteItem(articleId);
            ctx.page.redirect('/articles');
        }
    }
}
