using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Customer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler
{
    public enum Trait { Warm, Cold, Outdoorsy, Relaxing, Beautiful, Strange, Bizarre, Close, Far, Gloomy, LowPop, HighPop }
    private static string[] Descriptors = new string[]
    {
        "Likes warm weather",
        "Likes cold weather",
        "Likes to experience the outdoors",
        "Looking for somewhere to relax",
        "Wants pictures for their Instagram",
        "Wants to see new things",
        "Has unusual taste",
        "Wants to stay close to home",
        "Just wants to get away",
        "Broods often",
        "Looking for adventure",
        "Susceptible to peer pressure"
    };
    public static int[,] HappyBonuses = new int[,]
    {
        { -3,  5,  3,  1,  0, -5,  0,  0,  0,  0,  0,  0,  0,},
        {  3, -5, -3, -1,  0,  5,  0,  0,  0,  0,  0,  0,  0,},
        {  5,  0,  0,  1,  5, -1,  2,  0, -2,  0, -4,  0,  0,},
        {  0,  4,  0,  5,  0, -1,  2, -3, -3,  8, -5,  0,  0,},
        {  3,  1, -2,  0,  1,  0,  5,  1, -1,  0, -2,  2,  0,},
        {  0,  0,  0, -1, -1, -1,  3,  8,  5,  2,  1,  2,  0,},
        { -1, -3, -1, -1, -1, -1, -5,  5,  5,  4,  5,  8,  0,},
        {  0,  0,  0,  1,  0,  0,  1,  0,  0,  0,  0,  0,  0,},
        {  0,  0,  0,  1,  0,  0,  1,  0,  0,  0,  0,  0,  0,},
        {  1,  0,  0,  0,  1,  0, -2,  0,  0,  0,  8,  3,  0,},
        {  2,  2,  0, -4,  1,  0,  0,  5,  5, -5,  5,  0,  0,},
        {  0,  0,  0,  2,  0,  0,  0,  0,  0,  0,  0,  0,  0,},
    };

    public static int[] DistanceMods = new int[] { 0, 0, 0, 0, 0, 0, 0, -5, 5, 0, 3, 0 };
    public static int[] PopMods = new int[] { 0, 0, -1, 0, 1, 0, 0, 0, -3, -1, -5, 5 };

    public Image Shoes;
    public Image Pants;
    public Image Shirt;
    public Image Skin;
    public Image Eyes;
    public Image Hair;

    public Sprite[] Hairstyles;

    public Gradient skinColors;
    public Gradient naturalHairColors;
    public Gradient eyeColors;

    public TextAsset firstnameList;
    public TextAsset lastnameList;

    public string Name;
    public float Money;
    public List<Trait> Preferences;

    public bool WaitingOnShip;

    public void Generate()
    {
        Shoes.color = Random.ColorHSV(0, 1, 0, 0.5f, 0, 0.8f);
        Pants.color = Random.ColorHSV(0, 1, 0, 1, 0.3f, 1);
        Shirt.color = Random.ColorHSV(0, 1, 0, 1, 0.3f, 1);

        Skin.color = skinColors.Evaluate(Random.Range(0f, 1f));
        Eyes.color = eyeColors.Evaluate(Random.Range(0f, 1f));
        if (Random.Range(0f, 1f) < 0.5f)
        {
            Hair.color = naturalHairColors.Evaluate(Random.Range(0f, 1f));
        }
        else
        {
            Hair.color = Random.ColorHSV(0, 1, 0.7f, 1, 0.5f, 1);
        }
        int r = Random.Range(0, Hairstyles.Length);
        Hair.sprite = Hairstyles[r];

        r = Random.Range(0, 12);
        Preferences.Add((Trait)r);
        r = Random.Range(0, 12);
        Preferences.Add((Trait)r);

        Money = Random.Range(1f, 1000f);
        Money = Mathf.Round(Money * 10) / 10f;

        string[] firstnames = firstnameList.text.Split("\n"[0]);
        string[] lastnames = lastnameList.text.Split("\n"[0]);
        int fn = Random.Range(0, firstnames.Length);
        int ln = Random.Range(0, lastnames.Length);

        Name = string.Format("{0} {1}", firstnames[fn], lastnames[ln]);
    }

    public float CalcHappiness(Planet planet)
    {
        float happiness = 0;
        foreach (Trait c in Preferences)
        {
            foreach (Planet.Trait p in planet.Traits)
            {
                happiness += HappyBonuses[(int)c, (int)p];
            }

            float distance = Vector3.Distance(planet.transform.position, PlanetManager.Home.transform.position);
            distance = (distance / 7) * 2 - 1;
            happiness += DistanceMods[(int)c] * distance;

            float population = (planet.Population / 50) * 2 - 1;
            happiness += PopMods[(int)c] * population;
        }

        return happiness;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string tooltipFormat =
            "{0}\n" +
            "Money: ${1}M\n" +
            "* {2}\n" +
            "* {3}\n"; ;

        TooltipManager.Show(string.Format(tooltipFormat, Name, Money.ToString("##########0.#"), Descriptors[(int)Preferences[0]], Descriptors[(int)Preferences[1]]));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Hide();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        WaitingOnShip = true;
    }
}
