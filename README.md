# register-form-auth-and-forgetpassword-change-password
create database Sample
use Sample
------------------------------------
create table tblUsers(id int identity primary key not null ,email nvarchar(100) unique
,userName varchar(200),password varchar(200), [RetryAttempts] int,
 [IsLocked] bit,
 [LockedDateTime] datetime)
email unique varchar(200))
------------------------------------
//check the user  is unique or not at the time of registration and also perform insertion 

 alter proc spRegisterUser
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
 insert into tblUsers(userName,password_,email) values(@UserName,@password_,@email)
 end

 select @returncode as ReturnValue
 end
 ..................................................................
 //command to check the stored procedure sp_helptext spRegisterUser
 ..................................................................

//check authenticate user or not
alter Procedure spAuthenticateUser1
@email nvarchar(100)
as
Begin
 Declare @Count int
 
 Select @Count = COUNT(email) from tblUsers
 where [email] = @email 
 if(@Count = 1)
 Begin
  Select 1 as ReturnCode
 End
 Else
 Begin
  Select -1 as ReturnCode
 End
End
...........................................

CHECK A USER HOWMANY TIME TRY TO LOGGED IN   (IF IT >3) THEN BLOCK THE USER FOR 20 MINUTE
.............................................................................................


 Alter proc spAuthenticateUser
@email nvarchar(100),
@Password nvarchar(200)
as
Begin
 Declare @AccountLocked bit
 Declare @Count int
 Declare @RetryCount int
 
 Select @AccountLocked = IsLocked
 from tblUsers where email = @email
  
 --If the account is already locked
 if(@AccountLocked = 1)
 Begin
  Select 1 as AccountLocked, 0 as Authenticated, 0 as RetryAttempts
 End
 Else
 Begin
  -- Check if the username and password match
  Select @Count = COUNT(UserName) from tblUsers
  where email = @email and [Password_] = @Password
  
  -- If match found
  if(@Count = 1)
  Begin
   -- Reset RetryAttempts 
   Update tblUsers set RetryAttempts = 0
   where email = @email
       
   Select 0 as AccountLocked, 1 as Authenticated, 0 as RetryAttempts
  End
  Else
  Begin
   -- If a match is not found
   Select @RetryCount = IsNULL(RetryAttempts, 0)
   from tblUsers
   where email = @email
   
   Set @RetryCount = @RetryCount + 1
   
   if(@RetryCount <= 3)
   Begin
    -- If re-try attempts are not completed
    Update tblUsers set RetryAttempts = @RetryCount
    where email = @email 
    
    Select 0 as AccountLocked, 0 as Authenticated, @RetryCount as RetryAttempts
   End
   Else
   Begin
    -- If re-try attempts are completed
    Update tblUsers set RetryAttempts = @RetryCount,
    IsLocked = 1, LockedDateTime = GETDATE()
    where email = @email

    Select 1 as AccountLocked, 0 as Authenticated, 0 as RetryAttempts
   End
  End
 End
End
.....................................
ACCOUNT REMOVE AUTOMATICALY IN 24 MINUTE
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Let us now, schedule this update query to run every 5 minutes, every day. This can be very easily done using sql server agent jobs. In this video, we will discuss about creating and scheduling sql server agent jobs, for sql server 2008.
1. Open sql serevr management studio
2. In the object explorer, check if "SQL Server Agent" is running.
3. If "SQL Server Agent" is not running, right click and select "Start".
4. Click on the "+" sign, next to "SQL Server Agent" to expand.
5. Right click on "Jobs" folder and select "New Job".
6. In the "New Job" dialog box, provide a meaningful name. Let us call it, "Unlock user accounts job".
7. Fill in Owner, Category and Description fields accordingly. Make sure the Enabled checkbox is selected.
8. Select "Steps" tab, and click "New" button
9. In the "New Job Step" dialog box, give a meaningful step name. Let us call it "Execute Update Query"
10. Select Transact-SQL Script as "Type"
11. Select the respective Database.
12. In the "Command" text box, copy and paste the UPDATE query, and click OK
13. In the "New Job" dialog box, select "Schedules" and click "New" button
14. In the "New Job Schedule" dialog box, give a meaningful name to the schedule. Let us call it "Run Every 5 Minutes Daily"
15. Choose "Recurring" as "Schedule type"
16. Under "Frequency", set "Occurs" = "Daily" and "Recurs every" = "1" Days.
17. Under "Daily Frequency", set "Occurs every" = "5" Minutes.
18. Finally fill in the schedule start and end dates, under "Duration"
19. Click OK, twice and you are done.

This job, will run every 5 minutes daily, and unlocks the accounts that has been locked for more than 24 hours.
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

---------------------------------------------------

//send mail database code(reset password)

create table tblResetPasswordRequest(id uniqueIdentifier  primary key not null ,userId int foreign key references tblUsers(id),resetRequetDateTime Datetime)

----------------------------------------------

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

----------------------------------
//Stored Procedure to check, if the password reset link, is a valid link.


alter Proc spIsPasswordResetLinkValid 
@GUID uniqueidentifier
as
Begin
 Declare @UserId int
 
 If(Exists(Select UserId from tblResetPasswordRequest where Id = @GUID))
 Begin
  Select 1 as IsValidPasswordResetLink
 End
 Else
 Begin
  Select 0 as IsValidPasswordResetLink
 End
End

---------------------------------
//procedure to change the password 

alter Proc spChangePassword
@GUID uniqueidentifier,
@Password nvarchar(100)
as
Begin
 Declare @UserId int
 
 Select @UserId = userId 
 from tblResetPasswordRequest
 where id= @GUID
 
 if(@UserId is null)
 Begin
  -- If UserId does not exist
  Select 0 as IsPasswordChanged
 End
 Else
 Begin
  -- If UserId exists, Update with new password
  Update tblUsers set
  [Password_] = @Password
  where id = @UserId
  
  -- Delete the password reset request row 
  Delete from tblResetPasswordRequest
  where id = @GUID
  
  Select 1 as IsPasswordChanged
 End
End
-----------

//procedure to change the password with login

Create Proc spChangePasswordUsingCurrentPassword
@email nvarchar(100),
@CurrentPassword nvarchar(100),
@NewPassword nvarchar(100)
as
Begin
 if(Exists(Select Id from tblUsers 
     where email  = @email 
     and password_= @CurrentPassword))
 Begin
  Update tblUsers
  Set password_ = @NewPassword
  where email = @email
  
  Select 1 as IsPasswordChanged
 End
 Else
 Begin
  Select 0 as IsPasswordChanged
 End
End

