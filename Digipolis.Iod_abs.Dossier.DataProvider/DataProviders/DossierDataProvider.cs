using Digipolis.Iod_abs.Dossier.DataProvider.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Digipolis.Iod_abs.Dossier.Model.Models;
using System.Threading.Tasks;
using Digipolis.Common.DataStore.Models;
using Digipolis.Common.DataStore.Interfaces;
using System.Linq;

namespace Digipolis.Iod_abs.Dossier.DataProvider.DataProviders
{
    public class DossierDataProvider : IDossierDataProvider
    {
        private readonly IDataStoreHandler _dataStoreHandler;

        public DossierDataProvider(IDataStoreHandler dataStoreHandler)
        {
            _dataStoreHandler = dataStoreHandler;
        }

        public GenericDossier GetDossierById(Guid id)
        {
            return Map(_dataStoreHandler.GetDataObjectById(id));
        }

        public IEnumerable<GenericDossier> GetDossiers()
        {
            var dataTypes = _dataStoreHandler.GetDataTypeList().Data;
            var dataObjects = new List<DataObject>();

            foreach (var dataType in dataTypes)
            {
                if (dataType.Name.ToLower().EndsWith("-dossier"))
                {
                    dataObjects.AddRange(_dataStoreHandler.GetDataObjectsByDataTypeId(dataType.Id, 1, 1000000).Data);
                }
            }

            var returnValue = new List<GenericDossier>();
            foreach(var dataObject in dataObjects)
            {
                returnValue.Add(Map(dataObject));
            }
            return returnValue;
        }

        public int CountDossiers()
        {
            var dataTypes = _dataStoreHandler.GetDataTypeList().Data;
            var returnValue = 0;

            foreach (var dataType in dataTypes)
            {
                if (dataType.Name.ToLower().EndsWith("-dossier"))
                {
                    returnValue = returnValue + _dataStoreHandler.GetDataObjectIdsByDataTypeId(dataType.Id).Count();
                }
            }
            return returnValue;
        }

        public GenericDossier InsertDossier(GenericDossier dossier, Guid dossierTypeId)
        {
            var dataObject = Map(dossier, dossierTypeId);
            var insertedDataObject = _dataStoreHandler.InsertDataObject(dataObject);
            return Map(insertedDataObject);
        }

        public GenericDossier UpdateDossier(GenericDossier dossier)
        {
            var existingDataObject = _dataStoreHandler.GetDataObjectById((Guid)dossier.DataObjectId);
            var dataObjectToUpdate = Map(dossier, Guid.Empty);
            dataObjectToUpdate.DataTypeId = existingDataObject.DataTypeId;

            return Map(_dataStoreHandler.UpdateDataObject(dataObjectToUpdate));
        }

        private GenericDossier Map(DataObject dataObject)
        {
            var returnValue = new GenericDossier();
            returnValue.Address = dataObject.Values.ContainsKey("address") ? (string)dataObject.Values["address"] : null;
            returnValue.Applicant = dataObject.Values.ContainsKey("applicant") ? (string)dataObject.Values["applicant"] : null;
            returnValue.DataObjectId = dataObject.Id;
            returnValue.DateOfApplication = dataObject.Values.ContainsKey("dateOfApplication") ? (DateTime?)dataObject.Values["dateOfApplication"] : null;
            returnValue.DossierNr = dataObject.Name;
            returnValue.EndDateOfEvent = dataObject.Values.ContainsKey("endDateOfEvent") ? (DateTime?)dataObject.Values["endDateOfEvent"] : null;
            returnValue.GisData = dataObject.Values.ContainsKey("gisData") ? (string)dataObject.Values["gisData"] : null;
            returnValue.StartDateOfEvent = dataObject.Values.ContainsKey("startDateOfEvent") ? (DateTime?)dataObject.Values["startDateOfEvent"] : null;
            returnValue.GisData = dataObject.Values.ContainsKey("gisData") ? (string)dataObject.Values["gisData"] : null;
            returnValue.Status = dataObject.Values.ContainsKey("status") ? Convert.ToInt32((long)dataObject.Values["status"]) : 0;
            returnValue.Subject = dataObject.Values.ContainsKey("subject") ? (string)dataObject.Values["subject"] : null;

            return returnValue;
        }

        private DataObject Map(GenericDossier dossier, Guid dossierTypeId)
        {
            var returnValue = new DataObject();
            returnValue.Name = dossier.DossierNr;
            returnValue.DataTypeId = dossierTypeId;
            returnValue.Values = new Dictionary<string, object>();
            returnValue.Values.Add("dossierNr", dossier.DossierNr);
            if (dossier.Subject != null)
            {
                returnValue.Values.Add("subject", dossier.Subject);
            }
            if (dossier.Applicant != null)
            {
                returnValue.Values.Add("applicant", dossier.Applicant);
            }
            if (dossier.DateOfApplication != null)
            {
                returnValue.Values.Add("dateOfApplication", dossier.DateOfApplication);
            }
            if (dossier.StartDateOfEvent != null)
            {
                returnValue.Values.Add("startDateOfEvent", dossier.StartDateOfEvent);
            }
            if (dossier.EndDateOfEvent != null)
            {
                returnValue.Values.Add("endDateOfEvent", dossier.EndDateOfEvent);
            }
            if (dossier.Address != null)
            {
                returnValue.Values.Add("address", dossier.Address);
            }
            if (dossier.GisData != null)
            {
                returnValue.Values.Add("gisData", dossier.GisData);
            }

            returnValue.Values.Add("status", dossier.Status);

            if (dossier.DataObjectId != null)
            {
                returnValue.Id = (Guid)dossier.DataObjectId;
            }

            return returnValue;
        }
    }
}
