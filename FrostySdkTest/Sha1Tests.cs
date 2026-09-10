using FrostySdk;
using Xunit;

namespace FrostySdkTest
{
    public class Sha1Tests
    {
        [Theory]
        [InlineData("da39a3ee5e6b4b0d3255bfef95601890afd80709")]
        [InlineData("0000000000000000000000000000000000000000")]
        [InlineData("ffffffffffffffffffffffffffffffffffffffff")]
        [InlineData("0123456789abcdeffedcba98765432100f1e2d3c")]
        public void StringRoundTrip_PreservesValue(string hex)
        {
            Sha1 sha1 = new Sha1(hex);

            Assert.Equal(hex, sha1.ToString());
        }

        [Fact]
        public void ByteArrayRoundTrip_PreservesValue()
        {
            byte[] bytes = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x11, 0x12, 0x13, 0x14 };

            Sha1 sha1 = new Sha1(bytes);

            Assert.Equal(bytes, sha1.ToByteArray());
        }

        [Fact]
        public void SameBytes_AreEqual()
        {
            Sha1 a = new Sha1("0123456789abcdeffedcba98765432100f1e2d3c");
            Sha1 b = new Sha1("0123456789abcdeffedcba98765432100f1e2d3c");

            Assert.True(a == b);
            Assert.False(a != b);
            Assert.Equal(a, b);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void DifferentBytes_AreNotEqual()
        {
            Sha1 a = new Sha1("0123456789abcdeffedcba98765432100f1e2d3c");
            Sha1 b = new Sha1("da39a3ee5e6b4b0d3255bfef95601890afd80709");

            Assert.False(a == b);
            Assert.True(a != b);
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void Zero_EqualsAllZeroBytes()
        {
            Assert.Equal(Sha1.Zero, new Sha1("0000000000000000000000000000000000000000"));
        }
    }
}
