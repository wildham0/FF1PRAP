using Last.Data.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FF1PRAP.JsonConverters
{
	public class ProductConverter : JsonConverter<Product>
	{
		public override Product Read(
			ref Utf8JsonReader reader,
			Type typeToConvert,
			JsonSerializerOptions options)
		{
			Product product = new();

			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndObject)
				{
					return product;
				}

				if (reader.TokenType == JsonTokenType.PropertyName)
				{
					var propertyName = reader.GetString();
					reader.Read();
					switch (propertyName)
					{
						case "ContentId":
							product.ContentId = reader.GetInt32();
							break;
						case "GroupId":
							product.GroupId = reader.GetInt32();
							break;
						case "Id":
							product.Id = reader.GetInt32();
							break;
						case "PurchaseLimit":
							product.PurchaseLimit = reader.GetInt32();
							break;
					}
				}
			}
			throw new JsonException();
		}


		public override void Write(
			Utf8JsonWriter writer,
			Product product,
			JsonSerializerOptions options)
		{
			writer.WriteStartObject();
			writer.WriteNumber("ContentId", product.ContentId);
			writer.WriteNumber("GroupId", product.GroupId);
			writer.WriteNumber("Id", product.Id);
			writer.WriteNumber("PurchaseLimit", product.PurchaseLimit);
			writer.WriteEndObject();
		}
	}
}
