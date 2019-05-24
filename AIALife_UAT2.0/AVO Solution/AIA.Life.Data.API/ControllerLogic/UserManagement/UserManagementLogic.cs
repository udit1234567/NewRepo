using AIA.Life.Repository.AIAEntity;
using AIA.Life.Models.Common;
using AIA.Life.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using AIA.CrossCutting;
using System.Security.Cryptography;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.OleDb;
using Microsoft.Win32;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Collections;
using AIA.Life.Integration.Services.EmailandSMS;
using static AIA.Life.Models.UserManagement.Configuration;
using AIA.Life.Data.API.ControllerLogic.Policy;
using AIA.Life.Data.API.ControllerLogic.Common;


namespace AIA.Life.Data.API.ControllerLogic.UserManagement
{
    public class UserManagementLogic
    {
        AVOAIALifeEntities Entities = new AVOAIALifeEntities();
        //MasterController objMasterController = new MasterController();
        private static int DEFAULT_MIN_PASSWORD_LENGTH = 8;
        private static int DEFAULT_MAX_PASSWORD_LENGTH = 10;
        private static string PASSWORD_CHARS_LCASE = "abcdefgijkmnopqrstwxyz";
        private static string PASSWORD_CHARS_UCASE = "ABCDEFGHJKLMNPQRSTWXYZ";
        private static string PASSWORD_CHARS_NUMERIC = "23456789";
        private static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";
        Guid FGLoginUser = Guid.Empty;
        //private ApplicationUserManager _userManager;

        public static string Generate()
        {
            return Generate(DEFAULT_MIN_PASSWORD_LENGTH,
                            DEFAULT_MAX_PASSWORD_LENGTH);
        }
        public static string Generate(int length)
        {
            return Generate(length, length);
        }

        public static string Generate(int minLength, int maxLength)
        {
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;
            char[][] charGroups = new char[][]
            {
            PASSWORD_CHARS_LCASE.ToCharArray(),
            PASSWORD_CHARS_UCASE.ToCharArray(),
            PASSWORD_CHARS_NUMERIC.ToCharArray(),
            PASSWORD_CHARS_SPECIAL.ToCharArray()
            };

            int[] charsLeftInGroup = new int[charGroups.Length];
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;
            int[] leftGroupsOrder = new int[charGroups.Length];
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;
            byte[] randomBytes = new byte[4];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];
            Random random = new Random(seed);
            char[] password = null;
            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];

            int nextCharIdx;
            int nextGroupIdx;
            int nextLeftGroupsOrderIdx;
            int lastCharIdx;
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            for (int i = 0; i < password.Length; i++)
            {
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0,
                                                         lastLeftGroupsOrderIdx);

                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;
                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;

