using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OutputManager : MonoBehaviour
{
    public TMP_Text input;
    public TMP_Text output;
    public Rotors rotors;

    public Enigma enigma;

    [HideInInspector]
    public int total_letters = 0;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    void LateUpdate(){
        if (!enigma.InputLocked){
            if (Input.GetKeyDown(KeyCode.Space)){
                AddLetter(" ", " ");
            }
            if (Input.GetKeyDown(KeyCode.Return)){
                AddLetter("\n", "\n");
            }
        }
    }

    public void Reset(){
        output.text = "";
        input.text = "[" + rotors.reflector_text.text + "] [" + 
        rotors.rotor_type_text[0].text + ":" + rotors.rotor_type_text[1].text + ":" + rotors.rotor_type_text[2].text + ":" + rotors.rotor_type_text[3].text + "] [" + 
        rotors.rotors[0].text + ":" + rotors.rotors[1].text + ":" + rotors.rotors[2].text + ":" + rotors.rotors[3].text + "]\n";
    }

    public void AddLetter(string letter_in, string letter_out){
        total_letters++;
        input.text += letter_in;
        output.text += letter_out;
    }
    public void DeleteLetter(){
        if (total_letters <= 0){
            return;            
        }
        char letter = input.text[input.text.Length-1];
        total_letters--;
        input.text = input.text.Substring(0, input.text.Length-1);
        output.text = output.text.Substring(0, output.text.Length-1);

        if (letter == ' ' | letter == '\n'){
            DeleteLetter();
        }
    }
}
