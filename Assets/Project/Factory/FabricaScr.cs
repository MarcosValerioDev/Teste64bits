using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PatternFactory;

public class FabricaScr : MonoBehaviour
{
    [SerializeField] Fabrica fabrica = new Fabrica();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) fabrica.Fabricar("Green");
        if (Input.GetKeyDown(KeyCode.B)) fabrica.Fabricar("Red");
        if (Input.GetKeyDown(KeyCode.C)) fabrica.Fabricar("Blue");
    }
}
