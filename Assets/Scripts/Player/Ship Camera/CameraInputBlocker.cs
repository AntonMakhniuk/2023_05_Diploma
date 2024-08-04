namespace Player.Ship_Camera
{
    public static class CameraInputBlocker
    {
        public static void TurnOffCamera()
        {
            PlayerActions.InputActions.PlayerCamera.Disable();
        }
        
        public static void TurnOnCamera()
        {
            PlayerActions.InputActions.PlayerCamera.Enable();
        }
    }
}