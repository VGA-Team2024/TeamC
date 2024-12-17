using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICylinderAddable
{
    int CylinderCount { get; }
    
    void AddCylinder();
}
