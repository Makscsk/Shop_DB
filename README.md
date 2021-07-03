<h1 align="center">Automated System Of A Sports Equipment Store</h1>
<h2 align="center">

<p align="center">
  <img width="200" height="200" src="https://bs-soft.ru/templates/Default/images/icon7.png">
</p>
  
</h2>

## Description

**The conceptual model of the "Shop" database is shown in Image 1.**

<p align="center">
<img src="https://i.ibb.co/F6bKr7h/4.png" width="60%"></p>
<p align="center">
Image 1 - Conceptual model of the "Shop" database</p>
 
**This project is implemented in [C#](https://ru.wikipedia.org/wiki/C_Sharp), on [Windows Forms](https://ru.wikipedia.org/wiki/Windows_Forms).**

## How to use it

### You need to perform several *__operations__*:

- **Log** in to your profile (if you don't have one, then register).
- **Select** a product from the list of products.
- **Specify** the required quantity of the product
- **Click** the "Добавить в корзину" button.
- **Click** the "Корзина" button.
- **Click** the "Оформление заказа с копией в PDF" button.

Then go to the store to pick up your product.

**Image 2 shows the main form of the store.**

<p align="center">
<img src="https://i.ibb.co/ZSmchnz/image.png" width="50%"></p>
<p align="center">
Image 2 - The main form of the store</p>

## About the project

### SQL

- The MySQL database management system was used.
- A simple example of queries in the code: ```SELECT Категории.Наименование AS Категория, Продукты.Наименование, Количество, Цена FROM Продукты, Категории WHERE Категории.ID_Категория=Продукты.ID_Категория```.
- It was also possible to output images through the code.

## Project setup

1. The main thing is to install [Visual Studio](https://visualstudio.microsoft.com/ru/)
2. Launch "Shopping.sln".
3. Assign 'connectionString' the connection path to the "Shop" database.
