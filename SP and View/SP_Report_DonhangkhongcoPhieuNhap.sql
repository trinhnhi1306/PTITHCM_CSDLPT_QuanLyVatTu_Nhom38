USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[SP_Report_DonhangkhongcoPhieuNhap]    Script Date: 13/12/2021 09:52:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[SP_Report_DonhangkhongcoPhieuNhap]
AS
SELECT DH.MasoDDH,Ngay,NhaCC,HOTENNV, TENVT, SOLUONG, DONGIA 
FROM
	(SELECT MasoDDH,Ngay,NhaCC,HOTENNV = (select HOTEN = HO +' ' +Ten from dbo.NhanVien where NhanVien.MANV = DatHang.MANV) FROM dbo.DatHang
	WHERE DatHang.MasoDDH not in (
	SELECT MasoDDH FROM dbo.PhieuNhap)) DH,
	(SELECT MAVT,TENVT FROM dbo.Vattu) VT,
	CTDDH CT
WHERE (VT.MAVT =CT.MAVT) AND (CT.MasoDDH = DH.MasoDDH)
