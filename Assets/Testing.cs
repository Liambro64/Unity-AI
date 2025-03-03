using Unity.Mathematics;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public GameObject x;
    public GameObject y;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        y = Instantiate(x);
        y.name = "y";
    }

}
