using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoolApplicationMain.Models
{
    public class Product
    {
		int itemNumber;
		//string brand;
		//string color;
		string discription;

		public int ItemNumber { get => itemNumber; set => itemNumber = value; }
		//public string Brand { get => brand; set => brand = value; }
		//public string Color { get => color; set => color = value; }
		public string Description { get => discription; set => discription = value; }
		public string LongDescription { get; set; }
		public string UnitOfMeasure { get; set; }
		public string Color { get; set; }
		public string Size { get; set; }
		public string Brand { get; set; }
		public string Discount { get; set; } = "0.0";
		public double Price { get; set; }
		public string Stock { get; set; }
		public string FamilyName { get; set; }
		public string ClassName { get; set; }
		public string CommodityName { get; set; }
	}
}
