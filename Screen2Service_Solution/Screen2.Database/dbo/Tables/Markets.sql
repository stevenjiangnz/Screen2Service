CREATE TABLE [dbo].[Markets] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (MAX) NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [SymbolSuffix] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Markets] PRIMARY KEY CLUSTERED ([Id] ASC)
);

