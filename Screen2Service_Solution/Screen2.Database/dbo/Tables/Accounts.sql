CREATE TABLE [dbo].[Accounts] (
    [Id]                INT            IDENTITY (1, 1) NOT NULL,
    [Name]              NVARCHAR (MAX) NULL,
    [Description]       NVARCHAR (MAX) NULL,
    [Owner]             NVARCHAR (MAX) NULL,
    [Status]            NVARCHAR (MAX) NULL,
    [CreateDate]        DATETIME       NOT NULL,
    [CreatedBy]         NVARCHAR (MAX) NULL,
    [ZoneId]            INT            NULL,
    [BrokerId]          INT            NOT NULL,
    [IsTrackingAccount] BIT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_dbo.Accounts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Accounts_dbo.Brokers_BrokerId] FOREIGN KEY ([BrokerId]) REFERENCES [dbo].[Brokers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Accounts_dbo.Zones_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [dbo].[Zones] ([Id])
);








GO
CREATE NONCLUSTERED INDEX [IX_ZoneId]
    ON [dbo].[Accounts]([ZoneId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_BrokerId]
    ON [dbo].[Accounts]([BrokerId] ASC);

