using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(int?), typeof(string))]
class AbilityValueConverter : NullableIntValueConverter
{
    public AbilityValueConverter()
        : base(0, 999)
    {
    }
}