using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Unit
{
    public event EventHandler BarrierDestroyed;

    void Start()
    {
        IsHero = true;
        HealthUpdated += CheckHealth;
    }

    private void CheckHealth(object sender, EventArgs e)
    {
        HUDManager.Instance.SetBarrierHealth(Health, maxHealth);
        if (Health <= 0)
        {
            BarrierDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
