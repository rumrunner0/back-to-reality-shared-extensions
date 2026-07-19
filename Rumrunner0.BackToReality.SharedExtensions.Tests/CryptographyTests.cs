using System;
using System.Security.Cryptography;
using System.Text;
using Rumrunner0.BackToReality.SharedExtensions.Cryptography;
using Xunit;

namespace Rumrunner0.BackToReality.SharedExtensions.Tests;

public sealed class CryptographyTests
{
	[Fact]
	public void GenerateKey_Produces256BitBase64Key()
	{
		var key = AesGcmSymmetricEncryption.GenerateKey();
		Assert.Equal(32, Convert.FromBase64String(key).Length);
	}

	[Theory]
	[InlineData("")]
	[InlineData("hello")]
	[InlineData("длинный текст с юникодом 🚀")]
	public void Encrypt_Decrypt_RoundTripsPlaintext(string plaintext)
	{
		var key = AesGcmSymmetricEncryption.GenerateKey();
		var encrypted = AesGcmSymmetricEncryption.Encrypt(plaintext, key);
		Assert.Equal(plaintext, AesGcmSymmetricEncryption.Decrypt(encrypted, key));
	}

	[Fact]
	public void Encrypt_ProducesDifferentBlobsForTheSamePlaintext()
	{
		var key = AesGcmSymmetricEncryption.GenerateKey();
		Assert.NotEqual(AesGcmSymmetricEncryption.Encrypt("data", key), AesGcmSymmetricEncryption.Encrypt("data", key));
	}

	[Fact]
	public void Decrypt_ThrowsOnTamperedBlobOrWrongKey()
	{
		var key = AesGcmSymmetricEncryption.GenerateKey();
		var encrypted = AesGcmSymmetricEncryption.Encrypt("data", key);

		var blob = Convert.FromBase64String(encrypted);
		blob[^1] ^= 0xFF;
		var tampered = Convert.ToBase64String(blob);
		Assert.ThrowsAny<CryptographicException>(() => AesGcmSymmetricEncryption.Decrypt(tampered, key));

		var otherKey = AesGcmSymmetricEncryption.GenerateKey();
		Assert.ThrowsAny<CryptographicException>(() => AesGcmSymmetricEncryption.Decrypt(encrypted, otherKey));
	}

	[Fact]
	public void Encrypt_DoesNotLeakTheKeyIntoExceptions()
	{
		var shortKey = Convert.ToBase64String(new byte[16]);
		var exception = Assert.Throws<ArgumentException>(() => AesGcmSymmetricEncryption.Encrypt("data", shortKey));
		Assert.Equal("key", exception.ParamName);
		Assert.DoesNotContain(shortKey, exception.Message);

		var invalidKey = "not-base64!";
		exception = Assert.Throws<ArgumentException>(() => AesGcmSymmetricEncryption.Encrypt("data", invalidKey));
		Assert.Equal("key", exception.ParamName);
		Assert.DoesNotContain(invalidKey, exception.Message);
	}

	[Fact]
	public void Decrypt_ValidatesBlobAndKeyWithoutLeakingValues()
	{
		var key = AesGcmSymmetricEncryption.GenerateKey();

		var exception = Assert.Throws<ArgumentException>(() => AesGcmSymmetricEncryption.Decrypt("###", key));
		Assert.Equal("data", exception.ParamName);

		var tooShort = Convert.ToBase64String(new byte[10]);
		exception = Assert.Throws<ArgumentException>(() => AesGcmSymmetricEncryption.Decrypt(tooShort, key));
		Assert.Equal("data", exception.ParamName);

		var blob = AesGcmSymmetricEncryption.Encrypt("data", key);
		var shortKey = Convert.ToBase64String(new byte[16]);
		exception = Assert.Throws<ArgumentException>(() => AesGcmSymmetricEncryption.Decrypt(blob, shortKey));
		Assert.Equal("key", exception.ParamName);
		Assert.DoesNotContain(shortKey, exception.Message);
	}

	[Fact]
	public void Encrypt_ThrowsOnIllFormedStringsInsteadOfCorruptingThem()
	{
		var key = AesGcmSymmetricEncryption.GenerateKey();
		Assert.Throws<EncoderFallbackException>(() => AesGcmSymmetricEncryption.Encrypt("\uD800", key));
	}

	[Fact]
	public void FixedTimeEquals_ComparesOrdinally()
	{
		Assert.True(CryptographicOperationExtensions.FixedTimeEquals(null, null));
		Assert.False(CryptographicOperationExtensions.FixedTimeEquals(null, ""));
		Assert.False(CryptographicOperationExtensions.FixedTimeEquals("", null));
		Assert.True(CryptographicOperationExtensions.FixedTimeEquals("", ""));
		Assert.True(CryptographicOperationExtensions.FixedTimeEquals("secret", "secret"));
		Assert.False(CryptographicOperationExtensions.FixedTimeEquals("secret", "secreT"));
		Assert.False(CryptographicOperationExtensions.FixedTimeEquals("short", "longer-value"));
	}

	[Fact]
	public void FixedTimeEquals_DistinguishesDistinctUnpairedSurrogates()
	{
		Assert.False(CryptographicOperationExtensions.FixedTimeEquals("\uD800", "\uDC00"));
	}
}
