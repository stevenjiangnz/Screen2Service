CREATE TABLE [dbo].[Records] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Title]            NVARCHAR (MAX) NULL,
    [Type]             NVARCHAR (MAX) NULL,
    [ZoneId]           INT            NULL,
    [FileName]         NVARCHAR (MAX) NULL,
    [Path]             NVARCHAR (MAX) NULL,
    [Extension]        NVARCHAR (MAX) NULL,
    [CreateDT]         DATETIME       NOT NULL,
    [TradingDate]      INT            DEFAULT ((0)) NOT NULL,
    [Owner]            NVARCHAR (MAX) NULL,
    [OriginalFileName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Records] PRIMARY KEY CLUSTERED ([Id] ASC)
);



