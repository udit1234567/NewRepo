using AIA.Life.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.Hierarchy;
using AIA.CrossCutting;
using System.Data.Entity.Core.Objects;
using System.Data;

namespace AIA.Data.Life.API.ControllerLogic.Hierarchy
{
    public class HierarchyDataBusiness
    {        
        AVOAIALifeEntities entities = new AVOAIALifeEntities();
        public List<MasterListItem> GetHierarchyTypes()
        {
            List<MasterListItem> objlstTypes = new List<MasterListItem>();
            objlstTypes = (from obj in entities.tblMasCommonTypes
                           where obj.MasterType == "OrganizationType" && obj.IsValid == true
                           select new MasterListItem
                           {
                               ID = obj.CommonTypesID,
                               Value = obj.Description
                           }).ToList();
            //objlstTypes.Add(new MasterListItem { ID = 1, Value = "Corporate" });
            //objlstTypes.Add(new MasterListItem { ID = 2, Value = "Channel" });
            //objlstTypes.Add(new MasterListItem { ID = 3, Value = "Sub Channel" });
            //objlstTypes.Add(new MasterListItem { ID = 4, Value = "Partner" });
            //objlstTypes.Add(new MasterListItem { ID = 5, Value = "Geo Unit" });
            return objlstTypes;

        }
        public List<MasterListItem> GetHierarchyHistoryDetails()
        {
            List<MasterListItem> lstHistroyTypes = new List<MasterListItem>();
            lstHistroyTypes.Add(new MasterListItem { ID = 1, Value = "Reporting History" });
            lstHistroyTypes.Add(new MasterListItem { ID = 2, Value = "Status History" });
            return lstHistroyTypes;

        }
        //public List<MasterListItem> GetGEOUnitsParentEntity()
        //{
        //    //List<MasterListItem> objParentEntityTypes = new List<MasterListItem>();
        //    //objParentEntityTypes.Add(new MasterListItem { ID = 1, Value = "Sub Channel1" });
        //    //objParentEntityTypes.Add(new MasterListItem { ID = 2, Value = "Zone" });
        //    //objParentEntityTypes.Add(new MasterListItem { ID = 3, Value = "Region" });
        //    //objParentEntityTypes.Add(new MasterListItem { ID = 4, Value = "Branch" });            
        //    //return objParentEntityTypes;

        //    List<MasterListItem> objParentEntityTypes = (from cat in entities.tblOrgOffices
        //                                                where cat.Org_CategoryID == 1255 && cat.OfficeCode != null
        //                                                       select new MasterListItem
        //                                                {
        //                                                    Value = cat.OfficeName,
        //                                                    ID = (int)cat.OrgOfficeID

        //                                                       }).ToList();
        //    return objParentEntityTypes;

        //}
        public List<MasterListItem> GetGeoSubChannelParentEntity(string SubChannelCode)
        {
            List<MasterListItem> objParentEntityTypes = new List<MasterListItem>();
            if (SubChannelCode != null || SubChannelCode != "")
            {
                objParentEntityTypes = (from cat in entities.tblOrgOffices
                                        where cat.Org_CategoryID == 1255 && cat.OfficeCode == SubChannelCode
                                        select new MasterListItem
                                        {
                                            Value = cat.OfficeName,
                                            ID = (int)cat.OrgOfficeID

                                        }).ToList();
            }
            return objParentEntityTypes;
        }
        public List<MasterListItem> GetGeoPartnerParentEntity(string PartnerCode)
        {
            List<MasterListItem> objParentEntityTypes = new List<MasterListItem>();
            if (PartnerCode != null || PartnerCode != "")
            {
                objParentEntityTypes = (from cat in entities.tblOrgOffices
                                        where cat.Org_CategoryID == 1256 && cat.OfficeCode == PartnerCode
                                        select new MasterListItem
                                        {
                                            Value = cat.OfficeName,
                                            ID = (int)cat.OrgOfficeID

                                        }).ToList();
            }
            return objParentEntityTypes;

        }
        public List<HierarchyTarget> GetHierarchyTargetData()
        {
            List<HierarchyTarget> lstHierarchyTarget = new List<HierarchyTarget>();
            lstHierarchyTarget.Add(new HierarchyTarget { FinancialYear = "2017 - 2018", Targets = "Rs.1000000", Achievements = "Rs.1000000", LastUpdated = DateTime.Now });
            lstHierarchyTarget.Add(new HierarchyTarget { FinancialYear = "2016 - 2017", Targets = "Rs.900000", Achievements = "Rs.900000", LastUpdated = DateTime.Now });
            return lstHierarchyTarget;
        }
        //public List<Coordination> GetCoordination()
        //{
        //    List<Coordination> objCoordiantes = new List<Coordination>();
        //    objCoordiantes.Add(new Coordination { CoordinateID = 1, Partner = "Commercial Bank", Geounittype = "Branch", Name = "Colombo Metro Branch", Code = "BA91253701" });
        //    objCoordiantes.Add(new Coordination { CoordinateID = 2, Partner = "Commercial Bank", Geounittype = "Branch", Name = "Keselwatta Branch", Code = "BA77887845" });
        //    objCoordiantes.Add(new Coordination { CoordinateID = 3, Partner = "Assetline Leasing Company", Geounittype = "", Name = "Assetline Leasing Company", Code = "LC75847747" });
        //    return objCoordiantes;
        //}
        public List<MasterListItem> GetPartnerTypes()
        {
            List<MasterListItem> objParentEntityTypes = new List<MasterListItem>();
            objParentEntityTypes.Add(new MasterListItem { ID = 1, Value = "Broker" });
            objParentEntityTypes.Add(new MasterListItem { ID = 2, Value = "Bank" });
            objParentEntityTypes.Add(new MasterListItem { ID = 3, Value = "Leasing Company" });
            objParentEntityTypes.Add(new MasterListItem { ID = 4, Value = "Financial Corporation" });
            objParentEntityTypes.Add(new MasterListItem { ID = 5, Value = "Institute" });
            objParentEntityTypes.Add(new MasterListItem { ID = 6, Value = "Introducer" });
            return objParentEntityTypes;
        }
        public List<ManpowerDetails> GetManpowerDetails()
        {
            List<ManpowerDetails> objManpowerDetails = new List<ManpowerDetails>();
            objManpowerDetails.Add(new ManpowerDetails { Position = "ZSM", Designation = "Asst ZSM", NoofEmployees = 2 });
            objManpowerDetails.Add(new ManpowerDetails { Position = "ZSM", Designation = "ZSM", NoofEmployees = 1 });
            objManpowerDetails.Add(new ManpowerDetails { Position = "ZSM", Designation = "Sr.ZSM", NoofEmployees = 1 });
            return objManpowerDetails;
        }
        public List<DocumentDetails> GetDocumentDetails()
        {
            List<DocumentDetails> objDocumentDetails = new List<DocumentDetails>();
            objDocumentDetails.Add(new DocumentDetails { DocumentID = 1, DocumentName = "Memorandum of Understanding" });
            objDocumentDetails.Add(new DocumentDetails { DocumentID = 2, DocumentName = "IBSL License copy" });
            return objDocumentDetails;
        }
        //public List<CoordinationSearchData> GetCoordinationSearchDetails()
        //{
        //    List<CoordinationSearchData> lstSearchdata = new List<CoordinationSearchData>();
        //    lstSearchdata.Add(new CoordinationSearchData { Code = "BA98767676", Geounitname = "Colombo Metro Branch", Geounittype = "Branch", Partner = "Commercial Bank", Subchannel = "Bancassurance", Channel = "Corporates Agents" });
        //    lstSearchdata.Add(new CoordinationSearchData { Code = "BA98465676", Geounitname = "Ibbanwala Branch", Geounittype = "Branch", Partner = "Commercial Bank", Subchannel = "Bancassurance", Channel = "Corporates Agents" });
        //    lstSearchdata.Add(new CoordinationSearchData { Code = "BA97654646", Geounitname = "Keselwatta Branch", Geounittype = "Branch", Partner = "Commercial Bank", Subchannel = "Bancassurance", Channel = "Corporates Agents" });
        //    return lstSearchdata;
        //}
        
        public List<CoordinationSearchData> GetCoordinationSearchDetails(HierarchyData objHierarchyData)
        {
            //var EntityTypeValue = Convert.ToInt32(objHierarchyData.EntityType);
            //var Records = (from obj in entities.tblOrganizations
            //               join obj1 in entities.tblOrgStructures
            //               on obj.OrganizationID equals obj1.OrganizationID
            //               join obj3 in entities.tblOrgOffices
            //               on obj1.OrgStructureID equals obj3.OrgStructureID
            //               where (obj3.OfficeCode == objHierarchyData.CoordinationSearchCode && objHierarchyData.CoordinationSearchCode != null && obj3.Org_CategoryID == EntityTypeValue)
            //              || (obj3.OfficeName == objHierarchyData.CoordinationSearchGeoUnitName && objHierarchyData.CoordinationSearchGeoUnitName != null && obj3.Org_CategoryID == EntityTypeValue)                         
            //               select new CoordinationSearchData
            //               {
            //                   CoordinationSearchID = (int)obj.OrganizationID,
            //                   Partner = obj.PartnerType,
            //                   Geounittype = obj.Org_TypeID.ToString(),
            //                   Geounitname = obj.Org_Name,
            //                   Code = obj.GeoUnitCode
            //                   //IsCoordinationSearch = true 
            //               }).ToList();

            //return Records;

            List<CoordinationSearchData> Records = new List<CoordinationSearchData>();
            if (objHierarchyData.CoordinationSubChannel != null && objHierarchyData.EntityType != null)
            {
                int OrgOfficeID = Convert.ToInt32(objHierarchyData.CoordinationSubChannel);
                var InnerQuery = (from obj in entities.tblOrgOffices
                                  where obj.OrgOfficeID == OrgOfficeID
                                  select obj.OfficeCode).SingleOrDefault();

                var EntityTypeValue = Convert.ToInt32(objHierarchyData.EntityType);               
                Records = (from obj in entities.tblOrgOffices
                           where InnerQuery.Contains(obj.ReportingOfficeCode) && (obj.OfficeName != null || obj.OfficeName != "")
                           select new CoordinationSearchData
                           {
                               CoordinationSearchID = (int)obj.OrgOfficeID,
                               CoordinateID = (int)obj.OrgOfficeID,
                               Partner = obj.ReportingOfficeCode,
                               Geounittype = obj.OfficeCode,                              
                               Geounitname = obj.OfficeName,
                               Code = obj.OfficeCode                              
                           }).ToList();               
            }
            return Records;
        }      
        public List<Coordination> GetCoordinationDetails(HierarchyData objHierarchyData)
        {
            //var EntityTypeValue = Convert.ToInt32(objHierarchyData.EntityType);
            //var Records = (from obj in entities.tblOrganizations
            //               join obj1 in entities.tblOrgStructures
            //               on obj.OrganizationID equals obj1.OrganizationID
            //               join obj3 in entities.tblOrgOffices
            //               on obj1.OrgStructureID equals obj3.OrgStructureID
            //               where (obj3.OfficeCode == objHierarchyData.CoordinationSearchCode && objHierarchyData.CoordinationSearchCode != null && obj3.Org_CategoryID == EntityTypeValue)
            //              || (obj3.OfficeName == objHierarchyData.CoordinationSearchGeoUnitName && objHierarchyData.CoordinationSearchGeoUnitName != null && obj3.Org_CategoryID == EntityTypeValue)
            //               select new Coordination
            //               {
            //                   CoordinationSearchID = (int)obj.OrganizationID,
            //                   Partner = obj.PartnerType,
            //                   Geounittype = obj.Org_TypeID.ToString(),
            //                   Geounitname = obj.Org_Name,
            //                   Code = obj.GeoUnitCode
            //                   //IsCoordinationSearch = true
            //               }).ToList();

            //return Records;
            List<Coordination> Records = new List<Coordination>();
            if (objHierarchyData.CoordinationSubChannel != null && objHierarchyData.EntityType != null)
            {
                int OrgOfficeID = Convert.ToInt32(objHierarchyData.CoordinationSubChannel);
                var InnerQuery = (from obj in entities.tblOrgOffices
                                  where obj.OrgOfficeID == OrgOfficeID
                                  select obj.OfficeCode).SingleOrDefault();

                var EntityTypeValue = Convert.ToInt32(objHierarchyData.EntityType);
                Records = (from obj in entities.tblOrgOffices
                           where InnerQuery.Contains(obj.ReportingOfficeCode) && (obj.OfficeName != null || obj.OfficeName != "")
                           select new Coordination
                           {
                               CoordinationSearchID = (int)obj.OrgOfficeID,
                               CoordinateID = (int)obj.OrgOfficeID,
                               Partner = obj.ReportingOfficeCode,
                               Geounittype = obj.OfficeCode,
                               Geounitname = obj.OfficeName,
                               Code = obj.OfficeCode
                           }).ToList();
            }
            return Records;
        }

