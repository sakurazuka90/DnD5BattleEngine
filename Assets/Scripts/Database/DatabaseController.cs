using Mono.Data.Sqlite;
using UnityEngine;
using System;
using System.Data;
using System.Collections.Generic;

public class DatabaseController{

	private static string _connectionURI = "URI=file:" + Application.dataPath + "/Database/battleEngine.db";

	public static IDbConnection GetConnection()
	{
		IDbConnection dbconn;
		dbconn = (IDbConnection) new SqliteConnection(_connectionURI);
		dbconn.Open();

		return dbconn;
	}

	public static void CleanUp(IDataReader pmReader, IDbCommand pmCommand, IDbConnection pmConnection)
	{
		if (pmReader != null)
			pmReader.Close ();
		if (pmCommand != null)
			pmCommand.Dispose ();
		if (pmConnection != null)
			pmConnection.Close ();
	}

	public static Dictionary<int,string> GetListOfValues()
	{
		Dictionary<int,string> names = new Dictionary<int,string> ();

		IDbConnection dbconn = GetConnection ();
		IDbCommand dbcmd = dbconn.CreateCommand();
		string sqlQuery = "SELECT NAME, ID " + "FROM CHARACTER_STATS";
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();
		while (reader.Read())
		{
			string name = reader.GetString (0);
			int id = reader.GetInt32 (1);
			names.Add (id,name);
		}

		CleanUp (reader,dbcmd,dbconn);
		reader = null;
		dbcmd = null;
		dbconn = null;

		return names;
	}

	public static Player GetPlayerByID(int pmPlayerId)
	{
		Player lvPlayer = new Player ();

		IDbConnection dbconn = GetConnection ();
		IDbCommand dbcmd = dbconn.CreateCommand();
		string sqlQuery = 	"SELECT ST.NAME, FI.PICTURE_NAME, ST.LEVEL, ST.STR, ST.DEX, ST.FOR, ST.INT, ST.WIS, ST.CHA, ST.HP, ST.SPEED, FI.FIGURINE_NAME, ST.AI, ST.EQUIPPED_WEAPON_SLOT "
			+ "FROM CHARACTER_STATS ST JOIN FIGURINES FI ON FI.CHARACTER_ID = ST.ID WHERE ST.ID = " + pmPlayerId;
		dbcmd.CommandText = sqlQuery;

		IDataReader reader = dbcmd.ExecuteReader();
		if (reader.Read())
		{

			lvPlayer.playerName = reader.GetString (0);
			Sprite spr1 = Resources.Load<Sprite>(reader.GetString(1));
			lvPlayer.PlayerSprite = spr1;

			lvPlayer.SetAbility (AbilityNames.STRENGTH, reader.GetInt32(3));
			lvPlayer.SetAbility (AbilityNames.DEXTERITY, reader.GetInt32(4));
			lvPlayer.SetAbility (AbilityNames.CONSTITUTION, reader.GetInt32(5));
			lvPlayer.SetAbility (AbilityNames.INTELLIGENCE, reader.GetInt32(6));
			lvPlayer.SetAbility (AbilityNames.WISDOM, reader.GetInt32(7));
			lvPlayer.SetAbility (AbilityNames.CHARISMA, reader.GetInt32(8));
			lvPlayer.HpTotal = reader.GetInt32 (9);
			lvPlayer.hp = lvPlayer.HpTotal;
			lvPlayer.SetSpeed (reader.GetInt32(10));
			lvPlayer.Proficiency = Proficiency.CalculateProficiencyBonusByLevel (reader.GetInt32(2));
			lvPlayer.FigurineModelName = reader.GetString (11);

			if (reader.GetInt32 (12) != null && reader.GetInt32 (12) > 0)
				lvPlayer.isAi = true;

			lvPlayer.databaseEqWeaponId = reader.GetInt32 (13);
		}

		CleanUp (reader,dbcmd,dbconn);
		reader = null;
		dbcmd = null;
		dbconn = null;

		return lvPlayer;
	}

	public static void AddPlayersArmorsToInventory(int pmPlayerId, Dictionary<string,Item> pmInventory, Player player)
	{
		IDbConnection dbconn = GetConnection ();
		IDbCommand dbcmd = dbconn.CreateCommand();
		string sqlQuery = 	"select a.id, a.NAME,  a.AC, a.MAX_DEX, a.ICON_NAME, es.NAME FROM " +
			"ARMORS a join CHARACTERS_ITEMS ci on ci.ITEM_ID = a.ID and ci.ITEM_TYPE = 2  JOIN EQUIPEMENT_SLOTS es on ci.FIELD_ID = es.ID  where ci.CHARACTER_ID =" + pmPlayerId;
		dbcmd.CommandText = sqlQuery;

		IDataReader reader = dbcmd.ExecuteReader();
		while (reader.Read ()) {

			List<EquipementTypes> lvTypes = GetArmorEqTypes (reader.GetInt32 (0), dbconn);

			Item lvItem = new Armor (reader.GetString(1), lvTypes,reader.GetInt32(2),reader.GetInt32(3));
			lvItem.resourceImageName = reader.GetString (4);
			lvItem.inventoryFieldId = reader.GetString (5);
			pmInventory.Add (reader.GetString (5), lvItem);

			if ("INV_ARMOR".Equals (reader.GetString (5)))
				lvItem.Equip (player, false);

		}

		CleanUp (reader,dbcmd,dbconn);
		reader = null;
		dbcmd = null;
		dbconn = null;
	}

