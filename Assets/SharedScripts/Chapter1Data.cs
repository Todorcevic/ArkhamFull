using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1Data
{
    public bool LeadHomeUp { get; set; }
    public bool GhoulPriestAlive { get; set; }
    public bool LitaLeave { get; set; }
    public Dictionary<string, bool> Cultists { get; } = new Dictionary<string, bool>();
    public bool IsMidNight { get; set; }

}
