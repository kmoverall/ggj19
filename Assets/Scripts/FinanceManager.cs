using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinanceManager : MonoBehaviour
{
    public static float Money = 20;
    public Text MoneyText;

    // Update is called once per frame
    void Update()
    {
        MoneyText.text = string.Format("${0}M", Money.ToString("##########0.#"));
    }

    public static void CollectPayout(Customer[] customers, Planet planet)
    {
        float payout = 0;
        int happy = 0;
        int sad = 0;
        foreach (var c in customers)
        {
            float happinessMod = c.CalcHappiness(planet);
            happy += happinessMod > 0 ? (int)happinessMod : 0;
            sad += happinessMod < 0 ? (int)-happinessMod : 0;

            happinessMod = (20f + happinessMod) / 20f;
            float pay = planet.RealEstateCost * happinessMod;
            pay = Mathf.Min(c.Money, pay);
            payout += pay;
        }
        planet.EmitParticles(true, happy);
        planet.EmitParticles(false, sad);
        planet.ShowPayout(payout);
        Money += payout;
    }
}
