USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[sp_Giam_SL_VT]    Script Date: 9/12/2021 8:01:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Giam_SL_VT]
	@MAVT nvarchar(4),
	@SLGIAM int
AS
BEGIN
	--Update SOLUONG
	UPDATE Vattu
		SET SOLUONGTON = SOLUONGTON - @SLGIAM
		WHERE MAVT = @MAVT
END
