using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
namespace Templar.Helpers
{
    public enum SerializedFormat
    {
        Binary,
        Document
    }
    /// <summary>
    /// Handles XML and Binary serialization/deserialization for strongly typed objects.
    /// </summary>
    public static class ObjectXMLSerializer < T > where T: class
    {
        #region Load Methods
        public static T Load(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path), "Path cannot be null or empty.");
            return LoadFromDocumentFormat(null, path, null);
        }
        public static T Load(string path, SerializedFormat serializedFormat)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            return serializedFormat == SerializedFormat.Binary ? LoadFromBinaryFormat(path, null) : LoadFromDocumentFormat(null, path, null);
        }
        public static T Load(string path, Type[] extraTypes)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            return LoadFromDocumentFormat(extraTypes, path, null);
        }
        public static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));
            return LoadFromDocumentFormat(null, fileName, isolatedStorageDirectory);
        }
        public static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory, SerializedFormat serializedFormat)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));
            return serializedFormat == SerializedFormat.Binary ? LoadFromBinaryFormat(fileName, isolatedStorageDirectory) : LoadFromDocumentFormat(null, fileName, isolatedStorageDirectory);
        }
        public static T Load(string fileName, IsolatedStorageFile isolatedStorageDirectory, Type[] extraTypes)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));
            return LoadFromDocumentFormat(extraTypes, fileName, isolatedStorageDirectory);
        }
        #endregion
        #region Save Methods
        public static void Save(T serializableObject, string path)
        {
            if (serializableObject == null) throw new ArgumentNullException(nameof(serializableObject));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            SaveToDocumentFormat(serializableObject, null, path, null);
        }
        public static void Save(T serializableObject, string path, SerializedFormat serializedFormat)
        {
            if (serializableObject == null) throw new ArgumentNullException(nameof(serializableObject));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (serializedFormat == SerializedFormat.Binary) SaveToBinaryFormat(serializableObject, path, null);
            else SaveToDocumentFormat(serializableObject, null, path, null);
        }
        public static void Save(T serializableObject, string path, Type[] extraTypes)
        {
            if (serializableObject == null) throw new ArgumentNullException(nameof(serializableObject));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            SaveToDocumentFormat(serializableObject, extraTypes, path, null);
        }
        public static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory)
        {
            if (serializableObject == null) throw new ArgumentNullException(nameof(serializableObject));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));
            SaveToDocumentFormat(serializableObject, null, fileName, isolatedStorageDirectory);
        }
        public static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory, SerializedFormat serializedFormat)
        {
            if (serializableObject == null) throw new ArgumentNullException(nameof(serializableObject));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));
            if (serializedFormat == SerializedFormat.Binary) SaveToBinaryFormat(serializableObject, fileName, isolatedStorageDirectory);
            else SaveToDocumentFormat(serializableObject, null, fileName, isolatedStorageDirectory);
        }
        public static void Save(T serializableObject, string fileName, IsolatedStorageFile isolatedStorageDirectory, Type[] extraTypes)
        {
            if (serializableObject == null) throw new ArgumentNullException(nameof(serializableObject));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));
            SaveToDocumentFormat(serializableObject, extraTypes, fileName, isolatedStorageDirectory);
        }
        #endregion
        #region Private Helpers
        private static FileStream CreateFileStream(IsolatedStorageFile isolatedStorageFolder, string path)
        {
            return isolatedStorageFolder == null ? new FileStream(path, FileMode.OpenOrCreate) : new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, isolatedStorageFolder);
        }
        private static T LoadFromBinaryFormat(string path, IsolatedStorageFile isolatedStorageFolder)
        {
            using(FileStream fileStream = CreateFileStream(isolatedStorageFolder, path))
            {
                var binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(fileStream) as T;
            }
        }
        private static T LoadFromDocumentFormat(Type[] extraTypes, string path, IsolatedStorageFile isolatedStorageFolder)
        {
            using(TextReader textReader = CreateTextReader(isolatedStorageFolder, path))
            {
                XmlSerializer xmlSerializer = CreateXmlSerializer(extraTypes);
                return xmlSerializer.Deserialize(textReader) as T;
            }
        }
        private static TextReader CreateTextReader(IsolatedStorageFile isolatedStorageFolder, string path)
        {
            return isolatedStorageFolder == null ? new StreamReader(path) : new StreamReader(new IsolatedStorageFileStream(path, FileMode.Open, isolatedStorageFolder));
        }
        private static TextWriter CreateTextWriter(IsolatedStorageFile isolatedStorageFolder, string path)
        {
            return isolatedStorageFolder == null ? new StreamWriter(path) : new StreamWriter(new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, isolatedStorageFolder));
        }
        private static XmlSerializer CreateXmlSerializer(Type[] extraTypes)
        {
            return extraTypes != null ? new XmlSerializer(typeof(T), extraTypes) : new XmlSerializer(typeof(T));
        }
        private static void SaveToDocumentFormat(T serializableObject, Type[] extraTypes, string path, IsolatedStorageFile isolatedStorageFolder)
        {
            using(TextWriter textWriter = CreateTextWriter(isolatedStorageFolder, path))
            {
                XmlSerializer xmlSerializer = CreateXmlSerializer(extraTypes);
                xmlSerializer.Serialize(textWriter, serializableObject);
            }
        }
        private static void SaveToBinaryFormat(T serializableObject, string path, IsolatedStorageFile isolatedStorageFolder)
        {
            using(FileStream fileStream = CreateFileStream(isolatedStorageFolder, path))
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, serializableObject);
            }
        }
        #endregion
    }
}