        public IEnumerable<MasterListItem> GetHierarchySalutation()
        {
            IEnumerable<MasterListItem> lstSalutation = from cat in entities.tblMasCommonTypesJS
                                                        where cat.MasterType == "Salutation"

                                                        select new MasterListItem
                                                        {
                                                            Value = cat.Description,
                                                            ID = cat.CommonTypesID

                                                        };
            return lstSalutation;
        }
        public IEnumerable<MasterListItem> GetHierarchyPosition()
        {
            IEnumerable<MasterListItem> lstSelectPosition = from cat in entities.tblMasCommonTypesJS
                                                            where cat.MasterType == "Position" && cat.CommonTypesID != 51

                                                            select new MasterListItem
                                                            {
                                                                Value = cat.Description,
                                                                ID = cat.CommonTypesID

                                                            };
            return lstSelectPosition;
        }
        public List<AIA.Life.Models.Hierarchy.TreeView> MenuPermissionTree(string AppName, Guid? UserId, string ItemType)
        {

            List<AIA.Life.Models.Hierarchy.TreeView> permissionDatas = new List<AIA.Life.Models.Hierarchy.TreeView>();
            Guid appID;
            appID = entities.Users.Where(a => a.UserId == UserId).FirstOrDefault().ApplicationId;

            if (appID != null)
                AppName = entities.Applications.Where(a => a.ApplicationId == appID).FirstOrDefault().ApplicationName;

            //   Guid appID = (from obj in _iRepo.Select<Application>() where obj.ApplicationName == AppName select obj.ApplicationId).FirstOrDefault();
            var MenuPerm = entities.USP_GetMenuPermissions(AppName, "", ItemType).ToList();

            var usert = (from objtblUserDetails in entities.tblUserDetails
                         where objtblUserDetails.UserID == UserId
                         select objtblUserDetails).FirstOrDefault();
            var checklevel = usert.userlevel == "L0" ? 1 : 0;
            while (usert.userlevel != "L0")
            {
                usert = entities.tblUserDetails.Where(a => a.NodeID == usert.UserParentId).FirstOrDefault();
            }
            List<int> ParentPermissions = new List<int>();
            ParentPermissions = (from obj in entities.tblUserPermissions
                                 join masPerm in entities.tblMasPermissions on obj.PermissionId equals masPerm.PermissionID
                                 where obj.UserID == usert.UserID && masPerm.ItemType == ItemType
                                 select obj.PermissionId ?? 0).ToList();
            var parentIndentPerm = (from obj in entities.tblUserPermissions
                                    join masPerm in entities.tblMasPermissions on obj.PermissionId equals masPerm.PermissionID
                                    where obj.UserID == usert.UserID && masPerm.ItemType == ItemType && obj.IsIndeterminate == true
                                    select obj.PermissionId ?? 0).ToList();
            List<int> selectPermissions = new List<int>();
            selectPermissions = (from obj in entities.tblUserPermissions
                                 join masPerm in entities.tblMasPermissions on obj.PermissionId equals masPerm.PermissionID
                                 where obj.UserID == UserId && masPerm.ItemType == ItemType && obj.IsIndeterminate == false
                                 select obj.PermissionId ?? 0).ToList();
            List<int> IndetPerm = new List<int>();
            IndetPerm = (from obj in entities.tblUserPermissions
                         join masPerm in entities.tblMasPermissions on obj.PermissionId equals masPerm.PermissionID
                         where obj.UserID == UserId && masPerm.ItemType == ItemType && obj.IsIndeterminate == true
                         select obj.PermissionId ?? 0).ToList();
            if (checklevel != 1)
            {

                var parentPerm = (from obj in MenuPerm
                                  join obj1 in ParentPermissions on obj.id equals obj1
                                  orderby obj.ItemID
                                  select new AIA.Life.Models.Hierarchy.TreeView()
                                  {
                                      ItemId = obj.id ?? 0,
                                      ItemDesc = obj.text1,
                                      Parent = obj.ParentId ?? 0,
                                      depth = obj.Level ?? 0,
                                      IsSelected = selectPermissions.Count == 0 ? true : selectPermissions.Contains(obj.id.Value) ? true : false,
                                      IsIndet = IndetPerm.Count == 0 ? false : IndetPerm.Contains(obj.id.Value) ? true : false
                                  }).ToList();
                permissionDatas = parentPerm.Select(x => new AIA.Life.Models.Hierarchy.TreeView()
                {
                    ItemId = x.ItemId,
                    ItemDesc = x.ItemDesc,
                    Parent = x.Parent,
                    depth = x.depth,
                    IsSelected = x.IsSelected,
                    IsIndet = x.IsIndet
                }).OrderBy(o => o.ItemId).ToList();
            }
            else
            {
                permissionDatas = MenuPerm.Select(x => new AIA.Life.Models.Hierarchy.TreeView()
                {
                    ItemId = x.id ?? 0,
                    ItemDesc = x.text1,
                    Parent = x.ParentId ?? 0,
                    depth = x.Level ?? 0,
                    IsSelected = selectPermissions.Count == 0 ? true : selectPermissions.Contains(x.id.Value) ? true : false,
                    IsIndet = IndetPerm.Count == 0 ? false : IndetPerm.Contains(x.id.Value) ? true : false
                }).OrderBy(o => o.ItemId).ToList();
            }
            return permissionDatas;
        }
        public List<AIA.Life.Models.Hierarchy.TreeView> ProductPermissionTree(string AppName, Guid? UserId, string ItemType)
        {

            List<AIA.Life.Models.Hierarchy.TreeView> permissionDatas = new List<AIA.Life.Models.Hierarchy.TreeView>();


            var MenuPermissionId = (from masPermission in entities.tblMasPermissions
                                    join userPermissionObj in entities.tblUserPermissions
on masPermission.PermissionID equals userPermissionObj.PermissionId
                                    where userPermissionObj.UserID == UserId && masPermission.ItemType == "Menu"
                                    select masPermission.PermissionID).Distinct().ToList();
            if (MenuPermissionId.Count == 0) return new List<AIA.Life.Models.Hierarchy.TreeView>();

            List<int> selectedPermissions = new List<int>();
            selectedPermissions = (from obj in entities.tblUserPermissions
                                   where obj.UserID == UserId && obj.IsIndeterminate == false
                                   && obj.tblMasPermission.ItemType != CrossCuttingConstants.itemTypeMenu && obj.tblMasPermission.ItemType != CrossCuttingConstants.itemTypePayment
                                   select obj.PermissionId ?? 0).ToList();
            List<int> IndetPerm = new List<int>();
            IndetPerm = (from obj in entities.tblUserPermissions
                         where obj.UserID == UserId && obj.IsIndeterminate == true
                         && obj.tblMasPermission.ItemType != CrossCuttingConstants.itemTypeMenu && obj.tblMasPermission.ItemType != CrossCuttingConstants.itemTypePayment
                         select obj.PermissionId ?? 0).ToList();
            var usert = (from objtblUserDetails in entities.tblUserDetails
                         where objtblUserDetails.UserID == UserId
                         select objtblUserDetails).FirstOrDefault();
            var checklevel = usert.userlevel == "L0" ? 1 : 0;
            while (usert.userlevel != "L0")
            {
                usert = entities.tblUserDetails.Where(a => a.NodeID == usert.UserParentId).FirstOrDefault();
            }
            List<int> ParentPermissions = new List<int>();
            ParentPermissions = (from obj in entities.tblUserPermissions
                                 join masPerm in entities.tblMasPermissions on obj.PermissionId equals masPerm.PermissionID
                                 where obj.UserID == usert.UserID && (masPerm.ItemType == "Product" || masPerm.ItemType == "TransactionType")
                                 select obj.PermissionId ?? 0).ToList();

            Guid appID = (from obj in entities.Applications where obj.ApplicationName == AppName select obj.ApplicationId).FirstOrDefault();
            var ProdTxnPerm = entities.USP_GetProductPermissions(appID, UserId, "").ToList();

            List<AIA.Life.Models.Hierarchy.TreeView> prodPermission = new List<AIA.Life.Models.Hierarchy.TreeView>();
            prodPermission = (from obj in entities.tblMasPermissions
                              where obj.ItemType == "Product" && obj.IsDeleted == false
                              && MenuPermissionId.Contains(obj.MenuID ?? 0)
                              select new AIA.Life.Models.Hierarchy.TreeView
                              {
                                  ItemId = obj.PermissionID,
                                  ItemDesc = obj.ItemDescription,
                                  Parent = obj.ParentId.Value,
                                  ItemType = obj.ItemType,
                                  ProductId = obj.ItemID,
                                  IsSelected = selectedPermissions.Count == 0 ? true : selectedPermissions.Contains(obj.PermissionID) ? true : false,
                                  IsIndet = IndetPerm.Count == 0 ? false : IndetPerm.Contains(obj.PermissionID) ? true : false
                              }).Distinct().ToList();




            if (prodPermission.Count != 0)
            {
                permissionDatas = ProdTxnPerm.Select(x => new AIA.Life.Models.Hierarchy.TreeView()
                {
                    ItemId = x.id ?? 0,
                    ItemDesc = x.text1,
                    Parent = x.ParentId ?? 0,
                    depth = x.Level ?? 0,
                    ProductId = x.ItemID,
                    IsSelected = selectedPermissions.Count == 0 ? true : selectedPermissions.Contains(x.id.Value) ? true : false,
                    IsIndet = IndetPerm.Count == 0 ? false : IndetPerm.Contains(x.id.Value) ? true : false,
                    ItemType = x.ItemType
                }).Distinct().ToList();
                if (checklevel == 1)
                {
                    permissionDatas = permissionDatas.Union(prodPermission).OrderBy(o => o.ItemId).OrderBy(o => o.depth).GroupBy(o => o.ItemId).Select(a => a.FirstOrDefault()).ToList();
                }
                else
                {
                    var TempAllPerm = (from objSub in prodPermission
                                       join objParent in ParentPermissions on objSub.ItemId equals objParent
                                       select new AIA.Life.Models.Hierarchy.TreeView
                                       {
                                           ItemId = objSub.ItemId,
                                           ItemDesc = objSub.ItemDesc,
                                           Parent = objSub.Parent,
                                           ItemType = objSub.ItemType,
                                           ProductId = objSub.ProductId,
                                           IsSelected = objSub.IsSelected,
                                           IsIndet = objSub.IsIndet
                                       }).Distinct().ToList();
                    permissionDatas = (from obj in permissionDatas
                                       join obj1 in ParentPermissions on obj.ItemId equals obj1
                                       select new AIA.Life.Models.Hierarchy.TreeView
                                       {
                                           ItemId = obj.ItemId,
                                           ItemDesc = obj.ItemDesc,
                                           Parent = obj.Parent,
                                           depth = obj.depth,
                                           ProductId = obj.ProductId,
                                           IsSelected = obj.IsSelected,
                                           IsIndet = obj.IsIndet,
                                           ItemType = obj.ItemType
                                       }).Distinct().ToList();

                    permissionDatas = permissionDatas.Union(TempAllPerm).OrderBy(o => o.ItemId).OrderBy(o => o.depth).GroupBy(o => o.ItemId).Select(a => a.FirstOrDefault()).ToList();
                }
            }
            return permissionDatas;
        }
        public List<OrgStructureTree> GetHierarchyTree(HierarchyData objHierarchyData)
        {
            int i = 0;
            var OrgStructureId = (from obj in entities.tblOrgOffices
                                  where obj.OfficeCode == objHierarchyData.Code
                                  select obj.OrgStructureID).FirstOrDefault();

            List<tblOrgStructure> listtblMasLifeHierarchyTree = new List<tblOrgStructure>();
            List<OrgStructureTree> listViewHierarchyTree = new List<OrgStructureTree>();
            var MenuPerm = entities.usp_GetHierarchyStructure(OrgStructureId).ToList();
            foreach (var item in MenuPerm)
            {
                if (i == 0)
                {
                    item.ParentId = i;
                }

                i++;
            }
            listViewHierarchyTree = MenuPerm.Select(c => new OrgStructureTree
            {
                ItemId = (int)c.OrgStructureID,
                ItemDescription = c.LevelDefinition,
                ParentId = (int)c.ParentId
            }).OrderBy(x => x.ItemId).ToList();            
            return listViewHierarchyTree;
        }
        public List<MasterListItem> GetStatus()
        {
            List<MasterListItem> Lststatus = new List<MasterListItem>();
            Lststatus.Add(new MasterListItem { ID = 1, Value = "Active" });
            Lststatus.Add(new MasterListItem { ID = 2, Value = "Inactive" });
            return Lststatus;
        }
        public string GetCorporateCode()
        {
            try
            {

                ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
                entities.usp_GetNextHierarchyCorporateCode(nextNo);
                Int64 seqNo = Convert.ToInt64(nextNo.Value);
                string CorpCode = "CORP" + seqNo.ToString().PadLeft(8, '0');
                return CorpCode;
            }
            catch (Exception Ex)
            {
                return "00000";
            }
        }
        public string GetChannelCode()
        {
            try
            {

                ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
                entities.usp_GetNextHierarchyChannelCode(nextNo);
                Int64 seqNo = Convert.ToInt64(nextNo.Value);
                string ChannelCode = "CHAN" + seqNo.ToString().PadLeft(8, '0');
                return ChannelCode;
            }
            catch (Exception Ex)
            {
                return "00000";
            }
        }
        public string GetSubChannelCode()
        {
            try
            {

                ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
                entities.usp_GetNextHierarchySubChannelCode(nextNo);
                Int64 seqNo = Convert.ToInt64(nextNo.Value);
                string SubChannelCode = "SBC" + seqNo.ToString().PadLeft(9, '0');
                return SubChannelCode;
            }
            catch (Exception Ex)
            {
                return "00000";
            }
        }
        //public string GetPartnerCode()
        //{
        //    try
        //    {

        //        ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
        //        entities.usp_GetNextHierarchyPartnerCode(nextNo);
        //        Int64 seqNo = Convert.ToInt64(nextNo.Value);
        //        string PartnerCode = "PAR" + seqNo.ToString().PadLeft(9, '0');
        //        return PartnerCode;
        //    }
        //    catch (Exception Ex)
        //    {
        //        return "00000";
        //    }
        //}
        public string GetPartnerCode(string PartnerTypeId)
        {
            try
            {
                string PartnerCode = "";
                if (PartnerTypeId == "1")
                {
                    ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
                    entities.usp_GetNextHierarchyPartnerCode(nextNo);
                    Int64 seqNo = Convert.ToInt64(nextNo.Value);
                    PartnerCode = "BKL" + seqNo.ToString().PadLeft(9, '0');
                }
                else if (PartnerTypeId == "2")
                {
                    ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
                    entities.usp_GetNextHierarchyPartnerCode(nextNo);
                    Int64 seqNo = Convert.ToInt64(nextNo.Value);
                    PartnerCode = "BAL" + seqNo.ToString().PadLeft(9, '0');
                }
                else if (PartnerTypeId == "3")
                {
                    ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
                    entities.usp_GetNextHierarchyPartnerCode(nextNo);
                    Int64 seqNo = Convert.ToInt64(nextNo.Value);
                    PartnerCode = "LCL" + seqNo.ToString().PadLeft(9, '0');
                }
                else if (PartnerTypeId == "4")
                {
                    ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
                    entities.usp_GetNextHierarchyPartnerCode(nextNo);
                    Int64 seqNo = Convert.ToInt64(nextNo.Value);
                    PartnerCode = "FCL" + seqNo.ToString().PadLeft(9, '0');
                }
                else if (PartnerTypeId == "5")
                {
                    ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
                    entities.usp_GetNextHierarchyPartnerCode(nextNo);
                    Int64 seqNo = Convert.ToInt64(nextNo.Value);
                    PartnerCode = "CPL" + seqNo.ToString().PadLeft(9, '0');
                }
                else if (PartnerTypeId == "6")
                {
                    ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
                    entities.usp_GetNextHierarchyPartnerCode(nextNo);
                    Int64 seqNo = Convert.ToInt64(nextNo.Value);
                    PartnerCode = "CPL" + seqNo.ToString().PadLeft(9, '0');
                }
                return PartnerCode;

            }
            catch (Exception Ex)
            {
                return "00000";
            }
        }
        public string GetGeoUnitCode()
        {
            try
            {

                ObjectParameter nextNo = new ObjectParameter("NextNo", SqlDbType.Int);
                entities.usp_GetNextHierarchyGeoUnitCode(nextNo);
                Int64 seqNo = Convert.ToInt64(nextNo.Value);
                string GeoUnitCode = "GEO" + seqNo.ToString().PadLeft(9, '0');
                return GeoUnitCode;
            }
            catch (Exception Ex)
            {
                return "00000";
            }
        }
        //public HierarchyData FetchReportingdata(HierarchyData objHierarchyData, string EntityID)
        //{
        //    try
        //    {
        //        if (EntityID != null && EntityID != "")
        //        {
        //            int Entity = Convert.ToInt32(EntityID);
        //            //tblOrganization objtblOrganization = (from obj in entities.tblOrganizations
        //            //                                      join obj1 in entities.tblOrgOffices
        //            //                                      on obj.OrganizationID equals obj1.OrganizationID
        //            //                                      where obj.Org_TypeID == Entity
        //            //                                      select obj).OrderByDescending(a => a.OrganizationID).ToList();

