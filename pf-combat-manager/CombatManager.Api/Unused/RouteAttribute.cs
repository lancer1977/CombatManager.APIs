using System;

namespace CombatManager.Api.Unused
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class RouteAttribute : Attribute
    {
        public HttpVerbs Verb { get; set; }
        public string Address { get; set; }
        public RouteAttribute(HttpVerbs verb, string address)
        {
            Verb = verb;
            Address = address;
        }
    }
}