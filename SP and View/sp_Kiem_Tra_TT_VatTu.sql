USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[sp_Kiem_Tra_TT_VatTu]    Script Date: 23/10/2021 10:22:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[sp_Kiem_Tra_TT_VatTu]
@MAVT nchar(4)
AS
BEGIN
	 IF EXISTS(SELECT 1
				   FROM (SELECT MAVT FROM Vattu) VT  
				   WHERE VT.MAVT = @MAVT AND (
				   EXISTS(SELECT 1 FROM LINK1.QLVT.dbo.CTPN PN WHERE PN.MAVT = VT.MAVT) 
				   OR EXISTS(SELECT MAPX FROM LINK1.QLVT.dbo.CTPX PX WHERE PX.MAVT = VT.MAVT) 
						OR EXISTS(SELECT MasoDDH FROM LINK1.QLVT.dbo.CTDDH DH WHERE DH.MAVT = VT.MAVT)))
			RETURN 1; -- Mã Vattu đang dùng ở phân mảnh khác
	RETURN 0; -- Không dùng
END