USE [MilkTeaManagement]
GO

-- 1. Tìm ID của tài khoản anh Minh vừa đăng ký
DECLARE @UserId nvarchar(450)
SELECT @UserId = Id FROM AspNetUsers WHERE Email = 'vuhuyminh5@gmail.com'

-- 2. Nếu tìm thấy User thì gán vào vai trò Admin (ID: role-admin)
IF @UserId IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = 'role-admin')
    BEGIN
        INSERT INTO AspNetUserRoles (UserId, RoleId)
        VALUES (@UserId, 'role-admin')
        PRINT 'Thanh cong: Anh Minh da co quyen Admin!'
    END
    ELSE
        PRINT 'Tai khoan nay da co quyen Admin tu truoc.'
END
ELSE
    PRINT 'Loi: Khong tim thay email nay trong database. Anh hay thu dang ky lai tren web.'
GO