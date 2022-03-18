using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keyboard : MonoBehaviour
{
    public TMP_Text[] Keys;

    private int Lit = -1;

    public void Unlight(int index){
        Keys[index].color = Color.white;
    }

    public void Light(int index){
        if (Lit != -1){ Unlight(Lit); }
        Keys[index].color = Color.yellow;
        Lit = index;
    }
}
