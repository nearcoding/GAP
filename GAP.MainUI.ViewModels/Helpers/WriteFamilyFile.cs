using GAP.BL;
using System.Collections.Generic;
using Ninject;
using GAP.MainUI.ViewModels.ViewModel;
namespace GAP.MainUI.ViewModels.Helpers
{
    public class WriteFamilyFile
    {
        public static void Write()
        {
            var densityFamily = new Family
            {
                FamilyName = "Density",
                Units = new List<string> { "PPG", "gr/cm3" }
            };
            var diameterFamily = new Family
            {
                FamilyName = "Diameter",
                Units = new List<string> { "mm", "in" }
            };
            var pressureFamily = new Family
            {
                FamilyName = "Pressure",
                Units = new List<string> { "PSI", "KPa" }
            };
            var flowFamily = new Family
            {
                FamilyName = "Flow",
                Units = new List<string> { "gal/min", "bbls/hour", "m3/sec", "l/sec" }
            };
            var sonicFamily = new Family
            {
                FamilyName = "Sonic",
                Units = new List<string> { "µsec/ft", "µsec/m" }
            };
            var resistivityFamily = new Family
            {
                FamilyName = "Resistivity",
                Units = new List<string> { "ohm*m" }
            };
            var porosityFamily = new Family
            {
                FamilyName = "Porosity",
                Units = new List<string> { "%" }
            };
            var gammaRayFamily = new Family
            {
                FamilyName = "Gamma Ray",
                Units = new List<string> { "API" }
            };
            var ropFamily = new Family
            {
                FamilyName = "ROP",
                Units = new List<string> { "ft/hr", "m/min", "min/m", "min/ft" }
            };
            var torqueFamily = new Family
            {
                FamilyName = "Torque",
                Units = new List<string> { "lb*ft", "N*m", "Klb*ft", "KN*m" }
            };
            var totalGasFamily = new Family
            {
                FamilyName = "Total Gas",
                Units = new List<string> { "%", "ppm" }
            };
            var weightFamily = new Family
            {
                FamilyName = "Weight",
                Units = new List<string> { "Klb", "KN", "Kg", "Lb", "Tonnes" }
            };
            var rpmFamily = new Family
            {
                FamilyName = "RPM",
                Units = new List<string> { "RPM" }
            };
            var caliperFamily = new Family
            {
                FamilyName = "Caliper",
                Units = new List<string> { "In", "mm" }
            };
            var lithologyFamily = new Family
            {
                FamilyName = "Lithology",
                Units = new List<string> { "Lithology" }
            };
            var drillingExponentFamily = new Family
            {
                FamilyName = "Drilling exponent",
                Units = new List<string> { "Drilling Exponent" }
            };
            var temperatureFamily = new Family
            {
                FamilyName = "Temperature",
                Units = new List<string> { "ºC", "ºF" }
            };
            var gradientFamily = new Family
            {
                FamilyName = "Gradient",
                Units = new List<string> { "psi/ft", "ºC/ft", "psi/m", "KPa/m" }
            };
            var vShaleFamily = new Family
            {
                FamilyName = "VShale",
                Units = new List<string> { "%" }
            };
            var overBurdenFamily = new Family
            {
                FamilyName = "Overburden",
                Units = new List<string> { "psi/ft", "PPG", "psi/m", "KPa/m", "psi", "KPa" }
            };
            var porePressureFamily = new Family
            {
                FamilyName = "Pore Pressure",
                Units = new List<string> { "unidades psi/ft", "PPG", "psi/m", "KPa/m", "psi", "KPa" }
            };
            var fractureGradientFamily = new Family
            {
                FamilyName = "Fracture Gradient",
                Units = new List<string> { "unidades psi/ft", "PPG", "psi/m", "KPa/m", "psi", "KPa" }
            };
            List<Family> families = new List<Family>();
            families.Add(caliperFamily);
            families.Add(densityFamily);
            families.Add(diameterFamily);
            families.Add(drillingExponentFamily);
            families.Add(flowFamily);
            families.Add(fractureGradientFamily);
            families.Add(gammaRayFamily);
            families.Add(gradientFamily);
            families.Add(lithologyFamily);
            families.Add(overBurdenFamily);
            families.Add(porePressureFamily);
            families.Add(porosityFamily);
            families.Add(pressureFamily);
            families.Add(resistivityFamily);
            families.Add(ropFamily);
            families.Add(rpmFamily);
            families.Add(sonicFamily);
            families.Add(temperatureFamily);
            families.Add(torqueFamily);
            families.Add(totalGasFamily);
            families.Add(vShaleFamily);
            families.Add(weightFamily);
            string folderName = IoC.Kernel.Get<IGlobalDataModel>().GetAppDataFolder();
            GlobalSerializer.SerializeObject<List<Family>>(families, folderName + "\\Families.xml");
        }
    }//end class
}//end namespace
