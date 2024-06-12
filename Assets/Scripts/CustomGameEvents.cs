namespace CIPA
{
    public static class CustomGameEvents
    {
        public static event System.Action OnPlayerArrivedAtMinigameLocation;
        public static event System.Action OnPlayerWorePPEs;
        public static event System.Action OnMinigameStarted;
        public static event System.Action OnMinigameEnded;


        public static void InvokeOnPlayerArrivedAtMinigameLocation()
        {
            OnPlayerArrivedAtMinigameLocation?.Invoke();
        }

        public static void InvokeOnMinigameStarted()
        {
            OnMinigameStarted?.Invoke();
        }

        public static void InvokeOnMinigameEnded()
        {
            OnMinigameEnded?.Invoke();
        }

        public static void InvokeOnPlayerWorePPEs()
        {
            OnPlayerWorePPEs?.Invoke();
        }
    }
}
