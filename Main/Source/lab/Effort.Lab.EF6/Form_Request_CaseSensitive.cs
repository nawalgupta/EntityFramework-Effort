﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Effort.Lab.EF6
{
	public partial class Form_Request_CaseSensitive : Form
	{
		public Form_Request_CaseSensitive()
		{
			InitializeComponent();
			var connection = Effort.DbConnectionFactory.CreateTransient();

			
			// CLEAN
			using (var context = new EntityContext(connection))
			{
				context.EntitySimples.RemoveRange(context.EntitySimples);
				context.SaveChanges();


			}

			connection.IsCaseSensitive = false;

			// SEED
			using (var context = new EntityContext(connection))
			{
				context.EntitySimples.Add(new EntitySimple { ColumnInt = 1, ColumnString = "allo" });
				context.EntitySimples.Add(new EntitySimple { ColumnInt = 2, ColumnString = "aLlo" });
				context.EntitySimples.Add(new EntitySimple { ColumnInt = 3, ColumnString = "Allo" });
				context.EntitySimples.Add(new EntitySimple { ColumnInt = 3, ColumnString = "ATllo" });
				context.EntitySimples.Add(new EntitySimple { ColumnInt = 3 });
				context.SaveChanges();
			}

			// TEST
			using (var context = new EntityContext(connection))
			{
				var test = context.EntitySimples.Where(x => x.ColumnString.StartsWith("al")).ToList();
			}



			connection.IsCaseSensitive = true;


			// TEST
			using (var context = new EntityContext(connection))
			{
				var test = context.EntitySimples.Where(x => x.ColumnString.StartsWith("al")).ToList();
			}
		}

		public class EntityContext : DbContext
		{
			public EntityContext(DbConnection connection) : base(connection, true)
			{
			}

			public DbSet<EntitySimple> EntitySimples { get; set; }


			protected override void OnModelCreating(DbModelBuilder modelBuilder)
			{
				base.OnModelCreating(modelBuilder);
			}
		}

		public class EntitySimple
		{
			public int ID { get; set; }
			public int ColumnInt { get; set; }
			public String ColumnString { get; set; }
		}
	}
}