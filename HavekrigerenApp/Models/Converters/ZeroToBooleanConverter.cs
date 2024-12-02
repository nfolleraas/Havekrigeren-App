using System.Globalization;

namespace HavekrigerenApp.Models.Converters
{
    public class ZeroToBooleanConverter : IValueConverter
    {
        // Check if value is an int and count is == 0
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is int count && count == 0;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
