using EmbedIO.WebSockets;

namespace CombatManager.LocalService
{
    public class User
    {
        private IWebSocketContext _context;

        public User(IWebSocketContext context)
        {
            _context = context;
        }

        public string Name { get; set; }
        public string Id => _context.Id;
    }
}