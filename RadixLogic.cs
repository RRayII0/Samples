using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RadixLogic : MonoBehaviour
{
    public GameObject[] m_SmallDropsWhite;
    public GameObject[] m_LargeDropsWhite;
    public GameObject[] m_ConvertDropsWhite;
    public GameObject[] m_SmallDropsGreen;
    public GameObject[] m_LargeDropsGreen;
    public GameObject[] m_ConvertDropsGreen;
    public GameObject[] m_SmallDropsPurple;
    public GameObject[] m_LargeDropsPurple;
    public GameObject[] m_ConvertDropsPurple;
    public GameObject[] m_TopDigits;
    public GameObject[] m_MidDigits;
    public GameObject[] m_BottomDigits;
    public GameObject m_TopLight;
    public GameObject m_MidLight;
    public GameObject m_BottomLight;
    public Sprite m_redLight;
    public Sprite m_yellowLight;
    public Sprite m_greenLight;
    public GameObject m_NextBtn;
    public GameObject m_StartBtn;       // RDR NOTE: Not using for now since EASY and HARD modes use same code... (could lower TOTAL_SUM value from 170 to about 100, if necessary)
    public GameObject m_icon;
    public GameObject m_audioMngr;

    private static char alpha = 'α';
    private static char beta = 'β';
    private static char gamma = 'γ';
    private static char delta = 'δ';
    private static char epsilon = 'ε';
    private static char zeta = 'ζ';
    private static char eta = 'η';
    //private static char theta = 'θ';  // too much like 0
    //private static char iota = 'ι';   // too much like 1
    private static char kappa = 'κ';
    private static char lamda = 'λ';
    private static char mu = 'μ';
    private static char nu = 'ν';
    private char[] digit = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', alpha, beta, gamma, delta, epsilon, zeta, eta, kappa, lamda, mu, nu };

    private int[,] powerRanges = new int[22, 2] {  {0,    0},       // 0
                                                   {1,    1},       // 1
                                                   {8,    16},      // 2
                                                   {27,   81},      // 3
                                                   {64,   256},     // 4
                                                   {125,  625},     // 5
                                                   {216,  1296},    // 6
                                                   {343,  2401},    // 7
                                                   {512,  4096},    // 8
                                                   {729,  6561},    // 9
                                                   {1000, 10000},   // 10
                                                   {1331, 14641},   // 11
                                                   {1728, 20736},   // 12
                                                   {2197, 28561},   // 13
                                                   {2744, 38416},   // 14
                                                   {3375, 50625},   // 15
                                                   {4096, 65536},   // 16
                                                   {4913, 83521},   // 17
                                                   {5832, 104976},  // 18
                                                   {6859, 130321},  // 19
                                                   {8000, 160000},  // 20
                                                   {9261, 194481}  };   // 21

    private char[] m_TopSolve = new char[4];
    private char[] m_MidSolve = new char[4];
    private char[] m_BottomSolve = new char[4];
    private bool m_IsPuzzleSolved = false;

    private int TOTAL_SUM = 170;

    // min (radix^3 -1), max (radix^4 - 1)
    private int[] m_radixMins  = { -1, 0, 7, 26,  };
    private int[] m_radixMaxes = { };

    private int m_WhiteValue;
    private int m_GreenValue;
    private int m_PurpleValue;

    private int m_WhiteConvert;
    private int m_GreenConvert;
    private int m_PurpleConvert;

    private int m_WhiteLargeDrops;
    private int m_WhiteSmallDrops;
    private int m_GreenLargeDrops;
    private int m_GreenSmallDrops;
    private int m_PurpleLargeDrops;
    private int m_PurpleSmallDrops;

    private int m_TopTarget;
    private int m_MidTarget;
    private int m_BottomTarget;

    private int m_TopRadix;
    private int m_MidRadix;
    private int m_BottomRadix;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Alpha: " + alpha);

        // Use same code paths for both difficulty settings...
        //SetupPuzzle();
        //Restart();
    }

    public void Restart()
    {
        //Debug.Log("RadixLogic Restart() called...");
        int difficulty = PlayerPrefs.GetInt("difficultyLevel");
        string str = "Easy";
        if (difficulty > 0)
        {
            str = "Hard";
        }
        // Set level at which puzzles have been solved.
        int test = PlayerPrefs.GetInt(str);
        if (test >= 5)
        {
            m_icon.GetComponent<IconReveal>().RevealIcon();
            m_NextBtn.SetActive(true);
        }

        foreach (GameObject go in m_TopDigits)
        {
            go.GetComponent<DisplayRadixValue>().ResetDigit();
        }
        foreach (GameObject go in m_MidDigits)
        {
            go.GetComponent<DisplayRadixValue>().ResetDigit();
        }
        foreach (GameObject go in m_BottomDigits)
        {
            go.GetComponent<DisplayRadixValue>().ResetDigit();
        }


        TurnOnAllGameObjects();
        SetupPuzzle();
    }

    private void TurnOnAllGameObjects()
    {
        m_TopLight.GetComponent<Image>().sprite = m_redLight;
        m_MidLight.GetComponent<Image>().sprite = m_redLight;
        m_BottomLight.GetComponent<Image>().sprite = m_redLight;


        for (int i = 0; i < m_SmallDropsWhite.Length; i++)
        {
            m_SmallDropsWhite[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < m_LargeDropsWhite.Length; i++)
        {
            m_LargeDropsWhite[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < m_ConvertDropsWhite.Length; i++)
        {
            m_ConvertDropsWhite[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < m_SmallDropsGreen.Length; i++)
        {
            m_SmallDropsGreen[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < m_LargeDropsGreen.Length; i++)
        {
            m_LargeDropsGreen[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < m_ConvertDropsGreen.Length; i++)
        {
            m_ConvertDropsGreen[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < m_SmallDropsPurple.Length; i++)
        {
            m_SmallDropsPurple[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < m_LargeDropsPurple.Length; i++)
        {
            m_LargeDropsPurple[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < m_ConvertDropsPurple.Length; i++)
        {
            m_ConvertDropsPurple[i].gameObject.SetActive(true);
        }
    }


    public void ShutdownCanvas()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private char GetRadixChar(int radix)
    {
        return digit[radix];
    }

    public char ResetDigit()
    {
        return digit[0];
    }

    public char GetNextDigit(char curDigit, int radix)
    {
        if (m_IsPuzzleSolved == true)
        {
            return curDigit;
        }
        // Find index of curDigit
        // return index+1 value (return index = 0, if at maxDigit)
        int idx = Array.IndexOf(digit, curDigit);
        //Debug.Log("Old Index = " + idx);
        if (idx >= 0)
        {
            if (idx+1 >= radix)
            {
                idx = 0;
            }
            else
            {
                idx++;
            }

            return (digit[idx]);


        }



        return curDigit;
    }

    public char GetPrevDigit(char curDigit, int radix)
    {
        if (m_IsPuzzleSolved == true)
        {
            return curDigit;
        }
        // Find index of curDigit
        // return index-1 value (return index = 0radix-1, if at 0)
        int idx = Array.IndexOf(digit, curDigit);
        //Debug.Log("Old Index = " + idx);
        if (idx >= 0)
        {
            if (idx - 1 < 0)
            {
                idx = radix-1;
            }
            else
            {
                idx--;
            }

            return (digit[idx]);


        }



        return curDigit;
    }

    private int GetBestRadix(int target)
    {
        for (int i = 2; i < powerRanges.GetLength(0); i++)
        {
            if (powerRanges[i,1] > (target - 1))
            {
                return i;
            }
        }

        return -1;
    }

    private int GetBestConvert(int value)
    {
        int convertValue = 10;
        int testValue;
        int curItems = 0;
        int curLarge = 0;
        int curSmall = 0;


        if (value <= 20)
        {
            testValue = 10;
            while(testValue > 2)
            {
                curLarge = value / testValue;
                curSmall = value % testValue;
                if (curLarge + curSmall >= curItems)
                {
                    curItems = curLarge + curSmall;
                    convertValue = testValue;
                }
                testValue--;
            }
        }
        else
        {
            testValue = 16;
            while (testValue > 2)
            {
                curLarge = (value-20) / testValue;
                if (curLarge > 8)
                {
                    testValue = 2;
                    break;
                }
                curSmall = (value-20) % testValue;
                if (curLarge + curSmall >= curItems)
                {
                    curItems = curLarge + curSmall;
                    convertValue = testValue;
                }
                testValue--;
            }
        }

        return convertValue;
    }


    void DisplayConverts()
    {
        //m_WhiteConvert
        for (int i = m_WhiteConvert; i < m_ConvertDropsWhite.Length; i++)
        {
            m_ConvertDropsWhite[i].gameObject.SetActive(false);
        }
        for (int i = m_GreenConvert; i < m_ConvertDropsGreen.Length; i++)
        {
            m_ConvertDropsGreen[i].gameObject.SetActive(false);
        }
        for (int i = m_PurpleConvert; i < m_ConvertDropsPurple.Length; i++)
        {
            m_ConvertDropsPurple[i].gameObject.SetActive(false);
        }
    }

    void DisplayContainerDrops()
    {
        int largeDrops;
        int smallDrops;

        if (m_WhiteValue > 20)
        {
            largeDrops = ((m_WhiteValue - 20) / m_WhiteConvert) + 1;
            smallDrops = m_WhiteValue - (largeDrops * m_WhiteConvert);
        }
        else
        {
            largeDrops = m_WhiteValue / m_WhiteConvert;
            smallDrops = m_WhiteValue - (largeDrops * m_WhiteConvert);
        }
        // Display Container Drops
        for (int i = largeDrops; i < m_LargeDropsWhite.Length; i++)
        {
            m_LargeDropsWhite[i].gameObject.SetActive(false);
        }
        for (int i = smallDrops; i < m_SmallDropsWhite.Length; i++)
        {
            m_SmallDropsWhite[i].gameObject.SetActive(false);
        }
        
        if (m_GreenValue > 20)
        {
            largeDrops = ((m_GreenValue - 20) / m_GreenConvert) + 1;
            smallDrops = m_GreenValue - (largeDrops * m_GreenConvert);
        }
        else
        {
            largeDrops = m_GreenValue / m_GreenConvert;
            smallDrops = m_GreenValue - (largeDrops * m_GreenConvert);
        }
        // Display Container Drops
        for (int i = largeDrops; i < m_LargeDropsGreen.Length; i++)
        {
            m_LargeDropsGreen[i].gameObject.SetActive(false);
        }
        for (int i = smallDrops; i < m_SmallDropsGreen.Length; i++)
        {
            m_SmallDropsGreen[i].gameObject.SetActive(false);
        }
        
        if (m_PurpleValue > 20)
        {
            largeDrops = ((m_PurpleValue - 20) / m_PurpleConvert) + 1;
            smallDrops = m_PurpleValue - (largeDrops * m_PurpleConvert);
        }
        else
        {
            largeDrops = m_PurpleValue / m_PurpleConvert;
            smallDrops = m_PurpleValue - (largeDrops * m_PurpleConvert);
        }
        // Display Container Drops
        for (int i = largeDrops; i < m_LargeDropsPurple.Length; i++)
        {
            m_LargeDropsPurple[i].gameObject.SetActive(false);
        }
        for (int i = smallDrops; i < m_SmallDropsPurple.Length; i++)
        {
            m_SmallDropsPurple[i].gameObject.SetActive(false);
        }
    }

    void SetDigitCounters()
    {
        // SetMaxDigitChar(char) for each counter (top, mid, bottom)
        foreach (GameObject go in m_TopDigits)
        {
            go.GetComponent<DisplayRadixValue>().ResetInit();
            go.GetComponent<DisplayRadixValue>().SetMaxDigitChar(m_TopRadix);
        }
        foreach (GameObject go in m_MidDigits)
        {
            go.GetComponent<DisplayRadixValue>().ResetInit();
            go.GetComponent<DisplayRadixValue>().SetMaxDigitChar(m_MidRadix);
        }
        foreach (GameObject go in m_BottomDigits)
        {
            go.GetComponent<DisplayRadixValue>().ResetInit();
            go.GetComponent<DisplayRadixValue>().SetMaxDigitChar(m_BottomRadix);
        }
    }

    public void ResolveRadixDigits()
    {
        int remainder;
        int unit;

        // What digit chars will solve the puzzle? Need selected value and selected radix for top, mid and Bottom dollars.
        // m_TopSolve[] m_MidSolve[] m_BottomSolve[] 
        // m_TopTarget, m_MidTarget, m_BottomTarget ... white, white * green, white * green * purple
        // m_TopRadix

        remainder = m_TopTarget;
        unit = remainder / powerRanges[m_TopRadix, 0];
        m_TopSolve[3] = digit[unit];

        remainder -= unit * powerRanges[m_TopRadix, 0];
        unit = remainder / (m_TopRadix * m_TopRadix);
        m_TopSolve[2] = digit[unit];

        remainder -= unit * (m_TopRadix * m_TopRadix);
        unit = remainder / m_TopRadix;
        m_TopSolve[1] = digit[unit];

        remainder -= unit * m_TopRadix;
        unit = remainder;
        m_TopSolve[0] = digit[unit];
        //Debug.Log("[ " + m_TopSolve[3] + " ][ " + m_TopSolve[2] + " ][ " + m_TopSolve[1] + " ][ " + m_TopSolve[0] + " ]");

        remainder = m_MidTarget;
        unit = remainder / powerRanges[m_MidRadix, 0];
        m_MidSolve[3] = digit[unit];

        remainder -= unit * powerRanges[m_MidRadix, 0];
        unit = remainder / (m_MidRadix * m_MidRadix);
        m_MidSolve[2] = digit[unit];

        remainder -= unit * (m_MidRadix * m_MidRadix);
        unit = remainder / m_MidRadix;
        m_MidSolve[1] = digit[unit];

        remainder -= unit * m_MidRadix;
        unit = remainder;
        m_MidSolve[0] = digit[unit];
        //Debug.Log("[ " + m_MidSolve[3] + " ][ " + m_MidSolve[2] + " ][ " + m_MidSolve[1] + " ][ " + m_MidSolve[0] + " ]");

        remainder = m_BottomTarget;
        unit = remainder / powerRanges[m_BottomRadix, 0];
        m_BottomSolve[3] = digit[unit];

        remainder -= unit * powerRanges[m_BottomRadix, 0];
        unit = remainder / (m_BottomRadix * m_BottomRadix);
        m_BottomSolve[2] = digit[unit];

        remainder -= unit * (m_BottomRadix * m_BottomRadix);
        unit = remainder / m_BottomRadix;
        m_BottomSolve[1] = digit[unit];

        remainder -= unit * m_BottomRadix;
        unit = remainder;
        m_BottomSolve[0] = digit[unit];
        //Debug.Log("[ " + m_BottomSolve[3] + " ][ " + m_BottomSolve[2] + " ][ " + m_BottomSolve[1] + " ][ " + m_BottomSolve[0] + " ]");

    }


    public void EvaluateCounters()
    {
        bool testTopSolved;
        bool testMidSolved;
        bool testBottomSolved;

        if (m_IsPuzzleSolved == true)
        {
            // Puzzle is solved, go to next puzzle...
            return;
        }

        testTopSolved = EvaluateTopCounters();
        if (testTopSolved == true)
        {
            m_TopLight.GetComponent<Image>().sprite = m_greenLight;
        }
        testMidSolved = EvaluateMidCounters();
        if (testMidSolved == true)
        {
            m_MidLight.GetComponent<Image>().sprite = m_greenLight;
        }
        testBottomSolved = EvaluateBottomCounters();
        if (testBottomSolved == true)
        {
            m_BottomLight.GetComponent<Image>().sprite = m_greenLight;
        }

        if (testTopSolved == true && testMidSolved == true && testBottomSolved == true)
        {
            // All three sets of digits are correct... halt logic and flash green lights...
            m_IsPuzzleSolved = true;
            m_NextBtn.SetActive(true);
            m_icon.GetComponent<IconReveal>().RevealIcon();
            m_audioMngr.GetComponent<AudioManager>().PlaySuccess();
            //Debug.Log("PUZZLE\n     SOLVED !!");

            string str = "Easy";
            int difficultyLevel = PlayerPrefs.GetInt("difficultyLevel");
            if (difficultyLevel > 0)
            {
                str = "Hard";
            }
            // Set level at which puzzles have been solved.
            int test = PlayerPrefs.GetInt(str);
            if (test < 5)
            {
                PlayerPrefs.SetInt(str, 5);
                PlayerPrefs.Save();
            }
        }

    }

    private bool EvaluateTopCounters()
    {
        // Decide what color lighst should be. Check if all lights are green. If so freeze puzzle and flash lights for the solve!
        m_TopLight.GetComponent<Image>().sprite = m_redLight;
        for (int i = 0; i < m_TopSolve.Length; i++)
        {
            if (m_TopDigits[i].GetComponent<Text>().text[0] != '0')
            {
                // If any value is not zero, change to yellow light.
                m_TopLight.GetComponent<Image>().sprite = m_yellowLight;
            }
        }
        for (int i = 0; i < m_TopSolve.Length; i++)
        {
            if (m_TopDigits[i].GetComponent<Text>().text[0] != m_TopSolve[i])
            {
                return false; ;
            }
        }
        return true;
    }
    private bool EvaluateMidCounters()
    {
        // Decide what color lighst should be. Check if all lights are green. If so freeze puzzle and flash lights for the solve!
        m_MidLight.GetComponent<Image>().sprite = m_redLight;
        for (int i = 0; i < m_MidSolve.Length; i++)
        {
            if (m_MidDigits[i].GetComponent<Text>().text[0] != '0')
            {
                // If any value is not zero, change to yellow light.
                m_MidLight.GetComponent<Image>().sprite = m_yellowLight;
            }
        }
        for (int i = 0; i < m_MidSolve.Length; i++)
        {
            if (m_MidDigits[i].GetComponent<Text>().text[0] != m_MidSolve[i])
            {
                return false;
            }
        }
        return true;
    }

    private bool EvaluateBottomCounters()
    {
        // Decide what color lighst should be. Check if all lights are green. If so freeze puzzle and flash lights for the solve!
        m_BottomLight.GetComponent<Image>().sprite = m_redLight;
        for (int i = 0; i < m_BottomSolve.Length; i++)
        {
            if (m_BottomDigits[i].GetComponent<Text>().text[0] != '0')
            {
                // If any value is not zero, change to yellow light.
                m_BottomLight.GetComponent<Image>().sprite = m_yellowLight;
            }
        }
        for (int i = 0; i < m_BottomSolve.Length; i++)
        {
            if (m_BottomDigits[i].GetComponent<Text>().text[0] != m_BottomSolve[i])
            {
                return false;
            }
        }
        return true;
    }


    public void SetupPuzzle()
    {
        m_IsPuzzleSolved = false;

        int workingSum = TOTAL_SUM;
        int maxValue = (int)(workingSum * 0.5f);
        int minValue = (int)(workingSum * 0.1f);
        //Debug.Log("Purple Min: " + minValue + ", Max: " + maxValue);
        m_PurpleValue = UnityEngine.Random.Range(minValue, maxValue);
        workingSum -= m_PurpleValue;
        maxValue = (int)(workingSum * 0.5f);
        minValue = (int)(workingSum * 0.1f);
        //Debug.Log("Green Min: " + minValue + ", Max: " + maxValue);
        m_GreenValue = UnityEngine.Random.Range(minValue, maxValue);
        workingSum -= m_GreenValue;
        maxValue = (int)(workingSum * 0.9f);
        minValue = (int)(workingSum * 0.1f);
        //Debug.Log("White Min: " + minValue + ", Max: " + maxValue);
        m_WhiteValue = UnityEngine.Random.Range(minValue, maxValue);

        //Debug.Log("Purple(Bottom) Value: " + m_PurpleValue + ", Green(Mid) Value: " + m_GreenValue + ", White(Top) Value: " + m_WhiteValue);

        m_TopTarget = m_WhiteValue;
        m_MidTarget = m_WhiteValue * m_GreenValue;
        m_BottomTarget = m_WhiteValue * m_GreenValue * m_PurpleValue;
        //Debug.Log("Top Target: " + m_TopTarget);
        //Debug.Log("Mid Target: " + m_MidTarget);
        //Debug.Log("Bottom Target: " + m_BottomTarget);

        // Need radix of white, green and purple unit counters
        m_TopRadix = GetBestRadix(m_TopTarget);
        m_MidRadix = GetBestRadix(m_MidTarget);
        m_BottomRadix = GetBestRadix(m_BottomTarget);
        //Debug.Log("Top Radix: " + m_TopRadix);
        //Debug.Log("Mid Radix: " + m_MidRadix);
        //Debug.Log("Bottom Radix: " + m_BottomRadix);

        // Need # of drops for each large drop (white, green, purple) AND turn on in display
        m_WhiteConvert = GetBestConvert(m_WhiteValue);
        m_GreenConvert  = GetBestConvert(m_GreenValue);
        m_PurpleConvert = GetBestConvert(m_PurpleValue);
        //Debug.Log("Top Drop Convert: " + m_WhiteConvert);
        //Debug.Log("Mid Drop Convert: " + m_GreenConvert);
        //Debug.Log("Bottom Drop Convert: " + m_PurpleConvert);
        DisplayConverts();


        // Setup what drops to display (white, green, purple)
        DisplayContainerDrops();

        SetDigitCounters();

        ResolveRadixDigits();

    }
}
