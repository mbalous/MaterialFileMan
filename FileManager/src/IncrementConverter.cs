using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace FileManager
{
    class IncrementConverter : IValueConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object Convert(object value, Type targetType, dynamic parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return d + parameter;
            }
            else if (value is int i)
            {
                return i + parameter;
            }
            throw new ArgumentException(nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
