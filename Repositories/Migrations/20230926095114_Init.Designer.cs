﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repositories;

#nullable disable

namespace Repositories.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230926095114_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dal.Models.Experement", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Experements");
                });

            modelBuilder.Entity("Dal.Models.HelperModel.TokenExperement", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int");

                    b.Property<int>("ExperementId")
                        .HasColumnType("int");

                    b.HasKey("TokenId", "ExperementId");

                    b.HasIndex("ExperementId");

                    b.ToTable("TokenExperements");
                });

            modelBuilder.Entity("Dal.Models.TokenModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Dal.Models.HelperModel.TokenExperement", b =>
                {
                    b.HasOne("Dal.Models.Experement", "Experement")
                        .WithMany("Tokens")
                        .HasForeignKey("ExperementId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Dal.Models.TokenModel", "TokenModel")
                        .WithMany("Experements")
                        .HasForeignKey("TokenId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Experement");

                    b.Navigation("TokenModel");
                });

            modelBuilder.Entity("Dal.Models.Experement", b =>
                {
                    b.Navigation("Tokens");
                });

            modelBuilder.Entity("Dal.Models.TokenModel", b =>
                {
                    b.Navigation("Experements");
                });
#pragma warning restore 612, 618
        }
    }
}
