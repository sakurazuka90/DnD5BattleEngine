BEGIN TRANSACTION;
CREATE TABLE `WEAPONS_EQUIPEMENT_SLOTS` (
	`WEAPON_ID`	INTEGER NOT NULL,
	`EQUIPEMENT_SLOTS_ID`	INTEGER NOT NULL,
	FOREIGN KEY(`WEAPON_ID`) REFERENCES WEAPON(ID),
	FOREIGN KEY(`EQUIPEMENT_SLOTS_ID`) REFERENCES EQUIPEMENT_SLOTS(ID)
);
INSERT INTO `WEAPONS_EQUIPEMENT_SLOTS` (WEAPON_ID,EQUIPEMENT_SLOTS_ID) VALUES (1,75),
 (1,76),
 (2,75);
CREATE TABLE "WEAPONS" (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`NAME`	TEXT NOT NULL,
	`WEAPON_TYPE_ID`	INTEGER NOT NULL,
	`WEAPON_CATEGORY_ID`	INTEGER NOT NULL,
	`WEAPON_DAMAGE_DIE_SIDES`	INTEGER NOT NULL,
	`WEAPON_DAMAGE_DIE_QUANTITY`	INTEGER NOT NULL,
	`SHORT_RANGE`	INTEGER NOT NULL DEFAULT 0,
	`LONG_RANGE`	INTEGER NOT NULL DEFAULT 0,
	`ICON_NAME`	TEXT NOT NULL
);
INSERT INTO `WEAPONS` (ID,NAME,WEAPON_TYPE_ID,WEAPON_CATEGORY_ID,WEAPON_DAMAGE_DIE_SIDES,WEAPON_DAMAGE_DIE_QUANTITY,SHORT_RANGE,LONG_RANGE,ICON_NAME) VALUES (1,'Battleaxe',1,1,10,1,0,0,'Battleaxe'),
 (2,'Crossbow',2,0,10,1,6,24,'Crossbow');
CREATE TABLE "SKILLS" (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`NAME`	INTEGER NOT NULL UNIQUE,
	`ATRIBUTE_ID`	INTEGER NOT NULL,
	`DESCRIPTION`	TEXT
);
INSERT INTO `SKILLS` (ID,NAME,ATRIBUTE_ID,DESCRIPTION) VALUES (1,'Athletics',1,NULL),
 (2,'Acrobatics',2,NULL),
 (3,'Sleight of Hand',2,NULL),
 (4,'Stealth',2,NULL),
 (5,'Arcana',4,NULL),
 (6,'History',4,NULL),
 (7,'Investigation',4,NULL),
 (8,'Nature',4,NULL),
 (9,'Religion',4,NULL),
 (10,'Animal Handling',5,NULL),
 (11,'Insight',5,NULL),
 (12,'Medicine',5,NULL),
 (13,'Perception',5,NULL),
 (14,'Survival',5,NULL),
 (15,'Deception',6,NULL),
 (16,'Intimidation',6,NULL),
 (17,'Performance',6,NULL),
 (18,'Persuasion',6,NULL);
CREATE TABLE "RACES" (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`NAME`	TEXT NOT NULL,
	`SIZE`	INTEGER NOT NULL,
	`TYPE`	INTEGER NOT NULL,
	`SUBTYPE`	TEXT
);
INSERT INTO `RACES` (ID,NAME,SIZE,TYPE,SUBTYPE) VALUES (1,'Goblin',2,1,'Goblinoid'),
 (2,'Dwarf',3,1,'Dwarf');
CREATE TABLE "FIGURINES" (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`FIGURINE_NAME`	TEXT,
	`PICTURE_NAME`	TEXT,
	`CHARACTER_ID`	INTEGER NOT NULL,
	FOREIGN KEY(`CHARACTER_ID`) REFERENCES CHARACTER_STATS(ID)
);
INSERT INTO `FIGURINES` (ID,FIGURINE_NAME,PICTURE_NAME,CHARACTER_ID) VALUES (1,'Dwarf','001',2),
 (2,'goblin','002',1);
