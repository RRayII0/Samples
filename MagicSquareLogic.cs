using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MagicSquareLogic : MonoBehaviour
{

    public GameObject[] tiles;
    public GameObject[] numTiles;
    public GameObject[] highDifficultyObjects;
    public GameObject[] movableXObjects;
    public GameObject[] movableXYObjects;
    public Text[] sumsText;
    public GameObject startIcon;
    public GameObject resetIcon;
    public GameObject overlay;
    public GameObject nextBtn;
    public GameObject m_icon;
    public GameObject m_audioMngr;

    private int numOfTiles = 16;
    private bool isPuzzleSolved = false;
    private bool squareSet = false;

    float ROW_ARROW_X = 24f;
    float DIAG_ARROW_X = 5f;
    float DIAG_ARROW_Y = -265f;

    static GameObject[] puzzleTiles = new GameObject[16];
    static int[] puzzleTileValues = new int[16];
    static int[] sums = new int[10];

    static int difficultyLevel;

    int[] sum0Indices = { 0,  4,  8, 12};
    int[] sum1Indices = { 1,  5,  9, 13};
    int[] sum2Indices = { 2,  6, 10, 14};
    int[] sum3Indices = { 3,  7, 11, 15};
    int[] sum4Indices = { 0,  1,  2,  3};
    int[] sum5Indices = { 4,  5,  6,  7};
    int[] sum6Indices = { 8,  9, 10, 11};
    int[] sum7Indices = {12, 13, 14, 15};
    int[] sum8Indices = { 3,  6,  9, 12};
    int[] sum9Indices = { 0,  5, 10, 15};
    int[] sum8AltIndices = { 2, 3, 5, 8};

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Restart()
    {
        //Debug.Log("Move2DLogic Restart() called...");

        squareSet = false;
        isPuzzleSolved = false;

        Color c = overlay.GetComponent<Image>().color;
        c.a = 1f;
        overlay.GetComponent<Image>().color = c;
        overlay.SetActive(true);
        startIcon.SetActive(true);
        resetIcon.SetActive(false);

        foreach (GameObject obj in movableXObjects)
        {
            Vector3 startPos = obj.transform.localPosition;
            string name = obj.name;
            if (name.Contains("Row"))
            {
                startPos.x = ROW_ARROW_X;
            }
            else
            {
                startPos.x = DIAG_ARROW_X;
            }
            obj.transform.localPosition = startPos;
        }
        foreach (GameObject obj in movableXYObjects)
        {
            Vector3 startPos = obj.transform.localPosition;
            startPos.x = DIAG_ARROW_X;
            startPos.y = DIAG_ARROW_Y;
            obj.transform.localPosition = startPos;
        }

        for (int i=0; i < numTiles.Length; i++)
        {
            numTiles[i].GetComponent<NumMove>().SetStartPosition();
            ReleaseFromGrid(numTiles[i]);
        }

        // Setup based on difficulty
        difficultyLevel = PlayerPrefs.GetInt("difficultyLevel", 0); // Do I need the default value anymore??
        //Debug.Log("Diff Level: " + difficultyLevel);
        string str = "Easy";
        if (difficultyLevel > 0)
        {
            str = "Hard";
        }
        // Set level at which puzzles have been solved.
        int test = PlayerPrefs.GetInt(str);
        if (test >= 7)
        {
            m_icon.GetComponent<IconReveal>().RevealIcon();
            nextBtn.gameObject.SetActive(true);
        }

    }

    public  void SetupSquare()
    {
        if (squareSet == true)
        {
            // Reset numbers [1-16] to home positions
            for(int i=1; i<17; i++)
            {
                SendTileToHome(i);
            }

            return;
        }

        squareSet = true;
        startIcon.SetActive(false);
        resetIcon.SetActive(true);

        for (int i = 0; i < numOfTiles; i++)
        {
            MagicSquareLogic.puzzleTiles[i] = tiles[i];
            MagicSquareLogic.puzzleTileValues[i] = 0;
        }

        // If on level 0 difficulty, will need to turn off squares 3, 7, 11 and 12-15.
        // Also, turn off one row and one column sum AND move sums by -100 in X axis
        int difficultyLevel = PlayerPrefs.GetInt("difficultyLevel");
        //Debug.Log("Difficulty(for SetupSquare): " + difficultyLevel);

        if (difficultyLevel == 0)
        {
            MagicSquareLogic.puzzleTiles[3].SetActive(false);
            MagicSquareLogic.puzzleTiles[7].SetActive(false);
            MagicSquareLogic.puzzleTiles[11].SetActive(false);
            MagicSquareLogic.puzzleTiles[12].SetActive(false);
            MagicSquareLogic.puzzleTiles[13].SetActive(false);
            MagicSquareLogic.puzzleTiles[14].SetActive(false);
            MagicSquareLogic.puzzleTiles[15].SetActive(false);

            foreach (GameObject obj in highDifficultyObjects)
            {
                obj.SetActive(false);
            }

            foreach (GameObject obj in movableXObjects)
            {
                Vector3 altPos = new Vector3(obj.transform.position.x - 100, obj.transform.position.y, obj.transform.position.z);
                obj.transform.position = altPos;
            }
            foreach (GameObject obj in movableXYObjects)
            {
                Vector3 altPos = new Vector3(obj.transform.position.x - 100, obj.transform.position.y + 100, obj.transform.position.z);
                obj.transform.position = altPos;
            }

        }
        else
        {
            MagicSquareLogic.puzzleTiles[3].SetActive(true);
            MagicSquareLogic.puzzleTiles[7].SetActive(true);
            MagicSquareLogic.puzzleTiles[11].SetActive(true);
            MagicSquareLogic.puzzleTiles[12].SetActive(true);
            MagicSquareLogic.puzzleTiles[13].SetActive(true);
            MagicSquareLogic.puzzleTiles[14].SetActive(true);
            MagicSquareLogic.puzzleTiles[15].SetActive(true);

            foreach (GameObject obj in highDifficultyObjects)
            {
                obj.SetActive(true);
            }

            //foreach (GameObject obj in movableXObjects)
            //{
            //    Vector3 altPos = new Vector3(obj.transform.position.x - 100, obj.transform.position.y, obj.transform.position.z);
            //    obj.transform.position = altPos;
            //}
            //foreach (GameObject obj in movableXYObjects)
            //{
            //    Vector3 altPos = new Vector3(obj.transform.position.x - 100, obj.transform.position.y + 100, obj.transform.position.z);
            //    obj.transform.position = altPos;
            //}
        }

        // Run coroutine to fade out, then remove overlay
        StartCoroutine("OverlayFadeToInactive");
    }

    public Vector3 SnapToGrid(GameObject selectedTile)
    {
        GameObject onTile = null;
        float nearestDist = 1000f;
        float testDist;


        // TODO: Add if nearest tile and all tiles under piece are available, else send to home position(rotation => 0)

        foreach (GameObject tile in MagicSquareLogic.puzzleTiles)
        {
            //Debug.Log("Selected Tile: " + selectedTile.name);
            //Debug.Log("Test Grid Pos: " + tile.name);
            testDist = Vector2.Distance(selectedTile.transform.position, tile.transform.position);
            if (tile.activeSelf && testDist < nearestDist && testDist < 100.0f)
            {
                nearestDist = testDist;
                onTile = tile;
            }
        }

        if (onTile != null && nearestDist < 1000f)
        {
            // RDR TODO: I need to register the value with the grid...
            int idx = System.Array.IndexOf(MagicSquareLogic.puzzleTiles, onTile);
            int value = selectedTile.GetComponent<NumMove>().numValue;
            // If TileValue > 0, then number tile already placed.  Need to send value (as index) back to Num Tile?
            int prevValue = MagicSquareLogic.puzzleTileValues[idx];
            if (prevValue > 0)
            {
                SendTileToHome(prevValue);
            }



            MagicSquareLogic.puzzleTileValues[idx] = value;


            CalcMagicSquareSums();

            return (onTile.transform.position);
        }
        else
        {
            return (Vector3.zero);
        }

    }

    // Remove number value from grid...
    public void ReleaseFromGrid(GameObject selectedTile)
    {
        int value = selectedTile.GetComponent<NumMove>().numValue;
        // Remove this value (set to zero) from anywhere in the grid...
        foreach (GameObject tile in MagicSquareLogic.puzzleTiles)
        {
            int idx = System.Array.IndexOf(MagicSquareLogic.puzzleTiles, tile);
            if (MagicSquareLogic.puzzleTileValues[idx] == value)
            {
                // Set value back to zero...
                MagicSquareLogic.puzzleTileValues[idx] = 0;
                CalcMagicSquareSums();
            }
        }
    }


    void SendTileToHome(int value)
    {
        int idx = value - 1;
        numTiles[idx].GetComponent<NumMove>().SendHome();
        ReleaseFromGrid(numTiles[idx]);
    }

    public bool IsSolved()
    {
        return isPuzzleSolved;
    }

    void CalcMagicSquareSums()
    {
        for (int i = 0; i < 10; i++)
        {
            sums[i] = 0;
        }

        for (int i=0; i < 4; i++)
        {
            MagicSquareLogic.sums[0] += MagicSquareLogic.puzzleTileValues[sum0Indices[i]];
            MagicSquareLogic.sums[1] += MagicSquareLogic.puzzleTileValues[sum1Indices[i]];
            MagicSquareLogic.sums[2] += MagicSquareLogic.puzzleTileValues[sum2Indices[i]];
            MagicSquareLogic.sums[3] += MagicSquareLogic.puzzleTileValues[sum3Indices[i]];
            MagicSquareLogic.sums[4] += MagicSquareLogic.puzzleTileValues[sum4Indices[i]];
            MagicSquareLogic.sums[5] += MagicSquareLogic.puzzleTileValues[sum5Indices[i]];
            MagicSquareLogic.sums[6] += MagicSquareLogic.puzzleTileValues[sum6Indices[i]];
            MagicSquareLogic.sums[7] += MagicSquareLogic.puzzleTileValues[sum7Indices[i]];
            if (difficultyLevel == 0)
            {
                MagicSquareLogic.sums[8] += MagicSquareLogic.puzzleTileValues[sum8AltIndices[i]];
            }
            else
            {
                MagicSquareLogic.sums[8] += MagicSquareLogic.puzzleTileValues[sum8Indices[i]];
            }
            MagicSquareLogic.sums[9] += MagicSquareLogic.puzzleTileValues[sum9Indices[i]];
        }

        for (int i = 0; i < 10; i++)
        {
            Text numText = sumsText[i].GetComponent<Text>();

            sumsText[i].text = MagicSquareLogic.sums[i].ToString();
            if ((MagicSquareLogic.sums[i] == 15 && difficultyLevel == 0) || (MagicSquareLogic.sums[i] == 34 && difficultyLevel == 1))
            {
                numText.color = Color.green;
            }
            else
            {
                numText.color = Color.white;
            }
        }

        isPuzzleSolved = CheckForSolved();

        //  RDR TODO: Show puzzle is solved with "celebration"
        if (isPuzzleSolved == true)
        {
            //Debug.Log("Puzzle solved!");
            // turn all numbers to green?
            foreach(GameObject obj in numTiles)
            {
                // Get all the children (digits) and color them green...
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    obj.transform.GetChild(i).GetComponent<Image>().color = Color.green;
                }
            }
            nextBtn.SetActive(true);
            m_icon.GetComponent<IconReveal>().RevealIcon();
            m_audioMngr.GetComponent<AudioManager>().PlaySuccess();

            string str = "Easy";
            int difficultyLevel = PlayerPrefs.GetInt("difficultyLevel");
            if (difficultyLevel > 0)
            {
                str = "Hard";
            }
            // Set level at which puzzles have been solved.
            int test = PlayerPrefs.GetInt(str);
            if (test < 7)
            {
                PlayerPrefs.SetInt(str, 7);
                PlayerPrefs.Save();
            }
        }

    }

    bool CheckForSolved()
    {
        switch (difficultyLevel)
        {
            case 0:
                int[] indexes = { 0, 1, 2, 4, 5, 6, 8, 9 }; // skip 3 and 7 for 3x3 puzzle
                for (int i = 0; i <indexes.Length; i++)
                {
                    if (MagicSquareLogic.sums[indexes[i]] != 15)
                    {
                        return false;
                    }
                }
                break;
            case 1:
                for (int i=0; i<10; i++)
                {
                    if (MagicSquareLogic.sums[i] != 34)
                    {
                        return false;
                    }
                }
                break;
        }
        return true;

    }

    IEnumerator OverlayFadeToInactive()
    {
        overlay.GetComponent<Image>().DOFade(0, 0.5f);

        yield return new WaitForSeconds(0.6f);

        overlay.SetActive(false);

        yield return null;
    }

    public void ShutdownCanvas()
    {
        DOTween.KillAll();
        //Debug.Log("MagicSquareLogic: ShutdownCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
