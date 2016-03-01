using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Streams;

namespace GameExample
{
    internal static class XmlLoadManager<T>
    {
        public static async Task<T> Load(string path)
        {
            StorageFile file;
            try
            {
                file = await ApplicationData.Current.RoamingFolder.GetFileAsync(path);
            }
            catch (Exception)
            {
                return default(T);
            }   

            using (IRandomAccessStream inStream = await file.OpenReadAsync())
            {

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(inStream.AsStreamForRead());
            }
        }


        public static async void Save(T t, string path)
        {
            StorageFile file = await ApplicationData.Current.RoamingFolder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);

            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);
            using (IOutputStream outStream = stream.GetOutputStreamAt(0))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(outStream.AsStreamForWrite(), t);
                await outStream.FlushAsync();
            }
        }
    }
}