create table [Order](
	[Id] int IDENTITY PRIMARY KEY,
	[UniqueId] uniqueidentifier unique NOT NULL,
	[CustomerFirstName] varchar(MAX) NOT NULL,
	[CustomerLastName] varchar(MAX) NOT NULL,
	[Location] int NOT NULL,
	[Status] int NOT NULL,
	[Total] decimal(18, 4) NOT NULL,
	[isDeleted] BIT default 0 NOT NULL,
);

create table [Item](
	[Id] int PRIMARY KEY IDENTITY,
	[UniqueId] uniqueidentifier unique NOT NULL,
	[Name] varchar(MAX) NOT NULL,
	[Description] varchar(MAX) NOT NULL,
	[ImageURL] varchar(MAX) NOT NULL,
	[Price] decimal(18,4) NOT NULL,
	[Milk] int NOT NULL,
	[Size] int NOT NULL,
	[Quantity] int NOT NULL,
	[isDeleted] BIT default 0 NOT NULL
);

create table [Line](
	[Id] int PRIMARY KEY IDENTITY,
	[OrderUniqueId] uniqueidentifier NOT NULL,
	[ItemId] int REFERENCES [Item]([Id]) NOT NULL,
	[LineQty] int NOT NULL,
	[LinePrice] decimal(18, 4) NOT NULL,
	[isDeleted] BIT default 0 NOT NULL
);