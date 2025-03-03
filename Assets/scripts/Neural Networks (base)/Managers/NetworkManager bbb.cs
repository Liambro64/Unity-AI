using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
//todo  :
//add thing that makes them better
public class NetworkManager_bbb : MonoBehaviour
{
    public float mutationStrength;
    public float mutationChance;
    public float destroyChance;
    public float networks;
    public int generation;
    public List<NeuralNetwork> neuralNetwork = new List<NeuralNetwork>();
    public List<GameObject>     cars = new List<GameObject>();
    public GameObject car;

    System.Random random = new System.Random();
    public NetworkManager_bbb(int inputs, int[] middle, int outputs, int networks, float mutationStrength, float mutationChance, float destroyChance)
    {
        for (int i = 0; i < networks; i++)
        {
            cars.Add(Instantiate(car));
            NeuralNetwork net = cars[i].GetComponent<NeuralNetwork>();
            neuralNetwork.Add(net.make(inputs, middle, outputs, -1, -1, new LayerMask(), random));
        }
        generation = 0;
        this.networks = networks;
        this.mutationStrength = mutationStrength;
        this.mutationChance = mutationChance;
        this.destroyChance = destroyChance;
    }

    public void Start()
    {
    }










    // network stuff
    //---------------
    //---------------
    public void MakeNewGen()
    {
        //getting best 5
        NeuralNetwork neuralNetwork1 = neuralNetwork[0];
        for (int i = 0; i < neuralNetwork.Count; i++)
        {
            if (neuralNetwork1.score < neuralNetwork[i].score)
            {
                neuralNetwork1 = neuralNetwork[i];
            }
        }
        Debug.Log("score: " + neuralNetwork1.score);
        //deleting others
        neuralNetwork.Clear();
        //duplicating best
        for (int i = 0; i < networks; i++)
        {
			neuralNetwork.Add(new NeuralNetwork(neuralNetwork1));
        }
        generation += 1;
    }

    public void giveOneScore(double amount, int index)
    {
        neuralNetwork[index].addScore(amount);
    }
    public double[] feedOne(double[] inputs, int index)
    {
        return neuralNetwork[index].feedThrough(inputs);
    }
    public double[][] feedAll(double[][] inputs)
    {
        double[][] outs = new double[inputs.Length][];
        for (int i = 0; i < inputs.Length; i++)
        {
            feedThrough(neuralNetwork[i], inputs[i], outs, i);
        }
        while (allIsntThere(outs))
        {}
        return outs;
    }

    bool allIsntThere(double[][] outputs)
    {
        for (int i = 0; i < outputs.Length; i++)
        {
            if (outputs[i] == null)
                return true;
        }
        return false;
    }

    async void feedThrough(NeuralNetwork network, double[] inputs, double[][] outputs, int index)
    {
        outputs[index] = await Task.Run(() => network.feedThrough(inputs));
        //Debug.Log("done!");
    }

    public void Randomise()
    {
        for (int i = 0; i < neuralNetwork.Count; i++)
        {
            neuralNetwork[i].Randomise(mutationStrength, mutationChance, destroyChance);
        }
    }
    public bool isAllDead()
    {
		bool allDead = true;
        for (int i = 0; i < neuralNetwork.Count; i++)
		{
			if (neuralNetwork[i].active)
			{
				allDead = false;
				break;
			}
		}
		return allDead;
    }

}