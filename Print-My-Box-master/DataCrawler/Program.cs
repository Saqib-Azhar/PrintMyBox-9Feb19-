using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Practicing_OAuth.Models;
using Practicing_OAuth;
using Practicing_OAuth.Controllers;

namespace DataCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            DataCrawler();
            Console.ReadKey();
        }
        public static async Task DataCrawler()
        {
            try
            {
                List<Product1> ProductList = new List<Product1>();
                int count = 0;
                int failureCount = 0;
                var url = "http://printmybox.com/";
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var uls = htmlDocument.DocumentNode.Descendants("ul")?.Where(node => node.GetAttributeValue("class", "").Equals("dropdown-menu c-menu-type-classic c-pull-left"))?.ToList();

                foreach (var ul in uls)
                {
                    var catType = ul.Ancestors("li")?.FirstOrDefault()?.Descendants("a")?.FirstOrDefault()?.InnerText;
                    var CategoryList = ul?.Descendants("li")?.Where(nodes => nodes.GetAttributeValue("class", "").Equals("dropdown-submenu")).ToList();



                    foreach (var firstChild in CategoryList)
                    {
                        var productName = "";
                        var catName = firstChild?.Descendants("a")?.FirstOrDefault()?.InnerText;
                        var productList = firstChild?.Descendants("ul")?.FirstOrDefault()?.Descendants("li")?.ToList();
                        foreach (var childLi in productList)
                        {

                            try
                            {
                                count++;
                                Product1 product = new Product1();
                                productName = childLi?.Descendants("a")?.FirstOrDefault()?.InnerText;
                                var productLinkPSec = childLi?.Descendants("a")?.FirstOrDefault()?.ChildAttributes("href")?.FirstOrDefault()?.Value;
                                var productLink = "http://printmybox.com" + productLinkPSec;

                                var prodhtml = await httpClient.GetStringAsync(productLink);
                                var prodhtmlDocument = new HtmlDocument();
                                prodhtmlDocument.LoadHtml(prodhtml);

                                var descriptionDiv = prodhtmlDocument?.DocumentNode.Descendants("div")?.Where(node => node.GetAttributeValue("id", "").Equals("tab-31"))?.ToList();
                                var description = descriptionDiv?.FirstOrDefault()?.Descendants("ul")?.FirstOrDefault()?.Descendants("li")?.FirstOrDefault()?.Descendants("div")?.Where(node => node.GetAttributeValue("class", "").Equals("col-sm-11 col-xs-11")).FirstOrDefault()?.Descendants("p")?.FirstOrDefault()?.InnerText;

                                var imageList = prodhtmlDocument?.DocumentNode.Descendants("div").Where(node => node.GetAttributeValue("class", "").Equals("c-media c-content-overlay")).ToList();
                                if (imageList.Count == 5)
                                {
                                    product.Image1 = imageList[0].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                    product.Image2 = imageList[1].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                    product.Image3 = imageList[2].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                    product.Image4 = imageList[3].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                    product.Image5 = imageList[4].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                }
                                else if (imageList.Count == 2)
                                {
                                    product.Image1 = imageList[0].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                    product.Image2 = imageList[1].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                }
                                else if (imageList.Count == 3)
                                {
                                    product.Image1 = imageList[0].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                    product.Image2 = imageList[1].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                    product.Image3 = imageList[2].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                }
                                else if (imageList.Count == 4)
                                {
                                    product.Image1 = imageList[0].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                    product.Image2 = imageList[1].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                    product.Image3 = imageList[2].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                    product.Image4 = imageList[3].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                }
                                else
                                {
                                    product.Image1 = imageList[0].Descendants("img")?.FirstOrDefault()?.ChildAttributes("src")?.FirstOrDefault()?.Value;
                                }
                                product.CategoryType = catType;
                                product.Category = catName;
                                product.Description = description.Trim();
                                product.ProductName = productName;
                                ProductList.Add(product);
                                Console.WriteLine(count + ": " + product.ProductName);
                            }
                            catch (Exception ex)
                            {
                                failureCount = failureCount + 1;
                                Console.WriteLine("Error at categoy: " + catName + " product name: " + productName);
                            }


                        }
                    }


                }

    
                using (modelEntities dbObj = new modelEntities())
                {
                    var categoriesByIndustory = ProductList.Where(s => s.CategoryType == "By Indsutries").Select(s => s.Category).Distinct().ToList();
                    var categoriesByStyle = ProductList.Where(s => s.CategoryType == "By Box Style").Select(s => s.Category).Distinct().ToList();
                    foreach (var item in categoriesByIndustory)
                    {

                        var category = new Category();
                        category.CategoryName = item;
                        category.CategoryTypeId = 1;
                        dbObj.Categories.Add(category);
                        dbObj.SaveChanges();

                    }
                    foreach (var item in categoriesByStyle)
                    {

                        var category = new Category()
                        {
                            CategoryName = item,
                            CategoryTypeId = 2
                        };
                        dbObj.Categories.Add(category);
                        dbObj.SaveChanges();

                    }
                    foreach (var item in ProductList)
                    {
                        var product = new Product()
                        {
                            Name = item.ProductName,
                            CategoryId = dbObj.Categories.FirstOrDefault(s => s.CategoryName == item.Category).Id,
                            Description = item.Description,
                            Image1 = item.Image1,
                            Image2 = item.Image2,
                            Image3 = item.Image3,
                            Image4 = item.Image4,
                            Image5 = item.Image5,
                            UploadedDate = DateTime.Now,
                            IsEnabled = true
                        };
                        dbObj.Products.Add(product);
                        dbObj.SaveChanges();

                    }
                    var list = dbObj.Categories.ToList();
                    foreach (var item in list)
                    {
                        Console.WriteLine(item.CategoryName);
                    }
                }
                Console.WriteLine("total number of products: " + ProductList.Count);
                Console.WriteLine("total number of failure products: " + failureCount);

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }


    public class Product1
    {
        public string CategoryType { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Image5 { get; set; }
    }
}
