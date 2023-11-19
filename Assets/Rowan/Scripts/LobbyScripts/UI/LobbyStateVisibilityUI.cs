using UnityEngine;

namespace LobbyRelaySample.UI
{
    /// <summary>
    /// Show or hide a UI element based on the current GameState (e.g. in a lobby).
    /// </summary>
    public class LobbyStateVisibilityUI : UIPanelBase
    {
        [SerializeField]
        LGameState ShowThisWhen;

        void GameStateChanged(LGameState state)
        {
            if (!ShowThisWhen.HasFlag(state))
                Hide();
            else
                Show();
        }

        public override void Start()
        {
            base.Start();
            //Manager.onGameStateChanged += GameStateChanged;
        }

        void OnDestroy()
        {
            if (Manager == null)
                return;
            //Manager.onGameStateChanged -= GameStateChanged;
        }
    }
}
