using CA_PnP_Core.Attributes;
using PnP.Core.Model.SharePoint;

namespace CA_PnP_Core.Models {
    [SPList(listGuid: "128b4e17-691a-4e28-9934-7bd399ba907a", listTitle: "EmployeeTasks")]
    internal class EmployeeTaskModel
    {
        [SPField(internalName: "ID")]
        public string? Id { get; set; }

        [SPField(internalName: "EmployeeTask_Done")]
        public bool? Done { get; set; }

        [SPField(internalName: "EmployeeTask_Employeer")]
        public FieldLookupValue? Employeer { get; set; }
    }
}
