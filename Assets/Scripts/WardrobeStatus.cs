using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static GlobalVariables;

public class WardrobeStatus : MonoBehaviour
{
    PlayerStatus player;

    [SerializeField] List<GameObject> allEmojies = new List<GameObject>();

    [SerializeField] GameObject scrollUpButton;
    [SerializeField] GameObject scrollDownButton;
    [SerializeField] GameObject scrollLeftButton;
    [SerializeField] GameObject scrollRightButton;

    [SerializeField] GameObject scrollMenu;

    [SerializeField] GameObject scrollMenuFace;
    [SerializeField] GameObject scrollMenuHead;
    [SerializeField] GameObject scrollMenuEyes;
    [SerializeField] GameObject scrollMenuMouth;
    [SerializeField] GameObject scrollMenuWrist;

    int faceIndex = 0;
    int headIndex = 0;
    int eyesIndex = 0;
    int mouthIndex = 0;
    int wristIndex = 0;

    // Sections 
    // WardrobeSection activeSection = WardrobeSection.Face;
    // Angle of the Scroll menu to show activeSection at the front
    int activeAngle = 198;

    int scrollStep = 72;
    float scrollSpeed = 0.5f;

    void Start()
    {
        player = FindObjectOfType<PlayerStatus>();
        //player.ResetPlayer();
        player.LoadPlayer();

        // DEFAULT SECTION IS FACE
        // Find face index based on current face and all unlocked faces
        SetFaceIndex();
        // Disable horizontal scroll buttons if current face is at the edge of a list
        SetHorizontalButtons(WardrobeSection.Face);
        // Instantiate default face that is selected by player
        InstantiateNewFace();
    }

    public void ScrollUp()
    {
        scrollUpButton.GetComponent<Button>().interactable = false;
        // Rotate sections parent object
        StartCoroutine(Rotate(scrollMenu, Vector3.right, scrollStep, scrollSpeed, "scrollMenu"));
        // Update active angle for scroll menu
        SetActiveSection(activeAngle + scrollStep);
        // Counter rotate each section inside sections parent
        StartCoroutine(Rotate(scrollMenuFace, Vector3.right, 0, scrollSpeed));
        StartCoroutine(Rotate(scrollMenuHead, Vector3.right, 0, scrollSpeed));
        StartCoroutine(Rotate(scrollMenuEyes, Vector3.right, 0, scrollSpeed));
        StartCoroutine(Rotate(scrollMenuMouth, Vector3.right, 0, scrollSpeed));
        StartCoroutine(Rotate(scrollMenuWrist, Vector3.right, 0, scrollSpeed));
    }

    public void ScrollDown()
    {
        scrollDownButton.GetComponent<Button>().interactable = false;
        // Rotate sections parent object
        StartCoroutine(Rotate(scrollMenu, Vector3.right, -scrollStep, scrollSpeed, "scrollMenu"));
        // Update active angle for scroll menu
        SetActiveSection(activeAngle - scrollStep);
        // Counter rotate each section inside sections parent
        StartCoroutine(Rotate(scrollMenuFace, Vector3.right, 0, scrollSpeed));
        StartCoroutine(Rotate(scrollMenuHead, Vector3.right, 0, scrollSpeed));
        StartCoroutine(Rotate(scrollMenuEyes, Vector3.right, 0, scrollSpeed));
        StartCoroutine(Rotate(scrollMenuMouth, Vector3.right, 0, scrollSpeed));
        StartCoroutine(Rotate(scrollMenuWrist, Vector3.right, 0, scrollSpeed));
    }

    public void ScrollLeft()
    {
        if (faceIndex > 0)
        {
            faceIndex--;
            player.face = player.unlockedFaces[faceIndex];
            player.SavePlayer();
            InstantiateNewFace();
            SetHorizontalButtons(WardrobeSection.Face);
        }
    }

    public void ScrollRight()
    {
        if (faceIndex < player.unlockedFaces.Count - 1)
        {
            faceIndex++;
            player.face = player.unlockedFaces[faceIndex];
            player.SavePlayer();
            InstantiateNewFace();
            SetHorizontalButtons(WardrobeSection.Face);
        }
    }

