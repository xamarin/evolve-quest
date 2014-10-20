
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace EvolveQuest.WinPhone.Converters
{
  public class BooleanToVisibilityConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return ((Visibility)value == Visibility.Visible);
    }
  }

  public class PrizeImageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return (bool)value ? "/Assets/ic_secret_prize.png" : "/Assets/ic_game_completed.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return value;
    }
  }

  public class BeaconImageConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return (bool)value ? "/Assets/ic_banana.png" : "/Assets/ic_no_banana.png";
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return value;
    }
  }

  public class BoolToValueConverter<T> : IValueConverter
  {
    public T FalseValue { get; set; }
    public T TrueValue { get; set; }

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value == null)
        return FalseValue;
      else
        return (bool)value ? TrueValue : FalseValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return value != null ? value.Equals(TrueValue) : false;
    }
  }
  public class BoolToBrushConverter : BoolToValueConverter<Brush> { }
}