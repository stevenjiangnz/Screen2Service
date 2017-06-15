CREATE TABLE [dbo].[Functoids] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [name]         NVARCHAR (MAX) NULL,
    [returnType]   NVARCHAR (MAX) NULL,
    [defaultValue] NVARCHAR (MAX) NULL,
    [usage]        NVARCHAR (MAX) NULL,
    [Description]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Functoids] PRIMARY KEY CLUSTERED ([Id] ASC)
);



