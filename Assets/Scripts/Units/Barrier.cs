using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Unit
{
    public event EventHandler BarrierDestroyed;

    void Start()
    {
        state = UnitState.Waiting;
        IsHero = true;
        HealthUpdated += CheckHealth;
    }

    protected override void CheckHealth(object sender, EventArgs e)
    {
        HUDManager.Instance.SetBarrierHealth(Health, maxHealth);
        if (Health <= 0)
        {
            BarrierDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }
}
