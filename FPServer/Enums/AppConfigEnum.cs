﻿namespace FPServer.Enums
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

        ServiceDropTime,

        /// <summary>
        /// Ava>cur * ServiceInstanceObjectDestroylimit 将不再回收对象
        /// </summary>
        ServiceInstanceObjectDestroylimit


    }
}
