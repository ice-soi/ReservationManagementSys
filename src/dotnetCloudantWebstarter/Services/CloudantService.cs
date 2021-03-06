using CloudantDotNet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CloudantDotNet.Services
{
    ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    /// <summary>
    /// DB接続処理ｸﾗｽ
    /// </summary>
    ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    public class CloudantService : ICloudantService
    {
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region "'ﾒﾝﾊﾞ"
                private static readonly string _dbName = "todos";
                private readonly Creds _cloudantCreds;
                private readonly UrlEncoder _urlEncoder;
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ｺﾝｽﾄﾗｸﾀ"

            ///'''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// ｺﾝｽﾄﾗｸﾀ
            /// </summary>
            /// <param name="creds"></param>
            /// <param name="urlEncoder"></param>
            ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,, 
            public CloudantService(Creds creds, UrlEncoder urlEncoder)
                {
                    _cloudantCreds = creds;
                    _urlEncoder = urlEncoder;
                }

        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 非同期ﾒｿｯﾄﾞ"

                ///'''''''''''''''''''''''''''''''''''''''''''''''
                /// <summary>
                /// Create
                /// </summary>
                /// <param name="item"></param>
                /// <returns></returns>
                ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,, 
                public async Task<dynamic> CreateAsync(Schedule item)
                {
                    using (var client = CloudantClient())
                    {
                        var response = await client.PostAsJsonAsync(_dbName, item);
                        if (response.IsSuccessStatusCode)
                        {
                            var responseJson = await response.Content.ReadAsAsync<Schedule>();
                            return JsonConvert.SerializeObject(new { id = responseJson.Id, rev = responseJson.Rev});
                }
                        string msg = "Failure to POST. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
                        Console.WriteLine(msg);
                        return JsonConvert.SerializeObject(new { msg = "Failure to POST. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase });
                    }
                }
                ///'''''''''''''''''''''''''''''''''''''''''''''''
                /// <summary>
                /// 
                /// </summary>
                /// <param name="item"></param>
                /// <returns></returns>
                ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
                public async Task<dynamic> DeleteAsync(Schedule item)
                {
                    using (var client = CloudantClient())
                    {
                        var response = await client.DeleteAsync(_dbName + "/" + _urlEncoder.Encode(item.Id) + "?rev=" + _urlEncoder.Encode(item.Rev));
                        if (response.IsSuccessStatusCode)
                        {
                            var responseJson = await response.Content.ReadAsAsync<Schedule>();
                            return JsonConvert.SerializeObject(new { id = responseJson.Id, rev = responseJson.Rev });
                        }
                        string msg = "Failure to DELETE. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
                        Console.WriteLine(msg);
                        return JsonConvert.SerializeObject(new { msg = msg });
                    }
                }
                ///'''''''''''''''''''''''''''''''''''''''''''''''
                /// <summary>
                /// 
                /// </summary>
                /// <returns></returns>
                ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
                public async Task<dynamic> GetAllAsync()
                {
                    using (var client = CloudantClient())
                    {
                        var response = await client.GetAsync(_dbName + "/_all_docs?include_docs=true");
                        if (response.IsSuccessStatusCode)
                        {
                            return await response.Content.ReadAsStringAsync();
                        }
                        string msg = "Failure to GET. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
                        Console.WriteLine(msg);
                        return JsonConvert.SerializeObject(new { msg = msg });
                    }
                }
                ///'''''''''''''''''''''''''''''''''''''''''''''''
                /// <summary>
                /// 
                /// </summary>
                /// <returns></returns>
                ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
                public dynamic GetAllData()
                {
                    using (var client = CloudantClient())
                    {
                        var response = client.GetStringAsync(_dbName + "/_all_docs?include_docs=true");
                        return response.Result;

                        //string msg = "Failure to GET. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
                        //Console.WriteLine(msg);
                        //return JsonConvert.SerializeObject(new { msg = msg });
                    }
                }
                ///'''''''''''''''''''''''''''''''''''''''''''''''
                /// <summary>
                /// 
                /// </summary>
                /// <param name="item"></param>
                /// <returns></returns>
                ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
                public async Task<dynamic> UpdateAsync(Schedule item)
                {
                    using (var client = CloudantClient())
                    {
                        var response = await client.PutAsJsonAsync(_dbName + "/" + _urlEncoder.Encode(item.Id) + "?rev=" + _urlEncoder.Encode(item.Rev), item);
                        if (response.IsSuccessStatusCode)
                        {
                            var responseJson = await response.Content.ReadAsAsync<Schedule>();
                            return JsonConvert.SerializeObject(new { id = responseJson.Id, rev = responseJson.Rev });
                        }
                        string msg = "Failure to PUT. Status Code: " + response.StatusCode + ". Reason: " + response.ReasonPhrase;
                        Console.WriteLine(msg);
                        return JsonConvert.SerializeObject(new { msg = msg });
                    }
                }

        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ｸﾗｲｱﾝﾄ情報取得"

            ///'''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// 設定情報からｸﾗｲｱﾝﾄ情報取得
            /// </summary>
            /// <returns>ｸﾗｲｱﾝﾄ情報</returns>
            ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,, 
            private HttpClient CloudantClient()
            {
                if (_cloudantCreds.username == null || _cloudantCreds.password == null || _cloudantCreds.host == null)
                {
                    throw new Exception("Missing Cloudant NoSQL DB service credentials");
                }

                var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes(_cloudantCreds.username + ":" + _cloudantCreds.password));

                HttpClient client = HttpClientFactory.Create(new LoggingHandler());
                client.BaseAddress = new Uri("https://" + _cloudantCreds.host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
                return client;
            }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    }

    ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    /// <summary>
    /// ｻｰﾋﾞｽ資格情報取得ｸﾗｽ
    /// </summary>
    ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    class LoggingHandler : DelegatingHandler
    {
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " '非同期送信処理"

            ///'''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// 非同期送信処理
            /// </summary>
            /// <param name="request"></param>
            /// <param name="cancellationToken"></param>
            /// <returns>ﾚｽﾎﾟﾝｽﾃﾞｰﾀ</returns>
            ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,, 
            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                Console.WriteLine("{0}\t{1}", request.Method, request.RequestUri);
                var response = await base.SendAsync(request, cancellationToken);
                Console.WriteLine(response.StatusCode);
                return response;
            }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    }
}