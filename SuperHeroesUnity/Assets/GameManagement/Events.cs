using UnityEngine;
using System;
using System.Collections;

// These EventArgs all store special data for your use.

public class CharEventArgs : EventArgs
{
    public Character myChar;

    public CharEventArgs(Character myChar)
    {
        this.myChar = myChar;
    }
}

public class ConditionEventArgs : EventArgs
{
    public Character myChar;
    public Condition type;
    public int turns;

    public ConditionEventArgs(Character myChar, Condition type, int turns)
    {
        this.myChar = myChar;
        this.type = type;
        this.turns = turns;
    }
}

public class DamageEventArgs : EventArgs{

    public Character myChar;
    public DamageType type;
    public int damage;

    public DamageEventArgs(Character myChar, DamageType type, int damage)
    {
        this.myChar = myChar;
        this.type = type;
        this.damage = damage;
    }
}

public class HealEventArgs : EventArgs
{

    public Character myChar;
    public int heal;

    public HealEventArgs(Character myChar, int heal)
    {
        this.myChar = myChar;
        this.heal = heal;
    }
}

public delegate void EventHandler(object sender, EventArgs e);

// This section of character handles all of the events
public partial class Character {

    // Setup all the event handlers

    public event EventHandler OnSpawn;
    public event EventHandler OnBeginTurn;
    public event EventHandler OnEndTurn;
    public event EventHandler OnMoved;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnKilled;
    public event EventHandler OnGainCondition;
    public event EventHandler OnLoseCondition;

    // Make it safe so that functions are called properly.
    protected virtual void E_Spawn(CharEventArgs e)
    {
        if (OnSpawn != null)
        {
            OnSpawn(this, e);
        }

    }

    protected virtual void E_BeginTurn(CharEventArgs e)
    {
        if (OnBeginTurn != null)
        {
            OnBeginTurn(this, e);
        }
    }

    protected virtual void E_EndTurn(CharEventArgs e)
    {
        if (OnEndTurn != null)
        {
            OnEndTurn(this, e);
        }
    }
    protected virtual void E_Moved(CharEventArgs e)
    {
        if (OnMoved != null)
        {
            OnMoved(this, e);
        }
    }

    protected virtual void E_Damaged(DamageEventArgs e)
    {
        if (OnDamaged != null)
        {
            OnDamaged(this, e);
        }
    }

    protected virtual void E_Healed(HealEventArgs e)
    {
        if (OnHealed != null)
        {
            OnHealed(this, e);
        }
    }

    protected virtual void E_Killed(CharEventArgs e)
    {
        if (OnKilled != null)
        {
            OnKilled(this, e);
        }
    }

    protected virtual void E_GainCondition(ConditionEventArgs e)
    {
        if (OnGainCondition != null)
        {
            OnGainCondition(this, e);
        }
    }

    protected virtual void E_LoseCondition(ConditionEventArgs e)
    {
        if (OnLoseCondition != null)
        {
            OnLoseCondition(this, e);
        }
    }
}
