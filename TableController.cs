using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour {

    private ScoreTableEditor sce;
    public TargetManager tm;

    // Use this for initialization
    void Start () {
        sce = new ScoreTableEditor();
        sce.LoadGameData();
        tm.scoreTable.text = sce.getTable(); 
	}
	
	// Update is called once per frame
	void Update () {

    }

    void savePlayer ()
    {
        Player pl = TargetManager.currentPlayer;
        pl.score = tm.getScore();
        switch (tm.difficulty)
        {
            case 1:
                pl.difficultyString = "eazy";
                break;
            case 2:
                pl.difficultyString = "normal";
                break;
            case 3:
                pl.difficultyString = "hard";
                break;
        }
        sce.add(pl);
        sce.SaveGameData();
    }

    void OnEnable()
    {
        TargetManager.OnEnded += savePlayer;
    }


    void OnDisable()
    {
        TargetManager.OnEnded -= savePlayer;
    }
}
