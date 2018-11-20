﻿using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvHelper.Tests.AttributeMapping
{
	[TestClass]
	public class TypeConverterTests
	{
		[TestMethod]
		public void TypeConverterTest()
		{
			using (var reader = new StringReader("Id,Name\r\n1,one\r\n"))
			using (var csv = new CsvReader(reader))
			{
				var records = csv.GetRecords<TypeConverterClass>().ToList();

				Assert.AreEqual(1, records[0].Id);
				Assert.AreEqual("two", records[0].Name);
			}
		}

		[TestMethod]
		public void TypeConverterOnClassReferenceTest()
		{
			var records = new List<AClass>
			{
				new AClass { Id = 1, Name = new BClass() },
			};
			using (var writer = new StringWriter())
			using (var csv = new CsvWriter(writer))
			{
				csv.WriteRecords(records);

				var expected = "Id,Name\r\n1,two\r\n";

				Assert.AreEqual(expected, writer.ToString());
			}
		}

		[TestMethod]
		public void TypeConverterOnStructReferenceTest()
		{
			var records = new List<AStruct>
			{
				new AStruct { Id = 1, Name = new BStruct() },
			};
			using (var writer = new StringWriter())
			using (var csv = new CsvWriter(writer))
			{
				csv.WriteRecords(records);

				var expected = "Id,Name\r\n1,two\r\n";

				Assert.AreEqual(expected, writer.ToString());
			}
		}

		private class TypeConverterClass
		{
			public int Id { get; set; }

			[TypeConverter(typeof(StringTypeConverter))]
			public string Name { get; set; }
		}

		private class StringTypeConverter : ITypeConverter
		{
			public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
			{
				return "two";
			}

			public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
			{
				return "two";
			}
		}

		public class AClass
		{
			public int Id { get; set; }
			[TypeConverter(typeof(StringTypeConverter))]
			public BClass Name { get; set; }
		}

		public class BClass { }

		public class AStruct
		{
			public int Id { get; set; }
			[TypeConverter(typeof(StringTypeConverter))]
			public BStruct Name { get; set; }
		}

		public class BStruct { }
	}
}