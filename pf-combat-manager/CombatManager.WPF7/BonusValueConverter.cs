using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(int?), typeof(string))]
class BonusValueConverter : NullableIntValueConverter
{
    public BonusValueConverter()
        : base(-999, 999)
    {
    }
}