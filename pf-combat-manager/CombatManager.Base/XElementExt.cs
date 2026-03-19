using System.Xml.Linq;

namespace CombatManager
{
    public static class XElementExt
    {


        public static string ElementValue(this XElement x, string name)
        {
            return x.Element(name) == null ? null : x.Element(name).Value;
        }


        public static int ElementIntValue(this XElement it, string name)
        {
            int value = 0;
            XElement el = it.Element(name);
            if (el != null)
            {
                try
                {
                    value = int.Parse(el.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
            return value;
        }
    }
    
}