        //            tblOrganization objtblOrganization = entities.tblOrganizations.Where(a => a.Org_TypeID == Entity).OrderByDescending(a => a.OrganizationID).First();
        //            if (objtblOrganization != null)
        //            {
        //                objHierarchyData.ReportEntityType = Convert.ToString(objtblOrganization.Org_TypeID);
        //                objHierarchyData.ReportingName = objtblOrganization.Org_Name;
        //                objHierarchyData.Code = objtblOrganization.Code;
        //                var Effective = DateTime.Now;
        //                objHierarchyData.Effectivefrom = Effective.Day + "/" + Effective.Month + "/" + Effective.Year;
        //                if (EntityID == "1256" || EntityID == "1255")
        //                {
        //                    List<MasterListItem> lstGeounitSubchannel = new List<MasterListItem>();
        //                    //lstGeounitSubchannel = (from obj in entities.tblOrganizations
        //                    //                        join obj1 in entities.tblOrgOffices
        //                    //                        on obj.OrganizationID equals obj1.OrganizationID
        //                    //                        where obj.Org_TypeID == 1255 && obj1.OfficeName != null
        //                    //                        select new MasterListItem
        //                    //                        {
        //                    //                            ID = (int)obj.OrganizationID,
        //                    //                            Value = obj1.OfficeName
        //                    //                        }).ToList();
        //                    lstGeounitSubchannel = (from obj in entities.tblOrgOffices
        //                                            where obj.Org_CategoryID == 1255 && obj.OfficeName != null
        //                                            select new MasterListItem
        //                                            {
        //                                                ID = (int)obj.OrgOfficeID,
        //                                                Value = obj.OfficeName
        //                                            }).ToList();

        //                    List<MasterListItem> lstPartnerSubchannel = new List<MasterListItem>();
        //                    lstPartnerSubchannel = (from obj in entities.tblOrgOffices
        //                                            where obj.Org_CategoryID == 1255 && obj.OfficeName != null && obj.IsPartnerHierarchy == true
        //                                            select new MasterListItem
        //                                            {
        //                                                ID = (int)obj.OrgOfficeID,
        //                                                Value = obj.OfficeName
        //                                            }).ToList();

        //                    objHierarchyData.LstGeoUnitsSubChannel = lstGeounitSubchannel;
        //                    objHierarchyData.LstSubChannel = lstPartnerSubchannel;
        //                }
        //            }
        //        }
        //        return objHierarchyData;
        //    }
        //    catch (Exception Ex)
        //    {
        //        return objHierarchyData;
        //    }
        //}
        public HierarchyData FetchReportingdata(HierarchyData objHierarchyData, string EntityID)
        {
            try
            {
                if (EntityID != null && EntityID != "")
                {
                    int Entity = Convert.ToInt32(EntityID);
                    tblOrgOffice objtblOrgOffice = entities.tblOrgOffices.Where(a => a.Org_CategoryID == Entity).FirstOrDefault();
                    if (objtblOrgOffice != null)
                    {
                        objHierarchyData.ReportEntityType = Convert.ToString(objtblOrgOffice.Org_CategoryID);
                        objHierarchyData.ReportingName = objtblOrgOffice.OfficeName;
                        objHierarchyData.Code = objtblOrgOffice.OfficeCode;
                        var Effective = DateTime.Now;
                        objHierarchyData.Effectivefrom = Effective.Day + "/" + Effective.Month + "/" + Effective.Year;
                    }
                    if (EntityID == "1256" || EntityID == "1255")
                    {
                        List<MasterListItem> lstGeounitSubchannel = new List<MasterListItem>();                      
                        lstGeounitSubchannel = (from obj in entities.tblOrgOffices
                                                where obj.Org_CategoryID == 1255 && obj.OfficeName != null && obj.IsPartnerHierarchy == false
                                                select new MasterListItem
                                                {
                                                    ID = (int)obj.OrgOfficeID,
                                                    Value = obj.OfficeName
                                                }).ToList();

                        List<MasterListItem> lstPartnerSubchannel = new List<MasterListItem>();
                        lstPartnerSubchannel = (from obj in entities.tblOrgOffices
                                                where obj.Org_CategoryID == 1255 && obj.OfficeName != null && obj.IsPartnerHierarchy == true
                                                select new MasterListItem
                                                {
                                                    ID = (int)obj.OrgOfficeID,
                                                    Value = obj.OfficeName
                                                }).ToList();

                        objHierarchyData.LstGeoUnitsSubChannel = lstGeounitSubchannel;
                        objHierarchyData.LstSubChannel = lstPartnerSubchannel;
                    }
                }
                return objHierarchyData;
            }
            catch (Exception Ex)
            {
                return objHierarchyData;
            }
        }


        //Here we are saving Hierarchy
        #region SaveHierarchy
        public HierarchyData SaveHierarchy(HierarchyData objHierarchyData)
        {
            try
            {
                tblOrganization objtblOrganization = null;
                tblAddress objtblAddress = new tblAddress();
                if (objHierarchyData.EntityType == "483")
                {
                    //SaveCorporatePartners(objHierarchyData);
                }
                else if (objHierarchyData.EntityType == "1254")
                {
                    SaveChannelSubChannel(objHierarchyData);
                }
                else if (objHierarchyData.EntityType == "1255")
                {
                    SaveChannelSubChannel(objHierarchyData);
                }
                else if (objHierarchyData.EntityType == "1256")
                {
                    SaveCorporatePartners(objHierarchyData);
                    // objtblOrganization = entities.tblOrganizations.Where(a => a.PartnerCode == objHierarchyData.PartnerCode).FirstOrDefault();
                }
                else if (objHierarchyData.EntityType == "1257")
                {
                    SaveCorporatePartners(objHierarchyData);
                    // objtblOrganization = entities.tblOrganizations.Where(a => a.GeoUnitCode == objHierarchyData.GeoUnitCode).FirstOrDefault();
                }

                return objHierarchyData;
            }
            catch (Exception Ex)
            {
                objHierarchyData.Message = Ex.InnerException.Message;
                return objHierarchyData;
            }
        }
        #endregion
        public tblAddress FillAddressDetails(Address objAddress, tblAddress objtblAddress)
        {
            objtblAddress.Address1 = objAddress.Address1;
            objtblAddress.Address2 = objAddress.Address2;
            objtblAddress.Address3 = objAddress.Address3;
            objtblAddress.Address3 = objAddress.Pincode;
            objtblAddress.City = objAddress.City;
            objtblAddress.District = objAddress.District;
            objtblAddress.State = objAddress.State;
            objtblAddress.Country = objAddress.Country;
            return objtblAddress;
        }

