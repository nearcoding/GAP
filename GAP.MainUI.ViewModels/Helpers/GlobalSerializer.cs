using GAP.BL;
using GAP.MainUI.ViewModels.Helpers;
using GAP.MainUI.ViewModels.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Ninject;

namespace GAP.MainUI.ViewModels
{
    public static class GlobalSerializer
    {

        /// <summary>
        /// Serializes an object, used to save data to a file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public static void SerializeObject<T>(T serializableObject1, string fileName)
        {
            if (serializableObject1 == null) { return; }

            try
            {
                XmlDocument xmlDocument1 = new XmlDocument();
                Type type = serializableObject1.GetType();
                XmlSerializer serializer1 = new XmlSerializer(type);

                MemoryStream stream1 = new MemoryStream();
                serializer1.Serialize(stream1, serializableObject1);
                stream1.Position = 0;

                xmlDocument1.Load(stream1);
                xmlDocument1.Save(fileName);
                stream1.Close();
            }
            catch (Exception ex)
            {
                IoC.Kernel.Get<ISendMessage>().MessageBox(ex.Message.ToString(),
                    IoC.Kernel.Get<IGlobalDataModel>().MainViewModel.Token);
            }
        }

        public static List<Chart> DeserializeChart()
        {
            string folderName = IoC.Kernel.Get<IGlobalDataModel>().GetAppDataFolder();
            string fileName = folderName + "//DefaultCharts.xml";
            
            if (!File.Exists(fileName)) return null;
            try
            {
                Stream stream = File.Open(fileName, FileMode.Open);
                List<Chart> lst = new List<Chart>();
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(lst.GetType());

                lst = (List<Chart>)serializer.Deserialize(stream);
                stream.Close();
                return lst;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<Family> DeserializeFamily()
        {
            string folderName = IoC.Kernel.Get<IGlobalDataModel>().GetAppDataFolder();
            string fileName = folderName + "//Families.xml";
            if (!File.Exists(fileName)) return null;
            try
            {
                Stream stream = File.Open(fileName, FileMode.Open);
                List<Family> lst = new List<Family>();
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(lst.GetType());

                lst = (List<Family>)serializer.Deserialize(stream);
                stream.Close();
                return lst;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public static List<ToolbarInfo> DeserializeToolbar()
        {
            string folderName = IoC.Kernel.Get<IGlobalDataModel>().GetAppDataFolder();
            string fileName = folderName + "//ToolbarInfo.xml";
            if (!File.Exists(fileName)) return null;
            List<ToolbarInfo> lst = new List<ToolbarInfo>();
            Stream stream = File.Open(fileName, FileMode.Open);

            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(lst.GetType());

            lst = (List<ToolbarInfo>)serializer.Deserialize(stream);
            stream.Close();

            return lst;
        }

        /// <summary>
        /// Deserializes an object, in this case it must be a Projects and Wells type
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="paw"></param>
        /// <returns></returns>
        public static ProjectsAndWells DeSerializeObject(string filename, ProjectsAndWells paw)
        {
            try
            {
                ProjectsAndWells objectToSerialize;
                Stream stream = File.Open(filename, FileMode.Open);

                XmlDocument xmlDocument = new XmlDocument();

                XmlSerializer serializer1 = new XmlSerializer(paw.GetType());

                objectToSerialize = (ProjectsAndWells)serializer1.Deserialize(stream);

                stream.Close();

                return objectToSerialize;
            }
            catch (Exception)
            {

            }
            return null;
        }

    }//end class
}//end namespace
