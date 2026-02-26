namespace CombatManager.LocalService
{
    public class LocalServiceMessage
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public object Data { get; set; }

        private static int _nextId = 1;

        public static LocalServiceMessage Create(string name, object data)
        {
            return new LocalServiceMessage() { Name = name, Data = data, Id = _nextId++};

        }
    }


}
