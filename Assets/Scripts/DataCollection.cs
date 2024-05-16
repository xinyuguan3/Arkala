using System.Collections.Generic;
using UnityEngine;
using Arkala.Enum;

[System.Serializable]
public class Agent{

}

[System.Serializable]
public class Memory{
    public string content;
    public int recency;
}

[System.Serializable]
    public struct Persona
    {
        public string name;
        public Sprite image;
        public TraitType type;
        public string description;
    }


