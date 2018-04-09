using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DotnetCloudantWebstarter.Services;
using DotnetCloudantWebstarter.Models;
using CloudantDotNet.Services;
using Newtonsoft.Json;
using CloudantDotNet.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotnetCloudantWebstarter.Controllers
{
    ///'''''''''''''''''''''''''''''''''''''''''''''''''''''
    /// <summary>
    /// ｽｹｼﾞｭｰﾙ情報ｺﾝﾄﾛｰﾗｰ
    /// </summary>
    ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ﾒﾝﾊﾞ"
            //''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// DB取得Service
            /// </summary>
            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            private readonly ICloudantService _cloudantService;
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ｺﾝｽﾄﾗｸﾀ"
            //''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// ｺﾝｽﾄﾗｸﾀ
            /// </summary>
            /// <param name="cloudantService"></param>
            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            public ScheduleController(ICloudantService cloudantService)
            {
                _cloudantService = cloudantService;
            }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'Get"
            //''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// 初期表示用ｶﾚﾝﾀﾞｰ情報取得
            /// </summary>
            /// <returns></returns>
            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            [HttpGet]
            public string Get()
            {
                // ｶﾚﾝﾀﾞｰの基本ﾃﾞｰﾀを設定
                Calender calender = new Calender(DateTime.Today);
                // DBﾃﾞｰﾀを取得
                dynamic dbData = _cloudantService.GetAllData();
                // 取得したｽｹｼﾞｭｰﾙをｶﾚﾝﾀﾞｰに設定
                calender.SetSchedule(JsonConvert.DeserializeObject<JsonData>(dbData));
                // JSONﾃﾞｰﾀとしてｸﾗｲｱﾝﾄに返却
                return JsonConvert.SerializeObject(calender);
            }
            //''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// 月移動時ｶﾚﾝﾀﾞｰ情報取得
            /// </summary>
            /// <returns></returns>
            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            [HttpGet("{param}")]
            public string Get(string param)
            {
                // ｶﾚﾝﾀﾞｰの基本ﾃﾞｰﾀを設定
                Calender calender = new Calender(Calender.GetBaseDate(param.Split('_')));
                // DBﾃﾞｰﾀを取得
                dynamic dbData = _cloudantService.GetAllData();
                // 取得したｽｹｼﾞｭｰﾙをｶﾚﾝﾀﾞｰに設定
                calender.SetSchedule(JsonConvert.DeserializeObject<JsonData>(dbData));
                // JSONﾃﾞｰﾀとしてｸﾗｲｱﾝﾄに返却
                return JsonConvert.SerializeObject(calender);
            }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'Post"
            //'''''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// 情報登録
            /// </summary>
            /// <returns></returns>
            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            [HttpPost]
            public async Task<dynamic> Create(Schedule item)
            {
                // IDとRevisionを初期値設定
                item.Id = null;
                item.Rev = "null";

                return await _cloudantService.CreateAsync(item);
            }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'Put"
            //''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// 情報更新
            /// </summary>
            /// <returns></returns>
            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            [HttpPut]
            public async Task<dynamic> Update(Schedule item)
            {
                return await _cloudantService.UpdateAsync(item);
            }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'Delete"
            //''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// 情報削除
            /// </summary>
            /// <returns></returns>
            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            [HttpDelete]
            public async Task<dynamic> Delete(Schedule item)
            {
                return await _cloudantService.DeleteAsync(item);
            }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    }
}
