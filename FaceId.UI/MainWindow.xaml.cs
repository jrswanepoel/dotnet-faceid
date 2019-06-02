using FaceId.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FaceId.Core.FaceClassifier;

namespace FaceId.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FaceClassifier faceId;
        public MainWindow()
        {
            InitializeComponent();

            faceId = new FaceClassifier();
            faceId.FrameUpdated += FaceId_FrameUpdated;
        }

        private void FaceId_FrameUpdated(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.CameraFeed.Source = ((UpdatedFrameEventArgs)sender).ImageFrame;
            });
        }
    }
}
