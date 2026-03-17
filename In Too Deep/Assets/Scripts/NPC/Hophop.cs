using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hophop : MonoBehaviour
{
    [SerializeField] GameObject coffee;
    public void spawn_coffee()
    {
        coffee.SetActive(true);
    }
}
