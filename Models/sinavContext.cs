using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MoodleExam.Models
{
    public partial class sinavContext : DbContext
    {
        public sinavContext()
        {
        }

        public sinavContext(DbContextOptions<sinavContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Exam> Exams { get; set; } = null!;
        public virtual DbSet<Userexam> Userexams { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("Name=ConnectionStrings:DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exam>(entity =>
            {
                entity.ToTable("exam");

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AnswerPdf)
                    .HasMaxLength(256)
                    .HasColumnName("answerPdf");

                entity.Property(e => e.Answers)
                    .HasMaxLength(512)
                    .HasColumnName("answers");

                entity.Property(e => e.Courseid)
                    .HasColumnType("int(11)")
                    .HasColumnName("courseid");

                entity.Property(e => e.ExamPdf)
                    .HasMaxLength(256)
                    .HasColumnName("examPdf");

                entity.Property(e => e.Firstaccesstime)
                    .HasColumnType("datetime")
                    .HasColumnName("firstaccesstime");

                entity.Property(e => e.Lastaccesstime)
                    .HasColumnType("datetime")
                    .HasColumnName("lastaccesstime");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .HasColumnName("name");

                entity.Property(e => e.Timelimit)
                    .HasColumnType("int(11)")
                    .HasColumnName("timelimit");
            });

            modelBuilder.Entity<Userexam>(entity =>
            {
                entity.ToTable("userexam");

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Answers)
                    .HasMaxLength(512)
                    .HasColumnName("answers");

                entity.Property(e => e.Endtime)
                    .HasColumnType("datetime")
                    .HasColumnName("endtime");

                entity.Property(e => e.Examid)
                    .HasColumnType("int(11)")
                    .HasColumnName("examid");

                entity.Property(e => e.Score)
                    .HasMaxLength(128)
                    .HasColumnName("score");

                entity.Property(e => e.Starttime)
                    .HasColumnType("datetime")
                    .HasColumnName("starttime");

                entity.Property(e => e.Userid)
                    .HasColumnType("int(11)")
                    .HasColumnName("userid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
