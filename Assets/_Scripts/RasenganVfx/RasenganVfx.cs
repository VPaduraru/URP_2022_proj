using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasenganVfx : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 10f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
