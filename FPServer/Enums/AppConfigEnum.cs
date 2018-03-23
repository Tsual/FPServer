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

        [AppConfigDefault("0")]
        RandomStringCount,

        [AppConfigDefault("45")]
        ServiceDropTime,

        /// <summary>
        /// Ava>cur * ServiceInstanceObjectDestroylimit 将不再回收对象
        /// </summary>
        [AppConfigDefault("8")]
        ServiceInstanceObjectDestroylimit,

        [AppConfigDefault("Emgu\\Efr_config.xml")]
        Eigen_Face_Recognizer_default_location,

        [AppConfigDefault("Emgu\\haarcascade_frontalface_default.xml")]
        Cascade_Classifier_default_location




    }
}
