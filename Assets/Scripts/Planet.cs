using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Planet : MonoBehaviour
{
    public enum Trait { Mountainous = 0, Tropical, Dry, Mild, Forested, Snowy, Scenic, Alien, Sentient, Fluffy, Haunted, Postmodern, Boring }
    private int[] _traitWeights = new int[] { 8, 8, 8, 8, 8, 8, 8, 2, 1, 1, 1, 1, 0 };
    public float Population;
    public float RealEstateCost;
    public float FlightCost;
    public List<Trait> Traits;
    public Sprite[] Appearances;
    public string Name;

    public string[] firstNameOptions;
    public string[] secondNameOptions;

    public ParticleSystem happyParticles;
    public ParticleSystem sadParticles;

    public Text payIndicator;

    private bool _generated = false;

    public void Initialize()
    {
        if (_generated)
        {
            return;
        }

        Population = 0;

        for (int i = 1; i < _traitWeights.Length; i++)
        {
            _traitWeights[i] += _traitWeights[i - 1];
        }

        Traits.Add(ChooseTrait());
        Traits.Add(ChooseTrait());

        int r = Random.Range(0, Appearances.Length);
        GetComponent<SpriteRenderer>().sprite = Appearances[r];

        r = Random.Range(0, firstNameOptions.Length);
        Name = firstNameOptions[r];
        r = Random.Range(0, secondNameOptions.Length);
        Name = Name + " " + secondNameOptions[r];

        _generated = true;
    }

    private Trait ChooseTrait()
    {
        int trait = Random.Range(0, _traitWeights[_traitWeights.Length - 1] - 1);

        for (int i = 1; i < _traitWeights.Length; i++)
        {
            if (trait < _traitWeights[i])
            {
                return (Trait)i;
            }
        }

        return Trait.Boring;
    }

    private void OnMouseEnter()
    {
        string tooltipFormat;
        if (Name == "Earth")
        {
            tooltipFormat =
                "{0}\n" +
                "Population: {1}M\n" +
                "* {2} ";
            TooltipManager.Show(string.Format(tooltipFormat, Name, Population.ToString("##########0.#"), Traits[0]));

            return;
        }

        tooltipFormat =
            "{0}\n" +
            "Population: {1}M\n" +
            "Real Estate Value: ${4}M\n" +
            "Travel Cost: ${5}M\n" +
            "* {2}\n" +
            "* {3} ";

        TooltipManager.Show(string.Format(tooltipFormat, Name, Population.ToString("##########0.#"), Traits[0], Traits[1], RealEstateCost.ToString("##########0.#"), FlightCost.ToString("##########0.#")));
    }

    private void OnMouseExit()
    {
        TooltipManager.Hide();
    }

    private void OnMouseUpAsButton()
    {
        RocketManager.Target = this;
        TooltipManager.ShowLaunch(Name, Camera.main.WorldToScreenPoint(transform.position));
    }

    public void ShowPayout(float amount)
    {
        payIndicator.text = string.Format("+${0}M", amount.ToString("##########0.#"));
        payIndicator.gameObject.SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        StartCoroutine(HidePayout(3));
    }

    private IEnumerator HidePayout(float time)
    {
        yield return new WaitForSeconds(time);
        payIndicator.gameObject.SetActive(false);
    }

    public void EmitParticles(bool isHappy, float amount)
    {
        ParticleSystem target = isHappy ? happyParticles : sadParticles;
        var burst = target.emission.GetBurst(0);
        burst.count = amount;
        target.emission.SetBurst(0, burst);
        target.Play();
    }
}
