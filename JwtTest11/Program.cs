using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;

namespace JwtTest11
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            #region 加密

            ////在 payload 中增加一个名字为 exp 的值，值为过期时间和 1970 / 1 / 1 00:00：00 相差的秒数
            //var payload = new Dictionary<string, object>
            //{
            //     { "UserId", 123 },
            //     { "UserName", "admin" }
            //};
            //var secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";//不要泄露

            //IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            //IJsonSerializer serializer = new JsonNetSerializer();
            //IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            //IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            //var token = encoder.Encode(payload, secret);
            //Console.WriteLine(token);

            #endregion 加密

            #region 解密

            var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJVc2VySWQiOjEyMywiVXNlck5hbWUiOiJhZG1pbiJ9.Qjw1epD5P6p4Yy2yju3-fkq28PddznqRj3ESfALQy_U";
            var secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk"; try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(token, secret, verify: true);
                Console.WriteLine(json);
            }
            catch (FormatException)
            {
                Console.WriteLine("Token format invalid");
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
            }

            #endregion 解密
        }

        //payload 因为 payload 部分是明文的，所以在不知道秘钥的时候也可以用 Decode、DecodeToObject 等不需要秘钥的方法把payload部分解析出来。
        private static void 不用秘钥解析数据()
        {
            var token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJVc2VySWQiOjEyMywiVXNlck5hbWUiOiJhZG1pbiJ9.Qjw1epD5P6p4Yy2yju3-fkq28PddznqRj3ESfALQy_U";

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(token);
                Console.WriteLine(json);
            }
            catch (FormatException)
            {
                Console.WriteLine("Token format invalid");
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
            }
        }
    }
}