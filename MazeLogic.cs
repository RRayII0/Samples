using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MazeLogic : MonoBehaviour
{
    const int MAXWIDTH = 7;
    const int MAXHEIGHT = 9;
    const int maxDelta = 50;

    public int[,] level1Data = new int[MAXWIDTH, MAXHEIGHT];
    public int[,] level2Data = new int[MAXWIDTH, MAXHEIGHT];
    public bool editMaze = false;
    public GameObject levelOne;
    public GameObject levelTwo;
    public GameObject[] Lvl1HLines;
    public GameObject[] Lvl1VLines;
    public GameObject[] Lvl2HLines;
    public GameObject[] Lvl2VLines;
    public GameObject pinOne;
    public GameObject pinTwo;
    public GameObject nextBtn;
    public GameObject startBtn;
    public GameObject overlay;
    public GameObject m_icon;
    public GameObject m_audioMngr;

    static int difficultyLevel;


    private Vector3 curPos;
    //private int curPoint = 0;
    private bool pin1Flag = true;
    private bool pin2Flag = true;

    private int[] data1 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private int[] data2 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    private int[] data1Hard = new int[] { 0, 6, 14, 10, 10, 10, 12, 0, 7, 9, 0, 0, 6, 9, 2, 13, 0, 0, 0, 5, 0, 2, 11, 14, 10, 12, 3, 12, 0, 6, 11, 8, 3, 10, 9, 0, 5, 0, 0, 0, 6, 12, 0, 7, 10, 14, 12, 7, 13, 2, 11, 8, 5, 7, 15, 9, 0, 0, 0, 3, 11, 11, 8 };
    private int[] data2Hard = new int[] { 6, 14, 12, 0, 0, 0, 0, 7, 9, 5, 2, 10, 12, 0, 5, 2, 15, 14, 8, 5, 0, 1, 0, 5, 5, 2, 15, 8, 6, 10, 9, 1, 0, 5, 0, 3, 14, 10, 12, 0, 5, 0, 0, 5, 0, 3, 14, 13, 0, 0, 7, 12, 0, 5, 1, 0, 0, 1, 1, 0, 1, 0, 0 };
    private int[] data1Easy = new int[] { 0, 0, 0, 4, 0, 0, 0, 0, 6, 10, 9, 0, 0, 0, 0, 3, 12, 6, 10, 12, 0, 0, 0, 5, 5, 0, 5, 0, 0, 0, 7, 15, 8, 5, 0, 0, 0, 3, 9, 0, 5, 0, 0, 0, 0, 0, 6, 13, 0, 0, 0, 0, 4, 5, 3, 8, 0, 0, 0, 3, 9, 0, 0 };
    private int[] data2Easy = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0, 0, 6, 9, 0, 0, 0, 0, 0, 3, 10, 14, 12, 0, 0, 0, 0, 0, 3, 11, 14, 8, 0, 0, 0, 0, 0, 3, 12, 0, 0, 6, 12, 2, 14, 9, 0, 0, 3, 11, 10, 11, 8, 0, 0, 0, 0, 0, 0, 0, 0 };

    // Variables tracking pins 1 and 2 on Level 1
    int curX1Pin1 = 1;
    int curY1Pin1 = 1;
    int curX1Pin2 = 5;
    int curY1Pin2 = 7;
    // Variables tracking pins 1 and 2 on Level 2
    int curX2Pin1 = 1;
    int curY2Pin1 = 1;
    int curX2Pin2 = 5;
    int curY2Pin2 = 7;

    bool isSolved = false;

    // Start is called before the first frame update
    void Start()
    {
        //overlay.SetActive(true);


        //difficultyLevel = PlayerPrefs.GetInt("difficultyLevel", 0); // Do I need the default value anymore??
        //Debug.Log("Diff Level: " + difficultyLevel);
        //if (difficultyLevel == 1)
        //{
        //    // Use harder data set
        //    data1 = data1Hard;
        //    data2 = data2Hard;
        //}
        //else
        //{
        //    // Use easier data set
        //    data1 = data1Easy;
        //    data2 = data2Easy;
        //}
        //
        //Vector3 pin1Pos = pinOne.transform.position;
        //pin1Pos += new Vector3(-2 * 50, 3 * 50, 0);
        //Vector3 pin2Pos = pinTwo.transform.position;
        //pin2Pos += new Vector3( 2 * 50, -3 * 50, 0);
        //
        //pinOne.transform.position = new Vector3 (pin1Pos.x, pin1Pos.y, pin1Pos.z);
        //pinTwo.transform.position = new Vector3 (pin2Pos.x, pin2Pos.y, pin2Pos.z);
        //
        //
        //curPos = levelOne.transform.position;
        //
        //pin1Flag = true;
        //pin2Flag = true;
        //
        //FillLevel1Data();
        //FillLevel2Data();
        //
        //BuildMazeConnections();

    }

    public void Restart()
    {
        //Debug.Log("Maze Logic Restart() called...");
        isSolved = false;

        Color c = overlay.GetComponent<Image>().color;
        c.a = 1f;
        overlay.GetComponent<Image>().color = c;
        overlay.SetActive(true);

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
        if (test >= 9)
        {
            m_icon.GetComponent<IconReveal>().RevealIcon(0.615f);
            nextBtn.gameObject.SetActive(true);
        }

        level1Data = new int[MAXWIDTH, MAXHEIGHT];
        level2Data = new int[MAXWIDTH, MAXHEIGHT];

        levelOne.transform.localPosition = Vector3.zero;
        levelTwo.transform.localPosition = Vector3.zero;

        // Default scale is 0.4f
        Vector3 scaleUp = new Vector3(0.4f, 0.4f, 0.4f);

        pinOne.transform.localPosition = Vector3.zero;
        pinOne.transform.localScale = scaleUp;

        pinTwo.transform.localPosition = Vector3.zero;
        pinTwo.transform.localScale = scaleUp;

        curX1Pin1 = 1;
        curY1Pin1 = 1;
        curX1Pin2 = 5;
        curY1Pin2 = 7;

        curX2Pin1 = 1;
        curY2Pin1 = 1;
        curX2Pin2 = 5;
        curY2Pin2 = 7;

        pin1Flag = true;
        pin2Flag = true;

        // The Start button calls this...
        //SetupMaze();

        startBtn.SetActive(true);
    }

    public void ShutdownCanvas()
    {
        DOTween.KillAll();
        //Debug.Log("MazeLogic: ShutdownCanvas");
    }


    public void moveUpOne(GameObject level)
    {
        if (isSolved)
        {
            return;
        }

        if (level == levelOne)
        {
            if (curY1Pin1 == 8 || curY1Pin2 == 8)
            {
                // Can no longer move up...
                return;
            }
        }

        if (level == levelTwo)
        {
            if (curY2Pin1 == 8 || curY2Pin2 == 8)
            {
                // Can no longer move up...
                return;
            }
        }

        if (editMaze == false)
        {
            int pathTest = 0;
            int pathTest2 = 0;

            if (level == levelOne)
            {
                pathTest = level1Data[curX1Pin1, curY1Pin1];
                pathTest2 = level1Data[curX1Pin2, curY1Pin2];
            }
            else if (level == levelTwo)
            {
                pathTest = level2Data[curX2Pin1, curY2Pin1];
                pathTest2 = level2Data[curX2Pin2, curY2Pin2];
            }

            if ((!isDown(pathTest) && pin1Flag) || (!isDown(pathTest2) && pin2Flag))
            {
                return;
            }
        }

        Vector3 pos = level.transform.position;
        pos.y += 50;
        level.transform.position = new Vector3 (pos.x, pos.y, pos.z);

        if (editMaze == true)
        {
            if (level == levelOne)
            {
                // Update path data value and redraw lines
                int value = level1Data[curX1Pin1, curY1Pin1 + 1];
                if (!(isUp(value)))
                {
                    level1Data[curX1Pin1, curY1Pin1 + 1] += 1;
                    //Debug.Log("X:Y:Value = " + curX1Pin1 + ":" + (curY1Pin1 + 1) + ":" + level1Data[curX1Pin1, curY1Pin1 + 1]);
                }
                value = level1Data[curX1Pin1, curY1Pin1];
                if (!(isDown(value)))
                {
                    level1Data[curX1Pin1, curY1Pin1] += 4;
                    //Debug.Log("X:Y:Value = " + curX1Pin1 + ":" + curY1Pin1 + ":" + level1Data[curX1Pin1, curY1Pin1]);
                }
                // Pin2
                value = level1Data[curX1Pin2, curY1Pin2 + 1];
                if (!(isUp(value)))
                {
                    level1Data[curX1Pin2, curY1Pin2 + 1] += 1;
                    //Debug.Log("X:Y:Value = " + curX1Pin2 + ":" + (curY1Pin2 + 1) + ":" + level1Data[curX1Pin2, curY1Pin2 + 1]);
                }
                value = level1Data[curX1Pin2, curY1Pin2];
                if (!(isDown(value)))
                {
                    level1Data[curX1Pin2, curY1Pin2] += 4;
                    //Debug.Log("X:Y:Value = " + curX1Pin2 + ":" + curY1Pin2 + ":" + level1Data[curX1Pin2, curY1Pin2]);
                }
                BuildMazeConnections();
            }
            else if (level == levelTwo)
            {
                // Update path data value and redraw lines
                int value = level2Data[curX2Pin1, curY2Pin1 + 1];
                if (!(isUp(value)))
                {
                    level2Data[curX2Pin1, curY2Pin1 + 1] += 1;
                    //Debug.Log("X:Y:Value = " + curX2Pin1 + ":" + (curY2Pin1 + 1) + ":" + level2Data[curX2Pin1, curY2Pin1 + 1]);
                }
                value = level2Data[curX2Pin1, curY2Pin1];
                if (!(isDown(value)))
                {
                    level2Data[curX2Pin1, curY2Pin1] += 4;
                    //Debug.Log("X:Y:Value = " + curX2Pin1 + ":" + curY2Pin1 + ":" + level2Data[curX2Pin1, curY2Pin1]);
                }
                // Pin2
                value = level2Data[curX2Pin2, curY2Pin2 + 1];
                if (!(isUp(value)))
                {
                    level2Data[curX2Pin2, curY2Pin2 + 1] += 1;
                    //Debug.Log("X:Y:Value = " + curX2Pin2 + ":" + (curY2Pin2 + 1) + ":" + level2Data[curX2Pin2, curY2Pin2 + 1]);
                }
                value = level2Data[curX2Pin2, curY2Pin2];
                if (!(isDown(value)))
                {
                    level2Data[curX2Pin2, curY2Pin2] += 4;
                    //Debug.Log("X:Y:Value = " + curX2Pin2 + ":" + curY2Pin2 + ":" + level2Data[curX2Pin2, curY2Pin2]);
                }
                BuildMazeConnections();
            }
        }
        //curY1 += 1;
        if (level == levelOne)
        {
            if (pin1Flag)
            {
                curY1Pin1 += 1;
            }
            if (pin2Flag)
            {
                curY1Pin2 += 1;
            }
        }
        else if (level == levelTwo)
        {
            if (pin1Flag)
            {
                curY2Pin1 += 1;
            }
            if (pin2Flag)
            {
                curY2Pin2 += 1;
            }
        }
        CheckPins();
    }

    public void moveDownOne(GameObject level)
    {
        if (isSolved)
        {
            return;
        }

        if (level == levelOne)
        {
            if (curY1Pin1 == 0 || curY1Pin2 == 0)
            {
                // Can no longer move up...
                return;
            }
        }

        if (level == levelTwo)
        {
            if (curY2Pin1 == 0 || curY2Pin2 == 0)
            {
                // Can no longer move up...
                return;
            }
        }

        if (editMaze == false)
        {
            int pathTest = 0;
            int pathTest2 = 0;

            if (level == levelOne)
            {
                pathTest = level1Data[curX1Pin1, curY1Pin1];
                pathTest2 = level1Data[curX1Pin2, curY1Pin2];
            }
            else if (level == levelTwo)
            {
                pathTest = level2Data[curX2Pin1, curY2Pin1];
                pathTest2 = level2Data[curX2Pin2, curY2Pin2];
            }
            if ((!isUp(pathTest) && pin1Flag) || (!isUp(pathTest2) && pin2Flag))
            {
                return;
            }
        }

        Vector3 pos = level.transform.position;
        pos.y -= 50;
        level.transform.position = new Vector3(pos.x, pos.y, pos.z);


        if (editMaze == true)
        {
            if (level == levelOne)
            {
                // Update path data value and redraw lines
                int value = level1Data[curX1Pin1, curY1Pin1];
                if (!(isUp(value)))
                {
                    level1Data[curX1Pin1, curY1Pin1] += 1;
                    //Debug.Log("X:Y:Value = " + curX1Pin1 + ":" + curY1Pin1 + ":" + level1Data[curX1Pin1, curY1Pin1]);
                }
                value = level1Data[curX1Pin1, curY1Pin1 - 1];
                if (!(isDown(value)))
                {
                    level1Data[curX1Pin1, curY1Pin1 - 1] += 4;
                    //Debug.Log("X:Y:Value = " + curX1Pin1 + ":" + (curY1Pin1 - 1) + ":" + level1Data[curX1Pin1, curY1Pin1 - 1]);
                }
                // Pin 2
                value = level1Data[curX1Pin2, curY1Pin2];
                if (!(isUp(value)))
                {
                    level1Data[curX1Pin2, curY1Pin2] += 1;
                    //Debug.Log("X:Y:Value = " + curX1Pin2 + ":" + curY1Pin2 + ":" + level1Data[curX1Pin2, curY1Pin2]);
                }
                value = level1Data[curX1Pin2, curY1Pin2 - 1];
                if (!(isDown(value)))
                {
                    level1Data[curX1Pin2, curY1Pin2 - 1] += 4;
                    //Debug.Log("X:Y:Value = " + curX1Pin2 + ":" + (curY1Pin2 - 1) + ":" + level1Data[curX1Pin2, curY1Pin2 - 1]);
                }
                BuildMazeConnections();
            }
            else if (level == levelTwo)
            {
                // Update path data value and redraw lines
                int value = level2Data[curX2Pin1, curY2Pin1];
                if (!(isUp(value)))
                {
                    level2Data[curX2Pin1, curY2Pin1] += 1;
                    //Debug.Log("X:Y:Value = " + curX2Pin1 + ":" + curY2Pin1 + ":" + level2Data[curX2Pin1, curY2Pin1]);
                }
                value = level2Data[curX2Pin1, curY2Pin1 - 1];
                if (!(isDown(value)))
                {
                    level2Data[curX2Pin1, curY2Pin1 - 1] += 4;
                    //Debug.Log("X:Y:Value = " + curX2Pin1 + ":" + (curY2Pin1 - 1) + ":" + level2Data[curX2Pin1, curY2Pin1 - 1]);
                }
                // Pin 2
                value = level2Data[curX2Pin2, curY2Pin2];
                if (!(isUp(value)))
                {
                    level2Data[curX2Pin2, curY2Pin2] += 1;
                    //Debug.Log("X:Y:Value = " + curX2Pin2 + ":" + curY2Pin2 + ":" + level2Data[curX2Pin2, curY2Pin2]);
                }
                value = level2Data[curX2Pin2, curY2Pin2 - 1];
                if (!(isDown(value)))
                {
                    level2Data[curX2Pin2, curY2Pin2 - 1] += 4;
                    //Debug.Log("X:Y:Value = " + curX2Pin2 + ":" + (curY2Pin2 - 1) + ":" + level2Data[curX2Pin2, curY2Pin2 - 1]);
                }
                BuildMazeConnections();
            }
        }
        //curY1 -= 1;
        if (level == levelOne)
        {
            if (pin1Flag)
            {
                curY1Pin1 -= 1;
            }
            if (pin2Flag)
            {
                curY1Pin2 -= 1;
            }
        }
        else if (level == levelTwo)
        {
            if (pin1Flag)
            {
                curY2Pin1 -= 1;
            }
            if (pin2Flag)
            {
                curY2Pin2 -= 1;
            }
        }
        CheckPins();
    }

    public void moveLeftOne(GameObject level)
    {
        if (isSolved)
        {
            return;
        }

        if (level == levelOne)
        {
            if (curX1Pin1 == 6 || curX1Pin2 == 6)
            {
                // Can no longer move up...
                return;
            }
        }

        if (level == levelTwo)
        {
            if (curX2Pin1 == 6 || curX2Pin2 == 6)
            {
                // Can no longer move up...
                return;
            }
        }

        if (editMaze == false)
        {
            int pathTest = 0;
            int pathTest2 = 0;

            if (level == levelOne)
            {
                pathTest = level1Data[curX1Pin1, curY1Pin1];
                pathTest2 = level1Data[curX1Pin2, curY1Pin2];
            }
            else if (level == levelTwo)
            {
                pathTest = level2Data[curX2Pin1, curY2Pin1];
                pathTest2 = level2Data[curX2Pin2, curY2Pin2];
            }
            if ((!isRight(pathTest) && pin1Flag) || (!isRight(pathTest2) && pin2Flag))
            {
                return;
            }
        }

        Vector3 pos = level.transform.position;
        pos.x -= 50;
        level.transform.position = new Vector3(pos.x, pos.y, pos.z);

        if (editMaze == true)
        {
            if (level == levelOne)
            {
                // Update path data value and redraw lines. Pin 1
                int value = level1Data[curX1Pin1, curY1Pin1];
                if (!(isRight(value)))
                {
                    level1Data[curX1Pin1, curY1Pin1] += 2;
                }
                value = level1Data[curX1Pin1 + 1, curY1Pin1];
                if (!(isLeft(value)))
                {
                    level1Data[curX1Pin1 + 1, curY1Pin1] += 8;
                }
                // Pin 2
                value = level1Data[curX1Pin2, curY1Pin2];
                if (!(isRight(value)))
                {
                    level1Data[curX1Pin2, curY1Pin2] += 2;
                }
                value = level1Data[curX1Pin2 + 1, curY1Pin2];
                if (!(isLeft(value)))
                {
                    level1Data[curX1Pin2 + 1, curY1Pin2] += 8;
                }
                BuildMazeConnections();
            }
            else if (level == levelTwo)
            {
                // Update path data value and redraw lines. Pin 1
                int value = level2Data[curX2Pin1, curY2Pin1];
                if (!(isRight(value)))
                {
                    level2Data[curX2Pin1, curY2Pin1] += 2;
                }
                value = level2Data[curX2Pin1 + 1, curY2Pin1];
                if (!(isLeft(value)))
                {
                    level2Data[curX2Pin1 + 1, curY2Pin1] += 8;
                }
                // Pin 2
                value = level2Data[curX2Pin2, curY2Pin2];
                if (!(isRight(value)))
                {
                    level2Data[curX2Pin2, curY2Pin2] += 2;
                }
                value = level2Data[curX2Pin2 + 1, curY2Pin2];
                if (!(isLeft(value)))
                {
                    level2Data[curX2Pin2 + 1, curY2Pin2] += 8;
                }
                BuildMazeConnections();
            }
        }
        //curX1 += 1;
        if (level == levelOne)
        {
            if (pin1Flag)
            {
                curX1Pin1 += 1;
            }
            if (pin2Flag)
            {
                curX1Pin2 += 1;
            }
        }
        else if (level == levelTwo)
        {
            if (pin1Flag)
            {
                curX2Pin1 += 1;
            }
            if (pin2Flag)
            {
                curX2Pin2 += 1;
            }
        }
        CheckPins();
    }

    public void moveRightOne(GameObject level)
    {
        if (isSolved)
        {
            return;
        }

        if (level == levelOne)
        {
            if (curX1Pin1 == 0 || curX1Pin2 == 0)
            {
                // Can no longer move up...
                return;
            }
        }

        if (level == levelTwo)
        {
            if (curX2Pin1 == 0 || curX2Pin2 == 0)
            {
                // Can no longer move up...
                return;
            }
        }

        if (editMaze == false)
        {
            int pathTest = 0;
            int pathTest2 = 0;

            if (level == levelOne)
            {
                pathTest = level1Data[curX1Pin1, curY1Pin1];
                pathTest2 = level1Data[curX1Pin2, curY1Pin2];
            }
            else if (level == levelTwo)
            {
                pathTest = level2Data[curX2Pin1, curY2Pin1];
                pathTest2 = level2Data[curX2Pin2, curY2Pin2];
            }
            if ((!isLeft(pathTest) && pin1Flag) || (!isLeft(pathTest2) && pin2Flag))
            {
                return;
            }
        }

        Vector3 pos = level.transform.position;
        pos.x += 50;
        level.transform.position = new Vector3(pos.x, pos.y, pos.z);

        if (editMaze == true)
        {
            if (level == levelOne)
            {
                // Update path data value and redraw lines. Pin 1.
                int value = level1Data[curX1Pin1 - 1, curY1Pin1];
                if (!(isRight(value)))
                {
                    level1Data[curX1Pin1 - 1, curY1Pin1] += 2;
                }
                value = level1Data[curX1Pin1, curY1Pin1];
                if (!(isLeft(value)))
                {
                    level1Data[curX1Pin1, curY1Pin1] += 8;
                }
                // Pin 2.
                value = level1Data[curX1Pin2 - 1, curY1Pin2];
                if (!(isRight(value)))
                {
                    level1Data[curX1Pin2 - 1, curY1Pin2] += 2;
                }
                value = level1Data[curX1Pin2, curY1Pin2];
                if (!(isLeft(value)))
                {
                    level1Data[curX1Pin2, curY1Pin2] += 8;
                }
                BuildMazeConnections();
            }
            else if (level == levelTwo)
            {
                // Update path data value and redraw lines. Pin 1.
                int value = level2Data[curX2Pin1 - 1, curY2Pin1];
                if (!(isRight(value)))
                {
                    level2Data[curX2Pin1 - 1, curY2Pin1] += 2;
                }
                value = level2Data[curX2Pin1, curY2Pin1];
                if (!(isLeft(value)))
                {
                    level2Data[curX2Pin1, curY2Pin1] += 8;
                }
                // Pin 2.
                value = level2Data[curX2Pin2 - 1, curY2Pin2];
                if (!(isRight(value)))
                {
                    level2Data[curX2Pin2 - 1, curY2Pin2] += 2;
                }
                value = level2Data[curX2Pin2, curY2Pin2];
                if (!(isLeft(value)))
                {
                    level2Data[curX2Pin2, curY2Pin2] += 8;
                }
                BuildMazeConnections();
            }
        }
        //curX1 -= 1;
        if (level == levelOne)
        {
            if (pin1Flag)
            {
                curX1Pin1 -= 1;
            }
            if (pin2Flag)
            {
                curX1Pin2 -= 1;
            }
        }
        else if (level == levelTwo)
        {
            if (pin1Flag)
            {
                curX2Pin1 -= 1;
            }
            if (pin2Flag)
            {
                curX2Pin2 -= 1;
            }
        }
        CheckPins();
    }


    public void MovePinUp(GameObject pin)
    {
        if (isSolved)
        {
            return;
        }

        if (pin == pinOne)
        {
            //Debug.Log("Pin == PinOne");
            if (!pin1Flag)
            {
                return;
            }
            if (curY1Pin1 == 0 || curY2Pin1 == 0)
            {
                // Can no longer move up...
                //Debug.Log("PinOne at Min Y");
                return;
            }
            // Pins cannot overlap or pass through each other. Checking level 1 is sufficient.
            if (pin1Flag && pin2Flag)
            {
                if (curY1Pin2 == (curY1Pin1 - 1) && curX1Pin2 == curX1Pin1)
                {
                    return;
                }
            }

        }

        if (pin == pinTwo)
        {
            //Debug.Log("Pin == PinTwo, curY1Pin2: " + curY1Pin2 + ", curY2Pin2: " + curY2Pin2);
            if (!pin2Flag)
            {
                return;
            }
            if (curY1Pin2 == 0 || curY2Pin2 == 0)
            {
                // Can no longer move up...
                //Debug.Log("PinTwo at Min Y");
                return;
            }
            // Pins cannot overlap or pass through each other. Checking level 1 is sufficient.
            if (pin1Flag && pin2Flag)
            {
                if (curY1Pin1 == (curY1Pin2 - 1) && curX1Pin1 == curX1Pin2)
                {
                    return;
                }
            }
        }


        if (editMaze == false)
        {
            int pathTest = 0;
            int pathTest2 = 0;

            if (pin == pinOne)
            {
                pathTest = level1Data[curX1Pin1, curY1Pin1];
                pathTest2 = level2Data[curX2Pin1, curY2Pin1];

            }
            else if (pin == pinTwo)
            {
                pathTest = level1Data[curX1Pin2, curY1Pin2];
                pathTest2 = level2Data[curX2Pin2, curY2Pin2];
            }

            //Debug.Log("PathTest : " + pathTest);
            //Debug.Log("PathTest2: " + pathTest2);

            if (!isUp(pathTest) || !isUp(pathTest2))
            {
                //Debug.Log("Does not pass path test!");
                return;
            }
        }

        //Debug.Log("Path available");
        Vector3 pos = pin.transform.position;
        pos.y += 50;
        pin.transform.position = new Vector3(pos.x, pos.y, pos.z);


        if (editMaze == true)
        {
            if (pin == pinOne)
            {
                // Create paths on all layers. Redraw lines.
                int value = level1Data[curX1Pin1, curY1Pin1];
                if (!(isUp(value)))
                {
                    level1Data[curX1Pin1, curY1Pin1] += 1;
                    //Debug.Log("X:Y:Value = " + curX1Pin1 + ":" + curY1Pin1 + ":" + level1Data[curX1Pin1, curY1Pin1]);
                }
                value = level1Data[curX1Pin1, curY1Pin1 - 1];
                if (!(isDown(value)))
                {
                    level1Data[curX1Pin1, curY1Pin1 - 1] += 4;
                    //Debug.Log("X:Y:Value = " + curX1Pin1 + ":" + (curY1Pin1 - 1) + ":" + level1Data[curX1Pin1, curY1Pin1 - 1]);
                }

                // Layer 2
                value = level2Data[curX2Pin1, curY2Pin1];
                if (!(isUp(value)))
                {
                    level2Data[curX2Pin1, curY2Pin1] += 1;
                    //Debug.Log("X:Y:Value = " + curX2Pin1 + ":" + curY2Pin1 + ":" + level2Data[curX2Pin1, curY2Pin1]);
                }
                value = level2Data[curX2Pin1, curY2Pin1 - 1];
                if (!(isDown(value)))
                {
                    level2Data[curX2Pin1, curY2Pin1 - 1] += 4;
                    //Debug.Log("X:Y:Value = " + curX2Pin1 + ":" + (curY2Pin1 - 1) + ":" + level2Data[curX2Pin1, curY2Pin1 - 1]);
                }
            }
            else if (pin == pinTwo)
            {
                // Create paths on all layers. Redraw lines.
                int value = level1Data[curX1Pin2, curY1Pin2];
                if (!(isUp(value)))
                {
                    level1Data[curX1Pin2, curY1Pin2] += 1;
                    //Debug.Log("X:Y:Value = " + curX1Pin2 + ":" + curY1Pin2 + ":" + level1Data[curX1Pin2, curY1Pin2]);
                }
                value = level1Data[curX1Pin2, curY1Pin2 - 1];
                if (!(isDown(value)))
                {
                    level1Data[curX1Pin2, curY1Pin2 - 1] += 4;
                    //Debug.Log("X:Y:Value = " + curX1Pin2 + ":" + (curY1Pin2 - 1) + ":" + level1Data[curX1Pin2, curY1Pin2 - 1]);
                }

                // Layer 2
                value = level2Data[curX2Pin2, curY2Pin2];
                if (!(isUp(value)))
                {
                    level2Data[curX2Pin2, curY2Pin2] += 1;
                    //Debug.Log("X:Y:Value = " + curX2Pin2 + ":" + curY2Pin2 + ":" + level2Data[curX2Pin2, curY2Pin2]);
                }
                value = level2Data[curX2Pin2, curY2Pin2 - 1];
                if (!(isDown(value)))
                {
                    level2Data[curX2Pin2, curY2Pin2 - 1] += 4;
                    //Debug.Log("X:Y:Value = " + curX2Pin2 + ":" + (curY2Pin2 - 1) + ":" + level2Data[curX2Pin2, curY2Pin2 - 1]);
                }
            }
            BuildMazeConnections();
        }

        if (pin == pinOne)
        {
            curY1Pin1 -= 1;
            curY2Pin1 -= 1;
        }
        else if (pin == pinTwo)
        {
            curY1Pin2 -= 1;
            curY2Pin2 -= 1;
        }
        CheckPins();
    }

    public void MovePinDown(GameObject pin)
    {
        if (isSolved)
        {
            return;
        }

        if (pin == pinOne)
        {
            //Debug.Log("Pin == PinOne");
            if (!pin1Flag)
            {
                return;
            }
            if (curY1Pin1 == 8 || curY2Pin1 == 8)
            {
                // Can no longer move up...
                //Debug.Log("PinOne at Max Y");
                return;
            }
            // Pins cannot overlap or pass through each other. Checking level 1 is sufficient.
            if (pin1Flag && pin2Flag)
            {
                if (curY1Pin2 == (curY1Pin1 + 1) && curX1Pin2 == curX1Pin1)
                {
                    return;
                }
            }
        }

        if (pin == pinTwo)
        {
            //Debug.Log("Pin == PinTwo");
            if (!pin2Flag)
            {
                return;
            }
            if (curY1Pin2 == 8 || curY2Pin2 == 8)
            {
                // Can no longer move up...
                //Debug.Log("PinTwo at Max Y");
                return;
            }
            // Pins cannot overlap or pass through each other. Checking level 1 is sufficient.
            if (pin1Flag && pin2Flag)
            {
                if (curY1Pin1 == (curY1Pin2 + 1) && curX1Pin1 == curX1Pin2)
                {
                    return;
                }
            }
        }


        if (editMaze == false)
        {
            int pathTest = 0;
            int pathTest2 = 0;

            if (pin == pinOne)
            {
                pathTest = level1Data[curX1Pin1, curY1Pin1];
                pathTest2 = level2Data[curX2Pin1, curY2Pin1];

            }
            else if (pin == pinTwo)
            {
                pathTest = level1Data[curX1Pin2, curY1Pin2];
                pathTest2 = level2Data[curX2Pin2, curY2Pin2];
            }

            //Debug.Log("PathTest : " + pathTest);
            //Debug.Log("PathTest2: " + pathTest2);

            if (!isDown(pathTest) || !isDown(pathTest2))
            {
                //Debug.Log("Does not pass path test!");
                return;
            }
        }

        //Debug.Log("Path available");
        Vector3 pos = pin.transform.position;
        pos.y -= 50;
        pin.transform.position = new Vector3(pos.x, pos.y, pos.z);

        if (editMaze == true)
        {
            if (pin == pinOne)
            {
                // Create paths on all layers. Redraw lines.
                int value = level1Data[curX1Pin1, curY1Pin1];
                if (!(isDown(value)))
                {
                    level1Data[curX1Pin1, curY1Pin1] += 4;
                    //Debug.Log("X:Y:Value = " + curX1Pin1 + ":" + curY1Pin1 + ":" + level1Data[curX1Pin1, curY1Pin1]);
                }
                value = level1Data[curX1Pin1, curY1Pin1 + 1];
                if (!(isUp(value)))
                {
                    level1Data[curX1Pin1, curY1Pin1 + 1] += 1;
                    //Debug.Log("X:Y:Value = " + curX1Pin1 + ":" + (curY1Pin1 + 1) + ":" + level1Data[curX1Pin1, curY1Pin1 + 1]);
                }

                // Layer 2
                value = level2Data[curX2Pin1, curY2Pin1];
                if (!(isDown(value)))
                {
                    level2Data[curX2Pin1, curY2Pin1] += 4;
                    //Debug.Log("X:Y:Value = " + curX2Pin1 + ":" + curY2Pin1 + ":" + level2Data[curX2Pin1, curY2Pin1]);
                }
                value = level2Data[curX2Pin1, curY2Pin1 + 1];
                if (!(isUp(value)))
                {
                    level2Data[curX2Pin1, curY2Pin1 + 1] += 1;
                    //Debug.Log("X:Y:Value = " + curX2Pin1 + ":" + (curY2Pin1 - 1) + ":" + level2Data[curX2Pin1, curY2Pin1 + 1]);
                }
            }
            else if (pin == pinTwo)
            {
                // Create paths on all layers. Redraw lines.
                int value = level1Data[curX1Pin2, curY1Pin2];
                if (!(isDown(value)))
                {
                    level1Data[curX1Pin2, curY1Pin2] += 4;
                    //Debug.Log("X:Y:Value = " + curX1Pin2 + ":" + curY1Pin2 + ":" + level1Data[curX1Pin2, curY1Pin2]);
                }
                value = level1Data[curX1Pin2, curY1Pin2 + 1];
                if (!(isUp(value)))
                {
                    level1Data[curX1Pin2, curY1Pin2 + 1] += 1;
                    //Debug.Log("X:Y:Value = " + curX1Pin2 + ":" + (curY1Pin2 + 1) + ":" + level1Data[curX1Pin2, curY1Pin2 + 1]);
                }

                // Layer 2
                value = level2Data[curX2Pin2, curY2Pin2];
                if (!(isDown(value)))
                {
                    level2Data[curX2Pin2, curY2Pin2] += 4;
                    //Debug.Log("X:Y:Value = " + curX2Pin2 + ":" + curY2Pin2 + ":" + level2Data[curX2Pin2, curY2Pin2]);
                }
                value = level2Data[curX2Pin2, curY2Pin2 + 1];
                if (!(isUp(value)))
                {
                    level2Data[curX2Pin2, curY2Pin2 + 1] += 1;
                    //Debug.Log("X:Y:Value = " + curX2Pin2 + ":" + (curY2Pin2 - 1) + ":" + level2Data[curX2Pin2, curY2Pin2 + 1]);
                }
            }
            BuildMazeConnections();
        }

        if (pin == pinOne)
        {
            curY1Pin1 += 1;
            curY2Pin1 += 1;
        }
        else if (pin == pinTwo)
        {
            curY1Pin2 += 1;
            curY2Pin2 += 1;
        }
        CheckPins();
    }


    public void MovePinLeft(GameObject pin)
    {
        if (isSolved)
        {
            return;
        }

        if (pin == pinOne)
        {
            //Debug.Log("Pin == PinOne");
            if (!pin1Flag)
            {
                return;
            }
            if (curX1Pin1 == 0 || curX2Pin1 == 0)
            {
                // Can no longer move up...
                //Debug.Log("PinOne at Min X");
                return;
            }
            // Pins cannot overlap or pass through each other. Checking level 1 is sufficient.
            if (pin1Flag && pin2Flag)
            {
                if (curX1Pin2 == (curX1Pin1 - 1) && curY1Pin2 == curY1Pin1)
                {
                    return;
                }
            }
        }

        if (pin == pinTwo)
        {
            //Debug.Log("Pin == PinTwo");
            if (!pin2Flag)
            {
                return;
            }
            if (curX1Pin2 == 0 || curX2Pin2 == 0)
            {
                // Can no longer move up...
                //Debug.Log("PinTwo at Min X");
                return;
            }
            // Pins cannot overlap or pass through each other. Checking level 1 is sufficient.
            if (pin1Flag && pin2Flag)
            {
                if (curX1Pin1 == (curX1Pin2 - 1) && curY1Pin1 == curY1Pin2)
                {
                    return;
                }
            }
        }

        if (editMaze == false)
        {
            int pathTest = 0;
            int pathTest2 = 0;

            if (pin == pinOne)
            {
                pathTest = level1Data[curX1Pin1, curY1Pin1];
                pathTest2 = level2Data[curX2Pin1, curY2Pin1];

            }
            else if (pin == pinTwo)
            {
                pathTest = level1Data[curX1Pin2, curY1Pin2];
                pathTest2 = level2Data[curX2Pin2, curY2Pin2];
            }

            //Debug.Log("PathTest : " + pathTest);
            //Debug.Log("PathTest2: " + pathTest2);

            if (!isLeft(pathTest) || !isLeft(pathTest2))
            {
                //Debug.Log("Does not pass path test!");
                return;
            }
        }

        //Debug.Log("Path available");
        Vector3 pos = pin.transform.position;
        pos.x -= 50;
        pin.transform.position = new Vector3(pos.x, pos.y, pos.z);


        if (editMaze == true)
        {
            if (pin == pinOne)
            {
                // Create paths on all layers. Redraw lines.
                int value = level1Data[curX1Pin1, curY1Pin1];
                if (!(isLeft(value)))
                {
                    level1Data[curX1Pin1, curY1Pin1] += 8;
                }
                value = level1Data[curX1Pin1 - 1, curY1Pin1];
                if (!(isRight(value)))
                {
                    level1Data[curX1Pin1 - 1, curY1Pin1] += 2;
                }
                //Layer 2
                value = level2Data[curX2Pin1, curY2Pin1];
                if (!(isLeft(value)))
                {
                    level2Data[curX2Pin1, curY2Pin1] += 8;
                }
                value = level2Data[curX2Pin1 - 1, curY2Pin1];
                if (!(isRight(value)))
                {
                    level2Data[curX2Pin1 - 1, curY2Pin1] += 2;
                }
            }
            else if (pin == pinTwo)
            {
                // Create paths on all layers. Redraw lines.
                int value = level1Data[curX1Pin2, curY1Pin2];
                if (!(isLeft(value)))
                {
                    level1Data[curX1Pin2, curY1Pin2] += 8;
                }
                value = level1Data[curX1Pin2 - 1, curY1Pin2];
                if (!(isRight(value)))
                {
                    level1Data[curX1Pin2 - 1, curY1Pin2] += 2;
                }
                //Layer 2
                value = level2Data[curX2Pin2, curY2Pin2];
                if (!(isLeft(value)))
                {
                    level2Data[curX2Pin2, curY2Pin2] += 8;
                }
                value = level2Data[curX2Pin2 - 1, curY2Pin2];
                if (!(isRight(value)))
                {
                    level2Data[curX2Pin2 - 1, curY2Pin2] += 2;
                }
            }
            BuildMazeConnections();
        }

        if (pin == pinOne)
        {
            curX1Pin1 -= 1;
            curX2Pin1 -= 1;
        }
        else if (pin == pinTwo)
        {
            curX1Pin2 -= 1;
            curX2Pin2 -= 1;
        }
        CheckPins();
    }


    public void MovePinRight(GameObject pin)
    {
        if (isSolved)
        {
            return;
        }

        if (pin == pinOne)
        {
            //Debug.Log("Pin == PinOne");
            if (!pin1Flag)
            {
                return;
            }
            if (curX1Pin1 == 6 || curX2Pin1 == 6)
            {
                // Can no longer move up...
                //Debug.Log("PinOne at Max X");
                return;
            }
            // Pins cannot overlap or pass through each other. Checking level 1 is sufficient.
            if (pin1Flag && pin2Flag)
            {
                if (curX1Pin2 == (curX1Pin1 + 1) && curY1Pin2 == curY1Pin1)
                {
                    return;
                }
            }
        }

        if (pin == pinTwo)
        {
            //Debug.Log("Pin == PinTwo");
            if (!pin2Flag)
            {
                return;
            }
            if (curX1Pin2 == 6 || curX2Pin2 == 6)
            {
                // Can no longer move up...
                //Debug.Log("PinTwo at Max X");
                return;
            }
            // Pins cannot overlap or pass through each other. Checking level 1 is sufficient.
            if (pin1Flag && pin2Flag)
            {
                if (curX1Pin1 == (curX1Pin2 + 1) && curY1Pin1 == curY1Pin2)
                {
                    return;
                }
            }
        }

        if (editMaze == false)
        {
            int pathTest = 0;
            int pathTest2 = 0;

            if (pin == pinOne)
            {
                pathTest = level1Data[curX1Pin1, curY1Pin1];
                pathTest2 = level2Data[curX2Pin1, curY2Pin1];

            }
            else if (pin == pinTwo)
            {
                pathTest = level1Data[curX1Pin2, curY1Pin2];
                pathTest2 = level2Data[curX2Pin2, curY2Pin2];
            }

            //Debug.Log("PathTest : " + pathTest);
            //Debug.Log("PathTest2: " + pathTest2);

            if (!isRight(pathTest) || !isRight(pathTest2))
            {
                //Debug.Log("Does not pass path test!");
                return;
            }
        }

        //Debug.Log("Path available");
        Vector3 pos = pin.transform.position;
        pos.x += 50;
        pin.transform.position = new Vector3(pos.x, pos.y, pos.z);

        if (editMaze == true)
        {
            if (pin == pinOne)
            {
                // Create paths on all layers. Redraw lines.
                int value = level1Data[curX1Pin1, curY1Pin1];
                if (!(isRight(value)))
                {
                    level1Data[curX1Pin1, curY1Pin1] += 2;
                }
                value = level1Data[curX1Pin1 + 1, curY1Pin1];
                if (!(isLeft(value)))
                {
                    level1Data[curX1Pin1 + 1, curY1Pin1] += 8;
                }
                //Layer 2
                value = level2Data[curX2Pin1, curY2Pin1];
                if (!(isRight(value)))
                {
                    level2Data[curX2Pin1, curY2Pin1] += 2;
                }
                value = level2Data[curX2Pin1 + 1, curY2Pin1];
                if (!(isLeft(value)))
                {
                    level2Data[curX2Pin1 + 1, curY2Pin1] += 8;
                }
            }
            else if (pin == pinTwo)
            {
                // Create paths on all layers. Redraw lines.
                int value = level1Data[curX1Pin2, curY1Pin2];
                if (!(isRight(value)))
                {
                    level1Data[curX1Pin2, curY1Pin2] += 2;
                }
                value = level1Data[curX1Pin2 + 1, curY1Pin2];
                if (!(isLeft(value)))
                {
                    level1Data[curX1Pin2 + 1, curY1Pin2] += 8;
                }
                //Layer 2
                value = level2Data[curX2Pin2, curY2Pin2];
                if (!(isRight(value)))
                {
                    level2Data[curX2Pin2, curY2Pin2] += 2;
                }
                value = level2Data[curX2Pin2 + 1, curY2Pin2];
                if (!(isLeft(value)))
                {
                    level2Data[curX2Pin2 + 1, curY2Pin2] += 8;
                }
            }
            BuildMazeConnections();
        }

        if (pin == pinOne)
        {
            curX1Pin1 += 1;
            curX2Pin1 += 1;
        }
        else if (pin == pinTwo)
        {
            curX1Pin2 += 1;
            curX2Pin2 += 1;
        }
        CheckPins();
    }



    private bool isUp(int value)
    {
        int result = value & 1;
        if (result > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool isRight(int value)
    {
        int result = value & 2;
        if (result > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool isDown(int value)
    {
        int result = value & 4;
        if (result > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool isLeft(int value)
    {
        int result = value & 8;
        if (result > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void FillLevel1Data()
    {
        int index = -1;

        // Horizontal Lines
        for (int i = 0; i < MAXHEIGHT; i++)
        {
            for (int j = 0; j < MAXWIDTH; j++)
                {
                    index++;
                level1Data[j, i] = data1[index];
            }
        }
    }

    private void FillLevel2Data()
    {
        int index = -1;

        // Horizontal Lines
        for (int i = 0; i < MAXHEIGHT; i++)
        {
            for (int j = 0; j < MAXWIDTH; j++)
            {
                index++;
                level2Data[j, i] = data2[index];
            }
        }
    }

    private void BuildMazeConnections()
    {
        int value = 0;
        int index = -1;

        // Horizontal Lines
        for(int i=0; i< MAXHEIGHT; i++)
        {
            for(int j=0; j<MAXWIDTH-1; j++)
            {
                index++;
                value = level1Data[j, i];

                // Test for links and add them if they should exist...
                if(isRight(value))
                {
                    // Draw horizontal line at current index
                    Lvl1HLines[index].SetActive(true);

                }
                value = level2Data[j, i];

                // Test for links and add them if they should exist...
                if (isRight(value))
                {
                    // Draw horizontal line at current index
                    Lvl2HLines[index].SetActive(true);

                }
            }
        }

        // Vertical Lines
        index = -1;
        for (int i = 1; i < MAXHEIGHT; i++)
        {
            for (int j = 0; j < MAXWIDTH; j++)
            {
                index++;
                value = level1Data[j, i];
        
                // Test for links and add them if they should exist...
                if (isUp(value))
                {
                    // Draw horizontal line at current index
                    Lvl1VLines[index].SetActive(true);
        
                }
                value = level2Data[j, i];

                // Test for links and add them if they should exist...
                if (isUp(value))
                {
                    // Draw horizontal line at current index
                    Lvl2VLines[index].SetActive(true);

                }
            }
        }
        SaveLayerData();
    }


    public void SaveLayerData()
    {

        if (editMaze == false)
        {
            return;
        }
        string data= "";
        string data1 = "";
        string data2 = "";
        int value1;
        int value2;
        for (int i = 0; i < MAXHEIGHT; i++)
        {
            for (int j = 0; j < MAXWIDTH; j++)
            {
                value1 = level1Data[j, i];
                value2 = level2Data[j, i];
                data1 += value1 + ", ";
                data2 += value2 + ", ";
            }
        }

        data = "Level 1:\n" + data1 + "\nLevel 2:\n" + data2;
        File.WriteAllText(Application.dataPath + "/ufoData1.txt", data);

    }


    private void BuildAllConnections()
    {
        int value = 0;
        int index = -1;

        // Horizontal Lines
        for (int i = 0; i < MAXHEIGHT; i++)
        {
            for (int j = 0; j < MAXWIDTH - 1; j++)
            {
                index++;
                value = level1Data[j, i];

                // Test for links and add them if they should exist...
                if (isRight(value))
                {
                    // Draw horizontal line at current index
                    Lvl1HLines[index].SetActive(true);

                }
                value = level2Data[j, i];

                // Test for links and add them if they should exist...
                if (isRight(value))
                {
                    // Draw horizontal line at current index
                    Lvl2HLines[index].SetActive(true);

                }
            }
        }

        // Vertical Lines
        index = -1;
        for (int i = 1; i < MAXHEIGHT; i++)
        {
            for (int j = 0; j < MAXWIDTH; j++)
            {
                index++;
                value = level1Data[j, i];

                // Test for links and add them if they should exist...
                if (isUp(value))
                {
                    // Draw horizontal line at current index
                    Lvl1VLines[index].SetActive(true);

                }
                value = level2Data[j, i];

                // Test for links and add them if they should exist...
                if (isUp(value))
                {
                    // Draw horizontal line at current index
                    Lvl2VLines[index].SetActive(true);

                }
            }
        }
    }

    void CheckPins()
    {
        CheckPinDrops(pinOne);
        CheckPinDrops(pinTwo);

        if (!pin1Flag && !pin2Flag)
        {
            // Puzzle is complete...
            //Debug.Log("Puzzle Solved!");
            isSolved = true;
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
            if (test < 9)
            {
                PlayerPrefs.SetInt(str, 9);
                PlayerPrefs.Save();
            }

            // TODO: Possibly tween out the components, then call for next puzzle...
        }
    }


    void CheckPinDrops(GameObject pin)
    {
        if (pin == pinOne)
        {
            if (curX1Pin1 == 3 && curY1Pin1 == 4 && curX2Pin1 == 3 && curY2Pin1 == 4)
            {
                // tween pin scale to zero and set flag to ignore pin 1 (need DOTween for this)
                if (pin1Flag && pin2Flag)
                {
                    // Only play on first pin drop
                    m_audioMngr.GetComponent<AudioManager>().PlayPartialSuccess();
                }
                pin1Flag = false;
                pinOne.transform.DOScale(Vector3.zero, 2f)
                            .SetEase(Ease.Linear);
            }

        }
        else if (pin == pinTwo)
        {
            if (curX1Pin2 == 3 && curY1Pin2 == 4 && curX2Pin2 == 3 && curY2Pin2 == 4)
            {
                // tween pin scale to zero and set flag to ignore pin 2 (need DOTween for this)
                if (pin1Flag && pin2Flag)
                {
                    // Only play on first pin drop
                    m_audioMngr.GetComponent<AudioManager>().PlayPartialSuccess();
                }
                pin2Flag = false;
                pinTwo.transform.DOScale(Vector3.zero, 2f)
                            .SetEase(Ease.Linear);
            }
        }
    }

public void SetupMaze()
    {
        startBtn.SetActive(false);

        // Setup puzzle based on difficulty...
        difficultyLevel = PlayerPrefs.GetInt("difficultyLevel", 0); // Do I need the default value anymore??
        //Debug.Log("Diff Level: " + difficultyLevel);
        if (difficultyLevel == 1)
        {
            // Use harder data set
            data1 = data1Hard;
            data2 = data2Hard;
        }
        else
        {
            // Use easier data set
            data1 = data1Easy;
            data2 = data2Easy;
        }

        ClearMaze();

        Vector3 pin1Pos = pinOne.transform.position;
        pin1Pos += new Vector3(-2 * 50, 3 * 50, 0);
        Vector3 pin2Pos = pinTwo.transform.position;
        pin2Pos += new Vector3(2 * 50, -3 * 50, 0);

        pinOne.transform.position = new Vector3(pin1Pos.x, pin1Pos.y, pin1Pos.z);
        pinTwo.transform.position = new Vector3(pin2Pos.x, pin2Pos.y, pin2Pos.z);


        curPos = levelOne.transform.position;

        pin1Flag = true;
        pin2Flag = true;

        FillLevel1Data();
        FillLevel2Data();

        BuildMazeConnections();


        StartCoroutine("OverlayFadeToInactive");
    }


    void ClearMaze()
    {
        for (int i = 0; i < Lvl1HLines.Length; i++)
        {
            Lvl1HLines[i].SetActive(false);
        }
        for (int i = 0; i < Lvl1VLines.Length; i++)
        {
            Lvl1VLines[i].SetActive(false);
        }
        for (int i = 0; i < Lvl2HLines.Length; i++)
        {
            Lvl2HLines[i].SetActive(false);
        }
        for (int i = 0; i < Lvl2VLines.Length; i++)
        {
            Lvl2VLines[i].SetActive(false);
        }

    }


    IEnumerator OverlayFadeToInactive()
    {
        overlay.GetComponent<Image>().DOFade(0, 0.5f);

        yield return new WaitForSeconds(0.6f);

        overlay.SetActive(false);

        yield return null;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
