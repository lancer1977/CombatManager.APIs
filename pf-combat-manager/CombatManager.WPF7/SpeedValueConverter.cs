using System.Windows.Data;

namespace CombatManager.WPF7;

[ValueConversion(typeof(int?), typeof(string))]
class SpeedValueConverter : NullableIntValueConverter
{
    public SpeedValueConverter()
        : base(0, 30000)
    {
    }
}