using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkVisualizer : MonoBehaviour
{
    public NeuralNetwork network;
    public GameObject empty;
    public Transform inputs;
    public Transform middle;
    public Transform outputs;
    public Material lineMaterial;
    public GameObject Neuron;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NeuralNetwork oldnetwork = new NeuralNetwork(3, new int[3] {5, 3, 4}, 2, new System.Random());
        network = new NeuralNetwork(oldnetwork);
        //make the visualisation
        makeNetwork();
    }

    void makeNetwork()
    {
        int offset = 0;
        int off = 4 + 1*network.middle.Count; //halfs
        for (int i = 0; i < network.inputs.Count; i++)
        {
            Instantiate(Neuron, new Vector3(offset - off, i*3 - network.inputs.Count, 0), Quaternion.Euler(0, 0, 0), inputs);
        }
        offset += 4;
        for (int i = 0; i < network.middle.Count; i++)
        {
            Transform current = Instantiate(empty, middle).transform;
            current.gameObject.name = i.ToString();
            for (int j = 0; j < network.middle[i].Count; j++)
            {
                Transform cur = Instantiate(Neuron, new Vector3(offset - off, j*3 - network.middle[i].Count, 0), Quaternion.Euler(0, 0, 0), current).transform;
                makeLineRenderer(cur, i == 0 ? inputs : middle.GetChild(i - 1), i == 0 ? network.inputs.Count : network.middle[i - 1].Count);
            }
            offset += 2;
        }
        offset += 2;
        for (int i = 0; i < network.outputs.Count; i++)
        {
            Transform cur = Instantiate(Neuron, new Vector3(offset - off, i*3 - network.outputs.Count, 0), Quaternion.Euler(0, 0, 0), outputs).transform;
            makeLineRenderer(cur, middle.GetChild(network.middle.Count - 1).transform, network.middle[network.middle.Count - 1].Count);
        }
    }

    void makeLineRenderer(Transform current, Transform container, int count)
    {
        print(count);
        for (int i = 0; i < count; i++)
        {
            LineRenderer lr;
            container.GetChild(i).TryGetComponent<LineRenderer>(out lr);
            if (lr == null)
            {
                lr = container.GetChild(i).AddComponent<LineRenderer>();
                lr.material = lineMaterial;
                lr.positionCount = 3;
                lr.SetPositions(new Vector3[] {lr.gameObject.transform.position, current.position, lr.gameObject.transform.position});
                lr.startWidth = 0.2f;
                lr.endWidth = 0.2f;
            }
            List<Vector3> full = new List<Vector3>();
            full.Add(lr.gameObject.transform.position);
            full.Add(current.position);
            full.Add(lr.gameObject.transform.position);
            Vector3[] bit = new Vector3[lr.positionCount];
            lr.GetPositions(bit);
            full.AddRange(bit);
            lr.positionCount += 3;
            lr.SetPositions(full.ToArray());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
