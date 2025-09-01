using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField] ParticleSystem impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Ground impact detected.");
        /* TODO: fix this: https://www.youtube.com/watch?v=C3pf9XUsEtM
        Instantiate(
            impactEffect,
            new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z),
            new Quaternion(0, 0, 0, 1)
        );
        */
    }
}
