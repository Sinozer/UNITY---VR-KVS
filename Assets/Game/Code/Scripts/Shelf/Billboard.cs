using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    
    // Summary
    //UI can be seen from all angles
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
