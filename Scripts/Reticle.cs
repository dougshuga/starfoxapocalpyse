using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    [SerializeField] float xMin = -15;
    [SerializeField] float xMax = 15;
    [SerializeField] float yMin = -15;
    [SerializeField] float yMax = -15;
    [SerializeField] float speedFactor = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveReticle(float horizontalUpdate, float verticalUpdate)
    {
        transform.localPosition = new Vector3(
            Mathf.Clamp(transform.localPosition.x + horizontalUpdate * speedFactor, xMin, xMax),
            Mathf.Clamp(transform.localPosition.y + verticalUpdate * speedFactor, yMin, yMax),
            transform.localPosition.z
        );
    }
}
