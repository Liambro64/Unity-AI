using System.Collections.Generic;
using UnityEngine;

public class Save
{
    List<Person> people;
    string name;
    public Save(string Name)
    {
        this.name = Name;
    }
    public Save(string Name, List<Person> People)
    {
        this.name   = Name;
        this.people = People;
    }
    
}
