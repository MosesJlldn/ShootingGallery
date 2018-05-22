using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreTableEditor {

	private TableData tableData;
	private string dataPath = Application.dataPath + "/gamedata/tableData.json";

	public ScoreTableEditor () {
        tableData = new TableData();
        tableData.players = new List<Player> ();
        /*add(new Player("aaa", 0, 1));
        add(new Player("ggg", 100, 2));
        add(new Player("absdfasa", 14, 3));
        SaveGameData();*/
    }

	public string getTable() {

		string table = "";

		foreach (Player i in tableData.players) {

			table += i.Name + " : " + i.Score + " (" + i.difficultyString + ")\n";
		}

		return table;
	}

	public void add(Player player) {
		tableData.players.Add(player);
	}

	public void LoadGameData()
	{
		if (File.Exists (dataPath)) {
			string dataAsJson = File.ReadAllText (dataPath);
			tableData = JsonUtility.FromJson<TableData> (dataAsJson);
            if (tableData == null)
            {
                tableData = new TableData();
                tableData.players = new List<Player>();
            }
		} else 
		{
			tableData = new TableData ();
		}
	}

	public void SaveGameData()
	{

		string dataAsJson = JsonUtility.ToJson(tableData);
        Debug.Log(dataAsJson);
		File.WriteAllText (dataPath, dataAsJson);
	}
}
