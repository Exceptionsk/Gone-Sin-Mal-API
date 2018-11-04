CREATE DataBase Gone_Sin_Mal

--Customer Table--
CREATE TABLE User_Table(
User_id bigint not null,
User_Name varchar(255) ,
User_Type varchar(255) ,
User_Township varchar(255) ,
User_available_coin bigint ,
User_visited_restaurant bigint ,
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
Rest_Coin bigint,
Rest_special_coin bigint,
Rest_email varchar(255) ,
Rest_phno varchar(255) , 
Rest_myanpay_name varchar(255) ,
Rest_Township varchar(255) ,
Rest_Location varchar(255) ,
Rest_lat varchar(255) ,
Rest_long varchar(255) ,

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
User_id bigint not null,
Rest_id bigint not null,

CONSTRAINT FK__Fav_Table_User_id
FOREIGN KEY(User_id)
REFERENCES User_Table(User_id),

CONSTRAINT FK_Fav_Table_Rest_id
FOREIGN KEY(Rest_id)
REFERENCES Restaurant_Table(Rest_id),

CONSTRAINT PK_Fav_Table_ComposivePK
PRIMARY KEY (User_id,Rest_id)
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
User_promotion_amount int ,

CONSTRAINT FK_Promotion_Table_User_id
FOREIGN KEY(User_id)
REFERENCES User_Table(User_id),

CONSTRAINT FK_Promotion_Table_Rest_id
FOREIGN KEY(Rest_id)
REFERENCES Restaurant_Table(Rest_id),

CONSTRAINT PK_Promotion_Table_ComposivePK
PRIMARY KEY (User_id,Rest_id)
)

--Notification Table--
CREATE TABLE Notification_Table(
Noti_id bigint  IDENTITY(1,1) not null,
User_id bigint not null,
Noti_type varchar (255) ,
Notification varchar (255) ,
Noti_status bit ,

CONSTRAINT FK_Notification_Table_User_id
FOREIGN KEY(User_id)
REFERENCES User_Table(User_id),

CONSTRAINT PK_Notification_Table
PRIMARY KEY (Noti_id)
)


select* from User_Table

delete from User_Table

select* from Restaurant_Table