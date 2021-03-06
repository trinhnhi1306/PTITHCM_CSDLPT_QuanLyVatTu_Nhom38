USE [QLVT]
GO
/****** Object:  StoredProcedure [dbo].[sp_HoatDongNhanVien]    Script Date: 13/12/2021 09:47:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_HoatDongNhanVien]
@MANV int,
@FROM DATE, 
@TO DATE
AS
BEGIN
	SET NOCOUNT ON;
		IF 1=0 BEGIN
			SET FMTONLY OFF
		END
	SELECT FORMAT(PN.NGAY,'MM-yyyy') AS THANGNAM,
				PN.NGAY,
				PN.MAPN AS MAPHIEU,
				'Nhập' as LOAI,
				TENVT, 
				TENKHO, 
				ChiTiet.SOLUONG, 
				ChiTiet.DONGIA into #n
		FROM (SELECT NGAY, 
					MAPN,
					TENKHO = ( SELECT TENKHO FROM Kho WHERE P.MAKHO = Kho.MAKHO )
				FROM PhieuNhap AS P
				WHERE MANV = @MANV AND NGAY BETWEEN @FROM AND @TO) PN,
				(select MAPN, SOLUONG,DONGIA, TENVT = (select TENVT from Vattu where CTPN.MAVT = Vattu.MAVT) from CTPN) ChiTiet
		WHERE PN.MAPN =ChiTiet.MAPN
		ORDER BY THANGNAM, NGAY;
	
		SELECT FORMAT(PX.NGAY,'MM-yyyy') AS THANGNAM, 
				PX.NGAY, PX.MAPX AS MAPHIEU,
				'Xuất' as LOAI,
				TENVT, 
				TENKHO, 
				CTPX.SOLUONG, 
				CTPX.DONGIA into #x
		FROM (SELECT NGAY, 
					MAPX,
					TENKHO = ( SELECT TENKHO FROM Kho WHERE P.MAKHO = Kho.MAKHO )
				FROM PhieuXuat AS P
				WHERE MANV = @MANV AND NGAY BETWEEN @FROM AND @TO )PX,
				CTPX,
				(SELECT MAVT, TENVT FROM Vattu ) VT
		WHERE PX.MAPX =CTPX.MAPX
		AND VT.MAVT = CTPX.MAVT
		ORDER BY THANGNAM, NGAY
	
	-- gộp 2 bảng lại thành 1 bảng gồm các thuộc tính giống nhau
	select px.THANGNAM,px.NGAY,px.LOAI, px.MAPHIEU , px.TENVT, px.TENKHO, px.SOLUONG,px.DONGIA
	from #x as px
	UNION
	select pn.THANGNAM,pn.NGAY,pn.LOAI,  pn.MAPHIEU , TENVT, TENKHO, pn.SOLUONG,pn.DONGIA
	from #n as pn
	
END