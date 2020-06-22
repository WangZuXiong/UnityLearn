using System.IO;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
/// <summary>
/// 代理模式
/// </summary>
public class ProxyPattern : MonoBehaviour
{
    public interface IImage
    {
        void Display();
    }

    public class RealImage : IImage
    {
        private string _fileName;

        public RealImage(string fileName)
        {
            _fileName = fileName;
            LoadFromDisk(fileName);
        }

        public void Display()
        {
            Debug.Log("displaying" + _fileName);
        }

        private void LoadFromDisk(string fileName)
        {
            Debug.Log("Loading" + fileName);
        }

        public class ProxyImage : IImage
        {
            private RealImage _realImage;
            private string _fileName;

            public ProxyImage(string fileName)
            {
                _fileName = fileName;
            }

            public void Display()
            {
                if (_realImage == null)
                {
                    _realImage = new RealImage(_fileName);
                }
                _realImage.Display();
            }
        }

        public void Main()
        {
            IImage image = new ProxyImage("test_img_01.jpg");

            // 图像将从磁盘加载
            image.Display();
            // 图像不需要从磁盘加载  缓存了
            image.Display();
        }
    }
}
