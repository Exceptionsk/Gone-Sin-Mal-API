CREATE DataBase Gone_Sin_Mal

--Customer Table--
CREATE TABLE User_Table(
User_id bigint NOT NULL,
User_Name varchar(255) NOT NULL,
User_Type varbinary(MAX) NOT NULL,
User_Township varchar(255) NOT NULL,
User_available_coin bigint NOT NULL,
User_visited_restaurant bigint NOT NULL,
User_exceeded_date date,

CONSTRAINT PK_User_Table
PRIMARY KEY (User_id)
)

--Restaurant Table--
CREATE TABLE Restaurant_Table(
Rest_id bigint NOT NULL IDENTITY(1,1),
Rest_Name varchar(255) NOT NULL,
Rest_Password varchar(255) NOT NULL,
Rest_Gallery_1 varbinary(MAX) NOT NULL,
Rest_Gallery_2 varbinary(MAX) NOT NULL,
Rest_Gallery_3 varbinary(MAX) NOT NULL,
Rest_Gallery_4 varbinary(MAX) NOT NULL,
Rest_Coin bigint,
Rest_special_coin bigint,
Rest_email varchar(255) NOT NULL,
Rest_phno varchar(255) NOT NULL, 
Rest_myanpay_name varchar(255) NOT NULL,
Rest_Township varchar(255) NOT NULL,
Rest_Location varchar(255) NOT NULL,
Rest_lat varchar(255) NOT NULL,
Rest_long varchar(255) NOT NULL,

CONSTRAINT PK_Restaurant_Table
PRIMARY KEY (Rest_id)
)

--System Table--
CREATE TABLE System_Table(
Record_id bigint NOT NULL IDENTITY(1,1),
Masterkey varchar(255) NOT NULL,
Expired_coins bigint NOT NULL,
Sold_coins bigint NOT NULL,
Sold_special_coins bigint NOT NULL,

CONSTRAINT PK_System_Table
PRIMARY KEY (Record_id)
)

--Favorite List Table--
CREATE TABLE Favorite_Table(
User_id bigint NOT NULL,
Rest_id bigint NOT NULL,

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
Package_id  bigint NOT NULL IDENTITY(1,1),
Package_type varchar(255) NOT NULL,
Package_coin_amount bigint NOT NULL,
Myanpay_button_link varchar(255) NOT NULL,

CONSTRAINT PK_Package_Table
PRIMARY KEY (Package_id)
)

--Promotion Table--
CREATE TABLE Promotion_Table(
Rest_id bigint NOT NULL,
User_id bigint NOT NULL,
User_promotion_amount int NOT NULL,

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
Noti_id bigint NOT NULL IDENTITY(1,1),
User_id bigint NOT NULL,
Noti_type varchar (255) NOT NULL,
Notification varchar (255) NOT NULL,
Noti_status bit NOT NULL,

CONSTRAINT FK_Notification_Table_User_id
FOREIGN KEY(User_id)
REFERENCES User_Table(User_id),

CONSTRAINT PK_Notification_Table
PRIMARY KEY (Noti_id)
)
