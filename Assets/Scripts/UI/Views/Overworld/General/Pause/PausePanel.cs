using Player;
using UI.Systems.Panels;

namespace UI.Views.Overworld.General.Pause
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