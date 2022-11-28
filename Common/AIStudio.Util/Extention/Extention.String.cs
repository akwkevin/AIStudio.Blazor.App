using AIStudio.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;

namespace AIStudio.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extention
    {
        /// <summary>
        /// 转为字节数组
        /// </summary>
        /// <param name="base64Str">base64字符串</param>
        /// <returns></returns>
        public static byte[] ToBytes_FromBase64Str(this string base64Str)
        {
            return Convert.FromBase64String(base64Str);
        }

        /// <summary>
        /// 转换为MD5加密后的字符串（默认加密为32位）
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ToMD5String(this string str)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(str);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            md5.Dispose();

            return sb.ToString();
        }

        /// <summary>
        /// 验证指定长度的MD5
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="length">MD5长度（默认32）</param>
        /// <returns>
        ///   <c>true</c> if the specified length is MD5; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMd5(this string str, int length = 32)
        {
            if (str.Length < length || str.Length > length)
                return false;

            int count = 0;
            var charArray = "0123456789abcdefABCDEF".ToCharArray();

            foreach (var c in str.ToCharArray())
            {
                if (charArray.Any(x => x == c))
                    ++count;
            }
            return count == length;
        }

        /// <summary>
        /// 转换为MD5加密后的字符串（16位）
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ToMD5String16(this string str)
        {
            return str.ToMD5String().Substring(8, 16);
        }

        /// <summary>
        /// Base64加密
        /// 注:默认采用UTF8编码
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>
        /// 加密后的字符串
        /// </returns>
        public static string Base64Encode(this string source)
        {
            return Base64Encode(source, Encoding.UTF8);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <param name="encoding">加密采用的编码方式</param>
        /// <returns></returns>
        public static string Base64Encode(this string source, Encoding encoding)
        {
            string encode = string.Empty;
            byte[] bytes = encoding.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密
        /// 注:默认使用UTF8编码
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>
        /// 解密后的字符串
        /// </returns>
        public static string Base64Decode(this string result)
        {
            return Base64Decode(result, Encoding.UTF8);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <param name="encoding">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <returns>
        /// 解密后的字符串
        /// </returns>
        public static string Base64Decode(this string result, Encoding encoding)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encoding.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64Url编码
        /// </summary>
        /// <param name="text">待编码的文本字符串</param>
        /// <returns>
        /// 编码的文本字符串
        /// </returns>
        public static string Base64UrlEncode(this string text)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(text);
            var base64 = Convert.ToBase64String(plainTextBytes).Replace('+', '-').Replace('/', '_').TrimEnd('=');

            return base64;
        }

        /// <summary>
        /// Base64Url解码
        /// </summary>
        /// <param name="base64UrlStr">使用Base64Url编码后的字符串</param>
        /// <returns>
        /// 解码后的内容
        /// </returns>
        public static string Base64UrlDecode(this string base64UrlStr)
        {
            base64UrlStr = base64UrlStr.Replace('-', '+').Replace('_', '/');
            switch (base64UrlStr.Length % 4)
            {
                case 2:
                    base64UrlStr += "==";
                    break;
                case 3:
                    base64UrlStr += "=";
                    break;
            }
            var bytes = Convert.FromBase64String(base64UrlStr);

            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 计算SHA1摘要
        /// 注：默认使用UTF8编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(this string str)
        {
            return str.ToSHA1Bytes(Encoding.UTF8);
        }

        /// <summary>
        /// 计算SHA1摘要
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(this string str, Encoding encoding)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] inputBytes = encoding.GetBytes(str);
            byte[] outputBytes = sha1.ComputeHash(inputBytes);

            return outputBytes;
        }

        /// <summary>
        /// 转为SHA1哈希加密字符串
        /// 注：默认使用UTF8编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToSHA1String(this string str)
        {
            return str.ToSHA1String(Encoding.UTF8);
        }

        /// <summary>
        /// 转为SHA1哈希
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ToSHA1String(this string str, Encoding encoding)
        {
            byte[] sha1Bytes = str.ToSHA1Bytes(encoding);
            string resStr = BitConverter.ToString(sha1Bytes);
            return resStr.Replace("-", "").ToLower();
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToSHA256String(this string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] hash = SHA256.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }

            return builder.ToString();
        }

        /// <summary>
        /// HMACSHA256算法
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="secret">密钥</param>
        /// <returns></returns>
        public static string ToHMACSHA256String(this string text, string secret)
        {
            secret = secret ?? "";
            byte[] keyByte = Encoding.UTF8.GetBytes(secret);
            byte[] messageBytes = Encoding.UTF8.GetBytes(text);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage).Replace('+', '-').Replace('/', '_').TrimEnd('=');
            }
        }

        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            str = str.Replace("\0", "");
            if (string.IsNullOrEmpty(str))
                return 0;
            return Convert.ToInt32(str);
        }

        /// <summary>
        /// string转long
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static long ToLong(this string str)
        {
            str = str.Replace("\0", "");
            if (string.IsNullOrEmpty(str))
                return 0;

            return Convert.ToInt64(str);
        }

        /// <summary>
        /// 二进制字符串转为Int
        /// </summary>
        /// <param name="str">二进制字符串</param>
        /// <returns></returns>
        public static int ToInt_FromBinString(this string str)
        {
            return Convert.ToInt32(str, 2);
        }

        /// <summary>
        /// 将16进制字符串转为Int
        /// </summary>
        /// <param name="str">数值</param>
        /// <returns></returns>
        public static int ToInt0X(this string str)
        {
            int num = Int32.Parse(str, NumberStyles.HexNumber);
            return num;
        }

        /// <summary>
        /// 转换为double
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static double ToDouble(this string str)
        {
            return Convert.ToDouble(str);
        }

        /// <summary>
        /// string转byte[]
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str)
        {
            return Encoding.Default.GetBytes(str);
        }

        /// <summary>
        /// string转byte[]
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="theEncoding">需要的编码</param>
        /// <returns></returns>
        public static byte[] ToBytes(this string str, Encoding theEncoding)
        {
            return theEncoding.GetBytes(str);
        }

        /// <summary>
        /// 将16进制字符串转为Byte数组
        /// </summary>
        /// <param name="str">16进制字符串(2个16进制字符表示一个Byte)</param>
        /// <returns></returns>
        public static byte[] To0XBytes(this string str)
        {
            List<byte> resBytes = new List<byte>();
            for (int i = 0; i < str.Length; i = i + 2)
            {
                string numStr = $@"{str[i]}{str[i + 1]}";
                resBytes.Add((byte)numStr.ToInt0X());
            }

            return resBytes.ToArray();
        }

        /// <summary>
        /// 将ASCII码形式的字符串转为对应字节数组
        /// 注：一个字节一个ASCII码字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static byte[] ToASCIIBytes(this string str)
        {
            return str.ToList().Select(x => (byte)x).ToArray();
        }

        /// <summary>
        /// 转换为日期格式
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string str)
        {
            return Convert.ToDateTime(str);
        }

        /// <summary>
        /// 删除Json字符串中键中的@符号
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <returns></returns>
        public static string RemoveAt(this string jsonStr)
        {
            Regex reg = new Regex("\"@([^ \"]*)\"\\s*:\\s*\"(([^ \"]+\\s*)*)\"");
            string strPatten = "\"$1\":\"$2\"";
            return reg.Replace(jsonStr, strPatten);
        }

        /// <summary>
        /// json数据转实体类,仅仅应用于单个实体类，速度非常快
        /// </summary>
        /// <typeparam name="T">泛型参数</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T ToEntity<T>(this string json)
        {
            if (json == null || json == "")
                return default(T);

            Type type = typeof(T);
            object obj = Activator.CreateInstance(type, null);

            foreach (var item in type.GetProperties())
            {
                PropertyInfo info = obj.GetType().GetProperty(item.Name);
                string pattern;
                pattern = "\"" + item.Name + "\":\"(.*?)\"";
                foreach (Match match in Regex.Matches(json, pattern))
                {
                    switch (item.PropertyType.ToString())
                    {
                        case "System.String": info.SetValue(obj, match.Groups[1].ToString(), null); break;
                        case "System.Int32": info.SetValue(obj, match.Groups[1].ToString().ToInt(), null); ; break;
                        case "System.Int64": info.SetValue(obj, Convert.ToInt64(match.Groups[1].ToString()), null); ; break;
                        case "System.DateTime": info.SetValue(obj, Convert.ToDateTime(match.Groups[1].ToString()), null); ; break;
                    }
                }
            }
            return (T)obj;
        }

        /// <summary>
        /// 转为首字母大写
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToFirstUpperStr(this string str)
        {
            return str.Substring(0, 1).ToUpper() + str.Substring(1);
        }

        /// <summary>
        /// 转为首字母小写
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static string ToFirstLowerStr(this string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1);
        }

        /// <summary>
        /// 转为网络终结点IPEndPoint
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        /// =
        public static IPEndPoint ToIPEndPoint(this string str)
        {
            IPEndPoint iPEndPoint = null;
            try
            {
                string[] strArray = str.Split(':').ToArray();
                string addr = strArray[0];
                int port = Convert.ToInt32(strArray[1]);
                iPEndPoint = new IPEndPoint(IPAddress.Parse(addr), port);
            }
            catch
            {
                iPEndPoint = null;
            }

            return iPEndPoint;
        }

        /// <summary>
        /// 是否为弱密码
        /// 注:密码必须包含数字、小写字母、大写字母和其他符号中的两种并且长度大于8
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns>
        ///   <c>true</c> if [is weak password] [the specified password]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.Exception">pwd不能为空</exception>
        public static bool IsWeakPwd(this string pwd)
        {
            if (pwd.IsNullOrEmpty())
                throw new Exception("pwd不能为空");

            string pattern = "(^[0-9]+$)|(^[a-z]+$)|(^[A-Z]+$)|(^.{0,8}$)";
            if (Regex.IsMatch(pwd, pattern))
                return true;
            else
                return false;
        }

        //
        // 摘要:
        //     Adds a char to end of given string if it does not ends with the char.
        /// <summary>
        /// Ensures the ends with.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        public static string EnsureEndsWith(this string str, char c)
        {
            return str.EnsureEndsWith(c, StringComparison.Ordinal);
        }

        //
        // 摘要:
        //     Adds a char to end of given string if it does not ends with the char.
        /// <summary>
        /// Ensures the ends with.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="c">The c.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">str</exception>
        public static string EnsureEndsWith(this string str, char c, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.EndsWith(c.ToString(), comparisonType))
            {
                return str;
            }

            return str + c;
        }

        //
        // 摘要:
        //     Adds a char to end of given string if it does not ends with the char.
        /// <summary>
        /// Ensures the ends with.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="c">The c.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">str</exception>
        public static string EnsureEndsWith(this string str, char c, bool ignoreCase, CultureInfo culture)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.EndsWith(c.ToString(culture), ignoreCase, culture))
            {
                return str;
            }

            return str + c;
        }

        //
        // 摘要:
        //     Adds a char to beginning of given string if it does not starts with the char.
        /// <summary>
        /// Ensures the starts with.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        public static string EnsureStartsWith(this string str, char c)
        {
            return str.EnsureStartsWith(c, StringComparison.Ordinal);
        }

        //
        // 摘要:
        //     Adds a char to beginning of given string if it does not starts with the char.
        /// <summary>
        /// Ensures the starts with.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="c">The c.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">str</exception>
        public static string EnsureStartsWith(this string str, char c, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.StartsWith(c.ToString(), comparisonType))
            {
                return str;
            }

            return c + str;
        }

        //
        // 摘要:
        //     Adds a char to beginning of given string if it does not starts with the char.
        /// <summary>
        /// Ensures the starts with.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="c">The c.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">str</exception>
        public static string EnsureStartsWith(this string str, char c, bool ignoreCase, CultureInfo culture)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.StartsWith(c.ToString(culture), ignoreCase, culture))
            {
                return str;
            }

            return c + str;
        }

        //
        // 摘要:
        //     Indicates whether this string is null or an System.String.Empty string.
        /// <summary>
        /// Determines whether [is null or empty].
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if [is null or empty] [the specified string]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        //
        // 摘要:
        //     indicates whether this string is null, empty, or consists only of white-space
        //     characters.
        /// <summary>
        /// Determines whether [is null or white space].
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if [is null or white space] [the specified string]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        //
        // 摘要:
        //     Gets a substring of a string from beginning of the string.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     Thrown if str is null
        //
        //   T:System.ArgumentException:
        //     Thrown if len is bigger that string's length
        /// <summary>
        /// Lefts the specified length.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="len">The length.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">str</exception>
        /// <exception cref="System.ArgumentException">len argument can not be bigger than given string's length!</exception>
        public static string Left(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }

        //
        // 摘要:
        //     Converts line endings in the string to System.Environment.NewLine.
        /// <summary>
        /// Normalizes the line endings.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string NormalizeLineEndings(this string str)
        {
            return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
        }

        //
        // 摘要:
        //     Gets index of nth occurence of a char in a string.
        //
        // 参数:
        //   str:
        //     source string to be searched
        //
        //   c:
        //     Char to search in str
        //
        //   n:
        //     Count of the occurence
        /// <summary>
        /// NTHs the index of.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="c">The c.</param>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">str</exception>
        public static int NthIndexOf(this string str, char c, int n)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            int num = 0;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == c && ++num == n)
                {
                    return i;
                }
            }

            return -1;
        }

        //
        // 摘要:
        //     Removes first occurrence of the given postfixes from end of the given string.
        //     Ordering is important. If one of the postFixes is matched, others will not be
        //     tested.
        //
        // 参数:
        //   str:
        //     The string.
        //
        //   postFixes:
        //     one or more postfix.
        //
        // 返回结果:
        //     Modified string or the same string if it has not any of given postfixes
        /// <summary>
        /// Removes the post fix.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="postFixes">The post fixes.</param>
        /// <returns></returns>
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (string text in postFixes)
            {
                if (str.EndsWith(text))
                {
                    return str.Left(str.Length - text.Length);
                }
            }

            return str;
        }

        //
        // 摘要:
        //     Removes first occurrence of the given prefixes from beginning of the given string.
        //     Ordering is important. If one of the preFixes is matched, others will not be
        //     tested.
        //
        // 参数:
        //   str:
        //     The string.
        //
        //   preFixes:
        //     one or more prefix.
        //
        // 返回结果:
        //     Modified string or the same string if it has not any of given prefixes
        /// <summary>
        /// Removes the pre fix.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="preFixes">The pre fixes.</param>
        /// <returns></returns>
        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (preFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (string text in preFixes)
            {
                if (str.StartsWith(text))
                {
                    return str.Right(str.Length - text.Length);
                }
            }

            return str;
        }

        //
        // 摘要:
        //     Gets a substring of a string from end of the string.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     Thrown if str is null
        //
        //   T:System.ArgumentException:
        //     Thrown if len is bigger that string's length
        /// <summary>
        /// Rights the specified length.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="len">The length.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">str</exception>
        /// <exception cref="System.ArgumentException">len argument can not be bigger than given string's length!</exception>
        public static string Right(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }

        //
        // 摘要:
        //     Uses string.Split method to split given string by given separator.
        /// <summary>
        /// Splits the specified separator.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string[] Split(this string str, string separator)
        {
            return str.Split(new string[1] { separator }, StringSplitOptions.None);
        }

        //
        // 摘要:
        //     Uses string.Split method to split given string by given separator.
        /// <summary>
        /// Splits the specified separator.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="separator">The separator.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            return str.Split(new string[1] { separator }, options);
        }

        //
        // 摘要:
        //     Uses string.Split method to split given string by System.Environment.NewLine.
        /// <summary>
        /// Splits to lines.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string[] SplitToLines(this string str)
        {
            return Split(str, Environment.NewLine);
        }

        //
        // 摘要:
        //     Uses string.Split method to split given string by System.Environment.NewLine.
        /// <summary>
        /// Splits to lines.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static string[] SplitToLines(this string str, StringSplitOptions options)
        {
            return Split(str, Environment.NewLine, options);
        }

        //
        // 摘要:
        //     Converts PascalCase string to camelCase string.
        //
        // 参数:
        //   str:
        //     String to convert
        //
        //   invariantCulture:
        //     Invariant culture
        //
        // 返回结果:
        //     camelCase of the string
        /// <summary>
        /// Converts to camelcase.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="invariantCulture">if set to <c>true</c> [invariant culture].</param>
        /// <returns></returns>
        public static string ToCamelCase(this string str, bool invariantCulture = true)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                if (!invariantCulture)
                {
                    return str.ToLower();
                }

                return str.ToLowerInvariant();
            }

            return (invariantCulture ? char.ToLowerInvariant(str[0]) : char.ToLower(str[0])) + str.Substring(1);
        }

        //
        // 摘要:
        //     Converts PascalCase string to camelCase string in specified culture.
        //
        // 参数:
        //   str:
        //     String to convert
        //
        //   culture:
        //     An object that supplies culture-specific casing rules
        //
        // 返回结果:
        //     camelCase of the string
        /// <summary>
        /// Converts to camelcase.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static string ToCamelCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToLower(culture);
            }

            return char.ToLower(str[0], culture) + str.Substring(1);
        }

        //
        // 摘要:
        //     Converts given PascalCase/camelCase string to sentence (by splitting words by
        //     space). Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        //
        // 参数:
        //   str:
        //     String to convert.
        //
        //   invariantCulture:
        //     Invariant culture
        /// <summary>
        /// Converts to sentencecase.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="invariantCulture">if set to <c>true</c> [invariant culture].</param>
        /// <returns></returns>
        public static string ToSentenceCase(this string str, bool invariantCulture = false)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return Regex.Replace(str, "[a-z][A-Z]", (Match m) => m.Value[0] + " " + (invariantCulture ? char.ToLowerInvariant(m.Value[1]) : char.ToLower(m.Value[1])));
        }

        //
        // 摘要:
        //     Converts given PascalCase/camelCase string to sentence (by splitting words by
        //     space). Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
        //
        // 参数:
        //   str:
        //     String to convert.
        //
        //   culture:
        //     An object that supplies culture-specific casing rules.
        /// <summary>
        /// Converts to sentencecase.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static string ToSentenceCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return Regex.Replace(str, "[a-z][A-Z]", (Match m) => m.Value[0] + " " + char.ToLower(m.Value[1], culture));
        }

        //
        // 摘要:
        //     Converts string to enum value.
        //
        // 参数:
        //   value:
        //     String value to convert
        //
        // 类型参数:
        //   T:
        //     Type of enum
        //
        // 返回结果:
        //     Returns enum object
        //public static T ToEnum<T>(this string value) where T : struct
        //{
        //    if (value == null)
        //    {
        //        throw new ArgumentNullException("value");
        //    }

        //    return (T)Enum.Parse(typeof(T), value);
        //}

        //
        // 摘要:
        //     Converts string to enum value.
        //
        // 参数:
        //   value:
        //     String value to convert
        //
        //   ignoreCase:
        //     Ignore case
        //
        // 类型参数:
        //   T:
        //     Type of enum
        //
        // 返回结果:
        //     Returns enum object
        /// <summary>
        /// Converts to enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static T ToEnum<T>(this string value, bool ignoreCase) where T : struct
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// Converts to md5.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ToMd5(this string str)
        {
            using MD5 mD = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            foreach (byte b in array2)
            {
                stringBuilder.Append(b.ToString("X2"));
            }

            return stringBuilder.ToString();
        }

        //
        // 摘要:
        //     Converts camelCase string to PascalCase string.
        //
        // 参数:
        //   str:
        //     String to convert
        //
        //   invariantCulture:
        //     Invariant culture
        //
        // 返回结果:
        //     PascalCase of the string
        /// <summary>
        /// Converts to pascalcase.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="invariantCulture">if set to <c>true</c> [invariant culture].</param>
        /// <returns></returns>
        public static string ToPascalCase(this string str, bool invariantCulture = true)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                if (!invariantCulture)
                {
                    return str.ToUpper();
                }

                return str.ToUpperInvariant();
            }

            return (invariantCulture ? char.ToUpperInvariant(str[0]) : char.ToUpper(str[0])) + str.Substring(1);
        }

        //
        // 摘要:
        //     Converts camelCase string to PascalCase string in specified culture.
        //
        // 参数:
        //   str:
        //     String to convert
        //
        //   culture:
        //     An object that supplies culture-specific casing rules
        //
        // 返回结果:
        //     PascalCase of the string
        /// <summary>
        /// Converts to pascalcase.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static string ToPascalCase(this string str, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return str.ToUpper(culture);
            }

            return char.ToUpper(str[0], culture) + str.Substring(1);
        }

        //
        // 摘要:
        //     Gets a substring of a string from beginning of the string if it exceeds maximum
        //     length.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     Thrown if str is null
        /// <summary>
        /// Truncates the specified maximum length.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns></returns>
        public static string Truncate(this string str, int maxLength)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Left(maxLength);
        }

        //
        // 摘要:
        //     Gets a substring of a string from beginning of the string if it exceeds maximum
        //     length. It adds a "..." postfix to end of the string if it's truncated. Returning
        //     string can not be longer than maxLength.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     Thrown if str is null
        /// <summary>
        /// Truncates the with postfix.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns></returns>
        public static string TruncateWithPostfix(this string str, int maxLength)
        {
            return str.TruncateWithPostfix(maxLength, "...");
        }

        //
        // 摘要:
        //     Gets a substring of a string from beginning of the string if it exceeds maximum
        //     length. It adds given postfix to end of the string if it's truncated. Returning
        //     string can not be longer than maxLength.
        //
        // 异常:
        //   T:System.ArgumentNullException:
        //     Thrown if str is null
        /// <summary>
        /// Truncates the with postfix.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <param name="postfix">The postfix.</param>
        /// <returns></returns>
        public static string TruncateWithPostfix(this string str, int maxLength, string postfix)
        {
            if (str == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(str) || maxLength == 0)
            {
                return string.Empty;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            if (maxLength <= postfix.Length)
            {
                return postfix.Left(maxLength);
            }

            return str.Left(maxLength - postfix.Length) + postfix;
        }

        /// <summary>
        /// Objects to string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string ObjToString(this object obj)
        {
            if (obj is Enum)
            {
                return ((int)obj).ToString();
            }
            else
            {
                return obj?.ToString();
            }
        }
    }
}
