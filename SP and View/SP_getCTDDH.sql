USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[SP_getCTDDH]    Script Date: 13/11/2021 08:56:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_getCTDDH]
@MADDH nchar(10)
AS
BEGIN
	SELECT ct.MasoDDH,
	ct.MAVT,
	(SELECT Vattu.TENVT from Vattu where Vattu.MAVT = ct.MAVT) as TENVT,
	ct.SOLUONG,
	ct.DONGIA
	FROM CTDDH ct
	WHERE ct.MAVT not in (select chitietPN.MAVT from (select MasoDDH,MAPN from PhieuNhap) PN,
							(select CTPN.MAPN,CTPN.MAVT from CTPN) chitietPN
							where PN.MasoDDH = @MADDH
							and PN.MAPN = chitietPN.MAPN)
	
	and ct.MasoDDH = @MADDH
END