using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyUIGenerator
{
    public class UIHelper
    {
        public static ImageSource GetImageSource(string path)
        {
            return new BitmapImage(new Uri(path, UriKind.Relative));
        }
        public static BitmapImage GetImageFromByte(string path)
        {
            //  path = "asd";
            path = "/MyUIGenerator;component" + "/" + path;
            //MemoryStream ms = new MemoryStream(bytes);
            //ms.Write(bytes, 0, bytes.Length);
            //ms.Position = 0; // THIS !!!!!
            //return BitmapImage..FromStream(ms); 
            path = path.Replace("//", "/");
            BitmapImage image = new BitmapImage();
            //////MemoryStream stream = new MemoryStream(bytes);
            //////image.BeginInit();
            //////image.StreamSource = stream;
            //////image.EndInit();
            image.BeginInit();
            image.UriSource = new Uri(path, UriKind.Relative);
            image.EndInit();

            return image;
        }
        public static System.Windows.Media.Color getColorFromHexString(string color)
        {
            if (color.StartsWith("#"))
                color = color.Remove(0, 1);
            byte a = System.Convert.ToByte(color.Substring(0, 2), 16);
            byte r = System.Convert.ToByte(color.Substring(2, 2), 16);
            byte g = System.Convert.ToByte(color.Substring(4, 2), 16);
            byte b = System.Convert.ToByte(color.Substring(6, 2), 16);
            return System.Windows.Media.Color.FromArgb(a, r, g, b);
        }

        public static T Clone<T>(T obj)
        {
            DataContractSerializer dcSer = new DataContractSerializer(obj.GetType());
            MemoryStream memoryStream = new MemoryStream();

            dcSer.WriteObject(memoryStream, obj);
            memoryStream.Position = 0;

            T newObject = (T)dcSer.ReadObject(memoryStream);
            return newObject;
        }
        //internal static byte[] GetBytesFromFilePath(string path)
        //{
        //  return  System.IO.File.ReadAllBytes(path);
        //}



        //internal static Button GenerateCommand(MyUILibrary.I_Command command)
        //{
            
        //}
    }
}
