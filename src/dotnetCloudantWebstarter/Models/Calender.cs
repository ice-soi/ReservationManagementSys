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
    /// ｶﾚﾝﾀﾞｰ情報ｸﾗｽ
    /// </summary>
    ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    public class Calender
    {
        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " '定数"
            /// <summary>
            /// ｶﾚﾝﾀﾞｰの種類
            /// </summary>
            enum CalenderType {Month,Week,Day};
            /// <summary>
            /// 加算
            /// </summary>
            private const int Add = 1;
            /// <summary>
            /// 減算
            /// </summary>
            private const int Sub = -1;
            /// <summary>
            /// 週加算日
            /// </summary>
            private const int MonthCount = 1;
            /// <summary>
            /// 週加算日
            /// </summary>
            private const int WeekCount = 7;
            /// <summary>
            /// 日加算日
            /// </summary>
            private const int DayCount = 1;
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ﾌﾟﾛﾊﾟﾃｨ"
        /// <summary>
        /// 前月
        /// </summary>
        public List<Days> BeforeMonth { get; set; } = new List<Days>();
            /// <summary>
            /// 当月
            /// </summary>
            public List<Days> CurrentMonth { get; set; } = new List<Days>();
            /// <summary>
            /// 翌月
            /// </summary>
            public List<Days> AfterMonth { get; set; } = new List<Days>();
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ｺﾝｽﾄﾗｸﾀ"
            ///''''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// ﾃﾞﾌｫﾙﾄｺﾝｽﾄﾗｸﾀ
            /// </summary>
            ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            public Calender()
            {
            }
            ///''''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// ｶﾚﾝﾀﾞｰのﾃﾞｰﾀを設定する
            /// </summary>
            ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            public Calender(DateTime baseDate)
            {
                DateTime start = DateTimeUtil.BeginOfMonth(baseDate);     // 当月開始日
                DateTime end = DateTimeUtil.EndOfMonth(baseDate);         // 当月終了日
                DateTime before = start.AddDays(-(WeekCount));            // 前月最終週開始日                                                                   
                DateTime after = start.AddDays(WeekCount);                // 前月最終週終了日

                // 前月のｶﾚﾝﾀﾞｰ
                BeforeMonth = Enumerable.Range(DayCount - 1, WeekCount).Select(x => new Days(before.AddDays(x), baseDate)).ToList();

                // 当月のｶﾚﾝﾀﾞｰ
                CurrentMonth = Enumerable.Range(DayCount - 1, end.Day).Select(x => new Days(start.AddDays(x), baseDate)).ToList();
            
                // 次月のｶﾚﾝﾀﾞｰ
                AfterMonth = Enumerable.Range(DayCount, WeekCount).Select(x => new Days(end.AddDays(x), baseDate)).ToList();
        }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'ﾒｿｯﾄﾞ"
            ///'''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// ｽｹｼﾞｭｰﾙ日にｽｹｼﾞｭｰﾙ情報を設定する
            /// </summary>
            /// <param name="s">ｽｹｼﾞｭｰﾙ情報</param>
            ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            public void SetSchedule(JsonData s)
            {
                // ｽｹｼﾞｭｰﾙ日と一致する日にｽｹｼﾞｭｰﾙを追加
                Action<List<Days>> setSchedule = (l) =>
                {
                    l.ForEach(c =>
                    {
                        // 該当のｽｹｼﾞｭｰﾙﾘｽﾄ作成
                        var sch = s.rows.Where(p => p.doc.FromDate == string.Format("{0} / {1} / {2}", c.Year, c.Month, c.Day)).ToList();
                        // ｽｹｼﾞｭｰﾙを設定
                        sch.ForEach(p => c.Schedule.Add(new Schedule(p.doc)));
                        // ｽｹｼﾞｭｰﾙを昇順に設定
                        c.Schedule = c.Schedule.OrderBy(d => d.FromDate).ThenBy(t => t.FromTime).ToList();
                    });
                };

                // ｽｹｼﾞｭｰﾙ情報を設定
                setSchedule(BeforeMonth);   // 前月
                setSchedule(CurrentMonth);  // 当月
                setSchedule(AfterMonth);    // 翌月
            }
            ///'''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// ｶﾚﾝﾀﾞｰのﾃﾞｰﾀを設定する
            /// </summary>
            ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            public static DateTime GetBaseDate(string[] _param)
            {
                // ﾘｸｴｽﾄﾊﾟﾗﾒｰﾀの展開
                string move_type = _param[0];                               // 戻る・進む
                int year = int.Parse(_param[1]);                            // 年
                int month = int.Parse(_param[2]);                           // 月
                int day = int.Parse(_param[3]);                             // 日
                CalenderType cal_type = (CalenderType)int.Parse(_param[4]); // ｶﾚﾝﾀﾞｰの種類

                // 基準日(年,月,日)
                DateTime baseDate = new DateTime(year, month, day);

                // ｶﾚﾝﾀﾞｰの遷移方向の判定
                int addType = (move_type == Const.Prev) ? Sub : Add;

                // 基準日に加算する対象を判定
                switch (cal_type)
                {
                    case CalenderType.Month:
                        // 基準月
                        return baseDate.AddMonths(MonthCount * addType);
                    case CalenderType.Week:
                        // 基準週
                        return baseDate.AddDays(WeekCount * addType);
                    case CalenderType.Day:
                        // 基準日
                        return baseDate.AddDays(DayCount * addType);
                    default:
                        return baseDate.AddMonths(0);
                }
            }
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    }
}
