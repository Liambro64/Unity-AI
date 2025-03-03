using UnityEngine;

public class GraphGenerator : MonoBehaviour
{
    public GameObject saveObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        makeSave();
        makeSave();
        makeSave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void makeSave()
    {
        //move the saves
        int rand = Random.Range(-5, 5);
        Instantiate(saveObj, new Vector3(Random.Range(-1, 1) * rand, Random.Range(-1, 1) * rand, Random.Range(-1, 1) * rand),
                    Quaternion.Euler(0,0,0), gameObject.transform);
    }
}
