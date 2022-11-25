using System.Reflection;
using FluentAssertions;
using PoEGamblingHelper3.Shared;

namespace Frontend.Test.SharedTest;

public class UnitTest1
{
    [Fact]
    public void TestLastPoeNinjaUpdateJustNow()
    {
        #region Setup

        var filter = new Filter();

        var method = filter.GetType()
                           .GetMethod("LastPoeNinjaUpdateText", BindingFlags.NonPublic | BindingFlags.Instance);
        if (method is null) throw new Exception("Method not found.");

        #endregion

        var result = method.Invoke(filter, null);

        result.Should().NotBeNull().And.Be("just now");
    }

    [Fact]
    public void TestLastPoeNinjaUpdate5Minutes()
    {
        #region Setup

        var filter = new Filter { LastPoeNinjaUpdate = DateTime.Now.AddMinutes(-5) };

        // var prop = filter.GetType().GetField("LastPoeNinjaUpdate", BindingFlags.NonPublic | BindingFlags.Instance);
        // if (prop is null) throw new Exception("Property not found.");
        // prop.SetValue(filter, DateTime.Now.AddMinutes(-5));

        var method = filter.GetType()
                           .GetMethod("LastPoeNinjaUpdateText", BindingFlags.NonPublic | BindingFlags.Instance);
        if (method is null) throw new Exception("Method not found.");

        #endregion

        var result = method.Invoke(filter, null);

        result.Should().NotBeNull().And.Be("5 minutes ago");
    }
}