CREATE TABLE `EQUIPEMENT_SLOTS` (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`NAME`	TEXT NOT NULL
);
INSERT INTO `EQUIPEMENT_SLOTS` (ID,NAME) VALUES (1,'INV1'),
 (2,'INV2'),
 (3,'INV3'),
 (4,'INV4'),
 (5,'INV5'),
 (6,'INV6'),
 (7,'INV7'),
 (8,'INV8'),
 (9,'INV9'),
 (10,'INV10'),
 (11,'INV11'),
 (12,'INV12'),
 (13,'INV13'),
 (14,'INV14'),
 (15,'INV15'),
 (16,'INV16'),
 (17,'INV17'),
 (18,'INV18'),
 (19,'INV19'),
 (20,'INV20'),
 (21,'INV21'),
 (22,'INV22'),
 (23,'INV23'),
 (24,'INV24'),
 (25,'INV25'),
 (26,'INV26'),
 (27,'INV27'),
 (28,'INV28'),
 (29,'INV29'),
 (30,'INV30'),
 (31,'INV31'),
 (32,'INV32'),
 (33,'INV33'),
 (34,'INV34'),
 (35,'INV35'),
 (36,'INV36'),
 (37,'INV37'),
 (38,'INV38'),
 (39,'INV39'),
 (40,'INV40'),
 (41,'INV41'),
 (42,'INV42'),
 (43,'INV43'),
 (44,'INV44'),
 (45,'INV45'),
 (46,'INV46'),
 (47,'INV47'),
 (48,'INV48'),
 (49,'INV49'),
 (50,'INV50'),
 (51,'INV51'),
 (52,'INV52'),
 (53,'INV53'),
 (54,'INV54'),
 (55,'INV55'),
 (56,'INV56'),
 (57,'INV57'),
 (58,'INV58'),
 (59,'INV59'),
 (60,'INV60'),
 (61,'INV61'),
 (62,'INV62'),
 (63,'INV63'),
 (64,'INV64'),
 (65,'INV65'),
 (66,'INV66'),
 (67,'INV67'),
 (68,'INV68'),
 (69,'INV69'),
 (70,'INV70'),
 (71,'INV71'),
 (72,'INV72'),
 (73,'INV73'),
 (74,'INV74'),
 (75,'MAIN_HAND'),
 (76,'OFF_HAND'),
 (77,'ARMOR'),
 (78,'INV_OH1'),
 (79,'INV_OH2'),
 (80,'INV_OH3'),
 (81,'INV_OH4'),
 (82,'INV_OH5'),
 (83,'INV_MH1'),
 (84,'INV_MH2'),
 (85,'INV_MH3'),
 (86,'INV_MH4'),
 (87,'INV_MH5'),
 (88,'INV_ARMOR');
CREATE TABLE "CHARACTER_STATS" (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`NAME`	TEXT NOT NULL,
	`LEVEL`	INTEGER,
	`HP`	INTEGER,
	`SPEED`	INTEGER,
	`AI`	NUMERIC,
	`EQUIPPED_WEAPON_SLOT`	INTEGER,
	`RACE_ID`	INTEGER,
	`PLAYER_NAME`	TEXT
);
INSERT INTO `CHARACTER_STATS` (ID,NAME,LEVEL,HP,SPEED,AI,EQUIPPED_WEAPON_SLOT,RACE_ID,PLAYER_NAME) VALUES (1,'Goblin',1,7,4,1,83,1,NULL),
 (2,'Kurrdar the Mighty',1,22,4,0,83,2,NULL);
