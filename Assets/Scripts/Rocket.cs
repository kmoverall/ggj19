using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [System.NonSerialized]
    public Customer[] passengers = new Customer[3];
    public SpriteRenderer outline;
    public Color[] outlineColors;
    public RocketManager manager;

    public float Speed = 0;

    private Planet Target;

    public int Index;

    private bool returning;

    public void SetColor(int index)
    {
        outline.color = outlineColors[index];
    }

    private void Update()
    {
        if (Target == null)
        {
            return;
        }
        
        transform.Translate(Vector3.right * Speed * Time.deltaTime, Space.Self);

        if (Vector3.Distance(transform.position, Target.transform.position) < 0.25f)
        {
            if (Target == PlanetManager.Home)
            {
                manager.Return(this);
            }
            else 
            {
                FinanceManager.CollectPayout(passengers, Target);
                Target.Population += 0.1f * passengers.Length;
                foreach (var c in passengers)
                {
                    Destroy(c.gameObject);
                }
                passengers = new Customer[0];
                Launch(PlanetManager.Home, Speed);
            }
        }
    }

    public void Launch(Planet target, float speed)
    {
        Target = target;
        Speed = speed;

        Vector3 v = Target.transform.position - transform.position;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;

        if (v.x < 0)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
