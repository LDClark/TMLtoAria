using System;
using System.Collections.Generic;
using VMS.TPS.Common.Model.API;

namespace TMLtoAria
{
    public class PlanViewModel
    {
        public string PatientName { get; set; }
        public string PatientId { get; set; }
        public string PatientPrimaryOncologist { get; set; }
        public string PatientBirthdate { get; set; }
        public string PatientSex { get; set; }
        public string CourseId { get; set; }
        public string CourseIntent { get; set; }
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanIntent { get; set; }
        public IEnumerable<Beam> Beams { get; set; }
        public string PlanType { get; set; }
        public DateTime PlanCreation { get; set; }
        public string PlanStructureSetId { get; set; }
        public string PlanImageId { get; set; }
        public DateTime PlanImageCreation { get; set; }
        public string PlanIdWithFractionation { get; set; }
        public string TreatmentOrientation { get; set; }
        public string ImageUserOrigin { get; set; }       
        public string TargetVolumeId { get; set; }
        public string PrimaryReferencePointId { get; set; }
    }
}
