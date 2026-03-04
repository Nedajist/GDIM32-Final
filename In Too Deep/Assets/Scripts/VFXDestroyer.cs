using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXDestroyer : MonoBehaviour
{
    [SerializeField] float lifespan_seconds = 2f;

    // Update is called once per frame
    void Update()
    {
        lifespan_seconds -= Time.deltaTime;
        if (lifespan_seconds <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
