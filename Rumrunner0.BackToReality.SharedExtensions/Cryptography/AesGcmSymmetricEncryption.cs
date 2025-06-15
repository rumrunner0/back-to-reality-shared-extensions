using System;
using System.Security.Cryptography;
using System.Text;

namespace Rumrunner0.BackToReality.SharedExtensions.Cryptography;

// AES-GCM symmetric encryption, v.1.1.0, from Jun 14, 2025.

/// <summary>
/// AES-GCM symmetric encryption.
/// </summary>
public static class AesGcmSymmetricEncryption
{
	/// <summary>
	/// Size of the secret key in bytes (256 bits).
	/// </summary>
	private const int _KEY_SIZE = 32;

	/// <summary>
	/// Size of the nonce in bytes (96 bits).
	/// </summary>
	private const int _NONCE_SIZE = 12;

	/// <summary>
	/// Size of the authentication tag in bytes (128 bit).
	/// </summary>
	private const int _TAG_SIZE = 16;

	/// <summary>
	/// Size of the header (<see cref="_NONCE_SIZE" /> + <see cref="_TAG_SIZE" />).
	/// </summary>
	private const int _HEADER_SIZE = _NONCE_SIZE + _TAG_SIZE;

	/// <summary>
	/// Generates an encryption key encoded using Base64.
	/// </summary>
	/// <returns>New encryption key.</returns>
	public static string GenerateKey()
	{
		var keyBytes = (Span<byte>)stackalloc byte[_KEY_SIZE];
		RandomNumberGenerator.Fill(keyBytes);
		return Convert.ToBase64String(keyBytes);
	}

	/// <summary>
	/// Encrypts <paramref name="data" /> in a blob (nonce|tag|cipher) encoded using Base64.
	/// </summary>
	/// <param name="data">The plaintext.</param>
	/// <param name="key">The key.</param>
	/// <returns>Encrypted data.</returns>
	public static string Encrypt(string data, string key)
	{
		// Validates parameters to be not null.
		ArgumentNullException.ThrowIfNull(data);
		ArgumentNullException.ThrowIfNull(key);

		// Decodes the key.
		if (!TryGetBytesFromBase64String(key, out var keyBytes))
		{
			throw new ArgumentException("The key is not valid Base64 string", nameof(key));
		}

		// Validates key to have valid length.
		if (keyBytes.Length != _KEY_SIZE)
		{
			throw new ArgumentException($"Length of the key is not valid. It must be {_KEY_SIZE} bytes long", nameof(key));
		}

		// Gets plaintext bytes.
		var plaintextBytes = Encoding.UTF8.GetBytes(data);

		// Allocates memory for the header and ciphertext.
		var nonce = (Span<byte>)stackalloc byte[_NONCE_SIZE];
		var tag = (Span<byte>)stackalloc byte[_TAG_SIZE];
		var ciphertext = (Span<byte>)new byte[plaintextBytes.Length];

		// Generates the nonce.
		RandomNumberGenerator.Fill(nonce);

		try
		{
			// Encrypts the plaintext and generates the authentication tag.
			using var aes = new AesGcm(keyBytes, tag.Length);
			aes.Encrypt(nonce, plaintextBytes, ciphertext, tag);
		}
		catch (CryptographicException e)
		{
			throw new ApplicationException("Cryptographic operation failed", e);
		}
		catch (Exception e)
		{
			throw new ApplicationException("Unexpected error has occurred", e);
		}

		// Populates a blob.
		var blob = (Span<byte>)new byte[_HEADER_SIZE + ciphertext.Length];
		nonce.CopyTo(blob[.._NONCE_SIZE]);
		tag.CopyTo(blob[_NONCE_SIZE.._HEADER_SIZE]);
		ciphertext.CopyTo(blob[_HEADER_SIZE..]);

		// Encodes the blob.
		return Convert.ToBase64String(blob);
	}

	/// <summary>
	/// Decrypts a <paramref name="data" /> that has been encrypted using <see cref="Encrypt" />.
	/// </summary>
	/// <param name="data">The ciphertext.</param>
	/// <param name="key">The key.</param>
	/// <returns>Decrypted data.</returns>
	public static string Decrypt(string data, string key)
	{
		// Validates parameters to be not null.
		ArgumentNullException.ThrowIfNull(data);
		ArgumentNullException.ThrowIfNull(key);

		// Gets encrypted data bytes.
		if (!TryGetBytesFromBase64String(data, out var blob))
		{
			throw new ArgumentException("The data is not valid Base64 string", nameof(data));
		}

		// Validates blob to have valid length.
		if (blob.Length < _HEADER_SIZE)
		{
			throw new ArgumentException("Ciphertext blob is not valid", nameof(data));
		}

		// Decodes the key.
		if (!TryGetBytesFromBase64String(key, out var keyBytes))
		{
			throw new ArgumentException("The key is not valid Base64 string", nameof(key));
		}

		// Validates key to have valid length.
		if (keyBytes.Length != _KEY_SIZE)
		{
			throw new ArgumentException($"Length of the key is not valid. It must be {_KEY_SIZE} bytes long", nameof(key));
		}

		// Cuts the blob according to the scheme.
		var nonce = blob[.._NONCE_SIZE];
		var tag = blob[_NONCE_SIZE.._HEADER_SIZE];
		var ciphertext = blob[_HEADER_SIZE..];

		// Allocates memory for the plaintext.
		var plaintextBytes = (Span<byte>)new byte[ciphertext.Length];

		try
		{
			// Decrypts the ciphertext and populates the plaintext bytes.
			using var aes = new AesGcm(keyBytes, tag.Length);
			aes.Decrypt(nonce, ciphertext, tag, plaintextBytes);
		}
		catch (AuthenticationTagMismatchException e)
		{
			throw new ApplicationException("Authentication tag mismatch. Data has been tampered with or the wrong key was supplied", e);
		}
		catch (CryptographicException e)
		{
			throw new ApplicationException("Cryptographic operation failed", e);
		}
		catch (Exception e)
		{
			throw new ApplicationException("Unexpected error has occurred", e);
		}

		// Decodes the plaintext bytes.
		return Encoding.UTF8.GetString(plaintextBytes);
	}

	/// <summary>
	/// Gets the length of a decoded string that is encoded using Base64.
	/// </summary>
	/// <param name="base64String">The Base64-encoded string.</param>
	/// <returns>The length of a decoded string.</returns>
	private static int GetBase64DecodedStringLength(string base64String)
	{
		var padding = base64String.EndsWith("==", StringComparison.Ordinal) ? 2 : base64String.EndsWith('=') ? 1 : 0;
		return base64String.Length * 3 / 4 - padding;
	}

	/// <summary>
	/// Tries to decode the <paramref name="base64String" />.
	/// </summary>
	/// <param name="base64String">The string encoded using Base64.</param>
	/// <param name="bytes">The span that bytes will be written to.</param>
	/// <returns><c>true</c>, if the conversion was successful; <c>falses</c>, otherwise.</returns>
	private static bool TryGetBytesFromBase64String(string base64String, out Span<byte> bytes)
	{
		// TODO: Try return ReadOnlySpan.

		bytes = (Span<byte>)new byte[GetBase64DecodedStringLength(base64String)];
		if (!Convert.TryFromBase64String(base64String, bytes, out var bytesWritten))
		{
			bytes = default;
			return false;
		}

		if (bytesWritten != bytes.Length)
		{
			bytes = default;
			return false;
		}

		return true;
	}
}