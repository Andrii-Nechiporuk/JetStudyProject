using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetStudyProject.Core.Entities;

namespace JetStudyProject.Infrastracture.Configurations
{
    public class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasMany(x => x.Lecturers).WithOne(x => x.Event).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(x => x.Students).WithOne(x => x.Event).HasForeignKey(x => x.EventId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
