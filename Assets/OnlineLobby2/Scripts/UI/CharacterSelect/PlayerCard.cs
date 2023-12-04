using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private CharacterDatabase characterDatabase;
    [SerializeField] private GameObject visuals;
    [SerializeField] private Image characterIconImage;
    [SerializeField] private TMP_Text playerNameText;
    //[SerializeField] private TMP_Text characterNameText;

    public void UpdateDisplay(CharacterSelectState state)
    {
        if (state.CharacterId != -1)
        {
            var character = characterDatabase.GetCharacterById(state.CharacterId);
            characterIconImage.sprite = character.Icon;
            characterIconImage.enabled = true;
            //characterNameText.text = character.DisplayName;
        }
        else
        {
            characterIconImage.enabled = false;
        }

        //playerNameText.text = state.IsLockedIn ? $"Player {state.ClientId}" : $"Player {state.ClientId} (Picking...)";
        if (playerNameText.text == "NewPlayerName") {
            Debug.Log("Initial rename");
            playerNameText.text = $"Player {state.ClientId + 1}";
        }

        visuals.SetActive(true);
    }

    public void DisableDisplay()
    {
        visuals.SetActive(false);
    }

    void Awake() {
        //Register with scene manager
        var name = gameObject.name;
        int id = int.Parse(name.Substring(name.Length - 1));
        Debug.Log("PlayerCard ID: " + id);
        LobbySceneManagement.singleton.playerCards[id - 1] = this;
        LobbySceneManagement.singleton.playerNames[id - 1] = playerNameText;

    }
}
