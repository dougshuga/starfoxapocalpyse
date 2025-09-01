using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Reticle reticle;

    // Start is called before the first frame update
    void Start()
    {
        reticle = FindObjectOfType<Reticle>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(
            reticle.transform,
            Vector3.up
        );
    }
}
