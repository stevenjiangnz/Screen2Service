CREATE TABLE [dbo].[Rules] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Type]        NVARCHAR (MAX) NULL,
    [Formula]     NVARCHAR (MAX) NULL,
    [Note]        NVARCHAR (MAX) NULL,
    [Owner]       NVARCHAR (MAX) NULL,
    [IsSystem]    BIT            DEFAULT ((0)) NOT NULL,
    [Direction]   NVARCHAR (MAX) NULL,
    [Assembly]    NVARCHAR (MAX) NULL,
    [UpdatedDT]   DATETIME       DEFAULT ('1900-01-01T00:00:00.000') NOT NULL,
    CONSTRAINT [PK_dbo.Rules] PRIMARY KEY CLUSTERED ([Id] ASC)
);



