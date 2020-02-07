using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace KoolApplicationMain.Models
{
    public class Search:IProductInformation
    {
		public string ConnectionString { get; set; }



		public MySqlConnection GetConnection()
		{
			//database connection 
			ConnectionString = @"server=custom-mysql.gamification.svc.cluster.local; port=3306; database=sampledb;uid=xxuser;pwd=welcome1";
			return new MySqlConnection(ConnectionString);
		}

		public List<Product> GetProductsInformation()
		{
			List<Product> plist = new List<Product>();
			using (MySqlConnection conn = GetConnection())
			{
				conn.Open();
				MySqlCommand cmd = new MySqlCommand("select XXIBM_PRODUCT_SKU.Item_number,XXIBM_PRODUCT_SKU.description,XXIBM_PRODUCT_SKU.Long_description,XXIBM_PRODUCT_SKU.SKU_ATTRIBUTE_value1,XXIBM_PRODUCT_SKU.SKU_ATTRIBUTE_value2,XXIBM_PRODUCT_SKU.SKU_unit_of_measure,XXIBM_PRODUCT_STYLE.Brand," +
					"XXIBM_PRODUCT_PRICING.List_price,XXIBM_PRODUCT_PRICING.In_stock,XXIBM_PRODUCT_PRICING.Discount," +
	 "XXIBM_PRODUCT_CATALOGUE.Family_name,XXIBM_PRODUCT_CATALOGUE.Class_name,XXIBM_PRODUCT_CATALOGUE.Commodity_name " +
  "from XXIBM_PRODUCT_SKU JOIN XXIBM_PRODUCT_STYLE ON XXIBM_PRODUCT_SKU.Style_item=XXIBM_PRODUCT_STYLE.Item_number JOIN XXIBM_PRODUCT_PRICING ON XXIBM_PRODUCT_SKU.Item_number=XXIBM_PRODUCT_PRICING.Item_number" +
  " JOIN  XXIBM_PRODUCT_CATALOGUE ON XXIBM_PRODUCT_SKU.Catalogue_category=XXIBM_PRODUCT_CATALOGUE.Commodity", conn);
				using (var reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						plist.Add(new Product()
						{
							ItemNumber = Convert.ToInt32(reader["Item_number"]),
							Description = reader["description"].ToString(),
							LongDescription = reader["Long_description"].ToString(),
							UnitOfMeasure = reader["SKU_unit_of_measure"].ToString(),
							Color = reader["SKU_ATTRIBUTE_value2"].ToString(),
							Size = reader["SKU_ATTRIBUTE_value1"].ToString(),
							Brand = reader["Brand"].ToString(),
							Discount = (reader["Discount"].ToString()),
							FamilyName = reader["Family_name"].ToString(),
							ClassName = reader["Class_name"].ToString(),
							CommodityName = reader["Commodity_name"].ToString(),
							Price = Convert.ToDouble(reader["List_price"]),
							Stock = reader["In_stock"].ToString()

						});
					}
				}
			}
			return plist;
		}



		
	}
}
