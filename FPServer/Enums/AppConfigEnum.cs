using FPServer.Attribute;
using System;

namespace FPServer.Enums
{
    public enum AppConfigEnum {
        /// <summary>
        /// 应用默认加密对象
        /// </summary>
        AppAesObj,

        /// <summary>
        /// 实例初次部署标志
        /// </summary>
        FirstInstallFlag,

        /// <summary>
        /// 应用加密向量
        /// </summary>
        AppAesIV,

        /// <summary>
        /// 应用加密密钥
        /// </summary>
        AppAesKey,

        /// <summary>
        /// 应用最后启动时间
        /// </summary>
        AppDBex,

        /// <summary>
        /// 随机字符串序列计数
        /// </summary>
        [AppConfigDefault("0")]
        RandomStringCount,

        /// <summary>
        /// 服务缓存销毁时间
        /// </summary>
        [AppConfigDefault("45")]
        ServiceDropTime,

        /// <summary>
        /// Ava>cur * ServiceInstanceObjectDestroylimit 将不再回收对象
        /// </summary>
        [AppConfigDefault("8")]
        ServiceInstanceObjectDestroylimit,

        /// <summary>
        /// 人脸鉴别路径
        /// </summary>
        [AppConfigDefault("Emgu\\Efr_config.xml")]
        Eigen_Face_Recognizer_default_location,

        /// <summary>
        /// 人脸识别器配置路径
        /// </summary>
        [AppConfigDefault("Emgu\\haarcascade_frontalface_default.xml")]
        Cascade_Classifier_default_location,

        /// <summary>
        /// 监听端口
        /// </summary>
        [AppConfigDefault("5555")]
        AppPort,

        /// <summary>
        /// 维护器周期 分钟
        /// </summary>
        [AppConfigDefault("30")]
        MaintenanceTime



    }
}
