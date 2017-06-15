CREATE TABLE [dbo].[Brokers] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Shortable]   BIT            NOT NULL,
    [FeeRate]     FLOAT (53)     NOT NULL,
    [MinFee]      FLOAT (53)     DEFAULT ((0)) NOT NULL,
    [Owner]       NVARCHAR (MAX) NULL,
    [IsActive]    BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Brokers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