        public Address FetchAddressDetails(tblAddress objtblAddress)
        {

            Address objAddress = new Address();
            objAddress.Address1 = objtblAddress.Address1;
            objAddress.Address2 = objtblAddress.Address2;
            objAddress.Address3 = objtblAddress.Address3;
            objAddress.Pincode = objtblAddress.Address3;
            objAddress.City = objtblAddress.City;
            objAddress.District = objtblAddress.District;
            objAddress.State = objtblAddress.State;
            objAddress.Country = objtblAddress.Country;
            return objAddress;
        }
        public tblCustomer FillCustomerInfo(HierarchyData objHierarchyData, tblCustomer objtblcustomer)
        {
            objtblcustomer.CompanyName = objHierarchyData.Name;
            objtblcustomer.EmailID = objHierarchyData.Email;
            objtblcustomer.FullName = objHierarchyData.Name;
            objtblcustomer.HomeNo = Convert.ToString(objHierarchyData.ResidencePhone);
            objtblcustomer.MobileNo = Convert.ToString(objHierarchyData.Mobile1);
            objtblcustomer.NameWithInitials = objHierarchyData.Lastname;
            objtblcustomer.Title = objHierarchyData.Salutation;
            objtblcustomer.WorkNo = Convert.ToString(objHierarchyData.OfficePhone);
            objtblcustomer.CreatedDate = DateTime.Now;
            return objtblcustomer;
        }
        public tblPolicyClient FilltblpolicyClient(HierarchyData objHierarchyData, tblPolicyClient objPolicyClient)
        {
            objPolicyClient.CompanyName = objHierarchyData.Name;
            objPolicyClient.EmailID = objHierarchyData.Email;
            objPolicyClient.FullName = objHierarchyData.Name;
            objPolicyClient.HomeNo = Convert.ToString(objHierarchyData.ResidencePhone);
            objPolicyClient.MobileNo = Convert.ToString(objHierarchyData.Mobile1);
            objPolicyClient.NameWithInitials = objHierarchyData.Lastname;
            objPolicyClient.Title = objHierarchyData.Salutation;
            objPolicyClient.IsPermanentAddrSameasCommAddr = objHierarchyData.IsRegAsCommunication;
            objPolicyClient.WorkNo = Convert.ToString(objHierarchyData.OfficePhone);
            objPolicyClient.CreatedDate = DateTime.Now;
            #region FillAddressDetails
            tblAddress objtbladdress = objPolicyClient.tblAddress1;
            if (objtbladdress == null)
            {
                objtbladdress = new tblAddress();
            }
            objtbladdress = FillAddressDetails(objHierarchyData.objCommunicationAddress, objtbladdress);
            objPolicyClient.tblAddress1 = objtbladdress;
            tblAddress objtblPermanentaddress = objPolicyClient.tblAddress;
            if (objtblPermanentaddress == null)
            {
                objtblPermanentaddress = new tblAddress();
            }
            if (objHierarchyData.objRegistrationAddress != null)
            {
                objtblPermanentaddress = FillAddressDetails(objHierarchyData.objRegistrationAddress, objtblPermanentaddress);
                objPolicyClient.tblAddress = objtblPermanentaddress;
            }
            #endregion
            return objPolicyClient;
        }
        //Here we are saving document details
        public HierarchyData SaveDocumentDetails(HierarchyData objHierarchyData)
        {
            try
            {
                tblOrganization objtblOrganization = null;
                objtblOrganization = entities.tblOrganizations.Where(a => a.PartnerCode == objHierarchyData.PartnerCode).FirstOrDefault();
                if (objtblOrganization == null)
                {
                    objtblOrganization = new tblOrganization();
                }
                objtblOrganization.Org_CategoryID = Convert.ToInt32(objHierarchyData.EntityType);
                objtblOrganization.Org_TypeID = Convert.ToInt32(objHierarchyData.EntityType);
                #region FillingDocumentDetails
                if (objHierarchyData.LstdocumentName != null)
                {
                    var ID = Convert.ToInt32(objtblOrganization.OrganizationID);
                    List<tblProspectDocument> lsttblProspectDocument = objtblOrganization.tblProspectDocuments.ToList();
                    DeleteDocument(ID);
                    foreach (var item in objHierarchyData.LstdocumentName)
                    {
                        List<tblProspectDocument> deleteTraining = objtblOrganization.tblProspectDocuments.ToList();
                        tblProspectDocument objtblProspectDocument = new tblProspectDocument();
                        var ext = item.FileUploadname.Substring(item.FileUploadname.LastIndexOf('.') + 1);
                        objtblProspectDocument.DocID = item.DocumentID;
                        objtblProspectDocument.DocPath = item.FileUploadname.Split('\\')[2];
                        objtblProspectDocument.DocType = ext;
                        if (objtblProspectDocument.ProspectDocID == decimal.Zero)
                        {
                            objtblProspectDocument.tblOrganization = objtblOrganization;
                            entities.tblProspectDocuments.Add(objtblProspectDocument);
                        }
                    }
                }
                #endregion
                var Result = entities.SaveChanges();
                if (Result > 0)
                {
                    objHierarchyData.Message = "Success";
                }
                return objHierarchyData;
            }
            catch (Exception Ex)
            {
                objHierarchyData.Message = Ex.InnerException.Message;
                return objHierarchyData;
            }
        }
        public bool DeleteDocument(int DocumentId)
        {
            List<tblProspectDocument> objPolicyDocument;
            if (DocumentId != 0)
            {
                objPolicyDocument = entities.tblProspectDocuments.Where(o => o.OrganizationID == DocumentId).ToList();
                foreach (var item in objPolicyDocument)
                {
                    entities.tblProspectDocuments.Remove(item);
                }
                entities.SaveChanges();
                return true;
            }
            else
                return false;
        }
        //public HierarchyData FetchPartnerReporting(string SubChannelID)
        //{
        //    HierarchyData objHierarchyData = new HierarchyData();
        //    try
        //    {
        //        if (SubChannelID != null)
        //        {
        //            int Entity = Convert.ToInt32(SubChannelID);
        //            tblOrganization objtblOrganization = entities.tblOrganizations.Where(a => a.OrganizationID == Entity).FirstOrDefault();
        //            if (objtblOrganization != null)
        //            {
        //                objHierarchyData.ReportEntityType = Convert.ToString(objtblOrganization.Org_TypeID);
        //                objHierarchyData.ReportingName = objtblOrganization.Org_Name;
        //                objHierarchyData.Code = objtblOrganization.Code;
        //                var Effective = DateTime.Now;
        //                objHierarchyData.Effectivefrom = Effective.Day + "/" + Effective.Month + "/" + Effective.Year;
        //            }
        //        }
        //        return objHierarchyData;
        //    }
        //    catch (Exception Ex)
        //    {
        //        return objHierarchyData;
        //    }
        //}
        public HierarchyData FetchPartnerReporting(string SubChannelID)
        {
            HierarchyData objHierarchyData = new HierarchyData();
            try
            {
                if (SubChannelID != null)
                {
                    int Entity = Convert.ToInt32(SubChannelID);
                    tblOrgOffice objtblOrgOffice = entities.tblOrgOffices.Where(a => a.OrgOfficeID == Entity).FirstOrDefault();
                    if (objtblOrgOffice != null)
                    {
                        objHierarchyData.ReportEntityType = Convert.ToString(objtblOrgOffice.Org_CategoryID);
                        objHierarchyData.ReportingName = objtblOrgOffice.OfficeName;
                        objHierarchyData.Code = objtblOrgOffice.OfficeCode;
                        var Effective = DateTime.Now;
                        objHierarchyData.Effectivefrom = Effective.Day + "/" + Effective.Month + "/" + Effective.Year;
                    }
                }
                return objHierarchyData;
            }
            catch (Exception Ex)
            {
                return objHierarchyData;
            }
        }
        public List<SearchHierarchy> LoadSearchHierarchyData(HierarchyData objHierarchyData)
        {
            List<SearchHierarchy> objlstNICDetails = new List<SearchHierarchy>();
            if (objHierarchyData.SearchStatus != null)
            {
               
            }       
            string StatusValue = objHierarchyData.SearchStatus;
            bool Status = false;
            if (StatusValue == "1")
            {
                Status = true;
            }
            else if(StatusValue == "2")
            {
                Status = false;
            }
            else
            {
                Status = false;
            }
            List<SearchHierarchy> Records = new List<SearchHierarchy>();
            List<SearchHierarchy> FinalList = new List<SearchHierarchy>();
            var EntityTypeValue = Convert.ToInt32(objHierarchyData.SearchEntityType);
            //var Records = (from obj in entities.tblOrganizations
            //               join obj1 in entities.tblOrgStructures
            //               on obj.OrganizationID equals obj1.OrganizationID
            //               join obj3 in entities.tblOrgOffices
            //               on obj1.OrgStructureID equals obj3.OrgStructureID
            //               //where (obj3.OfficeCode == objHierarchyData.SearchCode && objHierarchyData.SearchCode != null && objHierarchyData.SearchCode != "null")
            //               where (obj3.OfficeCode.Contains(objHierarchyData.SearchCode) && objHierarchyData.SearchCode != null && objHierarchyData.SearchCode != "null")
            //              || (obj3.OfficeName.Contains(objHierarchyData.SearchName) && objHierarchyData.SearchName != null && objHierarchyData.SearchName != "null")
            //              //|| (obj3.IsActive == (string) objHierarchyData.SearchStatus && objHierarchyData.AgentCode != null)
            //              || (obj3.IsActive == Status && objHierarchyData.SearchStatus != null)
            //              //|| (obj3.Org_CategoryID == EntityTypeValue && objHierarchyData.SearchEntityType != null)
            //              || (obj3.Org_CategoryID == EntityTypeValue && EntityTypeValue != 0)
            //               select new SearchHierarchy
            //               {
            //                   Code = obj3.OfficeCode,
            //                   Name = obj3.OfficeName,
            //                   EntityType = obj3.Org_CategoryID.ToString(),                        
            //                   ParentType = obj3.ReportingOfficeCode,
            //                   Channel = obj3.ReportingOfficeCode,
            //                   SubChannel = obj3.ReportingOfficeCode,
            //                   Partner = obj3.ReportingOfficeCode,
            //                   LastModifiedDate = obj3.CreatedDate.Value.Day + "/" + obj3.CreatedDate.Value.Month + "/" + obj3.CreatedDate.Value.Year + " " + obj3.CreatedDate.Value.Hour + ":" + obj3.CreatedDate.Value.Minute,
            //                   IsActive = (bool)obj3.IsActive,
            //                   User = obj3.CreatedBy

            //               }).OrderByDescending(a => a.Code).ToList();
            Records = (from obj in entities.tblOrgOffices                         
                           where (obj.OfficeCode.Contains(objHierarchyData.SearchCode) && objHierarchyData.SearchCode != null && objHierarchyData.SearchCode != "null")
                          || (obj.OfficeName.Contains(objHierarchyData.SearchName) && objHierarchyData.SearchName != null && objHierarchyData.SearchName != "null")                          
                          || (obj.IsActive == Status && objHierarchyData.SearchStatus != null)                          
                          || (obj.Org_CategoryID == EntityTypeValue && EntityTypeValue != 0)
                           select new SearchHierarchy
                           {
                               OfficeID = obj.OrgOfficeID.ToString(),
                               Code = obj.OfficeCode,
                               Name = obj.OfficeName,
                               EntityType = obj.Org_CategoryID.ToString(),
                               ParentType = obj.ReportingOfficeCode,
                               Channel = "",
                               SubChannel = "",
                               Partner = obj.ReportingOfficeCode,
                               //Partner = "",
                               LastModifiedDate = obj.CreatedDate.Value.Day + "/" + obj.CreatedDate.Value.Month + "/" + obj.CreatedDate.Value.Year + " " + obj.CreatedDate.Value.Hour + ":" + obj.CreatedDate.Value.Minute,
                               IsActive = (bool)obj.IsActive,
                               User = obj.CreatedBy,
                               IsPartner = (bool)(obj.IsPartnerHierarchy != null ? obj.IsPartnerHierarchy: false)

                           }).OrderByDescending(a => a.Code).ToList();           
            foreach (var item in Records)
            {
                if (item.IsActive == true)
                    item.Status = "Active";
                else
                    item.Status = "InActive";
                if (item.EntityType == "483" || item.EntityType == "1254" || item.EntityType == "1255")
                {
                    item.Channel = "";
                    item.SubChannel = "";
                    item.Partner = "";
                }
                else if (item.EntityType == "1256")
                {
                    item.Partner = "";
                    item.SubChannel = GetReportingCode(item.Code);
                    item.Channel = GetReportingCode(item.SubChannel);
                }
                else
                {                  
                    //if(item.IsPartner == true)
                    //{
                        //item.Partner = GetReportingCode(item.Code);
                        item.SubChannel = GetReportingCode(item.Partner);
                        item.Channel = GetReportingCode(item.SubChannel);
                    //}
                    //else
                    //{
                       // item.SubChannel = GetReportingCode(item.Code);
                        //item.Channel = GetReportingCode(item.SubChannel);
                        //item.Partner = "";
                    //}
                                    
                }
                item.EntityType = GetEntityName(item.EntityType);
                FinalList.AddRange(Records);
            }
            //foreach (var item in Records)
            //{
               
            //}
           
            return FinalList;
        }
        public string GetEntityName(string EntityTypeID)
        {
            string EntityName = "";
            var EntityTypeValue = Convert.ToInt32(EntityTypeID);
            if (EntityTypeID != "" && EntityTypeID != null)
            {
                EntityName = (from obj in entities.tblMasCommonTypes
                              where obj.CommonTypesID == EntityTypeValue
                              select obj.Description).FirstOrDefault();
            }
            return EntityName;
        }
        public string GetReportingCode(string OfficeCode)
        {
            string record = "";
            if (OfficeCode != "" && OfficeCode != null)
            {
                record = (from obj in entities.tblOrgOffices
                          where obj.OfficeCode == OfficeCode
                          select obj.ReportingOfficeCode).FirstOrDefault();
            }
            return record;
        }
        public List<HeirarchyHistroyDetails> LoadHierarchyHistoryData(HierarchyData objHierarchyData)
        {            
            List<HeirarchyHistroyDetails> objHeirarchyHistroyDetails = new List<HeirarchyHistroyDetails>();
            if (objHierarchyData.SearchCodeGrid != null)
            {
                
            }           
            var Records = (from obj in entities.tblOrgOffices
                           where (obj.OfficeCode == objHierarchyData.SearchCodeGrid && objHierarchyData.SearchCodeGrid != null)
                           select new HeirarchyHistroyDetails
                           {
                               Code = obj.OfficeCode,                              
                               EntityType = obj.Org_CategoryID.ToString(),
                               StartDate = obj.CreatedDate.Value.Day + "/" + obj.CreatedDate.Value.Month + "/" + obj.CreatedDate.Value.Year,
                               EndDate = obj.CreatedDate.Value.Day + "/" + obj.CreatedDate.Value.Month + "/" + obj.CreatedDate.Value.Year,                             
                               //Repository = GetReportingName(obj.ReportingOfficeCode, obj.Org_CategoryID.ToString()),
                               Repository = obj.ReportingOfficeCode,
                               DateMode = obj.CreatedDate.Value.Day + "/" + obj.CreatedDate.Value.Month + "/" + obj.CreatedDate.Value.Year,
                               User = obj.CreatedBy
                           }).OrderByDescending(a => a.Code).ToList();
            foreach (var item in Records)
            {
                item.EntityType = GetEntityName(item.EntityType);
            }
            return Records;
        }

