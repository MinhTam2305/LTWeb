let openShopping = document.querySelector('.shopping');
let closeShopping = document.querySelector('.closeShopping');
let list = document.querySelector('.list');
let listCard = document.querySelector('.listCard');
let total = document.querySelector('.total');
let quantity = document.querySelector('.quantity');

openShopping.addEventListener('click', () => {
    document.body.classList.add('active');
});

closeShopping.addEventListener('click', () => {
    document.body.classList.remove('active');
});

let products = [
    {
        id: 1,
        name: 'Prisma',
        image: 'f1.1.1.jpg',
        price: 85000000
    },
    {
        id: 2,
        name: 'Onmyoji Senhime AniMester',
        image: 'f2.2.2.jpg',
        price: 22000000
    },
    {
        id: 3,
        name: 'Sakura Kinomoto',
        image: 'f3.3.4.jpg',
        price: 9780000
    },
    {
        id: 4,
        name: 'Sakura Kinomoto: Always Together',
        image: 'f4.4.4.jpg',
        price: 17700000
    },
    {
        id: 5,
        name: 'Kukuru Misakino & Fuka Miyazawa',
        image: 'f5.5.5.jpg',
        price: 17200000
    },
    {
        id: 6,
        name: 'Project Sekai: Hatsune Miku',
        image: 'f6.6.6.jpg',
        price: 16700000
    },
    {
        id: 7,
        name: 'HATSUNE MIKU Digital Stars',
        image: 'f7.7.7.jpg',
        price: 15650000
    },
    {
        id: 8,
        name: 'Azur Lane Illustrious Muse',
        image: 'f8.8.8.jpg',
        price: 120200000
    },
    {
        id: 9,
        name: 'Hatsune Miku with SOLWA',
        image: 'f11.jpg',
        price: 4500000
    },
    {
        id: 10,
        name: 'MAIDMADE Yuki Nagato',
        image: 'f12.jpg',
        price: 7400000
    },
    {
        id: 11,
        name: 'Arknights Surtr Colorful Wonderland',
        image: 'f13.jpg',
        price: 7550000
    },
    {
        id: 12,
        name: 'Mimoza Liliya Classical Blue Style',
        image: 'f14.jpg',
        price: 5180000
    },
    {
        id: 13,
        name: 'PRISMA WING Girls\'',
        image: 'f15.jpg',
        price: 9800000
    },
    {
        id: 14,
        name: 'Senkan Shoujo R Quincy',
        image: 'f16.jpg',
        price: 5480000
    },
    {
        id: 15,
        name: 'Tales of Destiny 2 Reala',
        image: 'f17.jpg',
        price: 10300000
    },
    {
        id: 16,
        name: 'Kagura Nana Artist',
        image: 'f18.jpg',
        price: 5580000
    }
];


let listCards = [];

function initApp() {
    products.forEach((value, key) => {
        let newDiv = document.createElement('div');
        newDiv.classList.add('item');
        newDiv.innerHTML = `
            <img src="img/products/${value.image}"/>
            <div class="title">${value.name}</div>
            <div class="price">${value.price.toLocaleString()}</div>
            <button onclick="addToCard(${key})">Add To Card</button>`;
        list.appendChild(newDiv);
    });
}

function addToCard(key) {
    if (listCards[key] == null) {
        // copy product from list to list card
        listCards[key] = JSON.parse(JSON.stringify(products[key]));
        listCards[key].quantity = 1;
    }
    reloadCard();
}

function reloadCard() {
    listCard.innerHTML = '';
    let count = 0;
    let totalPrice = 0;

    listCards.forEach((value, key) => {
        totalPrice += value.price;
        count += value.quantity;

        if (value != null) {
            let newDiv = document.createElement('li');
            newDiv.innerHTML = `
                <div><img src="img/products/${value.image}"/></div>
                <div>${value.name}</div>
                <div>${value.price.toLocaleString()}</div>
                <div>
                    <button onclick="changeQuantity(${key}, ${value.quantity - 1})">-</button>
                    <div class="count">${value.quantity}</div>
                    <button onclick="changeQuantity(${key}, ${value.quantity + 1})">+</button>
                </div>`;
            listCard.appendChild(newDiv);
        }
    });

    total.innerText = totalPrice.toLocaleString();
    quantity.innerText = count;
}

function changeQuantity(key, quantity) {
    if (quantity === 0) {
        delete listCards[key];
    } else {
        listCards[key].quantity = quantity;
        listCards[key].price = quantity * products[key].price;
    }
    reloadCard();
}

initApp();
