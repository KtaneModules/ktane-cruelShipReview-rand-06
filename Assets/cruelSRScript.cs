using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class cruelSRScript : MonoBehaviour
{
    //public KMBombInfo Bomb;
    public KMAudio Audio;
    public AudioClip pressSound;
    public GameObject[] Buttons;
    public GameObject[] people;
    public GameObject[] pair;


    private const int amountOfCharacters = 38;
    
    static int ModuleIdCounter;
    int ModuleId;
    private bool ModuleSolved;
    
    public Texture[] toons;
    public TextMesh currentNames;
    public TextMesh stageCounter;

    private static List<string> names = new List<string>
    {
        "Astro", "Bassie", "Bobette", "Blot", "Brusha", "Boxten", "Brightney", "Cocoa", "Connie", "Cosmo", "Dandy",
        "Dyle", "Eclipse", "Eggson", "Finn", "Flutter", "Flyte", "Gigi", "Ginger", "Glisten", "Goob", "Looey", "Poppy",
        "R&D", "Ribecca", "Rodger", "Rudie", "Scraps", "Shrimpo", "Shelly", "Soulvester", "Sprout", "Squirm", "Teagan",
        "Tisha", "Vee", "Waxwell", "Yatta"
    };
    
    private static sbyte[][] data =
    {
        new sbyte[]
        {
            0,0,0,0,5,2,0,-1,2,4,-4,0,0,3,2,1,-1,0,4,5,4,1,3,0,4,0,2,-5,3,0,2,0,3,4,-3,0,2,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,-2,3,-4,-4,0,0,4,1,1,-5,0,5,5,3,
            3,5,0,3,0,4,-5,3,0,2,0,-3,5,-4,0,-1,0,2,3,-3,-5,0,0,2,2,2,2,0,3,2,2,3,3,0,2,0,3,-4,4,0,2,-5,2,4,1,0,
            1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,-1,3,0,0,3,2,1,3,0,2,1,3,2,1,0,1,0,1,
            -3,2,-9,1,0,1,1,2,0,2,1,-1,0,0,3,1,1,-2,-9,3,3,4,2,2,0,2,0,2,-5,1,0,5,0,-2,3,-2,0,-2,-3,0,0,2,-4,-4,
            -1,0,1,2,2,-2,-5,0,3,0,-3,-5,-2,0,1,0,-1,-1,-5,0,-3,0,0,-1,-3,-3,4,0,2,1,1,-3,-4,0,5,0,1,-4,-2,0,1,0,3,
            -5,3,0,-5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,3,3,1,0,3,4,5,2,-1,0,-2,0,4,5,2,0,3,0,-3,2,-1,0,4,-9,-2,0,-1,2,-3,1,3,0,-3,0,2,-5,1,0,1,0,2,1,-1,0,
            -2,-3,0,-1,2,-3,1,3,0,-3,0,2,-5,1,0,1,0,1,1,-1,0,-2,0,-4,-3,-3,2,-4,0,4,0,-4,-2,2,0,2,0,3,-1,2,0,-4,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,3,2,4,0,5,0,1,-5,-2,0,3,0,3,2,-2,0,2,3,3,4,0,-1,0,-9,-5,3,0,4,0,1,2,
            -1,0,1,1,2,0,-2,0,3,-1,-1,0,2,0,1,2,-3,0,5,1,0,1,0,3,-5,4,0,-1,0,2,3,1,0,3,0,2,0,1,-5,1,0,1,0,2,3,-2,0,-5,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,-4,3,0,2,0,3,1,-5,0,-5,0,0,0,0,0,0,0,0,0,0,0,-5,2,0,1,0,3,4,1,0,4,-5,0,-2,0,
            4,-5,-3,0,-4,0,2,0,2,3,4,0,3,0,0,0,0,0,0,0,0,1,2,-2,0,-1,0,0,0,0,0,1,-3,0,-2,1,0,-4,0,-5,0
        },
        new sbyte[]
        {
            3,1,-1,0,-1,0,0,2,-2,-4,-2,1,-1,-1,0,0,1,1,-2,-2,-1,0,-3,-1,-1,-1,1,-2,5,0,0,0,0,1,4,0,2,0,1,-1,0,0,-5,0,0,0,0,0,
            -2,1,1,3,0,1,0,0,1,-2,-1,-2,0,0,1,0,-1,0,3,0,0,0,-1,0,0,0,0,0,0,0,1,0,2,0,0,-2,0,0,0,1,-3,-3,0,0,-1,-1,0,0,0,-1,0,
            -2,0,1,0,0,0,-4,0,0,1,-2,1,0,0,-1,0,-3,0,0,0,2,-2,2,0,-1,0,0,0,0,1,0,0,1,1,0,0,0,0,1,2,1,0,4,1,0,0,0,1,0,0,0,0,0,0,
            1,0,0,1,-2,1,-2,-2,0,1,0,-2,-1,-1,2,-1,0,0,-4,-1,0,0,2,1,0,-1,-2,0,1,-3,1,-2,-2,-1,0,0,-1,0,3,-2,1,-2,1,0,-1,-3,0,0,
            0,1,3,-1,0,1,0,-1,2,0,0,0,1,1,0,2,1,-2,2,0,-1,-2,2,0,2,1,-1,-2,-2,1,-1,0,-2,-2,-3,0,0,-2,0,0,-1,0,-2,2,-1,1,0,-2,-2,
            1,1,0,0,1,-1,3,-1,1,1,0,2,0,-3,0,0,0,2,1,-2,-1,0,0,1,2,2,2,0,0,0,4,0,0,1,0,1,0,-2,0,-9,0,0,-2,-2,-3,0,-1,0,0,1,0,-2,
            0,0,0,-9,-1,0,0,0,0,0,-2,0,1,0,0,0,-5,0,0,0,-1,0,1,1,-2,1,0,1,0,-1,0,-2,0,0,-2,-2,0,0,1,0,0,-2,0,-1,0,0,0,-2,0,0,0,0,
            -3,-3,1,0,0,1,0,-1,-2,0,0,1,0,-2,0,0,-1,0,0,2,0,-1,0,0,-2,1,0,1,0,0,0,3,1,-1,0,0,-2,3,1,1,-1,1,1,0,0,0,0,0,0,0,0,0,-1,
            -2,0,0,0,0,1,-2,0,0,0,0,0,0,0,0,1,1,0,0,0,2,0,4,2,0,-1,0,0,0,1,0,0,2,-3,1,0,0,0,-2,0,0,0,1,-9,1,0,-1,2,0,0,0,0,0,1,0,
            -1,0,0,0,0,0,0,0,0,0,2,2,-2,0,1,0,0,2,-1,-2,3,0,0,0,0,0,0,2,1,0,1,0,0,1,2,0,1,1,0,2,0,3,0,3,1,0,-2,-1,-1,0,0,-1,0,0,
            -1,1,0,0,2,0,0,0,3,4,0,-3,0,0,0,0,-1,0,1,0,0,-1,-2,-1,-3,-1,0,-1,0,1,1,0,0,0,1,2,-1,0,-3,0,-9,0,0,0,0,0,-1,0,0,0,2,2,
            -3,1,1,0,2,-2,1,0,1,0,-2,1,2,0,4,-1,0,0,1,0,0,-1,1,0,0,0,0,0,0,0,0,-3,0,0,-2,0,0,-1,0,0,0,0,0,0,0,1,0,0,-1,0,0,0,-1,0,
            -1,0,0,0,-3,-4,0,0,0,0,4,2,0,0,-1,2,-2,1,0,0,0,-2,1,0,0,1,1,0,2,1,0,-3,0,-2,0,0,-3,0,-2,0,1,1,-3,0,1,2,3,0,0,-4,-5,0,0,
            0,0,1,1,3,0,2,0,-2,0,3,0,2,0,0,0,0,0,-1,0,0,0,-2,0,0,0,0,0
        },
        new sbyte[]
        {
            0,0,4,-3,2,3,0,0,1,-3,1,0,0,0,0,1,-1,1,0,5,1,0,4,0,-3,0,0,-2,5,1,2,0,0,0,3,0,0,0,0,-5,0,0,-5,0,0,-1,0,0,0,0,0,0,-1,0,
            -1,1,1,0,0,0,-1,1,1,-4,3,-3,2,0,1,1,0,0,0,0,-5,0,1,1,0,1,-1,0,0,0,0,0,0,0,1,0,1,1,0,0,0,0,1,0,0,1,0,1,0,0,0,0,0,0,
            -1,5,0,0,0,2,0,1,0,0,2,0,0,0,0,0,1,4,0,4,0,0,0,0,0,0,5,1,0,0,2,0,0,1,-5,-3,-4,-2,-5,-5,-5,-4,-3,-5,-1,-4,-3,-4,-5,-5,
            -4,0,-4,-2,-4,-4,-5,-5,-4,-4,-3,-5,-3,-5,-4,-4,-2,1,0,0,5,-2,0,0,0,1,0,0,-1,0,4,4,2,1,3,0,2,0,0,-2,0,4,0,0,0,4,-1,0,0,
            0,3,1,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1,-1,1,1,0,-5,3,1,3,0,0,0,1,-1,0,0,0,0,0,0,0,1,0,1,0,0,0,0,0,1,0,0,0,0,0,0,0,0,
            0,0,2,-2,0,0,0,0,0,2,0,2,0,0,0,0,0,0,0,0,0,0,0,0,-9,0,0,0,0,0,0,0,-2,0,0,0,1,0,0,0,-9,0,4,5,0,3,0,-2,0,0,0,2,1,5,0,1,
            0,0,0,1,2,0,-1,-1,0,0,-4,0,-1,-5,-2,0,-1,0,-1,-1,-1,-4,0,-2,-4,0,-4,-2,-4,0,-1,0,0,1,0,0,-1,0,1,-4,0,0,0,0,0,0,0,-2,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,-2,0,0,0,0,0,0,0,0,0,-1,
            0,-1,1,3,0,1,0,0,0,0,5,1,1,0,0,0,0,0,0,0,-9,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,3,0,0,0,
            0,0,0,0,0,-1,0,0,0,0,0,-1,0,-1,-2,-1,-3,-4,0,-3,-3,0,0,-2,0,0,0,0,0,0,3,0,0,0,0,0,2,0,0,0,0,0,2,3,-1,0,-1,0,3,0,0,-2,-3,1,
            -1,0,1,0,-1,0,0,1,0,2,0,4,2,-9,1,3,1,3,0,0,0,-3,0,0,1,4,0,0,3,0,-3,1,4,5,0,0,0,-1,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,-5,0,1,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,-2,0,-1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,-3,1,0,0,0,0,1,0,0,4,-3,0,-2,
            -5,-3,-3,-4,0,-4,0,0,0,2,3,-4,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,-5,0,0,0,0,0
        },
        new sbyte[]
        {
            -1,-2,3,-3,-1,2,-2,0,0,4,0,-1,0,-1,1,0,-2,0,0,1,1,1,5,0,-5,-3,0,-5,3,0,1,-5,1,-3,4,1,-1,3,-1,-4,0,0,-5,-1,0,4,0,
            -2,0,1,1,1,2,-1,0,1,-2,2,0,-1,-4,-2,-1,-5,2,0,2,-5,3,-2,-1,-4,0,0,-3,0,0,1,-1,1,2,0,-2,-1,0,1,0,0,4,0,1,0,1,-1,0,
            -2,-1,4,-5,-1,0,0,-5,1,-1,0,-2,0,2,2,-1,1,1,0,-1,-2,1,-1,-3,1,0,1,0,-4,1,4,2,3,1,0,-3,-3,-5,-1,0,0,-5,0,-4,3,-4,3,
            -2,-2,-2,-4,-2,-2,-1,-3,-2,-1,1,-2,0,-3,-2,-2,-3,-2,-3,-5,-1,-2,-5,-3,-1,-2,-2,-5,3,5,0,0,-3,1,0,0,4,1,0,0,0,1,1,0,
            -1,1,5,1,1,3,0,1,0,-2,0,-5,0,0,1,-5,1,0,2,-4,0,-1,5,0,0,2,0,0,2,2,0,-1,0,-1,1,0,1,1,0,2,-2,2,-5,1,0,0,-5,0,0,4,-5,0,
            1,2,-1,-1,1,1,0,1,0,0,3,2,0,0,0,0,0,0,-2,1,-5,0,0,1,-5,0,0,0,-4,1,0,-2,0,2,-1,-2,2,0,4,0,3,0,0,1,0,3,-2,-3,1,-5,1,-9,
            -2,-5,-4,-1,1,-4,3,-3,0,0,-1,0,2,-1,1,-9,1,1,0,0,0,0,-3,-2,0,-5,0,0,4,-5,1,0,0,-4,0,0,0,-1,0,1,0,-2,-1,-2,-4,0,-1,0,
            -2,-4,-2,0,-5,-4,0,-4,-5,-5,1,-4,-4,-3,0,2,3,1,0,0,-1,2,-4,-2,0,0,0,3,-2,0,-5,2,0,1,-5,0,1,0,-5,-1,0,0,1,0,1,0,0,3,-1,
            1,0,0,0,-2,1,-5,0,1,-2,-5,0,-2,-1,-4,1,-1,0,-1,0,-1,-1,-2,0,-1,0,0,1,-3,-1,-5,-1,-1,-1,-5,0,-1,-1,-3,-1,1,0,0,0,-4,3,4,1,
            -1,0,0,-2,1,4,0,2,0,-5,0,-3,0,-3,2,-9,4,1,1,1,1,1,1,3,1,-2,1,-5,1,1,1,-5,1,1,1,-5,1,0,3,0,0,0,0,0,0,0,-2,0,-5,0,0,0,-5,
            0,0,0,-5,0,0,0,0,-1,2,0,0,0,-2,3,-5,0,-2,-1,-5,-1,-2,-3,1,1,0,0,0,1,0,0,0,-1,2,-5,-1,3,3,-5,1,0,0,-4,0,1,0,0,4,-2,5,-2,0,
            -5,-3,0,0,-5,1,1,2,-5,-1,-1,1,-1,0,-2,-1,-9,-5,0,0,1,-5,0,0,1,-2,2,4,1,0,0,-2,0,-5,-1,0,4,-5,0,0,-1,-5,4,0,0,0,-2,1,-5,-2,0,
            1,-5,0,0,0,-4,0,1,1,-2,0,-5,0,0,0,-5,0,0,3,-5,0,-1,-2,4,-5,3,2,0,-5,1,0,-1,-5,0,-2,0,-5,1,0,-4,-5,1,3,-5,-5,-3,-2,-5,-2,-2,-2,-5,
            -2,-2,-4,-4,-1,-5,0,0,-1,-5,0,-3,-5,-5,1,-5,-5,-5,-5,-5,-5,-5,-5,-5,0,2,-5,1,4,-2,-4,2,1,-5,1,1,-1,-4,0,-5,-1,-3,3,-4,0,-5,-5,-5,-5,
            -5,1,1,-4,0,-5,-4,-1,-4,-2,1
        },
        new sbyte[]
        {
            0,-1,0,0,2,0,0,-1,3,5,-1,0,0,2,0,0,1,0,-2,5,0,-1,-2,0,-2,-1,-2,-3,5,0,-3,0,-4,3,0,3,0,5,0,0,0,0,-5,0,0,0,0,0,-2,0,0,0,0,3,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,0,0,1,0,0,0,0,0,0,0,0,2,0,0,0,0,0,0,0,3,0,0,5,0,0,0,5,0,0,0,0,0,0,-5,0,0,0,0,0,0,0,3,0,0,0,0,0,0,
            5,3,-3,0,0,0,-5,0,0,0,0,0,0,-5,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-5,0,5,0,0,0,4,5,3,5,0,-3,0,0,4,0,0,3,5,5,4,5,5,
            5,0,-4,5,-2,0,4,0,4,0,5,5,-1,0,0,0,0,0,-5,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-5,3,0,0,-5,5,4,1,0,0,0,0,0,0,-2,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,
            0,0,0,0,0,0,0,0,0,3,0,0,0,0,4,0,0,5,5,5,0,4,5,4,0,-5,5,0,-3,0,-9,0,0,4,4,3,0,0,-5,-5,0,0,-1,0,0,0,-9,-3,-4,0,3,0,0,-5,0,0,0,0,0,5,0,-3,3,-3,0,
            0,5,0,0,-4,0,0,-4,-5,0,0,-4,-5,0,0,0,0,0,-5,-3,0,0,-3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,-3,0,4,3,0,0,-5,3,0,5,4,0,-1,0,0,0,0,0,0,-9,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-3,0,0,0,0,0,0,0,0,-5,3,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            -4,0,3,3,0,-5,-3,0,-3,0,0,0,-5,5,0,0,0,0,4,0,0,0,-5,0,-9,5,4,0,0,0,0,0,-3,5,2,5,5,0,-5,5,0,-3,0,0,5,0,-2,0,-5,0,5,0,0,0,0,0,0,0,0,0,0,0,5,0,0,0,0,
            -5,0,0,-5,0,0,-4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-5,-5,-5,-5,-5,-5,-5,5,0,-5,-5,-5,-5,-4,0,0,0,0,5,-4,0,0,0,-5,0,0,0,0,0,0,0,0,0,-3,0,0,-5,5,
            0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,-2,-3,0,0,0,0,0,0,0,0,5,0,0,0,0,0,0,4,0,2
        },
        new sbyte[]
        {
            3,3,-4,2,5,5,1,-3,-4,-5,3,5,-5,-2,-4,-5,1,2,-4,2,2,3,0,3,-5,5,2,-1,4,5,-5,4,2,0,3,5,-4,3,-4,1,5,1,5,0,-1,
            5,-5,1,-5,3,4,-5,2,5,0,2,-4,-2,3,-1,-5,5,4,-3,3,0,3,1,1,2,0,3,-1,-5,5,5,3,-5,1,-3,2,4,-4,0,3,4,-5,0,5,2,-2,
            -3,-2,-1,-5,-5,5,4,-5,0,2,3,1,0,-4,0,2,3,5,5,4,-5,-3,0,-5,2,4,0,-1,-2,-5,2,0,-3,-4,3,1,5,3,-5,5,2,1,-2,3,-4,
            -3,-1,-5,1,4,-4,5,0,3,4,0,5,-4,-3,-5,-1,4,-5,-2,-4,1,5,0,0,2,4,-5,5,-2,-5,-4,2,-5,3,5,0,1,5,0,5,5,5,5,5,5,5,5,5,5,
            -5,5,5,5,5,5,5,5,5,-5,5,5,5,5,5,5,5,5,5,5,5,5,4,0,2,5,-5,5,1,-2,3,-5,-1,0,4,0,1,2,0,0,-5,5,0,0,0,0,0,0,0,0,0,0,0,0,1,4,
            -1,5,-5,0,5,-5,0,3,1,4,0,-2,1,-4,-5,5,5,-2,5,4,4,-5,-4,-5,0,-5,3,0,0,0,0,0,0,0,-5,0,0,0,0,0,0,0,0,-5,5,0,0,0,
            -9,0,0,0,0,0,0,0,0,0,0,0,0,0,-5,0,-9,0,0,0,0,0,0,-5,5,0,0,0,0,5,0,0,0,0,0,0,-5,3,-1,-3,5,-5,-2,0,2,5,5,0,-4,-3,-5,5,4,
            -2,4,5,5,5,0,-2,-4,1,1,-5,5,-5,-4,-5,-5,-5,-5,-5,-5,-5,3,4,-5,5,-5,1,-5,2,-5,-5,-5,-5,-5,-5,-5,4,0,3,-5,-2,5,5,5,1,-1,-4,-5,-5,5,2,
            -3,5,5,0,2,3,-1,-3,-4,0,0,0,-5,0,0,0,0,0,0,0,0,-5,5,0,0,0,0,0,0,0,0,0,0,0,-1,-5,0,3,2,4,0,-4,1,-2,-5,5,-3,-5,5,-5,1,-2,-4,-3,1,0,3,
            -9,0,0,0,0,0,0,0,0,-5,5,0,0,0,0,0,0,0,0,0,0,0,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,0,0,0,0,0,0,0,-5,5,0,0,
            0,0,0,0,0,0,0,0,0,5,4,1,1,0,-2,-5,5,2,-2,4,0,3,-2,0,-1,-4,-5,-5,3,3,0,-2,-4,-5,5,0,-3,2,0,-2,-1,4,0,-2,0,1,5,5,-4,-5,-5,5,-9,-5,5,-1,0,
            5,1,-2,1,-3,5,5,0,-3,-5,5,5,1,-3,-4,0,2,-5,1,3,2,0,-4,-2,-5,5,1,-4,0,1,3,0,-3,-1,-4,-2,-3,2,-5,5,-1,-3,0,5,-2,0,4,-2,-4,-4,-4,-5,5,0,0,0,
            0,0,0,0,0,0,0,0,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,-5,5,5,5,5,5,5,5,5,5,5,5,-2,5,2,-3,-4,-5,0,5,1,4,-3,-3,-3,-3,-3,-3,-3,-3,-3,4,0,5,5,-2,-4,
            -5,-2,-4,5,4,1,3,-2,-5,5,-2,-4,-5,-3,0,-1,-3,-5,-5,0,3,1,-4,0,0,0,0,-4,1,0
        }
    };

    private List<string> expectedAnswers;
    private string enteredAnswer = "";
    private string buttonFunctions = "01234.56789";
    
    private bool submitting;
    private int page;
    private int currentAnswerIndex;
    private List<int> requestedOrder;
    private List<int[]> pairs = new List<int[]>();
    
    
    //void log(string message) {Debug.Log(message.Split('\n').Aggregate($"[Cruel Ship Review #{ModuleId}] ", (a,b) => a+ $"\n[Cruel Ship Review #{ModuleId}] " + b));}
    void log(string message) {Debug.Log(message.Split('\n').Select(x => $"[Cruel Ship Review #{ModuleId}] {x}").Aggregate((a,b) => a+ "\n" + b));}
    void Awake()
    {
        ModuleId = ++ModuleIdCounter;
        for (int i = 0; i < 5; i++)
        {
            Buttons[i].GetComponent<MeshRenderer>().material.color = new Color(0,1, 0);
            Buttons[10-i].GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0);
        }
        Buttons[5].GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1);
        for (int i = 0; i < 11; i++)
        {
            int i1 = i;
            Buttons[i1].GetComponent<KMSelectable>().OnInteract += delegate
            {
                press(i1);
                return false;
            };
        }
    }

    void send()
    {
        if (enteredAnswer == expectedAnswers[currentAnswerIndex])
        {
            if (currentAnswerIndex == 5)
            {
                render();
                GetComponent<KMBombModule>().HandlePass();
                pair[0].GetComponent<MeshRenderer>().material.mainTexture = null;
                pair[1].GetComponent<MeshRenderer>().material.mainTexture = null;
                pair[0].GetComponent<MeshRenderer>().material.color = Color.black;
                pair[1].GetComponent<MeshRenderer>().material.color = Color.black;
                currentNames.text = "Solved!";
                for (int i=0; i<5; i++)people[i].GetComponent<MeshRenderer>().material.color = Color.white;
                ModuleSolved = true;
            }
            else
            {
                currentAnswerIndex++;
                currentNames.text = "";
                enteredAnswer = "";
                render();
            }
        }
        else
        {
            GetComponent<KMBombModule>().HandleStrike();
        }
    }

    void render()
    {
        if (submitting)
        {
            stageCounter.text = $"{currentAnswerIndex+1} / 6";
            pair[0].GetComponent<MeshRenderer>().material.mainTexture = null;
            pair[1].GetComponent<MeshRenderer>().material.mainTexture = null;
            pair[0].GetComponent<MeshRenderer>().material.color = Color.black;
            pair[1].GetComponent<MeshRenderer>().material.color = Color.black;
            for (int i = 0; i < 6; i++)
            {
                people[i].GetComponent<MeshRenderer>().material.color = currentAnswerIndex>i?new Color(1, 1, 1):new Color(.2f,.2f,.2f);
            }
        }
        else
        {
            stageCounter.text = $"{page+1} / 6";
            currentNames.text = "";
            pair[0].GetComponent<MeshRenderer>().material.mainTexture = toons[pairs[page][0]];
            pair[1].GetComponent<MeshRenderer>().material.mainTexture = toons[pairs[page][1]];
            pair[0].GetComponent<MeshRenderer>().material.color = Color.white;
            pair[1].GetComponent<MeshRenderer>().material.color = Color.white;
            for (int i = 0; i < 6; i++)
            {
                bool preferred = ".#..#..#.#.#.#.####.#.#.#.##.##.#######.#.####.#######"[requestedOrder[i] * 6 + page] == '#';
                people[i].GetComponent<MeshRenderer>().material.color = preferred?new Color(1, 1, 1):new Color(.2f,.2f,.2f);
            }
            
        }
    }

    void press(int button)
    {
        Audio.HandlePlaySoundAtTransform(pressSound.name, transform);
        if (ModuleSolved) return;
        if (submitting)
        {
            if (button == 5)
            {
                if (enteredAnswer.Length == 6) return;
                if (enteredAnswer == "")
                {
                    submitting = false;
                    render();
                }
                else
                {
                    enteredAnswer += '.';
                    currentNames.text = enteredAnswer;
                }
            }
            else
            {
                if (enteredAnswer == "")
                {
                    enteredAnswer += button < 5 ? "+" : "-";
                    currentNames.text = enteredAnswer;
                } 
                else if (enteredAnswer.Length == 6)
                {
                    if (button < 5) send();
                    if (button > 5)
                    {
                        enteredAnswer = "";
                        currentNames.text = enteredAnswer;
                    }
                }
                else
                {
                    enteredAnswer+=buttonFunctions[button].ToString();
                    currentNames.text = enteredAnswer;
                }
            }
        }
        else
        {
            if (button == 5)
            {
                submitting = true;
            }
            else if (button < 5)
            {
                if (page > 0) page--;
            }
            else if (page < 5) page++;
            render();
        }
    }

    sbyte pairOfPerson(int a, int b, int person)
    {
        if (a == b) return -9;
        if (a > b) a = a ^ b ^ (b = a);
        int index = (amountOfCharacters - 1) * amountOfCharacters / 2 - (amountOfCharacters - 1 - a) * (amountOfCharacters - a) / 2 + b - a - 1;
        return data[person][index];
    }

    int[] getRandomPair()
    {
        int a, b;
        while (true)
        {
            a = Rnd.Range(0, amountOfCharacters);
            b = Rnd.Range(0, amountOfCharacters);
            if (pairOfPerson(a, b, 0) == -9) continue; 
            int sum = 0;
            for (int i = 0; i < 6; i++) sum+=Math.Abs(pairOfPerson(a,b,i));
            if (sum > 3) break;
        }
        return new[] { a, b };
    }

    string listToString(List<double> list) => $"[{string.Join(" ", list.Select(x => x.ToString(CultureInfo.InvariantCulture)))}]";
    
    List<string> generateStages()
    {
        List<int> stageData = new List<int>();
        while (true)
        {
            for (int j = 0; j < 6; j++)
            {
                int[] pairStage = getRandomPair();
                stageData.AddRange(Enumerable.Range(0, 6).Select(i => (int)pairOfPerson(pairStage[0], pairStage[1], i)));
                pairs.Add(pairStage);
            }
            if (matrixShenanigans.initialCheck(stageData)) 
                break;
            stageData.Clear();
            pairs.Clear();
        }
        log($"Your pairs are: {pairs.Select(x=> $"{names[x[0]]} and {names[x[1]]}").Aggregate((a,b)=>a+", "+b)}.");
        log($"The data is: [{stageData.Select(x=>x.ToString()).Aggregate((a,b)=> a+", "+b)}]");
        requestedOrder = Enumerable.Range(0,9).ToList().Shuffle().Take(6).ToList();
        log($"The order of submitting is: {requestedOrder.Select(x=>(x+1).ToString()).Aggregate((a, b) => a + ", " + b)}.");
        List<double> flattenedSVD = matrixShenanigans.getFlattenedSVDFromData(stageData, ModuleId);
        log($"Flattened R: {listToString(flattenedSVD)}");
        List<string> answers = Enumerable.Range(0,6).Select(
            i=>
                ((flattenedSVD[requestedOrder[i]]<0?"-":"+") + Math.Abs(flattenedSVD[requestedOrder[i]]).ToString(CultureInfo.InvariantCulture).Substring(1))
                    .Substring(0,6)).ToList();
        log($"Your answers will be: {answers.Aggregate((a, b) => a + ", " + b)}.");
        return answers;
    }
    
    void Start()
    {
        expectedAnswers = generateStages();
        render();
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} <digit>/. to press corresponding button. For example: !{0} 32.4500";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string Command)
    {
        yield return null;
        if (!Command.RegexMatch(@"^[.\d]+$")) yield return "sendtochaterror Invalid command!";
        else
        {
            foreach (var t in Command)
            {
                switch (t)
                {
                    case '0': press(0); break;
                    case '1': press(1); break;
                    case '2': press(2); break;
                    case '3': press(3); break;
                    case '4': press(4); break;
                    case '.': press(5); break;
                    case '5': press(6); break;
                    case '6': press(7); break;
                    case '7': press(8); break;
                    case '8': press(9); break;
                    case '9': press(10); break;
                    default: yield return "sendtochaterror Invalid command!"; break;
                }
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        yield return null;
        GetComponent<KMBombModule>().HandlePass();
    }

}
