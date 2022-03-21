using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public GameObject InfoRoot;
    public Enigma enigma;
    void Start()
    {
        
    }

    public void ToggleRoot(){
        InfoRoot.SetActive(!InfoRoot.activeSelf);
        enigma.InputLocked = InfoRoot.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