                else
                {
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    charsLeftInGroup[nextGroupIdx]--;
                }
                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                else
                {
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    lastLeftGroupsOrderIdx--;
                }
            }

            return new string(password);
        }

        public List<TreeView> MenuPermissionTree(string AppName, Guid? UserId, string ItemType)
        {
            AVOAIALifeEntities entity = new AVOAIALifeEntities();
            List<TreeView> permissionDatas = new List<TreeView>();
            Guid appID = (from obj in entity.Applications where obj.ApplicationName == AppName select obj.ApplicationId).FirstOrDefault();
            var MenuPerm = entity.USP_GetMenuPermissions(AppName, "", ItemType).ToList();

            //Added To Get Parent User apermission
            var usert = (from objtblUserDetails in Entities.tblUserDetails
                         where objtblUserDetails.UserID == UserId
                         select objtblUserDetails).FirstOrDefault();
            if (usert != null)
            {
                var checklevel = usert.userlevel == "L0" ? 1 : 0;
                while (usert.userlevel != "L0")
                {
                    usert = Entities.tblUserDetails.Where(a => a.NodeID == usert.UserParentId).FirstOrDefault();
                }
                List<int> ParentPermissions = new List<int>();
                ParentPermissions = (from obj in entity.tblUserPermissions
                                     join masPerm in entity.tblMasPermissions on obj.PermissionId equals masPerm.PermissionID
                                     where obj.UserID == usert.UserID && masPerm.ItemType == ItemType //&& obj.IsIndeterminate == false
                                     select obj.PermissionId ?? 0).ToList();
                var parentIndentPerm = (from obj in entity.tblUserPermissions
                                        join masPerm in entity.tblMasPermissions on obj.PermissionId equals masPerm.PermissionID
                                        where obj.UserID == usert.UserID && masPerm.ItemType == ItemType && obj.IsIndeterminate == true
                                        select obj.PermissionId ?? 0).ToList();

                //

                List<int> selectPermissions = new List<int>();
                selectPermissions = (from obj in entity.tblUserPermissions
                                     join masPerm in entity.tblMasPermissions on obj.PermissionId equals masPerm.PermissionID
                                     where obj.UserID == UserId && masPerm.ItemType == ItemType && obj.IsIndeterminate == false
                                     select obj.PermissionId ?? 0).ToList();
                List<int> IndetPerm = new List<int>();
                IndetPerm = (from obj in entity.tblUserPermissions
                             join masPerm in entity.tblMasPermissions on obj.PermissionId equals masPerm.PermissionID
                             where obj.UserID == UserId && masPerm.ItemType == ItemType && obj.IsIndeterminate == true
                             select obj.PermissionId ?? 0).ToList();
                if (checklevel != 1)
                {
                    //to checks parent permissions
                    var parentPerm = (from obj in MenuPerm
                                      join obj1 in ParentPermissions on obj.id equals obj1
                                      orderby obj.ItemID
                                      select new TreeView()
                                      {
                                          ItemId = obj.id ?? 0,
                                          ItemDesc = obj.text1,
                                          Parent = obj.ParentId ?? 0,
                                          depth = obj.Level ?? 0,
                                          IsSelected = selectPermissions.Count == 0 ? true : selectPermissions.Contains(obj.id.Value) ? true : false,
                                          IsIndet = IndetPerm.Count == 0 ? false : IndetPerm.Contains(obj.id.Value) ? true : false//(parentIndentPerm.Contains(obj.id.Value)) ? true : (IndetPerm.Contains(obj.id.Value) ? true : false)//IndetPerm.Count == 0 ? false : IndetPerm.Contains(obj.id.Value) ? true : false
                                      }).ToList();
                    permissionDatas = parentPerm.Select(x => new TreeView()
                    {
                        ItemId = x.ItemId,
                        ItemDesc = x.ItemDesc,
                        Parent = x.Parent,
                        depth = x.depth,
                        IsSelected = x.IsSelected,//selectPermissions.Count == 0 ? true : selectPermissions.Contains(x.ItemId) ? true : false,
                        IsIndet = x.IsIndet//IndetPerm.Count == 0 ? false : IndetPerm.Contains(x.Parent) ? true : false
                    }).OrderBy(o => o.ItemId).ToList();
                }
                else
                {
                    permissionDatas = MenuPerm.Select(x => new TreeView()
                    {
                        ItemId = x.id ?? 0,
                        ItemDesc = x.text1,
                        Parent = x.ParentId ?? 0,
                        depth = x.Level ?? 0,
                        IsSelected = selectPermissions.Count == 0 ? true : selectPermissions.Contains(x.id.Value) ? true : false,
                        IsIndet = IndetPerm.Count == 0 ? false : IndetPerm.Contains(x.id.Value) ? true : false
                    }).OrderBy(o => o.ItemId).ToList();
                }
            }
            else
            {
                List<int> ParentPermissions = new List<int>();
                ParentPermissions = (from obj in entity.tblUserPermissions
                                     join masPerm in entity.tblMasPermissions on obj.PermissionId equals masPerm.PermissionID
                                     where masPerm.ItemType == ItemType //&& obj.IsIndeterminate == false
                                     select obj.PermissionId ?? 0).ToList();


                List<int> IndetPerm = new List<int>();
                IndetPerm = (from obj in entity.tblUserPermissions
                             join masPerm in entity.tblMasPermissions on obj.PermissionId equals masPerm.PermissionID
                             where masPerm.ItemType == ItemType && obj.IsIndeterminate == true
                             select obj.PermissionId ?? 0).ToList();

                permissionDatas = MenuPerm.Select(x => new TreeView()
                {
                    ItemId = x.id ?? 0,
                    ItemDesc = x.text1,
                    Parent = x.ParentId ?? 0,
                    depth = x.Level ?? 0,
                    IsSelected = true,
                    IsIndet = IndetPerm.Count == 0 ? false : IndetPerm.Contains(x.id.Value) ? true : false
                }).OrderBy(o => o.ItemId).ToList();
            }
            return permissionDatas;

        }

        public List<TreeView> PaymentPermissionTree(string AppName, Guid? UserId, string ItemType)
        {
            return new List<TreeView>();
        }



        public List<TreeView> ProductPermissionTree(string AppName, Guid? UserId, string ItemType)
        {
            AVOAIALifeEntities entity = new AVOAIALifeEntities();
            List<TreeView> roleTree = new List<TreeView>();
            List<TreeView> userTree = new List<TreeView>();

            userTree = entity.tblMasPermissions.
                Join(entity.tblUserPermissions.Where(a => a.UserID == UserId),
                masp => masp.PermissionID, userp => userp.PermissionId,
                (masp, userp) => new TreeView()
                {
                    ItemId = masp.PermissionID,
                    ItemDesc = masp.ItemDescription,
                    Parent = masp.ParentId ?? 0,
                    depth = masp.ParentId == 0 ? 0 : 1,
                    IsSelected = true
                }).ToList();
            string roleName = entity.tblMasIMOUsers.Where(a => a.UserID == entity.tblUserDetails.Where(u => u.UserID == UserId).Select(u => u.IMDCode).FirstOrDefault()).Select(a => a.UserRole).FirstOrDefault();
            string roleid = entity.AspNetRoles.Where(a => a.Name == roleName).Select(a => a.Id).FirstOrDefault();
            if (!string.IsNullOrEmpty(roleid))
            {
                roleTree = entity.tblMasPermissions.
                    Join(entity.tblUserPermissions.Where(a => a.RoleId == roleid),
                    masp => masp.PermissionID, userp => userp.PermissionId,
                    (masp, userp) => new TreeView()
                    {
                        ItemId = masp.PermissionID,
                        ItemDesc = masp.ItemDescription,
                        Parent = masp.ParentId ?? 0,
                        depth = masp.ParentId == 0 ? 0 : 1,
                        IsSelected = true
                    }).ToList();
            }
            var selectedPer = userTree.Select(a => a.ItemId).ToList();
            if (userTree.Count() > 0)
            {
                roleTree.ForEach(a => a.IsSelected = (selectedPer.Contains(a.ItemId) == true ? true : false));
            }
            return roleTree;

        }

        public CreateUserModel FetchAdvisorCodeData(string AdvisorCode)
        {
            CreateUserModel objCreateUserModel = new CreateUserModel();
            if (AdvisorCode != null && AdvisorCode != "")
            {
                tblMasIMOUser objtblMasIMOUser = Entities.tblMasIMOUsers.Where(a => a.UserID == AdvisorCode).FirstOrDefault();
                objCreateUserModel.UserFirstName = objtblMasIMOUser.UserName;
                objCreateUserModel.MobileNo = objtblMasIMOUser.MobileNo;
                objCreateUserModel.EmailId = objtblMasIMOUser.Email;
                objCreateUserModel.UserRole = objtblMasIMOUser.UserRole;
                objCreateUserModel.NICNo = objtblMasIMOUser.NIC;
                objCreateUserModel.ReportingBranch = objtblMasIMOUser.Branch;
                objCreateUserModel.Gender = objtblMasIMOUser.Gender;
                objCreateUserModel.ReportingManager = objtblMasIMOUser.ReportingManager;
                objCreateUserModel.LicenseNumber = objtblMasIMOUser.LicenseNo;
                objCreateUserModel.Designation = objtblMasIMOUser.Designation;
            }

            //if (AdvisorCode != null && AdvisorCode != "")
            //{

            //    tblProspect objtblProspect = null;
            //    tblProspectOfficial objtblProspectOfficial = null;
            //    tblProspectInview objtblProspectInview = null;
            //    tblUserDetail objtblUserDetail = null;
            //    AgentonBoardingBusiness objAgentonBoardingBusiness = new AgentonBoardingBusiness();
            //    objtblProspect = Entities.tblProspects.Where(a => a.AgentCode == AdvisorCode).FirstOrDefault();
            //    objtblUserDetail = Entities.tblUserDetails.Where(a => a.LoginID == AdvisorCode).FirstOrDefault();
            //    if (objtblProspect == null)
            //    {
            //        objtblProspect = new tblProspect();
            //        objtblProspect.AgentCode = AdvisorCode;
            //        Entities.tblProspects.Add(objtblProspect);
            //    }
            //    if (objtblUserDetail == null)
            //    {
            //        objCreateUserModel.Parent = true;
            //        objCreateUserModel.Message = "Success";
            //    }
            //    else
            //    {
            objCreateUserModel.Parent = false;
            objCreateUserModel.Message = "Success";
            //    }
            //    objCreateUserModel.AgentName = objtblProspect.FirstName + " " + objtblProspect.LastName;
            //    objCreateUserModel.NICNo = objtblProspect.NIC;
            //    objCreateUserModel.LicenseNumber = objtblProspect.SLIINo;
            //    objCreateUserModel.MobileNo = Convert.ToString(objtblProspect.MobileNo);
            //    objCreateUserModel.officeTelNo = Convert.ToString(objtblProspect.OfficePhNo);
            //    objCreateUserModel.EmailId = objtblProspect.EmailId;
            //    objCreateUserModel.Gender = objtblProspect.Gender;
            //    objCreateUserModel.AdvisorCode = objtblProspect.AgentCode;                
            //    objtblProspectInview = objtblProspect.tblProspectInviews.FirstOrDefault();
            //    if(objtblProspectInview==null)
            //    {
            //        objtblProspectInview = new tblProspectInview();
            //    }
            //    objCreateUserModel.RoleName = objAgentonBoardingBusiness.GetRecruitmentDesignationDetails().Where(a => a.ID == Convert.ToInt32(objtblProspectInview.Designation)).Select(a => a.Value).FirstOrDefault();
            //    objtblProspectOfficial = objtblProspect.tblProspectOfficials.FirstOrDefault();
            //    if (objtblProspectOfficial == null)
            //    {
            //        objtblProspectOfficial = new tblProspectOfficial();
            //    }
            //    objCreateUserModel.ReportingBranch = objtblProspectOfficial.BranchName;
            //    objCreateUserModel.ReportingManager = objtblProspectOfficial.Position;
            //    return objCreateUserModel;
            //}
            //objCreateUserModel.Message = "Please Provide Valid Advisor code";
            return objCreateUserModel;
        }
        public IEnumerable<MasterListItem> GetSalutationsList()
        {
            IEnumerable<MasterListItem> SalutationList = (from obj in Entities.tblMasCommonTypes
                                                          where obj.MasterType == "Salutation"
                                                          select new MasterListItem
                                                          {
                                                              Value = obj.Description,

                                                          });
            return SalutationList;
        }
        //public IEnumerable<SelectListItem> GetIMDType()
        //{
        //    IEnumerable<SelectListItem> ImdType = (from obj in Entities.tblMasIMDs
        //                                           where obj.isValid == true
        //                                           select new SelectListItem
        //                                           {
        //                                               Text = obj.ShortDesc,
        //                                               Value = obj.Value.Trim()
        //                                           });
        //    return ImdType;
        //}

        //public IEnumerable<MasterListItem> GetIMDList()
        //{
        //    IEnumerable<MasterListItem> ImdType = (from obj in Entities.tblImdDetails.Where(a => a.IMDCode != null)
        //                                           where !obj.IMDCode.StartsWith("IMD") && !obj.IMDCode.Contains("$")
        //                                           select new MasterListItem
        //                                           {
        //                                               Value = obj.IMDCode,
        //                                               ID = (int)obj.ImdDetailsID
        //                                           });
        //    return ImdType;
        //}

        public IEnumerable<SelectListItem> GetIMDLevel()
        {
            IEnumerable<SelectListItem> ImdLevel = (from obj in Entities.tblUserDetails
                                                    join obj1 in Entities.tblUserAccounts on obj.UserAccountID equals obj1.UserAccountID
                                                    join obj3 in Entities.tblAddresses on obj.AddressID equals obj3.AddressID

                                                    select new SelectListItem
                                                    {
                                                        Value = obj.Title,
                                                        Text = obj.Title

                                                    });
            return ImdLevel;
        }



        public IEnumerable<SelectListItem> GetOffice()
        {
            IEnumerable<SelectListItem> office = (from obj in Entities.tblMasBranches
                                                  where obj.isValid == true
                                                  select new SelectListItem
                                                  {
                                                      Value = obj.ShortDesc,
                                                      Text = obj.ShortDesc

                                                  });
            return office;
        }

        public string getRandomPasswordS(int PasswordLength)
        {
            string allowedChars = "";
            allowedChars = "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?,";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";

            char[] sep = { ',' };

            string[] arr = allowedChars.Split(sep);

            string passwordString = "";

            string temp = "";

            Random rand = new Random();

            for (int i = 0; i < PasswordLength; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            return passwordString;
        }

        public string getRandomPassword(int PasswordLength)
        {
            String _allowedChars = CrossCuttingConstants.AllowedChars;
            Byte[] randomBytes = new Byte[PasswordLength];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)randomBytes[i] % allowedCharCount];
            }
            return new string(chars);

        }

        public CreateUserModel SaveUserAIA(CreateUserModel objUser)
        {
            Guid Userid = Guid.Parse(objUser.UserIdName);
            tblUserDetail objtblUserDetail = Entities.tblUserDetails.Where(a => a.UserID == Userid).FirstOrDefault();
            if (objtblUserDetail == null)
            {
                objtblUserDetail = new tblUserDetail();
            }
            objtblUserDetail.UserID = Userid;
            objtblUserDetail.FirstName = objUser.UserFirstName;
            objtblUserDetail.CreatedDate = DateTime.Now;
            // objtblUserDetail.LoginID = objUser.UserCode;
            objtblUserDetail.Email = objUser.EmailId;
            objtblUserDetail.Status = true;
            objtblUserDetail.UserCode = objUser.UserCode;
            objtblUserDetail.Gender = Convert.ToInt32(objUser.Gender);
            objtblUserDetail.ContactNo = objUser.MobileNo;
            objtblUserDetail.ContactNo2 = objUser.officeTelNo;
            objtblUserDetail.CreatedBy = FGLoginUser;
            objtblUserDetail.LoginID = objUser.AdvisorCode;
            objtblUserDetail.IMDCode = objUser.AdvisorCode;
            objtblUserDetail.IMDName = objUser.AgentName;
            objtblUserDetail.Title = objUser.Salutation;
            objtblUserDetail.LastPasswordChangedDate = DateTime.Now;
            objtblUserDetail.CreationDate = DateTime.Now;
            if (objUser.Parent)
            {
                tblUserAccount objtblUserAccount = Entities.tblUserAccounts.Where(a => a.UserID == Userid).FirstOrDefault();
                if (objtblUserAccount == null)
                {
                    objtblUserAccount = new tblUserAccount();
                }
                objtblUserDetail.IMDCode = objUser.AdvisorCode;
                objtblUserAccount.UserID = Userid;
                objtblUserDetail.userlevel = "L0";
                objtblUserAccount.CreatedDate = DateTime.Now;
                if (objtblUserAccount.UserAccountID == decimal.Zero)
                {
                    Entities.tblUserAccounts.Add(objtblUserAccount);
                    objtblUserDetail.tblUserAccount = objtblUserAccount;
                }
                tblUserExtension objtblUserExtension = Entities.tblUserExtensions.Where(a => a.UserID == Userid).FirstOrDefault();
                if (objtblUserExtension == null)
                {
                    objtblUserExtension = new tblUserExtension();
                }

                if (objtblUserExtension.UserExtensionID == decimal.Zero)
                {
                    // entity.tblUserExtensions.Add(objtblUserExtension);
                }
            }
            else
            {
                var nodeId = Entities.tblUserDetails.Where(a => a.IMDCode == objUser.AdvisorCode).Select(a => a.NodeID).FirstOrDefault();
                if (nodeId != decimal.Zero)
                {
                    objtblUserDetail.LoginID = objUser.AdvisorCode;
                    objtblUserDetail.userlevel = "L1";
                    objtblUserDetail.UserParentId = Convert.ToInt32(nodeId);
                }
            }
            if (objtblUserDetail.NodeID == decimal.Zero)
                Entities.tblUserDetails.Add(objtblUserDetail);
            Entities.SaveChanges();
            EmailIntegration objEmail = new EmailIntegration();
            // bool emailStatus = objEmail.sendUserActivationEmail(objUser.EmailId, objUser.Password, objUser.UserCode);
            return objUser;
        }

        public IEnumerable<MasterListItem> GetBranchCode()
        {
            IEnumerable<MasterListItem> Branchcode = (from obj in Entities.tblMasBranches
                                                      where obj.isValid == true && obj.Value != "**"
                                                      select new MasterListItem
                                                      {
                                                          Value = obj.ShortDesc,
                                                          ID = (int)obj.BranchId
                                                      }).OrderBy(a => a.Value.Trim());
            return Branchcode;
        }
        public IEnumerable<MasterListItem> GetReceiptingBankCode()
        {
            IEnumerable<MasterListItem> ReceiptingBankCode = (from obj in Entities.tblMasRecieptingBankCodes
                                                              where obj.IsActive == true
                                                              select new MasterListItem
                                                              {
                                                                  Value = obj.LONGDESC,
                                                                  ID = obj.ID
                                                              });
            return ReceiptingBankCode;
        }
        public List<MasterListItem> FetchParentId()
        {
            List<MasterListItem> Parent = new List<MasterListItem>();
            Parent = (from obj in Entities.tblUserDetails
                          //join obj1 in Entities.tblImdDetails on obj.IMDCode equals obj1.ClientCode 
                      where obj.UserCode != null && obj.UserCode != "" && obj.userlevel == "L0"
                      select new MasterListItem
                      {
                          ID = (int)obj.NodeID,
                          Value = obj.UserCode
                      }).OrderBy(a => a.Value).ToList();
            return Parent;
        }
        public List<MasterListItem> GetUserStatus()
        {
            List<MasterListItem> UserStatus = new List<MasterListItem>();
            UserStatus.Add(new MasterListItem { ID = 1, Text = "Active", Value = "Active", selected = 0 });
            UserStatus.Add(new MasterListItem { ID = 0, Text = "InActive", Value = "InActive", selected = 0 });

            return UserStatus;

        }
        public List<MasterListItem> FetchAuthorityLimit()
        {
            List<MasterListItem> Parent = new List<MasterListItem>();
            Parent = (from obj in Entities.tblMasCommonTypes

                      where obj.MasterType == "AuthorityLimit"
                      select new MasterListItem
                      {
                          Text = obj.Description,
                          Value = obj.Description
                      }).ToList();
            return Parent;
        }
        public List<MasterListItem> GetUserRole()
        {
            List<MasterListItem> Parent = new List<MasterListItem>();
            Parent = (from obj in Entities.AspNetRoles
                      where obj.Id != "" || obj.Id != null
                      select new MasterListItem
                      {
                          Text = obj.Name,
                          Value = obj.Name
                      }).ToList();
            return Parent;
        }

        //public IEnumerable<ImdcodeCreationModel> GetIMDUserInfo(string IMDCode, string FirstName, string LastName, string corporateName, string PanNo)
        //{
        //    var IMDList = Entities.tblUserDetails.Where(o => o.IMDCode != null).Select(o => o.IMDCode).ToList();
        //    var records = (from obj in Entities.tblImdDetails.Where(a => !IMDList.Contains(a.IMDCode))

        //                   where (obj.IMDCode == IMDCode || IMDCode == null || IMDCode == "")
        //                            && (obj.FirstName == FirstName || FirstName == null || FirstName == "")
        //                            && (obj.LastName == LastName || LastName == null || LastName == "")
        //                            && (obj.CorpName == corporateName || corporateName == null || corporateName == "")
        //                            && (obj.PAN == PanNo || PanNo == null || PanNo == "")

        //                   select new ImdcodeCreationModel
        //                   {
        //                       Imdcode = obj.IMDCode,
        //                       FirstName = obj.FirstName,
        //                       Lastname = obj.LastName,
        //                       CorporateName = obj.CorpName,
        //                       Pan = obj.PAN,
        //                   }).OrderBy(a => a.Imdcode);

        //    return records;
        //}
        public IEnumerable<IMDUsers> GetUserDetails(string UserName, string catagory, string userType, decimal? agentCode)
        {
            var records = (from obj in Entities.tblUserDetails
                           where obj.UserCode == UserName
                           select new IMDUsers
                           {
                               NodeID = (int)obj.NodeID,
                               IMDType = obj.UserType,
                               IMDCode = obj.IMDCode,
                               IMDName = obj.FirstName,
                               FGBranch = "",
                               DateOfCreation = obj.CreatedDate,
                               IMDStatus = obj.Status == true ? "Active" : "InActive"
                           }).OrderBy(a => a.NodeID);
            return records;
        }
        public IEnumerable<IMDUsers> FetchGridUserDetails(CreateUserModel CreateUser)
        {

            if (CreateUser.NICNo != null && CreateUser.NICNo != "")
            {
                CreateUser.IMDCode = (from obj in Entities.tblProspects
                                      where obj.NIC == CreateUser.NICNo
                                      select obj.AgentCode).FirstOrDefault();
            }
            var FetchDetails = (from obj in Entities.tblUserDetails
                                where (obj.IMDCode == CreateUser.IMDCode && CreateUser.IMDCode != null && CreateUser.IMDCode != "" && CreateUser.IMDCode != "null")
                                || (obj.UserCode == CreateUser.UserCode && CreateUser.UserCode != null && CreateUser.UserCode != "" && CreateUser.UserCode != "null")
                                select new IMDUsers
                                {
                                    NodeID = (int)obj.NodeID,
                                    IMDCode = obj.IMDCode,
                                    UserCode = obj.UserCode,
                                    UserIdName = obj.LoginID,
                                    IMDStatus = obj.Status == true ? "Active" : "InActive"
                                }).ToList();
            List<IMDUsers> ChildFetchDetails = new List<IMDUsers>();
            List<IMDUsers> ChildCurrentFetchDetails = new List<IMDUsers>();
            foreach (var item in FetchDetails)
            {

                ChildFetchDetails = (from obj in Entities.tblUserDetails
                                     where obj.UserParentId == item.NodeID
                                     select new IMDUsers
                                     {
                                         NodeID = (int)obj.NodeID,
                                         IMDCode = obj.IMDCode,
                                         UserCode = obj.UserCode,
                                         UserIdName = obj.LoginID,
                                         IMDStatus = obj.Status == true ? "Active" : "InActive"
                                     }).ToList();
                //ChildCurrentFetchDetails.AddRange(ChildFetchDetails);
            }
            FetchDetails.AddRange(ChildFetchDetails);
            return FetchDetails;
        }

        //    if (CreateUser.UserIdName == "superadmin")
        //    {
        //        return new List<IMDUsers>();
        //    }
        //if (CreateUser.IMDCode != "" && CreateUser.IMDCode != null && CreateUser.IMDCode != null && CreateUser.IMDCode != "null")
        //{
        //    var FetchDetails = (from obj in Entities.tblUserDetails
        //                        join obj1 in Entities.tblUserDetails on obj.UserAccountID equals obj1.UserAccountID
        //                        join objUserBranch in Entities.tblUserBranchMappings on obj1.NodeID equals objUserBranch.UserDetailsId
        //                        join objmasBranch in Entities.tblMasBranches on objUserBranch.FGBranchCode.Trim() equals objmasBranch.BranchId
        //                        where (obj.IMDCode == CreateUser.UserCode || CreateUser.UserCode == null || CreateUser.UserCode == "" || CreateUser.UserCode == "null")
        //                          && (obj1.UserCode == CreateUser.NICNo || CreateUser.NICNo == null || CreateUser.NICNo == "" || CreateUser.NICNo == "null")
        //                          && (obj1.LoginID == CreateUser.UserIdName || CreateUser.UserIdName == null || CreateUser.UserIdName == "" || CreateUser.UserIdName == "null")
        //                          && (objUserBranch.FGBranchCode == CreateUser.branhCode || CreateUser.branhCode == null || CreateUser.branhCode == "" || CreateUser.branhCode == "null")
        //                          && !obj.IMDCode.Contains("IMD")
        //                          && !obj.UserCode.Contains("IMD")

        //                        select new IMDUsers
        //                        {
        //                            NodeID = obj1.NodeID,
        //                            IMDCode = obj.IMDCode,
        //                            UserCode = obj1.UserCode,
        //                            UserIdName = obj1.LoginID,
        //                            // FGBranch = objmasBranch.ShortDesc,
        //                            //DateOfCreation = obj.CreatedDate,
        //                            IMDStatus = obj1.Status == true ? "Active" : "InActive"
        //                        }).Distinct().OrderByDescending(a => a.NodeID);
        //    return FetchDetails;
        //}

        //var FetchUserDetails = (from obj in Entities.tblUserDetails
        //                        join objUserBranch in Entities.tblUserBranchMappings on obj.NodeID equals objUserBranch.UserDetailsId
        //                        join objmasBranch in Entities.tblMasBranches on objUserBranch.FGBranchCode.Trim() equals SqlFunctions.StringConvert((decimal)objmasBranch.BranchId).Trim()
        //                        where (obj.IMDCode == CreateUser.IMDCode || CreateUser.IMDCode == null || CreateUser.IMDCode == "" || CreateUser.IMDCode == "null")
        //                          && (obj.UserCode == CreateUser.UserCode || CreateUser.UserCode == null || CreateUser.UserCode == "" || CreateUser.UserCode == "null")
        //                          && (obj.LoginID == CreateUser.UserIdName || CreateUser.UserIdName == null || CreateUser.UserIdName == "" || CreateUser.UserIdName == "null")
        //                          && (objUserBranch.FGBranchCode == CreateUser.branhCode || CreateUser.branhCode == null || CreateUser.branhCode == "" || CreateUser.branhCode == "null")
        //                          //&& !(obj.IMDCode.Contains("IMD"))
        //                          && !(obj.UserCode.Contains("IMD"))

        //                        select new IMDUsers
        //                        {
        //                            NodeID = obj.NodeID,
        //                            IMDCode = Entities.tblUserDetails.Where(c => c.UserAccountID == obj.UserAccountID && c.userlevel == "L0").Select(c => c.IMDCode).FirstOrDefault(),      //obj.IMDCode,
        //                            UserCode = obj.UserCode,
        //                            UserIdName = obj.LoginID,

        //                            ListBranchData = (from objuserbranchdet in Entities.tblUserBranchMappings
        //                                              join objmasBranchdet in Entities.tblMasBranches on objuserbranchdet.FGBranchCode.Trim() equals SqlFunctions.StringConvert((decimal)objmasBranchdet.BranchId).Trim()
        //                                              where objuserbranchdet.UserDetailsId == obj.NodeID
        //                                              select new MasterListItem
        //                                              {
        //                                                  ID = (int)objmasBranchdet.BranchId,
        //                                                  Value = objmasBranchdet.ShortDesc
        //                                              }).AsEnumerable()
        //                                        ,
        //                            //DateOfCreation = obj.CreatedDate,
        //                            IMDStatus = obj.Status == true ? "Active" : "InActive"
        //                        }).DistinctBy(a => a.NodeID).OrderByDescending(a => a.n).ToList();


        //var GridUserDetailsTemp = FetchUserDetails.Select(objImdbranchList => new IMDUsers
        //{
        //    NodeID = objImdbranchList.NodeID,
        //    IMDCode = objImdbranchList.IMDCode,
        //    UserCode = objImdbranchList.UserCode,
        //    UserIdName = objImdbranchList.UserIdName,
        //    FGBranch = string.Join(",", objImdbranchList.ListBranchData.Select(a => a.Value)),//objImdbranchList.ListBranchData.Aggregate("",(m,n)=>m+","+n)
        //    IMDStatus = objImdbranchList.IMDStatus
        //}).OrderBy(a => a.NodeID).Where(a => a.IMDCode != "").OrderByDescending(a => a.NodeID);


        //    return GridUserDetailsTemp;
        //}
        //public void FillCommissionDetails(CommissionDetails objcommissionDetails, tblImdComissionDetail objtblImdcomissiondetail)
        //{
        //    objtblImdcomissiondetail.PremiumClass = objcommissionDetails.premiumclass;
        //    objtblImdcomissiondetail.ContractType = objcommissionDetails.ContractType;
        //    objtblImdcomissiondetail.Comission = objcommissionDetails.Commission;
        //}
        //public void FillBranchDetails(BranchDetails objbranchdetails, tblIMDBranchDetail objtblimdbranchdetail)
        //{
        //    objtblimdbranchdetail.BranchCode = objbranchdetails.BranchCode;
        //    objtblimdbranchdetail.EmployeeCode = objbranchdetails.EmployeeCode;
        //    objtblimdbranchdetail.SMCode = objbranchdetails.SmCode;
        //}                 
        public CreateUserModel FetchUserDetails(CreateUserModel objuser, int? id)
        {
            {
                int i = 0;
                //decimal nodeId;
                //decimal.TryParse(UserId, out nodeId);
                objuser.imdusers = new IMDUsers();
                tblUserDetail objtblUserDetail = new tblUserDetail();
                //tblUserBranchMapping objtblUserBranchMapping = new tblUserBranchMapping();
                tblUserPhoto objtblUserPhoto = new tblUserPhoto();
                objtblUserDetail = Entities.tblUserDetails.FirstOrDefault(a => a.IMDCode == objuser.AdvisorCode);
                if (objtblUserDetail != null)
                {
                    if (objtblUserDetail.UserParentId == null)
                    {
                        objuser.Parent = true;
                        objuser.IMDCode = objtblUserDetail.IMDCode;
                        objuser.imdusers.IMDName = objtblUserDetail.IMDName;
                    }
                    //else if (objtblUserDetail.UserParentId != null && objtblUserDetail.userlevel == "L1")
                    //{
                    //    objtblUserDetail.IMDCode = objtblUserDetail.IMDCode.Remove(objtblUserDetail.IMDCode.Length - 4, 4);
                    //}
                    objuser.UserID = (Guid)objtblUserDetail.UserID;
                    objuser.userDetailsID = objtblUserDetail.NodeID;
                    objuser.UserIdName = objtblUserDetail.LoginID;
                    objuser.UserCode = objtblUserDetail.UserCode;
                    objuser.AdvisorCode = objtblUserDetail.IMDCode;
                    //objuser.text = Convert.ToString((from objImd in Entities.tblImdDetails
                    //                                 join objuser1 in Entities.tblUserDetails on objImd.IMDCode equals objuser1.IMDCode
                    //                                 join objusertemp in Entities.tblUserDetails on objuser1.UserAccountID equals objusertemp.UserAccountID
                    //                                 where objusertemp.NodeID == id
                    //                                 select objImd.IMDTypeID).FirstOrDefault());

                    // Convert.ToString(Entities.tblImdDetails.Where(a => a.IMDCode == Entities.tblUserDetails.Where(b=>b.NodeID==id).Select(b=>b.IMDCode).FirstOrDefault()).Select(a => a.IMDTypeID).FirstOrDefault());
                    if (objuser.Parent == false)
                    {
                        objuser.ReportingUserId = objtblUserDetail.UserParentId.ToString().ToInt();
                        objuser.ReportingUserName = Entities.tblUserDetails.Where(o => o.NodeID == objuser.ReportingUserId).FirstOrDefault().LoginID;
                    }
                    objuser.officeTelNo = objtblUserDetail.ContactNo2;
                    objuser.MobileNo = objtblUserDetail.ContactNo;
                    objuser.EmailId = objtblUserDetail.Email;
                    objuser.Secret_Question = objtblUserDetail.PasswordQuestionID.ToString();
                    objuser.Secret_Answer = objtblUserDetail.PasswordAnswer;
                    objuser.LicenseNumber = (from obj in Entities.tblProspects
                                             where obj.AgentCode == objtblUserDetail.IMDCode
                                             select obj.SLIINo).FirstOrDefault();
                    objuser.AgentName = objtblUserDetail.IMDName;
                    objuser.UserFirstName = objtblUserDetail.FirstName;
                    objuser.NICNo = (from obj in Entities.tblProspects
                                     where obj.AgentCode == objtblUserDetail.IMDCode
                                     select obj.NIC).FirstOrDefault();
                    objuser.ReportingBranch = (from obj in Entities.tblProspects
                                               join obj1 in Entities.tblProspectOfficials
                                               on obj.ProspectID equals obj1.ProspectID
                                               where obj.AgentCode == objtblUserDetail.IMDCode
                                               select obj1.BranchName).FirstOrDefault();
                    objuser.ReportingManager = (from obj in Entities.tblProspects
                                                join obj1 in Entities.tblProspectOfficials
                                                on obj.ProspectID equals obj1.ProspectID
                                                where obj.AgentCode == objtblUserDetail.IMDCode
                                                select obj1.Position).FirstOrDefault();
                    objuser.Gender = Convert.ToString(objtblUserDetail.Gender);
                    objuser.Salutation = objtblUserDetail.Title;
                    //var mappingDetails = (from obj in Entities.tblUserBranchMappings
                    //                      where obj.UserDetailsId == objtblUserDetail.NodeID
                    //                      select new UserBranchMapping
                    //                      {
                    //                          UserDetailsId = obj.UserDetailsId,
                    //                          FGBranchCode = string.IsNullOrEmpty(obj.FGBranchCode) ? " " : obj.FGBranchCode,
                    //                          FGBancaBranchCode = string.IsNullOrEmpty(obj.FGBancaBranchCode) ? " " : obj.FGBancaBranchCode,
                    //                          FGBancaBranchDescription = string.IsNullOrEmpty(obj.FGBancaBranchDescription) ? " " : obj.FGBancaBranchDescription,
                    //                          Receipting = string.IsNullOrEmpty(obj.Receipting) ? " " : obj.Receipting,
                    //                          AreaCode = string.IsNullOrEmpty(obj.AreaCode) ? " " : obj.AreaCode


                    //                      });
                    //if (mappingDetails.Count() != 0)
                    //{
                    //    IEnumerable<UserBranchMapping> gridDetails = (from obj in mappingDetails.AsEnumerable()
                    //                                                  select new UserBranchMapping
                    //                                                  {
                    //                                                      UserDetailsId = obj.UserDetailsId,
                    //                                                      FGBranchCode = obj.FGBranchCode,
                    //                                                      FGBancaBranchCode = obj.FGBancaBranchCode,
                    //                                                      FGBancaBranchDescription = obj.FGBancaBranchDescription,
                    //                                                      Receipting = obj.Receipting,
                    //                                                      AreaCode = obj.AreaCode,
                    //                                                      Index = Convert.ToString(++i)

                    //                                                  }).ToList();
                    //    objuser.FgbranchDetails = gridDetails.OrderBy(a => a.UserDetailsId);
                    //    // added Gowthami Fetching Saved Branch realted Areacode and Receipting 21-06-2016
                    //    foreach (UserBranchMapping objUserBranchMapping in objuser.FgbranchDetails)
                    //    {
                    //        if (!string.IsNullOrEmpty(objUserBranchMapping.FGBranchCode))
                    //        {
                    //            decimal BranchId = Convert.ToDecimal(objUserBranchMapping.FGBranchCode);
                    //            objUserBranchMapping.listAreaCode = GetAreaCodeBranch(BranchId);
                    //        }
                    //        if (!string.IsNullOrEmpty(objUserBranchMapping.AreaCode))
                    //        {
                    //            int AreaId = Convert.ToInt32(objUserBranchMapping.AreaCode);
                    //            // objUserBranchMapping.listReceipting = GetReceiptingAreaCode(AreaId);
                    //        }
                    //    }

                    //}
                    //objtblUserPhoto = Entities.tblUserPhotoes.FirstOrDefault(a => a.UserDetailsID == objtblUserDetail.NodeID);
                    //if (objtblUserPhoto != null)
                    //{
                    //    objuser.userPhotoDetails = new UserPhoto();
                    //    objuser.UserPhotoId = Convert.ToInt32(objtblUserPhoto.UserPhotoID);
                    //    objuser.userPhotoDetails.FileName = objtblUserPhoto.FileName;
                    //    objuser.userPhotoDetails.FileContent = objtblUserPhoto.FileContent;
                    //    objuser.userPhotoDetails.ContentType = objtblUserPhoto.ContentType;
                    //}
                }
            }
            return objuser;
        }

        //public IEnumerable<MasterListItem> GetAreaCodeBranch(decimal FGBranch)
        //{
        //    try
        //    {
        //        string areadesc = Entities.tblMasBranches.Where(a => a.BranchId == FGBranch).Select(a => a.ShortDesc).FirstOrDefault();
        //        IEnumerable<MasterListItem> AreaDetails = (from obj in Entities.tblMasAreaCodes
        //                                                   where obj.IsActive == true && obj.ITEM.Substring(0, 2) == areadesc.Substring(0, 2)
        //                                                   select new MasterListItem
        //                                                   {
        //                                                       Value = obj.SHORTDESC,
        //                                                       ID = obj.ID
        //                                                   });
        //        return AreaDetails;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}
        public List<MasterListItem> FetchSecretQuestion()
        {
            List<MasterListItem> secretQuestion = new List<MasterListItem>();
            secretQuestion = (from obj in Entities.tblMasPasswordQuestions
                              select new MasterListItem
                              {
                                  ID = (int)obj.MasPasswordQuestionID,
                                  Value = obj.Question
                              }).ToList();
            return secretQuestion;
        }

        public string IncreaseUserLevel(string userlevel)
        {
            string substring = userlevel.Substring(1, userlevel.Length - 1);
            int increment;
            int.TryParse(substring, out increment);
            increment++;
            string result = Convert.ToString(increment);
            result = "L" + result;
            return result;
        }

        public void DeleteCorrespondingProductPermission(int menuId, string RoleID)
        {
            string userName = (from obj in Entities.tblUserDetails where obj.UserID.ToString() == RoleID select obj.UserCode).FirstOrDefault();

            //fetch corresponding product permission id
            int productPermId = (from obj in Entities.tblMasPermissions
                                 join obj1 in Entities.Applications on obj.AppID equals obj1.ApplicationId
                                 where obj.MenuID == menuId && obj.IsDeleted == false
                                 && obj.ItemType == CrossCuttingConstants.itemTypeProduct && obj1.ApplicationName == "IMD"
                                 select obj.PermissionID).FirstOrDefault();

            //fetch all the sub level permission id's for this id

            List<int> output = new List<int>();
            output = Entities.USP_GetPermissionsInHierarchy(productPermId, "").Select(x => x.Value).ToList();
            foreach (var permId in output)
            {
                tblUserPermission userProdPerObj = new tblUserPermission();
                userProdPerObj =
                Entities.tblUserPermissions.Where(o => o.PermissionId == permId && o.UserID.ToString() == RoleID).FirstOrDefault();
                if (userProdPerObj != null)
                    Entities.tblUserPermissions.Remove(userProdPerObj);
                Entities.SaveChanges();
            }

            //entity.SaveChanges();
        }

        public void InsertToTabletblUserProductPermission(Guid userid, int permissionId, int? productId, string itemType, string pageName)
        {
            tblUserProductPermission userProdPerObj = new tblUserProductPermission();
            userProdPerObj.PermissionID = permissionId;
            if (pageName == CrossCuttingConstants.PageNameUsers)
            {
                userProdPerObj.UserID = userid;
                //userProdPerObj.RoleId = null;
            }
            else
            {
                //userProdPerObj.RoleId = userid;
                userProdPerObj.UserID = null;
            }
            int? parentId = 0;
            int tempId = permissionId;
            while (itemType != CrossCuttingConstants.itemTypeProduct)
            {
                parentId = (from obj in Entities.tblMasPermissions where obj.PermissionID == tempId select obj.ParentId).FirstOrDefault();
                itemType = (from obj in Entities.tblMasPermissions where obj.PermissionID == parentId select obj.ItemType).FirstOrDefault();
                productId = (from obj in Entities.tblMasPermissions where obj.PermissionID == parentId select obj.ItemID ?? 0).FirstOrDefault().ToString().ToInt();
                tempId = parentId ?? 0;
            }
            if (productId == 0)
                userProdPerObj.ProductID = null;
            else
                userProdPerObj.ProductID = productId;
            Entities.tblUserProductPermissions.Add(userProdPerObj);
            string username = (from obj in Entities.tblUserDetails where obj.UserID == userid select obj.LoginID).FirstOrDefault();
            //entity.SaveChanges("Modify Product Permissions", username);
            Entities.SaveChanges();
        }


        public bool SavePermission(List<int?> permissionIDs, List<int?> IndetPerm, string RoleName, string permissionType, string IMEINum, bool SelfInsp, bool RaiseInsp, bool Recommendation)
        {
            bool successMsg = false;
            string userName = (from obj in Entities.AspNetRoles where obj.Name == RoleName select obj.Id).FirstOrDefault();
            //Guid irpasLoginUser;
            tblUserPermission userPerObj = new tblUserPermission();
            List<int> PermIds = new List<int>();
            if (permissionIDs != null)
                PermIds = permissionIDs.Select(a => a.HasValue ? a.Value : 0).ToList();
            List<int> IndetPermIds = new List<int>();
            if (IndetPerm != null)
                IndetPermIds = IndetPerm.Select(a => a.HasValue ? a.Value : 0).ToList();
            var Role = Entities.AspNetRoles.FirstOrDefault(a => a.Name == RoleName);
            var RoleObj1 = Entities.AspNetRoles.Where(o => o.Name == RoleName).FirstOrDefault();
            #region fetch existing permissions
            List<int> SavedPermissionIds = new List<int>();
            List<int> SavedIndetPermIds = new List<int>();

            if (permissionType == CrossCuttingConstants.itemTypeMenu)
            {
                SavedPermissionIds = (from permissions in Entities.tblUserPermissions
                                      where permissions.UserID.ToString() == Role.Id && permissions.tblMasPermission.ItemType == CrossCuttingConstants.itemTypeMenu
                                      && permissions.IsIndeterminate == false
                                      select permissions.PermissionId ?? 0).ToList();
                SavedIndetPermIds = (from permissions in Entities.tblUserPermissions
                                     where permissions.UserID.ToString() == Role.Id && permissions.tblMasPermission.ItemType == CrossCuttingConstants.itemTypeMenu
                                     && permissions.IsIndeterminate == true
                                     select permissions.PermissionId ?? 0).ToList();

            }
            else if (permissionType == CrossCuttingConstants.itemTypePayment)
            {
                SavedPermissionIds = (from permissions in Entities.tblUserPermissions
                                      where permissions.UserID.ToString() == Role.Id && permissions.tblMasPermission.ItemType == CrossCuttingConstants.itemTypePayment
                                      && permissions.IsIndeterminate == false
                                      select permissions.PermissionId ?? 0).ToList();
                SavedIndetPermIds = (from permissions in Entities.tblUserPermissions
                                     where permissions.UserID.ToString() == Role.Id && permissions.tblMasPermission.ItemType == CrossCuttingConstants.itemTypePayment
                                     && permissions.IsIndeterminate == true
                                     select permissions.PermissionId ?? 0).ToList();
            }

            else if (permissionType == CrossCuttingConstants.itemTypeProduct)
            {
                SavedPermissionIds = (from permissions in Entities.tblUserPermissions
                                      where permissions.UserID.ToString() == Role.Id && permissions.IsIndeterminate == false
                                      && permissions.tblMasPermission.ItemType != CrossCuttingConstants.itemTypePayment && permissions.tblMasPermission.ItemType != CrossCuttingConstants.itemTypeMenu
                                      select permissions.PermissionId ?? 0).ToList();
                SavedIndetPermIds = (from permissions in Entities.tblUserPermissions
                                     where permissions.UserID.ToString() == Role.Id && permissions.IsIndeterminate == true
                                     && permissions.tblMasPermission.ItemType != CrossCuttingConstants.itemTypePayment && permissions.tblMasPermission.ItemType != CrossCuttingConstants.itemTypeMenu
                                     select permissions.PermissionId ?? 0).ToList();
            }
            else
            {
                // permission type  is not menu/ product -not an error
            }
            #endregion
            List<int> commonPermissionId = SavedPermissionIds.Intersect(PermIds).ToList();
            List<int> commonIndetPermissionId = SavedIndetPermIds.Intersect(IndetPermIds).ToList();
            #region delete extra permissions
            List<int> oldPermission = SavedPermissionIds.Except(commonPermissionId).ToList();
            List<int> oldIndetPermission = SavedIndetPermIds.Except(commonIndetPermissionId).ToList();

            foreach (var i in oldPermission)
            {
                userPerObj = new tblUserPermission();
                userPerObj = Entities.tblUserPermissions.Where(o => o.PermissionId == i && o.UserID.ToString() == Role.Id).FirstOrDefault();
                Entities.tblUserPermissions.Remove(userPerObj);
                Entities.SaveChanges();
                if (permissionType == CrossCuttingConstants.itemTypeMenu)
                {
                    DeleteCorrespondingProductPermission(i, Role.Id);
                    Entities.SaveChanges();
                }

            }

            foreach (var i in oldIndetPermission)
            {
                userPerObj = new tblUserPermission();
                userPerObj = Entities.tblUserPermissions.Where(o => o.PermissionId == i && o.UserID.ToString() == Role.Id).FirstOrDefault();
                Entities.tblUserPermissions.Remove(userPerObj);
                Entities.SaveChanges();
                if (permissionType == CrossCuttingConstants.itemTypeMenu)
                {
                    DeleteCorrespondingProductPermission(i, Role.Id);
                    Entities.SaveChanges();
                }
            }
            #endregion
            #region add new permissions
            List<int> newPermission = PermIds.Except(commonPermissionId).ToList();
            List<int> newIndetPerm = IndetPermIds.Except(commonIndetPermissionId).ToList();
            List<int> savedPermissionList = new List<int>();
            savedPermissionList = SavedPermissionIds;
            foreach (var i in newPermission)
            {
                userPerObj = new tblUserPermission();
                if (!savedPermissionList.Contains(i) || oldIndetPermission.Contains(i))
                {
                    userPerObj = new tblUserPermission();
                    userPerObj.PermissionId = i;
                    userPerObj.IsIndeterminate = false;
                    userPerObj.UserID = new Guid(Role.Id);
                    userPerObj.RoleId = Role.Id;
                    userPerObj.CreatedBy = Convert.ToString(RoleObj1.Id);
                    userPerObj.CreatedDate = DateTime.Now;
                    Entities.tblUserPermissions.Add(userPerObj);
                    Entities.SaveChanges();
                    savedPermissionList.Add(i);
                }
            }
            foreach (var i in newIndetPerm)
            {
                userPerObj = new tblUserPermission();
                if (!savedPermissionList.Contains(i) || oldPermission.Contains(i))
                {
                    userPerObj = new tblUserPermission();
                    userPerObj.PermissionId = i;
                    userPerObj.IsIndeterminate = true;
                    userPerObj.UserID = new Guid(Role.Id);
                    userPerObj.RoleId = Role.Id;
                    userPerObj.CreatedBy = Convert.ToString(RoleObj1.Id);
                    userPerObj.CreatedDate = DateTime.Now;
                    Entities.tblUserPermissions.Add(userPerObj);
                    Entities.SaveChanges();
                    savedPermissionList.Add(i);
                }
            }
            successMsg = true;
            #endregion
            #region Save IMEI Number to tbluserdetails

            //string AppName = (from obj in Entities.Users where obj.UserId == new Guid(Role.Id) select obj.Application.ApplicationName).FirstOrDefault();
            //if (AppName != "Employee")
            //{
            //    tblUserDetail users = new tblUserDetail();
            //    users = Entities.tblUserDetails.Where(a => a.UserID == new Guid(Role.Id)).FirstOrDefault();
            //    users.IMEINumber = IMEINum;
            //}
            //var saveStatus = Entities.SaveChanges();
            //if (saveStatus > 0)
            //{
            //    successMsg = true;
            //}

            #endregion
            #region Child user delete permission
            List<int> childuser = new List<int>();

            var nodetemp = (int)Entities.tblUserDetails.Where(a => a.UserID == new Guid(Role.Id)).Select(a => a.NodeID).FirstOrDefault();
            var UserLevel = Entities.tblUserDetails.Where(a => a.UserID == new Guid(Role.Id)).Select(a => a.userlevel).FirstOrDefault();
            if (UserLevel == "L0")
            {
                if (nodetemp != 0)
                {
                    CheckAllId(nodetemp, childuser);
                }
                List<int> deletepermissionsub = oldPermission.Union(oldIndetPermission).Except(newIndetPerm.Union(newPermission)).ToList();
                foreach (var i in childuser)
                {
                    foreach (var item in deletepermissionsub)
                    {
                        userPerObj = new tblUserPermission();
                        userPerObj = Entities.tblUserPermissions.Where(o => o.PermissionId == item && o.UserID == Entities.tblUserDetails.Where(b => b.NodeID == i).Select(b => b.UserID).FirstOrDefault()).FirstOrDefault();
                        if (userPerObj != null)
                        {
                            Entities.tblUserPermissions.Remove(userPerObj);
                            Entities.SaveChanges();
                        }
                    }
                }
            }


            #endregion
            //SaveInspPermDetails(RoleName, SelfInsp, RaiseInsp, Recommendation);// to save inspection permissions of user
            return successMsg;
        }
        public void CheckAllId(int nodetemp, List<int> ChildUser)
        {
            var child = Entities.tblUserDetails.Where(a => a.UserParentId == nodetemp).Select(a => a.NodeID).ToList();
            foreach (var i in child)
            {
                ChildUser.Add(Convert.ToInt32(i));
                CheckAllId(Convert.ToInt32(i), ChildUser);
            }
        }
        public void FindIMDCode(CreateUserModel data)
        {
            int? parentId = null;
            var result = (from obj in Entities.tblUserDetails where obj.NodeID == data.ReportingUserId select obj).FirstOrDefault();
            parentId = result.UserParentId;
            data.IMDCode = result.IMDCode;

            while (parentId != null)
            {
                var userDetails = Entities.tblUserDetails.Where(o => o.NodeID == parentId).FirstOrDefault();
                parentId = userDetails.UserParentId;
                data.IMDCode = userDetails.IMDCode;
            }
        }
        //public IEnumerable<checkBoxListValues> GetMakeModel(int productID, Guid? UserId)
        //{


        //    List<checkBoxListValues> lstMakeModel = new List<checkBoxListValues>();
        //    List<checkBoxListValues> lstTempMasterList = new List<checkBoxListValues>();
        //    List<checkBoxListValues> ParentFetch = new List<checkBoxListValues>();
        //    if (productID == 6 || productID == 13)
        //    {

        //        lstTempMasterList = (from objMasMakeModel in Entities.tblMasModels
        //                             join objProductPermission in Entities.tblUserProductPermissionDetails.Where(a => a.PermissionType == "MAKEMODEL" && a.UserID == UserId && a.ProductID == productID) on new { ProductID = productID, MAKEMODELID = objMasMakeModel.Model_ID_PK } equals new { ProductID = (int)objProductPermission.ProductID, MAKEMODELID = objProductPermission.PermissionTypeId.Value } into ProductPermission
        //                             from objProductPremission in ProductPermission.DefaultIfEmpty()
        //                                 //   where objMasProduct.Product_ID_PK == productID
        //                             where objMasMakeModel.RskType.Trim() == "FPC"
        //                             && objMasMakeModel.Body_Type.Trim() == "RICKSH"

        //                             //where objMasMakeModel.RSKTYPE == "FPV"
        //                             select new checkBoxListValues
        //                             {
        //                                 ChkId = objMasMakeModel.Model_ID_PK.ToString(),
        //                                 ChkName = objMasMakeModel.Variance,
        //                                 SelectedValue = objProductPremission.ProductPermDetID == null ? false : true
        //                             }
        //                       ).DistinctBy(a => a.ChkId).ToList();
        //    }
        //    else
        //    {
        //        lstTempMasterList = (from objMasMakeModel in Entities.tblMasModels
        //                             join objMasProduct in Entities.tblMasProducts on objMasMakeModel.RskType equals objMasProduct.RiskType into MasProducts
        //                             from objMasProduct in MasProducts.DefaultIfEmpty()
        //                             join objProductPermission in Entities.tblUserProductPermissionDetails.Where(a => a.PermissionType == "MAKEMODEL" && a.UserID == UserId && a.ProductID == productID) on new { ProductID = objMasProduct.Product_ID_PK, MAKEMODELID = objMasMakeModel.Model_ID_PK } equals new { ProductID = (int)objProductPermission.ProductID, MAKEMODELID = objProductPermission.PermissionTypeId.Value } into ProductPermission
        //                             from objProductPremission in ProductPermission.DefaultIfEmpty()
        //                             where objMasProduct.Product_ID_PK == productID


        //                             //where objMasMakeModel.RSKTYPE == "FPV"
        //                             select new checkBoxListValues
        //                             {
        //                                 ChkId = objMasMakeModel.Model_ID_PK.ToString(),
        //                                 ChkName = objMasMakeModel.Variance,
        //                                 SelectedValue = objProductPremission.ProductPermDetID == null ? false : true
        //                             }
        //                       ).DistinctBy(a => a.ChkId).ToList();
        //    }
        //    //added code to get Parent permission
        //    var usert = (from objtblUserDetails in Entities.tblUserDetails
        //                 where objtblUserDetails.UserID == UserId
        //                 select objtblUserDetails).FirstOrDefault();
        //    var checklevel = usert.userlevel == "L0" ? 1 : 0;
        //    while (usert.userlevel != "L0")
        //    {
        //        usert = Entities.tblUserDetails.Where(a => a.NodeID == usert.UserParentId).FirstOrDefault();
        //    }
        //    if (productID == 6 || productID == 13)
        //    {
        //        ParentFetch = (from objMasMakeModel in Entities.tblMasModels

        //                       join objProductPermission in Entities.tblUserProductPermissionDetails.Where(a => a.PermissionType == "MAKEMODEL" && a.UserID == usert.UserID && a.ProductID == productID) on new { ProductID = productID, MAKEMODELID = objMasMakeModel.Model_ID_PK } equals new { ProductID = (int)objProductPermission.ProductID, MAKEMODELID = objProductPermission.PermissionTypeId.Value } into ProductPermission
        //                       from objProductPremission in ProductPermission.DefaultIfEmpty()
        //                           // where objMasProduct.Product_ID_PK == productID

        //                       where objMasMakeModel.RskType == "FPC"
        //                        && objMasMakeModel.Body_Type == "RICKSH"

        //                       //where objMasMakeModel.RSKTYPE == "FPV"
        //                       select new checkBoxListValues
        //                       {
        //                           ChkId = objMasMakeModel.Model_ID_PK.ToString(),
        //                           ChkName = objMasMakeModel.Variance,
        //                           SelectedValue = objProductPremission.ProductPermDetID == null ? false : true
        //                       }
        //                     ).DistinctBy(a => a.ChkId).ToList();
        //    }
        //    else
        //    {
        //        ParentFetch = (from objMasMakeModel in Entities.tblMasModels
        //                       join objMasProduct in Entities.tblMasProducts on objMasMakeModel.RskType equals objMasProduct.RiskType into MasProducts
        //                       from objMasProduct in MasProducts.DefaultIfEmpty()
        //                       join objProductPermission in Entities.tblUserProductPermissionDetails.Where(a => a.PermissionType == "MAKEMODEL" && a.UserID == usert.UserID && a.ProductID == productID) on new { ProductID = objMasProduct.Product_ID_PK, MAKEMODELID = objMasMakeModel.Model_ID_PK } equals new { ProductID = (int)objProductPermission.ProductID, MAKEMODELID = objProductPermission.PermissionTypeId.Value } into ProductPermission
        //                       from objProductPremission in ProductPermission.DefaultIfEmpty()
        //                       where objMasProduct.Product_ID_PK == productID

        //                       //where objMasMakeModel.RSKTYPE == "FPV"
        //                       select new checkBoxListValues
        //                       {
        //                           ChkId = objMasMakeModel.Model_ID_PK.ToString(),
        //                           ChkName = objMasMakeModel.Variance,
        //                           SelectedValue = objProductPremission.ProductPermDetID == null ? false : true
        //                       }
        //                    ).DistinctBy(a => a.ChkId).ToList();
        //    }
        //    if (checklevel != 1)
        //    {
        //        lstTempMasterList = (from obj in lstTempMasterList
        //                             join obj1 in ParentFetch on obj.ChkId equals obj1.ChkId
        //                             where obj1.SelectedValue == true
        //                             select new checkBoxListValues
        //                             {
        //                                 ChkId = obj.ChkId,
        //                                 ChkName = obj.ChkName,
        //                                 SelectedValue = obj.SelectedValue == null || obj.SelectedValue == false ? false : true
        //                             }
        //                       ).ToList();
        //    }
        //    foreach (checkBoxListValues master in lstTempMasterList)
        //    {
        //        checkBoxListValues checkBoxValue = new checkBoxListValues { ChkId = Convert.ToString(master.ChkId), SelectedValue = master.SelectedValue, ChkName = master.ChkName };
        //        lstMakeModel.Add(checkBoxValue);
        //    }

        //    return lstMakeModel;
        //}    
        //public string CheckUserIDName(string UserIdName)
        //{
        //    var Username = (from obj in Entities.Users
        //                    where obj.UserName == UserIdName
        //                    select obj.UserName).FirstOrDefault();

        //    if (Username != null)
        //    {
        //        return Username;
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}
        internal void fillProductSpecificPrivilages(UserProductPermissions objProductPermissions, Guid userID, int productID)
        {
            tblUserProductPermission productPermission = Entities.tblUserProductPermissions.Where(a => a.UserID == userID && a.ProductID == productID).FirstOrDefault();

            //Added Code to get parent Permissions
            var usert = (from objtblUserDetails in Entities.tblUserDetails
                         where objtblUserDetails.UserID == userID
                         select objtblUserDetails).FirstOrDefault();
            var checklevel = usert.userlevel == "L0" ? 1 : 0;
            while (usert.userlevel != "L0")
            {
                usert = Entities.tblUserDetails.Where(a => a.NodeID == usert.UserParentId).FirstOrDefault();
            }
            var ParentproductPermission = Entities.tblUserProductPermissions.Where(a => a.UserID == usert.UserID && a.ProductID == productID).FirstOrDefault();
            if (productPermission != null)
            {
                objProductPermissions.AgeOfVehicle = productPermission.AgeOfVehicle;
                objProductPermissions.BackdationDays = productPermission.BackdationDays;
                objProductPermissions.AdvanceDays = productPermission.AdvanceDays;
                objProductPermissions.isNBAllowed = productPermission.isNBAllowed;
                objProductPermissions.isROAllowed = productPermission.isROAllowed;
                objProductPermissions.isUsedAllowed = productPermission.isUsedAllowed;
                objProductPermissions.isHOPrintAllowed = productPermission.isHOPrintAllowed;


            }
            if (checklevel == 0 && ParentproductPermission != null)
            {
                //added to disable radio button of Field level data points
                objProductPermissions.ParentNBAllowed = ParentproductPermission.isNBAllowed;
                objProductPermissions.ParentROAllowed = ParentproductPermission.isROAllowed;
                objProductPermissions.ParentUsedAllowed = ParentproductPermission.isUsedAllowed;
                objProductPermissions.ParentHOPrintAllowed = ParentproductPermission.isHOPrintAllowed;
            }
        }

        public int SaveUserPhoto(HttpPostedFileBase imgFile, int photoID)
        {
            int returnVal = 0;
            if (imgFile != null)
            {
                tblUserPhoto UserPhoto;
                if (photoID == 0)
                {
                    UserPhoto = new tblUserPhoto();
                }
                else
                {
                    UserPhoto = Entities.tblUserPhotoes.Where(o => o.UserPhotoID == photoID).FirstOrDefault();
                }
                var directoryPath = HttpContext.Current.Server.MapPath("Upload");
                var imageFileName = Path.Combine(directoryPath, Path.GetFileName(imgFile.FileName));
                var filename = Path.GetFileNameWithoutExtension(imgFile.FileName);
                //objUserPhoto.UserDetailsID = objUserDetail.NodeID;
                UserPhoto.FileName = filename;
                Stream fileStream = imgFile.InputStream;
                BinaryReader reader = new BinaryReader(fileStream);

                //byte[] documentBytes = new byte[fileStream.Length];
                //fileStream.Read(documentBytes, 0, documentBytes.Length);
                //UserPhoto.FileContent = documentBytes;
                UserPhoto.FileContent = reader.ReadBytes((int)imgFile.ContentLength);
                UserPhoto.ContentType = imgFile.ContentType;
                //objUserPhoto.CreatedBy = LoginModel.GetUserId();
                UserPhoto.CreatedDate = DateTime.Now;
                // UserPhoto.tblUserDetail = userDetail;
                if (photoID == 0)
                {
                    Entities.tblUserPhotoes.Add(UserPhoto);
                }
                //else
                //{
                //    returnVal = photoID;
                //}
                if (System.IO.File.Exists(imageFileName))
                {
                    System.IO.File.Delete(imageFileName);
                }
                Entities.SaveChanges();
                returnVal = Convert.ToInt32(UserPhoto.UserPhotoID);

            }
            return returnVal;
        }

        public IEnumerable<ChildIDs> FetchChildIDs(decimal nodeID)
        {
            var accountID = Entities.tblUserDetails.Where(o => o.NodeID == nodeID).FirstOrDefault().UserAccountID;
            var FetchChildIDs = (from obj in Entities.tblUserDetails
                                 where obj.UserAccountID == accountID && obj.NodeID != nodeID

                                 select new ChildIDs
                                 {
                                     NodeID = obj.NodeID,
                                     UserCode = obj.UserCode,
                                     UserIdName = obj.LoginID,
                                     ReportingUserId = Entities.tblUserDetails.Where(o => o.NodeID == obj.UserParentId).Select(a => a.UserCode).FirstOrDefault(),
                                     ReportingUserName = Entities.tblUserDetails.Where(o => o.NodeID == obj.UserParentId).Select(a => a.LoginID).FirstOrDefault()
                                 }).OrderBy(a => a.NodeID);

            return FetchChildIDs;
        }

        //public IEnumerable<DuplicatePanDetails> FetchDuplicatePanDetails(string panNo)
        //{
        //    var FetchPanDetails = (from obj in Entities.tblImdDetails
        //                           where obj.PAN == panNo && !obj.IMDCode.Contains("IMD")

        //                           select new DuplicatePanDetails
        //                           {
        //                               Imdcode = obj.IMDCode,
        //                               FirstName = obj.FirstName,
        //                               Lastname = obj.LastName,
        //                               CorporateName = obj.CorpName,
        //                               FGChannel = obj.tblMasFGChannel.LongDesc,
        //                               FGChannelCode = obj.tblMasFGChannel.Value

        //                           }).OrderBy(a => a.Imdcode);

        //    return FetchPanDetails;
        //}

        //public Boolean CheckIMDType(string imdCode)
        //{
        //    var IMDType = (from obj in Entities.tblImdDetails
        //                   where obj.IMDCode == imdCode
        //                   select obj.IMDTypeID).FirstOrDefault();

        //    if (IMDType == 3 || IMDType == 4)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public IEnumerable<Mapping> CheckExistingMapping(string imdCode)
        //{
        //    var ImdUserID = Entities.tblImdDetails.Where(a => a.IMDCode == imdCode).FirstOrDefault().ImdDetailsID;
        //    var FetchMappingDetails = (from obj in Entities.tblIrdaSrMappings
        //                               where obj.IMDUserID == ImdUserID

        //                               select new Mapping
        //                               {
        //                                   index = obj.IrdaSrMappingID,
        //                                   mappingcode1 = Entities.tblImdDetails.Where(a => a.ImdDetailsID == obj.IMDUserID).FirstOrDefault().IMDCode,
        //                                   mappingcode2 = Entities.tblImdDetails.Where(a => a.ImdDetailsID == obj.MappedIMDId).FirstOrDefault().IMDCode,
        //                               }).OrderBy(a => a.index);

        //    return FetchMappingDetails;
        //}

        //public string SaveMappingCode(string mappingcode1, string mappingcode2)
        //{
        //    string returnMsg = "";
        //    int saveStatus = 0;

        //    var IMDUserID = (from obj in Entities.tblImdDetails
        //                     where obj.IMDCode == mappingcode1
        //                     select obj.ImdDetailsID).FirstOrDefault();

        //    var CheckIMDCode = (from obj in Entities.tblIrdaSrMappings
        //                        where obj.IMDUserID == IMDUserID
        //                        select obj.IMDUserID).FirstOrDefault();
        //    if (CheckIMDCode == null)
        //    {
        //        var MappedIMDId = (from obj in Entities.tblImdDetails
        //                           where obj.IMDCode == mappingcode2
        //                           select obj.ImdDetailsID).FirstOrDefault();

        //        tblIrdaSrMapping SrMapping = new tblIrdaSrMapping();
        //        SrMapping.IMDUserID = IMDUserID;
        //        SrMapping.MappedIMDId = MappedIMDId;
        //        SrMapping.CreatedDate = DateTime.Now;
        //        Entities.tblIrdaSrMappings.Add(SrMapping);
        //        returnMsg = "Mapping Done Successfully.";

        //        saveStatus = Entities.SaveChanges();
        //        if (saveStatus > 0)
        //        {
        //            return returnMsg;
        //        }
        //        else
        //        {
        //            return "Mapping Failed.";
        //        }
        //    }
        //    else
        //    {
        //        return "";
        //    }


        //}

        //public string RemoveMapping(decimal index)
        //{
        //    string returnMsg = "";
        //    int saveStatus = 0;
        //    tblIrdaSrMapping mappingDetails = new tblIrdaSrMapping();

        //    mappingDetails = (from obj in Entities.tblIrdaSrMappings
        //                      where obj.IrdaSrMappingID == index
        //                      select obj).FirstOrDefault();

        //    if (mappingDetails != null)
        //    {
        //        Entities.tblIrdaSrMappings.Remove(mappingDetails);
        //    }

        //    returnMsg = "Mapping Removed Successfully.";
        //    saveStatus = Entities.SaveChanges();
        //    if (saveStatus > 0)
        //    {
        //        return returnMsg;
        //    }
        //    else
        //    {
        //        return "Removed Mapping Failed.";
        //    }
        //}

        //public decimal GetGridRowID(string mappingcode1)
        //{
        //    tblIrdaSrMapping mappingDetails = new tblIrdaSrMapping();

        //    var IMDUserID = (from obj in Entities.tblImdDetails
        //                     where obj.IMDCode == mappingcode1
        //                     select obj.ImdDetailsID).FirstOrDefault();

        //    var mappingid = (from obj in Entities.tblIrdaSrMappings
        //                     where obj.IMDUserID == IMDUserID
        //                     select obj.IrdaSrMappingID).FirstOrDefault();

        //    if (mappingid != 0)
        //    {
        //        return mappingid;
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        public void FetchIMIENumber(PermissionTree obj, Guid? userId)
        {
            obj.IMEINumber = Entities.tblUserDetails.Where(a => a.UserID == userId).FirstOrDefault().IMEINumber;
            if (obj.IMEINumber != null && obj.IMEINumber != "")
            {
                obj.isIMEIChecked = true;
            }
            else
            {
                obj.isIMEIChecked = false;
            }
        }

        //public string CheckIMDName(string firstName, string lastName)
        //{
        //    var IMDDetails = (from obj in Entities.tblImdDetails
        //                      where (obj.FirstName == firstName) && (obj.LastName == lastName)
        //                      select obj).FirstOrDefault();

        //    if (IMDDetails != null)
        //    {
        //        return IMDDetails.FirstName + " " + IMDDetails.LastName;
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        //public string CheckCorporateName(string corporateName)
        //{
        //    var IMDCorporateName = (from obj in Entities.tblImdDetails
        //                            where obj.CorpName == corporateName
        //                            select obj.CorpName).FirstOrDefault();

        //    if (IMDCorporateName != null)
        //    {
        //        return IMDCorporateName;
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        //public BankDetails SearchIfscCode(string ifscCode)
        //{
        //    BankDetails objBankDetails = new BankDetails();
        //    tblMasBankMaster objtblMasBankMaster = new tblMasBankMaster();
        //    objtblMasBankMaster = (from obj in Entities.tblMasBankMasters
        //                           where obj.IFSCCODE == ifscCode
        //                           select obj).FirstOrDefault();
        //    if (objtblMasBankMaster != null)
        //    {
        //        objBankDetails.MICRCode = objtblMasBankMaster.MICRCODE.Trim();
        //        objBankDetails.BankCode = objtblMasBankMaster.IFSCCODE.Trim();
        //        objBankDetails.BranchName = objtblMasBankMaster.BANK_ADDR01.Trim();
        //    }
        //    return objBankDetails;
        //}

        //public UserPhoto GetUserDetails()
        //{
        //    UserPhoto objUserDet = new UserPhoto();
        //    string userName = HttpContext.Current.User.Identity.Name;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(userName))
        //        {
        //            User objUser = Entities.Users.Where(x => x.UserName == userName).FirstOrDefault();
        //            tblUserDetail objTblUserDetails = Entities.tblUserDetails.Where(x => x.UserID == objUser.UserId).FirstOrDefault();
        //            tblUserPhoto objTblUserPhoto = Entities.tblUserPhotoes.Where(x => x.UserDetailsID == objTblUserDetails.NodeID).FirstOrDefault();
        //            //if (objTblUserPhoto != null)
        //            //{
        //            //    objUserDet = Mapper.Map<tblUserPhoto, UserPhoto>(objTblUserPhoto);
        //            //}
        //            //else
        //            //{
        //            //    objUserDet = new UserPhoto();
        //            //}
        //            objUserDet.userName = objTblUserDetails.LoginID;
        //            objUserDet.FileContent = objTblUserPhoto.FileContent;
        //            objUserDet.ContentType = objTblUserPhoto.ContentType;
        //            objUserDet.gender = Entities.tblMasCommonTypes.Where(x => x.CommonTypesID == objTblUserDetails.Gender).Select(o => o.Description).FirstOrDefault(); //TO DO: Make this dynamic.

        //        }
        //        return objUserDet;
        //    }
        //    catch (Exception)
        //    {
        //        // CommonException.HandleException(ex, CrossCutting_Constants.ControllerLogicPolicy);
        //        return objUserDet;
        //    }

        //}

        //public IEnumerable<IMDHistory> FetchGridImdHistory(string imdCode)
        //{
        //    var FetchImdHistory = (from obj in Entities.tblImdHistories
        //                           where obj.IMDCode == imdCode
        //                           select new IMDHistory
        //                           {
        //                               imdCode = obj.IMDCode,
        //                               imdName = obj.IMDName,
        //                               //createdBy = LoginUser.GetUserName(obj.CreatedBy),
        //                               createdBy = Entities.Users.Where(a => a.UserId == obj.CreatedBy).FirstOrDefault().UserName,
        //                               createDate = obj.CreatedDate,
        //                               //modifiedBy = LoginUser.GetUserName(obj.ModifiedBy)
        //                               modifiedBy = Entities.Users.Where(a => a.UserId == obj.ModifiedBy).FirstOrDefault().UserName,
        //                               modifiedDate = obj.ModifiedDate,

        //                           }).OrderBy(a => a.imdCode);

        //    return FetchImdHistory;
        //}

        /// <summary>
        /// Fetch user details throught the UserDetailsId.
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        public CreateUserModel FetchUserDetails(string userName)
        {
            CreateUserModel objCreateUserModel = new CreateUserModel();
            var UserDetails = Entities.tblUserDetails.Where(a => a.LoginID == userName).FirstOrDefault();
            objCreateUserModel.FirstName = UserDetails.FirstName;
            objCreateUserModel.LastName = UserDetails.LastName;
            objCreateUserModel.MobileNo = UserDetails.ContactNo;
            objCreateUserModel.EmailId = UserDetails.Email;
            return objCreateUserModel;
        }

        //---------------------added Gowthami-------------------------------
        /// <summary>
        /// get internal user Function list from tblEmployees
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<MasterListItem> GetInterUserFunction()
        //{
        //    var lstFunction = (from objMasFun in Entities.tblMasFunctions
        //                       select new MasterListItem
        //                       {
        //                           ID = objMasFun.MasFunctionID,
        //                           Value = objMasFun.FunctionName
        //                       }).ToList();

        //    return lstFunction;
        //}  /// <summary>
        /// get internal user Branchcode list from tblMasBranches
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MasterListItem> GetUserBranchCode()
        {
            IEnumerable<MasterListItem> Branchcode = (from obj in Entities.tblMasBranches
                                                      where obj.isValid == true //&& obj.Value != "**"
                                                      select new MasterListItem
                                                      {
                                                          Value = obj.ShortDesc,
                                                          ID = (int)obj.BranchId
                                                      }).OrderBy(a => a.Value.Trim().ToUpper() != "ALL BRANCH").ThenBy(a => a.Value.Trim());
            return Branchcode;
        }


        public string GetIMDCode(string UserName)
        {
            string IMD = Entities.tblUserDetails.Where(a => a.UserAccountID == (Entities.tblUserDetails.Where(c => c.LoginID == UserName).FirstOrDefault().UserAccountID) && a.userlevel == "L0").Select(a => a.IMDCode).FirstOrDefault();
            return IMD;
        }



        public string VerifyClientCode(string ClientCode)
        {
            var clientCode = (from obj in Entities.tblCustomers
                              where obj.CustUniqueID == ClientCode
                              select obj.CustUniqueID).FirstOrDefault();

            if (clientCode != null)
            {
                return clientCode;
            }
            else
            {
                return "";
            }
        }

        public IEnumerable<MasterListItem> FetchGenders()
        {
            IEnumerable<MasterListItem> lstGenders = null;

            //lstGenders = from cat in Entities.tblMasCommonTypesJS
            //             where cat.MasterType == AgentOnBoarding.AgentGender
            //             select new MasterListItem
            //             {
            //                 Value = cat.Description,
            //                 ID = cat.CommonTypesID

            //             };
            return lstGenders;


        }
        public bool SaveOTPInformation(Guid UserID, string OTP, string UserName)
        {
            tblUserOTP objUserOtp = new tblUserOTP();
            objUserOtp.LoginName = UserName;
            objUserOtp.UserID = UserID;
            objUserOtp.OTPGenerated = OTP;
            objUserOtp.GeneartedDateTime = DateTime.Now;
            Entities.tblUserOTPs.Add(objUserOtp);
            Entities.SaveChanges();
            string mobileNumber = null;
            string UserId = null;
            string email = null;
            var userDetails = Entities.tblUserDetails.FirstOrDefault(a => a.LoginID == UserName);
            if (userDetails != null)
            {
                mobileNumber = userDetails.ContactNo;
                UserId = Convert.ToString(userDetails.UserID);
                email = userDetails.Email;
            }
            SMSIntegration objSms = new SMSIntegration();
            bool status = false;// objSms.SendOTPtoResetPassword(mobileNumber, OTP);
            EmailIntegration objEmail = new EmailIntegration();
            //status = objEmail.SendOTPToResetPassword(email, OTP);
            return status;
        }
        public string VerifyOTPInformation(string OTP, string UserName)
        {
            string Valid = string.Empty;
            tblUserOTP objUserOtp = new tblUserOTP();
            objUserOtp = Entities.tblUserOTPs.Where(a => a.LoginName == UserName).OrderByDescending(a => a.id).FirstOrDefault();
            if (objUserOtp != null)
            {
                DateTime CheckTime = DateTime.Now.AddMinutes(-30);
                if (objUserOtp.GeneartedDateTime >= CheckTime)
                {
                    if (OTP == objUserOtp.OTPGenerated)
                    {
                        return "true";
                    }
                    else
                    {
                        return "OTP doesn't match";
                    }
                }
                else
                {
                    return "Generated OTP Expired";
                }
            }
            else
            {
                return "Please input Correct Information";
            }
        }
        public string ResendOTPInformation(string UserName)
        {
            string Valid = string.Empty;
            tblUserOTP objUserOtp = new tblUserOTP();
            objUserOtp = Entities.tblUserOTPs.Where(a => a.LoginName == UserName).OrderByDescending(a => a.id).FirstOrDefault();
            if (objUserOtp != null)
            {
                DateTime CheckTime = DateTime.Now.AddMinutes(-30);
                if (objUserOtp.GeneartedDateTime >= CheckTime)
                {
                    string mobileNumber = null;
                    string Email = null;
                    var userDetails = Entities.tblUserDetails.FirstOrDefault(a => a.LoginID == UserName);
                    if (userDetails != null)
                    {
                        mobileNumber = userDetails.ContactNo;
                        Email = userDetails.Email;
                    }
                    SMSIntegration objSms = new SMSIntegration();
                    EmailIntegration objEmail = new EmailIntegration();
                    bool status = false;// objSms.SendOTPtoResetPassword(mobileNumber, objUserOtp.OTPGenerated);
                    //status = objEmail.SendOTPToResetPassword(Email, objUserOtp.OTPGenerated);
                    string Result = Convert.ToString(status);
                    return Result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public string GenerateRandomOTP()
        {
            int iOTPLength = 4;
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string sOTP = String.Empty;

            string sTempChars = String.Empty;

            Random rand = new Random();

            for (int i = 0; i < iOTPLength; i++)
            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }

            return sOTP;

        }
        public bool Sendmailsmsresetpassword(string MobileNo, string emailID)
        {
            bool result;
            SMSIntegration objSMS = new SMSIntegration();
            EmailIntegration objEmail = new EmailIntegration();
            //result = objEmail.SendEmailOnResetPasswordSuccess(emailID);
            result = false;// objSMS.SendResetPasswordSuccessMessage(MobileNo);
            return result;
        }
        public List<ConfigurationGridData> GetConfiguration()
        {
            List<ConfigurationGridData> ObjConfigurationGridData = new List<ConfigurationGridData>();

            ObjConfigurationGridData.Add(new ConfigurationGridData { ConvertionID = 1, ConvertionFrom = "Suspect", ConvertionTo = "Prospect", SpecifiedTimeLine = "" });
            ObjConfigurationGridData.Add(new ConfigurationGridData { ConvertionID = 2, ConvertionFrom = "Prospect New", ConvertionTo = "Confirmed Prospect", SpecifiedTimeLine = "" });
            ObjConfigurationGridData.Add(new ConfigurationGridData { ConvertionID = 3, ConvertionFrom = "Confirmed Prospect", ConvertionTo = "Prospect Needs Analysis Completed", SpecifiedTimeLine = "" });
            ObjConfigurationGridData.Add(new ConfigurationGridData { ConvertionID = 4, ConvertionFrom = "Prospect Needs Analysis Completed", ConvertionTo = "Quotation", SpecifiedTimeLine = "" });
            ObjConfigurationGridData.Add(new ConfigurationGridData { ConvertionID = 5, ConvertionFrom = "Quotation", ConvertionTo = "Proposal new", SpecifiedTimeLine = "" });
            ObjConfigurationGridData.Add(new ConfigurationGridData { ConvertionID = 6, ConvertionFrom = "Proposal new", ConvertionTo = "Proposal Submitted", SpecifiedTimeLine = "" });
            ObjConfigurationGridData.Add(new ConfigurationGridData { ConvertionID = 7, ConvertionFrom = "Claims new", ConvertionTo = "Claims Submitted", SpecifiedTimeLine = "" });
            ObjConfigurationGridData.Add(new ConfigurationGridData { ConvertionID = 8, ConvertionFrom = "Endorsment new", ConvertionTo = "Endorsment  Submitted", SpecifiedTimeLine = "" });

            return ObjConfigurationGridData;
        }
        public IMOUsers FilltblMasIMOUserDetails(IMOUsers objIMOUsers)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            try
            {
                Context.CreateUser(objIMOUsers.UserId, objIMOUsers.UserName, objIMOUsers.Email, objIMOUsers.MobNo, objIMOUsers.UserRole, objIMOUsers.NIC, objIMOUsers.Branch
                    , objIMOUsers.Channel, objIMOUsers.AgentCode, objIMOUsers.ReportingManager, objIMOUsers.UserStatus, objIMOUsers.AuthLimit, objIMOUsers.LockoutEnable).FirstOrDefault();

            }
            catch (Exception ex)
            {
                objIMOUsers.StatusCode = "Error";
                return objIMOUsers;
            }
            
            return objIMOUsers;

        }

        //public CreateUserModel CreateUser()
        //{

        //}
        public Login UserLogin(Login objLogin)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            try
            {
                tblUserDetail obj = new tblUserDetail();
                obj = Context.tblUserDetails.Where(a => a.LoginID == objLogin.UserId).FirstOrDefault();
                string userId = Common.CommonBusiness.GetUserId(objLogin.UserId);
                string role = Context.GetRoleByUserName(objLogin.UserId).FirstOrDefault();
                objLogin.Role = role;
                Guid UserId = new Guid(userId);

                if (string.IsNullOrEmpty(obj.DeviceID))
                {
                    if (role == "Banca Agent ")
                    {
                        obj.DeviceID = objLogin.DeviceID;
                        obj.APKVersion = objLogin.APKVersion;
                        obj.DeviceModelName = objLogin.DeviceModelName;
                        obj.DeviceOSVersion = objLogin.DeviceOS;
                        obj.DeviceToken = objLogin.DeviceToken;
                        Context.SaveChanges();
                    }
                    else
                    {
                        bool isExists = Context.tblUserDetails.Any(x => x.DeviceID == objLogin.DeviceID);
                        if (isExists)
                        {
                            objLogin.Message = "Device ID is already mapped.";
                            return objLogin;
                        }
                        else
                        {
                            obj.DeviceID = objLogin.DeviceID;
                            obj.APKVersion = objLogin.APKVersion;
                            obj.DeviceModelName = objLogin.DeviceModelName;
                            obj.DeviceOSVersion = objLogin.DeviceOS;
                            obj.DeviceToken = objLogin.DeviceToken;
                            Context.SaveChanges();
                        }
                    }

                }
                else
                {
                    obj.APKVersion = objLogin.APKVersion;
                    obj.DeviceModelName = objLogin.DeviceModelName;
                    obj.DeviceOSVersion = objLogin.DeviceOS;
                    obj.DeviceToken = objLogin.DeviceToken;
                    Context.SaveChanges();
                }
                if (obj.Status == true)
                {
                    if (objLogin.DeviceID == obj.DeviceID && !string.IsNullOrEmpty(objLogin.DeviceID))
                    {
                        List<Permission> data = Context.tblUserPermissions.Where(a => a.UserID == UserId).Select(a => new Permission
                        {
                            PermissionID = a.PermissionId,
                        }).ToList();
                        objLogin.lstPermission = data;

                        List<GetIntroducerCode_Result> listIntroducer = new List<GetIntroducerCode_Result>();
                        if (!string.IsNullOrEmpty(role))
                        {
                            if (role == "Banca Agent ")
                            {
                                listIntroducer = Context.GetIntroducerCode(objLogin.UserId).ToList();
                                objLogin.listIntroducer = listIntroducer.Select(a => a.Introducercode).ToList();

                            }
                        }
                        if (obj.Status == true)
                        {
                            if (objLogin.lstPermission.Count > 0)
                            {
                                objLogin.Message = "Success";
                            }
                            else
                            {
                                objLogin.Message = "UserId is not valid";
                            }
                        }


                    }
                    else
                    {
                        objLogin.Message = "Invalid Device ID";
                    }
                }
                else
                {
                    objLogin.Message = "User is not Active";
                }

                return objLogin;
            }
            catch (Exception ex)
            {

                return objLogin;
            }


        }
        public UserToken ValidateUserLogin(UserToken objLogin)
        {
            AVOAIALifeEntities Context = new AVOAIALifeEntities();
            try
            {
                tblUserDetail obj = new tblUserDetail();
                obj = Context.tblUserDetails.Where(a => a.LoginID == objLogin.UserId).FirstOrDefault();
                bool userStatus = Context.tblMasIMOUsers.Where(a => a.UserID == objLogin.UserId).Select(a => a.UserStatus).FirstOrDefault();
                if (objLogin.UserId == obj.LoginID && objLogin.Password == ConfigurationManager.AppSettings["TokenPassword"] && userStatus == true)
                {
                    PolicyLogic objLogic = new PolicyLogic();
                    CommonBusiness objcommonBusiness = new CommonBusiness();
                    objLogin.ServiceTraceID = objcommonBusiness.GenerateTraceNumber("MAPP");
                    objLogic.InsertIntoAPPAuthenticationDetails(objLogin.ServiceTraceID, objLogin.UserId, objLogin.UserName);
                    objLogin.ValidUser = "Valid User";
                }
                else
                {
                    objLogin.ValidUser = "Invalid User ID or Password";
                }

                return objLogin;
            }
            catch (Exception ex)
            {

                return objLogin;
            }


        }
        public CreateUserModel FetchUserData(CreateUserModel objuser, bool ISsingleUSer = false)
        {
            using (AVOAIALifeEntities Entities = new AVOAIALifeEntities())
            {
                if (ISsingleUSer)
                {
                    var objtblUserDetail = Entities.USP_GetUserDetails(objuser.UserName, objuser.UserCode, objuser.DeviceID).FirstOrDefault();

                    if (objtblUserDetail != null)
                    {
                        objuser.objDeviceDetails = new DeviceDetails();
                        objuser.IMDCode = objtblUserDetail.LoginID;
                        objuser.UserName = objtblUserDetail.LoginID;
                        objuser.MobileNo = objtblUserDetail.ContactNo;
                        objuser.EmailId = objtblUserDetail.Email;
                        objuser.branhCode = objtblUserDetail.Branch;
                        objuser.objDeviceDetails.DeviceID = objtblUserDetail.DeviceID;
                        objuser.objDeviceDetails.DeviceName = objtblUserDetail.DeviceModelName;
                        objuser.AuthLimit = Convert.ToInt32(objtblUserDetail.AuthorityId);
                        objuser.lstUserstatus = GetUserStatus();
                        objuser.UserStatus = objtblUserDetail.Status == true ? "1" : "0";
                        objuser.UserRole = objtblUserDetail.userrole;
                        objuser.ReportingCode = objtblUserDetail.ReportingManager;
                        objuser.ReportingManager = objtblUserDetail.ReportingManager; //06-02-2019
                        //objuser.DeactivateUser = objtblUserDetail.LockoutEndDateUtc != null ? true : false;
                        //objuser.DeactivateStatus = objtblUserDetail.LockoutEndDateUtc.ToString();
                        //if (objuser.DeactivateUser==false)
                        //{
                        //    objuser.UserStatus ="0";

                        //}
                        //else
                        //{
                        //    objuser.UserStatus = "1";
                        //}
                    }
                }
                else
                {
                    List<CreateUserModel> objLstUser = null;
                    var objtblUserDetail = Entities.USP_GetUserDetails(objuser.UserName, objuser.UserCode, objuser.DeviceID).ToList();
                    objLstUser = (from item in objtblUserDetail
                                  select new CreateUserModel
                                  {
                                      NodeID = item.NodeID,
                                      IMDCode = (item.IMDCode == null? item.LoginID: item.IMDCode),
                                      UserName = item.LoginID,
                                      MobileNo = item.ContactNo,
                                      EmailId = item.Email,
                                      branhCode = item.Branch,
                                      DeviceID = item.DeviceID,
                                      DeviceName = item.DeviceModelName,
                                      AuthLimit = Convert.ToInt32(item.AuthorityId),
                                      UserStatus = item.Status == true ? "1" : "0",
                                      UserRole = item.userrole,
                                      ReportingCode = item.ReportingManager
                                  }).ToList();
                    objuser.LstCreateUserModel = objLstUser;
                }


                objuser.lstUserstatus = GetUserStatus();
            }
            return objuser;
        }
        public CreateUserModel FetchDeviceName(CreateUserModel objuser, bool ISsingleUSer = false)
        {
            using (AVOAIALifeEntities Entities = new AVOAIALifeEntities())
            {
                if (ISsingleUSer)
                {
                    var DeviceModelName = Entities.tblUserDetails.Where(a => a.DeviceID == objuser.DeviceID).Select(a => a.DeviceModelName).FirstOrDefault();

                    if (!string.IsNullOrEmpty(DeviceModelName))
                    {
                        objuser.DeviceName = DeviceModelName;
                    }
                }


            }

            return objuser;
        }

        public CreateUserModel FetchUserExistance(CreateUserModel objuser)
        {
            using (AVOAIALifeEntities Entities = new AVOAIALifeEntities())
            {

                List<tblUserDetail> objLstUser = Entities.tblUserDetails.Where(a => a.LoginID == objuser.UserName || a.LoginID == objuser.UserCode).ToList();
                if (objLstUser.Count == 0)
                {
                    objuser.UserExist = false;
                }
                else
                {
                    objuser.UserExist = true;
                }
            }

            return objuser;
        }
        public CreateUserModel FetchUserDataByNodeID(CreateUserModel objuser)
        {
            using (AVOAIALifeEntities Entities = new AVOAIALifeEntities())
            {
                List<CreateUserModel> objLstUser = null;
                var objtblUserDetail = Entities.USP_GetUserDetailsByNodeiD(objuser.UserName, objuser.UserCode, objuser.DeviceID).FirstOrDefault();
                objuser.LstCreateUserModel = objLstUser;
                if (objtblUserDetail != null)
                {
                    objuser.objDeviceDetails = new DeviceDetails();
                    objuser.IMDCode = objtblUserDetail.IMDCode;
                    objuser.UserName = objtblUserDetail.LoginID;
                    objuser.MobileNo = objtblUserDetail.ContactNo;
                    objuser.EmailId = objtblUserDetail.Email;
                    objuser.branhCode = objtblUserDetail.Branch;
                    objuser.objDeviceDetails.DeviceID = objtblUserDetail.DeviceID;
                    objuser.objDeviceDetails.DeviceName = objtblUserDetail.DeviceModelName;
                    objuser.AuthLimit = Convert.ToInt32(objtblUserDetail.AuthorityId);
                    objuser.lstUserstatus = GetUserStatus();
                    objuser.UserStatus = objtblUserDetail.Status == true ? "1" : "0";
                    objuser.UserRole = objtblUserDetail.userrole;
                    objuser.ReportingCode = objtblUserDetail.ReportingManager;
                    objuser.ReportingManager = objtblUserDetail.ReportingManager;
                }
            }

            return objuser;
        }
        public CreateUserModel UpdateDevicedetails(CreateUserModel objuser)
        {
            using (AVOAIALifeEntities Entities = new AVOAIALifeEntities())
            {
                var objtblUserDetail = Entities.tblUserDetails.FirstOrDefault(a => a.LoginID == objuser.UserName);
                if (objtblUserDetail != null && (objtblUserDetail.DeviceID != objuser.DeviceID || objtblUserDetail.DeviceModelName != objuser.DeviceName))
                {
                    if (!string.IsNullOrEmpty(objtblUserDetail.DeviceID) && !string.IsNullOrEmpty(objtblUserDetail.DeviceModelName))
                    {
                        tblDevicedetail objtblDevicedetail = new tblDevicedetail();
                        objtblDevicedetail.DeviceId = objtblUserDetail.DeviceID;
                        objtblDevicedetail.NodeID = objtblUserDetail.NodeID;
                        objtblDevicedetail.DeviceName = objtblUserDetail.DeviceModelName;
                        objtblDevicedetail.CreatedDate = DateTime.Now;
                        objtblDevicedetail.Updatedate = DateTime.Now;
                        Entities.tblDevicedetails.Add(objtblDevicedetail);
                    }
                    objtblUserDetail.DeviceID = objuser.objDeviceDetails.DeviceID;
                    objtblUserDetail.DeviceModelName = objuser.objDeviceDetails.DeviceName;
                    Entities.SaveChanges();
                }
                else
                {

                }
            }
            return objuser;
        }
        public CreateUserModel FetchDeviceHistory(CreateUserModel objDeviceDetails)
        {
            using (AVOAIALifeEntities Entities = new AVOAIALifeEntities())
            {
                List<DeviceDetails> ObjDeviceHistory = new List<DeviceDetails>();
                ObjDeviceHistory = (from obj in Entities.tblDevicedetails
                                    join obj1 in Entities.tblUserDetails on obj.NodeID equals obj1.NodeID
                                    where obj1.LoginID == objDeviceDetails.UserName
                                    select new DeviceDetails
                                    {
                                        DeviceID = obj.DeviceId,
                                        DeviceName = obj.DeviceName,
                                        UPdatedDate = obj.Updatedate.ToString().Remove(12, 7)
                                    }).ToList();
                var objCurrent = (from obj1 in Entities.tblUserDetails
                                  where obj1.LoginID == objDeviceDetails.UserName
                                  select new DeviceDetails
                                  {
                                      DeviceID = obj1.DeviceID,
                                      DeviceName = obj1.DeviceModelName
                                  }).FirstOrDefault();

                objDeviceDetails.objDeviceDetails = objCurrent;
                objDeviceDetails.LstDeviceDetails = ObjDeviceHistory;
                return objDeviceDetails;

            }



        }
        public AppNotification SaveAppUpdate(AppNotification objAppNotification)
        {
            using (AVOAIALifeEntities Entities = new AVOAIALifeEntities())
            {
                tblAppVersion objtblAppVersion = new tblAppVersion();
                //objtblAppVersion.AppName = objAppNotification.SearchRecepient;
                objtblAppVersion.CreatedDate = DateTime.Now;
                objtblAppVersion.CustomMessage = objAppNotification.WhatsNew;
                Entities.tblAppVersions.Add(objtblAppVersion);
                Entities.SaveChanges();
                objAppNotification.Result = "Success";
            }
            objAppNotification.AllDeviceTokenID = GetDeviceToken(objAppNotification.SelectRecepient);
            return objAppNotification;



        }
        public string GetDeviceToken(int Parameter)
        {
            string Result = string.Empty;
            using (AVOAIALifeEntities Entities = new AVOAIALifeEntities())
            {
                string Description = Entities.tblMasCommonTypes.Where(x => x.CommonTypesID == Parameter).FirstOrDefault().Description;
                var data = Entities.USP_GetDeviceToken(Description).ToList();
                if (data != null && data.Count() > 0)
                {
                    Result = string.Join(",", data.ToArray());
                }
            }
            return Result;

        }
        public IEnumerable<MasterListItem> GetAppUpdateDropdown()
        {
            IEnumerable<MasterListItem> Lstddl = (from obj in Entities.tblMasCommonTypes
                                                  where obj.MasterType == "AppUpdate"
                                                  select new MasterListItem
                                                  {
                                                      ID = obj.CommonTypesID,
                                                      Value = obj.Description

                                                  });
            return Lstddl;
        }

        public IEnumerable<MasterListItem> GetAppNotificationDropdown()
        {
            IEnumerable<MasterListItem> Lstddl = (from obj in Entities.tblMasCommonTypes
                                                  where obj.MasterType == "AppNotification"
                                                  select new MasterListItem
                                                  {
                                                      ID = obj.CommonTypesID,
                                                      Value = obj.Description

                                                  });
            return Lstddl;
        }
        public List<UserSearch> SearchIndividual()
        {
            List<UserSearch> Lstddl = (from obj in Entities.tblUserDetails
                                       select new UserSearch
                                       {
                                           NodeId = obj.NodeID,
                                           Value = obj.IMDCode + " " + "|" + " " + obj.LoginID.ToUpper(),
                                           Userid = obj.UserID,
                                           DeviceID = obj.DeviceToken

                                       }).ToList();

            return Lstddl;
        }
        public ContentManagement FetchResourceCatagory(ContentManagement objContentManagement)
        {
            objContentManagement.lstContentManagement = new List<ContentManagement>();
            objContentManagement.lstContentManagement = (from obj in Entities.tblResourceCatagories
                                                             select new ContentManagement
                                                             {
Resoucecatagory=obj.ResourceCategory,
ResourceID=obj.ResourceCatagoryPK,
ExpiryDate=obj.Expirydate.ToString(),
EffectiveDate=obj.Effectivedate.ToString()
                                                             }).ToList();
            return objContentManagement;

        }
        public ContentManagement FetchResourceCatagoryMaster(ContentManagement objContentManagement)
        {
            objContentManagement.objContentManagementDeatils = new ContentManagementDeatils();
            objContentManagement.objContentManagementDeatils.lstContentType = (from obj in Entities.tblMasCommonTypes
                                                  where obj.MasterType == "ContentType"
                                                                               select new MasterListItem
                                                  {
                                                      ID = obj.CommonTypesID,
                                                      Value = obj.Description

                                                  }).ToList();
            objContentManagement.objContentManagementDeatils.lstContentLanguage = (from obj in Entities.tblMasCommonTypes
                                                                               where obj.MasterType == "ContentLanguage"
                                                                               select new MasterListItem
                                                                               {
                                                                                   ID = obj.CommonTypesID,
                                                                                   Value = obj.Description

                                                                               }).ToList();            
            return objContentManagement;

        }
        public ContentManagement AddResourceCatagoryDetails(ContentManagement objContentManagement)
        {
            if (objContentManagement.ResourceID == 0)
            {
                tblResourceCatagory objtblResourceCatagory = new tblResourceCatagory();
                tblResourceCategoryDetail objtblResourceCategoryDetail = new tblResourceCategoryDetail();
                objtblResourceCatagory.ResourceCategory = objContentManagement.Resoucecatagory;
                objtblResourceCatagory.Effectivedate =Convert.ToDateTime(objContentManagement.EffectiveDate);
                objtblResourceCatagory.Expirydate = Convert.ToDateTime(objContentManagement.ExpiryDate);
                objtblResourceCatagory.CreatedDate = DateTime.Now;
                objtblResourceCategoryDetail.ContentType = objContentManagement.objContentManagementDeatils.ContentType;
                objtblResourceCategoryDetail.ContentLanguage = objContentManagement.objContentManagementDeatils.ContentLanguage;
                objtblResourceCategoryDetail.ExpiryDate =Convert.ToDateTime(objContentManagement.objContentManagementDeatils.ExpiryDate);
                objtblResourceCategoryDetail.StartDate = Convert.ToDateTime(objContentManagement.objContentManagementDeatils.EffectiveDate);
                objtblResourceCategoryDetail.FileDescription = objContentManagement.objContentManagementDeatils.FileDescription;
                objtblResourceCategoryDetail.Link = objContentManagement.objContentManagementDeatils.Link;
                objtblResourceCategoryDetail.FileName = objContentManagement.objContentManagementDeatils.FileName;
                objtblResourceCategoryDetail.ImgName = objContentManagement.objContentManagementDeatils.ImgName;
                objtblResourceCategoryDetail.CreatedDate = DateTime.Now;
                objtblResourceCategoryDetail.tblResourceCatagory = objtblResourceCatagory;
                Entities.tblResourceCatagories.Add(objtblResourceCatagory);
                Entities.tblResourceCategoryDetails.Add(objtblResourceCategoryDetail);                
            }
            else
            {
                tblResourceCategoryDetail objtblResourceCategoryDetail = new tblResourceCategoryDetail();
                if (objContentManagement.ResourceChildID != 0)
                {
                     objtblResourceCategoryDetail = Entities.tblResourceCategoryDetails.Where(x => x.ResourcecatagoryDetailsPK == objContentManagement.ResourceChildID).FirstOrDefault();
                }
                else
                     objtblResourceCategoryDetail = new tblResourceCategoryDetail();
                objtblResourceCategoryDetail.ContentType = objContentManagement.objContentManagementDeatils.ContentType;
                objtblResourceCategoryDetail.ContentLanguage = objContentManagement.objContentManagementDeatils.ContentLanguage;
                objtblResourceCategoryDetail.ExpiryDate =Convert.ToDateTime(objContentManagement.objContentManagementDeatils.ExpiryDate);
                objtblResourceCategoryDetail.StartDate = Convert.ToDateTime(objContentManagement.objContentManagementDeatils.EffectiveDate);
                objtblResourceCategoryDetail.FileDescription = objContentManagement.objContentManagementDeatils.FileDescription;
                objtblResourceCategoryDetail.Link = objContentManagement.objContentManagementDeatils.Link;
                objtblResourceCategoryDetail.FileName = objContentManagement.objContentManagementDeatils.FileName;
                objtblResourceCategoryDetail.ImgName = objContentManagement.objContentManagementDeatils.ImgName;
                objtblResourceCategoryDetail.CreatedDate = DateTime.Now;
                objtblResourceCategoryDetail.ResourcecatagoryPK = objContentManagement.ResourceID;
                if (objContentManagement.ResourceChildID == 0)
                {
                    Entities.tblResourceCategoryDetails.Add(objtblResourceCategoryDetail);                    
                }                
            }
            Entities.SaveChanges();
            objContentManagement.ResourceID = Entities.tblResourceCatagories.Where(x => x.ResourceCategory == objContentManagement.Resoucecatagory).FirstOrDefault().ResourceCatagoryPK;
            objContentManagement = FetchResource(objContentManagement);
            return objContentManagement;
        }
        public ContentManagement FetchResource(ContentManagement objContentManagement)
        {
            objContentManagement.lstContentManagementDeatils = new List<ContentManagementDeatils>();
            if (objContentManagement.ResourceID == 0)
            {
                var Resoucecatagory = Entities.tblResourceCatagories.Where(x => x.ResourceCategory == objContentManagement.Resoucecatagory).FirstOrDefault();
                if (Resoucecatagory != null)
                {
                    objContentManagement.ResourceID = Resoucecatagory.ResourceCatagoryPK;
                    var Data = Entities.USP_GetResouceDetails(objContentManagement.ResourceID).ToList();
                    objContentManagement.lstContentManagementDeatils = (from item in Data
                                                                        select new ContentManagementDeatils
                                                                        {
                                                                            ContentLanguageName = item.ContentLanguage,
                                                                            ContentTypeName = item.ContentType,
                                                                            ExpiryDate = item.ExpiryDate,
                                                                            EffectiveDate = item.StartDate,
                                                                            Link = item.Link,
                                                                            ResoucecatagoryDetailsPK = item.ResourceCatagoryPKChild,
                                                                            ResoucecatagoryFK = item.ResourceCatagoryPKMain,
                                                                            Resoucecatagory = item.ResourceCategory

                                                                        }).ToList();
                }
            }
            else
            {
                var Data = Entities.USP_GetResouceDetails(objContentManagement.ResourceID).ToList();
                objContentManagement.lstContentManagementDeatils = (from item in Data
                                                                    select new ContentManagementDeatils
                                                                    {
                                                                        ContentLanguageName = item.ContentLanguage,
                                                                        ContentTypeName = item.ContentType,
                                                                        ExpiryDate = item.ExpiryDate,
                                                                        EffectiveDate = item.StartDate,
                                                                        Link = item.Link,
                                                                        ResoucecatagoryDetailsPK = item.ResourceCatagoryPKChild,
                                                                        ResoucecatagoryFK = item.ResourceCatagoryPKMain,
                                                                        Resoucecatagory = item.ResourceCategory

                                                                    }).ToList();
                if (objContentManagement.lstContentManagementDeatils != null && objContentManagement.lstContentManagementDeatils.Count() > 0)
                {
                    var ResourceDeatils = objContentManagement.lstContentManagementDeatils.FirstOrDefault();
                    objContentManagement.Resoucecatagory = ResourceDeatils.Resoucecatagory;
                    objContentManagement.EffectiveDate = ResourceDeatils.EffectiveDate;
                    objContentManagement.ExpiryDate = ResourceDeatils.ExpiryDate;
                }
            }
            return objContentManagement;
        }
        public ContentManagement DeleteResource(ContentManagement objContentManagement)
        {
            var tblResourceCatagories = Entities.tblResourceCatagories.Where(x => x.ResourceCatagoryPK == objContentManagement.ResourceID).ToList();
            if (tblResourceCatagories != null && tblResourceCatagories.Count() > 0)
            {
                foreach (var item in tblResourceCatagories)
                {
                   for(int i=0;i< item.tblResourceCategoryDetails.Count;i++)
                    { 
                        Entities.Entry(item.tblResourceCategoryDetails.FirstOrDefault()).State = System.Data.Entity.EntityState.Deleted;
                    }
                    Entities.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                Entities.SaveChanges();
            }
            objContentManagement=FetchResourceCatagory(objContentManagement);
            return objContentManagement;
        }
        public ContentManagement DeleteResourceDetails(ContentManagement objContentManagement)
        {
            var tblResourceCatagoriesDetails = Entities.tblResourceCategoryDetails.Where(x => x.ResourcecatagoryDetailsPK == objContentManagement.ResourceChildID).First();
            Entities.Entry(tblResourceCatagoriesDetails).State = System.Data.Entity.EntityState.Deleted;
            Entities.SaveChanges();
            objContentManagement=FetchResource(objContentManagement);
            return objContentManagement;
        }
        public ContentManagement EditResourceContent(ContentManagement objContentManagement)
        {         
            objContentManagement = FetchResourceCatagoryMaster(objContentManagement);          
               var tblResourceCategoryDetails = Entities.tblResourceCategoryDetails.Where(x => x.ResourcecatagoryDetailsPK == objContentManagement.ResourceChildID).FirstOrDefault();
                if (tblResourceCategoryDetails != null)
                {                
                objContentManagement.objContentManagementDeatils.ContentLanguage =Convert.ToInt32(tblResourceCategoryDetails.ContentLanguage);
                objContentManagement.objContentManagementDeatils.ContentType = Convert.ToInt32(tblResourceCategoryDetails.ContentType);
                objContentManagement.objContentManagementDeatils.Link = tblResourceCategoryDetails.Link;
                objContentManagement.objContentManagementDeatils.FileDescription = tblResourceCategoryDetails.FileDescription;
                objContentManagement.objContentManagementDeatils.ExpiryDate = Convert.ToDateTime(tblResourceCategoryDetails.ExpiryDate).ToddMMyyyyString();
                objContentManagement.objContentManagementDeatils.EffectiveDate = Convert.ToDateTime(tblResourceCategoryDetails.StartDate).ToddMMyyyyString();                
            }            
            return objContentManagement;
        }
        public ResouceManagent ContentList(ResouceManagent ObjResouceManagent)
        {
            ObjResouceManagent.lstResourceCatagory = new List<ResourceCatagory>();
            var ContentResource = Entities.tblResourceCatagories.Where(x=>x.Effectivedate<=DateTime.Today && x.Expirydate>=DateTime.Today).ToList();
            if (ContentResource != null && ContentResource.Count()>0)
            {
                foreach (var item in ContentResource)
                {
                    ResourceCatagory objResourceCatagory = new ResourceCatagory();
                    objResourceCatagory.Resoucecatagory = item.ResourceCategory;
                    objResourceCatagory.ResoucecatagoryPK = item.ResourceCatagoryPK;
                    objResourceCatagory.EffectiveDate =Convert.ToDateTime(item.Effectivedate).ToddMMyyyyString();
                    objResourceCatagory.ExpiryDate = Convert.ToDateTime(item.Expirydate).ToddMMyyyyString();
                    if (item.tblResourceCategoryDetails != null && item.tblResourceCategoryDetails.Count() > 0)
                    {
                        objResourceCatagory.lstResourceCatagorydetails = new List<ResourceCatagorydetails>();
                        foreach (var item1 in item.tblResourceCategoryDetails.ToList())
                        {
                            ResourceCatagorydetails objResourceCatagorydetails = new ResourceCatagorydetails();
                            objResourceCatagorydetails.ContentLanguage = item1.ContentLanguage;
                            objResourceCatagorydetails.ContentType = item1.ContentType;
                            if (item1.ContentType == 2879)
                            {
                                objResourceCatagorydetails.Link = item1.Link;
                            }
                            else
                            {
                                objResourceCatagorydetails.Link = "ContentUpload/" + item1.FileName;
                            }
                            objResourceCatagorydetails.imgLink = "ContentUpload/" + item1.ImgName;
                            objResourceCatagorydetails.FileDescription = item1.FileDescription;
                            objResourceCatagorydetails.EffectiveDate =Convert.ToDateTime(item1.StartDate).ToddMMyyyyString();
                            objResourceCatagorydetails.ExpiryDate = Convert.ToDateTime(item1.ExpiryDate).ToddMMyyyyString();
                            objResourceCatagorydetails.ResoucecatagoryDetailsPK = item1.ResourcecatagoryDetailsPK;
                            objResourceCatagory.lstResourceCatagorydetails.Add(objResourceCatagorydetails);
                        }
                    }
                    ObjResouceManagent.lstResourceCatagory.Add(objResourceCatagory);
                }
            }
            return ObjResouceManagent;
        }

    }
}
