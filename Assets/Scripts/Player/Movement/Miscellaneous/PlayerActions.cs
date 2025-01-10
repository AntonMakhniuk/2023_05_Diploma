namespace Player.Movement.Miscellaneous
{
    public static class PlayerActions
    {
        private static PlayerInputActions _inputActions;
        public static PlayerInputActions InputActions
        {
            get
            {
                if (_inputActions != null)
                {
                    return _inputActions;
                }

                _inputActions = new PlayerInputActions();
                
                _inputActions.Enable();

                return _inputActions;
            }
        }
    }
}