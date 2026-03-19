using System.Linq;
using System.Reflection;

namespace CombatManager.Api.Unused
{
    public static class AttributeExtensions
    {
        public static T GetAttribute<T>(this MethodBase method)
        {
            var attributes = method.GetCustomAttributes(typeof(T), false);
            return (T)attributes.First();
        }
    }
}