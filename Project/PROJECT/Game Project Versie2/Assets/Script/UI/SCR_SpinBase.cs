using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCR_SpinBase : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * 10);
    }
}