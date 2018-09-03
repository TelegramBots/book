# Import RSA Key

In order to decrypt the credentials you need to provide the private RSA key to [DecryptCredentials] method.
If you might have the RSA key in [PEM format], you cannot simply instantiate a [RSA .NET object] from it.
Here we discuss two ways of importing your PEM private key.

## From PEM Format

This is the easier option and recommended **for development time only**.
We can generate a [RSA .NET object] from PEM key using [BouncyCastle package].

```bash
dotnet add package BouncyCastle
```

![bouncy castle c# logo](../docs/photo-bouncy_castle.gif)

Code snippet here shows the conversion from PEM file to needed RSA object.

```c#
// using System.IO;
// using System.Security.Cryptography;
// using Org.BouncyCastle.Crypto;
// using Org.BouncyCastle.Crypto.Parameters;
// using Org.BouncyCastle.OpenSsl;
// using Org.BouncyCastle.Security;

static RSA GetPrivateKey() {
  string privateKeyPem = File.ReadAllText("/path/to/private-key.pem");
  PemReader pemReader = new PemReader(new StringReader(privateKeyPem));
  AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair) pemReader.ReadObject();
  RSAParameters rsaParameters = DotNetUtilities
    .ToRSAParameters(keyPair.Private as RsaPrivateCrtKeyParameters);
  RSA rsa = RSA.Create(rsaParameters);
  return rsa;
}
```

> **Warning**: You don't necessarily need to have a dependency on [BouncyCastle package] in your bot project.
> Next section below offers a better alternative.

## From RSA Parameters

We recommend to JSON-serialize [RSA Parameters] of your key and create RSA object using its values without any
dependency on [BouncyCastle package] in production deployment.

```c#
RSAParameters rsaParameters = JsonConvert.DeserializeObject<RSAParameters>(json);
RSA key = RSA.Create(rsaParameters);
```

You still need to **use BouncyCastle only once** to read RSA key in PEM format and serialize its parameters:

```c#
// using System.IO;
// using System.Security.Cryptography;
// using Newtonsoft.Json;
// using Org.BouncyCastle.Crypto;
// using Org.BouncyCastle.Crypto.Parameters;
// using Org.BouncyCastle.OpenSsl;
// using Org.BouncyCastle.Security;

// ONLY ONCE: read RSA private key and serialize its parameters to JSON
static void Main() {
  string privateKeyPem = File.ReadAllText("/path/to/private-key.pem");
  PemReader pemReader = new PemReader(new StringReader(privateKeyPem));
  AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair) pemReader.ReadObject();
  RSAParameters rsaParameters = DotNetUtilities
    .ToRSAParameters(keyPair.Private as RsaPrivateCrtKeyParameters);
  
  string json = JsonConvert.SerializeObject(rsaParameters);
  File.WriteAllText("/path/to/private-key-params.json", json);
}
```

and pass JSON-serialized parameters to the app.

```json
// private-key-params.json
{
  "D": "nrXEeOl2Ky...JIQ==",
  "DP": "KZYZWbsy.../lk60=",
  "DQ": "Y25KgzPj...AdBd0=",
  "Exponent": "AQAB",
  "InverseQ": "0153...N6Y=",
  "Modulus": "0VElW...Fw==",
  "P": "56Mdiw...i7FSwDaM=",
  "Q": "51UN2sd...J44NTf0="
}
```

<!-- ----------- -->

[DecryptCredentials]: https://github.com/TelegramBots/Telegram.Bot.Extensions.Passport/blob/master/src/Telegram.Bot.Extensions.Passport/Decryption/IDecrypter.cs
[PEM format]: https://en.wikipedia.org/wiki/Privacy-Enhanced_Mail
[RSA .NET object]: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsa?redirectedfrom=MSDN&view=netstandard-2.0
[BouncyCastle package]: https://www.nuget.org/packages/BouncyCastle/
[RSA Parameters]: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsaparameters?view=netstandard-2.0