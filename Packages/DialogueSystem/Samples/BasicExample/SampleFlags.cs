using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;

public class SampleFlags : MonoBehaviour
{
    public List<GameEventFlag> gameEventFlags = new List<GameEventFlag>();

    // Start is called before the first frame update
    void Start()
    {
        GameEventFlag A = new GameEventFlag("A", false);
        GameEventFlag B = new GameEventFlag("B", true);

        gameEventFlags.Add(A);
        gameEventFlags.Add(B);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
