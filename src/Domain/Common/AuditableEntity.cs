using System;

namespace Domain.Common;

public class AuditableEntity : Entity
{
    public DateTime Created { get; set; }
    public string CreatedBy { get; set; } = "API";
    public DateTime? LastModified { get; set; }
    public string LastModifiedBy { get; set; } = "API";
}