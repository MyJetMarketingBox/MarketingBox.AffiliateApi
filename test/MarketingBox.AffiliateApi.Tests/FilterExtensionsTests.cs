using MarketingBox.AffiliateApi.Extensions;
using NUnit.Framework;

namespace MarketingBox.AffiliateApi.Tests;

[TestFixture]
public class FilterExtensionsTests
{
    enum TestEnum
    {
        Test1 = 1,
        Test2
    }

    private static object[] _enumData =
    {
        new object[] {"Test1,Test2,Test3,Test2,Test1"},
        new object[] {"1,2,3,1,2"},
        new object[] {"b,Test1,Test2,,Test2,Test1,a"},
        new object[] {"b,1,2,a,1,2"}
    };

    private static object[] _longData =
    {
        new object[] {"1,2,1,2"},
        new object[] {"b,1,2,a,1,2"}
    };

    private static object[] _emptyEnumData =
    {
        new object[] {"b,a,s,d,f,a"},
        new object[] {"4,5,4,6,8,7,,"},
        new object[] {"Test1:Test2"},
        new object[] {string.Empty},
        new object[] {null}
    };

    private static object[] _emptyLongData =
    {
        new object[] {"b,a,s,d,f,a"},
        new object[] {"2:1:3:4"},
        new object[] {string.Empty},
        new object[] {null}
    };

    [TestCaseSource(nameof(_longData))]
    public void LongTest(string values)
    {
        var ids = values.Parse<long>();
        Assert.That(ids, Has.Exactly(2).Items);
        Assert.AreEqual(1, ids[0]);
        Assert.AreEqual(2, ids[1]);
    }

    [TestCaseSource(nameof(_enumData))]
    public void EnumTest(string values)
    {
        var ids = values.Parse<TestEnum>();
        Assert.That(ids, Has.Exactly(2).Items);
        Assert.AreEqual(TestEnum.Test1, ids[0]);
        Assert.AreEqual(TestEnum.Test2, ids[1]);
    }

    [TestCaseSource(nameof(_emptyEnumData))]
    public void NoParsedEnumDataTest(string values)
    {
        var idsEnum = values.Parse<TestEnum>();
        Assert.IsEmpty(idsEnum);
    }

    [TestCaseSource(nameof(_emptyLongData))]
    public void NoParsedLongDataTest(string values)
    {
        var idsEnum = values.Parse<long>();
        Assert.IsEmpty(idsEnum);
    }
}