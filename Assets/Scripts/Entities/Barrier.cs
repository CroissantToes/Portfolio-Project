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
    }

    private void Update()
    {
        if (Health <= 0)
        {
            BarrierDestroyed?.Invoke(this, EventArgs.Empty);
        }
    }

    public override void ShowMoveArea()
    {
        return;
    }

    public override void HideMoveArea()
    {
        return;
    }
}
