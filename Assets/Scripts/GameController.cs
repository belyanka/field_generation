using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour {
    
    public int wallBlockNumber;
    public int enemyNumber;
    public Text loadingText;
    public Text lifeText;
    public Button mainMenuButton;
    
    private int[][] levelGrid;
    private PlayerController playerController;
    private bool loaded;

    private const int fieldSize = 10;

    // Use this for initialization
    void Start()
    {
        //Show "Loading..."
        Time.timeScale = 1;
        loaded = false;
        mainMenuButton.gameObject.SetActive(false);
        loadingText.text = "Loading...";
        
        StartCoroutine(GenerateLevel());
        
    }

    public void GameOver()
    {
        loadingText.text = "Game Over";
        mainMenuButton.gameObject.SetActive(true);
        GamePause();
    }

    public void Win()
    {
        loadingText.text = "You win!";
        mainMenuButton.gameObject.SetActive(true);
        GamePause();
    }

    public void UpdateLifeCount(int life)
    {
        lifeText.text = "Life: " + life;
    }

    private void GamePause()
    {
        Time.timeScale = 0;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator GenerateLevel()
    {
        /*
            Level generation
        */
        LevelGenerator generator = new LevelGenerator();
        generator.Generate(fieldSize, wallBlockNumber, enemyNumber);
        levelGrid = generator.GetLevelGrid();
        //yield return new WaitForSeconds(2);
       
        //Set the obstacles, Start and Finish, Player and Enemies
        SetLevelEnvironment();

        loaded = true;
        loadingText.text = "";
        yield return true;
    }
    private void SetLevelEnvironment() {
        /*
       Loading resources
       */
        GameObject wall = Resources.Load<GameObject>("Wall Block");
        GameObject start = Resources.Load<GameObject>("Start Position");
        GameObject finish = Resources.Load<GameObject>("Finish Position");
        GameObject enemy = Resources.Load<GameObject>("Enemy");
        GameObject player = Resources.Load<GameObject>("Player");

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