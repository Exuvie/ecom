DROP Table Article;
DROP Table Cart;
DROP Table CartArticle;
DROP Table CaTegory;
DROP Table Users;


CREATE TABLE [dbo].[Article] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [title]       VARCHAR (50)    NULL,
    [description] TEXT            NULL,
    [price]       DECIMAL (10, 2) NULL,
    [addDate]     DATETIME        NULL,
    [urlImage]    VARCHAR (MAX)   NULL,
    [idCategory]  INT             NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
); 

CREATE TABLE [dbo].[Cart] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [userId] INT          NOT NULL,
    [total]  DECIMAL (10,2) NOT NULL,
    [registerDate] DATETIME NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
); 

CREATE TABLE [dbo].[CartArticle] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [cartId]    INT NOT NULL,
    [articleId] INT NOT NULL,
    [quantity]  INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Category] (
    [Id]    INT           IDENTITY (1, 1) NOT NULL,
    [title] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
); 

CREATE TABLE [dbo].[Users] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [lastName]  VARCHAR (20)  NULL,
    [firstName] VARCHAR (20)  NULL,
    [userName]  VARCHAR (20)  NULL,
    [phone]     VARCHAR (10)  NULL,
    [email]     VARCHAR (30)  NULL,
    [address]   VARCHAR (100) NULL,
    [zip]       INT           NULL,
    [city]      VARCHAR (50)  NULL,
    [password]  VARCHAR (50)  NULL,
    [admin]     VARCHAR (5)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)