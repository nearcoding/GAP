using Ninject;
using GAP.BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace GAP.MainUI.ViewModels.Helpers
{
    public class WITSMLImporter
    {
        static string _xmlNamespace = @"http://www.witsml.org/schemas/110";
        static string _filePath;
        static string _token;
        public static List<Dataset> Import(string filePath, string token)
        {
            if (!File.Exists(filePath)) return null;
            _filePath = filePath;
            _token = token;
            lstDatasets.Clear();
            XElement elem = XElement.Parse(File.ReadAllText(filePath));

            XNamespace nsr = _xmlNamespace;

            IEnumerable<XElement> datasets = elem.Element(nsr + "log").Element(nsr + "logHeader").Elements(nsr + "logCurveInfo");
            if (datasets == null) return null;

            if (!GetDatasetInformation(datasets)) return null;
            if (!FillDataset()) return null;
            lstDatasets.RemoveAll(u => !u.DepthAndCurves.Any());
            return lstDatasets;
        }

        private static List<string> SeparateDepthFromFirstColumn(string firstColumnWithDepth)
        {
            var lst = firstColumnWithDepth.Split(' ').ToList();
            lst.RemoveAll(u => u == "");
            return lst;
        }
        private static bool FillDataset()
        {
            XElement element = XElement.Parse(File.ReadAllText(_filePath));
            XNamespace nsr = _xmlNamespace;
            IEnumerable<XElement> dataNodes = element.Element(nsr + "log").Element(nsr + "logData").Elements(nsr + "data");

            foreach (XElement dataNode in dataNodes)
            {
                string elementValue = dataNode.Value;
                var lst = elementValue.ToString().Split(',').ToList();
                lst.RemoveAll(u => u == "");

                var list = SeparateDepthFromFirstColumn(lst[0]);
                lst.RemoveAt(0);
                if (list.Count < 2)
                {
                    SendMessage("Invalid first column");
                    return false;
                }
                decimal depth = 0;
                if (!decimal.TryParse(list[0], out depth))
                {
                    SendMessage(string.Format("Invalid value {0} for depth", list[0]));
                    return false;
                }
                lst.Insert(0, list[1]);

                if (!AddDepthCurveInfoToDatasets(lst, depth)) return false;
            }
            return true;
        }

        private static void SendMessage(string text)
        {
            IoC.Kernel.Get<ISendMessage>().MessageBoxWithExclamation(_token, text);
        }

        private static bool AddDepthCurveInfoToDatasets(List<string> values, decimal depth)
        {
            for (int i = 0; i < values.Count; i++)
            {
                var datasetsWithSameIndex = lstDatasets.Where(u => u.DisplayIndex == i + 1);
                if (datasetsWithSameIndex.Count() != 1)
                {
                    SendMessage(string.Format("Invalid column index {0} found for dataset", i + 1));
                    return false;
                }
                Dataset currentDataset = datasetsWithSameIndex.First();

                decimal decCurve;
                if (!decimal.TryParse(values[i], out decCurve))
                {
                    SendMessage(string.Format("Invalid  value {0} for curve", values[i]));
                    return false;
                }
                currentDataset.DepthAndCurves.Add(new DepthCurveInfo
                {
                    Depth = depth,
                    Curve = decCurve
                });
            }
            return true;
        }

        private static bool GetDatasetInformation(IEnumerable<XElement> datasets)
        {
            foreach (var element in datasets)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(element.ToString());
                var nodes = (doc.ChildNodes[0] as XmlElement).ChildNodes;
                if (nodes == null)
                {
                    SendMessage("Dataset nodes not found in passed xml file");
                    return false;
                }
                CreateDatasetObject(nodes);
            }
            return true;
        }
        static List<Dataset> lstDatasets = new List<Dataset>();
        private static bool CreateDatasetObject(XmlNodeList nodes)
        {
            Dataset dataset = new Dataset();
            foreach (XmlElement node in nodes)
            {
                switch (node.LocalName)
                {
                    case "mnemonic":
                        if (string.IsNullOrWhiteSpace(node.InnerText)) 
                        {
                            SendMessage("Empty name found for dataset in xml file");
                            return false;
                        }
                        dataset.Name = node.InnerText;
                        break;
                    case "columnIndex":
                        int intVar = 0;
                        if (!Int32.TryParse(node.InnerText, out intVar))
                        {
                            SendMessage("Empty column index for dataset in xml file");
                            return false;
                        }
                        dataset.DisplayIndex = intVar;
                        break;
                }
            }
            lstDatasets.Add(dataset);
            return true;
        }
    }//end class
}//end namespace
