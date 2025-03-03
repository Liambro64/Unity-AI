using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Synapse
{
    
    public double Value;
    public double multiplier;
    public bool active = true;
    public Neuron to;
    public System.Random random;
    public Synapse(System.Random random)
    {
        Value = 0;
        multiplier = (double)random.Next(-1000, 1000) / 1000;
        this.random = random;
    }
    public Synapse(double multiplier, System.Random random)
    {
        this.multiplier = multiplier;
        this.random = random;
        Value = 0;
    } 
    public void FeedForward(double val)
    {
        if (active)
        {
            Value = Mathf.Atan((float)(multiplier * val)*5f)/Mathf.PI/2;
            to.Value = Mathf.Clamp((float)(to.Value + Value), -1f, 1f);
        }
        Value = 0;
        //Debug.Log("Value: " + to.Value + " Mulitplier: " + multiplier);
    }
    public void Set(double Value, double multiplier)
    {
        this.Value = Value;
        this.multiplier = multiplier;
    }
    public void toggleActive()
    {
        active = !active;
    }
    public void Randomise(double strength, float randChance)
    {
        if (random.Next(0, 100) < randChance) {
            multiplier += (double)random.Next(-1000, 1000) / 1000 *strength;
        }
            
    }
}
