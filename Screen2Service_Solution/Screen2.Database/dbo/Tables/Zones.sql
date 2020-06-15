CREATE TABLE [dbo].[Zones] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [StartDate]   DATETIME       NOT NULL,
    [Status]      NVARCHAR (MAX) NULL,
    [Note]        NVARCHAR (MAX) NULL,
    [Owner]       NVARCHAR (MAX) NULL,
    [EndDate]     DATETIME       NULL,
    [TradingDate] INT            DEFAULT ((0)) NOT NULL,
    [IsCurrent]   BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Zones] PRIMARY KEY CLUSTERED ([Id] ASC)
);



