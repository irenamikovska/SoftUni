import { html } from '../lib.js';
import { search } from '../api/data.js';

const searchTemplate = (cars, onSearch, year) => html`
<section id="search-cars">
    <h1>Filter by year</h1>

    <div class="container">
        <input id="search-input" type="text" name="search" placeholder="Enter desired production year" .value=${year || ''}>
        <button @click=${onSearch} class="button-list">Search</button>
    </div>

    <h2>Results:</h2>
    <div class="listings">
        ${cars.length == 0 
            ? html`<p class="no-cars">No results.</p>`             
            : cars.map(itemTemplate)} 
    </div>
</section>`;

const itemTemplate = (car) => html`
<div class="listing">
    <div class="preview">
        <img src=${car.imageUrl}>
    </div>
    <h2>${car.brand} ${car.model}</h2>
    <div class="info">
        <div class="data-info">
            <h3>Year: ${car.year}</h3>
            <h3>Price: ${car.price} $</h3>
        </div>
        <div class="data-buttons">
            <a href="/details/${car._id}" class="button-carDetails">Details</a>
        </div>
    </div>
</div>`;

export async function searchPage(ctx){
    // take by url
    //search?query=2016
    const year = Number(ctx.querystring.split('=')[1]);

    const cars = Number.isNaN(year) ? [] : await search(year);

    ctx.render(searchTemplate(cars, onSearch, year));

    function onSearch(){
        // go to url
        const queryYear = Number(document.getElementById('search-input').value);
        
        if(Number.isNaN(queryYear) == false){
            ctx.page.redirect('/search?query=' + queryYear); 
        } else {
            alert('Year must be a positive number!');
        }               
    }
}