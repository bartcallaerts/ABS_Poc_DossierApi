using System;

namespace Digipolis.Iod_abs.Dossier.Model.Models
{
    public class GenericDossier
    {
        public Guid? DataObjectId { get; set; }

        public string DossierNr { get; set; }
        public string Subject { get; set; }
        public DateTime? DateOfApplication { get; set; }
        public string Applicant { get; set; }
        public int Status { get; set; }
        public DateTime? StartDateOfEvent { get; set; }
        public DateTime? EndDateOfEvent { get; set; }
        public string Address { get; set; }
        public string GisData { get; set; }
    }
}
