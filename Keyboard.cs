using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class KeyboardData{
    public static KeyCode[] Keys = new KeyCode[] {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G,
        KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P,
        KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z
    };
}

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
