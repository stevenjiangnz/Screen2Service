CREATE TABLE [dbo].[OrderReviews] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Content] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.OrderReviews] PRIMARY KEY CLUSTERED ([Id] ASC)
);

