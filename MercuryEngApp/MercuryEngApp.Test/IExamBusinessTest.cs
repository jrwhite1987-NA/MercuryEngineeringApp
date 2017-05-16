using Core.Models.ExamModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MercuryEngApp.Test
{
    public interface IExamBusinessTest
    {
        /// Get All Vessels By Exam Procedure Id.
        /// </summary>
        /// <param name="examProcId">The exam proc identifier.</param>
        /// <returns>ExamProcedureSettings</returns>
        ExamProcedureSettings GetAllVesselsByExamProcedureId(int examProcId);
    }
}
