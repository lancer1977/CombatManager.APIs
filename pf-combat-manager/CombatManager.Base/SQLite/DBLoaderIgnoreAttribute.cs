using System;

namespace CombatManager
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class DBLoaderIgnoreAttribute : Attribute
    {
        public DBLoaderIgnoreAttribute() { }
    }
}