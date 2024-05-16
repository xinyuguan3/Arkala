using UnityEngine;
using Arkala.Enum;

[CreateAssetMenu(fileName = "Agent_SO", menuName = "Agent_SO", order = 0)]
public class Agent_SO : ScriptableObject {
    [TextArea(3,10)]
    public string agentPrompt;
    public string StartPrompt;
    
    // [SerializeField] public Persona[] personas;
    public Memory[] agentMemory;
    public int hunger;
    public int rest;
    public int entertain;
    public Persona[] personas= new Persona[3];
    
}

