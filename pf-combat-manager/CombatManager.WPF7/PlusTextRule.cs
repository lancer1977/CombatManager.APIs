using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace CombatManager.WPF7;

public class PlusTextRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        try
        {
            var text = (string)value;

            if (Regex.Match(text, "([\r\n])|(or)", RegexOptions.IgnoreCase).Success)
            {
                    
                return new ValidationResult(false, "Illegal characters");
            }


        }
        catch (Exception e)
        {
            return new ValidationResult(false, e.Message);
        }

            
        return new ValidationResult(true, null);
            
    }
}