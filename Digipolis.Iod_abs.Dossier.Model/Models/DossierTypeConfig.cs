using System;
using System.Collections.Generic;
using System.Text;

namespace Digipolis.Iod_abs.Dossier.Model.Models
{
    public class DossierTypeConfig
    {
        public Guid DossierTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DossierNrStructure { get; set; }
        public ICollection<ProcessConfig> Processes { get; set; }
    }
}
