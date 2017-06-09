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

        public List<FPGARegister> FPGARegisterList { get; set; }

        public FPGARegister SelectedRegister { get; set; }

        public int Value
        {
            get { return SelectedRegister.Value; }
            set
            {
                SelectedRegister.Value = value;
                RaisePropertyChanged();
            }
        }


        public FPGAViewModel()
        {
            FPGARegisterList = FillRegisterList();
            if (FPGARegisterList != null && FPGARegisterList.Count > 0)
            {
                SelectedRegister = FPGARegisterList[0];
            }
        }

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
    }
}
