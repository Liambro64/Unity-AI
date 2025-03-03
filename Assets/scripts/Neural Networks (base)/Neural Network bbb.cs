using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class NeuralNetwork_bbb
{
    public List<Neuron> inputs;
    public List<List<Neuron>> middle;
    public List<Neuron> outputs;
    public int number;
    public int basedoff;
    public double score;

    public bool active = true;

    public System.Random random = new System.Random();
    public NeuralNetwork_bbb(int inputs, int[] middle, int outputs, System.Random random)
    {
        this.inputs = new List<Neuron>();
        this.middle = new List<List<Neuron>>();
        this.outputs = new List<Neuron>();
        MakeInputs(inputs, middle[0]);
        MakeMiddle(inputs, middle, outputs);
        MakeOutputs(middle, outputs);
        this.random = random;
    }
    public NeuralNetwork_bbb(NeuralNetwork network)
    {
        this.inputs = new List<Neuron>();
        this.middle = new List<List<Neuron>>();
        this.outputs = new List<Neuron>();
        active = true;
        //start with outputs
        for (int i = 0; i < network.outputs.Count; i++)
        {
            outputs.Add(new Neuron(network.outputs[i].bias, random));
        }
        List<List<Neuron>> tmpMid = new List<List<Neuron>>();
        for (int i = 0; i < network.middle.Count; i++)
        {
            List<Neuron> tmp = new List<Neuron>();
            for (int j = 0; j < network.middle[i].Count; j++)
            {
                tmp.Add(new Neuron(network.middle[i][j], i == 0 ? outputs.ToArray() : tmpMid[i-1].ToArray(), random));
            }
            tmpMid.Add(tmp);
        }
        for (int i = tmpMid.Count - 1; i >= 0; i--)
        {
            middle.Add(tmpMid[i]);
        }
        for (int i = 0; i < network.inputs.Count; i++)
        {
            inputs.Add(new Neuron(network.inputs[i], middle[0].ToArray(), random));
        }
        random = network.random;

    }
    
    public double[] feedThrough(double[] inputs)
    {
        if (active)
        {
        for (int i = 0; i < inputs.Length; i++)
        {
            this.inputs[i].FeedForward(inputs[i]);
        }
        
        for (int i = 0; i < middle.Count; i++)
        {
            for (int j = 0; j < middle[i].Count; j++)
            {
                middle[i][j].FeedForward();
            }
        }
        List<double> outs = new List<double>();
        for (int i = 0; i < outputs.Count; i++)
        {
            outs.Add(outputs[i].Value);
            outputs[i].Value = 0;
        }
        return outs.ToArray();
        }
        return null;
    }
    void MakeInputs(int inputs, int middle)
    {
        for (int i = 0; i < inputs; i++)
        {
            this.inputs.Add(new Neuron(random));
            this.inputs[i].MakeSynapses(middle);
        }
    }
    void MakeMiddle(int inputs, int[] middle, int outputs)
    {
        List<Neuron> mid;
        for (int i = 0; i < middle.Length; i++)
        {
            mid = new List<Neuron>();   
            for (int j = 0; j < middle[i]; j++)
            {
                mid.Add(new Neuron(random));
                if (i < middle.Length - 1)
                    mid[j].MakeSynapses(middle[i+1]);
                if (i == middle.Length - 1)
                    mid[j].MakeSynapses(outputs);
                if (i == 0)
                    for (int k = 0; k < inputs; k++)
                        this.inputs[k].synapses[j].to = mid[j];
                else
                    for (int k = 0; k < middle[i-1]; k++)
                        this.middle[i-1][k].synapses[j].to = mid[j];
            }
            this.middle.Add(mid);
        }
    }
    void MakeOutputs(int[] middle, int outputs)
    {
        for (int i = 0; i < outputs; i++)
        {
            this.outputs.Add(new Neuron(random));
            for (int j = 0; j < middle[middle.Length-1]; j++)
            {
                this.middle[middle.Length-1][j].synapses[i].to = this.outputs[i];
            }
        }
    }
    public void Randomise(double strength, float randChance, float destroyChance)
    {
        for (int i = 0; i < inputs.Count; i++)
        {
            inputs[i].Randomise(strength, randChance, destroyChance, true);
        }
        for (int i = 0; i < middle.Count; i++)
        {
            for (int j = 0; j < middle[i].Count; j++)
            {
                middle[i][j].Randomise(strength, randChance, destroyChance, true);
            }
        }
        for (int i = 0; i < outputs.Count; i++)
        {
            outputs[i].Randomise(strength, randChance, destroyChance, false);
        }
    }
    
	public bool isActive()
	{
		return active;
	}
    public void addScore(double amount)
    {
        //Debug.Log("added score");
        score += amount;
    }
    public void multiplyscore(double amount)
    {
        score *= amount;
    }
}
