using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public string face = "SlightlySmilingFace";
    public List<string> unlockedFaces = new List<string>() { "SlightlySmilingFace", "AngryFace", "PensiveFace", "RelievedFace" };

    public PlayerData (PlayerStatus player)
    {
        face = player.face;
        unlockedFaces = player.unlockedFaces;
    }
}
