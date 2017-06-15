CREATE TABLE [dbo].[Settings] (
    [Id]    INT             IDENTITY (1, 1) NOT NULL,
    [Key]   NVARCHAR (100)  NULL,
    [Value] NVARCHAR (2000) NULL,
    CONSTRAINT [PK_dbo.Settings] PRIMARY KEY CLUSTERED ([Id] ASC)
);

