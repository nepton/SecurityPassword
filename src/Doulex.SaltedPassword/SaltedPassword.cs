/******************************************************************** FR 1.20E *******
* Filename:    Doulex.SaltedPassword.cs
* Author:      Nepton
* Create Date: 2004-06-10
* Description:    Password Class is used to encrypt and verify the password. Once the password is encrypted successfully, it cannot be regenerated.
*     
*************************************************************************************/

using System.Security.Cryptography;
using System.Text;

namespace Doulex.PasswordUtility
{
    /******************************************************************** CR 1.21E *******
    *
    * Name:   Doulex.SaltedPassword        
    * Description:
    *     Password Class is used to encrypt and verify the password. The encryption algorithm is asymmetric, and the password cannot be regenerated once it is successfully encrypted
    *
    * History:
    *     2004-06-10    Nepton    Create
    *     2007-08-08    Nepton    Change to a static class
    *     2010-07-16    Nepton    modifying the validation module
    *     2011-06-09    Nepton    Change the password authentication policy. If FinalPassword is empty, enter the empty password. The authentication succeeds
    *     2020-07-28    Nepton    Refactor the code to remove the intermediate password
    *
    *************************************************************************************/
    /// <summary>
    /// Password class is used to encrypt and verify passwords
    /// Call CreatePassword to create an encrypted password that the user will keep in the database for authentication purposes
    /// The VerifyPassword function is called to verify the validity of the password
    /// 
    /// Plaintext password: can be read directly, easy to reveal personal privacy
    /// salted password: Created by the intermediate password, irreversible, can not be read, the same plaintext password each time to create the final password is different, random
    /// </summary>
    public class SaltedPassword : ISaltedPassword
    {
        private readonly SaltedPasswordOptions _options;

        /// <summary>
        /// the length of salt
        /// </summary>
        private readonly int _saltLength = 4;

        public SaltedPassword()
        {
            _options = new()
            {
                PlainPasswordComparable = true
            };
        }

        public SaltedPassword(SaltedPasswordOptions options)
        {
            _options = options;
        }

        /// <summary>
        /// Create strongly encrypted random numbers
        /// </summary>
        /// <returns>An encrypted random number of the specified length</returns>
        private string CreateSalt()
        {
            var       bSalt = new byte[_saltLength];
            using var rng   = RandomNumberGenerator.Create();
            rng.GetBytes(bSalt);
            return BytesToString(bSalt);
        }

        /// <summary>
        /// Converts a byte type to a string
        /// </summary>
        /// <param name="buf"></param>
        /// <returns></returns>
        private string BytesToString(byte[] buf)
        {
            return string.Join("", buf.Select(c => $"{c:X2}"));
        }

        /// <summary>
        /// Creates a final password based on the plaintext entered by the user
        /// </summary>
        /// <param name="inputPassword">Prepare the plaintext password to create the final password</param>
        /// <returns>the program returns the final password</returns>
        public string CreatePassword(string inputPassword)
        {
            if (inputPassword == null) throw new ArgumentNullException(nameof(inputPassword));

            return CreatePasswordCore(inputPassword, CreateSalt());
        }

        /// <summary>
        /// create the final password
        /// </summary>
        /// <param name="inputPassword">The intermediate password to be converted to the final password</param>
        /// <param name="strSalt">random value</param>
        /// <returns>the program returns the final password</returns>
        private string CreatePasswordCore(string inputPassword, string strSalt)
        {
            // 中间密码
            using var sha                = SHA1.Create();
            var       transitionPassword = BytesToString(sha.ComputeHash(Encoding.UTF8.GetBytes(inputPassword)));

            // 最终密码
            var bPassword = sha.ComputeHash(Encoding.UTF8.GetBytes(strSalt + transitionPassword));
            return strSalt + BytesToString(bPassword);
        }

        /// <summary>
        /// check the validity of the plaintext password
        /// </summary>
        /// <param name="saltedPassword">eventually the password</param>
        /// <param name="inputPassword">specifies the plaintext password to be checked</param>
        /// <returns>Return True if the two passed passwords match, false otherwise</returns>
        public bool VerifyPassword(string inputPassword, string saltedPassword)
        {
            if (inputPassword == null) throw new ArgumentNullException(nameof(inputPassword));
            if (saltedPassword == null) throw new ArgumentNullException(nameof(saltedPassword));

            // If no password is set, the user does not have a data password
            if (_options.PlainPasswordComparable && inputPassword == saltedPassword)
                return true;

            // If the password is not created, an error message is displayed
            if (!IsSaltedPassword(saltedPassword))
                return false;

            // Create an encrypted password using the entered password, and then compare the results
            var inputPasswordToCheck = CreatePasswordCore(inputPassword, saltedPassword.Substring(0, _saltLength * 2));
            return saltedPassword == inputPasswordToCheck;
        }

        /// <summary>
        /// Check that the salted password is in the correct format
        /// </summary>
        /// <param name="saltedPassword"></param>
        /// <returns></returns>
        public bool IsSaltedPassword(string? saltedPassword)
        {
            return saltedPassword != null && saltedPassword.Length == 40 + _saltLength * 2;
        }
    };
}
