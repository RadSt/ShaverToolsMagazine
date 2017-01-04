using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ShaverToolsShop.Data;
using ShaverToolsShop.Conventions.Enums;

namespace ShaverToolsShop.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170104170843_AddSubscriptionTable")]
    partial class AddSubscriptionTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ShaverToolsShop.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<decimal>("Price");

                    b.Property<Guid?>("SubscriptionId");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ShaverToolsShop.Entities.Subscription", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("SubscriptionStatus");

                    b.Property<int>("SubscriptionType");

                    b.HasKey("Id");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("ShaverToolsShop.Entities.Product", b =>
                {
                    b.HasOne("ShaverToolsShop.Entities.Subscription")
                        .WithMany("Products")
                        .HasForeignKey("SubscriptionId");
                });
        }
    }
}
