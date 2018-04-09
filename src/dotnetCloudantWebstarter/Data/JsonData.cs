using CloudantDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetCloudantWebstarter.Models
{
    ///'''''''''''''''''''''''''''''''''''''''''''''''''''''
    /// <summary>
    /// DBﾃﾞｰﾀ取得用ｸﾗｽ
    /// </summary>
    ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    public class JsonData
    {
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ﾌﾟﾛﾊﾟﾃｨ"
            /// <summary>
            /// 総数
            /// </summary>
            public string total_rows { get; set; }
            /// <summary>
            /// 行数
            /// </summary>
            public string offset { get; set; }
            /// <summary>
            /// Jsonﾃﾞｰﾀﾘｽﾄ
            /// </summary>
            public List<JsonRows> rows { get; set; }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    }
    ///'''''''''''''''''''''''''''''''''''''''''''''''''''''
    /// <summary>
    /// ｽｹｼﾞｭｰﾙﾘｽﾄｸﾗｽ
    /// </summary>
    ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    public class JsonRows
    {
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ﾌﾟﾛﾊﾟﾃｨ"
            /// <summary>
            /// ｽｹｼﾞｭｰﾙﾃﾞｰﾀ
            /// </summary>
            public JsonSchedule doc { get; set; }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    }

    ///'''''''''''''''''''''''''''''''''''''''''''''''''''''
    /// <summary>
    /// ｽｹｼﾞｭｰﾙﾃﾞｰﾀｸﾗｽ
    /// </summary>
    ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    public class JsonSchedule
    {
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ﾌﾟﾛﾊﾟﾃｨ"
            /// <summary>
            /// ID
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// ID(ﾚｽﾎﾟﾝｽ)
            /// </summary>
            public string _Id { get; set; }
            /// <summary>
            /// ﾘﾋﾞｼﾞｮﾝ
            /// </summary>
            public string Rev { get; set; }
            /// <summary>
            /// ﾘﾋﾞｼﾞｮﾝ(ﾚｽﾎﾟﾝｽ)
            /// </summary>
            public string _Rev { get; set; }
            /// <summary>
            /// 開始日
            /// </summary>
            public string FromDate { get; set; }
            /// <summary>
            /// 開始時間
            /// </summary>
            public string FromTime { get; set; }
            /// <summary>
            /// 終了日
            /// </summary>
            public string ToDate { get; set; }
            /// <summary>
            /// 終了時間
            /// </summary>
            public string ToTime { get; set; }
            /// <summary>
            /// ﾀｲﾄﾙ
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 予定内容
            /// </summary>
            public string Remarks { get; set; }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    }
}