        public List<HierachyStatusDetails> LoadHierarchyStatusData(HierarchyData objHierarchyData)
        {
            List<HierachyStatusDetails> objlstNICDetails = new List<HierachyStatusDetails>();
            if (objHierarchyData.SearchCodeGrid != null)
            {

            }
            //var StatusValue = Convert.ToBoolean(Convert.ToInt32(SearchCode));
            //var EntityTypeValue = Convert.ToInt32(SearchEntityType);
            var Records = (from obj in entities.tblOrgOffices
                           where (obj.OfficeCode == objHierarchyData.SearchCodeGrid && objHierarchyData.SearchCodeGrid != null)
                           select new HierachyStatusDetails
                           {
                               Code = obj.OfficeCode,
                               EntityType = obj.Org_CategoryID.ToString(),
                               StartDate = obj.CreatedDate.Value.Day + "/" + obj.CreatedDate.Value.Month + "/" + obj.CreatedDate.Value.Year,
                               EndDate = obj.CreatedDate.Value.Day + "/" + obj.CreatedDate.Value.Month + "/" + obj.CreatedDate.Value.Year,
                               Status = obj.IsActive.ToString(),
                               DateMode= obj.CreatedDate.Value.Day + "/" + obj.CreatedDate.Value.Month + "/" + obj.CreatedDate.Value.Year,
                               User = obj.CreatedBy
                           }).OrderByDescending(a => a.Code).ToList();
            foreach (var item in Records)
            {
                if (item.Status == "True")
                    item.Status = "Active";
                else
                    item.Status = "InActive";

                item.EntityType = GetEntityName(item.EntityType);
            }
            //foreach (var item in Records)
            //{
            //    item.EntityType = GetEntityName(item.EntityType);
            //}
            return Records;
        }
        public HierarchyData SaveCorporatePartners(HierarchyData objHierarchyData)
        {
            try
            {
                tblOrganization objtblOrganization = null;
                tblAddress objtblAddress = new tblAddress();
                tblContactDetail objtblContactDetail = null;
                tblOrgStructure objtblOrgStructure = null;
                tblOrgOffice objtblOrgOffice = null;
                tblHierarchyCoordination objtblHierarchyCoordination = null; 
                tblMasCommonType objtblMasCommonType = new tblMasCommonType();
                if (objHierarchyData.EntityType == "483")
                {
                    objtblOrganization = entities.tblOrganizations.Where(a => a.CorporateCode == objHierarchyData.CorporateCode).FirstOrDefault();
                }
                else if (objHierarchyData.EntityType == "1256")
                {
                    //objtblOrganization = entities.tblOrganizations.Where(a => a.PartnerCode == objHierarchyData.PartnerCode).FirstOrDefault();
                    //objtblOrgOffice = entities.tblOrgOffices.Where(a => a.OfficeCode == objHierarchyData.PartnerCode).FirstOrDefault();
                    objtblOrganization = entities.tblOrganizations.Where(a => a.PartnerCode == objHierarchyData.PartnerCode).FirstOrDefault();
                    objtblOrgOffice = entities.tblOrgOffices.Where(a => a.OfficeCode == objHierarchyData.PartnerCode).FirstOrDefault();
                }
                else if (objHierarchyData.EntityType == "1257")
                {
                    //objtblOrganization = entities.tblOrganizations.Where(a => a.GeoUnitCode == objHierarchyData.GeoUnitCode).FirstOrDefault();
                    //objtblOrganization = entities.tblOrganizations.Where(a => a.GeoUnitCode == objHierarchyData.GeoUnitCode).FirstOrDefault();
                    objtblOrgOffice = entities.tblOrgOffices.Where(a => a.OfficeCode == objHierarchyData.GeoUnitCode).FirstOrDefault();
                }
                if (objtblOrganization == null)
                {
                    objtblOrganization = new tblOrganization();
                }
                objtblOrganization.Org_Name = objHierarchyData.Name;
                objtblOrganization.YearOfEstablishment = objHierarchyData.YearofEstablish;
                objtblOrganization.RegistrationNo = objHierarchyData.RegNo;
                objtblOrganization.Reg_no_st = objHierarchyData.Status;
                objtblOrganization.Code = objHierarchyData.Code;
                objtblOrganization.MobileNo = objHierarchyData.Mobile1;
                objtblOrganization.OfficePhone1 = objHierarchyData.OfficePhone1;
                objtblOrganization.OfficePhone2 = objHierarchyData.OfficePhone2;
                objtblOrganization.Email = objHierarchyData.Email;
                objtblOrganization.FaxNo = objHierarchyData.Fax;
                objtblOrganization.CorporateCode = objHierarchyData.CorporateCode;
                objtblOrganization.ChannelCode = objHierarchyData.ChannelCode;
                objtblOrganization.SubChannelCode = objHierarchyData.SubChannelCode;
                //objtblOrganization.PartnerCode = objHierarchyData.PartnerCode;
                //objtblOrganization.GeoUnitCode = objHierarchyData.GeoUnitCode;
                if (objHierarchyData.EntityType == "1256")
                {
                    objtblOrganization.PartnerCode = objHierarchyData.PartnerCode;
                    //objtblOrganization.PartnerCode = objHierarchyData.Code;
                }                   
                if (objHierarchyData.EntityType == "1257")
                {
                    objtblOrganization.GeoUnitCode = objHierarchyData.GeoUnitCode;
                    //objtblOrganization.GeoUnitCode = objHierarchyData.Code;
                }                    
                objtblOrganization.Org_CategoryID = Convert.ToInt32(objHierarchyData.EntityType);
                objtblOrganization.Org_TypeID = Convert.ToInt32(objHierarchyData.EntityType);
                objtblOrganization.LicenseNo = objHierarchyData.LicenseNo;
                objtblOrganization.IssueDate = objHierarchyData.IssueDate;
                objtblOrganization.ExpiryDate = objHierarchyData.ExperiyDate;
                objtblOrganization.ContractEffectiveFrom = objHierarchyData.ContractEffetiveFrom;
                objtblOrganization.ContractEffectiveTo = objHierarchyData.ContractEffetiveTo;
                objtblOrganization.EffectiveFrom = Convert.ToDateTime(objHierarchyData.Effectivefrom);
                objtblOrganization.PartnerType = objHierarchyData.PartnerType;

                //#region tblAddress
                //tblPolicyRelationship objtblpolicyrelationship = objtblOrganization.tblPolicyRelationships.FirstOrDefault();
                //if (objtblpolicyrelationship == null)
                //{
                //    objtblpolicyrelationship = new tblPolicyRelationship();
                //}

                //tblPolicyClient objtblPolicyClient = objtblpolicyrelationship.tblPolicyClient;
                //if (objtblPolicyClient == null)
                //{
                //    objtblPolicyClient = new tblPolicyClient();
                //}
                //objtblPolicyClient = FilltblpolicyClient(objHierarchyData, objtblPolicyClient);
                //objtblPolicyClient.tblAddress1 = objtblPolicyClient.tblAddress;
                //objtblpolicyrelationship.tblPolicyClient = objtblPolicyClient;
                //objtblpolicyrelationship.tblOrganization = objtblOrganization;
                //if (objtblpolicyrelationship.PolicyRelationshipID == decimal.Zero)
                //{
                //    entities.tblPolicyRelationships.Add(objtblpolicyrelationship);
                //}
                //#endregion
                #region Fill Communication Address
                if (objHierarchyData.objCommunicationAddress.Address1 != null)
                {
                    objtblAddress = entities.tblAddresses.Where(a => a.AddressID == objHierarchyData.CommunicationAddressId).FirstOrDefault();
                    if (objtblAddress == null)
                    {
                        objtblAddress = new tblAddress();
                    }
                    objtblAddress.Address1 = objHierarchyData.objCommunicationAddress.Address1;
                    objtblAddress.Address2 = objHierarchyData.objCommunicationAddress.Address2;
                    objtblAddress.Address3 = objHierarchyData.objCommunicationAddress.Address3;
                    objtblAddress.State = objHierarchyData.objCommunicationAddress.State;
                    objtblAddress.District = objHierarchyData.objCommunicationAddress.District;
                    objtblAddress.City = objHierarchyData.objCommunicationAddress.City;
                    objtblAddress.Pincode = objHierarchyData.objCommunicationAddress.Pincode;
                    objtblAddress.CreatedBy = objHierarchyData.UserName;
                    objtblAddress.CreatedDate = DateTime.Now;
                    objtblAddress.Status = true;
                    if (objtblAddress.AddressID == decimal.Zero)
                    {
                        entities.tblAddresses.Add(objtblAddress);
                    }
                    var ResultCommAddress = entities.SaveChanges();
                    if (ResultCommAddress > 0)
                    {
                        objHierarchyData.CommunicationAddressId = Convert.ToInt32(objtblAddress.AddressID);
                    }

                }
                #endregion
                #region Fill Registration Address
                if (objHierarchyData.IsRegAsCommunication == false)
                {
                    if (objHierarchyData.objRegistrationAddress.Address1 != null)
                    {
                        objtblAddress = entities.tblAddresses.Where(a => a.AddressID == objHierarchyData.RegistrationAddressId).FirstOrDefault();
                        if (objtblAddress == null)
                        {
                            objtblAddress = new tblAddress();
                        }
                        objtblAddress.Address1 = objHierarchyData.objRegistrationAddress.Address1;
                        objtblAddress.Address2 = objHierarchyData.objRegistrationAddress.Address2;
                        objtblAddress.Address3 = objHierarchyData.objRegistrationAddress.Address3;
                        objtblAddress.State = objHierarchyData.objRegistrationAddress.State;
                        objtblAddress.District = objHierarchyData.objRegistrationAddress.District;
                        objtblAddress.City = objHierarchyData.objRegistrationAddress.City;
                        objtblAddress.Pincode = objHierarchyData.objRegistrationAddress.Pincode;
                        objtblAddress.CreatedBy = objHierarchyData.UserName;
                        objtblAddress.CreatedDate = DateTime.Now;
                        objtblAddress.Status = true;
                        if (objtblAddress.AddressID == decimal.Zero)
                        {
                            entities.tblAddresses.Add(objtblAddress);
                        }
                        var ResultRegAddress = entities.SaveChanges();
                        if (ResultRegAddress > 0)
                        {
                            objHierarchyData.RegistrationAddressId = Convert.ToInt32(objtblAddress.AddressID);
                        }
                    }
                }
                #endregion
                #region tblContactDetails
                if (objHierarchyData.LstPointOfContacts != null)
                {
                    foreach (var item in objHierarchyData.LstPointOfContacts)
                    {
                        List<tblContactDetail> deleteacgievements = objtblOrganization.tblContactDetails.ToList();
                        if (deleteacgievements != null)
                        {
                            foreach (var items in deleteacgievements)
                            {
                                var deleteobj = entities.tblContactDetails.Where(a => a.OfficeID == items.OfficeID).ToList();
                                foreach (tblContactDetail delobj in deleteobj)
                                {
                                    entities.tblContactDetails.Remove(delobj);
                                }
                            }
                            entities.SaveChanges();
                        }
                        objtblContactDetail = new tblContactDetail();
                        objtblContactDetail.Email = item.Email;
                        objtblContactDetail.FirstName = item.FirstName;
                        objtblContactDetail.LastName = item.LastName;
                        objtblContactDetail.MiddleName = item.MiddleName;
                        objtblContactDetail.MobileNo1 = item.Mobile1;
                        objtblContactDetail.MobileNo2 = item.Mobile2;
                        objtblContactDetail.SLIIRegNo = item.SLIIRegNo;
                        objtblContactDetail.OfficePhone1 = item.OfficePhone;
                        objtblContactDetail.ResidenceNo = item.ResidencePhone;
                        objtblContactDetail.Position = item.Position;
                        objtblContactDetail.Salutation = item.Salutation;
                        objtblContactDetail.OfficeID = item.Index;
                        if (objtblContactDetail.ContactID == decimal.Zero)
                        {
                            objtblContactDetail.tblOrganization = objtblOrganization;
                            entities.tblContactDetails.Add(objtblContactDetail);
                        }
                    }
                }
                #endregion

                if (objHierarchyData.EntityType == "483" || objHierarchyData.EntityType == "1256")
                {
                    objtblOrgStructure = objtblOrganization.tblOrgStructures.FirstOrDefault();
                    if (objtblOrgStructure == null)
                    {
                        objtblOrgStructure = new tblOrgStructure();
                    }
                    var ParentEntity = "";
                    objtblOrgStructure.LevelDefinition = GetHierarchyTypes().Where(a => a.ID == Convert.ToInt32(objHierarchyData.EntityType)).FirstOrDefault().Value;
                    if (objHierarchyData.ReportEntityType != null)
                    {
                        ParentEntity = GetHierarchyTypes().Where(a => a.ID == Convert.ToInt32(objHierarchyData.ReportEntityType)).FirstOrDefault().Value;
                    }
                    List<tblOrgStructure> lsttblOrgStructure = entities.tblOrgStructures.ToList();
                    foreach (var item in lsttblOrgStructure)
                    {
                        if (item.LevelDefinition == ParentEntity)
                        {
                            objtblOrgStructure.LevelDefinition = objHierarchyData.Name;
                            objtblOrgStructure.ParentID = item.OrgStructureID;
                        }
                        entities.SaveChanges();
                    }
                    objtblOrgStructure.LevelDefinition = objHierarchyData.Name;
                    objtblOrgStructure.ParentID = Convert.ToInt64(GetParentId(objHierarchyData.ReportingCode));
                    objtblOrgStructure.IsActive = true;
                    objtblOrgStructure.CreatedBy = objHierarchyData.UserName;
                    objtblOrgStructure.CreatedDate = DateTime.Now;
                    if (objtblOrgStructure.OrgStructureID == decimal.Zero)
                    {
                        objtblOrgStructure.tblOrganization = objtblOrganization;
                        entities.tblOrgStructures.Add(objtblOrgStructure);
                    }

                    #region geounitTypes
                    if (objHierarchyData.ObjGEOUnitDetails != null)
                    {
                        foreach (var item in objHierarchyData.ObjGEOUnitDetails)
                        {
                            var Entity = entities.tblOrgStructures.Where(a => a.LevelDefinition == item.Parententity).FirstOrDefault();
                            var GeoUnitName = entities.tblOrgStructures.Where(a => a.LevelDefinition == item.Geounitname).FirstOrDefault();
                            var OrgID = objtblOrganization.OrganizationID;
                            if (Entity == null || GeoUnitName == null)
                            {
                                //if (Entity == null)
                                //{
                                //    tblOrgStructure obj1 = new tblOrgStructure();
                                //    obj1.LevelDefinition = item.Parententity;
                                //    obj1.OrganizationID = OrgID;                              
                                //    obj1.IsActive = true;
                                //    obj1.CreatedDate = DateTime.Now;
                                //    entities.tblOrgStructures.Add(obj1);
                                //}
                                if (GeoUnitName == null)
                                {
                                    tblOrgStructure obj2 = new tblOrgStructure();
                                    obj2.LevelDefinition = item.Geounitname;
                                    //obj2.OrganizationID = OrgID;
                                    obj2.OrganizationID = 1;
                                    obj2.IsActive = true;
                                    obj2.CreatedDate = DateTime.Now;
                                    entities.tblOrgStructures.Add(obj2);
                                }
                                entities.SaveChanges();
                            }
                            List<tblOrgStructure> lstorganization = entities.tblOrgStructures.ToList();
                            foreach (var items in lstorganization)
                            {
                                if (Entity != null)
                                {
                                    if (items.LevelDefinition == item.Geounitname)
                                    {
                                        items.ParentID = Entity.OrgStructureID;
                                        // Entity.ParentID = items.OrgStructureID;
                                        //items.OrgStructureID =Convert.ToDecimal(Entity.ParentID);
                                    }
                                    entities.SaveChanges();
                                }
                            }
                        }
                    }
                    #endregion
                }
                //objtblOrgOffice = objtblOrganization.tblOrgOffices.FirstOrDefault();
                if (objtblOrgOffice == null)
                {
                    objtblOrgOffice = new tblOrgOffice();
                }
                objtblOrgOffice.Org_CategoryID = Convert.ToInt32(objHierarchyData.EntityType);
                objtblOrgOffice.OrganizationID = objtblOrganization.OrganizationID;
                objtblOrgOffice.OfficeName = objHierarchyData.Name;
                objtblOrgOffice.PhoneNo = Convert.ToString(objHierarchyData.Mobile1);
                objtblOrgOffice.OfficePhone1 = objHierarchyData.OfficePhone1;
                objtblOrgOffice.OfficePhone2 = objHierarchyData.OfficePhone2;
                //objtblOrgOffice.OfficeCode = objHierarchyData.Code;
                if (objHierarchyData.EntityType == "1256")
                {
                    objtblOrgOffice.OfficeCode = objHierarchyData.PartnerCode;                    
                }
                if (objHierarchyData.EntityType == "1257")
                {
                    objtblOrgOffice.OfficeCode = objHierarchyData.GeoUnitCode;                    
                }
                objtblOrgOffice.EffectiveFrom = Convert.ToDateTime(objHierarchyData.Effectivefrom);
                objtblOrgOffice.ReportingOfficeCode = objHierarchyData.ReportingCode;
                objtblOrgOffice.ReportingOfficeID = Convert.ToInt32(objHierarchyData.ReportEntityType);
                objtblOrgOffice.Email = objHierarchyData.Email;
                objtblOrgOffice.FaxNo = objHierarchyData.Fax;
                objtblOrgOffice.IsActive = true;
                objtblOrgOffice.CreatedBy = objHierarchyData.UserName;
                objtblOrgOffice.CreatedDate = DateTime.Now;
                objtblOrgOffice.CommAddressId = objHierarchyData.CommunicationAddressId.ToString();
                objtblOrgOffice.IsRegAddressSameAsCommAddress = objHierarchyData.IsRegAsCommunication;
                if (objHierarchyData.IsRegAsCommunication == true)
                {
                    objtblOrgOffice.RegistrationAddressId = objHierarchyData.CommunicationAddressId.ToString();
                }
                else
                {
                    objtblOrgOffice.RegistrationAddressId = objHierarchyData.RegistrationAddressId.ToString();
                }
                if (objHierarchyData.IsPartnerHierarchy == true)
                {
                    objtblOrgOffice.IsPartnerHierarchy = true;
                }
                else
                {
                    objtblOrgOffice.IsPartnerHierarchy = false;
                }
                if (objHierarchyData.IsPartnerInsuranceType == true)
                {
                    objtblOrgOffice.IsPartnerCentralizedType = true;
                }
                else
                {
                    objtblOrgOffice.IsPartnerCentralizedType = false;
                }
                if (objtblOrgOffice.OrgOfficeID == decimal.Zero)
                {
                    objtblOrgOffice.tblOrganization = objtblOrganization;
                    objtblOrgOffice.tblOrgStructure = objtblOrgStructure;
                    entities.tblOrgOffices.Add(objtblOrgOffice);
                }
                if (objtblOrganization.OrganizationID == decimal.Zero)
                {

                    entities.tblOrganizations.Add(objtblOrganization);
                }
                #region Fill HierarchyCoordination
                objtblHierarchyCoordination = objtblOrgOffice.tblHierarchyCoordinations.FirstOrDefault();
                if (objtblHierarchyCoordination == null)
                {
                    objtblHierarchyCoordination = new tblHierarchyCoordination();
                }
                if (objHierarchyData.ObjCoordination != null)
                {
                    List<tblHierarchyCoordination> deleteRecords = objtblOrgOffice.tblHierarchyCoordinations.ToList();
                    if (deleteRecords != null)
                    {
                        foreach (var items in deleteRecords)
                        {
                            var deleteobj = entities.tblHierarchyCoordinations.Where(a => a.OrgOfficeID == items.OrgOfficeID).ToList();
                            foreach (tblHierarchyCoordination delobj in deleteobj)
                            {
                                entities.tblHierarchyCoordinations.Remove(delobj);
                            }
                        }
                        entities.SaveChanges();
                    }
                    foreach (var item in objHierarchyData.ObjCoordination)
                    {
                        objtblHierarchyCoordination = new tblHierarchyCoordination();
                        //objtblHierarchyCoordination.PartnerID = item.CoordinateID;
                        objtblHierarchyCoordination.Partner = item.Partner;
                        objtblHierarchyCoordination.UnitType = item.Geounittype;
                        objtblHierarchyCoordination.Name = item.Geounitname;
                        objtblHierarchyCoordination.Code = item.Code;
                        if (objtblHierarchyCoordination.CoordinationID == decimal.Zero)
                        {
                            objtblHierarchyCoordination.tblOrgOffice = objtblOrgOffice;
                            entities.tblHierarchyCoordinations.Add(objtblHierarchyCoordination);
                        }
                    }                  
                }
                #endregion
                var Result = entities.SaveChanges();
                if (Result > 0)
                {
                    objHierarchyData.Message = "Success";
                }
                return objHierarchyData;
            }
            catch (Exception Ex)
            {
                objHierarchyData.Message = Ex.InnerException.Message;
                return objHierarchyData;
            }
        }    
        public HierarchyData SaveChannelSubChannel(HierarchyData objHierarchyData)
        {
            try
            {               
                tblOrganization objtblOrganization = null;
                tblAddress objtblAddress = new tblAddress();
                tblOrgStructure objtblOrgStructure = null;
                tblOrgOffice objtblOrgOffice = null;
                tblMasCommonType objtblMasCommonType = new tblMasCommonType();
                objtblOrganization = entities.tblOrganizations.Where(a => a.Org_CategoryID == 483).FirstOrDefault();                
                if (objHierarchyData.EntityType == "1254")
                {                    
                    objtblOrgOffice = entities.tblOrgOffices.Where(a => a.OfficeCode == objHierarchyData.ChannelCode).FirstOrDefault();
                }
                else if (objHierarchyData.EntityType == "1255")
                {
                    objtblOrgOffice = entities.tblOrgOffices.Where(a => a.OfficeCode == objHierarchyData.SubChannelCode).FirstOrDefault();
                }
                if (objtblOrganization == null)
                {
                    objtblOrganization = new tblOrganization();
                }
                //#region tblAddress
                //tblPolicyRelationship objtblpolicyrelationship = objtblOrganization.tblPolicyRelationships.FirstOrDefault();
                //if (objtblpolicyrelationship == null)
                //{
                //    objtblpolicyrelationship = new tblPolicyRelationship();
                //}

                //tblPolicyClient objtblPolicyClient = objtblpolicyrelationship.tblPolicyClient;
                //if (objtblPolicyClient == null)
                //{
                //    objtblPolicyClient = new tblPolicyClient();
                //}
                //objtblPolicyClient = FilltblpolicyClient(objHierarchyData, objtblPolicyClient);
                //objtblPolicyClient.tblAddress1 = objtblPolicyClient.tblAddress;
                //objtblpolicyrelationship.tblPolicyClient = objtblPolicyClient;
                //objtblpolicyrelationship.tblOrganization = objtblOrganization;
                //if (objtblpolicyrelationship.PolicyRelationshipID == decimal.Zero)
                //{
                //    entities.tblPolicyRelationships.Add(objtblpolicyrelationship);
                //}
                //#endregion

                #region Fill Communication Address
                if (objHierarchyData.objCommunicationAddress.Address1 != null)
                {                   
                    objtblAddress = entities.tblAddresses.Where(a => a.AddressID == objHierarchyData.CommunicationAddressId).FirstOrDefault();
                    if (objtblAddress == null)
                    {
                        objtblAddress = new tblAddress();
                    }
                    objtblAddress.Address1 = objHierarchyData.objCommunicationAddress.Address1;
                    objtblAddress.Address2 = objHierarchyData.objCommunicationAddress.Address2;
                    objtblAddress.Address3 = objHierarchyData.objCommunicationAddress.Address3;
                    objtblAddress.State = objHierarchyData.objCommunicationAddress.State;
                    objtblAddress.District = objHierarchyData.objCommunicationAddress.District;
                    objtblAddress.City = objHierarchyData.objCommunicationAddress.City;                    
                    objtblAddress.Pincode = objHierarchyData.objCommunicationAddress.Pincode;
                    objtblAddress.CreatedBy = objHierarchyData.UserName;
                    objtblAddress.CreatedDate = DateTime.Now;
                    objtblAddress.Status = true;                   
                    if (objtblAddress.AddressID == decimal.Zero)
                    {
                        entities.tblAddresses.Add(objtblAddress);
                    }
                    var ResultCommAddress = entities.SaveChanges();
                    if (ResultCommAddress > 0)
                    {
                        objHierarchyData.CommunicationAddressId = Convert.ToInt32(objtblAddress.AddressID);
                    }
                   
                }
                #endregion
                #region Fill Registration Address
                if (objHierarchyData.IsRegAsCommunication == false)
                {                   
                    if (objHierarchyData.objRegistrationAddress.Address1 != null)
                    {
                        objtblAddress = entities.tblAddresses.Where(a => a.AddressID == objHierarchyData.RegistrationAddressId).FirstOrDefault();
                        if (objtblAddress == null)
                        {
                            objtblAddress = new tblAddress();
                        }
                        objtblAddress.Address1 = objHierarchyData.objRegistrationAddress.Address1;
                        objtblAddress.Address2 = objHierarchyData.objRegistrationAddress.Address2;
                        objtblAddress.Address3 = objHierarchyData.objRegistrationAddress.Address3;
                        objtblAddress.State = objHierarchyData.objRegistrationAddress.State;
                        objtblAddress.District = objHierarchyData.objRegistrationAddress.District;
                        objtblAddress.City = objHierarchyData.objRegistrationAddress.City;
                        objtblAddress.Pincode = objHierarchyData.objRegistrationAddress.Pincode;
                        objtblAddress.CreatedBy = objHierarchyData.UserName;
                        objtblAddress.CreatedDate = DateTime.Now;
                        objtblAddress.Status = true;
                        if (objtblAddress.AddressID == decimal.Zero)
                        {
                            entities.tblAddresses.Add(objtblAddress);
                        }
                        var ResultRegAddress = entities.SaveChanges();
                        if (ResultRegAddress > 0)
                        {
                            objHierarchyData.RegistrationAddressId = Convert.ToInt32(objtblAddress.AddressID);
                        }
                    }                   
                }
                #endregion
                //objtblOrgStructure = objtblOrganization.tblOrgStructures.FirstOrDefault();
                if (objHierarchyData.EntityType == "1254")
                {
                    if (objHierarchyData.OrgStructureIDChannel == 0)
                    {
                        objtblOrgStructure = entities.tblOrgStructures.Where(a => a.OrgStructureID == objHierarchyData.OrgStructureIDChannel).FirstOrDefault();
                    }
                    else
                    {
                        objtblOrgStructure = entities.tblOrgStructures.Where(a => a.OrgStructureID == objtblOrgOffice.OrgStructureID).FirstOrDefault();
                    }
                    if (objtblOrgStructure == null)
                    {
                        objtblOrgStructure = new tblOrgStructure();
                    }
                    //objtblOrgStructure.LevelDefinition = GetHierarchyTypes().Where(a => a.ID == Convert.ToInt32(objHierarchyData.EntityType)).FirstOrDefault().Value;
                    //var ParentEntity = GetHierarchyTypes().Where(a => a.ID == Convert.ToInt32(objHierarchyData.ReportEntityType)).FirstOrDefault().Value;
                    //List<tblOrgStructure> lsttblOrgStructure = entities.tblOrgStructures.ToList();
                    //foreach (var item in lsttblOrgStructure)
                    //{
                    //    if (item.LevelDefinition == ParentEntity)
                    //    {
                    //        objtblOrgStructure.ParentID = item.OrgStructureID;
                    //    }
                    //    entities.SaveChanges();
                    //}
                    objtblOrgStructure.LevelDefinition = objHierarchyData.Name;
                    objtblOrgStructure.ParentID = Convert.ToInt64(GetParentId(objHierarchyData.ReportingCode));
                    objtblOrgStructure.OrganizationID = objtblOrganization.OrganizationID;
                    objtblOrgStructure.IsActive = true;
                    objtblOrgStructure.CreatedBy = objHierarchyData.UserName;
                    objtblOrgStructure.CreatedDate = DateTime.Now;                 
                    if (objtblOrgStructure.OrgStructureID == decimal.Zero)
                    {
                        //objtblOrgStructure.tblOrganization = objtblOrganization;
                        entities.tblOrgStructures.Add(objtblOrgStructure);
                    }
                }
                if (objHierarchyData.EntityType == "1255")
                {
                    if (objHierarchyData.OrgStructureIDSubChannel == 0)
                    {
                        objtblOrgStructure = entities.tblOrgStructures.Where(a => a.OrgStructureID == objHierarchyData.OrgStructureIDSubChannel).FirstOrDefault();
                    }
                    else
                    {
                        objtblOrgStructure = entities.tblOrgStructures.Where(a => a.OrgStructureID == objtblOrgOffice.OrgStructureID).FirstOrDefault();
                    }
                    if (objtblOrgStructure == null)
                    {
                        objtblOrgStructure = new tblOrgStructure();
                    }
                    //objtblOrgStructure.LevelDefinition = GetHierarchyTypes().Where(a => a.ID == Convert.ToInt32(objHierarchyData.EntityType)).FirstOrDefault().Value;
                    //var ParentEntity = GetHierarchyTypes().Where(a => a.ID == Convert.ToInt32(objHierarchyData.ReportEntityType)).FirstOrDefault().Value;
                    //List<tblOrgStructure> lsttblOrgStructure = entities.tblOrgStructures.ToList();
                    //foreach (var item in lsttblOrgStructure)
                    //{
                    //    if (item.LevelDefinition == ParentEntity)
                    //    {
                    //        objtblOrgStructure.ParentID = item.OrgStructureID;
                    //    }
                    //    entities.SaveChanges();
                    //}
                    objtblOrgStructure.LevelDefinition = objHierarchyData.Name;
                    objtblOrgStructure.ParentID = Convert.ToInt64(GetParentId(objHierarchyData.ReportingCode));
                    objtblOrgStructure.OrganizationID = objtblOrganization.OrganizationID;
                    objtblOrgStructure.IsActive = true;
                    objtblOrgStructure.CreatedBy = objHierarchyData.UserName;
                    objtblOrgStructure.CreatedDate = DateTime.Now;              
                    if (objtblOrgStructure.OrgStructureID == decimal.Zero)
                    {
                        //objtblOrgStructure.tblOrganization = objtblOrganization;
                        entities.tblOrgStructures.Add(objtblOrgStructure);
                    }
                }

                //#region geounitTypes
                //if (objHierarchyData.ObjGEOUnitDetails != null)
                //{

                //    foreach (var item in objHierarchyData.ObjGEOUnitDetails)
                //    {
                //        var Entity = entities.tblOrgStructures.Where(a => a.LevelDefinition == item.Parententity).FirstOrDefault();
                //        var GeoUnitName = entities.tblOrgStructures.Where(a => a.LevelDefinition == item.Geounitname).FirstOrDefault();
                //        if (Entity == null || GeoUnitName == null)
                //        {
                //            if (Entity == null)
                //            {
                //                tblOrgStructure obj1 = new tblOrgStructure();
                //                obj1.LevelDefinition = item.Parententity;
                //                obj1.IsActive = true;
                //                obj1.CreatedDate = DateTime.Now;
                //                entities.tblOrgStructures.Add(obj1);
                //            }
                //            if (GeoUnitName == null)
                //            {
                //                tblOrgStructure obj2 = new tblOrgStructure();
                //                obj2.LevelDefinition = item.Geounitname;
                //                obj2.IsActive = true;
                //                obj2.CreatedDate = DateTime.Now;
                //                entities.tblOrgStructures.Add(obj2);
                //            }
                //            entities.SaveChanges();
                //        }
                //        List<tblOrgStructure> lstorganization = entities.tblOrgStructures.ToList();
                //        foreach (var items in lstorganization)
                //        {
                //            if (Entity != null)
                //            {
                //                if (items.LevelDefinition == item.Geounitname)
                //                {
                //                    items.ParentID = Entity.OrgStructureID;
                //                    // Entity.ParentID = items.OrgStructureID;
                //                    //items.OrgStructureID =Convert.ToDecimal(Entity.ParentID);
                //                }
                //                entities.SaveChanges();
                //            }
                //        }
                //    }
                //}
                //#endregion
                #region geounitTypes
                if (objHierarchyData.ObjGEOUnitDetails != null)
                {
                    foreach (var item in objHierarchyData.ObjGEOUnitDetails)
                    {
                        var Entity = entities.tblOrgStructures.Where(a => a.LevelDefinition == item.Parententity).FirstOrDefault();
                        var GeoUnitName = entities.tblOrgStructures.Where(a => a.LevelDefinition == item.Geounitname).FirstOrDefault();
                        var OrgID = objtblOrganization.OrganizationID;
                        if (Entity == null || GeoUnitName == null)
                        {
                            //if (Entity == null)
                            //{
                            //    tblOrgStructure obj1 = new tblOrgStructure();
                            //    obj1.LevelDefinition = item.Parententity;
                            //    obj1.OrganizationID = OrgID;                              
                            //    obj1.IsActive = true;
                            //    obj1.CreatedDate = DateTime.Now;
                            //    entities.tblOrgStructures.Add(obj1);
                            //}
                            if (GeoUnitName == null)
                            {
                                tblOrgStructure obj2 = new tblOrgStructure();
                                obj2.LevelDefinition = item.Geounitname;
                                obj2.OrganizationID = OrgID;
                                obj2.IsActive = true;
                                obj2.CreatedDate = DateTime.Now;
                                entities.tblOrgStructures.Add(obj2);
                            }
                            entities.SaveChanges();
                        }
                        List<tblOrgStructure> lstorganization = entities.tblOrgStructures.ToList();
                        foreach (var items in lstorganization)
                        {
                            if (Entity != null)
                            {
                                if (items.LevelDefinition == item.Geounitname)
                                {
                                    items.ParentID = Entity.OrgStructureID;
                                    // Entity.ParentID = items.OrgStructureID;
                                    //items.OrgStructureID =Convert.ToDecimal(Entity.ParentID);
                                }
                                entities.SaveChanges();
                            }
                        }
                    }
                }
                #endregion
                //objtblOrgOffice = objtblOrganization.tblOrgOffices.FirstOrDefault();
                if (objtblOrgOffice == null)
                {
                    objtblOrgOffice = new tblOrgOffice();
                }
                objtblOrgOffice.Org_CategoryID = Convert.ToInt32(objHierarchyData.EntityType);
                objtblOrgOffice.OrganizationID = objtblOrganization.OrganizationID;
                objtblOrgOffice.OfficeName = objHierarchyData.Name;
                objtblOrgOffice.PhoneNo = Convert.ToString(objHierarchyData.Mobile1);
                objtblOrgOffice.OfficePhone1 = objHierarchyData.OfficePhone1;
                objtblOrgOffice.OfficePhone2 = objHierarchyData.OfficePhone2;
                objtblOrgOffice.ReportingOfficeCode = objHierarchyData.ReportingCode;
                objtblOrgOffice.EffectiveFrom = Convert.ToDateTime(objHierarchyData.Effectivefrom);
                if (objHierarchyData.EntityType == "1254")
                {
                    objtblOrgOffice.OfficeCode = objHierarchyData.ChannelCode;
                }
                else
                {
                    objtblOrgOffice.OfficeCode = objHierarchyData.SubChannelCode;
                }                    
                //objtblOrgOffice.OfficeCode = objHierarchyData.Code;
                //objtblOrgOffice.OfficeCode = GetChannelCode();
                objtblOrgOffice.ReportingOfficeID = Convert.ToInt32(objHierarchyData.ReportEntityType);
                objtblOrgOffice.Email = objHierarchyData.Email;
                objtblOrgOffice.FaxNo = objHierarchyData.Fax;
                //objtblOrgOffice.IsActive = objHierarchyData.Status;
                //objtblOrgOffice.IsActive = Convert.ToBoolean(Convert.ToInt32(objHierarchyData.Status));
                objtblOrgOffice.IsActive = true;
                objtblOrgOffice.CreatedBy = objHierarchyData.UserName;
                objtblOrgOffice.CreatedDate = DateTime.Now;
                objtblOrgOffice.CommAddressId = objHierarchyData.CommunicationAddressId.ToString();
                objtblOrgOffice.IsRegAddressSameAsCommAddress = objHierarchyData.IsRegAsCommunication;
                if (objHierarchyData.IsRegAsCommunication == true)
                {
                    objtblOrgOffice.RegistrationAddressId = objHierarchyData.CommunicationAddressId.ToString();
                }
                else
                {
                    objtblOrgOffice.RegistrationAddressId = objHierarchyData.RegistrationAddressId.ToString();
                }           
                if (objHierarchyData.IsPartnerHierarchy == true)
                {
                    objtblOrgOffice.IsPartnerHierarchy = true;
                }
                else
                {
                    objtblOrgOffice.IsPartnerHierarchy = false;
                }
                if (objHierarchyData.IsPartnerInsuranceType == true)
                {
                    objtblOrgOffice.IsPartnerCentralizedType = true;
                }
                else
                {
                    objtblOrgOffice.IsPartnerCentralizedType = false;
                }
                if (objtblOrgOffice.OrgOfficeID == decimal.Zero)
                {
                    objtblOrgOffice.tblOrganization = objtblOrganization;
                    objtblOrgOffice.tblOrgStructure = objtblOrgStructure;                    
                    entities.tblOrgOffices.Add(objtblOrgOffice);
                }
                
                if (objtblOrganization.OrganizationID == decimal.Zero)
                {
                    entities.tblOrganizations.Add(objtblOrganization);
                }
                var Result = entities.SaveChanges();

                if (objHierarchyData.EntityType == "1254")
                {
                    objHierarchyData.OrgStructureIDChannel = Convert.ToInt32(objtblOrgOffice.OrgStructureID);
                }
                if (objHierarchyData.EntityType == "1255")
                {
                    objHierarchyData.OrgStructureIDSubChannel = Convert.ToInt32(objtblOrgOffice.OrgStructureID);
                }                    
                if (Result > 0)
                {
                    objHierarchyData.Message = "Success";
                }
                return objHierarchyData;
            }
            catch (Exception Ex)
            {
                objHierarchyData.Message = Ex.InnerException.Message;
                return objHierarchyData;
            }
        }

