CREATE TABLE [eventstore].[EventStream] (
  [RowNumber] [bigint] NOT NULL IDENTITY(-9223372036854775808, 1),
	[StreamId] [uniqueidentifier] NOT NULL,
	[StreamVersion] [bigint] NOT NULL,
	[StreamContract] [nvarchar](512) NOT NULL,
	[EventDataContract] [nvarchar](512) NOT NULL,
	[EventData] [nvarchar](max) NOT NULL,
	[UtcStoreTime] [datetimeoffset](7) NOT NULL, 
  CONSTRAINT [PK_EventStream] PRIMARY KEY ([RowNumber])
)

GO

CREATE INDEX [IX_EventStream_StreamContract] ON [eventstore].[EventStream] ([StreamContract])
