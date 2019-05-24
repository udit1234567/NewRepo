using AIA.CrossCutting;
using AIA.Life.Models.Allocation;
using AIA.Life.Models.Common;
using AIA.Life.Repository.AIAEntity;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AIA.Life.Data.API.ControllerLogic.Allocation
{
    public class AllocationLogic
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Environment.MachineName);
        public AllocationModel LoadAllocationDetails(AllocationModel objAllocationModel)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    string RoleID = Context.AspNetRoles.Where(a => a.Name == "UW User").FirstOrDefault().Id;
                    var idParam = new SqlParameter
                    {
                        ParameterName = "@RoleId",
                        Value = RoleID
                    };
                    List<string> UserIDs = Context.Database.SqlQuery<string>(
                       "exec GetUsersByRoleId @RoleId", idParam).ToList();

                    if (UserIDs != null)
                    {
                        objAllocationModel.objUWdetails = (from aspnetusers in Context.AspNetUsers.Where(a => UserIDs.Contains(a.Id))
                                                           join userdetails in Context.tblUserDetails
                                                           on aspnetusers.UserName equals userdetails.LoginID
                                                           select new UWDetails
                                                           {
                                                               UWName = aspnetusers.UserName,
                                                               ID = aspnetusers.Id,
                                                               Availabiliy = false
                                                           }).ToList();

                    }
                    objAllocationModel.objChannelDetails = (from objchannel in Context.tblmasChannels
                                                            select new ChannelDetails
                                                            {
                                                                ChannelName = objchannel.ChannelName,
                                                                ChannelId = objchannel.ChannelID,
                                                                Availabiliy = false
                                                            }).ToList();

                    objAllocationModel.Message = "Success";
                    return objAllocationModel;
                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objAllocationModel.Message = "Error";
                return objAllocationModel;


            }
        }
        public ManualAllocation ManualAllocationDetails(ManualAllocation objAllocationModel)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    string RoleID = Context.AspNetRoles.Where(a => a.Name == "UW User").FirstOrDefault().Id;
                    var idParam = new SqlParameter
                    {
                        ParameterName = "@RoleId",
                        Value = RoleID
                    };
                    List<string> UserIDs = Context.Database.SqlQuery<string>(
                       "exec GetUsersByRoleId @RoleId", idParam).ToList();

                    if (UserIDs != null)
                    {
                        objAllocationModel.LstUWName = (from aspnetusers in Context.AspNetUsers.Where(a => UserIDs.Contains(a.Id) && a.LockoutEndDateUtc==null) 
                                                        join userdetails in Context.tblUserDetails
                                                        on aspnetusers.UserName equals userdetails.LoginID
                                                        select new MasterListItem
                                                        {
                                                            Text = aspnetusers.UserName,
                                                            Value = aspnetusers.Id
                                                        }).ToList();
                        int i = 0;
                        objAllocationModel.objLstAllocationProposals = (from Proposal in Context.tblPolicies.Where(a => a.IsAllocated == false)
                                                                        select new AllocationProposals
                                                                        {
                                                                            ProposalNoDisplay = Proposal.ProposalNo,
                                                                            ProposalNo = Proposal.ProposalNo,
                                                                            PolicyID = Proposal.PolicyID
                                                                        }).OrderByDescending(a => a.PolicyID).ToList().Select((item, index) => new AllocationProposals
                                                                        {
                                                                            ProposalNoDisplay = item.ProposalNoDisplay,
                                                                            ProposalNo = item.ProposalNo,
                                                                            Index = index,
                                                                            PolicyID = item.PolicyID
                                                                        }).ToList();

                        objAllocationModel.objLstResetProposals = (from Proposal in Context.tblPolicies.Where(a => a.PolicyStageStatusID == 193) // Ref to UW
                                                                   join UWAllocation in Context.tblPolicyUWAllocations
                                                                   on Proposal.PolicyID equals UWAllocation.PolicyID
                                                                   join ASpnetUsers in Context.AspNetUsers
                                                                   on UWAllocation.AllocatedTo equals ASpnetUsers.Id
                                                                   where Proposal.ModifiedDate == UWAllocation.AllocatedDate
                                                                   select new AllocationProposals
                                                                   {
                                                                       ProposalNoDisplay = Proposal.ProposalNo,
                                                                       ProposalNo = Proposal.ProposalNo,
                                                                       UWName = ASpnetUsers.UserName,
                                                                       PolicyID = Proposal.PolicyID

                                                                   }).OrderByDescending(a => a.PolicyID).ToList().Select((item, index) => new AllocationProposals
                                                                   {
                                                                       ProposalNoDisplay = item.ProposalNoDisplay,
                                                                       ProposalNo = item.ProposalNo,
                                                                       UWName = item.UWName,
                                                                       Index = index,
                                                                       PolicyID = item.PolicyID
                                                                   }).ToList();

                        if (objAllocationModel.objLstAllocationProposals == null)
                        {
                            objAllocationModel.objLstAllocationProposals = new List<AllocationProposals>();
                        }

                        if (objAllocationModel.objLstResetProposals == null)
                        {
                            objAllocationModel.objLstResetProposals = new List<AllocationProposals>();
                        }

                    }


                    objAllocationModel.Message = "Success";
                    return objAllocationModel;
                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objAllocationModel.Message = "Error";
                return objAllocationModel;


            }
        }
        public AllocationModel SaveAllocation(AllocationModel objAllocationModel)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    DeleteExisitingAllocationInfo();

                    #region Add Allocation Info
                    foreach (var UWDetails in objAllocationModel.objUWdetails)
                    {
                        tblUWAllocation objUWAllocation = new tblUWAllocation();
                        objUWAllocation.UWName = UWDetails.UWName;
                        objUWAllocation.Availability = UWDetails.Availabiliy;
                        Context.tblUWAllocations.Add(objUWAllocation);


                        var UserInfo = Context.AspNetUsers.Where(a => a.UserName == UWDetails.UWName).FirstOrDefault();
                        if (UserInfo != null)
                        {
                            Guid _Guid = new Guid(UserInfo.Id);
                            var UserDetails = Context.tblUserDetails.Where(a => a.UserID == _Guid).FirstOrDefault();
                            tblUserAvalibility objUserAvailability = new tblUserAvalibility();
                            objUserAvailability.NodeId = UserDetails.NodeID;
                            objUserAvailability.UserAvailability = UWDetails.Availabiliy;
                            Context.tblUserAvalibilities.Add(objUserAvailability);
                        }


                    }


                    foreach (var ChannelDetails in objAllocationModel.objChannelDetails)
                    {
                        tblChannelAllocation objChannelAllocation = new tblChannelAllocation();
                        objChannelAllocation.ChannelName = ChannelDetails.ChannelName;
                        objChannelAllocation.Availability = ChannelDetails.Availabiliy;
                        Context.tblChannelAllocations.Add(objChannelAllocation);
                    }
                    Context.SaveChanges();
                    #endregion

                    #region Call Allocation

                    List<AllocationSummary> Result = Context.Database.SqlQuery<AllocationSummary>(
                       "exec usp_AllocateProposals").ToList();

                    if (Result != null)
                    {
                        objAllocationModel.objAllocationSummary = new List<AllocationSummary>();
                        objAllocationModel.objAllocationSummary = Result;
                    }
                    #endregion
                    objAllocationModel.Message = "Success";
                    return objAllocationModel;
                }
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objAllocationModel.Message = "Error";
                return objAllocationModel;


            }
        }
        public AllocationModel ResetAllocation(AllocationModel objAllocationModel)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    List<string> UWNames = objAllocationModel.objUWdetails.Where(a => a.IsChecked == true).Select(a => a.UWName).ToList();
                    if (UWNames != null)
                    {
                        string UserInfo = string.Empty;
                        #region Coma Seperated User Ids 

                        int Count = 0;
                        foreach (string Userid in Context.AspNetUsers.Where(a => UWNames.Contains(a.UserName)).Select(a => a.Id).ToList())
                        {
                            if (Count == 0)
                            {
                                UserInfo = Userid;
                            }
                            else
                            {
                                UserInfo = "," + Userid;
                            }
                            Count++;

                        }
                        #endregion


                        var idParam = new SqlParameter
                        {
                            ParameterName = "UserId",
                            Value = UserInfo
                        };


                        var Result = Context.Database.SqlQuery<string>(
                            "exec usp_UnAllocateProposals @UserId ", idParam).FirstOrDefault();
                    }

                }
                objAllocationModel.Message = "Success";
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objAllocationModel.Message = "Error";
            }
            return objAllocationModel;
        }
        public void DeleteExisitingAllocationInfo()
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    var LstUWAllocation = Context.tblUWAllocations.ToList();
                    Context.tblUWAllocations.RemoveRange(LstUWAllocation);

                    var LstChannelAllocation = Context.tblChannelAllocations.ToList();
                    Context.tblChannelAllocations.RemoveRange(LstChannelAllocation);

                    var LstUserAvailability = Context.tblUserAvalibilities.ToList();
                    Context.tblUserAvalibilities.RemoveRange(LstUserAvailability);

                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {


            }
        }

        public ManualAllocation SaveManualAllocation(ManualAllocation objAllocationModel)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    foreach (var Item in objAllocationModel.objLstAllocationProposals.Where(a => a.ISSelected == true).ToList())
                    {
                        if (!string.IsNullOrEmpty(Item.AssignTo))
                        {

                            DateTime Date = DateTime.Now;
                            var policy = Context.tblPolicies.Where(a => a.ProposalNo == Item.ProposalNo).FirstOrDefault();
                            if (policy != null)
                            {
                                decimal PolicyID = policy.PolicyID;
                                var PolicyAllocation = Context.tblPolicyUWAllocations.Where(a => a.PolicyID == PolicyID).FirstOrDefault();
                                if (PolicyAllocation != null)
                                {
                                    PolicyAllocation.AllocatedFrom = PolicyAllocation.AllocatedTo;
                                    PolicyAllocation.AllocatedTo = Item.AssignTo;
                                    PolicyAllocation.AllocatedDate = Date;
                                    policy.IsAllocated = true;
                                    policy.ModifiedDate = Date;
                                }
                                else
                                {
                                    tblPolicyUWAllocation objPolicyUWAllocation = new tblPolicyUWAllocation();
                                    objPolicyUWAllocation.PolicyID = PolicyID;
                                    objPolicyUWAllocation.AllocatedFrom = string.Empty;
                                    objPolicyUWAllocation.AllocatedTo = Item.AssignTo;
                                    objPolicyUWAllocation.AllocatedDate = Date;
                                    objPolicyUWAllocation.Remarks = string.Empty;
                                    objPolicyUWAllocation.CreatedDate = Date;
                                    Context.tblPolicyUWAllocations.Add(objPolicyUWAllocation);
                                    policy.IsAllocated = true;
                                    policy.ModifiedDate = Date;
                                }

                            }

                            Context.SaveChanges();
                        }

                    }
                }
                objAllocationModel.Message = "Success";
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objAllocationModel.Message = "Error";
            }
            return objAllocationModel;
        }


        public ManualAllocation ResetManualAllocation(ManualAllocation objAllocationModel)
        {
            try
            {
                using (AVOAIALifeEntities Context = new AVOAIALifeEntities())
                {
                    foreach (var Item in objAllocationModel.objLstResetProposals.Where(a => a.ISSelected == true).ToList())
                    {
                        if (!string.IsNullOrEmpty(Item.AssignTo))
                        {
                            DateTime Date = DateTime.Now;
                            var policy = Context.tblPolicies.Where(a => a.ProposalNo == Item.ProposalNo).FirstOrDefault();
                            if (policy != null)
                            {
                                decimal PolicyID = policy.PolicyID;
                                var PolicyAllocation = Context.tblPolicyUWAllocations.Where(a => a.PolicyID == PolicyID).FirstOrDefault();
                                if (PolicyAllocation != null)
                                {
                                    PolicyAllocation.AllocatedFrom = PolicyAllocation.AllocatedTo;
                                    PolicyAllocation.AllocatedTo = Item.AssignTo;
                                    PolicyAllocation.AllocatedDate = Date;
                                    // Context.tblPolicyUWAllocations.Remove(PolicyAllocation);  // As per Ravi
                                }

                            }
                            policy.ModifiedDate = Date;
                            //  policy.IsAllocated = false;
                            Context.SaveChanges();
                        }

                    }
                }
                objAllocationModel.Message = "Success";
            }
            catch (Exception ex)
            {
                log4net.GlobalContext.Properties["ErrorCode"] = Codes.GetErrorCode();
                Logger.Error(ex);
                objAllocationModel.Message = "Error";
            }
            return objAllocationModel;
        }
    }
}