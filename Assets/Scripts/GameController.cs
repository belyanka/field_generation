using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour {
    
    public int wallBlockNumber;
    public int enemyNumber;
    public Text loadingText;
    
    private int[][] levelGrid;
    private GameObject wall;
    private GameObject start;
    private GameObject player;
    private GameObject enemy;
    private GameObject finish;
    private bool loaded;

    private const int fieldSize = 10;

    // Use this for initialization
    void Start()
    {
        //Show "Loading..."
        loaded = false;
        loadingText.text = "Loading...";
        
        StartCoroutine(GenerateLevel());
        
    }

    private void Update()
    {
        if (loaded)
        {
            //Hide "Loading..."
            //loadingText.text = "";    
        }
    }

    IEnumerator GenerateLevel()
    {
        /*
            Level generation
        */
        LevelGenerator generator = new LevelGenerator();
        generator.Generate(fieldSize, wallBlockNumber, enemyNumber);
        levelGrid = generator.GetLevelGrid();
        yield return new WaitForSeconds(2);
        /*
        Loading resources
        */
        wall = Resources.Load<GameObject>("Wall Block");
        start = Resources.Load<GameObject>("Start Position");
        finish = Resources.Load<GameObject>("Finish Position");
        enemy = Resources.Load<GameObject>("Enemy");
        player = Resources.Load<GameObject>("Player");

        //Set the obstacles, Start and Finish, Player and Enemies
        SetLevelEnvironment();

        loaded = true;
        loadingText.text = "";
        yield return true;
    }
    private void SetLevelEnvironment() {
        for (int i = 0; i < fieldSize; i++) {
            for (int j = 0; j < fieldSize; j++) {
                switch (levelGrid[i][j]) {
                    case (int) Block.Wall:
                        Instantiate(wall, new Vector3((float) j, 0.5f, (float) i), Quaternion.identity);
                        break;
                    case (int) Block.Start:
                        Instantiate(start, new Vector3((float) j, 0.05f, (float) i), Quaternion.identity);
                        Instantiate(player, new Vector3((float) j, 0.5f, (float) i), Quaternion.identity);
                        break;
                    case (int) Block.Finish:
                        Instantiate(finish, new Vector3((float) j, 0.05f, (float) i), Quaternion.identity);
                        break;                  
                }
            }
        }

        int genZ, genX;
        while (enemyNumber>0) {
            genZ = Random.Range(0, fieldSize - 1);
            genX = Random.Range(0, fieldSize - 1);
            if (levelGrid[genZ][genX]==(int)Block.Empty) {
                Instantiate(enemy, new Vector3((float) genX, 0.5f, (float) genZ), Quaternion.identity);
                enemyNumber--;
            }
        }
        
    }
}