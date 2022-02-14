using ShowNet.Services;
using Xunit;

namespace ShowNet.Tests.Services;

public class InterfacesFinderTests
{
    [Fact]
    public void UpdateInterfaces_CreatesNewInterfaces()
    {
        var interfaceFinder = new InterfacesFinder();

        var oldInterfaces = interfaceFinder.GetInterfaces();
        interfaceFinder.UpdateInterfaces();
        var newInterfaces = interfaceFinder.GetInterfaces();

        if (oldInterfaces.Count != 0 || newInterfaces.Count != 0)
            Assert.NotEqual(oldInterfaces, newInterfaces);
        else
            Assert.Equal(oldInterfaces, newInterfaces);
    }
}