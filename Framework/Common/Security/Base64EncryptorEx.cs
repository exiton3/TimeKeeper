namespace Framework.Common.Security
{
    using System;
    using System.Text;

    public class Base64EncryptorEx : IEncryptor
    {

        private readonly RandomStringGenerator randomStringGenerator;
        private const int Length = 3;
        public Base64EncryptorEx()
        {

            randomStringGenerator = new RandomStringGenerator();
        }
        public string Encode(string data)
        {
            try
            {
                byte[] encDataByte = Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encDataByte);
                return randomStringGenerator.Generate(Length) + encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }

        }

        public string Decode(string dataToEncode)
        {
            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecodeByte = Convert.FromBase64String(dataToEncode.Substring(Length));
                int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                char[] decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                string result = new String(decodedChar);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }
    }
}