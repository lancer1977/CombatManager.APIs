using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(int), typeof(string))]
class HPModConverter : IntValueConverter
{
    public HPModConverter()
        : base(0, 32000)
    {
    }
}