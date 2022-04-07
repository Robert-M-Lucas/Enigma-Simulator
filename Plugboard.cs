using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Plugboard : MonoBehaviour
{
    public List<Tuple<int, int>> Plugs = new List<Tuple<int, int>>();

    public TMP_Text ButtonText;

    public Enigma enigma;
    private bool setting = false;

    public TMP_Text PlugboardText;

    private int from = -1;
    private int to = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public int plugboard(int letter_in){
        foreach (Tuple<int, int> plug in Plugs){
            if (plug.Item1 == letter_in){
                return plug.Item2;
            }
            else if (plug.Item2 == letter_in){
                return plug.Item1;
            }
        }

        return letter_in;
    }

    public void ToggleSetting(){
        if (!setting){
            enigma.InputLocked = true;
            setting = true;
            ButtonText.text = "Stop Configuring";
        }
        else{
            enigma.InputLocked = false;
            setting = false;
            ButtonText.text = "Configure Plugboard";
        }
    }

    void AddPlug(){
        int offset = 0;
        for (int i = 0; i < Plugs.Count; i++){
            if (Plugs[i-offset].Item1 == from | Plugs[i-offset].Item1 == to | Plugs[i-offset].Item2 == from | Plugs[i-offset].Item2 == to){
                Plugs.RemoveAt(i-offset);
                offset++;
            }
        }
        if (from != to){
            Plugs.Add(new Tuple<int, int>(from, to));
        }
        from = -1;
        to = -1;
        string new_text = "";
        foreach (Tuple<int, int> plug in Plugs){
            new_text += enigma.rotors.alph[plug.Item1];
            new_text += "-";
            new_text += enigma.rotors.alph[plug.Item2];
            new_text += "   ";
        }
        PlugboardText.text = new_text;
    }

    // Update is called once per frame
    void Update()
    {
        if (setting)
        {
            if (Input.GetKeyDown(KeyCode.Return)){
                ToggleSetting();
                return;
            }

            for (int i = 0; i < KeyboardData.Keys.Length; i++){
                if (Input.GetKeyDown(KeyboardData.Keys[i])) { 
                    if (from == -1){
                        from = i;
                    }
                    else{
                        to = i;
                        AddPlug();
                    }
                }
            }
        }
    }
}
