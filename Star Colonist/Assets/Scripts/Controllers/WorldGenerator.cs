using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    string[] syllable1;
    string[] syllable2;
    string[] word1;
    string[] word2;
    string planetName;

    public static string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999)) throw new System.ArgumentOutOfRangeException("wrong value");
        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        throw new System.ArgumentOutOfRangeException("something bad happened");
    }

    public void RandomName()
    {
        int rnd1;
        int rnd2;
        int rnd3;
        int rnd4;
        int rndMethod = Random.Range(0, 8);
        TextAsset syllable1Asset = (TextAsset)Resources.Load("World/Syllable1");
        TextAsset syllable2Asset = (TextAsset)Resources.Load("World/Syllable2");
        TextAsset word1Asset = (TextAsset)Resources.Load("World/Word1");
        TextAsset word2Asset = (TextAsset)Resources.Load("World/Word2");
        syllable1 = syllable1Asset.text.Split("\n"[0]);
        syllable2 = syllable2Asset.text.Split("\n"[0]);
        word1 = word1Asset.text.Split("\n"[0]);
        word2 = word2Asset.text.Split("\n"[0]);

        switch (rndMethod)
        {
            case 0:
                rnd1 = Random.Range(0, syllable1.Length);
                rnd2 = Random.Range(0, syllable2.Length);
                planetName = syllable1[rnd1] + syllable2[rnd2];
                break;
            case 1:
                rnd1 = Random.Range(0, syllable1.Length);
                rnd2 = Random.Range(0, syllable2.Length);
                rnd3 = Random.Range(0, word2.Length);
                planetName = syllable1[rnd1] + syllable2[rnd2] + " " + word2[rnd3];
                break;
            case 2:
                rnd1 = Random.Range(0, word1.Length);
                rnd2 = Random.Range(0, syllable1.Length);
                rnd3 = Random.Range(0, syllable2.Length);
                planetName = word1[rnd1] + " " + syllable1[rnd2] + syllable2[rnd3];
                break;
            case 3:
                rnd1 = Random.Range(0, word1.Length);
                rnd2 = Random.Range(0, word2.Length);
                planetName = word1[rnd1] + " " + word2[rnd2];
                break;
            case 4:
                rnd1 = Random.Range(0, syllable1.Length);
                rnd2 = Random.Range(0, syllable2.Length);
                rnd3 = Random.Range(0, syllable1.Length);
                rnd4 = Random.Range(0, syllable2.Length);
                planetName = syllable1[rnd1] + syllable2[rnd2] + " " + syllable1[rnd3] + syllable2[rnd4];
                break;
            case 5:
                rnd1 = Random.Range(0, syllable1.Length);
                rnd2 = Random.Range(0, syllable2.Length);
                planetName = syllable1[rnd1] + "'" + syllable2[rnd2];
                break;
            case 6:
                rnd1 = Random.Range(0, syllable1.Length);
                rnd2 = Random.Range(0, syllable2.Length);
                rnd3 = Random.Range(0, syllable2.Length);
                planetName = syllable1[rnd1] + syllable2[rnd2] + "'" + syllable2[rnd3];
                break;
            case 7:
                rnd1 = Random.Range(0, syllable1.Length);
                rnd2 = Random.Range(0, syllable2.Length);
                rnd3 = Random.Range(0, syllable1.Length);
                rnd4 = Random.Range(0, syllable2.Length);
                planetName = syllable1[rnd1] + syllable2[rnd2] + "-" + syllable1[rnd3] + syllable2[rnd4];
                break;
            default:
                Debug.Log("Wrong method, please regenerate world");
                return;
        }

        int numChance = Random.Range(0, 100);

        if(numChance < 80)
        {
            Debug.Log("World name is " + planetName);
        }
        else
        {
            int numType = Random.Range(0, 100); //roll for Roman or Arab number

            if(numType < 50) //Arab
            {
                int num = Random.Range(0, 9);
                Debug.Log("World name is " + planetName + " " + num);
            }
            else //Roman
            {
                int num = Random.Range(0, 3999);
                Debug.Log("World name is " + planetName + " " + ToRoman(num));
            }
        }
    }
}