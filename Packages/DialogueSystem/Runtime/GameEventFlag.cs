namespace DialogueSystem
{
    [System.Serializable]
    public class GameEventFlag
    {
        public string name;
        public bool active;
        public GameEventFlag(string name, bool active)
        {
            this.name = name;
            this.active = active;
        }
    }
}