CREATE TABLE `CHARACTER_PROFICIENT_SKILLS` (
	`CHARACTER_STATS_ID`	INTEGER NOT NULL,
	`SKILLS_ID`	INTEGER NOT NULL
);
INSERT INTO `CHARACTER_PROFICIENT_SKILLS` (CHARACTER_STATS_ID,SKILLS_ID) VALUES (1,4);
CREATE TABLE `CHARACTER_PROFICIENT_SAVING_THROWS` (
	`CHARACTER_ID`	INTEGER NOT NULL,
	`ABILITY_ID`	INTEGER NOT NULL
);
INSERT INTO `CHARACTER_PROFICIENT_SAVING_THROWS` (CHARACTER_ID,ABILITY_ID) VALUES (2,3);
CREATE TABLE "CHARACTER_ABILITY_VALUES" (
	`ABILITY_ID`	INTEGER NOT NULL,
	`CHARACTER_ID`	INTEGER NOT NULL,
	`VALUE`	INTEGER NOT NULL
);
INSERT INTO `CHARACTER_ABILITY_VALUES` (ABILITY_ID,CHARACTER_ID,VALUE) VALUES (1,1,8),
 (2,1,14),
 (3,1,10),
 (4,1,10),
 (5,1,8),
 (6,1,8),
 (1,2,16),
 (2,2,10),
 (3,2,20),
 (4,2,12),
 (5,2,10),
 (6,2,8);
CREATE TABLE "CHARACTERS_ITEMS" (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`CHARACTER_ID`	INTEGER NOT NULL,
	`FIELD_ID`	INTEGER NOT NULL,
	`ITEM_ID`	INTEGER NOT NULL,
	`ITEM_TYPE`	INTEGER NOT NULL,
	FOREIGN KEY(`CHARACTER_ID`) REFERENCES `CHARACTER_STATS`(`ID`),
	FOREIGN KEY(`ITEM_ID`) REFERENCES `EQUIPEMENT_SLOTS`(`ID`)
);
INSERT INTO `CHARACTERS_ITEMS` (ID,CHARACTER_ID,FIELD_ID,ITEM_ID,ITEM_TYPE) VALUES (1,2,83,1,1),
 (2,2,88,1,2),
 (3,2,84,2,1),
 (4,1,83,1,1);
CREATE TABLE `ARMORS_EQUIPEMENT_SLOTS` (
	`ARMORS_ID`	INTEGER NOT NULL,
	`EQUIPEMENT_SLOTS`	INTEGER NOT NULL,
	FOREIGN KEY(`ARMORS_ID`) REFERENCES ARMORS(ID),
	FOREIGN KEY(`EQUIPEMENT_SLOTS`) REFERENCES EQUIPEMENT_SLOTS(ID)
);
INSERT INTO `ARMORS_EQUIPEMENT_SLOTS` (ARMORS_ID,EQUIPEMENT_SLOTS) VALUES (1,77);
CREATE TABLE "ARMORS" (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`NAME`	TEXT NOT NULL,
	`AC`	INTEGER NOT NULL,
	`MAX_DEX`	INTEGER NOT NULL,
	`ICON_NAME`	TEXT NOT NULL
);
INSERT INTO `ARMORS` (ID,NAME,AC,MAX_DEX,ICON_NAME) VALUES (1,'Breastplate',16,0,'ArmorColor');
CREATE TABLE `ABILITIES` (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`NAME`	TEXT NOT NULL UNIQUE,
	`DESCRIPTION`	TEXT
);
INSERT INTO `ABILITIES` (ID,NAME,DESCRIPTION) VALUES (1,'Strength','Strength measures bodily power, athletic training, and the extent to which you can exert raw physical force.'),
 (2,'Dexterity','Dexterity measures agility, reflexes, and balance.'),
 (3,'Constitution','Dexterity measures agility, reflexes, and balance.'),
 (4,'Intelligence','Intelligence measures mental acuity, accuracy of recall, and the ability to reason.'),
 (5,'Wisdom','Wisdom reflects how attuned you are to the world around you and represents perceptiveness and intuition.'),
 (6,'Charisma','Charisma measures your ability to interact effectively with others. It includes such factors as confidence and eloquence, and it can represent a charming or commanding personality.');
COMMIT;
