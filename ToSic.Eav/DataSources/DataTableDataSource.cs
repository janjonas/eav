﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ToSic.Eav.DataSources
{
	/// <summary>
	/// Provide Entities from a System.Data.DataTable
	/// </summary>
	public class DataTableDataSource : BaseDataSource
	{
		private IDictionary<int, IEntity> _entities;

		#region Configuration-properties

		private const string TitleFieldKey = "TitleField";
		private const string EntityIdFieldKey = "EntityIdField";
		private const string ContentTypeKey = "ContentType";

		/// <summary>
		/// Default Name of the EntityId Column
		/// </summary>
		public static readonly string EntityIdDefaultColumnName = "EntityId";

		/// <summary>
		/// Default Name of the EntityTitle Column
		/// </summary>
		public static readonly string EntityTitleDefaultColumnName = "EntityTitle";

		/// <summary>
		/// Source DataTable
		/// </summary>
		public DataTable Source { get; set; }

		/// <summary>
		/// Gets or sets the Name of the ContentType
		/// </summary>
		public string ContentType
		{
			get { return Configuration[ContentTypeKey]; }
			set { Configuration[ContentTypeKey] = value; }
		}

		/// <summary>
		/// Gets or sets the Name of the Title Attribute of the Source DataTable
		/// </summary>
		public string TitleField
		{
			get { return Configuration[TitleFieldKey]; }
			set { Configuration[TitleFieldKey] = value; }
		}

		/// <summary>
		/// Gets or sets the Name of the Column used as EntityId
		/// </summary>
		public string EntityIdField
		{
			get { return Configuration[EntityIdFieldKey]; }
			set { Configuration[EntityIdFieldKey] = value; }
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the DataTableDataSource class
		/// </summary>
		public DataTableDataSource()
		{
			Out.Add(DataSource.DefaultStreamName, new DataStream(this, DataSource.DefaultStreamName, GetEntities));
			Configuration.Add(TitleFieldKey, EntityTitleDefaultColumnName);
			Configuration.Add(EntityIdFieldKey, EntityIdDefaultColumnName);
			Configuration.Add(ContentTypeKey, "[Settings:ContentType]");
		}

		/// <summary>
		/// Initializes a new instance of the DataTableDataSource class
		/// </summary>
		public DataTableDataSource(DataTable source, string contentType, string entityIdField = null, string titleField = null)
			: this()
		{
			Source = source;
			ContentType = contentType;
			TitleField = titleField;
			EntityIdField = entityIdField ?? EntityIdDefaultColumnName;
			TitleField = titleField ?? EntityTitleDefaultColumnName;
		}

		private IDictionary<int, IEntity> GetEntities()
		{
			if (_entities != null)
				return _entities;

			EnsureConfigurationIsLoaded();

			_entities = ConvertToEntityDictionary(Source, ContentType, EntityIdField, TitleField);

			return _entities;
		}

		/// <summary>
		/// Convert a DataTable to a Dictionary of EntityModels
		/// </summary>
		private static Dictionary<int, IEntity> ConvertToEntityDictionary(DataTable source, string contentType, string entityIdField, string titleField)
		{
			// Validate Columns
			if (!source.Columns.Contains(entityIdField))
				throw new Exception(string.Format("DataTable doesn't contain an EntityId Column with Name \"{0}\"", entityIdField));
			if (!source.Columns.Contains(titleField))
				throw new Exception(string.Format("DataTable doesn't contain an EntityTitle Column with Name \"{0}\"", titleField));

			// Pupulate a new Dictionary with EntityModels
			var result = new Dictionary<int, IEntity>();
			foreach (DataRow row in source.Rows)
			{
				var entityId = Convert.ToInt32(row[entityIdField]);
				var values = row.Table.Columns.Cast<DataColumn>().Where(c => c.ColumnName != entityIdField).ToDictionary(c => c.ColumnName, c => row.Field<object>(c.ColumnName));
				var entity = new EntityModel(entityId, contentType, values, titleField);
				result.Add(entity.EntityId, entity);
			}
			return result;
		}
	}
}