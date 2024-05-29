using Player;
using UI.Systems;
using UI.Systems.Panels;

namespace UI.Panels.Pause
{
    public class PausePanel : UIPanel
    {
        public override void Open()
        {
            base.Open();
            
            PlayerActions.InputActions.Disable();
            PlayerActions.InputActions.UI.CloseWindowOpenPause.Enable();
        }

        public override void Close()
        {
            base.Close();
            
            PlayerActions.InputActions.Enable();
        }
    }
}