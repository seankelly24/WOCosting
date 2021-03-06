﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WOCosting
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class thas01Entities : DbContext
    {
        public thas01Entities()
            : base("name=thas01Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<WorksOrder> WorksOrders { get; set; }
        public virtual DbSet<WorksOrderTransfer> WorksOrderTransfers { get; set; }
    
        public virtual int CostCompletedWO_CostWorksOrder(string worksOrderNumber, string worksOrderSuffix, Nullable<int> employee)
        {
            var worksOrderNumberParameter = worksOrderNumber != null ?
                new ObjectParameter("WorksOrderNumber", worksOrderNumber) :
                new ObjectParameter("WorksOrderNumber", typeof(string));
    
            var worksOrderSuffixParameter = worksOrderSuffix != null ?
                new ObjectParameter("WorksOrderSuffix", worksOrderSuffix) :
                new ObjectParameter("WorksOrderSuffix", typeof(string));
    
            var employeeParameter = employee.HasValue ?
                new ObjectParameter("Employee", employee) :
                new ObjectParameter("Employee", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("CostCompletedWO_CostWorksOrder", worksOrderNumberParameter, worksOrderSuffixParameter, employeeParameter);
        }
    
        public virtual ObjectResult<THAS_CONNECT_GetCompletedWorksorders_Result> THAS_CONNECT_GetCompletedWorksorders()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<THAS_CONNECT_GetCompletedWorksorders_Result>("THAS_CONNECT_GetCompletedWorksorders");
        }
    
        public virtual ObjectResult<THAS_CONNECT_GetSingleWorksOrder_Result> THAS_CONNECT_GetSingleWorksOrder(string wONumber)
        {
            var wONumberParameter = wONumber != null ?
                new ObjectParameter("WONumber", wONumber) :
                new ObjectParameter("WONumber", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<THAS_CONNECT_GetSingleWorksOrder_Result>("THAS_CONNECT_GetSingleWorksOrder", wONumberParameter);
        }
    
        public virtual int THAS_CONNECT_WOCostingEnforcePrep()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("THAS_CONNECT_WOCostingEnforcePrep");
        }
    
        public virtual int THAS_CONNECT_WOCostingOrganicPrep()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("THAS_CONNECT_WOCostingOrganicPrep");
        }
    
        public virtual ObjectResult<string> THAS_CONNECT_EnforcedWOs()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("THAS_CONNECT_EnforcedWOs");
        }
    }
}
