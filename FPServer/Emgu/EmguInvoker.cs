using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using FPServer.Core;
using System;
using System.Drawing;
using System.IO;

namespace FPServer.Emgu
{
    public class EmguInvoker : IDisposable
    {
        private static EmguInvoker _Current;
        public static EmguInvoker Current
        {
            get
            {
                _Current = new EmguInvoker();
                return _Current;
            }
        }

        private EmguInvoker()
        {

        }

        private bool efr_init = false;
        private EigenFaceRecognizer efr = new EigenFaceRecognizer();
        public EigenFaceRecognizer Efr
        {
            get
            {
                if (!efr_init)
                {
                    FileInfo fi = new FileInfo(FrameCorex.Config[Enums.AppConfigEnum.Eigen_Face_Recognizer_default_location]);
                    if (fi.Exists)
                        efr.Read(FrameCorex.Config[Enums.AppConfigEnum.Eigen_Face_Recognizer_default_location]);
                    efr_init = true;
                }
                return efr;
            }
        }

        private CascadeClassifier ccfr;
        public CascadeClassifier Ccfr
        {
            get
            {
                if (ccfr == null)
                    ccfr = new CascadeClassifier(FrameCorex.Config[Enums.AppConfigEnum.Cascade_Classifier_default_location]);
                return ccfr;
            }
        }

        public FaceRecognizer.PredictionResult Predict(string filepath)
        {
            using (Mat image = new Mat(filepath))
            {
                using (Mat uimg = new Mat())
                {
                    CvInvoke.CvtColor(image, uimg, ColorConversion.Bgr2Gray);
                    CvInvoke.EqualizeHist(uimg, uimg);
                    return Efr.Predict(uimg);
                }
            }
        }

        public void Train(string filepath, int index)
        {
            using (Mat image = new Mat(filepath))
            {
                using (Mat uimg = new Mat())
                {
                    CvInvoke.CvtColor(image, uimg, ColorConversion.Bgr2Gray);
                    CvInvoke.EqualizeHist(uimg, uimg);
                    Efr.Train(new VectorOfMat(uimg), new VectorOfInt(index));
                }
            }
        }

        public byte[] Detect(string filepath, string ext)
        {
            using (Mat uimg = new Mat())
            {
                Mat image = new Mat(filepath);
                CvInvoke.CvtColor(image, uimg, ColorConversion.Bgr2Gray);
                CvInvoke.EqualizeHist(uimg, uimg);
                foreach (var face in Ccfr.DetectMultiScale(uimg, 1.1, 10, new Size(20, 20)))
                    CvInvoke.Rectangle(image, face, new Bgr(Color.Violet).MCvScalar, 2);
                var res = new VectorOfByte();
                CvInvoke.Imencode(ext, image, res, null);
                return res.ToArray();
            }
        }

        public void SaveEntity()
        {
            Efr.Write(FrameCorex.Config[Enums.AppConfigEnum.Eigen_Face_Recognizer_default_location]);
        }

        public void Dispose()
        {
            Efr.Write(FrameCorex.Config[Enums.AppConfigEnum.Eigen_Face_Recognizer_default_location]);
            Efr.Dispose();
            Ccfr.Dispose();
        }
    }






}
