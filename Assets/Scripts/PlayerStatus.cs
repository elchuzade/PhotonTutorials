using UnityEngine;
using System.Collections.Generic;

public class PlayerStatus : MonoBehaviour
{
    public string face = "SlightlySmilingFace";
    public List<string> unlockedFaces = new List<string>() { "SlightlySmilingFace", "AngryFace", "PensiveFace", "RelievedFace" };

    void Awake()
    {
        // Singleton
        int instances = FindObjectsOfType<PlayerStatus>().Length;
        if (instances > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void ResetPlayer()
    {
        face = "SlightlySmilingFace";
        unlockedFaces = new List<string>() { "SlightlySmilingFace", "AngryFace", "PensiveFace", "RelievedFace"};

        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
        {
            ResetPlayer();
            data = SaveSystem.LoadPlayer();
        }

        face = data.face;
        unlockedFaces = data.unlockedFaces;
    }
}
