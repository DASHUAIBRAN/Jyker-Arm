namespace OpenBLive.Runtime.Data
{
    public struct BToken
    {
        /// <summary>
        /// -1=non oauthKey
        /// -2=oauthKey not marching
        /// -4=未扫码
        /// -5=已扫码
        ///  0=登录成功
        /// </summary>
        public int code;
        /// <summary>
        /// 登录用户的uid
        /// </summary>
        public long dedeUserID;
        /// <summary>
        /// 用户md5key
        /// </summary>
        public string dedeUserIDCkMd5;
        /// <summary>
        /// accesskey 或 浏览器中的cookie
        /// </summary>
        public string sessData;
        /// <summary>
        /// 登录cookie
        /// </summary>
        public string biliJct;
    }
}