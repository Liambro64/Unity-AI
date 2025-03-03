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
public class NetworkManager : MonoBehaviour
{
    public float mutationStrength;
    public float mutationChance;
    public float destroyChance;
    public int networks;
    public int inputs;
    public int[] middle;
    public int outputs;
    public LayerMask raycastMask;
    public int generation;
	public float maxTime = 30;
	public float curTime = 0;
    List<GameObject> Agents = new List<GameObject>();
    public List<NeuralNetwork> brains = new List<NeuralNetwork>();
    public int nextGenAmount = 5;

    public float averageScore = 0;

    public GameObject agent;
    System.Random random;
    //show these ones

    public void Start()
    {
        random = new System.Random();
        for (int i = 0; i < networks; i++)
        {
            Agents.Add(Instantiate(agent, transform));
            brains.Add(Agents[i].GetComponent<NeuralNetwork>());
        }
		firstWipe();
        generation = 0;
    }

	public void Update()
	{
		bool quickie = true;
		for (int i = 0; i < networks; i++)
		{
			if (brains[i].active)
			{quickie = false;break;}
		}
		curTime += Time.deltaTime;
		if (quickie || curTime > maxTime)
		{
			MakeNewGen();
		}

	}

    public void firstWipe()
    {
        for (int i = 0; i < brains.Count; i++)
        {
            brains[i].make(inputs, middle, outputs, i, -1, raycastMask, random);
			brains[i].active = true;
        }
    }
    public void MakeNewGen()
    {
        NeuralNetwork[] best = new NeuralNetwork[nextGenAmount];
		curTime = 0;
		generation++;
        for (int i = 0; i < best.Length; i++)
        {
            best[i] = agent.GetComponent<NeuralNetwork>();
        }
        for (int i = 0; i < brains.Count; i++)
        {
            averageScore += (float)brains[i].score;
            for (int j = 0; j < best.Length; j++)
            {
                if (best[j].score < brains[i].score)
                {
					for (int k = best.Length-1; k > j; k--)
					{
						best[k] = best[k-1];
					}
					best[j] = brains[i];
                    break;
				}
            
            }
        }
        averageScore /= brains.Count;
		for (int i = 0; i < best.Length; i++)
		{
			best[i].gameObject.tag = "Best";
		}
		destroyCarOfTag("car");
		Agents.Clear();
		brains.Clear();
		makeNews(best);
    }
	public void makeNews(NeuralNetwork[] best)
	{
		for (int i = 0; i < best.Length; i++)
		{
			best[i].gameObject.tag = "car";
		}
        for (int i = 0; i < networks; i++)
        {
            Agents.Add(Instantiate(agent, transform));
            brains.Add(Agents[i].GetComponent<NeuralNetwork>());
            brains[i].Set(best[i/(networks/nextGenAmount)]);
			brains[i].Randomise(mutationStrength, mutationChance, destroyChance);
        }
		for (int i = 0; i < best.Length; i++)
		{
            if (best[i].gameObject != agent)
			    Destroy(best[i].gameObject);
		}
	}

	public void destroyCarOfTag(string tag)
	{
		GameObject[] gos = GameObject. FindGameObjectsWithTag(tag); foreach(GameObject go in gos) { Destroy(go);};
	}

}