	public static void AddPlayersWeaponsToInventory(int pmPlayerId, Dictionary<string,Item> pmInventory, Player player)
	{
		IDbConnection dbconn = GetConnection ();
		IDbCommand dbcmd = dbconn.CreateCommand();
		string sqlQuery = 	"select w.ID, w.NAME, w.WEAPON_TYPE_ID, w.WEAPON_CATEGORY_ID, w.WEAPON_DAMAGE_DIE_SIDES, w.WEAPON_DAMAGE_DIE_QUANTITY, w.SHORT_RANGE, w.LONG_RANGE, w.ICON_NAME, es.NAME, es.ID " +
			"FROM WEAPONS w join CHARACTERS_ITEMS ci on ci.ITEM_ID = w.ID and ci.ITEM_TYPE = 1  JOIN EQUIPEMENT_SLOTS es on ci.FIELD_ID = es.ID  where ci.CHARACTER_ID = " + pmPlayerId;
		dbcmd.CommandText = sqlQuery;

		IDataReader reader = dbcmd.ExecuteReader();
		while (reader.Read ()) {

			List<EquipementTypes> lvTypes = GetWeaponEqTypes (reader.GetInt32 (0), dbconn);

			Item lvItem = new Weapon (reader.GetString (1), GetWeaponTypeById (reader.GetInt32 (2)), GetWeaponCategoryById (reader.GetInt32 (3)), reader.GetInt32 (4), reader.GetInt32 (5), lvTypes, reader.GetInt32 (6), reader.GetInt32 (7));
			lvItem.resourceImageName = reader.GetString (8);
			lvItem.inventoryFieldId = reader.GetString (9);

			if (reader.GetInt32 (10) == player.databaseEqWeaponId)
				lvItem.Equip (player, false);
				

			pmInventory.Add (reader.GetString (9), lvItem);
	}

		CleanUp (reader,dbcmd,dbconn);
		reader = null;
		dbcmd = null;
		dbconn = null;
	}

	private static WeaponType GetWeaponTypeById(int pmId)
	{
		switch (pmId) {
		case 1:
			return WeaponType.MELEE;
			break;
		case 2:
			return WeaponType.RANGED;
			break;
		default:
			return WeaponType.MELEE;
			break;
		}
	}

	private static WeaponCategory GetWeaponCategoryById(int pmId)
	{
		switch (pmId) {
		case 1:
			return WeaponCategory.SIMPLE;
			break;
		case 2:
			return WeaponCategory.MARTIAL;
			break;
		default:
			return WeaponCategory.SIMPLE;
			break;
		}
	}

	private static List<EquipementTypes> GetArmorEqTypes(int pmArmorId, IDbConnection pmConnection)
	{
		List<EquipementTypes> lvList = new List<EquipementTypes> ();

		IDbCommand dbcmd = pmConnection.CreateCommand();
		string sqlQuery = "select EQUIPEMENT_SLOTS from ARMORS_EQUIPEMENT_SLOTS where ARMORS_ID = " + pmArmorId;
		dbcmd.CommandText = sqlQuery;

		IDataReader reader = dbcmd.ExecuteReader();
		while (reader.Read ()) {
			
			lvList.Add (GetEqTypeById (reader.GetInt32 (0)));

		}

		CleanUp (reader,dbcmd,null);
		reader = null;
		dbcmd = null;

		return lvList;
	}

	private static List<EquipementTypes> GetWeaponEqTypes(int pmArmorId, IDbConnection pmConnection)
	{
		List<EquipementTypes> lvList = new List<EquipementTypes> ();

		IDbCommand dbcmd = pmConnection.CreateCommand();
		string sqlQuery = "select EQUIPEMENT_SLOTS_ID from WEAPONS_EQUIPEMENT_SLOTS where WEAPON_ID = " + pmArmorId;
		dbcmd.CommandText = sqlQuery;

		IDataReader reader = dbcmd.ExecuteReader();
		while (reader.Read ()) {

			lvList.Add (GetEqTypeById (reader.GetInt32 (0)));

		}

		CleanUp (reader,dbcmd,null);
		reader = null;
		dbcmd = null;

		return lvList;
	}

	private static EquipementTypes GetEqTypeById(int pmEqTypeId)
	{
		EquipementTypes lvType = EquipementTypes.ANY;

		switch (pmEqTypeId) {
		case 77:
			lvType = EquipementTypes.ARMOR;
			break;
		case 75:
			lvType = EquipementTypes.MAIN_HAND;
			break;
		case 76:
			lvType = EquipementTypes.OFF_HAND;
			break;
		}

		return lvType;
	}

	public static string GetFigurineNameByPlayerID(int pmPlayerId)
	{
		string lvFigurineName = "";

		IDbConnection dbconn = GetConnection ();
		IDbCommand dbcmd = dbconn.CreateCommand();
		string sqlQuery = 	"SELECT FI.FIGURINE_NAME  "
			+ "FROM CHARACTER_STATS ST JOIN FIGURINES FI ON FI.CHARACTER_ID = ST.ID WHERE ST.ID = " + pmPlayerId;
		dbcmd.CommandText = sqlQuery;

		IDataReader reader = dbcmd.ExecuteReader();
		if (reader.Read())
		{
			lvFigurineName = reader.GetString (0);
		}

		CleanUp (reader,dbcmd,dbconn);
		reader = null;
		dbcmd = null;
		dbconn = null;

		return lvFigurineName;
	}
		

}
