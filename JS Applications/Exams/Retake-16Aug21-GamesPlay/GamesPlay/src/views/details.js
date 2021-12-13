import { deleteItem, getById, addComment, getComments } from '../api/data.js';
import { html } from '../lib.js';

const detailsTemplate = (game, isOwner, onDelete, comments, showAddBtn, onComment) => html`
<section id="game-details">
    <h1>Game Details</h1>
    <div class="info-section">
        <div class="game-header">
            <img class="game-img" src=${game.imageUrl}/>
            <h1>${game.title}</h1>
            <span class="levels">MaxLevel: ${game.maxLevel}</span>
            <p class="type">${game.category}</p>
        </div>
        <p class="text">${game.summary}</p>

        <!-- Bonus ( for Guests and Users ) -->
        <div class="details-comments">
            <h2>Comments:</h2>
            <ul>
            ${comments.length == 0 
            ? html`<p class="no-comment">No comments.</p>`             
            : comments.map(commentTemplate)}  
                <!-- list all comments for current game (If any) -->                
            </ul>
            <!-- Display paragraph: If there are no games in the database -->            
        </div>

        <!-- Edit/Delete buttons ( Only for creator of this game )  -->
        ${isOwner ? html`
        <div class="buttons">
            <a href="/edit/${game._id}" class="button">Edit</a>
            <a @click=${onDelete} href="javascript:void(0)" class="button">Delete</a>
        </div>` : null}
    </div>

    <!-- Bonus -->
    <!-- Add Comment ( Only for logged-in users, which is not creators of the current game ) -->
    ${showAddBtn ? html`
    <article class="create-comment">
        <label>Add new comment:</label>
        <form @submit=${onComment} class="form">
            <textarea id="com" name="comment" placeholder="Comment......"></textarea>
            <input class="btn submit" type="submit" value="Add Comment">
        </form>
    </article>` : null}
</section>`;


const commentTemplate = (com) => html`
<li class="comment">
    <p>Content: ${com.comment}</p>
</li>`;

export async function detailsPage(ctx) {
    const gameId = ctx.params.id;

    //const game = await getById(gameId);
    
    const [game, comments] = await Promise.all([
        getById(gameId),
        getComments(gameId)        
    ]);

    const isOwner = ctx.userData && ctx.userData._id == game._ownerId;
    const showAddBtn = ctx.userData && isOwner == false;

    ctx.render(detailsTemplate(game, isOwner, onDelete, comments, showAddBtn, onComment));

    async function onDelete() {
        const confirmed = confirm('Are you sure you want to delete it?');

        if (confirmed) {
            await deleteItem(gameId);
            ctx.page.redirect('/');
        }
    }

    async function onComment(event) {
        event.preventDefault();
        const formData = new FormData(event.target);
        const comment = formData.get('comment').trim();
       
        if (comment == '') {
            return alert('You have to write at least one symbol!');
            //return;
        }
       
        await addComment(gameId, comment);
       
        //redirect to the same page will add comment
        event.target.reset();
        ctx.page.redirect('/details/' + gameId);
    }
}
