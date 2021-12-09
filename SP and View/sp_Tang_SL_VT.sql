USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[sp_Tang_SL_VT]    Script Date: 9/12/2021 8:03:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Tang_SL_VT]
	@MAVT nvarchar(4),
	@SLTANG int
AS
BEGIN
	--Update SOLUONG
	UPDATE Vattu
		SET SOLUONGTON = SOLUONGTON + @SLTANG
		WHERE MAVT = @MAVT
END
