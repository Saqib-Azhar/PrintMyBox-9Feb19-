using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.IO;
using Practicing_OAuth;
using System.Web.Configuration;
using Practicing_OAuth.Models;
using Schema.NET;

namespace Practicing_OAuth.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private static string SenderEmailId = WebConfigurationManager.AppSettings["DefaultEmailId"];
        private static string SenderEmailPassword = WebConfigurationManager.AppSettings["DefaultEmailPassword"];
        private static int SenderEmailPort = Convert.ToInt32(WebConfigurationManager.AppSettings["DefaultEmailPort"]);
        private static string SenderEmailHost = WebConfigurationManager.AppSettings["DefaultEmailHost"];
        private Practicing_OAuthEntities db = new Practicing_OAuthEntities();

        public ActionResult Index()
        {
            return RedirectToAction("IndexView");
        }

        [Route("Index")]
        public ActionResult IndexView()
        {
            var ProductsList = db.Products.ToList();
            ViewBag.ProductsList = ProductsList.Take(15).ToList();
            ViewBag.CategoriesList = db.Categories.ToList();
            ViewBag.CategoryTypesList = db.CategoryTypes.ToList();
            return View("Index");
        }

        //public ActionResult Contact()
        //{
        //    return RedirectToAction("ContactView", "Contact");
        //}

        //public ActionResult Blog()
        //{
        //    return RedirectToAction("BlogView", "Blog");
        //}

        //public ActionResult About()
        //{
        //    return RedirectToAction("AboutView", "About");
        //}

        //public ActionResult PriceQuote()
        //{
        //    return RedirectToAction("PriceQuoteView", "PriceQuote");
        //}
        public ActionResult ContactDetails(FormCollection contactDetails)
        {
            var Name = contactDetails["Name"];
            var Email = contactDetails["Email"];
            var Phone = contactDetails["Phone"];
            var Message = contactDetails["Message"];

            try
            {
                ContactUsData ContactUsObject = new ContactUsData();

                ContactUsObject.UserName = Name;
                ContactUsObject.Email = Email;
                ContactUsObject.PhoneNumber = Phone;
                ContactUsObject.Message = Message;
                ContactUsObject.IsDeleted = false;
                ContactUsObject.Seen = false;
                ContactUsObject.SeenTime = null;
                ContactUsObject.SubmittedTime = DateTime.Now;


                db.ContactUsDatas.Add(ContactUsObject);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                infoMessage(ex.Message);
                writeErrorLog(ex);
            }



            var fromAddress = new MailAddress(SenderEmailId, "Contact Message By: " + Name);
            var toAddress = new MailAddress(SenderEmailId, "Print My Box");
            string fromPassword = SenderEmailPassword;
            string subject = "PrintMyBox Contact Us form Submission by: " + Name;
            string body = "Name: " + Name + "<br>Phone: " + Phone + "<br>" + "Email: " + Email + "<br>" + "Message: " + Message + "<br>Time: " + DateTime.Now;

            var smtp = new SmtpClient
            {
                Host = SenderEmailHost,
                Port = SenderEmailPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body,

            })
            {
                //message.Bcc.Add("support@printmybox.com");
                smtp.Send(message);
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            var fromSiteAddress = new MailAddress(SenderEmailId, "Print My Box");
            var toCustomerAddress = new MailAddress(Email, Name);
            string fromSitePassword = SenderEmailPassword;
            subject = "PrintMyBox: Your Contact Form has submitted, Thankyou for your precious time ";
            body = " <b>Form</b><br>Name: " + Name + "<br>Phone: " + Phone + "<br>" + "Email: " + Email + "<br>" + "Message: " + Message + "<br>Time: " + DateTime.Now;

            var smtp1 = new SmtpClient
            {
                Host = SenderEmailHost,
                Port = SenderEmailPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromSiteAddress.Address, fromSitePassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromSiteAddress, toCustomerAddress)
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            })
            {
                smtp1.Send(message);
            }


            return RedirectToAction("Contact");
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult QuoteRequest(FormCollection fc, HttpPostedFileBase File_input)/*string ProductName, string Name, string Email, string Phone, string Stock, string Color, string Quantity, string Height, string Width, string Depth, string File, string Comments*/
        {
            try
            {
                string ProductName = fc["ProductName-input"];
                string Name = fc["Name-input"];
                string Email = fc["Email-input"];
                string Phone = fc["Phone-input"];
                string Stock = fc["Stock-input"];
                string Color = fc["Color-input"];
                string Quantity = fc["Quantity-input"];
                string Height = fc["Height-input"];
                string Width = fc["Width-input"];
                string Depth = fc["Depth-input"];
                string Comments = fc["Comments-input"];
                string ViewPath = fc["ViewPath"];
                HttpPostedFileBase AttachmentUserCopy = File_input;

                try
                {
                    PriceQuote QuoteObject = new PriceQuote();
                    if (AttachmentUserCopy != null)
                    {
                        string pic = System.IO.Path.GetFileName(AttachmentUserCopy.FileName);
                        string path = System.IO.Path.Combine(
                                               Server.MapPath("~/UploadedProductImages"), pic);
                        // file is uploaded
                        AttachmentUserCopy.SaveAs(path);

                        // save the image path path to the database or you can send image 
                        // directly to database
                        // in-case if you want to store byte[] ie. for DB
                        using (MemoryStream ms = new MemoryStream())
                        {
                            AttachmentUserCopy.InputStream.CopyTo(ms);
                            byte[] array = ms.GetBuffer();
                        }
                        QuoteObject.File = AttachmentUserCopy.FileName;

                    }
                    QuoteObject.UserName = Name;
                    QuoteObject.Email = Email;
                    QuoteObject.PhoneNumber = Phone;
                    QuoteObject.Quantity = Convert.ToInt32(Quantity);
                    QuoteObject.Stock = Stock;
                    QuoteObject.Color = Color;
                    QuoteObject.Height = Convert.ToInt32(Height);
                    QuoteObject.Width = Convert.ToInt32(Width);
                    QuoteObject.Depth = Convert.ToInt32(Depth);
                    QuoteObject.Comments = Comments;
                    QuoteObject.SubmittedTime = DateTime.Now;
                    QuoteObject.IsDeleted = false;
                    QuoteObject.SeenTime = null;
                    QuoteObject.Seen = false;

                    db.PriceQuotes.Add(QuoteObject);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    infoMessage(ex.Message);
                    writeErrorLog(ex);
                }



                var fromAddress = new MailAddress(SenderEmailId, "Quote Request: " + Name);
                var toAddress = new MailAddress(Email, "Quote Request" + Name);
                string fromPassword = SenderEmailPassword;
                string subject = "Quote Request for Product " + ProductName;
                string body = "Name: " + Name + "<br>Phone: " + Phone + "<br>" + "Email: " + Email + "<br>" + "Product Details: " + ProductName + "<br>Stock: " + Stock + "<br>Color: " + Color + "<br>Quantity: " + Quantity + "<br>Dimensions: " + Height + "x" + Width + "x" + Depth + "<br>Comments: " + Comments + "<br>Time: " + DateTime.Now;

                var smtp = new SmtpClient
                {
                    Host = SenderEmailHost,
                    Port = SenderEmailPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                })
                {

                    if (File_input != null)

                    {

                        string fileName = Path.GetFileName(File_input.FileName);
                        File_input.InputStream.Seek(0, SeekOrigin.Begin);
                        message.Attachments.Add(new Attachment(File_input.InputStream, fileName, MediaTypeNames.Application.Octet));
                        message.Bcc.Add(SenderEmailId);
                    }

                    smtp.Send(message);
                }


                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                // var fromSiteAddress = new MailAddress(SenderEmailId, "Print My Box");
                //var toCustomerAddress = new MailAddress(Email, Name);
                //string fromSitePassword = SenderEmailPassword;
                //subject = "Quote Request Confirmation";
                //body = "Your Following Quote Request Submitted, Thankyou for your precious time <br><br><b>Quote Request</b><br>Name: " + Name + "<br>Phone: " + Phone + " <br> " + "Email: " + Email + " <br> " + "Product Details: " + ProductName + " <br> Stock: " + Stock + " <br> Color: " + Color + " <br> Quantity: " + Quantity + " <br> Dimensions: " + Height +"x"+ Width +"x" + Depth + " <br> Time: " + DateTime.Now;
                //HttpPostedFileBase File = null;
                //if (Request.Files.Count > 0)
                //{
                //    File = Request.Files[0];
                //}

                //var smtp1 = new SmtpClient
                //{
                //    Host = SenderEmailHost,
                //    Port = SenderEmailPort,
                //    EnableSsl = true,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    Credentials = new NetworkCredential(fromSiteAddress.Address, fromSitePassword),
                //    Timeout = 20000
                //};
                //using (var message1 = new MailMessage(fromSiteAddress, toCustomerAddress)
                //{
                //    IsBodyHtml = true,
                //    Subject = subject,
                //    Body = body
                //})
                //{

                //    if (AttachmentUserCopy != null)

                //    {

                //        string fileName = Path.GetFileName(AttachmentUserCopy.FileName);
                //        AttachmentUserCopy.InputStream.Seek(0, SeekOrigin.Begin);
                //        message1.Attachments.Add(new Attachment(AttachmentUserCopy.InputStream, fileName, MediaTypeNames.Application.Octet));

                //    }
                //    smtp1.Send(message1);
                //}


                //return new FilePathResult(ViewPath, "text/html");
                var smtp1 = new SmtpClient
                {
                    Host = SenderEmailHost,
                    Port = SenderEmailPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };

                using (var message1 = new MailMessage(SenderEmailId, SenderEmailId)
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body
                })
                {

                    if (File_input != null)

                    {

                        string fileName = Path.GetFileName(File_input.FileName);
                        File_input.InputStream.Seek(0, SeekOrigin.Begin);
                        message1.Attachments.Add(new Attachment(File_input.InputStream, fileName, MediaTypeNames.Application.Octet));

                    }

                    smtp1.Send(message1);
                }
                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                infoMessage(ex.Message);
                writeErrorLog(ex);
                return RedirectToAction("Error");
                //return Json("Something Went Wrong!", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Error()
        {
            return View();
        }

        [Authorize]
        public List<Models.Product> updateSlugs()
        {
            try
            {
                var productsList = db.Products.ToList();
                foreach (var item in productsList)
                {
                    var productItem = db.Products.Find(item.Id);
                    productItem.SlugURL = item.Name.Replace(" ", "_");
                    productItem.SlugURL = productItem.SlugURL.Replace("-", "_");
                    productItem.SlugURL = productItem.SlugURL.Replace("/", "_");
                    productItem.SlugURL = productItem.SlugURL.Replace("\\", "_");
                    productItem.SlugURL = productItem.SlugURL.ToLower();
                    productItem.Specifications = productItem.Description;
                    db.SaveChanges();
                }
                return db.Products.ToList();
            }
            catch
            {
                return db.Products.ToList();
            }
        }




        [AllowAnonymous]
        public ActionResult PaperWorkPlan(FormCollection fc, HttpPostedFileBase FormFile)
        {
            var Url = fc["urlField"];
            try
            {
                var Carrier = fc["Carrier"];
                var Trailer = fc["Trailer"];
                var AreasOfOperation = fc["AreasOfOperation"];
                var LiabilityInCargoInsurance = fc["LiabilityInCargoInsurance"];
                var FreightInvoices = fc["FreightInvoices"];
                var NoOfTrucksInFleet = fc["NoOfTrucksInFleet"];
                var SafetyRating = fc["SafetyRating"];
                var NoOfCompanyTrucks = fc["NoOfCompanyTrucks"];
                var MinRatePerMile = fc["MinRatePerMile"];
                var MaxPickAmount = fc["MaxPickAmount"];
                var MaxDropsAmount = fc["MaxDropsAmount"];
                var HazmatCertified = fc["HazmatCertified"];
                var ScacCode = fc["ScacCode"];
                var TwicCard = fc["TwicCard"];
                var TruckNo = fc["TruckNo"];
                var TrailerNo = fc["TrailerNo"];
                var TrailerType = fc["TrailerType"];
                var MaxWeight = fc["MaxWeight"];
                var FirstName = fc["FirstName"];
                var LastName = fc["LastName"];
                var Company = fc["Company"];
                var MCNo = fc["MCNo"];
                var FAX = fc["FAX"];
                var YearsInBusiness = fc["YearsInBusiness"];
                var EMAIL = fc["EMAIL"];
                var PHONE = fc["PHONE"];



                var fromAddress = new MailAddress(SenderEmailId, "Contact Message By: " + FirstName + " " + LastName);
                var toAddress = new MailAddress("info@premier-dispatch.com", "Premium Dispatch Services LLC");
                string fromPassword = SenderEmailPassword;
                string subject = "Premium Dispatch Services LLC Form Submission";
                string body = "Carrier: " + Carrier + "<br>Trailer: " + Trailer + "<br>" + "Areas Of Operation: "
                    + AreasOfOperation + "<br>" + "Liability In Cargo Insurance: " + LiabilityInCargoInsurance + "<br>Safety Rating: " + SafetyRating
                    + "<br>No Of Company Trucks: " + NoOfCompanyTrucks
                    + "<br>Min Rate Per Mile: " + MinRatePerMile
                    + "<br>Max Pick Amount: " + MaxPickAmount
                    + "<br>Hazmat Certified: " + HazmatCertified
                    + "<br>Scac Code: " + ScacCode
                    + "<br>Twic Card: " + TwicCard
                    + "<br>Truck No: " + TruckNo
                    + "<br>Trailer Type: " + TrailerType
                    + "<br>Max Weight: " + MaxWeight
                    + "<br>First Name: " + FirstName
                    + "<br>Last Name: " + LastName
                    + "<br>MCNo: " + MCNo
                    + "<br>FAX: " + FAX
                    + "<br>YearsInBusiness: " + YearsInBusiness
                    + "<br>EMAIL: " + EMAIL
                    + "<br>PHONE: " + PHONE
                    + "<br>Company: " + Company;

                var smtp = new SmtpClient
                {
                    Host = SenderEmailHost,
                    Port = SenderEmailPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body,

                })
                {
                    //message.Bcc.Add("support@printmybox.com");
                    smtp.Send(message);
                }

            }
            catch (Exception ex)
            {
                infoMessage(ex.Message);
                writeErrorLog(ex);

            }
            return new RedirectResult(Url);
        }

        public static void infoMessage(string _message)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + " " + _message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static void writeErrorLog(Exception ex)
        {
            StreamWriter sw = null;
            try
            {
                sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(DateTime.Now.ToString() + " " + ex.Source.ToString().Trim() + " " + ex.Message.ToString().Trim());
                sw.Flush();
                sw.Close();
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

        public ActionResult CustomPlan(FormCollection fc)
        {
            var Url = fc["urlField"];
            try
            {
                var Name = fc["Name"];
                var Email = fc["Email"];
                var Message = fc["Message"];
                var fromAddress = new MailAddress(SenderEmailId, "Contact Message By: " + Name);
                var toAddress = new MailAddress("info@premier-dispatch.com", "Premium Dispatch Services LLC");
                string fromPassword = SenderEmailPassword;
                string subject = "Premium Dispatch Services LLC Form Submission";
                string body = "Name: " + Name + "<br>Email: " + Email + "<br>" + "Message: " + Message;
                    

                var smtp = new SmtpClient
                {
                    Host = SenderEmailHost,
                    Port = SenderEmailPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body,

                })
                {
                    //message.Bcc.Add("support@printmybox.com");
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                infoMessage(ex.Message);
                writeErrorLog(ex);

            }
            return new RedirectResult(Url);
        }

        public ActionResult SubmitPremiumDispatchAgreement(FormCollection fc)
        {
                var Url = fc["urlField"];
            try
            {
                var fromAddress = new MailAddress(SenderEmailId, "Agreement");
                var toAddress = new MailAddress("info@premier-dispatch.com", "Premium Dispatch Services LLC");
                string fromPassword = SenderEmailPassword;
                string subject = "Premium Dispatch Services LLC Form Submission";


                var html = "";
                foreach (var key in fc.Keys)
                {
                    html = html + key.ToString() + ": " + fc[key.ToString()];
                }

                string body = html;
                var smtp = new SmtpClient
                {
                    Host = SenderEmailHost,
                    Port = SenderEmailPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = body,

                })
                {
                    //message.Bcc.Add("support@printmybox.com");
                    smtp.Send(message);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return new RedirectResult(Url);
        }
        public PartialViewResult _JsonLdPartial()
        {
            Person author = new Person()
            {
                Name="Stive"
            };
            var localBusiness = new LocalBusiness()
            {
                PriceRange = "0.2$",
                Name = "Print My Box",
                Image = new Uri("https://www.printmybox.com/assets/base/img/layout/logos/logo-3.png"),
                Address = new PostalAddress()
                {
                    StreetAddress = "2252 Valencia Dr, Lexington, KY 40513",
                    AddressLocality = "Lexington",
                    AddressRegion = "Kentucky",
                    PostalCode = "40513",
                    AddressCountry = "United States",
                    Email = "support@printmybox.com",
                    Telephone = "646-633-4664",
                },
                AggregateRating = new AggregateRating()
                {
                    RatingCount = 5,
                    RatingValue = 5
                },
                Review = new Review()
                {
                    Author = author,
                    DatePublished = new DateTimeOffset(2018, 6, 28, 10, 58, 56, TimeSpan.Zero),
                    ReviewBody = "We needed a large number of boxes for our line of foundations in weeks’ time and the people at www.printmbyox.com really delivered after our usual vendor failed to come through. The quality of the boxes was also superior and we have since switched to them for our regular order of boxes. Great job guys!",
                    ReviewRating = new Rating
                    {
                        BestRating = 5,
                        RatingValue = 5,
                        WorstRating = 1
                    }

                }
            };
            




            var JasonLd = localBusiness.ToString();
            ViewBag.JsonLDBag = JasonLd;

            return PartialView();
        }
    }
}