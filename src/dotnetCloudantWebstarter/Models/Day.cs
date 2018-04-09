using CloudantDotNet.Models;
using DotnetCloudantWebstarter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetCloudantWebstarter.Models
{
    ///'''''''''''''''''''''''''''''''''''''''''''''''''''''
    /// <summary>
    /// 日付情報ｸﾗｽ
    /// </summary>
    ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    public class Days
    {
        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " '定数"
            /// <summary>
            /// 基準日
            /// </summary>
            private const string ThisDays = "1";
            /// <summary>
            /// 基準日以外
            /// </summary>
            private const string OtherDays = "0";
            /// <summary>
            /// 基準日を含む週
            /// </summary>
            private const string ThisWeeks = "1";
            /// <summary>
            /// 基準日を含まない週
            /// </summary>
            private const string OtherWeeks = "0";
            /// <summary>
            /// 前月最終週開始日
            /// </summary>
            private const string BeforeStartDays = "1";
            /// <summary>
            /// 前月最終週開始日以外
            /// </summary>
            private const string OtherBeforeStartDays = "0";
            /// <summary>
            /// 次月初週終了日
            /// </summary>
            private const string AfterEndDays = "1";
            /// <summary>
            /// 次月初週終了日以外
            /// </summary>
            private const string OtherAfterEndDays = "0";
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ﾌﾟﾛﾊﾟﾃｨ"
            /// <summary>
            /// 年
            /// </summary>
            public string Year { get; set; }
            /// <summary>
            /// 月
            /// </summary>
            public string Month { get; set; }
            /// <summary>
            /// 日
            /// </summary>
            public string Day { get; set; }
            /// <summary>
            /// 曜日
            /// </summary>
            public string Dow { get; set; }
            /// <summary>
            /// 基準日
            /// </summary>
            public string ThisDay { get; set; }
            /// <summary>
            /// 基準週
            /// </summary>
            public string ThisWeek { get; set; }
            /// <summary>
            /// 前月最終週開始日
            /// </summary>
            public string BeforeStartDay { get; set; }
            /// <summary>
            /// 次月初週終了日
            /// </summary>
            public string AfterEndDay { get; set; }
            /// <summary>
            /// 予定
            /// </summary>
            public List<Schedule> Schedule { get; set; } = new List<Schedule>();
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,   

        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ｺﾝｽﾄﾗｸﾀ"
            /// <summary>
            /// ﾃﾞﾌｫﾙﾄｺﾝｽﾄﾗｸﾀ
            /// </summary>
            public Days()
            {
            }
            /// <summary>
            /// ﾊﾟﾗﾒｰﾀを元に日付ﾃﾞｰﾀを初期化
            /// </summary>
            public Days(DateTime compDate,DateTime baseDate)
            {
                // 基準日を含む週の判定処理
                Func<string> setWeek = () => 
                {
                    DateTime before = baseDate.AddDays(-3);   // 3日前
                    DateTime after = baseDate.AddDays(3);     // 3日後
                    // 基準日の前後3日以内の場合基準週とする
                    if (compDate.Date >= before && compDate.Date <= after) { return ThisWeeks; }
                    return OtherWeeks;
                };

                // 基準日判定処理
                Func<string> setDay = () =>
                {
                    // 当日を基準日とする
                    if (compDate.Date == baseDate.Date) { return ThisDays; }
                    return OtherDays;
                };

                // 前月最終週開始日判定処理
                Func<string> setBeforeStartDay = () =>
                {
                    DateTime start = DateTimeUtil.BeginOfMonth(baseDate);         // 当月開始日
                    start = start.AddDays(((int)(start.DayOfWeek) == (int)DayOfWeek.Sunday) ? -7 : -(int)(start.DayOfWeek));
                    // 前月最終週の日曜日を開始日とする
                    if (compDate.Date < baseDate && compDate.Date >= start.Date) { return BeforeStartDays; }
                    return OtherBeforeStartDays;
                };

                // 次月初週終了日判定処理
                Func<string> setAfterEndDay = () =>
                {
                    DateTime after = DateTimeUtil.EndOfMonth(baseDate);             // 当月終了日
                    after = after.AddDays(((int)(after.DayOfWeek) == (int)DayOfWeek.Saturday) ? 7 : 6 - (int)(after.DayOfWeek));
                    // 次月初週の土曜日を終了日とする
                    if (compDate.Date > baseDate &&  compDate.Date <= after.Date) {return AfterEndDays;}
                    return OtherAfterEndDays;
                };

                // ﾊﾟﾗﾒｰﾀを設定
                Year = compDate.Year.ToString();       // 年
                Month = compDate.Month.ToString("00"); // 月   
                Day = compDate.Day.ToString("00");     // 日
                Dow = compDate.DayOfWeek.ToString();   // 曜日
                ThisDay = setDay();                    // 基準日
                ThisWeek = setWeek();                  // 基準週
                BeforeStartDay = setBeforeStartDay();  // 前月最終週開始日
                AfterEndDay = setAfterEndDay();        // 前月最終週終了日
        }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,   
    }
}
