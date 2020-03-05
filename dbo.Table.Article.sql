CREATE TABLE [dbo].[Article] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [title]       VARCHAR (50)    NOT NULL,
    [description] TEXT            NOT NULL,
    [price]       DECIMAL (10, 2) NOT NULL,
    [addDate]     DATETIME        NOT NULL,
    [urlImage]       VARCHAR (MAX)   NOT NULL,
    [idCategory]  INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
