CREATE TABLE [dbo].[Users] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [lastName] VARCHAR (20) NULL,
    [firstName]  VARCHAR (20) NULL,
    [userName]  VARCHAR (20) NULL,
	[phone]     VARCHAR (10)  NULL,
    [email]     VARCHAR (30)  NULL,
    [address]     VARCHAR (100)  NULL,
	[zip]     INT  NULL,
	[city]     VARCHAR (50)  NULL,
    [password]  VARCHAR (50)  NULL,
    [admin]     VARCHAR (5)  NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
	)