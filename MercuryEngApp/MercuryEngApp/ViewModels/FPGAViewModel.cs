using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MercuryEngApp
{
    public class FPGAViewModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets the FPGA Register List
        /// </summary>
        public List<FPGARegister> FPGARegisterList { get; set; }

        /// <summary>
        /// Gets or sets the Selected Register
        /// </summary>
        public FPGARegister SelectedRegister { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// </summary>
        public int Value
        {
            get { return SelectedRegister.Value; }
            set
            {
                SelectedRegister.Value = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public FPGAViewModel()
        {
            FPGARegisterList = FillRegisterList();
            if (FPGARegisterList != null && FPGARegisterList.Count > 0)
            {
                SelectedRegister = FPGARegisterList[0];
            }
        }

        /// <summary>
        /// Fill the Register List
        /// </summary>
        /// <returns></returns>
        private List<FPGARegister> FillRegisterList()
        {
            try
            {
                XDocument xmlDoc = XDocument.Load("LocalFolder/FPGARegisters.xml");
                return xmlDoc.Root.Elements("FPGARegister")
                    .Select(x => new FPGARegister
                    {
                        DefaultValue = (string)(x.Element("DefaultValue").Value),
                        Name = (string)x.Element("Name").Value,
                        MemoryLocation = (string)(x.Element("MemoryLocation").Value),
                        Description = (string)x.Element("Desciption").Value,
                        NoOfBytes = Convert.ToInt32(x.Element("NoOfBytes").Value)
                    }).ToList();
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

        /// <summary>
        /// Get the FPGA User Guide Content
        /// </summary>
        /// <returns></returns>
        public string GetFPGAUserGuideContent()
        {            
            return System.IO.File.ReadAllText("LocalFolder/FPGAUserGuide.txt");
        }
    }
}
