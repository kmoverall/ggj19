using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketManager : MonoBehaviour
{
    public float rocketSpeed;
    public Rocket rocketPrefab;

    public GameObject[] uiRockets;

    public static Planet Target;

    public Button[] launchButtons;

    public CustomerManager customerManager;

    private void Update()
    {
        for(int i = 0; i < uiRockets.Length; i++)
        {
            launchButtons[i].interactable = uiRockets[i].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Waiting");
        }
    }

    public void Launch(int index)
    {
        var newRocket = Instantiate(rocketPrefab);
        newRocket.SetColor(index);
        newRocket.transform.position = PlanetManager.Home.transform.position;
        newRocket.Index = index;
        newRocket.manager = this;

        uiRockets[index].GetComponent<Animator>().SetBool("IsShipping", true);
        newRocket.passengers = uiRockets[index].GetComponentsInChildren<Customer>();
        customerManager.RemoveCustomers(newRocket.passengers);

        foreach (var c in newRocket.passengers)
        {
            c.gameObject.SetActive(false);
        }

        FinanceManager.Money -= Target.FlightCost;

        GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        newRocket.Launch(Target, rocketSpeed);
    }

    public void Return(Rocket rocket)
    {
        uiRockets[rocket.Index].GetComponent<Animator>().SetBool("IsShipping", false);
        Destroy(rocket.gameObject);
    }
}
