using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Block {
    Empty,
    Wall,
    Start,
    Finish
}

public class LevelGenerator
{

	private int fieldSize;
	private int wallBlockNumber;
	private int enemyNumber;
	
	private int[][] levelGrid;
	private int[][] stepArray;
//	private GameObject wall;
//	private GameObject start;
//	private GameObject player;
//	private GameObject enemy;
//	private GameObject finish;

	
	public void Generate(int size, int blockNumber, int enemies)
	{
		fieldSize = size;
		wallBlockNumber = blockNumber;
		enemyNumber = enemies;
	    
	    GenerateLevelGrid();
	    
	    /*wall = Resources.Load<GameObject>("Wall Block");
	    start = Resources.Load<GameObject>("Start Position");
	    finish = Resources.Load<GameObject>("Finish Position");
	    enemy = Resources.Load<GameObject>("Enemy");
	    player = Resources.Load<GameObject>("Player");

	    //Set the obstacles, Start and Finish, Player and Enemies
	    SetLevelEnvironment();
*/
	}
	
	private void GenerateLevelGrid() {
        //Init with zeroes
        levelGrid = new int[fieldSize][];

        for (int i = 0; i < fieldSize; i++) {
            levelGrid[i] = new int[fieldSize];
            for (int j = 0; j < fieldSize; j++) {
                levelGrid[i][j] = (int) Block.Empty;
            }
        }

        //Initiate Start and Finish points
        int startZ = Random.Range(0, fieldSize);
        int finishZ = Random.Range(0, fieldSize);
        levelGrid[startZ][0] = (int) Block.Start;
        levelGrid[finishZ][fieldSize - 1] = (int) Block.Finish;

        //Wave algorithm

        //Wave expansion
        stepArray = new int[fieldSize][];
        for (int i = 0; i < fieldSize; i++) {
            stepArray[i] = new int[fieldSize];
            for (int j = 0; j < fieldSize; j++) {
                stepArray[i][j] = -1;
            }
        }

        stepArray[startZ][0] = 0; //Start
        bool found = false;
        int step = -1;
        while (!found || step > fieldSize * fieldSize) {
            step++;
            for (int i = 0; i < stepArray.Length; i++) {
                for (int j = 0; j < stepArray[0].Length; j++) {
                    if (stepArray[i][j] == step) {
                        if (i == finishZ && j == fieldSize - 1) {
                            found = true;
                        }
                        else {
                            FillStep(i + 1, j, step); //up
                            FillStep(i, j + 1, step); //right
                            FillStep(i - 1, j, step); //down
                            FillStep(i, j - 1, step); //left
                        }
                    }
                }
            }
        }

        //Choose the right route
        List<int[]> route = new List<int[]>();
        route.Add(new[] {finishZ, fieldSize - 1});
        for (int i = 0; i < step; i++) {
            route.Add(ChooseNextStep(route[i], step - i));
        }
        route.Add(new[] {startZ, 0});

        //Set levelGrid
        int wallsSet = 0;
        while (wallsSet < wallBlockNumber) {
            int genZ = Random.Range(0, fieldSize - 1);
            int genX = Random.Range(0, fieldSize - 1);
            bool onRoute = false;
            for (int i = 0; i < route.Count; i++) {
                if (route[i][0] == genZ && route[i][1] == genX) {
                    onRoute = true;
                    break;
                }
            }
            if (!onRoute) {
                levelGrid[genZ][genX] = (int) Block.Wall;
                wallsSet++;
            }
        }
    }

    private void FillStep(int i, int j, int step) {
        if (ChechStepInBoundsAndEquals(i, j, -1)) {
            stepArray[i][j] = step + 1;
        }
    }

    private int[] ChooseNextStep(int[] previousStep, int step) {
        List<int[]> possibleSteps = new List<int[]>();
        if (ChechStepInBoundsAndEquals(previousStep[0] + 1, previousStep[1], step - 1)) {
            //up
            possibleSteps.Add(new[] {previousStep[0] + 1, previousStep[1]});
        }
        if (ChechStepInBoundsAndEquals(previousStep[0], previousStep[1] + 1, step - 1)) {
            //right
            possibleSteps.Add(new[] {previousStep[0], previousStep[1] + 1});
        }
        if (ChechStepInBoundsAndEquals(previousStep[0] - 1, previousStep[1], step - 1)) {
            //down
            possibleSteps.Add(new[] {previousStep[0] - 1, previousStep[1]});
        }
        if (ChechStepInBoundsAndEquals(previousStep[0], previousStep[1] - 1, step - 1)) {
            //left
            possibleSteps.Add(new[] {previousStep[0], previousStep[1] - 1});
        }
        return possibleSteps[Random.Range(0, possibleSteps.Count - 1)];
    }

    private bool ChechStepInBoundsAndEquals(int i, int j, int equal) {
        if (i < stepArray.Length && i >= 0 && j < stepArray[0].Length && j >= 0 && stepArray[i][j] == equal) {
            return true;
        }
        return false;
    }

    public int[][] GetLevelGrid()
    {
        return levelGrid;
    }
}
