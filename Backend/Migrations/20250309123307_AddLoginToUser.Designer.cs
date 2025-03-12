﻿// <auto-generated />
using System;
using GoodsAndOrders.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GoodsAndOrders.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250309123307_AddLoginToUser")]
    partial class AddLoginToUser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.OrderStatus", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("character varying(20)")
                    .HasColumnName("name");

                b.HasKey("Id")
                    .HasName("pk_order_statuses");

                b.ToTable("order_statuses", (string)null);
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.Product", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<Guid>("CategoryId")
                    .HasColumnType("uuid")
                    .HasColumnName("category_id");

                b.Property<string>("Code")
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasColumnType("character varying(12)")
                    .HasColumnName("code");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("character varying(20)")
                    .HasColumnName("name");

                b.Property<decimal>("Price")
                    .HasColumnType("NUMERIC(18,2)")
                    .HasColumnName("price");

                b.HasKey("Id")
                    .HasName("pk_products");

                b.HasIndex("CategoryId")
                    .HasDatabaseName("ix_products_category_id");

                b.ToTable("products", (string)null);
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.ProductCategory", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnType("character varying(30)")
                    .HasColumnName("name");

                b.HasKey("Id")
                    .HasName("pk_product_categories");

                b.ToTable("product_categories", (string)null);
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.ProductUserOrder", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<int>("ProductCount")
                    .HasColumnType("integer")
                    .HasColumnName("product_count");

                b.Property<Guid>("ProductId")
                    .HasColumnType("uuid")
                    .HasColumnName("product_id");

                b.Property<decimal>("Product_Price")
                    .HasColumnType("NUMERIC(18,2)")
                    .HasColumnName("product_price");

                b.Property<Guid>("UserOrderId")
                    .HasColumnType("uuid")
                    .HasColumnName("user_order_id");

                b.HasKey("Id")
                    .HasName("pk_products_user_orders");

                b.HasIndex("ProductId")
                    .HasDatabaseName("ix_products_user_orders_product_id");

                b.HasIndex("UserOrderId")
                    .HasDatabaseName("ix_products_user_orders_user_order_id");

                b.ToTable("products_user_orders", (string)null);
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.User", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<string>("Address")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("character varying(200)")
                    .HasColumnName("address");

                b.Property<string>("Code")
                    .IsRequired()
                    .HasMaxLength(9)
                    .HasColumnType("character varying(9)")
                    .HasColumnName("code");

                b.Property<Guid?>("CustomerId")
                    .HasColumnType("uuid")
                    .HasColumnName("customer_id");

                b.Property<int>("Discount")
                    .HasColumnType("integer")
                    .HasColumnName("discount");

                b.Property<string>("Login")
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("character varying(20)")
                    .HasColumnName("login");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnType("character varying(30)")
                    .HasColumnName("name");

                b.Property<string>("PasswordHash")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("character varying(255)")
                    .HasColumnName("password_hash");

                b.Property<Guid>("UserRoleId")
                    .HasColumnType("uuid")
                    .HasColumnName("user_role_id");

                b.HasKey("Id")
                    .HasName("pk_users");

                b.HasIndex("CustomerId")
                    .HasDatabaseName("ix_users_customer_id");

                b.HasIndex("UserRoleId")
                    .HasDatabaseName("ix_users_user_role_id");

                b.ToTable("users", (string)null);
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.UserOrder", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<Guid>("CustomerId")
                    .HasColumnType("uuid")
                    .HasColumnName("customer_id");

                b.Property<DateTime>("OrderDate")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("order_date");

                b.Property<int>("OrderNumber")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer")
                    .HasColumnName("order_number");

                NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("OrderNumber"));

                b.Property<DateTime?>("ShipmentDate")
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("shipment_date");

                b.Property<Guid>("StatusId")
                    .HasColumnType("uuid")
                    .HasColumnName("status_id");

                b.HasKey("Id")
                    .HasName("pk_user_orders");

                b.HasIndex("CustomerId")
                    .HasDatabaseName("ix_user_orders_customer_id");

                b.HasIndex("StatusId")
                    .HasDatabaseName("ix_user_orders_status_id");

                b.ToTable("user_orders", (string)null);
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.UserRole", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uuid")
                    .HasColumnName("id");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("character varying(20)")
                    .HasColumnName("name");

                b.HasKey("Id")
                    .HasName("pk_user_roles");

                b.ToTable("user_roles", (string)null);
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.Product", b =>
            {
                b.HasOne("GoodsAndOrders.Data.Entities.ProductCategory", "Category")
                    .WithMany("Products")
                    .HasForeignKey("CategoryId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("fk_products_product_categories_category_id");

                b.Navigation("Category");
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.ProductUserOrder", b =>
            {
                b.HasOne("GoodsAndOrders.Data.Entities.Product", "Product")
                    .WithMany("OrderElements")
                    .HasForeignKey("ProductId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("fk_products_user_orders_products_product_id");

                b.HasOne("GoodsAndOrders.Data.Entities.UserOrder", "UserOrder")
                    .WithMany("OrderElements")
                    .HasForeignKey("UserOrderId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("fk_products_user_orders_user_orders_user_order_id");

                b.Navigation("Product");

                b.Navigation("UserOrder");
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.User", b =>
            {
                b.HasOne("GoodsAndOrders.Data.Entities.User", "Customer")
                    .WithMany("Clients")
                    .HasForeignKey("CustomerId")
                    .HasConstraintName("fk_users_users_customer_id");

                b.HasOne("GoodsAndOrders.Data.Entities.UserRole", "UserRole")
                    .WithMany("Users")
                    .HasForeignKey("UserRoleId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired()
                    .HasConstraintName("fk_users_user_roles_user_role_id");

                b.Navigation("Customer");

                b.Navigation("UserRole");
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.UserOrder", b =>
            {
                b.HasOne("GoodsAndOrders.Data.Entities.User", "Customer")
                    .WithMany("UserOrders")
                    .HasForeignKey("CustomerId")
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired()
                    .HasConstraintName("fk_user_orders_users_customer_id");

                b.HasOne("GoodsAndOrders.Data.Entities.OrderStatus", "OrderStatus")
                    .WithMany("UserOrders")
                    .HasForeignKey("StatusId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired()
                    .HasConstraintName("fk_user_orders_order_statuses_status_id");

                b.Navigation("Customer");

                b.Navigation("OrderStatus");
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.OrderStatus", b =>
            {
                b.Navigation("UserOrders");
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.Product", b =>
            {
                b.Navigation("OrderElements");
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.ProductCategory", b =>
            {
                b.Navigation("Products");
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.User", b =>
            {
                b.Navigation("Clients");

                b.Navigation("UserOrders");
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.UserOrder", b =>
            {
                b.Navigation("OrderElements");
            });

            modelBuilder.Entity("GoodsAndOrders.Data.Entities.UserRole", b =>
            {
                b.Navigation("Users");
            });
#pragma warning restore 612, 618
        }
    }
}
