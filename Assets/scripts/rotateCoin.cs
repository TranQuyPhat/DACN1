using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateCoin : MonoBehaviour
{
    private bool shouldUpdate = true;
    public float speed = 85f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!shouldUpdate) return;
        transform.rotation *= Quaternion.Euler(0f, 0f, speed * Time.deltaTime);
    }
}
