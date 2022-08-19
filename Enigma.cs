using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enigma : MonoBehaviour
{
    public Keyboard InputKeyboard;
    public Keyboard OutputKeyboard;

    [HideInInspector]
    public bool InputLocked = false;

    public Rotors rotors;
    public OutputManager outputManager;

    public Plugboard plugboard;

    public TMP_Text DebugTextLeft;
    public TMP_Text DebugTextRight;

    

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
            letter = EnigmaData.RotorEncodings[rotors.rotor_type[r], letter];
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
            letter = EnigmaData.RotorEncodings[rotors.rotor_type[0], letter];
            out_str += "[W" + 0 + " encode] " + rotors.alph[letter] + "\n";
            letter -= rotors.rotor_pos[0];
            if (letter < 0){
                letter += 26;
            }
            out_str += "[W" + 0 + " rotated] " +  rotors.alph[letter] + "-->\n";
        }

        letter = EnigmaData.ReflectorEncodings[rotors.reflector, letter];
        out_str += "[Reflector] " + rotors.alph[letter] + "->";
        DebugTextLeft.text = out_str;
        out_str = "";

        if (rotors.rotor_type[0] != -1){
            letter += rotors.rotor_pos[0];
            if (letter > 25){
                letter -= 26;
            }
            out_str += "[W" + 0 + " rotated] " + rotors.alph[letter] + "\n";
            letter = ReflectArr(EnigmaData.RotorEncodings, rotors.rotor_type[0], letter);
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
            letter = ReflectArr(EnigmaData.RotorEncodings, rotors.rotor_type[r], letter);
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
        outputManager.AddLetter(rotors.alph[letter_in].ToString(), rotors.alph[letter].ToString());
        OutputKeyboard.Light(letter);
    }

    // Update is called once per frame
    void Update()
    {
        if (!InputLocked){
            for (int i = 0; i < KeyboardData.Keys.Length; i++){
                if (Input.GetKeyDown(KeyboardData.Keys[i])) { InputKeyboard.Light(i); rotors.LinkedIncrement(); EnigmaAlgorithm(i); }
            }

            if (Input.GetKeyDown(KeyCode.Backspace) && outputManager.total_letters > 0){
                outputManager.DeleteLetter();
                rotors.LinkedDecrement();
            }
        }
    }
}
