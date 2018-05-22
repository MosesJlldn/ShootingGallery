using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour 
{
    public CharacterCreator cc;
    public DifficultChange dc;

	public void NewGameBt(string newGameLevel)
	{
        Player pl = new Player(cc.charName, 0, dc.dif);
        string dataPath = Application.dataPath + "/gamedata/settings.json";
        string dataAsJson = JsonUtility.ToJson(pl);
        Debug.Log(dataAsJson);
        File.WriteAllText(dataPath, dataAsJson);
       
        SceneManager.LoadScene("game_beta", LoadSceneMode.Single);
    }

	public void ExitGameBt()
	{
		Application.Quit ();
	}
		
}
