using System.Globalization;
using System.Windows;
using Datenlotsen.WPF.Converters;

namespace Datenlotsen.WPF.Tests;

public class ConverterTests
{
    [Fact]
    public void NullToBoolConverter_ReturnsFalseForNull()
    {
        var converter = new NullToBoolConverter();
        Assert.False((bool)converter.Convert(null, typeof(bool), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void NullToBoolConverter_ReturnsTrueForNonNull()
    {
        var converter = new NullToBoolConverter();
        Assert.True((bool)converter.Convert("x", typeof(bool), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void StringNullOrEmptyToVisibilityConverter_VisibleForNullOrEmpty()
    {
        var converter = new StringNullOrEmptyToVisibilityConverter();
        Assert.Equal(Visibility.Visible, converter.Convert(null, typeof(Visibility), null, CultureInfo.InvariantCulture));
        Assert.Equal(Visibility.Visible, converter.Convert("", typeof(Visibility), null, CultureInfo.InvariantCulture));
    }

    [Fact]
    public void StringNullOrEmptyToVisibilityConverter_CollapsedForNonEmpty()
    {
        var converter = new StringNullOrEmptyToVisibilityConverter();
        Assert.Equal(Visibility.Collapsed, converter.Convert("abc", typeof(Visibility), null, CultureInfo.InvariantCulture));
    }
}
