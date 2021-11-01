USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[SP_Report_DS_VatTu]    Script Date: 31/10/2021 11:43:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[SP_Report_DS_VatTu]
AS
	SELECT MAVT, TENVT, DVT, SOLUONGTON 
	FROM dbo.Vattu 
	ORDER BY TENVT