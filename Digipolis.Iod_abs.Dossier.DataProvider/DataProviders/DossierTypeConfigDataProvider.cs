using Digipolis.Common.DataStore.Interfaces;
using Digipolis.Common.DataStore.Models;
using Digipolis.Common.DataStore.Models.Conditions;
using Digipolis.Iod_abs.Dossier.DataProvider.Interfaces;
using Digipolis.Iod_abs.Dossier.Model.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Digipolis.Iod_abs.Dossier.DataProvider.DataProviders
{
    public class DossierTypeConfigDataProvider : IDossierTypeConfigDataProvider
    {
        private readonly IDataStoreHandler _dataStoreHandler;

        public DossierTypeConfigDataProvider(IDataStoreHandler dataStoreHandler)
        {
            _dataStoreHandler = dataStoreHandler;
        }

        public DossierTypeConfig GetDossierTypeConfigByDossierTypeId(Guid dossierTypeId)
        {
            var dossierTypeConfigDataType = _dataStoreHandler.GetDataTypeByName("DossierTypeConfig");

            var condition = new Comparison("dossierTypeId", new Operator(Operator.EQUAL), dossierTypeId.ToString());

            var searchResult = _dataStoreHandler.SearchDataObjectsOfDataTypeByCondition(dossierTypeConfigDataType, condition);
            if (searchResult.Data.Count() != 1)
            {
                throw new Exception("No Config DataObject exists for DossierType with id " + dossierTypeId);
            }
            return Map(searchResult.Data.First());
        }

        private DossierTypeConfig Map(DataObject dataObject)
        {
            var returnValue = new DossierTypeConfig();

            returnValue.Description = dataObject.Values.ContainsKey("omschrijving") ? (string)dataObject.Values["omschrijving"] : null;
            returnValue.DossierNrStructure = dataObject.Values.ContainsKey("dossierNummerStructuur") ? (string)dataObject.Values["dossierNummerStructuur"] : null;
            returnValue.DossierTypeId = dataObject.Values.ContainsKey("dossierTypeId") ? Guid.Parse((string)dataObject.Values["dossierTypeId"]) : Guid.Empty;
            returnValue.Name= dataObject.Values.ContainsKey("naam") ? (string)dataObject.Values["naam"] : null;
            returnValue.Processes = new List<ProcessConfig>();
            if (dataObject.Values.ContainsKey("processes"))
            {
                var arr = dataObject.Values["processes"] as JArray;
                foreach (var token in arr)
                {
                    returnValue.Processes.Add(new ProcessConfig
                    {
                        ProcessId = token["processId"].ToString(),
                        Name = token["naam"].ToString(),
                        Type = token["type"].ToString()
                    });
                }
            }

            return returnValue;
        }
    }
}
