using Emgu.CV;
using Emgu.CV.Structure;



namespace FaceId.Core
{
    public class Face
    {
        public Image<Gray, byte> Image { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
