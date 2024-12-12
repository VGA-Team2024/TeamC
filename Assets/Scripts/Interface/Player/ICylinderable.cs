using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICylinderable
{
    int CylinderCount { get; }
    
    void AddCylinder();
}
