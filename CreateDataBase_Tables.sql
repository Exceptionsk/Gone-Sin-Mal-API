CREATE DataBase Gone_Sin_Mal

--Customer Table--
CREATE TABLE User_Table(
User_id bigint not null,
User_Name varchar(255) ,
User_Type varchar(255) ,
User_Township varchar(255) ,
User_available_coin bigint default 0,
User_visited_restaurant bigint default 0,
User_exceeded_date date,

CONSTRAINT PK_User_Table
PRIMARY KEY (User_id)
)

--Restaurant Table--
CREATE TABLE Restaurant_Table(
Rest_id bigint  IDENTITY(1,1) not null,
User_id bigint,
Rest_Name varchar(255) ,
Rest_profile_picture varbinary(MAX),
Rest_Password varchar(255) ,
Rest_Gallery_1 varbinary(MAX) ,
Rest_Gallery_2 varbinary(MAX) ,
Rest_Gallery_3 varbinary(MAX) ,
Rest_Gallery_4 varbinary(MAX) ,
Rest_Coin bigint default 0,
Rest_special_coin bigint default 0,
Rest_email varchar(255) ,
Rest_phno varchar(255) , 
Rest_myanpay_name varchar(255) ,
Rest_Township varchar(255) ,
Rest_Location varchar(255) ,
Rest_lat varchar(255) ,
Rest_long varchar(255),
Rest_created_date date,
Rest_coin_purchased bigint default 0,

CONSTRAINT PK_Restaurant_Table
PRIMARY KEY (Rest_id),

CONSTRAINT FK__Restaurant_User_id
FOREIGN KEY(User_id)
REFERENCES User_Table(User_id),
)

--System Table--
CREATE TABLE System_Table(
Record_id bigint  IDENTITY(1,1) not null,
Masterkey varchar(255) ,
Expired_coins bigint ,
Sold_coins bigint ,
Sold_special_coins bigint ,

CONSTRAINT PK_System_Table
PRIMARY KEY (Record_id)
)

--Favorite List Table--
CREATE TABLE Favorite_Table(
Fav_id bigint  IDENTITY(1,1) not null,
User_id bigint not null,
Rest_id bigint not null,

CONSTRAINT FK__Fav_Table_User_id
FOREIGN KEY(User_id)
REFERENCES User_Table(User_id),

CONSTRAINT FK_Fav_Table_Rest_id
FOREIGN KEY(Rest_id)
REFERENCES Restaurant_Table(Rest_id),

CONSTRAINT PK_Fav_Table_ComposivePK
PRIMARY KEY (Fav_id)
)

--Package Table--
CREATE TABLE Package_Table(
Package_id  bigint  IDENTITY(1,1) not null,
Package_type varchar(255) ,
Package_coin_amount bigint ,
Myanpay_button_link varchar(255) ,

CONSTRAINT PK_Package_Table
PRIMARY KEY (Package_id)
)




--Promotion Table--
CREATE TABLE Promotion_Table(
Rest_id bigint not null,
User_id bigint not null,
User_promotion_amount int default 0,

CONSTRAINT FK_Promotion_Table_User_id
FOREIGN KEY(User_id)
REFERENCES User_Table(User_id),

CONSTRAINT FK_Promotion_Table_Rest_id
FOREIGN KEY(Rest_id)
REFERENCES Restaurant_Table(Rest_id),

CONSTRAINT PK_Promotion_Table_ComposivePK
PRIMARY KEY (User_id,Rest_id)
)


--Transaction Table--
CREATE TABLE Transaction_Table(
ID bigint IDENTITY(1,1) not null,
User_id bigint not null,
Tran_id bigint,
Tran_Type varchar(255),
Pending bit,
Tran_Date date,

CONSTRAINT FK_Transaction_Table_User_id
FOREIGN KEY(User_id)
REFERENCES User_Table(User_id),

CONSTRAINT PK_Transaction_Table
PRIMARY KEY (ID)
)


