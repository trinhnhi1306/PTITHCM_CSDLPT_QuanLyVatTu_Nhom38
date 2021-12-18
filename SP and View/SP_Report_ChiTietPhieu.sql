USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[SP_Report_ChiTietPhieu]    Script Date: 12/18/2021 9:43:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[SP_Report_ChiTietPhieu]
@ROLE NVARCHAR(8),
@LOAI NVARCHAR(4), 
@FROM Date, 
@TO Date
AS
BEGIN


	IF (@ROLE = 'CONGTY')
	BEGIN
		IF (@LOAI = 'NHAP')
		BEGIN
			SELECT FORMAT(NGAY,'MM-yyyy') AS THANGNAM,TENVT, SOLUONG, DONGIA
			FROM 
				((SELECT MAPN, NGAY FROM PhieuNhap
				WHERE year(PhieuNhap.NGAY) * 100 + month(PhieuNhap.NGAY)  BETWEEN year(@FROM) * 100 + month(@FROM)  AND year(@TO) * 100 + month(@TO))
				UNION
				(SELECT MAPN,NGAY FROM LINK1.QLVT.dbo.PhieuNhap 
				WHERE year(PhieuNhap.NGAY) * 100 + month(PhieuNhap.NGAY)  BETWEEN year(@FROM) * 100 + month(@FROM)  AND year(@TO) * 100 + month(@TO)  )) PN,
				(SELECT MAPN,SOLUONG,DONGIA, TENVT = (SELECT TENVT FROM LINK1.QLVT.dbo.VATTU WHERE Vattu.MAVT = CTPN.MAVT) FROM LINK1.QLVT.dbo.CTPN) CT
			WHERE PN.MAPN=CT.MAPN
			ORDER BY NGAY 
		END
		ELSE
		BEGIN
			SELECT FORMAT(NGAY,'MM-yyyy') AS THANGNAM,TENVT, SOLUONG, DONGIA
			FROM 
				((SELECT MAPX,NGAY FROM PhieuXuat 
				WHERE  year(PhieuXuat.NGAY) * 100 + month(PhieuXuat.NGAY)  BETWEEN year(@FROM) * 100 + month(@FROM)  AND year(@TO) * 100 + month(@TO) ) 
				UNION
				(SELECT MAPX,NGAY FROM LINK1.QLVT.dbo.PhieuXuat 
				WHERE  year(PhieuXuat.NGAY) * 100 + month(PhieuXuat.NGAY)  BETWEEN year(@FROM) * 100 + month(@FROM)  AND year(@TO) * 100 + month(@TO)) ) PX,
				(SELECT MAPX,SOLUONG,DONGIA, TENVT = (SELECT TENVT FROM LINK1.QLVT.dbo.VATTU WHERE Vattu.MAVT = CTPX.MAVT) FROM LINK1.QLVT.dbo.CTPX)  CT
				
			WHERE PX.MAPX=CT.MAPX
			ORDER BY NGAY 
		END
	END
	ELSE
	BEGIN
		IF (@LOAI = 'NHAP')
		BEGIN
			SELECT FORMAT(NGAY,'MM-yyyy') AS THANGNAM,TENVT, SOLUONG, DONGIA
			FROM (SELECT MAPN,SOLUONG,DONGIA, TENVT = (SELECT TENVT FROM VATTU WHERE Vattu.MAVT = CTPN.MAVT) FROM CTPN) AS CT,
				(SELECT MAPN,NGAY FROM PhieuNhap 
				WHERE year(PhieuNhap.NGAY) * 100 + month(PhieuNhap.NGAY)  BETWEEN year(@FROM) * 100 + month(@FROM)  AND year(@TO) * 100 + month(@TO)) PN
			WHERE PN.MAPN=CT.MAPN
		END
		ELSE
		BEGIN
			SELECT FORMAT(NGAY,'MM-yyyy') AS THANGNAM,TENVT, SOLUONG, DONGIA
			FROM (SELECT MAPX,SOLUONG,DONGIA, TENVT = (SELECT TENVT FROM VATTU WHERE Vattu.MAVT = CTPX.MAVT) FROM CTPX) AS CT, 
			(SELECT MAPX,NGAY FROM PhieuXuat 
			WHERE year(PhieuXuat.NGAY) * 100 + month(PhieuXuat.NGAY)  BETWEEN year(@FROM) * 100 + month(@FROM)  AND year(@TO) * 100 + month(@TO)) PX
			WHERE PX.MAPX=CT.MAPX
		END
	END
END
