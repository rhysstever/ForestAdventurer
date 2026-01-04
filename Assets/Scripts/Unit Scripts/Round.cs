using System.Collections.Generic;
using UnityEngine;

public class Round
{
    private List<GameObject> enemies;

    public List<GameObject> Enemies { get { return enemies; } }

    public Round(List<GameObject> enemies)
    {
        this.enemies = enemies;
    }
}
