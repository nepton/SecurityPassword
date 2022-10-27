using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SecurityPassword.OneTimePassword
{
    /// <summary>
    /// Provide security code generation procedures, security code generation, within the specified time valid
    /// </summary>
    public class HotpProvider : IHotpProvider
    {
        /// <summary>
        /// Length of the verification code
        /// </summary>
        private readonly int _codeLength;

        /// <summary>
        /// token secretKey
        /// </summary>
        private readonly byte[] _securityTokenBytes;

        /// <summary>
        /// Type of hash algorithm
        /// </summary>
        private readonly HashAlgorithmType _hashType;

        public HotpProvider(HotpOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(options.SecurityToken))
                throw new ArgumentException("Missing TOTP security token");

            if (options.CodeLength is < 2 or > 8)
                throw new ArgumentOutOfRangeException(nameof(options.CodeLength), "The length of the code must be between 2 and 8");

            _securityTokenBytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(options.SecurityToken));
            _codeLength         = options.CodeLength;
            _hashType           = options.HashType;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsConfigure"></param>
        public HotpProvider(Action<HotpOptions> optionsConfigure) : this(ApplyOptions(optionsConfigure))
        {
        }

        private static HotpOptions ApplyOptions(Action<HotpOptions> optionsConfigure)
        {
            var options = new HotpOptions();
            optionsConfigure(options);
            return options;
        }

        /// <summary>
        /// Generate a one time password for the given time and modifier
        /// </summary>
        /// <param name="modifier">Can be null or mobile number, Email etc</param>
        /// <param name="counter">The counter for the one time password</param>
        /// <returns></returns>
        public string Generate(string? modifier, long counter)
        {
            var code = GenerateCore(_securityTokenBytes, modifier, counter);
            return code;
        }

        /// <summary>
        /// Verify a one time password for the given time and modifier
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="code"></param>
        /// <param name="counter">The counter for the one time password</param>
        /// <returns></returns>
        public bool Verify(string? modifier, string code, long counter)
        {
            var generatedCode = GenerateCore(_securityTokenBytes, modifier, counter);
            return generatedCode == code;
        }

        private string GenerateCore(byte[] securityToken, string? modifier, long counter)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException(nameof(securityToken));
            }

            using HashAlgorithm hashAlgorithm = _hashType switch
            {
                HashAlgorithmType.SHA1   => new HMACSHA1(securityToken),
                HashAlgorithmType.SHA256 => new HMACSHA256(securityToken),
                HashAlgorithmType.SHA512 => new HMACSHA512(securityToken),
                _                        => throw new ArgumentOutOfRangeException()
            };

            var code = ComputeHotp(hashAlgorithm, counter, modifier);
            return code.ToString($"D{_codeLength}");
        }

        private int ComputeHotp(HashAlgorithm hashAlgorithm, long counter, string? modifier)
        {
            // # of 0's = length of pin
            int mod = (int) Math.Pow(10, _codeLength);

            // We can add an optional modifier
            var timeStepAsBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(counter));
            var buffer          = ApplyModifier(timeStepAsBytes, modifier);
            var hash            = hashAlgorithm.ComputeHash(buffer);

            // we got 20 bytes back, but we need 4 bytes

            // Generate DT string
            var offset = hash.Last() & 0xf;
            Debug.Assert(offset + 4 < hash.Length);
            var binaryCode = (hash[offset] & 0x7f) << 24
                             | (hash[offset + 1] & 0xff) << 16
                             | (hash[offset + 2] & 0xff) << 8
                             | (hash[offset + 3] & 0xff);

            return binaryCode % mod;
        }

        private byte[] ApplyModifier(byte[] input, string? modifier)
        {
            if (string.IsNullOrEmpty(modifier))
            {
                return input;
            }

            var modifierBytes = Encoding.UTF8.GetBytes(modifier);
            var combined      = new byte[checked(input.Length + modifierBytes.Length)];
            Buffer.BlockCopy(input,         0, combined, 0,            input.Length);
            Buffer.BlockCopy(modifierBytes, 0, combined, input.Length, modifierBytes.Length);
            return combined;
        }
    };
}
