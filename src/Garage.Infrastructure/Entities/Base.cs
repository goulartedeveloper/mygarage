using System;

namespace Garage.Infrastructure.Entities;

public abstract class Base
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}