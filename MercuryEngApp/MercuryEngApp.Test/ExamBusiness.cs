using Core.Models.ExamModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MercuryEngApp.Test
{
    class ExamBusiness : IExamBusinessTest 
    {
        private IExamBusinessTest examRepository;
        private Core.Interfaces.ExamInterface.IExamRepository examRepository1;

        public ExamBusiness(IExamBusinessTest examRepository)
        {
            // TODO: Complete member initialization
            this.examRepository = examRepository;
        }

        public ExamProcedureSettings GetAllVesselsByExamProcedureId(int examProcId)
        {
            ExamProcedureSettings examProcedureSettings = new ExamProcedureSettings();
            List<ExamProcedureSetting> TestList = new List<ExamProcedureSetting>();
            ExamProcedureSetting Setting = new ExamProcedureSetting();
            Setting.ExamProcedureID = 3;
            TestList.Add(Setting);
            examProcedureSettings.ExamProcedureSettingList = TestList;           
            return examProcedureSettings;
        }
    }
}
