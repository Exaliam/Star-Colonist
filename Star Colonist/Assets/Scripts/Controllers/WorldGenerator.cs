﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject planetPrefab;

    GameObject thisPlanet;
    WorldManager worldManager;

    string[] syllable1;
    string[] syllable2;
    string[] word1;
    string[] word2;
    string planetName;
    int nBiomes;
    int nFlora;
    int nFauna;

    public void CreateWorld()
    {
        WorldName();
        worldManager = Instantiate(planetPrefab).GetComponent<WorldManager>();
        WorldBiomes();
        WorldFlora();
        WorldFauna();
        Debug.Log(planetName + " is a planet with " + nBiomes + " different biomes, inhabitated by " + nFlora + " flora species and " + nFauna + " creatures.");
    }

    void WorldName()
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

        if(numChance > 80)
        {
            int numType = Random.Range(0, 100); //roll for Roman or Arab number

            if(numType < 50) //Arab
            {
                int num = Random.Range(0, 9);
                planetName = planetName + " " + num;
            }
            else //Roman
            {
                int num = Random.Range(0, 3999);
                planetName = planetName + " " + GameManager.ToRoman(num);
            }
        }
    }

    void WorldBiomes()
    {
        nBiomes = Random.Range(2, 10);
    }

    void WorldFlora()
    {
        nFlora = Random.Range(2, 10) * nBiomes;
    }

    void WorldFauna()
    {
        nFauna = Random.Range(2, 10) * nBiomes;
    }
}