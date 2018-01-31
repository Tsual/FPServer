using FPServer.Attribute;
using System;

namespace FPServer.Enums
{
    public enum AppConfigEnum {
        /// <summary>
        /// 应用默认加密对象
        /// </summary>
        AppAesObj,

        FirstInstallFlag,

        AppAesIV,

        AppAesKey,

        AppDBex,

        RandomStringCount,

        [AppConfigDefault("45")]
        ServiceDropTime,

        /// <summary>
        /// Ava>cur * ServiceInstanceObjectDestroylimit 将不再回收对象
        /// </summary>
        [AppConfigDefault("8")]
        ServiceInstanceObjectDestroylimit


    }
}
