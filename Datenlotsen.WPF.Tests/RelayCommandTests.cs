using Datenlotsen.WPF.Commands;

namespace Datenlotsen.WPF.Tests;

public class RelayCommandTests
{
    [Fact]
    public void CanExecute_ReturnsTrue_WhenNoPredicate()
    {
        var cmd = new RelayCommand(_ => { });
        Assert.True(cmd.CanExecute(null));
    }

    [Fact]
    public void CanExecute_UsesPredicate()
    {
        var cmd = new RelayCommand(_ => { }, _ => false);
        Assert.False(cmd.CanExecute(null));
    }

    [Fact]
    public void Execute_CallsAction()
    {
        bool called = false;
        var cmd = new RelayCommand(_ => called = true);
        cmd.Execute(null);
        Assert.True(called);
    }
}
