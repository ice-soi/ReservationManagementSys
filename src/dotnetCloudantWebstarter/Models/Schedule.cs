using DotnetCloudantWebstarter.Models;
using System;

namespace CloudantDotNet.Models
{
    ///'''''''''''''''''''''''''''''''''''''''''''''''''''''
    /// <summary>
    /// ｽｹｼﾞｭｰﾙｸﾗｽ
    /// </summary>
    ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    public class Schedule
    {
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ﾌﾟﾛﾊﾟﾃｨ"
            /// <summary>
            /// ID
            /// </summary>
            public string Id { get; set; }
            /// <summary>
            /// ﾘﾋﾞｼﾞｮﾝ
            /// </summary>
            public string Rev { get; set; }
            /// <summary>
            /// 開始日
            /// </summary>
            public string FromDate { get; set; }
            /// <summary>
            /// 開始時間
            /// </summary>
            public string FromTime { get; set; }
            /// <summary>
            /// 午前/午後
            /// </summary>
            public string AmPm { get; set; }
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

        //'''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ｺﾝｽﾄﾗｸﾀ"
            ///''''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// ﾃﾞﾌｫﾙﾄｺﾝｽﾄﾗｸﾀ
            /// </summary>
            ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            public Schedule()
            {
            }
            ///''''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// DB取得ﾃﾞｰﾀを設定
            /// </summary>
            ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            public Schedule(JsonSchedule doc)
            {
                // 開始時間の午前午後判定処理
                Func<string> isAMPM = () =>
                {
                    DateTime d = DateTime.Parse(FromTime);
                    // 午前午後を判定する
                    return (d.Hour < Const.Noon) ? Const.AM : Const.PM;
                };

                // ﾌﾟﾛﾊﾟﾃｨ設定
                Id = doc._Id;             // ID
                Rev = doc._Rev;           // rev
                FromDate = doc.FromDate;  // 開始日
                FromTime = doc.FromTime;  // 開始時間
                ToDate = doc.ToDate;      // 終了日
                ToTime = doc.ToTime;      // 終了時間
                Title = doc.Title;        // ﾀｲﾄﾙ
                Remarks = doc.Remarks;    // 内容
                AmPm = isAMPM();          // 開始時間の午前午後
            }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    }
}