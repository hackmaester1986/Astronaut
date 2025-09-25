using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StargateAPI.Business.Data
{
    [Table("ProcessLogs")]
    public class ProcessLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Level { get; set; } = string.Empty; // e.g., "INFO", "ERROR"
        public string Message { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
        public string? Context { get; set; } // optional: e.g. "PersonService.AddPersonAsync"
    }


    public class ProcessLogConfiguration : IEntityTypeConfiguration<ProcessLog>
    {
        public void Configure(EntityTypeBuilder<ProcessLog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.Level)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Message)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Timestamp)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}