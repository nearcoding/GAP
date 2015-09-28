using System.Collections.Generic;

namespace GAP.Helpers
{
    public static class LithologyImageFiles
    {
        static LithologyImageFiles()
        {
            Images = new Dictionary<int, string>();

            Images.Add(0, "Not Available");
            Images.Add(1, "Breccia");
            Images.Add(2, "Conglomerate");
            Images.Add(3, "Sand-SS Friable");
            Images.Add(4, "SS Conglomerate");
            Images.Add(5, "SS Coarse");
            Images.Add(6, "SS Medium");
            Images.Add(7, "SS Fine-VF");
            Images.Add(8, "SS Calcareous");
            Images.Add(9, "Slst Green");
            Images.Add(10, "Slst Brown");
            Images.Add(11, "Slst Green Gray");
            Images.Add(12, "Slst Gray");
            Images.Add(13, "Slst Yellow");
            Images.Add(14, "Clyst Violet");
            Images.Add(15, "Clyst Green");
            Images.Add(16, "Clyst Brown,Yellow");
            Images.Add(17, "Clyst Red");
            Images.Add(18, "Clyst Lt Gray");
            Images.Add(19, "Clyst Green Gray");
            Images.Add(20, "Clyst Dk Gray");
            Images.Add(21, "Shale Green");
            Images.Add(22, "Shale Gray,Blk,Carb");
            Images.Add(23, "Shale Brown");
            Images.Add(24, "Chert");
            Images.Add(25, "Marlstone");
            Images.Add(26, "Limestone");
            Images.Add(27, "Limestone Foss");
            Images.Add(28, "Limestone Sandy");
            Images.Add(29, "Limestone Dol");
            Images.Add(30, "Dolomite");
            Images.Add(31, "Gypsum-Anhidrite");
            Images.Add(32, "Coal");

        }
        public static Dictionary<int, string> Images { get; set; }
    }
}