    IEnumerator Rotate(GameObject item, Vector3 axis, float angle, float duration = 1.0f, string source = "")
    {
        // Source is for deciding which index to update
        Quaternion from = item.transform.rotation;
        Quaternion to = item.transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            item.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        item.transform.rotation = to;

        scrollUpButton.GetComponent<Button>().interactable = true;
        scrollDownButton.GetComponent<Button>().interactable = true;
        // Update indexes based on source
        if (source == "scrollMenu")
        {
            // Set active window for the scroll menu items
            SetActiveSection(Mathf.DeltaAngle(360, item.transform.localEulerAngles.x));
            // Hide some items from scroll menu and show some others based on which one is active
        }
    }

    void SetActiveSection(float rotationAngle)
    {
        // To base it with 360 and positive angles
        if (rotationAngle > 360)
        {
            rotationAngle -= 360;
        } else if (rotationAngle < 0)
        {
            rotationAngle += 360;
        }
        // Keep track of angle in every rotate action to know which section is active
        // Hide non active sections
        switch (rotationAngle)
        {
            case 198:
                activeAngle = 198;
                //activeSection = WardrobeSection.Face;
                SetFaceIndex();
                SetHorizontalButtons(WardrobeSection.Face);
                HideSomeSections(new GameObject[] { scrollMenuWrist, scrollMenuMouth });
                break;
            case 270:
                activeAngle = 270;
                //activeSection = WardrobeSection.Eyes;
                HideSomeSections(new GameObject[] { scrollMenuWrist, scrollMenuHead });
                break;
            case 342:
                activeAngle = 342;
                //activeSection = WardrobeSection.Mouth;
                HideSomeSections(new GameObject[] { scrollMenuHead, scrollMenuFace });
                break;
            case 54:
                activeAngle = 54;
                //activeSection = WardrobeSection.Wrist;
                HideSomeSections(new GameObject[] { scrollMenuFace, scrollMenuEyes });
                break;
            case 126:
                activeAngle = 126;
                //activeSection = WardrobeSection.Head;
                HideSomeSections(new GameObject[] { scrollMenuEyes, scrollMenuMouth });
                break;
        }
    }

    void HideSomeSections(GameObject[] sectionsToHide)
    {
        // Set all sections visible 
        scrollMenuFace.SetActive(true);
        scrollMenuHead.SetActive(true);
        scrollMenuWrist.SetActive(true);
        scrollMenuEyes.SetActive(true);
        scrollMenuMouth.SetActive(true);
        // Then hide some of them
        foreach (GameObject section in sectionsToHide)
        {
            section.SetActive(false);
        }
    }

    void SetFaceIndex()
    {
        // Find index of current face from list of faces player already owns
        for (int i = 0; i < player.unlockedFaces.Count; i++)
        {
            if (player.unlockedFaces[i] == player.face)
            {
                faceIndex = i;
            }
        }
    }

    void SetHorizontalButtons(WardrobeSection activeSection)
    {
        if (activeSection == WardrobeSection.Face)
        {
            if (faceIndex == 0)
            {
                scrollLeftButton.SetActive(false);
                scrollRightButton.SetActive(true);
            }
            else if (faceIndex == player.unlockedFaces.Count - 1)
            {
                scrollLeftButton.SetActive(true);
                scrollRightButton.SetActive(false);
            }
        } else if(activeSection == WardrobeSection.Head)
        {
            // Same thing for Head and the rest of items
        }
    }

    void InstantiateNewFace()
    {
        // Remove previous face
        Destroy(scrollMenuFace.transform.GetChild(0).gameObject);
        // Instantiate a new one
        GameObject currentFacePrefab = Instantiate(
            allEmojies.Find(emoji => emoji.name == player.face),
            scrollMenuFace.transform.position,
            scrollMenuFace.transform.rotation);
        // Set parent to scroll menu face to be able to delete again when scrolled
        currentFacePrefab.transform.SetParent(scrollMenuFace.transform);
        currentFacePrefab.transform.localScale = Vector3.one;
    }
}
