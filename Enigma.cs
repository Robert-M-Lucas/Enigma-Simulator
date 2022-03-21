using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enigma : MonoBehaviour
{
    [HideInInspector]
    public KeyCode[] Keys = new KeyCode[] {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G,
        KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P,
        KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z
    };
    
    public Keyboard InputKeyboard;
    public Keyboard OutputKeyboard;

    [HideInInspector]
    public bool InputLocked = false;

    public Rotors rotors;
    public OutputManager outputManager;

    public Plugboard plugboard;

    public TMP_Text DebugTextLeft;
    public TMP_Text DebugTextRight;

    private int[,] RotorEncodings = new int[,] {
        { 4, 10, 12, 5, 11, 6, 3, 16, 21, 25, 13, 19, 14, 22, 24, 7, 23, 20, 18, 15, 0, 8, 1, 17, 2, 9 },
        { 0, 9, 3, 10, 18, 8, 17, 20, 23, 1, 11, 7, 22, 19, 12, 2, 16, 6, 25, 13, 15, 24, 5, 21, 14, 4 },
        { 1, 3, 5, 7, 9, 11, 2, 15, 17, 19, 23, 21, 25, 13, 24, 4, 8, 22, 6, 0, 10, 12, 20, 18, 16, 14 },
        { 4, 18, 14, 21, 15, 25, 9, 0, 24, 16, 20, 8, 17, 7, 23, 11, 13, 5, 19, 6, 10, 3, 2, 12, 22, 1 },
        { 21, 25, 1, 17, 6, 8, 19, 24, 20, 15, 18, 3, 13, 7, 11, 23, 0, 22, 12, 9, 16, 14, 5, 4, 2, 10 },
        { 9, 15, 6, 21, 14, 20, 12, 5, 24, 16, 1, 4, 13, 7, 25, 17, 3, 10, 0, 18, 23, 11, 8, 2, 19, 22 },
        { 13, 25, 9, 7, 6, 17, 2, 23, 12, 24, 18, 22, 1, 14, 20, 5, 0, 8, 21, 11, 15, 4, 10, 16, 3, 19 },
        { 5, 10, 16, 7, 19, 11, 23, 14, 2, 1, 9, 18, 15, 3, 25, 17, 0, 12, 4, 22, 13, 8, 20, 24, 6, 21 },
        { 11, 4, 24, 9, 21, 2, 13, 8, 23, 22, 15, 1, 16, 12, 3, 17, 19, 0, 10, 25, 6, 5, 20, 7, 14, 18 },
        { 5, 18, 14, 10, 0, 13, 20, 4, 17, 7, 12, 1, 19, 8, 24, 2, 22, 11, 16, 15, 25, 23, 21, 6, 9, 3 },
    }; // I -> VIII + B + G https://en.wikipedia.org/wiki/Enigma_rotor_details#Rotor_wiring_tables

    private int[,] ReflectorEncodings = new int[,] {
        { 4, 9, 12, 25, 0, 11, 24, 23, 21, 1, 22, 5, 2, 17, 16, 20, 14, 13, 19, 18, 15, 8, 10, 7, 6, 3 },
        { 24, 17, 20, 7, 16, 18, 11, 3, 15, 23, 13, 6, 14, 10, 12, 8, 4, 1, 5, 25, 2, 22, 21, 9, 0, 19 },
        { 5, 21, 15, 9, 8, 0, 14, 24, 4, 3, 17, 25, 23, 22, 6, 2, 19, 10, 20, 16, 18, 1, 13, 12, 7, 11 },
        { 4, 13, 10, 16, 0, 20, 24, 22, 9, 8, 2, 14, 15, 1, 11, 12, 3, 23, 25, 21, 5, 19, 7, 17, 6, 18 },
        { 17, 3, 14, 1, 9, 13, 19, 10, 21, 4, 7, 12, 11, 5, 2, 22, 25, 0, 23, 6, 24, 8, 15, 18, 20, 16 },
    }; // A, B, C, B Thin, C Thin

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Simulates electricity flowing backwards though a rotor
    int ReflectArr(int[,] arr, int type, int letter){
        for (int i = 0; i < 26; i++){
            if (arr[type,i] == letter){
                return i;
            }
        }
        return -1;
    }

    void EnigmaAlgorithm(int letter_in){
        string out_str = "";
        out_str += "[Letter in] " + rotors.alph[letter_in] + "-->\n";
        int letter = letter_in;

        letter = plugboard.plugboard(letter);
        out_str += "[Plugboard] " + rotors.alph[letter] + "-->\n";

        for (int r = 3; r > 0; r--){
            letter += rotors.rotor_pos[r];
            if (letter > 25){
                letter -= 26;
            }
            out_str += "[W" + r + " rotated] " + rotors.alph[letter] + "\n";
            letter = RotorEncodings[rotors.rotor_type[r], letter];
            out_str += "[W" + r + " encode] " + rotors.alph[letter] + "\n";
            letter -= rotors.rotor_pos[r];
            if (letter < 0){
                letter += 26;
            }
            out_str += "[W" + r + " rotated] " +  rotors.alph[letter] + "-->\n";
        }

        if (rotors.rotor_type[0] != -1){
            letter += rotors.rotor_pos[0];
            if (letter > 25){
                letter -= 26;
            }
            out_str += "[W" + 0 + " rotated] " + rotors.alph[letter] + "\n";
            letter = RotorEncodings[rotors.rotor_type[0], letter];
            out_str += "[W" + 0 + " encode] " + rotors.alph[letter] + "\n";
            letter -= rotors.rotor_pos[0];
            if (letter < 0){
                letter += 26;
            }
            out_str += "[W" + 0 + " rotated] " +  rotors.alph[letter] + "-->\n";
        }

        letter = ReflectorEncodings[rotors.reflector, letter];
        out_str += "[Reflector] " + rotors.alph[letter] + "->";
        DebugTextLeft.text = out_str;
        Debug.Log(out_str);
        out_str = "";

        if (rotors.rotor_type[0] != -1){
            letter += rotors.rotor_pos[0];
            if (letter > 25){
                letter -= 26;
            }
            out_str += "[W" + 0 + " rotated] " + rotors.alph[letter] + "\n";
            letter = ReflectArr(RotorEncodings, rotors.rotor_type[0], letter);
            out_str += "[W" + 0 + " encoded] " + rotors.alph[letter] + "\n";
            letter -= rotors.rotor_pos[0];
            if (letter < 0){
                letter += 26;
            }
            out_str += "[W" + 0 + " rotated] " +  rotors.alph[letter] + "-->\n";
        }

        for (int r = 1; r < 4; r++){
            letter += rotors.rotor_pos[r];
            if (letter > 25){
                letter -= 26;
            }
            out_str += "[W" + r + " rotated] " + rotors.alph[letter] + "\n";
            letter = ReflectArr(RotorEncodings, rotors.rotor_type[r], letter);
            out_str += "[W" + r + " encoded] " + rotors.alph[letter] + "\n";
            letter -= rotors.rotor_pos[r];
            if (letter < 0){
                letter += 26;
            }
            out_str += "[W" + r + " rotated] " +  rotors.alph[letter] + "-->\n";
        }

        letter = plugboard.plugboard(letter);
        out_str += "[Plugboard] " + rotors.alph[letter] + "-->";
        DebugTextRight.text = out_str;
        Debug.Log(out_str);
        outputManager.AddLetter(rotors.alph[letter_in].ToString(), rotors.alph[letter].ToString());
        OutputKeyboard.Light(letter);
    }

    // Update is called once per frame
    void Update()
    {
        if (!InputLocked){
            for (int i = 0; i < Keys.Length; i++){
                if (Input.GetKeyDown(Keys[i])) { InputKeyboard.Light(i); rotors.LinkedIncrement(); EnigmaAlgorithm(i); }
            }

            if (Input.GetKeyDown(KeyCode.Backspace) && outputManager.total_letters > 0){
                outputManager.DeleteLetter();
                rotors.LinkedDecrement();
            }
        }
    }
}
