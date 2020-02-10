using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KoolApplicationMain.Models;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.VisualRecognition.v3;
using IBM.Cloud.SDK.Core.Http;
using IBM.Watson.VisualRecognition.v3.Model;
using Newtonsoft.Json.Linq;

namespace KoolApplicationMain.Controllers
{
    public class HomeController : Controller
    {
        private IProductInformation _productInformation;

        private IHostingEnvironment _iweb;
        public HomeController(IProductInformation productInformation, IHostingEnvironment iweb)
        {
            _productInformation = productInformation;
            _iweb = iweb;
        }
        public IActionResult Index()
        {
            Search search = new Search();
            var model = new List<Product>();
            DataTable dt = new DataTable();
            MySqlDataAdapter mda;
            using (MySqlConnection conn = search.GetConnection())
            {

                //conn.Open();
                //string str = "select XXIBM_PRODUCT_SKU.Item_number,XXIBM_PRODUCT_SKU.description,XXIBM_PRODUCT_PRICING.List_price,XXIBM_PRODUCT_PRICING.In_stock from XXIBM_PRODUCT_SKU JOIN XXIBM_PRODUCT_PRICING ON XXIBM_PRODUCT_SKU.Item_number=XXIBM_PRODUCT_PRICING.Item_number ";

                ////MySqlCommand cmd = new MySqlCommand(, conn);
                //mda = new MySqlDataAdapter(str, search.GetConnection());
                //mda.Fill(dt);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    model.Add(new Product()
                //    {
                //        ItemNumber = Convert.ToInt32(dt.Rows[i]["Item_number"]),
                //        Description = dt.Rows[i]["description"].ToString(),
                //        Price = Convert.ToDouble(dt.Rows[i]["List_price"]),
                //        Stock = dt.Rows[i]["In_stock"].ToString()

                //    });
                //}

            }
            return View(model);

            //return View();
        }
        public IActionResult ProductDetail()
        {

            var list = _productInformation.GetProductsInformation();
            ViewBag.name = "All products";
            return View(list);
        }
        [HttpPost]
        public IActionResult Search(string search)
        {

            string s = search.ToUpper();
            var result = _productInformation.GetProductsInformation();
            result = result.Where(l => string.Compare(l.Brand, s, true) == 0 || l.ClassName.ToUpper().Contains(s) ||
            l.CommodityName.ToUpper().Contains(s) || l.FamilyName.ToUpper().Contains(s) ||
            l.LongDescription.ToUpper().Contains(s) || l.Color.ToUpper().Contains(s) || l.Size.ToUpper().Contains(s)).ToList();
            if (result.Count == 0)
            {
                return View("NoResults");
            }
            ViewBag.name = search;
            return View("EachProductDetails", result);

        }

        public IActionResult Brands(string brand)
        {

            var result = _productInformation.GetProductsInformation();

            result = result.Where(l => string.Compare(l.Brand, brand, true) == 0).ToList();
            if (result.Count == 0)
            {
                return View("NoResults");
            }
            ViewBag.name = brand;
            return View("ProductDetail", result);

        }

        public IActionResult Color(string color)
        {

            var result = _productInformation.GetProductsInformation();

            result = result.Where(l => string.Compare(l.Color, color, true) == 0).ToList();
            if (result.Count == 0)
            {
                return View("NoResults");
            }
            ViewBag.name = color;
            return View("ProductDetail", result);

        }

        public IActionResult Type(string type)
        {

            var result = _productInformation.GetProductsInformation();

            result = result.Where(l => string.Compare(l.CommodityName, type, true) == 0).ToList();
            if (result.Count == 0)
            {
                return View("NoResults");
            }
            ViewBag.name = type;
            return View("ProductDetail", result);

        }



        public IActionResult EachProductDetails()
        {
            var list = _productInformation.GetProductsInformation();
            ViewBag.name = "All products";
            return View(list);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> imageSearch(IFormFile imagefile)
        {
            var result = _productInformation.GetProductsInformation();
            List<string> threshold = new List<string>();
            string ext = Path.GetExtension(imagefile.FileName);
            if (ext == ".jpg" || ext == ".gif")
            {
                var imageSave = Path.Combine(_iweb.WebRootPath, "images", imagefile.FileName);
                var filestream = new FileStream(imageSave, FileMode.Create);
                await imagefile.CopyToAsync(filestream);
                filestream.Close();
                IamAuthenticator authenticator = new IamAuthenticator(
        apikey: "9qgeef7jn__HEuNYblTDxq4eAlbifXYTMBj1b4PGDw7X"
        );
                VisualRecognitionService visualRecognition = new VisualRecognitionService("2018-03-19", authenticator);
                visualRecognition.SetServiceUrl("https://api.us-south.visual-recognition.watson.cloud.ibm.com/instances/2d9c4e1b-6a3a-47f5-808a-2144e02dce84");
                DetailedResponse<ClassifiedImages> results;
                using (FileStream fs = System.IO.File.OpenRead(imageSave))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);
                        results = visualRecognition.Classify(
                            imagesFile: ms,
                            imagesFilename: imagefile.FileName
                            );
                    }
                }
                JObject json = JObject.Parse(results.Response);
                dynamic obj = json["images"][0]["classifiers"][0]["classes"];
                for (int i = 0; i < obj.Count; i++)
                {
                    threshold.Add(obj[i]["class"].ToString().ToLower());
                }
                var brands = result.Where(m => m.Brand != null).Select(m => m.Brand).Distinct();
                var colors = result.Where(m => m.Color != "").Select(m => m.Color).Distinct();
                var sizes = result.Where(m => m.Size != null).Select(m => m.Size).Distinct();
                ViewBag.brand = brands;
                ViewBag.color = colors;
                ViewBag.size = sizes;
                var imgresult = result.Where(l => threshold.Any(s => l.ClassName.ToLower().Contains(s) ||
            threshold.Any(l.CommodityName.ToLower().Contains) || threshold.Any(l.Color.ToLower().Contains) || threshold.Any(l.LongDescription.ToLower().Contains))).ToList();
                FileInfo fi = new FileInfo(imageSave);
                if (fi != null)
                {
                    System.IO.File.Delete(imageSave);
                    fi.Delete();
                }
                if (imgresult.Count == 0)
                {
                    return View("NoResults");
                }
                else
                {
                    return View("EachProductDetails", imgresult);
                }
            }
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
