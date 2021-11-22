using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodController : MonoBehaviour
{
    PlayerStatus player;

    public GameObject angryButton;
    public GameObject disappointedButton;
    public GameObject happyButton;

    void Start()
    {
        player = FindObjectOfType<PlayerStatus>();

        player.LoadPlayer();

        SetSelectedMood(player.face);
    }

    public void OnClickMood(string face)
    {
        SetSelectedMood(face);
        player.face = face;
        player.SavePlayer();
    }

    void SetSelectedMood(string face)
    {
        switch(face)
        {
            case "SlightlySmilingFace":
                angryButton.GetComponent<Image>().color = new Color32(178, 231, 171, 255);
                disappointedButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                happyButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                break;
            case "AngryFace":
                angryButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                disappointedButton.GetComponent<Image>().color = new Color32(178, 231, 171, 255);
                happyButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                break;
            case "PensiveFace":
                angryButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                disappointedButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                happyButton.GetComponent<Image>().color = new Color32(178, 231, 171, 255);
                break;
            default:
                break;
        }
    }
}
