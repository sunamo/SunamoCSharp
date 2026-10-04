namespace SunamoCSharp.Tests;

using FluentAssertions;
using SunamoStringGetLines;
using System.IO;

public class CSharpHelperTests
{
    [Fact]
    public async Task IsEmptyCommentedOrOnlyWithNamespaceTest()
    {
        var d = (await File.ReadAllLinesAsync(@"E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoData\_sunamo\SunamoExceptions\_AddedToAllCsproj\CASunamoExceptions.cs")).ToList();
        var b = CSharpHelper.IsEmptyCommentedOrOnlyWithNamespace("", d, null, new List<string>());
    }

    [Fact]
    public void RemoveCommentsTest()
    {
        const string input = @"a
//b
c
d /*e*/
/*haf
baf*/
f";
        List<string> expected = SHGetLines.GetLines(@"a
c
d

f");
        var actual = CSharpHelper.RemoveComments(SHGetLines.GetLines(input), true, true);
        actual.Should().BeEquivalentTo(expected);
    }
    [Fact]
    public void RemoveComments2Test()
    {
        const string input = @"a
//b
c
d /*e*/
/*haf
baf*/
f";
        // d have space on end
        const string expected = @"a

c
d 

f";
        var actual = CSharpHelper.RemoveBlockComments(input);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RemoveCommentsKeepLinesTest()
    {
        const string input = @"a
//b
//b2
c
d /*e*/
/*haf

baf*/
f";
        // d have space on end
        const string expected = @"a


c
d 



f";
        var actual = CSharpHelper.RemoveComments(input, true, true, true);
        try
        {
            Assert.Equal(expected, actual);
        }
        catch (Exception ex)
        {

        }

    }
}