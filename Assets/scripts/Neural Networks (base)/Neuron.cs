    using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Neuron
{
    public double Value;
    public double bias;
    public List<Synapse> synapses;
    System.Random random;
    public Neuron(System.Random random)
    {
        Value = 0;
        this.random = random;
        bias = random.Next(-1000, 1000) / 1000;
    }
    public Neuron(double bias, System.Random random)
    {
        this.bias = bias;
        Value = 0;
        this.random = random;
    }
    public Neuron(Neuron toCopy, Neuron[] attached, System.Random random)
    {
        bias = toCopy.bias;
        MakeSynapses(toCopy.synapses);
        for (int i = 0; i < attached.Length; i++)
        {
            synapses[i].to = attached[i];
        }
        this.random = random;
    }
    public void SetSynapses(List<Synapse> synapses)
    {
        this.synapses = synapses;
    }
    public void FeedForward(double val)
    {
        Value = Mathf.Atan((float)(bias + val + Value)*5f)/Mathf.PI/2;
        for (int i = 0; i < synapses.Count; i++)
        {
            synapses[i].FeedForward(Value);
        }
        Value = 0;
    }
    public void FeedForward()
    {
        for (int i = 0; i < synapses.Count; i++)
        {
            synapses[i].FeedForward(Value);
        }
        Value = 0;
    }
    public void MakeSynapses(int amount)
    {
        synapses = new List<Synapse>();
        for (int i = 0; i < amount; i++)
        {
            synapses.Add(new Synapse(random));
        }
    }
    public void MakeSynapses(List<Synapse> Synapses)
    {
        synapses = new List<Synapse>();
        for (int i = 0; i < Synapses.Count; i++)
        {
            synapses.Add(new Synapse(Synapses[i].multiplier, random));
        }
    }
    public Neuron Duplicate()
    {
        return new Neuron(bias, random);
    }
    public void Set(double Value, double bias)
    {
        this.Value = Value;
        this.bias = bias;
    }
    public void Randomise(double strength, float randChance, float destroyChance, bool doSynapse)
    {
        if (random.Next(0, 10000) / 100f < randChance)
            bias +=  random.Next(-1000, 1000) / 1000 * strength;
        if (doSynapse)
            for (int i = 0; i < synapses.Count; i++)
            {
                if (random.Next(0, 10000) / 100f < destroyChance) {
                    synapses[i].toggleActive();
                    continue;
                }
                if (random != synapses[i].random)
                    synapses[i].random = random;
                synapses[i].Randomise(strength, randChance);
            }
    }
    
}
