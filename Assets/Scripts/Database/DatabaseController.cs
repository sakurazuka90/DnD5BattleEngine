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

	public static List<string> GetListOfValues()
	{
		List<string> names = new List<string> ();

		IDbConnection dbconn = GetConnection ();
		IDbCommand dbcmd = dbconn.CreateCommand();
		string sqlQuery = "SELECT NAME " + "FROM CHARACTER_STATS";
		dbcmd.CommandText = sqlQuery;
		IDataReader reader = dbcmd.ExecuteReader();
		while (reader.Read())
		{
			string name = reader.GetString (0);
			names.Add (name);
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
		string sqlQuery = 	"SELECT ST.NAME, FI.PICTURE_NAME, ST.LEVEL, ST.STR, ST.DEX, ST.FOR, ST.INT, ST.WIS, ST.CHA, ST.HP, ST.SPEED  "
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
		}

		CleanUp (reader,dbcmd,dbconn);
		reader = null;
		dbcmd = null;
		dbconn = null;

		return lvPlayer;
	}



}