--Notification Table--
CREATE TABLE Notification_Table(
Noti_id bigint  IDENTITY(1,1) not null,
User_id bigint not null,
Noti_type varchar (255) ,
Notification varchar (255) ,
Noti_status bit ,
ID bigint,


CONSTRAINT FK_Notification_Table_ID
FOREIGN KEY(ID)
REFERENCES Transaction_Table(ID),

CONSTRAINT FK_Notification_Table_User_id
FOREIGN KEY(User_id)
REFERENCES User_Table(User_id),

CONSTRAINT PK_Notification_Table
PRIMARY KEY (Noti_id)
)






select* from User_Table

delete from User_Table

select* from Restaurant_Table


delete from Restaurant_Table

INSERT INTO Notification_Table(User_id, Notification)
VALUES ('1106122069563593', 'rip gg wp');

Insert into Transaction_Table(User_id,Tran_Type,Pending)
values('1106122069563593','normal', 0)

select* from Transaction_Table

delete from Transaction_Table

delete from Notification_Table

select* from Notification_Table

select* from Package_Table

INSERT INTO Package_Table(Package_type,Package_coin_amount,Myanpay_button_link)
VALUES('normal','1000','https://www.myanpay.com.mm/Personal/ButtonDonationLogIn.aspx?sid=18ad6219-7b30-49a2-99d9-8d95c2d0cf30')
INSERT INTO Package_Table(Package_type,Package_coin_amount,Myanpay_button_link)
VALUES('normal','2000','https://www.myanpay.com.mm/Personal/ButtonDonationLogIn.aspx?sid=18ad6219-7b30-49a2-99d9-8d95c2d0cf30')
INSERT INTO Package_Table(Package_type,Package_coin_amount,Myanpay_button_link)
VALUES('normal','3000','https://www.myanpay.com.mm/Personal/ButtonDonationLogIn.aspx?sid=18ad6219-7b30-49a2-99d9-8d95c2d0cf30')
INSERT INTO Package_Table(Package_type,Package_coin_amount,Myanpay_button_link)
VALUES('normal','4000','https://www.myanpay.com.mm/Personal/ButtonDonationLogIn.aspx?sid=18ad6219-7b30-49a2-99d9-8d95c2d0cf30')
INSERT INTO Package_Table(Package_type,Package_coin_amount,Myanpay_button_link)
VALUES('normal','5000','https://www.myanpay.com.mm/Personal/ButtonDonationLogIn.aspx?sid=18ad6219-7b30-49a2-99d9-8d95c2d0cf30')

INSERT INTO Package_Table(Package_type,Package_coin_amount,Myanpay_button_link)
VALUES('special','10000','https://www.myanpay.com.mm/Personal/ButtonDonationLogIn.aspx?sid=18ad6219-7b30-49a2-99d9-8d95c2d0cf30')
INSERT INTO Package_Table(Package_type,Package_coin_amount,Myanpay_button_link)
VALUES('special','20000','https://www.myanpay.com.mm/Personal/ButtonDonationLogIn.aspx?sid=18ad6219-7b30-49a2-99d9-8d95c2d0cf30')
INSERT INTO Package_Table(Package_type,Package_coin_amount,Myanpay_button_link)
VALUES('special','30000','https://www.myanpay.com.mm/Personal/ButtonDonationLogIn.aspx?sid=18ad6219-7b30-49a2-99d9-8d95c2d0cf30')
INSERT INTO Package_Table(Package_type,Package_coin_amount,Myanpay_button_link)
VALUES('special','40000','https://www.myanpay.com.mm/Personal/ButtonDonationLogIn.aspx?sid=18ad6219-7b30-49a2-99d9-8d95c2d0cf30')
INSERT INTO Package_Table(Package_type,Package_coin_amount,Myanpay_button_link)
VALUES('special','50000','https://www.myanpay.com.mm/Personal/ButtonDonationLogIn.aspx?sid=18ad6219-7b30-49a2-99d9-8d95c2d0cf30')