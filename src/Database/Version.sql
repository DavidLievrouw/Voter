﻿CREATE TABLE [dbo].[Version] (
  [Number] SMALLINT NOT NULL PRIMARY KEY, 
  [Creation] DATETIME2(0) NOT NULL DEFAULT now()
)