        public IEnumerable<MasterListItem> GetHierarchyChannel()
        {
            IEnumerable<MasterListItem> lstChannel = from cat in entities.tblOrgOffices
                                                     where cat.Org_CategoryID == 1254 && cat.OfficeName != null

                                                     select new MasterListItem
                                                     {
                                                         Value = cat.OfficeName,
                                                         ID = (int)cat.OrgOfficeID

                                                     };
            return lstChannel;
        }
        public List<MasterListItem> GetHierarchySubChannel()
        {
            List<MasterListItem> lstSubChannel = (from cat in entities.tblOrgOffices
                                                        where cat.Org_CategoryID == 1255 && cat.OfficeName != null
                                                        select new MasterListItem
                                                        {
                                                            Value = cat.OfficeName,
                                                            ID = (int)cat.OrgOfficeID

                                                        }).ToList();
            return lstSubChannel;
        }
        public List<MasterListItem> GetHierarchySubChannel(HierarchyData objHierarchyData)
        {
            List<MasterListItem> lstSubChannel = new List<MasterListItem>();
            if (objHierarchyData.CoordinationChannel != null && objHierarchyData.CoordinationChannel != "")
            {
                int OrgOfficeID = Convert.ToInt32(objHierarchyData.CoordinationChannel);
                var InnerQuery = (from obj in entities.tblOrgOffices
                                  where obj.OrgOfficeID == OrgOfficeID
                                  select obj.OfficeCode).SingleOrDefault();

                if (objHierarchyData.GeoPartner == null)
                {
                    lstSubChannel = (from cat in entities.tblOrgOffices
                                     where InnerQuery.Contains(cat.ReportingOfficeCode) && (cat.OfficeName != null || cat.OfficeName != "") && cat.IsPartnerHierarchy == true
                                     select new MasterListItem
                                     {
                                         Value = cat.OfficeName,
                                         ID = (int)cat.OrgOfficeID
                                     }).ToList();
                }
                else
                {
                    lstSubChannel = (from cat in entities.tblOrgOffices
                                     where InnerQuery.Contains(cat.ReportingOfficeCode) && (cat.OfficeName != null || cat.OfficeName != "") && cat.IsPartnerHierarchy == null
                                     select new MasterListItem
                                     {
                                         Value = cat.OfficeName,
                                         ID = (int)cat.OrgOfficeID
                                     }).ToList();
                }
                
            }
            return lstSubChannel;
        }
        public List<MasterListItem> GetGeoParentEntity()
        {
            List<MasterListItem> lstGeoUnitParentEntity = (from cat in entities.tblOrgStructures
                                                           where cat.LevelDefinition != null
                                                           select new MasterListItem
                                                           {
                                                               Value = cat.LevelDefinition,
                                                               ID = (int)cat.OrgStructureID

                                                           }).ToList();
            return lstGeoUnitParentEntity;
        }
        public List<MasterListItem> GetHierarchyPartners()
        {
            List<MasterListItem> lstSubChannel = (from cat in entities.tblOrgOffices
                                                  where cat.Org_CategoryID == 1256 && cat.OfficeName != null
                                                  select new MasterListItem
                                                  {
                                                      Value = cat.OfficeName,
                                                      ID = (int)cat.OrgOfficeID

                                                  }).ToList();
            return lstSubChannel;
        }
        public List<MasterListItem> GetHierarchyGeoUnits()
        {
            List<MasterListItem> lstSubChannel = (from cat in entities.tblOrgOffices
                                                  where cat.Org_CategoryID == 1257 && cat.OfficeName != null
                                                  select new MasterListItem
                                                  {
                                                      Value = cat.OfficeName,
                                                      ID = (int)cat.OrgOfficeID

                                                  }).ToList();
            return lstSubChannel;
        }
        public List<Coordination> GetCoordinationDataGrid(string CoordinationSearchID)
        {
            HierarchyDataBusiness hierarchyBusiness = new HierarchyDataBusiness();
            List<Coordination> objLstCoordinationSearchData = new List<Coordination>();
            //char[] data = { ',' };
            //string[] CoordinationCode = CoordinationSearchID.Split(data);
            //objLstCoordinationSearchData = (from obj in hierarchyBusiness.GetCoordination()
            //                                //where CoordinationCode.Contains(obj.CoordinationSearchID)
            //                                  select new CoordinationTaskDetails
            //                                  {
            //                                      CoordinateID = (int)obj.CoordinateID,
            //                                      Geounittype = obj.Geounittype,
            //                                      Name = obj.Name,
            //                                      Code = obj.Code                                 
            //                                  }).ToList();
            //return objLstCoordinationSearchData;
            //objLstCoordinationSearchData = GetCoordination();
            return objLstCoordinationSearchData;
        }
        public List<MasterListItem> GetPartner(string GeoSubChannelId)
        {
            //int ChannelID = Convert.ToInt32(GeoSubChannelId);
            //var InnerQuery = (from catOrganisation in entities.tblOrganizations
            //                  where catOrganisation.OrganizationID == ChannelID
            //                  select catOrganisation.Code).SingleOrDefault();

            //List<MasterListItem> lstChannel = (from catOffice in entities.tblOrgOffices
            //                                   where InnerQuery.Contains(catOffice.ReportingOfficeCode) && catOffice.IsPartnerHierarchy == true
            //                                   select new MasterListItem
            //                                   {
            //                                       Value = catOffice.OfficeName,
            //                                       ID = (int)catOffice.OrgOfficeID
            //                                   }).ToList();
                        
            List<MasterListItem> lstChannel = new List<MasterListItem>();            
            if (GeoSubChannelId != null || GeoSubChannelId != "")
            {
                int ChannelID = Convert.ToInt32(GeoSubChannelId);
                var InnerQuery = (from catOffice in entities.tblOrgOffices
                                  where catOffice.OrgOfficeID == ChannelID && catOffice.IsPartnerHierarchy == true
                                  select catOffice.OfficeCode).SingleOrDefault();

                lstChannel = (from cat in entities.tblOrgOffices
                              where InnerQuery.Contains(cat.ReportingOfficeCode) && (cat.OfficeName != null || cat.OfficeName != "")
                              select new MasterListItem
                              {
                                  Value = cat.OfficeName,
                                  ID = (int)cat.OrgOfficeID
                              }).ToList();
            }
            return lstChannel;
        }
        public List<MasterListItem> GetGeoUnit(string PartnerTypId)
        {
            List<MasterListItem> lstChannel = new List<MasterListItem>();
            if (PartnerTypId != null || PartnerTypId != "")
            {
                //int ChannelID = Convert.ToInt32(PartnerTypId);
                //var InnerQuery = (from catOffice in entities.tblOrgOffices
                //                  where catOffice.OrgOfficeID == ChannelID
                //                  select catOffice.OfficeCode).SingleOrDefault();

                //lstChannel = (from cat in entities.tblOrgOffices
                //              where InnerQuery.Contains(cat.ReportingOfficeCode) && (cat.OfficeName != null || cat.OfficeName != "")
                //              select new MasterListItem
                //              {
                //                  Value = cat.OfficeName,
                //                  ID = (int)cat.OrgOfficeID
                //              }).ToList();

                int ChannelID = Convert.ToInt32(PartnerTypId);
                var InnerQuery = (from catOffice in entities.tblOrgOffices
                                  where catOffice.OrgOfficeID == ChannelID
                                  select catOffice.OrgStructureID).SingleOrDefault();

                lstChannel = (from cat in entities.tblOrgStructures
                              where cat.ParentID == InnerQuery && (cat.ParentID != null)
                              select new MasterListItem
                              {
                                  Value = cat.LevelDefinition,
                                  ID = (int)cat.OrgStructureID
                              }).ToList();
            }
            return lstChannel;
        }       
        public List<MasterListItem> GetReportingEntityCode(HierarchyData objHierarchyData)
        {
            var OrgCategoryID = Convert.ToInt32(objHierarchyData.ReportEntityType);
            List<MasterListItem> records = (from obj in entities.tblOrgOffices
                                            where obj.OfficeCode.StartsWith(objHierarchyData.SearchTerm) && obj.Org_CategoryID == OrgCategoryID
                                            select new MasterListItem
                                            {
                                                Value = obj.OfficeCode + "," + obj.OfficeName,
                                                ID = (int)obj.OrgOfficeID
                                            }).ToList();
            return records;
        }
        public List<HierarchyData> GetReportingEntityCodeAndName(string ReportingEntityCode)
        {
            List<HierarchyData> records = (from obj in entities.tblOrgOffices
                                           where obj.OfficeCode.StartsWith(ReportingEntityCode)
                                           select new HierarchyData
                                           {
                                               ReportingCode = obj.OfficeCode,
                                               ReportingName = obj.OfficeName
                                           }).ToList();
            return records;
        }
        //temp for GeoUnit
        public List<MasterListItem> GetReportingEntityCodeGeoUnit(HierarchyData objHierarchyData)
        {
            var OrgCategoryID = Convert.ToInt32(objHierarchyData.ReportEntityType);
            List<MasterListItem> records = (from obj in entities.tblOrgOffices
                                            where obj.OfficeCode.StartsWith(objHierarchyData.SearchTerm) && (obj.Org_CategoryID == 1255 || obj.Org_CategoryID == 1256)
                                            select new MasterListItem
                                            {
                                                Value = obj.OfficeCode + "," + obj.OfficeName,
                                                ID = (int)obj.OrgOfficeID
                                            }).ToList();
            return records;
        }
        //temp for GeoUnit
        public List<HierarchyData> GetReportingEntityCodeAndNameGeoUnit(string ReportingEntityCode)
        {
            List<HierarchyData> records = (from obj in entities.tblOrgOffices
                                           where obj.OfficeCode.StartsWith(ReportingEntityCode)
                                           select new HierarchyData
                                           {
                                               ReportingCode = obj.OfficeCode,
                                               ReportingName = obj.OfficeName
                                           }).ToList();
            return records;
        }
        public List<MasterListItem> GetReportingEntityName(HierarchyData objHierarchyData)
        {
            var OrgCategoryID = Convert.ToInt32(objHierarchyData.ReportEntityType);
            List<MasterListItem> records = (from obj in entities.tblOrgOffices
                                            where obj.OfficeName.StartsWith(objHierarchyData.SearchTerm) && obj.Org_CategoryID == OrgCategoryID
                                            select new MasterListItem
                                            {
                                                Value = obj.OfficeCode + "," + obj.OfficeName,
                                                ID = (int)obj.OrgOfficeID
                                            }).ToList();
            return records;
        }
        //temp for geounit
        public List<MasterListItem> GetReportingEntityNameGeoUnit(HierarchyData objHierarchyData)
        {
            var OrgCategoryID = Convert.ToInt32(objHierarchyData.ReportEntityType);
            List<MasterListItem> records = (from obj in entities.tblOrgOffices
                                            where obj.OfficeName.StartsWith(objHierarchyData.SearchTerm) && (obj.Org_CategoryID == 1255 || obj.Org_CategoryID == 1256)
                                            select new MasterListItem
                                            {
                                                Value = obj.OfficeCode + "," + obj.OfficeName,
                                                ID = (int)obj.OrgOfficeID
                                            }).ToList();
            return records;
        }
        public List<MasterListItem> LoadQuickActions()
        {
            List<MasterListItem> objlstQuickActions = new List<MasterListItem>();
            objlstQuickActions.Add(new MasterListItem { ID = 1, Value = "View" });
            return objlstQuickActions;
        }
        public HierarchyData FetchHierarchy(HierarchyData objHierarchyData)
        {
            try
            {
                tblContactDetail objtblContactDetail = null;
                tblOrganization objtblOrganization = null;
                tblOrgOffice objtblOrgOffice = null;
                tblOrgStructure objtblOrgStructure = null;
                tblAddress objtblAddress = null;
                if (objHierarchyData != null)
                {
                    objtblOrgOffice = entities.tblOrgOffices.Where(a => a.OfficeCode == objHierarchyData.SearchCodeGrid).FirstOrDefault();
                    objtblOrganization = objtblOrgOffice.tblOrganization;
                    if (objtblOrgOffice != null)
                    {
                        objHierarchyData.EntityType = objtblOrgOffice.Org_CategoryID.ToString();
                        objHierarchyData.Name = objtblOrgOffice.OfficeName;
                        if (objHierarchyData.EntityType == "483" || objHierarchyData.EntityType == "1256")
                        {
                            objHierarchyData.YearofEstablish = Convert.ToInt32(objtblOrganization.YearOfEstablishment);
                            objHierarchyData.RegNo = objtblOrganization.RegistrationNo;
                            objHierarchyData.Status = objtblOrganization.Reg_no_st;
                            objHierarchyData.Code = objtblOrganization.Code;
                        }
                        //objHierarchyData.Status = objtblOrgOffice.IsActive.ToString();
                        var StatusValue = objtblOrgOffice.IsActive.ToString();
                        if (StatusValue == "True")
                        {
                            objHierarchyData.Status = "1";
                        }
                        else
                        {
                            objHierarchyData.Status = "2";
                        }
                        objHierarchyData.Code = objtblOrgOffice.OfficeCode;
                        objHierarchyData.ChannelCode = objtblOrgOffice.OfficeCode;
                        objHierarchyData.SubChannel = objtblOrgOffice.OfficeCode;
                        objHierarchyData.PartnerCode = objtblOrgOffice.OfficeCode;
                        objHierarchyData.GeoUnitCode = objtblOrgOffice.OfficeCode;
                        if (objtblOrgOffice.PhoneNo != "")
                            objHierarchyData.Mobile1 = Convert.ToInt64(objtblOrgOffice.PhoneNo);
                        if (objtblOrgOffice.OfficePhone1 != null)
                            objHierarchyData.OfficePhone1 = Convert.ToInt64(objtblOrgOffice.OfficePhone1);
                        if (objtblOrgOffice.OfficePhone2 != null)
                            objHierarchyData.OfficePhone2 = Convert.ToInt64(objtblOrgOffice.OfficePhone2);
                        objHierarchyData.Effectivefrom = objtblOrgOffice.EffectiveFrom.ToString();
                        objHierarchyData.ReportingCode = objtblOrgOffice.ReportingOfficeCode;
                        objHierarchyData.ReportEntityType = objtblOrgOffice.ReportingOfficeID.ToString();
                        objHierarchyData.ReportingName = GetReportingName(objHierarchyData.ReportingCode, objHierarchyData.ReportEntityType);                                   
                        objHierarchyData.Email = objtblOrgOffice.Email;
                        objHierarchyData.Fax = objtblOrgOffice.FaxNo;
                        objHierarchyData.SubChannel = GetPartnerSubChannel(objHierarchyData.ReportingCode);
                        if(objtblOrgOffice.IsRegAddressSameAsCommAddress != null)
                        objHierarchyData.IsRegAsCommunication = (bool)objtblOrgOffice.IsRegAddressSameAsCommAddress;
                        objHierarchyData.CommunicationAddressId = Convert.ToInt32(objtblOrgOffice.CommAddressId);
                        if (objtblOrgOffice.IsRegAddressSameAsCommAddress == true)
                            objHierarchyData.RegistrationAddressId = Convert.ToInt32(objtblOrgOffice.CommAddressId);
                        objHierarchyData.RegistrationAddressId = Convert.ToInt32(objtblOrgOffice.RegistrationAddressId);
                    }
                    //#region FetchAddressDetails
                    //tblPolicyRelationship objtblpolicyrelationship = objtblOrganization.tblPolicyRelationships.FirstOrDefault();
                    //if (objtblpolicyrelationship != null)
                    //{
                    //    tblPolicyClient objtblPolicyClient = objtblpolicyrelationship.tblPolicyClient;

                    //    tblAddress objtbladdress = objtblPolicyClient.tblAddress1;
                    //    tblAddress objtblRegistrationaddress = objtblPolicyClient.tblAddress;
                    //    Address objAddress = new Address();

                    //    objHierarchyData.objCommunicationAddress = FetchAddressDetails(objtbladdress);

                    //    objHierarchyData.IsRegAsCommunication = Convert.ToBoolean(objtblPolicyClient.IsPermanentAddrSameasCommAddr);
                    //    if (objtblRegistrationaddress != null)
                    //    {
                    //        if (objHierarchyData.IsRegAsCommunication)
                    //        {
                    //            objHierarchyData.objRegistrationAddress = FetchAddressDetails(objtbladdress);
                    //        }
                    //        else
                    //        {
                    //            objHierarchyData.objRegistrationAddress = FetchAddressDetails(objtblRegistrationaddress);
                    //        }
                    //    }                     
                    //}
                    //#endregion               
                    #region FetchCommunicationAddressDetails
                    objtblAddress = entities.tblAddresses.Where(a => a.AddressID == objHierarchyData.CommunicationAddressId).FirstOrDefault();
                    if (objtblAddress == null)
                    {
                        objtblAddress = new tblAddress();
                    }
                    if (objHierarchyData.objCommunicationAddress == null)
                    {
                        objHierarchyData.objCommunicationAddress = new Address();
                    }
                    objHierarchyData.objCommunicationAddress.AddressID = objtblAddress.AddressID;
                    objHierarchyData.objCommunicationAddress.Address1 = objtblAddress.Address1;
                    objHierarchyData.objCommunicationAddress.Address2 = objtblAddress.Address2;
                    objHierarchyData.objCommunicationAddress.Address3 = objtblAddress.Address3;
                    objHierarchyData.objCommunicationAddress.State = objtblAddress.State;
                    objHierarchyData.objCommunicationAddress.District = objtblAddress.District;
                    objHierarchyData.objCommunicationAddress.City = objtblAddress.City;
                    objHierarchyData.objCommunicationAddress.Pincode = objtblAddress.Pincode;
                    #endregion
                    #region FetchRegistrationAddressDetails
                    objtblAddress = entities.tblAddresses.Where(a => a.AddressID == objHierarchyData.RegistrationAddressId).FirstOrDefault();
                    if (objtblAddress == null)
                    {
                        objtblAddress = new tblAddress();
                    }
                    if (objHierarchyData.objRegistrationAddress == null)
                    {
                        objHierarchyData.objRegistrationAddress = new Address();
                    }
                    objHierarchyData.objRegistrationAddress.AddressID = objtblAddress.AddressID;
                    objHierarchyData.objRegistrationAddress.Address1 = objtblAddress.Address1;
                    objHierarchyData.objRegistrationAddress.Address2 = objtblAddress.Address2;
                    objHierarchyData.objRegistrationAddress.Address3 = objtblAddress.Address3;
                    objHierarchyData.objRegistrationAddress.State = objtblAddress.State;
                    objHierarchyData.objRegistrationAddress.District = objtblAddress.District;
                    objHierarchyData.objRegistrationAddress.City = objtblAddress.City;
                    objHierarchyData.objRegistrationAddress.Pincode = objtblAddress.Pincode;
                    #endregion

                    #region FetchLicenseDetails
                    if (objtblOrganization != null)
                    {
                        objHierarchyData.LicenseNo = objtblOrganization.LicenseNo;
                        objHierarchyData.IssueDate = objtblOrganization.IssueDate;
                        objHierarchyData.ExperiyDate = objtblOrganization.ExpiryDate;
                        objHierarchyData.ContractEffetiveFrom = objtblOrganization.ContractEffectiveFrom;
                        objHierarchyData.ContractEffetiveTo = objtblOrganization.ContractEffectiveTo;
                    }
                    #endregion

                    #region FetchtblContactDetails
                    objtblContactDetail = objtblOrganization.tblContactDetails.FirstOrDefault();
                    if (objtblContactDetail == null)
                    {
                        objtblContactDetail = new tblContactDetail();
                    }
                    objHierarchyData.Position = objtblContactDetail.Position;
                    objHierarchyData.Salutation = objtblContactDetail.Salutation;
                    objHierarchyData.Firstname = objtblContactDetail.FirstName;
                    objHierarchyData.Middlename = objtblContactDetail.MiddleName;
                    objHierarchyData.Lastname = objtblContactDetail.LastName;
                    objHierarchyData.SLIIRegNo = objtblContactDetail.SLIIRegNo;
                    objHierarchyData.Mobile1 = Convert.ToInt64(objtblContactDetail.MobileNo1);
                    objHierarchyData.Mobile2 = Convert.ToInt64(objtblContactDetail.MobileNo2);
                    objHierarchyData.OfficePhone = Convert.ToInt64(objtblContactDetail.OfficePhone1);
                    objHierarchyData.ResidencePhone = Convert.ToInt64(objtblContactDetail.ResidenceNo);
                    objHierarchyData.Email = objtblContactDetail.Email;
                    #endregion

                    #region FetchGeoUnits
                    objtblOrgStructure = objtblOrgOffice.tblOrgStructure;
                    if (objtblOrgStructure != null)
                    {
                        var StructureID = objtblOrgStructure.OrgStructureID;
                        if (StructureID != 0)
                        {
                            objHierarchyData.ObjGEOUnitDetails = new List<GEOUnitDetails>();
                            List<tblOrgStructure> objListtblOrgStructure = entities.tblOrgStructures.Where(a => a.ParentID == StructureID).ToList();
                            if (objListtblOrgStructure != null)
                            {
                                foreach (var item in objListtblOrgStructure)
                                {
                                    GEOUnitDetails objGEOUnitDetails = new GEOUnitDetails();
                                    objGEOUnitDetails.Geounitname = item.LevelDefinition;
                                    objGEOUnitDetails.Parententity = item.ParentID.ToString();
                                    objHierarchyData.ObjGEOUnitDetails.Add(objGEOUnitDetails);
                                }
                            }
                        }
                    }
                    #endregion
                    #region FetchHierarchyCoordination
                    objHierarchyData.ObjCoordination = new List<Coordination>();
                    List<tblHierarchyCoordination> objtblHierarchyCoordination = objtblOrgOffice.tblHierarchyCoordinations.ToList();
                    if (objtblHierarchyCoordination != null)
                    {
                        foreach (var item in objtblHierarchyCoordination)
                        {
                            Coordination objHrCoordination = new Coordination();
                            objHrCoordination.CoordinateID = Convert.ToInt32(item.CoordinationID);
                            objHrCoordination.Partner = item.Partner;
                            objHrCoordination.Geounittype = item.UnitType;
                            objHrCoordination.Geounitname = item.Name;
                            objHrCoordination.Code = item.Code;
                            objHierarchyData.ObjCoordination.Add(objHrCoordination);
                        }
                    }
                    #endregion
                }
                return objHierarchyData;
            }
            catch (Exception Ex)
            {
                objHierarchyData.Message = Ex.InnerException.Message;
                return objHierarchyData;
            }
        }

