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
 (2,'Dwarf',3,1,'Dwarf'),
 (3,'Elf',3,1,'Elf'),
 (4,'Human',3,1,'Human'),
 (5,'Half-elf',3,1,'Half-elf'),
 (6,'Half-orc',3,1,'Half-orc'),
 (7,'Tiefling',3,1,'Tiefling'),
 (8,'Dragonborn',3,1,'Dragonborn'),
 (9,'Halfling',2,1,'Halfling'),
 (10,'Gnome',2,1,'Gnome');
CREATE TABLE "FIGURINES" (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`FIGURINE_NAME`	TEXT,
	`PICTURE_NAME`	TEXT,
	`CHARACTER_ID`	INTEGER NOT NULL,
	FOREIGN KEY(`CHARACTER_ID`) REFERENCES CHARACTER_STATS(ID)
);
INSERT INTO `FIGURINES` (ID,FIGURINE_NAME,PICTURE_NAME,CHARACTER_ID) VALUES (1,'Dwarf','001',2),
 (2,'goblin','002',1),
 (4,'Dwarf','002',14);
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
 (2,'Kurrdar the Mighty',1,22,4,0,83,2,NULL),
 (14,'Dubbi',NULL,NULL,NULL,NULL,NULL,NULL,'Bibi');
CREATE TABLE `CHARACTER_RACE` (
	`ID`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	`RACE_ID`	INTEGER NOT NULL,
	`DESCRIPTION`	TEXT
);
INSERT INTO `CHARACTER_RACE` (ID,RACE_ID,DESCRIPTION) VALUES (1,2,'Dwarves are a stoic but stern race, ensconced in cities carved from the hearts of mountains and fiercely determined to repel the depredations of savage races like orcs and goblins. More than any other race, dwarves have acquired a reputation as dour and humorless artisans of the earth. It could be said that their history shapes the dark disposition of many dwarves, for they reside in high mountains and dangerous realms below the earth, constantly at war with giants, goblins, and other such horrors.'),
 (2,3,'The long-lived elves are children of the natural world, similar in many superficial ways to fey creatures, though with key differences. While fey are truly linked to the flora and fauna of their homes, existing as the nearly immortal voices and guardians of the wilderness, elves are instead mortals who are in tune with the natural world around them. Elves seek to live in balance with the wild and understand it better than most other mortals. Some of this understanding is mystical, but an equal part comes from the elves'' long lifespans, which in turn gives them long-ranging outlooks. Elves can expect to remain active in the same locale for centuries. By necessity, they must learn to maintain sustainable lifestyles, and this is most easily done when they work with nature, rather than attempting to bend it to their will. However, their links to nature are not entirely driven by pragmatism. Elves'' bodies slowly change over time, taking on a physical representation of their mental and spiritual states, and those who dwell in a region for a long period of time find themselves physically adapting to match their surroundings, most noticeably taking on coloration that reflects the local environment.'),
 (3,4,'Humans possess exceptional drive and a great capacity to endure and expand, and as such are currently the dominant race in the world. Their empires and nations are vast, sprawling things, and the citizens of these societies carve names for themselves with the strength of their sword arms and the power of their spells. Humanity is best characterized by its tumultuousness and diversity, and human cultures run the gamut from savage but honorable tribes to decadent, devil-worshiping noble families in the most cosmopolitan cities. Humans'' curiosity and ambition often triumph over their predilection for a sedentary lifestyle, and many leave their homes to explore the innumerable forgotten corners of the world or lead mighty armies to conquer their neighbors, simply because they can.

Human society is a strange amalgam of nostalgia and futurism, being enamored of past glories and wistfully remembered “golden ages,” yet at the same time quick to discard tradition and history and strike off into new ventures. Relics of the past are kept as prized antiques and museum pieces, as humans love to collect things—not only inanimate relics but also living creatures—to display for their amusement or to serve by their side. Other races suggest this behavior is due to a deep-rooted urge to dominate and assert power in the human psyche, an urge to take, till, or tame the wild things and places of the world. Those with a more charitable view believe humans are simply collectors of experiences, and the things they take and keep, whether living, dead, or never alive, are just tokens to remind themselves of the places they have gone, the things they have seen, and the deeds they have accomplished. Their present and future value is just a bonus; their real value is as an ongoing reminder of the inevitable progress of humanity.'),
 (5,5,'Elves have long drawn the covetous gazes of other races. Their generous lifespans, magical affinity, and inherent grace each contribute to the admiration or bitter envy of their neighbors. Of all their traits, however, none so entrance their human associates as their beauty. Since the two races first came into contact with each other, humans have held up elves as models of physical perfection, seeing in these fair folk idealized versions of themselves. For their part, many elves find humans attractive despite their comparatively barbaric ways, and are drawn to the passion and impetuosity with which members of the younger race play out their brief lives.

Sometimes this mutual infatuation leads to romantic relationships. Though usually short-lived, even by human standards, such trysts may lead to the birth of half-elves, a race descended from two cultures yet inheritor of neither. Half-elves can breed with one another, but even these “pureblood” half-elves tend to be viewed as bastards by humans and elves alike. Caught between destiny and derision, half-elves often view themselves as the middle children of the world.

'),
 (6,6,'As seen by civilized races, half-orcs are monstrosities, the result of perversion and violence—whether or not this is actually true. Half-orcs are rarely the result of loving unions, and as such are usually forced to grow up hard and fast, constantly fighting for protection or to make names for themselves. Half-orcs as a whole resent this treatment, and rather than play the part of the victim, they tend to lash out, unknowingly confirming the biases of those around them. A few feared, distrusted, and spat-upon half-orcs manage to surprise their detractors with great deeds and unexpected wisdom—though sometimes it''s easier just to crack a few skulls. Some half-orcs spend their entire lives proving to full-blooded orcs that they are just as fierce. Others opt for trying to blend into human society, constantly demonstrating that they aren''t monsters. Their need to always prove themselves worthy encourages half-orcs to strive for power and greatness within the society around them.

'),
 (7,8,'Your draconic heritage manifests in a variety of traits you share with other dragonborn.'),
 (8,7,'Simultaneously more and less than mortal, tieflings are the offspring of humans and fiends. With otherworldly blood and traits to match, tieflings are often shunned and despised out of reactionary fear. Most tieflings never know their fiendish sire, as the coupling that produced their curse occurred generations earlier. The taint is long-lasting and persistent, often manifesting at birth or sometimes later in life, as a powerful, though often unwanted, boon. Despite their fiendish appearance and netherworld origins, tieflings have a human''s capacity of choosing their fate, and while many embrace their dark heritage and side with fiendish powers, others reject their darker predilections. Though the power of their blood calls nearly every tiefling to fury, destruction, and wrath, even the spawn of a succubus can become a saint and the grandchild of a pit fiend an unsuspecting hero.'),
 (9,9,'Optimistic and cheerful by nature, blessed with uncanny luck, and driven by a powerful wanderlust, halflings make up for their short stature with an abundance of bravado and curiosity. At once excitable and easy-going, halflings like to keep an even temper and a steady eye on opportunity, and are not as prone to violent or emotional outbursts as some of the more volatile races. Even in the jaws of catastrophe, halflings almost never lose their sense of humor. Their ability to find humor in the absurd, no matter how dire the situation, often allows halflings to distance themselves ever so slightly from the dangers that surround them. This sense of detachment can also help shield them from terrors that might immobilize their allies.

Halflings are inveterate opportunists. They firmly believe they can turn any situation to their advantage, and sometimes gleefully leap into trouble without any solid plan to extricate themselves if things go awry. Often unable to physically defend themselves from the rigors of the world, they know when to bend with the wind and when to hide away. Yet halflings'' curiosity often overwhelms their good sense, leading to poor decisions and narrow escapes. While harsh experience sometimes teaches halflings a measure of caution, it rarely makes them completely lose faith in their luck or stop believing that the universe, in some strange way, exists for their entertainment and would never really allow them to come to harm. Though their curiosity drives them to seek out new places and experiences, halflings possess a strong sense of hearth and home, often spending above their means to enhance the comforts of domestic life. Without a doubt, halflings enjoy luxury and comfort, but they have equally strong reasons to make their homes a showcase. Halflings consider this urge to devote time, money, and energy toward improving their dwellings a sign of both respect for strangers and affection for their loved ones. Whether for their own blood kin, cherished friends, or honored guests, halflings make their homes beautiful in order to express their feelings toward those they welcome inside. Even traveling halflings typically decorate their wagons or carry a few cherished keepsakes to adorn their campsites.'),
 (10,10,'Gnomes are distant relatives of the fey, and their history tells of a time when they lived in the fey''s mysterious realm, a place where colors are brighter, the wildlands wilder, and emotions more primal. Unknown forces drove the ancient gnomes from that realm long ago, forcing them to seek refuge in this world; despite this, the gnomes have never completely abandoned their fey roots or adapted to mortal culture. Though gnomes are no longer truly fey, their fey heritage can be seen in their innate magic powers, their oft-capricious natures, and their outlooks on life and the world.

Gnomes can have the same concerns and motivations as members of other races, but just as often they are driven by passions and desires that non-gnomes see as eccentric at best, and nonsensical at worst. A gnome may risk his life to taste the food at a giant''s table, to reach the bottom of a pit just because it would be the lowest place he''s ever been, or to tell jokes to a dragon—and to the gnome those goals are as worthy as researching a new spell, gaining vast wealth, or putting down a powerful evil force. While such apparently fickle and impulsive acts are not universal among gnomes, they are common enough for the race as a whole to have earned a reputation for being impetuous and at least a little mad.

Combined with their diminutive sizes, vibrant coloration, and lack of concern for the opinions of others, these attitudes have caused gnomes to be widely regarded by the other races as alien and strange. Gnomes, in turn, are often amazed how alike other common, civilized races are. It seems stranger to a gnome that humans and elves share so many similarities than that the gnomes do not. Indeed, gnomes often confound their allies by treating everyone who is not a gnome as part of a single, vast non-gnome collective race.');
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
