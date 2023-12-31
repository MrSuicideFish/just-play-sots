using System;
using UnityEngine;

[Serializable]
public class Caption
{
    public double time;
    public string text;
}

[CreateAssetMenu(menuName = "In the Shadow of the Sun/Captions Info")]
public class CaptionsInfo : ScriptableObject
{
    public string preShowLabel = "[ ENGLISH CAPTIONS ]";
    public double startTime, endTime;
    public Caption[] captions;
    
}
