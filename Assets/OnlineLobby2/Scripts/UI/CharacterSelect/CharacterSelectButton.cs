using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectButton : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private GameObject disabledOverlay;
    [SerializeField] public Button button;

    private CharacterSelectDisplay characterSelect;

    public Character Character { get; private set; }
    public bool IsDisabled { get; private set; }

    private bool wasLockedIn = false;
    public bool currentlySelected = false;

    public void SetCharacter(CharacterSelectDisplay characterSelect, Character character)
    {
        Debug.Log("Set character button to: " + character.name);

        iconImage.sprite = character.Icon;

        this.characterSelect = characterSelect;
        
        Character = character;
    }

    public void SelectCharacter()
    {
        Debug.Log("selecting character " + Character);
        button.interactable = false;
        currentlySelected = true;
        foreach (CharacterSelectButton otherButton in characterSelect.characterButtons) {
            if (otherButton != this && !otherButton.IsDisabled) {
                otherButton.button.interactable = true;
                otherButton.currentlySelected = false;
            }
        }
        /*
        for (int i = 0; i < 3; i++) {
            if (characterSelect.characterButtons[i].GetComponentInChildren<Button>() == button) {
                Debug.Log("is button");
                button.interactable = false;
                //IsDisabled = true;
            } else {
                Debug.Log("other button");
                button.interactable = true;
                //IsDisabled = false;
            }
        }*/
    /*
        foreach (CharacterSelectButton thatButton in characterSelect.characterButtons) {
            if (thatButton == this) {
                Debug.Log("is button");
                //thatButton.SetActive(false);
                button.interactable = false;
                //IsDisabled = true;
            } else {
                Debug.Log("other button");
                //thatButton.SetActive(true);
                button.interactable = true;
                //IsDisabled = false;
            }
        }*/

        characterSelect.Select(Character);
        
    }

    public void SetDisabled()
    {
        IsDisabled = true;
        disabledOverlay.SetActive(true);
        button.interactable = false;
    }

    public void SetEnabled()
    {   
        disabledOverlay.SetActive(false);
        IsDisabled = false;
        if (!currentlySelected) {
            button.interactable = true;
        }
 
    }

    void Update() {

        var i = -1;
        foreach (CharacterSelectState player in characterSelect.players) {
            i++;
            if (player.ClientId != LobbySceneManagement.singleton.getLocalPlayerNetworkID()) { continue; }
                if (player.IsLockedIn) {
                    button.interactable = false;
                }
                if (wasLockedIn && !player.IsLockedIn && !IsDisabled) {
                    button.interactable = !currentlySelected;
                }
                wasLockedIn = player.IsLockedIn;
        }

        

    }

}

