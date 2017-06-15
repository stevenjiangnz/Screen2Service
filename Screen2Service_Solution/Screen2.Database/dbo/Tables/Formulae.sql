CREATE TABLE [dbo].[Formulae] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Defintion]   NVARCHAR (MAX) NULL,
    [Type]        NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Formulae] PRIMARY KEY CLUSTERED ([Id] ASC)
);

