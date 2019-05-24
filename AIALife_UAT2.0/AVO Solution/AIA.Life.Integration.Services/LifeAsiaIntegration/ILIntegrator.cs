using AIA.CrossCutting;
using AIA.Life.Models.Common;
using AIA.Life.Models.Payment;
using AIA.Life.Models.UWDecision;
using AIA.Life.Repository.AIAEntity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace AIA.Life.Integration.Services.LifeAsiaIntegration
{
    public class ILIntegrator
    {
        /// <summary>
        /// this method modifies property by adding required extra charecters
        /// </summary>
        /// <param name="propertValue"></param>
        /// <param name="PropertyLength"></param>
        /// <returns></returns>
        public string CreatePattern(string propertValue, int PropertyLength)
        {
            if (propertValue == "" || propertValue == null)
            {
                propertValue = " ";
                propertValue = propertValue.PadRight(PropertyLength, ' ');
                // propertValue += "|";
            }
            else if (propertValue.Length < PropertyLength)
            {
                propertValue = propertValue.PadRight(PropertyLength, ' ');
                //propertValue += "|";
            }
            else
            {
                propertValue = propertValue.Substring(0, PropertyLength);
                //propertValue += "|";
            }
            return propertValue;
        }
        /// <summary>
        /// this method modifies data based on datatype 
        /// </summary>
        /// <param name="Property">property value to be appended to plain taxt</param>
        /// <param name="PlainText">plain text which has other data</param>
        /// <param name="PropLength">length of the property as per Policy Asia</param>
        /// <param name="DataType">Data type of the property</param>
        /// <returns>plain text</returns>
        public string AppendToPlainText(string Property, string PlainText, int PropLength, string DataType, string FieldValue = null, string Prod = null)
        {
            switch (DataType)
            {
                case "bool":
                    if (Property == "true" || Property == "True" || Property == "TRUE")
                    {
                        Property = "Y";
                    }
                    else
                    {
                        Property = "N";
                    }

                    break;
                case "datetime":
                    if (!(string.IsNullOrEmpty(Property)))
                    {
                        Property = Property.Substring(0, 10);
                        string[] date = Property.Split('-');
                        //to replace the date from ddmmyyyy to yyyymmdd format
                        if (date[0].Length < 3)
                        {
                            Property = (date[2] == "" ? "0000" : date[2]) +
                                                                   (date[1] == "" ? "00" : date[1]) +
                                                                   (date[0] == "" ? "00" : date[0]);
                        }
                        else
                        {
                            Property = (date[0] == "" ? "0000" : date[0]) +
                                       (date[1] == "" ? "00" : date[1]) +
                                       (date[2] == "" ? "00" : date[2]);
                        }
                        //Added for initiated dates
                        if (Property == "01010001")
                        {
                            Property = "99999999";
                        }
                    }
                    else
                    {
                        Property = "99999999";
                    }
                    break;
                case "int":
                    if (Property == null)
                    {
                        Property = "0";
                    }
                    if (Property != null)
                    {
                        if (Property != "")
                        {
                            if (!Property.Contains("+") && !Property.Contains("-"))
                            {
                                Property = Convert.ToInt32(Property).ToString();
                            }
                        }
                        if (Property.Length < PropLength)
                        {
                            if (!(string.IsNullOrEmpty(Property)))
                            {
                                Property = Property.PadLeft(PropLength, '0');
                            }
                        }
                    }
                    break;
                case "long":
                    if (Property == null)
                    {
                        Property = "0";
                    }
                    if (Property != null)
                    {
                        if (Property != "")
                        {
                            if (!Property.Contains("+") && !Property.Contains("-"))
                            {
                                Property = Convert.ToInt64(Property).ToString();
                            }
                        }
                        if (Property.Length < PropLength)
                        {
                            if (!(string.IsNullOrEmpty(Property)))
                            {
                                Property = Property.PadLeft(PropLength, '0');
                            }
                        }
                    }
                    break;
                case "numeric":
                case "decimal":
                    if (Property == null)
                    {
                        Property = "0";
                    }
                    if (Property != null)
                    {
                        if (Property != "")
                        {
                            if (!Property.Contains("+") && !Property.Contains("-"))
                            {
                                Property = Math.Round(Convert.ToDecimal(Property) * 100).ToString();
                            }
                        }
                        if (Property.Length < PropLength)
                        {
                            if (!(string.IsNullOrEmpty(Property)))
                            {
                                Property = Property.PadLeft(PropLength, '0');
                            }
                        }
                    }
                    break;
                case "percentage":
                    if (!string.IsNullOrEmpty(Property) && Property != "0")
                    {
                        string sign = null;
                        if (Property.Contains("+") || Property.Contains("-"))
                        {
                            sign = Property.Substring(0, 1);
                            Property = Property.Remove(0, 1);
                        }
                        string[] field = FieldValue.Split(',');
                        int decimalValue = Convert.ToInt32(field[0]);
                        string l1 = "0";
                        l1 = l1.PadLeft(decimalValue, '0');
                        int decimalPoints = Convert.ToInt32(field[1]);
                        string l2 = "0";
                        l2 = l2.PadLeft(decimalPoints, '0');
                        Property = string.Format("{0:" + l1 + "." + l2 + "}", Convert.ToDecimal(Property)).Replace(".", "");
                        if (sign != null)
                        {
                            Property = Property + sign;
                        }
                    }
                    else
                    {
                        Property = "";
                    }
                    break;
                case "time":
                    if (!(string.IsNullOrEmpty(Property)))
                    {
                        if (Property.Length > 15)
                        {
                            Property = Property.Substring(11, 5);
                        }
                    }
                    else
                    {
                        Property = "";
                    }
                    break;
                default:
                    if (string.IsNullOrEmpty(Property))
                    {

                        StringBuilder str = new StringBuilder();
                        for (int i = 0; i < PropLength; i++)
                        {
                            str.Append(" ");
                        }
                        Property = str.ToString();
                    }
                    break;


            }
            Property = CreatePattern(Property, PropLength);
            PlainText += Property;
            return PlainText;
        }
        /// <summary>
        /// this method fetches Leader header, Session Header and Entity Header
        /// </summary>
        /// <param name="PlainText"></param>
        /// <param name="CallingFunction"></param>
        /// <param name="seq">sequence no for the request</param>
        /// <returns>Sequence no for the request</returns>
        public string FetchHeaderData(string PlainText, string CallingFunction, ref string msgRefNo)
        {
            AVOAIALifeEntities entity = new AVOAIALifeEntities();
            string[] Methods = new string[4];
            Methods = new string[3] { "LeaderHeader", "SessionHeader", "DataHeader" };
            List<tblILIntegration> data = new List<tblILIntegration>();
            string Property = string.Empty;
            int? seq = entity.usp_GetNextSequenceNumber("MSGREFNO").FirstOrDefault();
            msgRefNo = seq.ToString().PadLeft(8, '0');
            foreach (string methodName in Methods)
            {
                data = (from obj in entity.tblILIntegrations
                        where obj.CallingFunction == methodName
                        select obj).OrderBy(a => a.FeildOrder).ToList();
                int fieldLength = 0;
                foreach (tblILIntegration row in data)
                {
                    fieldLength = 0;
                    fieldLength = row.Length ?? 0;
                    switch (row.ColumnName)
                    {
                        case "MSGREFNO":
                            Property = msgRefNo;
                            break;
                        case "TOTMSGLNG":
                            Property = (from obj in entity.tblMasHeaderDatas
                                        where obj.CallingFunction == CallingFunction && obj.ColumnName == "TOTMSGLNG"
                                        select obj.FieldValue).FirstOrDefault();
                            if (Property == null)
                            {
                                Property = row.FieldValue == null ? "" : "";
                            }

                            break;
                        case "MSGLNG":
                            Property = (from obj in entity.tblMasHeaderDatas
                                        where obj.CallingFunction == CallingFunction && obj.ColumnName == "MSGLNG"
                                        select obj.FieldValue).FirstOrDefault();
                            if (Property == null)
                            {
                                Property = row.FieldValue == null ? "" : "";
                            }

                            break;
                        case "MSGID":
                            Property = (from obj in entity.tblMasHeaderDatas
                                        where obj.CallingFunction == CallingFunction && obj.ColumnName == "MSGID"
                                        select obj.FieldValue).FirstOrDefault();
                            if (Property == null)
                            {
                                Property = row.FieldValue == null ? "" : "";
                            }

                            break;
                        case "OBJID":
                            Property = (from obj in entity.tblMasHeaderDatas
                                        where obj.CallingFunction == CallingFunction && obj.ColumnName == "OBJID"
                                        select obj.FieldValue).FirstOrDefault();
                            if (Property == null)
                            {
                                Property = row.FieldValue == null ? "" : "";
                            }

                            break;
                        case "VRBID":
                            Property = (from obj in entity.tblMasHeaderDatas
                                        where obj.CallingFunction == CallingFunction && obj.ColumnName == "VRBID"
                                        select obj.FieldValue).FirstOrDefault();
                            if (Property == null)
                            {
                                Property = row.FieldValue == null ? "" : "";
                            }

                            break;
                        //case "ACCTYR":
                        //    Property = DateTime.Now.Year.ToString();
                        //    break;
                        //case "ACCTMN":
                        //    Property = DateTime.Now.Month.ToString().PadLeft(2, '0');
                        //    break;
                        default:
                            Property = row.FieldValue == null ? "" : row.FieldValue;
                            break;

                    }

                    if (row.ColumnName == "MSGID" && methodName == "SessionHeader")
                    {
                        Property = row.FieldValue;
                    }

                    PlainText = AppendToPlainText(Property, PlainText, fieldLength, row.DataType);
                    PlainText = PlainText.Replace('~', ' ');//Added to replace header data by emty space
                }
                //tblIntegrationTxnLog objLog = new tblIntegrationTxnLog();
                //objLog.CallingFunction = methodName;
                //objLog.PlainText = PlainText;
                //objLog.CreatedBy = null;
                //objLog.CreatedDate = DateTime.Now;
                //objLog.PlainTextLength = PlainText.Length;
                //entity.tblIntegrationTxnLogs.Add(objLog);
                //entity.SaveChanges();
            }
            return PlainText;
        }
        /// <summary>
        /// this method converts xml of a sepcific class to Plain text,
        /// sends request to MQueue
        /// and Reads the response received
        /// and converts to xml
        /// </summary>
        /// <param name="xmlData">xml form of the class object</param>
        /// <param name="CallingFunction">Function Name in iNubePortals from which GeneratePlainText is called</param>
        /// <param name="MethodName">Service method name</param>
        /// <returns>processed Response xml string</returns>
        public string GeneratePlainText(object obj, string CallingFunction, string responFunc)
        {
            string PlainText = string.Empty;
            string Property = string.Empty;
            string ResponseXml = string.Empty;
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                string seqNo = string.Empty;
                PlainText = FetchHeaderData(PlainText, CallingFunction, ref seqNo);
                IQueryable<MappingData> mappingData = entity.tblILIntegrations.Where(a => a.CallingFunction == CallingFunction).OrderBy(a => a.FeildOrder).Select(a => new MappingData
                {
                    SourceObjectProperty = a.XMLPath,
                    FieldValue = a.FieldValue,
                    IsHardCoded = a.IsHardCode,
                    DataType = a.DataType,
                    ColumnName = a.ColumnName,
                    Occurance = a.Occurance,
                    PropertyLength = a.Length
                });
                switch (CallingFunction)
                {
                    case "AddLifeRequest":
                        PlainText += AddLifeDetails(obj, mappingData);
                        break;
                    case "ModifyLifeRequest":
                        PlainText += ModifyLifeDetails(obj, mappingData);
                        break;
                    case "FollowUpModifyRequest":
                        PlainText += FollowupModifyRequest(obj, mappingData);
                        break;
                    case "WithdrawRequest":
                        PlainText += WithdrawProposalRequest(obj, mappingData);
                        break;
                    case "ClientRelCreationRequest":
                        PlainText += ClientRelationCreationRequest(obj, mappingData);
                        break;
                    case "DeleteLifeRequest":
                        PlainText += DeleteLife(obj, mappingData);
                        break;
                    default:
                        PlainText += AppendObjectPropertyValue(obj, mappingData);
                        break;
                }
                PlainText = PlainText.Replace('~', ' ');
                PlainText.Replace('.', ' ');
                PlainText = PlainText.ToUpper();

                #region


                Guid guid = Guid.NewGuid();
                byte[] bytes = guid.ToByteArray();
                string refCorrMsgID = Convert.ToBase64String(bytes);

                tblIntegrationTxnLog objLog = new tblIntegrationTxnLog
                {
                    CallingFunction = CallingFunction,
                    PlainText = PlainText,
                    CreatedBy = null,
                    CreatedDate = DateTime.Now,
                    PlainTextLength = PlainText.Length,
                    MsgID = seqNo,
                    CorelationID = refCorrMsgID
                };
                entity.tblIntegrationTxnLogs.Add(objLog);
                entity.SaveChanges();

                string Result = QueueProcessor.PutQueue(entity, PlainText, refCorrMsgID, CallingFunction, responFunc);

                int retryCount = 0;
                string Response = string.Empty;
                do
                {
                    Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["WaitTime1"]));
                    Response = QueueProcessor.GetQueue(entity, System.Text.Encoding.UTF8.GetBytes(refCorrMsgID), responFunc);
                    if (Response.Contains("MQRC_NO_MSG_AVAILABLE"))
                    {
                        retryCount = retryCount + 1;
                    }
                    else
                    {
                        retryCount = 6;
                    }
                } while (retryCount < 6);//need to configure retry count in web.config

                tblIntegrationTxnLog objLog1 = new tblIntegrationTxnLog
                {
                    CallingFunction = responFunc,
                    PlainText = Response,
                    CreatedBy = null,
                    CreatedDate = DateTime.Now,
                    PlainTextLength = Response.Length,
                    MsgID = seqNo,
                    CorelationID = refCorrMsgID
                };
                entity.tblIntegrationTxnLogs.Add(objLog1);
                entity.SaveChanges();
                #endregion
                return Response;
            }
        }
        internal string AppendObjectPropertyValue(object Sourceobj, IQueryable<MappingData> MappingDatavalues)
        {
            object Output = null;
            string plaintText = string.Empty;
            IEnumerable<PropertyInfo> properties = Sourceobj.GetType().GetProperties();

            foreach (MappingData Path in MappingDatavalues)
            {
                if (Path.SourceObjectProperty != null)
                {
                    Output = Sourceobj;
                    #region //Getting value from Source object

                    if (Path.SourceObjectProperty.Contains("."))
                    {
                        string[] bits = Path.SourceObjectProperty.Split('.');
                        object Newobj = Sourceobj;
                        for (int i = 0; i < bits.Length - 1; i++)
                        {

                            PropertyInfo propertyToGet = Newobj.GetType().GetProperty(bits[i]);
                            if (propertyToGet == null)
                            {
                                FieldInfo Fieldproperty = Newobj.GetType().GetField(bits[i]);
                                Newobj = Fieldproperty.GetValue(Newobj);
                            }
                            else
                            {
                                Newobj = propertyToGet.GetValue(Newobj);
                            }
                        }
                        if (Newobj != null)
                        {
                            PropertyInfo propertyToSet = Newobj.GetType().GetProperty(bits.Last());
                            if (propertyToSet == null)
                            {
                                FieldInfo property = Newobj.GetType().GetField(bits.Last());
                                Output = property.GetValue(Newobj);
                            }
                            else
                            {
                                Output = propertyToSet.GetValue(Newobj);
                            }
                        }
                        else
                        {
                            Output = null;
                        }
                    }
                    else
                    {
                        PropertyInfo propertyToSet = Sourceobj.GetType().GetProperty(Path.SourceObjectProperty);
                        if (propertyToSet == null)
                        {
                            FieldInfo FieldProperty = Sourceobj.GetType().GetField(Path.SourceObjectProperty);
                            Output = FieldProperty.GetValue(Sourceobj);
                        }
                        else
                        {
                            Output = propertyToSet.GetValue(Sourceobj);
                        }
                    }


                    #endregion

                }
                else
                {
                    if (Path.IsHardCoded)
                    {
                        Output = Path.FieldValue;
                    }
                }

                plaintText = AppendToPlainText(Output == null ? null : Output.ToString(), plaintText, Path.PropertyLength ?? 0, Path.DataType);
            }


            return plaintText;
        }
        internal object FillObjectPropertyValue(object Destinationobj, IQueryable<MappingData> MappingDatavalues)
        {
            object Output = null;
            string plaintText = string.Empty;

            foreach (MappingData Path in MappingDatavalues)
            {
                Output = Path.FieldValue;
                #region //assign value to destination object
                if (Output != null)
                {
                    string Destinationspath = Path.SourceObjectProperty;
                    if (Destinationspath != null)
                    {
                        if (Destinationspath.Contains("."))
                        {
                            object NewDestobj = Destinationobj;
                            string[] bits = Destinationspath.Split('.');
                            for (int i = 0; i < bits.Length - 1; i++)
                            {
                                PropertyInfo propertyToGet = NewDestobj.GetType().GetProperty(bits[i]);
                                if (propertyToGet == null)
                                {
                                    FieldInfo Fieldproperty = NewDestobj.GetType().GetField(bits[i]);
                                    NewDestobj = Fieldproperty.GetValue(NewDestobj);
                                }
                                else
                                {
                                    NewDestobj = propertyToGet.GetValue(NewDestobj, null);
                                }
                            }
                            PropertyInfo propertyToSet = NewDestobj.GetType().GetProperty(bits.Last());
                            if (propertyToSet == null)
                            {
                                FieldInfo property = NewDestobj.GetType().GetField(bits.Last());
                                property.SetValue(NewDestobj, FillPropertyOnDataType(Output, Path.DataType));
                            }
                            else
                            {
                                propertyToSet.SetValue(NewDestobj, FillPropertyOnDataType(Output, Path.DataType), null);
                            }
                        }
                        else
                        {
                            PropertyInfo propertyToSet = Destinationobj.GetType().GetProperty(Destinationspath);
                            if (propertyToSet == null)
                            {
                                FieldInfo FieldProperty = Destinationobj.GetType().GetField(Destinationspath);
                                FieldProperty.SetValue(Destinationobj, FillPropertyOnDataType(Output, Path.DataType));
                            }
                            else
                            {
                                propertyToSet.SetValue(Destinationobj, FillPropertyOnDataType(Output, Path.DataType), null);
                            }
                        }

                    }

                }
                #endregion
            }


            return Destinationobj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseText"></param>
        /// <param name="obj"></param>
        /// <param name="respFunc"></param>
        /// <returns></returns>
        public object ReadResponseString(string responseText, object obj, string respFunc)
        {
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                if (responseText.Length > 0)
                {
                    if (responseText.Substring(0, 5) == "Error" || responseText.Substring(0, 5) == "error" || responseText.Contains("MQRC_NO_MSG_AVAILABLE") || responseText.Contains("Held by user"))
                    {

                    }
                    else
                    {
                        if (responseText.ToLower().Contains("boverr"))
                        {
                            //process error message 
                            FillResponseObject(entity, obj, "Boverr", responseText);
                        }
                        else if (responseText.ToUpper().Contains("SFERRREC"))
                        {
                            //process error message
                            FillResponseObject(entity, obj, "SFERRREC", responseText);
                        }
                        else
                        {
                            //process success message
                            switch (respFunc)
                            {
                                case "ClientRelEnqResponse":
                                    obj = ClientRelationEnquiryResponse(entity, obj, respFunc, responseText);
                                    break;
                                case "PreIssueRulesResponse":
                                    obj = PreIssueValidationResponse(entity, obj, respFunc, responseText);
                                    break;
                                case "FollowUpEnqResponse":
                                    obj = FollowUpEnquiryResponse(entity, obj, respFunc, responseText);
                                    break;
                                default:
                                    FillResponseObject(entity, obj, respFunc, responseText);
                                    break;
                            }

                        }
                    }
                }
                else
                {
                    //objService.ErrorMsg = "No Output Msg received";
                }

                return obj;
            }
        }
        internal object FillResponseObject(AVOAIALifeEntities entity, object obj, string respFunc, string responseText)
        {
            List<MappingData> mappingData = entity.tblILIntegrations.Where(a => a.CallingFunction == respFunc).OrderBy(a => a.FeildOrder).Select(a => new MappingData
            {
                SourceObjectProperty = a.XMLPath,
                FieldValue = a.FieldValue,
                IsHardCoded = a.IsHardCode,
                DataType = a.DataType,
                PropertyLength = a.Length
            }).ToList();
            int msgLength = Convert.ToInt32(entity.tblMasHeaderDatas.Where(a => a.CallingFunction == respFunc && a.ColumnName == "MSGLNG").Select(a => a.FieldValue).FirstOrDefault());
            int totalLength = Convert.ToInt32(entity.tblMasHeaderDatas.Where(a => a.CallingFunction == respFunc && a.ColumnName == "TOTMSGLNG").Select(a => a.FieldValue).FirstOrDefault());
            string msgString = responseText.Substring(totalLength - msgLength, msgLength);
            int dataLength = 0;

            foreach (MappingData row in mappingData)
            {
                if (msgString.Length >= (dataLength + row.PropertyLength))
                {
                    row.FieldValue = msgString.Substring(dataLength, row.PropertyLength ?? 0).Trim();
                    dataLength += row.PropertyLength ?? 0;
                }
            }
            obj = FillObjectPropertyValue(obj, mappingData.Where(a => a.SourceObjectProperty != null).AsQueryable());
            return obj;
        }
        internal object FillPropertyOnDataType(object value, string dataType)
        {
            object newObj = new object();
            switch (dataType)
            {
                case "int":
                    newObj = Convert.ToInt32(value);
                    break;
                case "bool":
                    newObj = Convert.ToBoolean(value);
                    break;
                case "datetime":
                    if (value.ToString() == "99999999" || value.ToString() == "00000000")
                    {
                        newObj = DateTime.Now;
                    }
                    else
                    {
                        newObj = new DateTime(Convert.ToInt32(value.ToString().Substring(0, 4)), Convert.ToInt32(value.ToString().Substring(4, 2)), Convert.ToInt32(value.ToString().Substring(6, 2)));
                    }

                    break;
                case "long":
                    newObj = Convert.ToInt64(value);
                    break;
                case "double":
                    newObj = Convert.ToDouble(value);
                    break;
                case "decimal":
                    newObj = Convert.ToDecimal(Convert.ToDecimal(value) / 100);
                    break;
                default:
                    newObj = value;
                    break;
            }
            return newObj;
        }
        internal string AddLifeDetails(object Sourceobj, IQueryable<MappingData> MappingDatavalues)
        {
            object Output = null;
            MemberDetails member = (MemberDetails)Sourceobj;
            string plainText = string.Empty;
            foreach (MappingData Path in MappingDatavalues.Where(a => a.Occurance == 0).ToList())
            {
                switch (Path.ColumnName)
                {
                    case "BGEN-S5002-CHDRSEL":
                        Output = member.ProposalNo;
                        break;
                    case "BGEN-MAIN-COVERAGE":
                        if (member.RelationShipWithPropspect == "267")
                        {
                            Output = member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault();
                        }
                        else if (member.RelationShipWithPropspect == "268")
                        {
                            Output = "SPOU";
                        }
                        else if (member.RelationShipWithPropspect == "271")
                        {
                            Output = "CHIL";
                        }

                        break;
                    case "BGEN-S5005-LIFESEL":
                        Output = member.ClientCode;
                        break;
                    case "BGEN-S5005-ANNE":
                        Output = Convert.ToInt64(member.MonthlyIncome);
                        break;
                    case "BGEN-S5005-HEIGHT":
                        Output = member.objLifeStyleQuetions.Height;
                        break;
                    case "BGEN-S5005-WEIGHT":
                        Output = member.objLifeStyleQuetions.Weight;
                        break;
                    case "BGEN-S6765-ANSWER":
                        Output = member.AnyAdverseRemarks == true ? "Y" : "N";
                        break;
                    default:
                        Output = Path.FieldValue;
                        break;
                }

                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, Path.PropertyLength ?? 0, Path.DataType);
            }
            List<MappingData> riders = MappingDatavalues.Where(a => a.Occurance == 10).ToList();

            if (member.objBenifitDetails.Where(a => a.RiderCode == "TFCA").FirstOrDefault() != null)
            {
                int tfcaIndex = member.objBenifitDetails.IndexOf(member.objBenifitDetails.Where(a => a.RiderCode == "TFCA").FirstOrDefault());
                member.objBenifitDetails.Insert(tfcaIndex + 1, new BenifitDetails() { RiderCode = "TFCR", RiderSuminsured = "100000", BenifitOpted = true });
            }

            for (int i = 0; i < 10; i++)
            {
                string riderText = string.Empty;
                foreach (MappingData Path in riders)
                {
                    if (member.objBenifitDetails.Count >= i + 1)
                    {
                        switch (Path.ColumnName)
                        {

                            case "BGEN-CRTABLE":
                                if (member.Age > 46)
                                {
                                    switch (member.objBenifitDetails[i].RiderCode)//for CIB if >46 CIB with Angioplasty has to be sent
                                    {
                                        case "TMCI":
                                            Output = "TMCJ";
                                            break;
                                        case "TMCD":
                                            Output = "TMCE";
                                            break;
                                        case "TSCA":
                                            Output = "TSCB";
                                            break;
                                        default:
                                            Output = member.objBenifitDetails[i].RiderCode;
                                            break;
                                    }
                                }
                                else
                                {
                                    Output = member.objBenifitDetails[i].RiderCode;
                                }

                                break;
                            case "BGEN-MATTRM":
                                if (member.objBenifitDetails[i].BenifitName == "Basic Life Cover")
                                {
                                    Output = member.PolicyTerm;
                                }
                                else
                                {
                                    Output = Convert.ToInt32(0);
                                }

                                break;
                            case "BGEN-PCESSTRM":
                                if (member.objBenifitDetails[i].BenifitName == "Basic Life Cover")
                                {
                                    Output = member.PremiumTerm;
                                }
                                else
                                {
                                    Output = Convert.ToInt32(0);
                                }

                                break;
                            case "BGEN-SINGPRM":
                                Output = Convert.ToDecimal(member.objBenifitDetails[i].RiderPremium);
                                break;
                            case "BGEN-SUMIN":
                                if (member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() != "TPPG")
                                {
                                    List<string> wop = new List<string> { "TMWG", "TMWH", "TMWI", "TMWK", "TMWL", "TMWM", "TEPB", "TPPH","TMWO" };
                                    if (wop.Contains(member.objBenifitDetails[i].RiderCode))
                                    {
                                        Output = 0M;
                                    }
                                    else
                                    {
                                        Output = Convert.ToDecimal(member.objBenifitDetails[i].RiderSuminsured);
                                    }
                                }
                                else if (member.objBenifitDetails[i].RiderCode == "TMCI" || member.objBenifitDetails[i].RiderCode == "TMHF")
                                {
                                    Output = Convert.ToDecimal(member.objBenifitDetails[i].RiderSuminsured);
                                }
                                else
                                {
                                    Output = Path.FieldValue;
                                }

                                break;
                            case "BGEN-GUARPERD":
                                if (member.MaturityBenefit == "2381")
                                {
                                    if (member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TPPG" ||
                                        member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TPPH")
                                    {
                                        Output = 10;
                                    }
                                    else
                                    {
                                        Output = 0;
                                    }
                                }
                                else
                                {
                                    Output = member.PensionTerm;
                                }

                                break;
                            case "BGEN-ZEXTSFLDA"://annual basic premium for smart builder gold
                                if (member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TSBL"
                                    || member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TSBM")
                                {
                                    Output = Convert.ToDecimal(member.BasicSumInsured);
                                }
                                else
                                {
                                    Output = Path.FieldValue;
                                }

                                break;
                            case "BGEN-ZEXTNFLDA"://Sam value for smart builder gold
                                if (member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TSBL"
                                    || member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TSBM")
                                {
                                    Output = Convert.ToDecimal(member.SAM);
                                }
                                else
                                {
                                    Output = Path.FieldValue;
                                }

                                break;
                            case "BGEN-ZEXTCFLDA"://If accident rider is opted for Smart builder gold the send Y
                                if (member.objBenifitDetails[i].RiderCode == "TSBL" || member.objBenifitDetails[i].RiderCode == "TSBM")
                                {
                                    if (member.objBenifitDetails.Where(a => a.RiderCode == "TMAO").FirstOrDefault() != null ||
                                        member.objBenifitDetails.Where(a => a.RiderCode == "TMAO").FirstOrDefault() != null)
                                    {
                                        Output = "Y";
                                    }
                                    else
                                    {
                                        Output = "N";
                                    }
                                }
                                else
                                {
                                    Output = "N";
                                }

                                break;
                            case "BGEN-OPCDA":
                                if (Convert.ToDecimal(string.IsNullOrEmpty(member.objBenifitDetails[i].LoadingPercentage) == true ? "0" : member.objBenifitDetails[i].LoadingPercentage) > 0 || Convert.ToDecimal(string.IsNullOrEmpty(member.objBenifitDetails[i].LoadinPerMille) == true ? "0" : member.objBenifitDetails[i].LoadinPerMille) > 0)
                                {
                                    Output = "OC";
                                }
                                else
                                {
                                    Output = Path.FieldValue;
                                }

                                break;
                            case "BGEN-OPPC":
                                if (Convert.ToDecimal(string.IsNullOrEmpty(member.objBenifitDetails[i].LoadingPercentage) == true ? "0" : member.objBenifitDetails[i].LoadingPercentage) > 0)
                                {
                                    Output = Convert.ToDecimal(member.objBenifitDetails[i].LoadingPercentage) + 100;
                                }
                                else
                                {
                                    Output = Path.FieldValue;
                                }

                                break;
                            case "BGEN-INSPRM":
                                if (Convert.ToInt32(string.IsNullOrEmpty(member.objBenifitDetails[i].LoadinPerMille) == true ? "0" : member.objBenifitDetails[i].LoadinPerMille) > 0)
                                {
                                    Output = Convert.ToInt32(member.objBenifitDetails[i].LoadinPerMille) * 1000;
                                }
                                else
                                {
                                    Output = Path.FieldValue;
                                }

                                break;
                            case "BGEN-OPTEXTIND":
                                if (Convert.ToDecimal(string.IsNullOrEmpty(member.objBenifitDetails[i].LoadingPercentage)==true?"0": member.objBenifitDetails[i].LoadingPercentage) > 0 || Convert.ToDecimal(string.IsNullOrEmpty(member.objBenifitDetails[i].LoadinPerMille) == true ? "0" : member.objBenifitDetails[i].LoadinPerMille) > 0)
                                {
                                    Output = "X";
                                }
                                else
                                {
                                    Output = Path.FieldValue;
                                }

                                break;
                            case "BGEN-DTHPERLS":
                                if (member.objBenifitDetails.Where(a => a.RiderCode == "TPPH").FirstOrDefault() != null)
                                {
                                    Output = Convert.ToDecimal(100 - member.MonthlySavingBenifit);
                                }
                                else
                                {
                                    Output = Path.FieldValue;
                                }

                                break;
                            case "BGEN-DDFDEDUC":
                                Output = member.Deductible;
                                break;
                            default:
                                Output = Path.FieldValue;
                                break;
                        }
                    }
                    else
                    {
                        Output = Path.FieldValue;
                    }

                    riderText = AppendToPlainText(Output == null ? null : Output.ToString(), riderText, Path.PropertyLength ?? 0, Path.DataType);
                }
                plainText += riderText;
            }
            return plainText;
        }
        internal string ModifyLifeDetails(object Sourceobj, IQueryable<MappingData> MappingDatavalues)
        {
            object Output = null;
            AVOAIALifeEntities entities = new AVOAIALifeEntities();
            AIA.Life.Models.Policy.Policy policy = (AIA.Life.Models.Policy.Policy)Sourceobj;
            string plainText = string.Empty;
            int iterationCnt = MappingDatavalues.Where(a => a.Occurance == 1).Count();
            iterationCnt = iterationCnt + MappingDatavalues.Where(a => a.Occurance > 1).Select(a => a.Occurance).Distinct().Count();
            List<MappingData> mappedData = MappingDatavalues.ToList();
            for (int i = 0; i < iterationCnt; i++)
            {
                if (mappedData[i].Occurance == 1)
                {
                    switch (mappedData[i].ColumnName)
                    {
                        case "BGEN-S5002-CHDRSEL":
                            Output = policy.ProposalNo;
                            break;
                        case "BGEN-MOD-HEADER":
                            Output = "Y";
                            break;
                        case "BGEN-MOD-LIFE":
                            Output = "N";
                            break;
                        case "BGEN-MOD-LIFENUM":
                            Output = "00";
                            break;
                        case "BGEN-DEL-LIFE":
                            Output = "N";
                            break;
                        case "BGEN-DEL-LIFENUM":
                            Output = "00";
                            break;
                        case "BGEN-DEL-COVER":
                            Output = "N";
                            break;
                        case "BGEN-S5004-AGNTSEL":
                            Output = policy.AgentCode;
                            break;
                        case "BGEN-S5004-ZNEEDTYP":
                            int needID = Convert.ToInt32(policy.ProposalNeed);
                            Output = entities.tblMasCommonTypes.Where(a => a.CommonTypesID == needID).Select(a => a.Code).FirstOrDefault();
                            break;
                        case "BGEN-S5004-EMPNUM":
                            Output = policy.IntroducerCode;
                            break;
                        case "BGEN-S5004-BNFYING":
                            if (policy.objNomineeDetails.Count() > 0)
                            {
                                Output = "X";
                            }
                            else
                            {
                                Output = null;
                            }

                            break;
                        case "BGEN-S5004-MOP":
                            int pmID = Convert.ToInt32(policy.PaymentMethod);
                            Output = entities.tblMasCommonTypes.Where(a => a.CommonTypesID == pmID).Select(a => a.Code).FirstOrDefault();
                            break;
                        case "BGEN-S5004-OCCdatetime":
                            Output = policy.CommencementDate;
                            break;
                        case "BGEN-S5004-HPROPDTE":
                            Output = policy.ProposalDate;
                            break;
                        case "BGEN-S5004-HPRRCVDT":
                            Output = policy.ProposalSubmittedDate;
                            break;
                        case "BGEN-S5004-OWNERSEL":
                            if (policy.objProspectDetails.IsproposerlifeAssured == false)
                            {
                                Output = policy.objProspectDetails.ClientCode;
                            }
                            else
                            {
                                Output = mappedData[i].FieldValue;
                            }

                            break;
                        default:
                            Output = mappedData[i].FieldValue;
                            break;
                    }
                    plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, mappedData[i].PropertyLength ?? 0, mappedData[i].DataType);
                }
                else if (mappedData[i].Occurance == 2)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 2).ToList())
                        {
                            Output = item.FieldValue;
                            plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                            if (j == 0)
                            {
                                mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                            }
                        }
                    }
                    i = i - 1;
                    iterationCnt = iterationCnt - 1;
                }
                else if (mappedData[i].Occurance == 5)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 5).ToList())
                        {
                            if (policy.objNomineeDetails.Count() >= j + 1)
                            {
                                switch (item.ColumnName)
                                {
                                    case "BGEN-S5010-BNYSEL":
                                        Output = policy.objNomineeDetails[j].NomineeClientCode;
                                        break;
                                    case "BGEN-S5010-BNYPC":
                                        Output = Convert.ToDecimal(policy.objNomineeDetails[j].NomineePercentage);
                                        break;
                                    default:
                                        Output = item.FieldValue;
                                        break;
                                }
                            }
                            else
                            {
                                Output = item.FieldValue;
                            }
                            plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                            if (j == 0)
                            {
                                mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                            }
                        }
                    }
                    i = i - 1;
                    iterationCnt = iterationCnt - 1;
                }
                else if (mappedData[i].Occurance == 14)
                {
                    for (int j = 0; j < 14; j++)
                    {
                        foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 14).ToList())
                        {
                            Output = item.FieldValue;
                            plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                            if (j == 0)
                            {
                                mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                            }
                        }
                    }
                    i = i - 1;
                    iterationCnt = iterationCnt - 1;
                }
                else if (mappedData[i].Occurance == 10)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 10).ToList())
                        {
                            Output = item.FieldValue;
                            plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                            if (j == 0)
                            {
                                mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                            }
                        }
                    }
                    i = i - 1;
                    iterationCnt = iterationCnt - 1;
                }

            }
            return plainText;
        }
        internal string ModifyLifeDetailsPolicyRemarks(object Sourceobj)
        {
            string plainText = string.Empty;
            string Property = string.Empty;
            string ResponseXml = string.Empty;
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                string seqNo = string.Empty;
                plainText = FetchHeaderData(plainText, "ModifyLifeRequest", ref seqNo);
                IQueryable<MappingData> MappingDatavalues = entity.tblILIntegrations.Where(a => a.CallingFunction == "ModifyLifeRequest").OrderBy(a => a.FeildOrder).Select(a => new MappingData
                {
                    SourceObjectProperty = a.XMLPath,
                    FieldValue = a.FieldValue,
                    IsHardCoded = a.IsHardCode,
                    DataType = a.DataType,
                    ColumnName = a.ColumnName,
                    Occurance = a.Occurance,
                    PropertyLength = a.Length
                });
                object Output = null;
                AIA.Life.Models.Policy.Policy policy = (AIA.Life.Models.Policy.Policy)Sourceobj;
                int iterationCnt = MappingDatavalues.Where(a => a.Occurance == 1).Count();
                iterationCnt = iterationCnt + MappingDatavalues.Where(a => a.Occurance > 1).Select(a => a.Occurance).Distinct().Count();
                List<MappingData> mappedData = MappingDatavalues.ToList();
                for (int i = 0; i < iterationCnt; i++)
                {
                    if (mappedData[i].Occurance == 1)
                    {
                        switch (mappedData[i].ColumnName)
                        {
                            case "BGEN-S5002-CHDRSEL":
                                Output = policy.ProposalNo;
                                break;
                            case "BGEN-MOD-HEADER":
                                Output = "Y";
                                break;
                            case "BGEN-MOD-LIFE":
                                Output = "N";
                                break;
                            case "BGEN-MOD-LIFENUM":
                                Output = "00";
                                break;
                            case "BGEN-DEL-LIFE":
                                Output = "N";
                                break;
                            case "BGEN-DEL-LIFENUM":
                                Output = "00";
                                break;
                            case "BGEN-DEL-COVER":
                                Output = "N";
                                break;
                            case "BGEN-S5004-AGNTSEL":
                                Output = policy.AgentCode;
                                break;
                            case "BGEN-S5004-ZNEEDTYP":
                                int needID = Convert.ToInt32(policy.ProposalNeed);
                                Output = entity.tblMasCommonTypes.Where(a => a.CommonTypesID == needID).Select(a => a.Code).FirstOrDefault();
                                break;
                            case "BGEN-S5004-EMPNUM":
                                Output = policy.IntroducerCode;
                                break;
                            case "BGEN-S5004-BNFYING":
                                if (policy.objNomineeDetails.Count() > 0)
                                {
                                    Output = "X";
                                }
                                else
                                {
                                    Output = null;
                                }

                                break;
                            case "BGEN-S5004-MOP":
                                int pmID = Convert.ToInt32(policy.PaymentMethod);
                                Output = entity.tblMasCommonTypes.Where(a => a.CommonTypesID == pmID).Select(a => a.Code).FirstOrDefault();
                                break;
                            case "BGEN-S5004-OCCdatetime":
                                Output = policy.CommencementDate;
                                break;
                            case "BGEN-S5004-HPROPDTE":
                                Output = policy.ProposalDate;
                                break;
                            case "BGEN-S5004-HPRRCVDT":
                                Output = policy.ProposalSubmittedDate;
                                break;
                            case "BGEN-S5004-IND":
                                if (!string.IsNullOrEmpty(policy.UWComments))
                                {
                                    Output = "X";
                                }
                                else
                                {
                                    Output = mappedData[i].FieldValue;
                                }

                                break;
                            default:
                                Output = mappedData[i].FieldValue;
                                break;
                        }
                        plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, mappedData[i].PropertyLength ?? 0, mappedData[i].DataType);
                    }
                    else if (mappedData[i].Occurance == 2)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 2).ToList())
                            {
                                Output = item.FieldValue;
                                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                                if (j == 0)
                                {
                                    mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                                }
                            }
                        }
                        i = i - 1;
                        iterationCnt = iterationCnt - 1;
                    }
                    else if (mappedData[i].Occurance == 5)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 5).ToList())
                            {
                                if (policy.objNomineeDetails.Count() >= j + 1)
                                {
                                    switch (item.ColumnName)
                                    {
                                        case "BGEN-S5010-BNYSEL":
                                        Output = policy.objNomineeDetails[j].NomineeClientCode;
                                        break;
                                        case "BGEN-S5010-BNYPC":
                                        Output = Convert.ToDecimal(policy.objNomineeDetails[j].NomineePercentage);
                                        break;
                                        default:
                                        Output = item.FieldValue;
                                        break;
                                    }
                                }
                                else
                                {
                                    Output = item.FieldValue;
                                }
                                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                                if (j == 0)
                                {
                                    mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                                }
                            }
                        }
                        i = i - 1;
                        iterationCnt = iterationCnt - 1;
                    }
                    else if (mappedData[i].Occurance == 14)
                    {
                        List<string> policyNotes = new List<string>();
                        if (!string.IsNullOrEmpty(policy.UWComments))
                        {
                            string tempPolicyNotes = policy.UWComments;
                            while (tempPolicyNotes.Length != 0)
                            {
                                if (tempPolicyNotes.Length < 78)
                                {
                                    policyNotes.Add(tempPolicyNotes);
                                    tempPolicyNotes = string.Empty;
                                }
                                else
                                {
                                    policyNotes.Add(tempPolicyNotes.Substring(0, 78));
                                    tempPolicyNotes = tempPolicyNotes.Remove(0, 78);
                                }
                            }
                        }
                        for (int j = 0; j < 14; j++)
                        {
                            foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 14).ToList())
                            {
                                if (policyNotes.Count() >= j + 1)
                                {
                                    Output = policyNotes[j];
                                }
                                else
                                {
                                    Output = "";
                                }

                                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                                if (j == 0)
                                {
                                    mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                                }
                            }
                        }
                        i = i - 1;
                        iterationCnt = iterationCnt - 1;
                    }
                    else if (mappedData[i].Occurance == 10)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 10).ToList())
                            {
                                Output = item.FieldValue;
                                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                                if (j == 0)
                                {
                                    mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                                }
                            }
                        }
                        i = i - 1;
                        iterationCnt = iterationCnt - 1;
                    }

                }
                plainText = plainText.Replace('~', ' ');
                plainText.Replace('.', ' ');
                plainText = plainText.ToUpper();

                #region


                Guid guid = Guid.NewGuid();
                byte[] bytes = guid.ToByteArray();
                string refCorrMsgID = Convert.ToBase64String(bytes);

                tblIntegrationTxnLog objLog = new tblIntegrationTxnLog
                {
                    CallingFunction = "ModifyLife Policy Remarks",
                    PlainText = plainText,
                    CreatedBy = null,
                    CreatedDate = DateTime.Now,
                    PlainTextLength = plainText.Length,
                    MsgID = seqNo,
                    CorelationID = refCorrMsgID
                };
                entity.tblIntegrationTxnLogs.Add(objLog);
                entity.SaveChanges();

                string Result = QueueProcessor.PutQueue(entity, plainText, refCorrMsgID, "ModifyLifeRequest", "ModifyLifeResponse");

                int retryCount = 0;
                string Response = string.Empty;
                do
                {
                    Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["WaitTime1"]));
                    Response = QueueProcessor.GetQueue(entity, System.Text.Encoding.UTF8.GetBytes(refCorrMsgID), "ModifyLifeResponse");
                    if (Response.Contains("MQRC_NO_MSG_AVAILABLE"))
                    {
                        retryCount = retryCount + 1;
                    }
                    else
                    {
                        retryCount = 6;
                    }
                } while (retryCount < 6);//need to configure retry count in web.config

                tblIntegrationTxnLog objLog1 = new tblIntegrationTxnLog
                {
                    CallingFunction = "ModifyLife Policy Remarks",
                    PlainText = Response,
                    CreatedBy = null,
                    CreatedDate = DateTime.Now,
                    PlainTextLength = Response.Length,
                    MsgID = seqNo,
                    CorelationID = refCorrMsgID
                };
                entity.tblIntegrationTxnLogs.Add(objLog1);
                entity.SaveChanges();
                #endregion
                return Response;
            }
        }
        internal string ModifyLifeDetailsRiderRefresh(object Sourceobj)
        {
            string plainText = string.Empty;
            string Property = string.Empty;
            string ResponseXml = string.Empty;
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                string seqNo = string.Empty;
                plainText = FetchHeaderData(plainText, "ModifyLifeRequest", ref seqNo);
                IQueryable<MappingData> MappingDatavalues = entity.tblILIntegrations.Where(a => a.CallingFunction == "ModifyLifeRequest").OrderBy(a => a.FeildOrder).Select(a => new MappingData
                {
                    SourceObjectProperty = a.XMLPath,
                    FieldValue = a.FieldValue,
                    IsHardCoded = a.IsHardCode,
                    DataType = a.DataType,
                    ColumnName = a.ColumnName,
                    Occurance = a.Occurance,
                    PropertyLength = a.Length
                });

                object Output = null;
                MemberDetails member = (MemberDetails)Sourceobj;
                int iterationCnt = MappingDatavalues.Where(a => a.Occurance == 1).Count();
                iterationCnt = iterationCnt + MappingDatavalues.Where(a => a.Occurance > 1).Select(a => a.Occurance).Distinct().Count();
                if (member.objBenifitDetails.Where(a => a.RiderCode == "TFCA").FirstOrDefault() != null)
                {
                    int tfcaIndex = member.objBenifitDetails.IndexOf(member.objBenifitDetails.Where(a => a.RiderCode == "TFCA").FirstOrDefault());
                    member.objBenifitDetails.Insert(tfcaIndex + 1, new BenifitDetails() { RiderCode = "TFCR", RiderSuminsured = "100000", BenifitOpted = true });
                }
                List<MappingData> mappedData = MappingDatavalues.ToList();
                for (int i = 0; i < iterationCnt; i++)
                {
                    if (mappedData[i].Occurance == 1)
                    {
                        switch (mappedData[i].ColumnName)
                        {
                            case "BGEN-S5002-CHDRSEL":
                                Output = member.ProposalNo;
                                break;
                            case "BGEN-MOD-HEADER":
                                Output = "N";
                                break;
                            case "BGEN-MOD-LIFE":
                                Output = "Y";
                                break;
                            case "BGEN-MOD-LIFENUM":
                                Output = member.LifeNum;
                                break;
                            case "BGEN-DEL-LIFE":
                                Output = "N";
                                break;
                            case "BGEN-DEL-LIFENUM":
                                Output = "00";
                                break;
                            case "BGEN-DEL-COVER":
                                Output = "N";
                                break;
                            default:
                                Output = mappedData[i].FieldValue;
                                break;
                        }
                        plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, mappedData[i].PropertyLength ?? 0, mappedData[i].DataType);
                    }
                    else if (mappedData[i].Occurance == 2)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 2).ToList())
                            {
                                Output = item.FieldValue;
                                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                                if (j == 0)
                                {
                                    mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                                }
                            }
                        }
                        i = i - 1;
                        iterationCnt = iterationCnt - 1;
                    }
                    else if (mappedData[i].Occurance == 5)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 5).ToList())
                            {
                                Output = item.FieldValue;
                                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                                if (j == 0)
                                {
                                    mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                                }
                            }
                        }
                        i = i - 1;
                        iterationCnt = iterationCnt - 1;
                    }
                    else if (mappedData[i].Occurance == 14)
                    {
                        for (int j = 0; j < 14; j++)
                        {
                            foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 14).ToList())
                            {
                                Output = item.FieldValue;
                                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                                if (j == 0)
                                {
                                    mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                                }
                            }
                        }
                        i = i - 1;
                        iterationCnt = iterationCnt - 1;
                    }
                    else if (mappedData[i].Occurance == 10)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            string riderText = string.Empty;
                            foreach (MappingData Path in MappingDatavalues.Where(a => a.Occurance == 10).ToList())
                            {
                                if (member.objBenifitDetails.Count >= j + 1)
                                {
                                    switch (Path.ColumnName)
                                    {
                                        case "BGEN-CHDRNUM":
                                            Output = member.ProposalNo;
                                            break;
                                        case "BGEN-LIFE":
                                            Output = member.LifeNum;
                                            break;
                                        case "BGEN-COVERAGE":
                                            Output = "01";
                                            break;
                                        case "BGEN-RIDER":
                                            if (member.RelationShipWithPropspect == "267")
                                            {
                                                Output = "0" + (j).ToString();
                                            }
                                            else
                                            {
                                                Output = "0" + (j + 1).ToString();
                                            }

                                            break;
                                        case "BGEN-CRTABLE":
                                            if (member.Age > 46)
                                            {
                                                switch (member.objBenifitDetails[j].RiderCode)//for CIB if >46 CIB with Angioplasty has to be sent
                                                {
                                                    case "TMCI":
                                                        Output = "TMCJ";
                                                        break;
                                                    case "TMCD":
                                                        Output = "TMCE";
                                                        break;
                                                    case "TSCA":
                                                        Output = "TSCB";
                                                        break;
                                                    default:
                                                        Output = member.objBenifitDetails[j].RiderCode;
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                Output = member.objBenifitDetails[j].RiderCode;
                                            }

                                            break;
                                        case "BGEN-MATTRM":
                                            if (member.objBenifitDetails[j].BenifitName == "Basic Life Cover")
                                            {
                                                Output = member.PolicyTerm;
                                            }
                                            else
                                            {
                                                Output = Convert.ToInt32(0);
                                            }

                                            break;
                                        case "BGEN-PCESSTRM":
                                            if (member.objBenifitDetails[j].BenifitName == "Basic Life Cover")
                                            {
                                                Output = member.PremiumTerm;
                                            }
                                            else
                                            {
                                                Output = Convert.ToInt32(0);
                                            }

                                            break;
                                        case "BGEN-SUMIN":
                                            if (member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() != "TPPG")
                                            {
                                                List<string> wop = new List<string> { "TMWG", "TMWH", "TMWI", "TMWK", "TMWL", "TMWM", "TEPB", "TPPH","TMWO" };
                                                if (wop.Contains(member.objBenifitDetails[j].RiderCode))
                                                {
                                                    Output = 0M;
                                                }
                                                else
                                                {
                                                    Output = Convert.ToDecimal(member.objBenifitDetails[j].RiderSuminsured);
                                                }
                                            }
                                            else if (member.objBenifitDetails[j].RiderCode == "TMCI" || member.objBenifitDetails[j].RiderCode == "TMHF")
                                            {
                                                Output = Convert.ToDecimal(member.objBenifitDetails[j].RiderSuminsured);
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-GUARPERD":
                                            if (member.MaturityBenefit == "2381")
                                            {
                                                if (member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TPPG" ||
                                                    member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TPPH")
                                                {
                                                    Output = 10;
                                                }
                                                else
                                                {
                                                    Output = 0;
                                                }
                                            }
                                            else
                                            {
                                                Output = member.PensionTerm;
                                            }

                                            break;
                                        case "BGEN-ZEXTSFLDA"://annual basic premium for smart builder gold
                                            if (member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TSBL"
                                                || member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TSBM")
                                            {
                                                Output = Convert.ToDecimal(member.BasicSumInsured);
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-ZEXTNFLDA"://Sam value for smart builder gold
                                            if (member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TSBL"
                                                || member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TSBM")
                                            {
                                                Output = Convert.ToDecimal(member.SAM);
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-ZEXTCFLDA"://If accident rider is opted for Smart builder gold the send Y
                                            if (member.objBenifitDetails[j].RiderCode == "TSBL" || member.objBenifitDetails[j].RiderCode == "TSBM")
                                            {
                                                if (member.objBenifitDetails.Where(a => a.RiderCode == "TMAO").FirstOrDefault() != null ||
                                                    member.objBenifitDetails.Where(a => a.RiderCode == "TMAO").FirstOrDefault() != null)
                                                {
                                                    Output = "Y";
                                                }
                                                else
                                                {
                                                    Output = "N";
                                                }
                                            }
                                            else
                                            {
                                                Output = "N";
                                            }

                                            break;
                                        case "BGEN-OPCDA":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count >= 1)
                                            {
                                                Output = member.objBenifitDetails[j].BenefitLoadings[0].LoadingBasis;
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-OPPC":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count >= 1 && member.objBenifitDetails[j].BenefitLoadings[0].LoadingType == "2204")
                                            {
                                                Output = Convert.ToDecimal(member.objBenifitDetails[j].BenefitLoadings[0].LoadingPercentage) + 100;
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-INSPRM":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count >= 1 && member.objBenifitDetails[j].BenefitLoadings[0].LoadingType == "2203")
                                            {
                                                Output = Convert.ToInt32(member.objBenifitDetails[j].BenefitLoadings[0].LoadingPercentage) * 1000;
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-OPCDA1":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count >= 2)
                                            {
                                                Output = member.objBenifitDetails[j].BenefitLoadings[1].LoadingBasis;
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-OPPC1":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count >= 2 && member.objBenifitDetails[j].BenefitLoadings[1].LoadingType == "2204")
                                            {
                                                Output = Convert.ToDecimal(member.objBenifitDetails[j].BenefitLoadings[1].LoadingPercentage) + 100;
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-INSPRM1":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count >= 2 && member.objBenifitDetails[j].BenefitLoadings[1].LoadingType == "2203")
                                            {
                                                Output = Convert.ToInt32(member.objBenifitDetails[j].BenefitLoadings[1].LoadingPercentage) * 1000;
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-OPCDA2":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count >= 3)
                                            {
                                                Output = member.objBenifitDetails[j].BenefitLoadings[2].LoadingBasis;
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-OPPC2":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count >= 3 && member.objBenifitDetails[j].BenefitLoadings[2].LoadingType == "2204")
                                            {
                                                Output = Convert.ToDecimal(member.objBenifitDetails[j].BenefitLoadings[2].LoadingPercentage) + 100;
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-INSPRM2":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count >= 3 && member.objBenifitDetails[j].BenefitLoadings[2].LoadingType == "2203")
                                            {
                                                Output = Convert.ToInt32(member.objBenifitDetails[j].BenefitLoadings[2].LoadingPercentage) * 1000;
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-OPCDA3":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count >= 4)
                                            {
                                                Output = member.objBenifitDetails[j].BenefitLoadings[3].LoadingBasis;
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-OPPC3":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count >= 4 && member.objBenifitDetails[j].BenefitLoadings[3].LoadingType == "2204")
                                            {
                                                Output = Convert.ToDecimal(member.objBenifitDetails[j].BenefitLoadings[3].LoadingPercentage) + 100;
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-INSPRM3":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count >= 4 && member.objBenifitDetails[j].BenefitLoadings[3].LoadingType == "2203")
                                            {
                                                Output = Convert.ToInt32(member.objBenifitDetails[j].BenefitLoadings[3].LoadingPercentage) * 1000;
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-OPTEXTIND":
                                            if (member.objBenifitDetails[j].BenefitLoadings.Count > 0)
                                            {
                                                Output = "X";
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-DTHPERLS":
                                            if (member.objBenifitDetails.Where(a => a.RiderCode == "TPPH").FirstOrDefault() != null)
                                            {
                                                Output = Convert.ToDecimal(100 - member.MonthlySavingBenifit);
                                            }
                                            else
                                            {
                                                Output = Path.FieldValue;
                                            }

                                            break;
                                        case "BGEN-DDFDEDUC":
                                            Output = member.Deductible;
                                            break;
                                        default:
                                            Output = Path.FieldValue;
                                            break;
                                    }
                                }
                                else
                                {
                                    Output = Path.FieldValue;
                                }

                                riderText = AppendToPlainText(Output == null ? null : Output.ToString(), riderText, Path.PropertyLength ?? 0, Path.DataType);
                                if (j == 0)
                                {
                                    mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == Path.ColumnName));
                                }
                            }
                            plainText += riderText;
                        }
                        i = i - 1;
                        iterationCnt = iterationCnt - 1;
                    }

                }

                plainText = plainText.Replace('~', ' ');
                plainText.Replace('.', ' ');
                plainText = plainText.ToUpper();

                #region


                Guid guid = Guid.NewGuid();
                byte[] bytes = guid.ToByteArray();
                string refCorrMsgID = Convert.ToBase64String(bytes);

                tblIntegrationTxnLog objLog = new tblIntegrationTxnLog
                {
                    CallingFunction = "ModifyLifeRefreshRequest",
                    PlainText = plainText,
                    CreatedBy = null,
                    CreatedDate = DateTime.Now,
                    PlainTextLength = plainText.Length,
                    MsgID = seqNo,
                    CorelationID = refCorrMsgID
                };
                entity.tblIntegrationTxnLogs.Add(objLog);
                entity.SaveChanges();

                string Result = QueueProcessor.PutQueue(entity, plainText, refCorrMsgID, "ModifyLifeRequest", "ModifyLifeResponse");

                int retryCount = 0;
                string Response = string.Empty;
                do
                {
                    Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["WaitTime1"]));
                    Response = QueueProcessor.GetQueue(entity, System.Text.Encoding.UTF8.GetBytes(refCorrMsgID), "ModifyLifeResponse");
                    if (Response.Contains("MQRC_NO_MSG_AVAILABLE"))
                    {
                        retryCount = retryCount + 1;
                    }
                    else
                    {
                        retryCount = 6;
                    }
                } while (retryCount < 6);//need to configure retry count in web.config

                tblIntegrationTxnLog objLog1 = new tblIntegrationTxnLog
                {
                    CallingFunction = "ModifyLifeRefreshResponse",
                    PlainText = Response,
                    CreatedBy = null,
                    CreatedDate = DateTime.Now,
                    PlainTextLength = Response.Length,
                    MsgID = seqNo,
                    CorelationID = refCorrMsgID
                };
                entity.tblIntegrationTxnLogs.Add(objLog1);
                entity.SaveChanges();
                #endregion
                return Response;
            }
        }
        internal string ModifyLifeRefreshAll(object Sourceobj)
        {
            string plainText = string.Empty;
            string Property = string.Empty;
            string ResponseXml = string.Empty;
            using (AVOAIALifeEntities entity = new AVOAIALifeEntities())
            {
                string seqNo = string.Empty;
                plainText = FetchHeaderData(plainText, "ModifyLifeRequest", ref seqNo);
                IQueryable<MappingData> MappingDatavalues = entity.tblILIntegrations.Where(a => a.CallingFunction == "ModifyLifeRequest").OrderBy(a => a.FeildOrder).Select(a => new MappingData
                {
                    SourceObjectProperty = a.XMLPath,
                    FieldValue = a.FieldValue,
                    IsHardCoded = a.IsHardCode,
                    DataType = a.DataType,
                    ColumnName = a.ColumnName,
                    Occurance = a.Occurance,
                    PropertyLength = a.Length
                });

                object Output = null;
                MemberDetails member = (MemberDetails)Sourceobj;
                int iterationCnt = MappingDatavalues.Where(a => a.Occurance == 1).Count();
                iterationCnt = iterationCnt + MappingDatavalues.Where(a => a.Occurance > 1).Select(a => a.Occurance).Distinct().Count();
                if (member.objBenifitDetails.Where(a => a.RiderCode == "TFCA").FirstOrDefault() != null)
                {
                    int tfcaIndex = member.objBenifitDetails.IndexOf(member.objBenifitDetails.Where(a => a.RiderCode == "TFCA").FirstOrDefault());
                    member.objBenifitDetails.Insert(tfcaIndex + 1, new BenifitDetails() { RiderCode = "TFCR", RiderSuminsured = "100000", BenifitOpted = true });
                }
                List<MappingData> mappedData = MappingDatavalues.ToList();
                for (int i = 0; i < iterationCnt; i++)
                {
                    if (mappedData[i].Occurance == 1)
                    {
                        switch (mappedData[i].ColumnName)
                        {
                            case "BGEN-S5002-CHDRSEL":
                            Output = member.ProposalNo;
                            break;
                            case "BGEN-MOD-HEADER":
                            Output = "N";
                            break;
                            case "BGEN-MOD-LIFE":
                            Output = "Y";
                            break;
                            case "BGEN-MOD-LIFENUM":
                            Output = member.LifeNum;
                            break;
                            case "BGEN-DEL-LIFE":
                            Output = "N";
                            break;
                            case "BGEN-DEL-LIFENUM":
                            Output = "00";
                            break;
                            case "BGEN-DEL-COVER":
                            Output = "N";
                            break;
                            case "BGEN-S5005-ANNE":
                            Output = Convert.ToInt64(member.MonthlyIncome);
                            break;
                            case "BGEN-S5005-HEIGHT":
                            Output = member.objLifeStyleQuetions.Height;
                            break;
                            case "BGEN-S5005-WEIGHT":
                            Output = member.objLifeStyleQuetions.Weight;
                            break;
                            case "BGEN-S6765-ANSWER":
                            Output = member.AnyAdverseRemarks == true ? "Y" : "N";
                            break;
                            case "BGEN-S5005-DOB":
                            Output = Convert.ToDateTime(member.DateOfBirth);
                            break;
                            default:
                            Output = mappedData[i].FieldValue;
                            break;
                        }
                        plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, mappedData[i].PropertyLength ?? 0, mappedData[i].DataType);
                    }
                    else if (mappedData[i].Occurance == 2)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 2).ToList())
                            {
                                Output = item.FieldValue;
                                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                                if (j == 0)
                                {
                                    mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                                }
                            }
                        }
                        i = i - 1;
                        iterationCnt = iterationCnt - 1;
                    }
                    else if (mappedData[i].Occurance == 5)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 5).ToList())
                            {
                                Output = item.FieldValue;
                                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                                if (j == 0)
                                {
                                    mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                                }
                            }
                        }
                        i = i - 1;
                        iterationCnt = iterationCnt - 1;
                    }
                    else if (mappedData[i].Occurance == 14)
                    {
                        for (int j = 0; j < 14; j++)
                        {
                            foreach (MappingData item in MappingDatavalues.Where(a => a.Occurance == 14).ToList())
                            {
                                Output = item.FieldValue;
                                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, item.PropertyLength ?? 0, item.DataType);
                                if (j == 0)
                                {
                                    mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == item.ColumnName));
                                }
                            }
                        }
                        i = i - 1;
                        iterationCnt = iterationCnt - 1;
                    }
                    else if (mappedData[i].Occurance == 10)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            string riderText = string.Empty;
                            foreach (MappingData Path in MappingDatavalues.Where(a => a.Occurance == 10).ToList())
                            {
                                if (member.objBenifitDetails.Count >= j + 1)
                                {
                                    switch (Path.ColumnName)
                                    {
                                        case "BGEN-CHDRNUM":
                                        Output = member.ProposalNo;
                                        break;
                                        case "BGEN-LIFE":
                                        Output = member.LifeNum;
                                        break;
                                        case "BGEN-COVERAGE":
                                        Output = "01";
                                        break;
                                        case "BGEN-RIDER":
                                        if (member.RelationShipWithPropspect == "267")
                                        {
                                            Output = "0" + (j).ToString();
                                        }
                                        else
                                        {
                                            Output = "0" + (j + 1).ToString();
                                        }

                                        break;
                                        case "BGEN-CRTABLE":
                                        if (member.Age > 46)
                                        {
                                            switch (member.objBenifitDetails[j].RiderCode)//for CIB if >46 CIB with Angioplasty has to be sent
                                            {
                                                case "TMCI":
                                                Output = "TMCJ";
                                                break;
                                                case "TMCD":
                                                Output = "TMCE";
                                                break;
                                                case "TSCA":
                                                Output = "TSCB";
                                                break;
                                                default:
                                                Output = member.objBenifitDetails[j].RiderCode;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            Output = member.objBenifitDetails[j].RiderCode;
                                        }

                                        break;
                                        case "BGEN-MATTRM":
                                        if (member.objBenifitDetails[j].BenifitName == "Basic Life Cover")
                                        {
                                            Output = member.PolicyTerm;
                                        }
                                        else
                                        {
                                            Output = Convert.ToInt32(0);
                                        }

                                        break;
                                        case "BGEN-PCESSTRM":
                                        if (member.objBenifitDetails[j].BenifitName == "Basic Life Cover")
                                        {
                                            Output = member.PremiumTerm;
                                        }
                                        else
                                        {
                                            Output = Convert.ToInt32(0);
                                        }

                                        break;
                                        case "BGEN-SUMIN":
                                        if (member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() != "TPPG")
                                        {
                                            List<string> wop = new List<string> { "TMWG", "TMWH", "TMWI", "TMWK", "TMWL", "TMWM", "TEPB", "TPPH" };
                                            if (wop.Contains(member.objBenifitDetails[j].RiderCode))
                                            {
                                                Output = 0M;
                                            }
                                            else
                                            {
                                                Output = Convert.ToDecimal(member.objBenifitDetails[j].RiderSuminsured);
                                            }
                                        }
                                        else if (member.objBenifitDetails[j].RiderCode == "TMCI" || member.objBenifitDetails[j].RiderCode == "TMHF")
                                        {
                                            Output = Convert.ToDecimal(member.objBenifitDetails[j].RiderSuminsured);
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-GUARPERD":
                                        if (member.MaturityBenefit == "2381")
                                        {
                                            if (member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TPPG" ||
                                                member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TPPH")
                                            {
                                                Output = 10;
                                            }
                                            else
                                            {
                                                Output = 0;
                                            }
                                        }
                                        else
                                        {
                                            Output = member.PensionTerm;
                                        }

                                        break;
                                        case "BGEN-ZEXTSFLDA"://annual basic premium for smart builder gold
                                        if (member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TSBE"
                                            || member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TSBF")
                                        {
                                            Output = Convert.ToDecimal(member.BasicSumInsured);
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-ZEXTNFLDA"://Sam value for smart builder gold
                                        if (member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TSBE"
                                            || member.objBenifitDetails.Where(a => a.BenifitName == "Basic Life Cover").Select(a => a.RiderCode).FirstOrDefault() == "TSBF")
                                        {
                                            Output = Convert.ToDecimal(member.SAM);
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-ZEXTCFLDA"://If accident rider is opted for Smart builder gold the send Y
                                        if (member.objBenifitDetails[j].RiderCode == "TSBE" || member.objBenifitDetails[j].RiderCode == "TSBF")
                                        {
                                            if (member.objBenifitDetails.Where(a => a.RiderCode == "TMAH").FirstOrDefault() != null ||
                                                member.objBenifitDetails.Where(a => a.RiderCode == "TMAI").FirstOrDefault() != null)
                                            {
                                                Output = "Y";
                                            }
                                            else
                                            {
                                                Output = "N";
                                            }
                                        }
                                        else
                                        {
                                            Output = "N";
                                        }

                                        break;
                                        case "BGEN-OPCDA":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count >= 1)
                                        {
                                            Output = member.objBenifitDetails[j].BenefitLoadings[0].LoadingBasis;
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-OPPC":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count >= 1 && member.objBenifitDetails[j].BenefitLoadings[0].LoadingType == "2204")
                                        {
                                            Output = Convert.ToDecimal(member.objBenifitDetails[j].BenefitLoadings[0].LoadingPercentage) + 100;
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-INSPRM":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count >= 1 && member.objBenifitDetails[j].BenefitLoadings[0].LoadingType == "2203")
                                        {
                                            Output = Convert.ToInt32(member.objBenifitDetails[j].BenefitLoadings[0].LoadingPercentage) * 1000;
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-OPCDA1":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count >= 2)
                                        {
                                            Output = member.objBenifitDetails[j].BenefitLoadings[1].LoadingBasis;
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-OPPC1":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count >= 2 && member.objBenifitDetails[j].BenefitLoadings[1].LoadingType == "2204")
                                        {
                                            Output = Convert.ToDecimal(member.objBenifitDetails[j].BenefitLoadings[1].LoadingPercentage) + 100;
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-INSPRM1":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count >= 2 && member.objBenifitDetails[j].BenefitLoadings[1].LoadingType == "2203")
                                        {
                                            Output = Convert.ToInt32(member.objBenifitDetails[j].BenefitLoadings[1].LoadingPercentage) * 1000;
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-OPCDA2":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count >= 3)
                                        {
                                            Output = member.objBenifitDetails[j].BenefitLoadings[2].LoadingBasis;
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-OPPC2":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count >= 3 && member.objBenifitDetails[j].BenefitLoadings[2].LoadingType == "2204")
                                        {
                                            Output = Convert.ToDecimal(member.objBenifitDetails[j].BenefitLoadings[2].LoadingPercentage) + 100;
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-INSPRM2":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count >= 3 && member.objBenifitDetails[j].BenefitLoadings[2].LoadingType == "2203")
                                        {
                                            Output = Convert.ToInt32(member.objBenifitDetails[j].BenefitLoadings[2].LoadingPercentage) * 1000;
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-OPCDA3":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count >= 4)
                                        {
                                            Output = member.objBenifitDetails[j].BenefitLoadings[3].LoadingBasis;
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-OPPC3":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count >= 4 && member.objBenifitDetails[j].BenefitLoadings[3].LoadingType == "2204")
                                        {
                                            Output = Convert.ToDecimal(member.objBenifitDetails[j].BenefitLoadings[3].LoadingPercentage) + 100;
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-INSPRM3":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count >= 4 && member.objBenifitDetails[j].BenefitLoadings[3].LoadingType == "2203")
                                        {
                                            Output = Convert.ToInt32(member.objBenifitDetails[j].BenefitLoadings[3].LoadingPercentage) * 1000;
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-OPTEXTIND":
                                        if (member.objBenifitDetails[j].BenefitLoadings.Count > 0)
                                        {
                                            Output = "X";
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-DTHPERLS":
                                        if (member.objBenifitDetails.Where(a => a.RiderCode == "TPPH").FirstOrDefault() != null)
                                        {
                                            Output = Convert.ToDecimal(100 - member.MonthlySavingBenifit);
                                        }
                                        else
                                        {
                                            Output = Path.FieldValue;
                                        }

                                        break;
                                        case "BGEN-DDFDEDUC":
                                        Output = member.Deductible;
                                        break;
                                        default:
                                        Output = Path.FieldValue;
                                        break;
                                    }
                                }
                                else
                                {
                                    Output = Path.FieldValue;
                                }

                                riderText = AppendToPlainText(Output == null ? null : Output.ToString(), riderText, Path.PropertyLength ?? 0, Path.DataType);
                                if (j == 0)
                                {
                                    mappedData.RemoveAt(mappedData.FindIndex(a => a.ColumnName == Path.ColumnName));
                                }
                            }
                            plainText += riderText;
                        }
                        i = i - 1;
                        iterationCnt = iterationCnt - 1;
                    }

                }

                plainText = plainText.Replace('~', ' ');
                plainText.Replace('.', ' ');
                plainText = plainText.ToUpper();

                #region


                Guid guid = Guid.NewGuid();
                byte[] bytes = guid.ToByteArray();
                string refCorrMsgID = Convert.ToBase64String(bytes);

                tblIntegrationTxnLog objLog = new tblIntegrationTxnLog
                {
                    CallingFunction = "ModifyLifeRefreshRequest",
                    PlainText = plainText,
                    CreatedBy = null,
                    CreatedDate = DateTime.Now,
                    PlainTextLength = plainText.Length,
                    MsgID = seqNo,
                    CorelationID = refCorrMsgID
                };
                entity.tblIntegrationTxnLogs.Add(objLog);
                entity.SaveChanges();

                string Result = QueueProcessor.PutQueue(entity, plainText, refCorrMsgID, "ModifyLifeRequest", "ModifyLifeResponse");

                int retryCount = 0;
                string Response = string.Empty;
                do
                {
                    Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["WaitTime1"]));
                    Response = QueueProcessor.GetQueue(entity, System.Text.Encoding.UTF8.GetBytes(refCorrMsgID), "ModifyLifeResponse");
                    if (Response.Contains("MQRC_NO_MSG_AVAILABLE"))
                    {
                        retryCount = retryCount + 1;
                    }
                    else
                    {
                        retryCount = 6;
                    }
                } while (retryCount < 6);//need to configure retry count in web.config

                tblIntegrationTxnLog objLog1 = new tblIntegrationTxnLog
                {
                    CallingFunction = "ModifyLifeRefreshResponse",
                    PlainText = Response,
                    CreatedBy = null,
                    CreatedDate = DateTime.Now,
                    PlainTextLength = Response.Length,
                    MsgID = seqNo,
                    CorelationID = refCorrMsgID
                };
                entity.tblIntegrationTxnLogs.Add(objLog1);
                entity.SaveChanges();
                #endregion
                return Response;
            }
        }
        internal object PreIssueValidationResponse(AVOAIALifeEntities entity, object obj, string respFunc, string responseText)
        {
            PaymentModel paymentModel = (PaymentModel)obj;
            List<MappingData> mappingData = entity.tblILIntegrations.Where(a => a.CallingFunction == respFunc).OrderBy(a => a.FeildOrder).Select(a => new MappingData
            {
                ColumnName = a.ColumnName,
                SourceObjectProperty = a.XMLPath,
                FieldValue = a.FieldValue,
                IsHardCoded = a.IsHardCode,
                DataType = a.DataType,
                PropertyLength = a.Length,
                Occurance = a.Occurance
            }).ToList();
            int msgLength = Convert.ToInt32(entity.tblMasHeaderDatas.Where(a => a.CallingFunction == respFunc && a.ColumnName == "MSGLNG").Select(a => a.FieldValue).FirstOrDefault());
            int totalLength = Convert.ToInt32(entity.tblMasHeaderDatas.Where(a => a.CallingFunction == respFunc && a.ColumnName == "TOTMSGLNG").Select(a => a.FieldValue).FirstOrDefault());
            string msgString = responseText.Substring(totalLength - msgLength, msgLength);
            int dataLength = 0;
            List<MappingData> nonOccuranceData = mappingData.Where(a => a.Occurance == 1).ToList();
            List<MappingData> occuranceData = mappingData.Where(a => a.Occurance > 1).ToList();
            foreach (MappingData row in nonOccuranceData)
            {
                if (msgString.Length >= (dataLength + row.PropertyLength))
                {
                    if (row.ColumnName == "BGEN-S5002-CHDRSEL")
                    {
                        paymentModel.ProposalNo = msgString.Substring(dataLength, row.PropertyLength ?? 0).Trim();
                    }

                    dataLength += row.PropertyLength ?? 0;
                }
            }
            List<string> validations = new List<string>();
            for (int i = 0; i < occuranceData.FirstOrDefault().Occurance; i++)
            {
                foreach (MappingData row in occuranceData)
                {
                    if (msgString.Length >= (dataLength + row.PropertyLength))
                    {
                        if (row.ColumnName == "BGEN-S6378-ERORDSC" && msgString.Substring(dataLength, row.PropertyLength ?? 0).Trim() != "")
                        {
                            validations.Add(msgString.Substring(dataLength, row.PropertyLength ?? 0).Trim());
                        }
                        //row.FieldValue = msgString.Substring(dataLength, row.PropertyLength ?? 0).Trim();
                        dataLength += row.PropertyLength ?? 0;
                    }
                }
            }
            paymentModel.PreIssueValidations = validations;

            return paymentModel;
        }
        internal object FollowUpEnquiryResponse(AVOAIALifeEntities entity, object obj, string respFunc, string responseText)
        {
            Models.Policy.Policy policy = (Models.Policy.Policy)obj;
            List<MappingData> mappingData = entity.tblILIntegrations.Where(a => a.CallingFunction == respFunc).OrderBy(a => a.FeildOrder).Select(a => new MappingData
            {
                ColumnName = a.ColumnName,
                SourceObjectProperty = a.XMLPath,
                FieldValue = a.FieldValue,
                IsHardCoded = a.IsHardCode,
                DataType = a.DataType,
                PropertyLength = a.Length,
                Occurance = a.Occurance
            }).ToList();
            int msgLength = Convert.ToInt32(entity.tblMasHeaderDatas.Where(a => a.CallingFunction == respFunc && a.ColumnName == "MSGLNG").Select(a => a.FieldValue).FirstOrDefault());
            int totalLength = Convert.ToInt32(entity.tblMasHeaderDatas.Where(a => a.CallingFunction == respFunc && a.ColumnName == "TOTMSGLNG").Select(a => a.FieldValue).FirstOrDefault());
            string msgString = responseText.Substring(totalLength - msgLength, msgLength);
            int dataLength = 0;
            List<MappingData> nonOccuranceData = mappingData.Where(a => a.Occurance == 1).ToList();
            List<MappingData> occuranceData = mappingData.Where(a => a.Occurance > 1).ToList();
            foreach (MappingData row in nonOccuranceData)
            {
                if (msgString.Length >= (dataLength + row.PropertyLength))
                {
                    if (row.ColumnName == "BGEN-S5011-CHDRNUM")
                    {
                        policy.ProposalNo = msgString.Substring(dataLength, row.PropertyLength ?? 0).Trim();
                    }

                    dataLength += row.PropertyLength ?? 0;
                }
                msgString = msgString.Substring(row.PropertyLength ?? 0, msgString.Length - row.PropertyLength ?? 0);
            }

            for (int i = 0; i < occuranceData.FirstOrDefault().Occurance; i++)
            {
                int fupdataLength = 0;
                FollowUp followUp = new FollowUp();
                foreach (MappingData row in occuranceData)
                {
                    if (msgString.Length >= (fupdataLength + row.PropertyLength))
                    {
                        row.FieldValue = msgString.Substring(fupdataLength, row.PropertyLength ?? 0).Trim();
                        fupdataLength += row.PropertyLength ?? 0;

                    }
                }


                FollowUp tempFup = (FollowUp)FillObjectPropertyValue(followUp, occuranceData.Where(a => a.SourceObjectProperty != null).AsQueryable());
                if (!string.IsNullOrEmpty(followUp.FupCode))
                {
                    policy.FollowUps.Add(tempFup);
                }

                msgString = msgString.Substring(fupdataLength, msgString.Length - fupdataLength);
            }


            return policy;
        }
        internal string FollowupModifyRequest(object Sourceobj, IQueryable<MappingData> MappingDatavalues)
        {

            Models.Policy.Policy policy = (Models.Policy.Policy)Sourceobj;
            string plainText = string.Empty;

            plainText += AppendObjectPropertyValue(Sourceobj, MappingDatavalues.Where(a => a.Occurance == 1));
            IQueryable<MappingData> fupData = MappingDatavalues.Where(a => a.Occurance > 1);

            for (int i = 0; i < policy.FollowUps.Count(); i++)
            {
                policy.FollowUps[i].FupStatus = "W";
                plainText += AppendObjectPropertyValue(policy.FollowUps[i], fupData);
            }

            plainText += AppendObjectPropertyValue(Sourceobj, MappingDatavalues.Where(a => a.Occurance == 0));
            return plainText;
        }
        internal string WithdrawProposalRequest(object Sourceobj, IQueryable<MappingData> MappingDatavalues)
        {
            object Output = null;
            Models.Policy.Policy policy = (Models.Policy.Policy)Sourceobj;
            string plainText = string.Empty;
            string[] medicalCodes = policy.objMemberDetails[0].ObjUwDecision.UWMedicalCode !=null? policy.objMemberDetails[0].ObjUwDecision.UWMedicalCode.Split(','): new string[0];
            
            foreach (MappingData Path in MappingDatavalues.ToList())
            {
                switch (Path.ColumnName)
                {
                    case "BGEN-S5073-ACTION":
                        if (policy.Decision == CrossCuttingConstants.UWDecisionDecline)//  Decline 
                        {
                            Output = "C";
                        }
                        else if (policy.Decision == CrossCuttingConstants.UWDecisionPostPone)// Postpone
                        {
                            Output = "D";
                        }
                        else if (policy.Decision == CrossCuttingConstants.UWDecisionWithDrawn)//   with Drawn
                        {
                            Output = "B";
                        }
                        else if (policy.Decision == CrossCuttingConstants.UWDecisionNotTaken)//   with Drawn
                        {
                            Output = "E";
                        }
                        break;
                    case "BGEN-S5073-CHDRSEL":
                        Output = policy.ProposalNo;
                        break;
                    case "BGEN-S5075-ALFLAG":
                        if (policy.Decision == CrossCuttingConstants.UWDecisionDecline)
                        {
                            Output = "Y";
                        }
                        else
                        {
                            Output = Path.FieldValue;
                        }

                        break;
                    case "BGEN-S5075-REASONCD":
                        Output = policy.objMemberDetails[0].ObjUwDecision.UWReason;
                        break;
                    case "BGEN-S5075-MLMEDCDE01":
                        Output = medicalCodes.Count() >= 1 ? medicalCodes[0] : null;
                        break;
                    case "BGEN-S5075-MLMEDCDE02":
                        Output = medicalCodes.Count() >= 2 ? medicalCodes[1] : null;
                        break;
                    case "BGEN-S5075-MLMEDCDE03":
                        Output = medicalCodes.Count() >= 3 ? medicalCodes[2] : null;
                        break;
                    case "BGEN-SJ53M-ZRQNTYPE":
                        Output = "1";
                        break;
                    case "BGEN-S5075-POLINC":
                        Output = 1;
                        break;
                    case "BGEN-S5075-MLHSPERD":
                        if (policy.Decision == CrossCuttingConstants.UWDecisionPostPone)
                            Output = Convert.ToInt32(policy.objMemberDetails[0].ObjUwDecision.UWMonth ?? 0) * 30;
                        else
                            Output = Path.FieldValue;
                        break;
                    default:
                        Output = Path.FieldValue;
                        break;
                }
                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, Path.PropertyLength ?? 0, Path.DataType);
            }

            return plainText;
        }

        internal string ClientRelationCreationRequest(object Sourceobj, IQueryable<MappingData> MappingDatavalues)
        {
            object Output = null;
            MemberDetails memberDetails = (MemberDetails)Sourceobj;
            string plainText = string.Empty;
            List<MappingData> nonOccurance = MappingDatavalues.Where(a => a.Occurance == 0).ToList();
            List<MappingData> occurance = MappingDatavalues.Where(a => a.Occurance != 0).ToList();
            foreach (MappingData Path in nonOccurance)
            {
                if (Path.ColumnName == "BGEN-S2418-CRELSEL")
                {
                    Output = memberDetails.ClientCode;
                }
                else
                {
                    Output = Path.FieldValue;
                }

                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, Path.PropertyLength ?? 0, Path.DataType);
            }
            for (int i = 0; i < 30; i++)
            {
                foreach (MappingData Path in occurance)
                {
                    if (memberDetails.ClientRelations.Count() >= i + 1)
                    {
                        switch (Path.ColumnName)
                        {
                            case "BGEN-S2420-CLTRELN":
                                Output = memberDetails.ClientRelations[i].Relation;
                                break;
                            case "BGEN-S2420-PAYEESEL":
                                Output = memberDetails.ClientRelations[i].ClientCode;
                                break;
                        }
                    }
                    else
                    {
                        Output = Path.FieldValue;
                    }
                    plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, Path.PropertyLength ?? 0, Path.DataType);
                }
            }
            return plainText;
        }
        internal object ClientRelationEnquiryResponse(AVOAIALifeEntities entity, object obj, string respFunc, string responseText)
        {
            MemberDetails member = (MemberDetails)obj;
            List<MappingData> mappingData = entity.tblILIntegrations.Where(a => a.CallingFunction == respFunc).OrderBy(a => a.FeildOrder).Select(a => new MappingData
            {
                ColumnName = a.ColumnName,
                SourceObjectProperty = a.XMLPath,
                FieldValue = a.FieldValue,
                IsHardCoded = a.IsHardCode,
                DataType = a.DataType,
                PropertyLength = a.Length,
                Occurance = a.Occurance
            }).ToList();
            int msgLength = Convert.ToInt32(entity.tblMasHeaderDatas.Where(a => a.CallingFunction == respFunc && a.ColumnName == "MSGLNG").Select(a => a.FieldValue).FirstOrDefault());
            int totalLength = Convert.ToInt32(entity.tblMasHeaderDatas.Where(a => a.CallingFunction == respFunc && a.ColumnName == "TOTMSGLNG").Select(a => a.FieldValue).FirstOrDefault());
            string msgString = responseText.Substring(totalLength - msgLength, msgLength);


            for (int i = 0; i < mappingData.FirstOrDefault().Occurance; i++)
            {
                int dataLength = 0;
                ClientRelation clientRelation = new ClientRelation();
                foreach (MappingData row in mappingData)
                {
                    if (msgString.Length >= (dataLength + row.PropertyLength))
                    {
                        switch (row.ColumnName)
                        {
                            case "BGEN-S2420-PAYEESEL":
                                clientRelation.ClientCode = msgString.Substring(dataLength, row.PropertyLength ?? 0).Trim();
                                break;
                            case "BGEN-S2420-CLTRELN":
                                clientRelation.Relation = msgString.Substring(dataLength, row.PropertyLength ?? 0).Trim();
                                break;
                        }
                        dataLength += row.PropertyLength ?? 0;
                    }
                }
                msgString = msgString.Substring(dataLength, msgString.Length - dataLength);
                if (!string.IsNullOrEmpty(clientRelation.ClientCode))
                {
                    member.ClientRelations.Add(clientRelation);
                }
            }


            return member;
        }
        internal string DeleteLife(object Sourceobj, IQueryable<MappingData> MappingDatavalues)
        {
            object Output = null;
            MemberDetails member = (MemberDetails)Sourceobj;
            string plainText = string.Empty;
            foreach (MappingData Path in MappingDatavalues.Where(a => a.Occurance == 1).ToList())
            {
                switch (Path.ColumnName)
                {
                    case "BGEN-S5002-CHDRSEL":
                        Output = member.ProposalNo;
                        break;
                    case "BGEN-DEL-LIFENUM":
                        Output = member.LifeNum;
                        break;
                    default:
                        Output = Path.FieldValue;
                        break;
                }

                plainText = AppendToPlainText(Output == null ? null : Output.ToString(), plainText, Path.PropertyLength ?? 0, Path.DataType);
            }
            List<MappingData> riders = MappingDatavalues.Where(a => a.Occurance == 10).ToList();

            for (int i = 0; i < 10; i++)
            {
                string riderText = string.Empty;
                foreach (MappingData Path in riders)
                {
                    Output = Path.FieldValue;
                    riderText = AppendToPlainText(Output == null ? null : Output.ToString(), riderText, Path.PropertyLength ?? 0, Path.DataType);
                }
                plainText += riderText;
            }
            return plainText;
        }
    }
}
