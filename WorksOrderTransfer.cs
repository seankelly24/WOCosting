//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class WorksOrderTransfer
    {
        public int WOTID { get; set; }
        public string WorksOrderNumber { get; set; }
        public string WorksOrderSuffix { get; set; }
        public string ParentSuffix { get; set; }
        public string SerialNumberRange { get; set; }
        public Nullable<short> SerialNumberFormatID { get; set; }
        public Nullable<bool> OnHold { get; set; }
        public Nullable<System.DateTime> DateCreatedParentSuffix { get; set; }
        public short WorksOrderStatusCode { get; set; }
        public Nullable<bool> StockReceived { get; set; }
        public decimal BatchQuantity { get; set; }
        public Nullable<decimal> QuantityStored { get; set; }
        public System.DateTime CompletionDate { get; set; }
        public Nullable<System.DateTime> PlannedIssueDate { get; set; }
        public decimal PlannedMaterialCost { get; set; }
        public decimal PlannedSubcontractCost { get; set; }
        public decimal PlannedRunCost { get; set; }
        public decimal PlannedSetCost { get; set; }
        public System.DateTime PlannedDateSet { get; set; }
        public Nullable<System.DateTime> ActualIssueDate { get; set; }
        public Nullable<System.DateTime> ActualReceiptDate { get; set; }
        public Nullable<decimal> ActualMaterialCost { get; set; }
        public Nullable<decimal> ActualSubcontractCost { get; set; }
        public Nullable<decimal> ActualRunCost { get; set; }
        public Nullable<decimal> ActualSetCost { get; set; }
        public Nullable<System.DateTime> ActualDateSet { get; set; }
        public System.DateTime LastModifiedDate { get; set; }
        public short Printed { get; set; }
        public Nullable<bool> Spooled { get; set; }
        public bool ShowRePrintTag { get; set; }
        public bool OnConcession { get; set; }
        public short WorksOrderCostStatusCode { get; set; }
        public bool Exclude { get; set; }
        public bool QualityAssurance { get; set; }
        public bool IsEnforced { get; set; }
        public int RaisedBy { get; set; }
        public Nullable<int> ResponsibilityCodeID { get; set; }
        public int WorksOrderTypeID { get; set; }
        public decimal PlannedLandedCost1 { get; set; }
        public decimal PlannedLandedCost2 { get; set; }
        public decimal PlannedLandedCost3 { get; set; }
        public decimal PlannedLandedCost4 { get; set; }
        public decimal PlannedLandedCost5 { get; set; }
    
        public virtual WorksOrder WorksOrder { get; set; }
    }
}