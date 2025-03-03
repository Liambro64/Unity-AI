using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class NeuralNetwork : MonoBehaviour
{
    public List<Neuron> inputs;
    public List<List<Neuron>> middle;
    public List<Neuron> outputs;
    public int number;
    public LayerMask layermask;
    public int basedoff;
    public double score;

    public bool active = true;

    float speedMulti = 10000;
    float turnMulti = 1;

    public int next = 1;
    Rigidbody rb;

    public System.Random random = new System.Random();
    public NeuralNetwork(int inputs, int[] middle, int outputs, System.Random random)
    {
        this.inputs = new List<Neuron>();
        this.middle = new List<List<Neuron>>();
        this.outputs = new List<Neuron>();
        MakeInputs(inputs, middle[0]);
        MakeMiddle(inputs, middle, outputs);
        MakeOutputs(middle, outputs);
        this.random = random;
    }
    public NeuralNetwork(NeuralNetwork network)
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
                tmp.Add(new Neuron(network.middle[i][j], i == 0 ? outputs.ToArray() : tmpMid[i - 1].ToArray(), random));
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
    public NeuralNetwork make(int inputs, int[] middle, int outputs, int number, int basedoff, LayerMask layermask, System.Random random)
    {
        this.inputs = new List<Neuron>();
        this.middle = new List<List<Neuron>>();
        this.outputs = new List<Neuron>();
        this.layermask = layermask;
        this.random = random;
        this.number = number;
        this.basedoff = basedoff;
        MakeInputs(inputs, middle[0]);
        MakeMiddle(inputs, middle, outputs);
        MakeOutputs(middle, outputs);
        rb = gameObject.GetComponent<Rigidbody>();
        return this;
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
                    mid[j].MakeSynapses(middle[i + 1]);
                if (i == middle.Length - 1)
                    mid[j].MakeSynapses(outputs);
                if (i == 0)
                    for (int k = 0; k < inputs; k++)
                        this.inputs[k].synapses[j].to = mid[j];
                else
                    for (int k = 0; k < middle[i - 1]; k++)
                        this.middle[i - 1][k].synapses[j].to = mid[j];
            }
            this.middle.Add(mid);
        }
    }
    void MakeOutputs(int[] middle, int outputs)
    {
        for (int i = 0; i < outputs; i++)
        {
            this.outputs.Add(new Neuron(random));
            for (int j = 0; j < middle[middle.Length - 1]; j++)
            {
                this.middle[middle.Length - 1][j].synapses[i].to = this.outputs[i];
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

    //List<double> ins = new List<double>();
    //for (int j = -90; j <= 90; j += 45)
    //{
    //	RaycastHit rch = new RaycastHit();
    //	Physics.Raycast(cars[i].transform.position, Quaternion.Euler(0, j, 0) * cars[i].transform.up, out rch);
    //	//Debug.DrawRay(cars[i].transform.position, Quaternion.Euler(j, 0, 0) * cars[i].transform.up, Color.red, 0.1f);
    //	//Debug.DrawRay(cars[i].transform.position, Quaternion.Euler(0, j, 0) * cars[i].transform.up, Color.green, 0f); correct
    //	//Debug.DrawRay(cars[i].transform.position, Quaternion.Euler(0, 0, j) * cars[i].transform.up, Color.blue, 0.1f);
    //	ins.Add(1/rch.distance);
    //}
    //double[] outputs = networkManager.feedOne(ins.ToArray(), i);
    //if (outputs == null)
    //{	
    //	//print("its null");
    //	return;
    //}
    ////Debug.LogWarning(i + "\nmove: " + outputs[0] + "\nturn: " + outputs[1]);
    //cars[i].transform.Rotate(0, 0, (float)outputs[1]*5);
    //cars[i].transform.Translate(cars[i].transform.forward*(float)outputs[0]);
    public void Update()
    {
        if (active)
        {
            double[] ins = new double[inputs.Count];
            for (int i = 0; i < inputs.Count; i++)
            {
                RaycastHit hit;
                Physics.Raycast(transform.position, Quaternion.Euler(0, -i * 45 - 90, 0) * transform.up, out hit, 500, layermask);
                Debug.DrawRay(transform.position, Quaternion.Euler(0, -i * 45 - 90, 0) * transform.up*hit.distance, Color.red, 5f);
                ins[i] = 1 / hit.distance;
            }
            print("Ins: "
            +ins[0]+", "
            +ins[1]+", "
            +ins[2]+", "
            +ins[3]+", "
            +ins[4]);
            double[] outs = feedThrough(ins);
            //move and turn through rigidbody
            rb.AddForce(transform.up * (float)(outs[0] * speedMulti * Time.deltaTime));
            rb.AddTorque(Vector3.forward * (float)(outs[1] * turnMulti * Time.deltaTime));
            print("Outs: "+outs[0]+", "+outs[1]);

            //move and turn through transform
            //transform.Rotate(0, 0, (float)ins[1]*720*Time.deltaTime);
            //transform.Translate(transform.forward*(float)ins[0]*(100f*Time.deltaTime));
            //score += ins[0]*0.05;
        }
    }
    List<Neuron> mid;
    public void Set(NeuralNetwork network)
    {
        this.inputs = new List<Neuron>();
        this.middle = new List<List<Neuron>>();
        this.outputs = new List<Neuron>();
        random = network.random;
        for (int i = 0; i < network.outputs.Count; i++)
        {
            outputs.Add(new Neuron(network.outputs[i].bias, random));
        }
        print(outputs.Count);
        List<List<Neuron>> bkwdMid = new List<List<Neuron>>();
        for (int i = network.middle.Count - 1; i >= 0; i--)
        {
            mid = new List<Neuron>();
            for (int j = 0; j < network.middle[i].Count; j++)
            {

                mid.Add(new Neuron(network.middle[i][j], i == network.middle.Count - 1 ? outputs.ToArray() : bkwdMid[Mathf.Abs(i - (network.middle.Count - 2))].ToArray(), random));
            }
            bkwdMid.Add(mid);
        }
        print(bkwdMid.Count);
        for (int i = 1; i <= bkwdMid.Count; i++)
        {
            middle.Add(bkwdMid[bkwdMid.Count - i]);
        }
        for (int i = 0; i < network.inputs.Count; i++)
        {
            inputs.Add(new Neuron(network.inputs[i], middle[0].ToArray(), random));
        }
        number = network.number;
        basedoff = network.basedoff;
        layermask = network.layermask;
        score = 0;
        active = true;
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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
            active = false;
        print("layer: " + collision.gameObject.layer);
    }
    public void OnTriggerEnter(Collider other)
    {
        print("trigger layer: " + other.gameObject.layer);
        if (other.gameObject.layer == 3)
            active = false;
        if (other.gameObject.layer == 6)
        {
            if (Int32.Parse(other.gameObject.name) == next)
            {
                score += ++next;
                if (next > 13)
                    next = 1;
            }
        }
    }
    //redundant editor variables
    public bool open;
}
