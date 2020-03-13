DROP Table Article;
DROP Table Cart;
DROP Table CartArticle;
DROP Table Category;
DROP Table Users;


CREATE TABLE Article (
    Id          INT				NOT NULL AUTO_INCREMENT,
    title       VARCHAR (50)    NULL,
    description TEXT            NULL,
    price       DECIMAL (10, 2) NULL,
    addDate     DATETIME        NULL,
    urlImage    VARCHAR (500)   NULL,
    idCategory  INT             NULL,
    PRIMARY KEY (Id)
); 

CREATE TABLE Cart (
    Id     INT          NOT NULL AUTO_INCREMENT,
    userId INT          NOT NULL,
    total  DECIMAL (10,2) NOT NULL,
    PRIMARY KEY (Id)
); 

CREATE TABLE CartArticle (
    Id        INT NOT NULL AUTO_INCREMENT,
    cartId    INT NOT NULL,
    articleId INT NOT NULL,
    quantity  INT NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE Category (
    Id    INT           NOT NULL AUTO_INCREMENT,
    title NVARCHAR (50) NOT NULL,
    PRIMARY KEY (Id)
); 

CREATE TABLE Users (
    Id        INT           NOT NULL AUTO_INCREMENT,
    lastName  VARCHAR (20)  NULL,
    firstName VARCHAR (20)  NULL,
    userName  VARCHAR (20)  NULL,
    phone     VARCHAR (10)  NULL,
    email     VARCHAR (30)  NULL,
    address   VARCHAR (100) NULL,
    zip       INT           NULL,
    city      VARCHAR (50)  NULL,
    password  VARCHAR (50)  NULL,
    admin     VARCHAR (5)   NULL,
    PRIMARY KEY (Id ASC)
)