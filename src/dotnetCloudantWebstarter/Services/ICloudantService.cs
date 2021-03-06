using CloudantDotNet.Models;
using System.Threading.Tasks;

namespace CloudantDotNet.Services
{
    ///''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    /// <summary>
    /// DB接続処理ｲﾝﾀｰﾌｪｰｽ
    /// </summary>
    ///,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    public interface ICloudantService
    {
        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " '新規作成"
            //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// 新規作成
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            Task<dynamic> CreateAsync(Schedule item);
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " '削除"
            //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// 削除
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            Task<dynamic> DeleteAsync(Schedule item);
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'DB全項目取得"
            //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// DB全項目取得
            /// </summary>
            /// <returns></returns>
            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            Task<dynamic> GetAllAsync();
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " 'DB全項目取得(同期処理)"
            //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// DB全項目取得(同期処理)
            /// </summary>
            /// <returns></returns>
            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            dynamic GetAllData();
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,

        //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        #region " '更新"
            //'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            /// <summary>
            /// 更新
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
            Task<dynamic> UpdateAsync(Schedule item);
        #endregion
        //,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,
    }
}