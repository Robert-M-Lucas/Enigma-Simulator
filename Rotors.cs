using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rotors : MonoBehaviour
{
    public TMP_Text[] rotors;
    public TMP_Text[] rotor_type_text;

    public OutputManager outputManager;

    public TMP_Text reflector_text;
    [HideInInspector]
    public string alph = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    [HideInInspector]
    public int[] rotor_pos = new int[] {0, 0, 0, 0};
    [HideInInspector]
    public int[] rotor_type = new int[] { -1, 0, 0, 0 };

    // Rotor types and turnover positions https://en.wikipedia.org/wiki/Enigma_machine#Turnover
    private int reserved_rotors = 2;
    private string[] rotor_type_names = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "B", "G"};
    private int[,] rotor_hooks = new int[,] { {17, -1}, {5, -1}, {22, -1}, {10, -1}, {0, -1}, {0, 13}, {0, 13}, {0, 13}, {-1, -1}, {-1, -1} };
    private string[] reflector_names = new string[] { "A", "B", "C", "BT", "CT"};
    [HideInInspector]
    public int reflector;

    // Used when changing initial positions
    public void Increment(int rotor){
        if (rotor_type[rotor] == -1){
            return;
        }

        rotor_pos[rotor]++;
        if (rotor_pos[rotor] > 25){
            rotor_pos[rotor] = 0;
        }
        rotors[rotor].text = alph[rotor_pos[rotor]].ToString();
        outputManager.Reset();
    }

    public void CheckTurnover(int rotor){
        if (rotor_pos[rotor] > 25){
            rotor_pos[rotor] = 0;
        }

        if ((rotor_pos[rotor] == rotor_hooks[rotor_type[rotor], 0] | rotor_pos[rotor] == rotor_hooks[rotor_type[rotor], 1]) & rotor > 1){
            rotor_pos[rotor-1]++;
            CheckTurnover(rotor - 1);
        }
    }
    
    // Used when typing
    public void LinkedIncrement(){
        rotor_pos[3]++;
        CheckTurnover(3);
        rotors[3].text = alph[rotor_pos[3]].ToString();
        rotors[2].text = alph[rotor_pos[2]].ToString();
        rotors[1].text = alph[rotor_pos[1]].ToString();
    }

    public void CheckDeTurnover(int rotor){
        if (rotor_pos[rotor] < 0){
            rotor_pos[rotor] = 25;
        }

        int hook1 = rotor_hooks[rotor_type[rotor], 0] - 1;
        int hook2 = rotor_hooks[rotor_type[rotor], 1] - 1;
        if (hook1 < 0){
            hook1 += 26;
        }
        if (hook2 < 0){
            hook2 += 26;
        }

        if ((rotor_pos[rotor] == hook1 | rotor_pos[rotor] == hook2) & rotor > 1){
            rotor_pos[rotor-1]--;
            CheckDeTurnover(rotor - 1);
        }
    }

    // Used for backspace
    public void LinkedDecrement(){
        rotor_pos[3]--;
        CheckDeTurnover(3);
        rotors[3].text = alph[rotor_pos[3]].ToString();
        rotors[2].text = alph[rotor_pos[2]].ToString();
        rotors[1].text = alph[rotor_pos[1]].ToString();
    }

    // Used for setting initial rotors
    public void Decrement(int rotor){
        if (rotor_type[rotor] == -1){
            return;
        }

        rotor_pos[rotor]--;
        if (rotor_pos[rotor] < 0){
            rotor_pos[rotor] = 25;
        }
        rotors[rotor].text = alph[rotor_pos[rotor]].ToString();
        outputManager.Reset();
    }

    public void ChangeRotor(int rotor){
        rotor_type[rotor] += 1;
        if (rotor_type[rotor] >= rotor_type_names.Length - reserved_rotors){
            rotor_type[rotor] = 0;
        }
        rotor_type_text[rotor].text = rotor_type_names[rotor_type[rotor]];
        outputManager.Reset();
    }

    // Change leftmost rotor
    public void ChangeSpecialRotor(){
        if (rotor_type[0] == -1){
            rotor_type[0] = 8;
        }
        else if (rotor_type[0] == 8){
            rotor_type[0] = 9;
        }
        else{
            rotor_type[0] = -1;
            rotor_type_text[0].text = "-";
            return;
        }
        rotor_type_text[0].text = rotor_type_names[rotor_type[0]];
        outputManager.Reset();
    }

    public void ChangeReflector(){
        reflector++;
        if (reflector >= reflector_names.Length){ reflector = 0; }
        reflector_text.text = reflector_names[reflector];
        outputManager.Reset();
    }
}