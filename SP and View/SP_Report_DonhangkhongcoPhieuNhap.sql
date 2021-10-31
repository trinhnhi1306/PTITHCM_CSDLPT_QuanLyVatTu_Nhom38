USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[SP_Report_DonhangkhongcoPhieuNhap]    Script Date: 31/10/2021 11:43:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[SP_Report_DonhangkhongcoPhieuNhap]
AS
SELECT DH.MasoDDH,Ngay,NhaCC,HOTENNV, TENVT, SOLUONG, DONGIA 
FROM
	(SELECT MasoDDH,Ngay,NhaCC, MANV FROM dbo.DatHang
	WHERE DatHang.MasoDDH not in (
	SELECT MasoDDH FROM dbo.PhieuNhap)) DH,
	(SELECT MANV,HOTENNV = HO +' ' +Ten FROM dbo.NhanVien) NV,
	(SELECT MAVT,TENVT FROM dbo.Vattu) VT,
	CTDDH CT
WHERE (NV.MANV = DH.MANV) AND (VT.MAVT =CT.MAVT) AND (CT.MasoDDH = DH.MasoDDH)
