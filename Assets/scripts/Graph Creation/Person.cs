using Unity.VisualScripting;
using UnityEngine;

public class Person
{
    public string Name;
    float refnum;
    public Person(string name, float refnum)
    {
        Name = name;
        this.refnum = refnum;
    }
    int compare(float refnum)
    {
        if (refnum == this.refnum)
            return (1);
        return (0);
    }
}
