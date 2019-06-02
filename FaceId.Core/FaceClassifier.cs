using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;

namespace FaceId.Core
{
    public class FaceClassifier
    {
        private VideoCapture _webcam;
        private EigenFaceRecognizer _faceRecognition;
        private CascadeClassifier _faceDetection;
        private CascadeClassifier _eyeDetection;
        private Mat _frame;

        public List<Face> Faces;

        public int ImageHeight = 150;
        public int ImageWidth = 128;
        public int TimerCounter = 0;
        public int TimeLimit = 30;
        public int ScanCounter = 0;

        private string YMLPath = @"../../data/trainingData.yml";

        public bool FaceSquare = false;
        public bool EyeSquare = false;

        public Bitmap ImageFrame;

        public event EventHandler FrameUpdated;
        public class UpdatedFrameEventArgs
        {
            public BitmapSource ImageFrame;
        }

        public FaceClassifier()
        {
            _faceRecognition = new EigenFaceRecognizer(80, double.PositiveInfinity);
            _faceDetection = new CascadeClassifier(Path.GetFullPath(@"haarcascades/haarcascade_frontalface_default.xml"));
            _eyeDetection = new CascadeClassifier(Path.GetFullPath(@"haarcascades/haarcascade_eye.xml"));

            Faces = new List<Face>();
            _frame = new Mat();
            StartCapture();
        }

        public void StartCapture()
        {
            if (_webcam == null)
                _webcam = new VideoCapture();

            _webcam.ImageGrabbed += Webcam_ImageGrabbed;
            _webcam.Start();
        }

        private void Webcam_ImageGrabbed(object sender, System.EventArgs e)
        {
            _webcam.Retrieve(_frame);
            var imageFrame = _frame.ToImage<Bgr, byte>();

            if (imageFrame != null)
            {
                var grayFrame = imageFrame.Convert<Gray, byte>();
                var faces = _faceDetection.DetectMultiScale(grayFrame, 1.3, 5);
                var eyes = _eyeDetection.DetectMultiScale(grayFrame, 1.3, 5);

                if (FaceSquare)
                    foreach (var face in faces)
                        imageFrame.Draw(face, new Bgr(Color.White), 3);
                if (EyeSquare)
                    foreach (var eye in eyes)
                        imageFrame.Draw(eye, new Bgr(Color.Blue), 3);

                var obj = new UpdatedFrameEventArgs()
                {
                    ImageFrame = imageFrame.ToBitmapSource()
                };


                FrameUpdated(obj, null);
            }
        }
    }
}