        public string GetReportingName(string ReportingCode, string ReportingEntityTypeId)
        {
            var ReportingName = "";
            if (ReportingEntityTypeId == "483")
            {
                if (ReportingCode != null && ReportingCode != "")
                {
                    ReportingName = (from obj in entities.tblOrganizations
                                     where obj.Code == ReportingCode
                                     select obj.Org_Name).FirstOrDefault();
                }
            }
            else
            {
                if (ReportingCode != null && ReportingCode != "")
                {
                    ReportingName = (from obj in entities.tblOrgOffices
                                     where obj.OfficeCode == ReportingCode
                                     select obj.OfficeName).FirstOrDefault();
                }
            }                      
            return ReportingName;
        }

        public string GetPartnerSubChannel(string ReportingCode)
        {
            string SubChannelOfficeId = "";
            if (ReportingCode != null)
            {
                SubChannelOfficeId = (from obj in entities.tblOrgOffices
                                      where obj.OfficeCode == ReportingCode
                                      select obj.OrgOfficeID).FirstOrDefault().ToString();
            }
            return SubChannelOfficeId;
        }
        public string GetParentId(string ReportingCode)
        {
            string ParentOrgStructureId = "";
            if (ReportingCode != null)
            {
                ParentOrgStructureId = (from obj in entities.tblOrgOffices
                                      where obj.OfficeCode == ReportingCode
                                      select obj.OrgStructureID).FirstOrDefault().ToString();
            }
            return ParentOrgStructureId;
        }        
        public List<MasterListItem> SearchGeoUnitCode(string CoordinationCode)
        {
            List<MasterListItem> records = new List<MasterListItem>();
            if (CoordinationCode != null)
            {
                records = (from obj in entities.tblOrgStructures
                           where obj.LevelDefinition.StartsWith(CoordinationCode)
                           select new MasterListItem
                           {
                               Value = obj.LevelDefinition,
                               ID = (int)obj.OrgStructureID
                           }).ToList();
            }           
            return records;
        }
        public List<HierarchyData> GetGeoUnitName(string CoordinationCode)
        {
            List<HierarchyData> records = new List<HierarchyData>();
            if (CoordinationCode != null)
            {
                records = (from obj in entities.tblOrgStructures
                           where obj.LevelDefinition.StartsWith(CoordinationCode)
                           select new HierarchyData
                           {
                               CoordinationSearchCode = obj.LevelDefinition,
                               CoordinationSearchGeoUnitName = obj.LevelDefinition                          
                           }).ToList();
            }
            return records;
        }
    }
}