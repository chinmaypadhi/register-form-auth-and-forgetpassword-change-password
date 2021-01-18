# register-form-auth-and-forgetpassword-change-password
create database Sample
use Sample

create table tblUsers(id int identity primary key not null 
,userName varchar(200),password varchar(200),
email unique varchar(200))

//check the user  is unique or not at the time of insertion


create proc spRegisterUser
@UserName nvarchar(100),
@password_ nvarchar(200),
@email nvarchar(200)
as
begin

declare @count int
declare @returncode int

select @count=count(email) from tblUsers
 where email=@email

 if @count>0
 begin

 set @returncode=-1
 end

 else

 begin
 set @returncode=1
 insert into tblUsers values(@UserName,@password_,@email)
 end

 select @returncode as ReturnValue
 end

//check authenticate user or not

 Create Procedure spAuthenticateUser
@email nvarchar(100),
@Password_ nvarchar(100)
as
Begin
 Declare @Count int
 
 Select @Count = COUNT(UserName) from tblUsers
 where [email] = @email and [Password_] = @password_
 
 if(@Count = 1)
 Begin
  Select 1 as ReturnCode
 End
 Else
 Begin
  Select -1 as ReturnCode
 End
End



//send mail database code(reset password)

create table tblResetPasswordRequest(id uniqueIdentifier  primary key not null ,userId int foreign key references tblUsers(id),resetRequetDateTime Datetime)



create procedure SpResetPassword 
@email varchar(100)
as
begin
declare @userId int
declare @userName nvarchar(100)
select @userId=id,@userName=userName from [dbo].[tblUsers] where email=@email
if(@email is not null)
begin
declare @guid uniqueIdentifier 
set @guid=newId()
insert into tblResetPasswordRequest(id,userId,resetRequetDateTime) values(@guid,@userId,GETDATE())
 select 1 as returncode,@guid as uniqueid,@userName as userName
end
else
begin
select 0 as returncode,null as uniqueid,null as userName
end
end


//Stored Procedure to check, if the password reset link, is a valid link.


Create Proc spIsPasswordResetLinkValid 
@GUID uniqueidentifier
as
Begin
 Declare @UserId int
 
 If(Exists(Select UserId from tblResetPasswordRequests where Id = @GUID))
 Begin
  Select 1 as IsValidPasswordResetLink
 End
 Else
 Begin
  Select 0 as IsValidPasswordResetLink
 End
End
