using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CustomerManager : MonoBehaviour
{
    private List<Customer> customers = new List<Customer>();
    public Customer customerPrefab;
    public Transform[] uiSlots;
    private int page = 0;

    public GameObject leftScrollButton;
    public GameObject rightScrollButton;

    public Text customerText;

    public float MaxCustomerChance;
    public float CustomerSatisfaction;
    public int MaxCustomers;

    private bool OnFirstPage
    {
        get { return page == 0; }
    }
    private bool OnLastPage
    {
        get { return page >= (customers.Count - 1) / uiSlots.Length; }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
        UpdateUI();
        TimeManager.OnDayTick += TryGenerateCustomer;
        GenerateCustomer();
    }
    
    private void TryGenerateCustomer()
    {
        if (customers.Count >= MaxCustomers)
        {
            return;
        }

        float r = Random.Range(0f, 1f);
        if (r <= MaxCustomerChance * CustomerSatisfaction)
        {
            GenerateCustomer();
        }
    }

    private void GenerateCustomer()
    {
        var newCustomer = Instantiate(customerPrefab);
        newCustomer.Generate();
        newCustomer.gameObject.SetActive(false);
        customers.Add(newCustomer);
        UpdateText();

        leftScrollButton.SetActive(!OnFirstPage);
        rightScrollButton.SetActive(!OnLastPage);

        if (OnLastPage)
        {
            UpdateUI();
        }
    }

    public void RemoveCustomers(Customer[] toRemove)
    {
        customers = customers.Except(toRemove).ToList();
        UpdateUI();
    }

    private void UpdateUI()
    {
        int start = page * uiSlots.Length;
        int end = start + uiSlots.Length;

        foreach (var c in customers)
        {
            if (!c.WaitingOnShip)
            {
                c.gameObject.SetActive(false);
            }
        }
        

        for (int i = start; i < end; i++)
        {
            if (i >= customers.Count)
            {
                break;
            }

            if (!customers[i].WaitingOnShip)
            {
                customers[i].gameObject.SetActive(true);
                customers[i].transform.SetParent(uiSlots[i - start]);
                customers[i].transform.localPosition = Vector3.zero;
                customers[i].transform.localScale = Vector3.one;
            }
        }

        leftScrollButton.SetActive(!OnFirstPage);
        rightScrollButton.SetActive(!OnLastPage);
    }

    private void UpdateText()
    {
        string formatString = customers.Count == 1 ? "{0} Customer" : "{0} Customers";
        customerText.text = string.Format(formatString, customers.Count);
    }

    public void PageLeft()
    {
        if (!OnFirstPage)
        {
            page--;
            UpdateUI();
        }
    }

    public void PageRight()
    {
        if (!OnLastPage)
        {
            page++;
            UpdateUI();
        }
    }
}
