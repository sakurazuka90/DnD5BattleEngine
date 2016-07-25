using Mono.Data.Sqlite;
using UnityEngine;
using System;
using System.Data;
using System.Collections.Generic;

public class PlayerDatabaseSaver
{
	public static int Save(Player player, string pmSelectedFigurine)
	{

		Dictionary<int,string> names = new Dictionary<int,string> ();

		IDbConnection dbconn = DatabaseController.GetConnection ();
		IDbCommand dbcmd = dbconn.CreateCommand();
		string sqlQuery = "INSERT INTO CHARACTER_STATS (NAME, PLAYER_NAME) VALUES ('" + player.playerName + "', '" + player.gamerName + "') ";
		dbcmd.CommandText = sqlQuery;
		dbcmd.ExecuteScalar ();

		dbcmd.Dispose ();

		dbcmd = dbconn.CreateCommand();
		sqlQuery = "SELECT last_insert_rowid()";
		dbcmd.CommandText = sqlQuery;

		int id = 0;

		IDataReader reader = dbcmd.ExecuteReader();
		if (reader.Read())
		{
			id = reader.GetInt32(0);
		}

		Debug.Log(id);

		InsertFigurine (pmSelectedFigurine, player.PlayerSprite.name, id, dbconn);

		DatabaseController.CleanUp (reader, dbcmd,dbconn);
		dbcmd = null;
		dbconn = null;

		return id;
	}

	public static int InsertFigurine(string pmFigurineName, string pmImageName, int pmCharacterId, IDbConnection pmConnection)
	{
		IDbCommand dbcmd = pmConnection.CreateCommand();
		string sqlQuery = "INSERT INTO FIGURINES (FIGURINE_NAME, PICTURE_NAME, CHARACTER_ID) VALUES ('" + pmFigurineName + "', '" + pmImageName + "', " + pmCharacterId + ") ";
		dbcmd.CommandText = sqlQuery;
		dbcmd.ExecuteScalar ();

		dbcmd.Dispose ();

		dbcmd = pmConnection.CreateCommand();
		sqlQuery = "SELECT last_insert_rowid()";
		dbcmd.CommandText = sqlQuery;

		int id = 0;

		IDataReader reader = dbcmd.ExecuteReader();
		if (reader.Read())
		{
			id = reader.GetInt32(0);
		}

		Debug.Log(id);

		dbcmd.Dispose ();
		dbcmd = null;


		return id;
	}
}


