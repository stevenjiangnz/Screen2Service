CREATE TABLE [dbo].[Ideas] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Topic]    NVARCHAR (MAX) NULL,
    [Content]  NVARCHAR (MAX) NULL,
    [Status]   NVARCHAR (MAX) NULL,
    [Owner]    NVARCHAR (MAX) NULL,
    [Created]  DATETIME       NOT NULL,
    [Modified] DATETIME       NOT NULL,
    [Type]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Ideas] PRIMARY KEY CLUSTERED ([Id] ASC)
);

