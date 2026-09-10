using Frosty.Hash;
using Xunit;

namespace FrostySdkTest
{
    public class HashTests
    {
        [Fact]
        public void Fnv1_EmptyString_ReturnsOffsetBasis()
        {
            Assert.Equal(5381, Fnv1.HashString(""));
        }

        [Fact]
        public void Fnv1a_EmptyString_ReturnsOffsetBasis()
        {
            Assert.Equal(5381, Fnv1a.HashString(""));
        }

        [Theory]
        [InlineData("chunks", 1548502573)]
        [InlineData("a", 177604)]
        [InlineData("TestBundle", -37695513)]
        public void Fnv1_KnownValues(string input, int expected)
        {
            Assert.Equal(expected, Fnv1.HashString(input));
        }

        [Theory]
        [InlineData("chunks", -1751282899)]
        [InlineData("a", 180708)]
        [InlineData("TestBundle", 1379159207)]
        public void Fnv1a_KnownValues(string input, int expected)
        {
            Assert.Equal(expected, Fnv1a.HashString(input));
        }

        [Fact]
        public void HashString_IsCaseSensitive()
        {
            Assert.NotEqual(Fnv1.HashString("TestBundle"), Fnv1.HashString("testbundle"));
            Assert.NotEqual(Fnv1a.HashString("TestBundle"), Fnv1a.HashString("testbundle"));
        }
    }
}
