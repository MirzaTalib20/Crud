using CrudOneMore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using OfficeOpenXml;
using System.IO;
using System;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text.pdf;
using System.Web.Helpers;
using iTextSharp;
using Rotativa.Options;
using Rotativa;

namespace CrudOneMore.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public string connectionString = @"Data Source=DESKTOP-4QTM0CJ;Initial Catalog=DapperDB;User ID=sa;Password=123";
        public ActionResult Index(string SortOrder, string SortBy,int Page = 1)
        {
            DynamicParameters param = new DynamicParameters();
            ViewBag.SortOrder = SortOrder;
            ViewBag.SortBy = SortBy;
            var a = DapperORM.ReturnList<EmpModel>("Employees_View", param);
            EmpModel emp = new EmpModel();
            switch (SortBy)
            {
                case "Name":
                    {
                        switch (SortOrder)
                        {
                            case "Asc":
                                {
                                    a = a.OrderBy(x => x.Name).ToList();
                                    break;
                                }
                            case "Desc":
                                {
                                    a = a.OrderByDescending(x => x.Name).ToList();
                                    break;
                                }
                            default:
                                {
                                    a = a.OrderBy(x => x.Name).ToList();
                                    break;
                                }
                        }

                        break;
                    }
                case "Email":
                    {
                        switch (SortOrder)
                        {
                            case "Asc":
                                {
                                    a = a.OrderBy(x => x.Email).ToList();
                                    break;
                                }
                            case "Desc":
                                {
                                    a = a.OrderByDescending(x => x.Email).ToList();
                                    break;
                                }
                            default:
                                {
                                    a = a.OrderBy(x => x.Email).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "Password":
                    {
                        switch (SortOrder)
                        {
                            case "Asc":
                                {
                                    a = a.OrderBy(x => x.Password).ToList();
                                    break;
                                }
                            case "Desc":
                                {
                                    a = a.OrderByDescending(x => x.Password).ToList();
                                    break;
                                }
                            default:
                                {
                                    a = a.OrderBy(x => x.Password).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                case "Gender":
                    {
                        switch (SortOrder)
                        {
                            case "Asc":
                                {
                                    a = a.OrderBy(x => x.Gender).ToList();
                                    break;
                                }
                            case "Desc":
                                {
                                    a = a.OrderByDescending(x => x.Gender).ToList();
                                    break;
                                }
                            default:
                                {
                                    a = a.OrderBy(x => x.Gender).ToList();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        a = a.OrderBy(x => x.Name).ToList();
                        break;
                    }
            }

            //List<EmpModel> studet = new List<EmpModel>();

            //using (SqlConnection con = new SqlConnection(connectionString))
            //{
            //    a = DapperORM.ReturnList<EmpModel>("Employees_View", param).ToList();
            //}
            ViewBag.TotalPages = Math.Ceiling(a.Count() / 10.0);
            a = a.Skip((Page - 1) * 10).Take(10).ToList();
            ViewBag.Page = Page;
            return View(a);
        }
        [HttpPost]
        public ActionResult Index(string searchText)
        {
            EmpModel emp = new EmpModel();
            DynamicParameters param = new DynamicParameters();


            var a = DapperORM.ReturnList<EmpModel>("Employees_View", param).ToList();

            if (searchText != null)
            {
               
                a = DapperORM.ReturnList<EmpModel>("Employees_View", param).Where(x => x.Name.Contains(searchText)).ToList();

            }
         
            return View(a);
        }







        [HttpGet]
        public ActionResult Add(int id = 0)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(EmpModel emp, string CSharp, string Java, string Python)
        {
            if (CSharp == "true")
            {
                emp.IsInterestedCSharp = true;
            }
            else
            {
                emp.IsInterestedCSharp = false;
            }
            if (Java == "true")
            {
                emp.IsInterestedJava = true;
            }
            else
            {
                emp.IsInterestedJava = false;
            }
            if (Python == "true")
            {
                emp.IsInterestedPython = true;

            }
            else
            {
                emp.IsInterestedPython = false;
            }

            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", emp.Id);
            param.Add("@Name", emp.Name);
            param.Add("@Email", emp.Email);
            param.Add("@Password", emp.Password);
            param.Add("@Gender", emp.Gender);
            param.Add("@Status", 1);
            param.Add("@IsInterestedCSharp", emp.IsInterestedCSharp);
            param.Add("@IsInterestedJava", emp.IsInterestedJava);
            param.Add("@IsInterestedPython", emp.IsInterestedPython);
            DapperORM.ExecuteWithoutReturn("Employees_AddNew", param);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@Id", id);
                EmpModel model = DapperORM.ReturnList<EmpModel>("sp_getUserDetail", param).FirstOrDefault();

                //return View(DapperORM.ReturnList<EmpModel>("EmployeeViewByID", param).FirstOrDefault<EmpModel>());
                return View(model);
            }
        }


        [HttpPost]
        public ActionResult Edit(EmpModel emp)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", emp.Id);
            param.Add("@Name", emp.Name);
            param.Add("@Email", emp.Email);
            param.Add("@Password", emp.Password);
            param.Add("@Gender", emp.Gender);
            param.Add("@Status", 1);
            DapperORM.ExecuteWithoutReturn("Employee_Edit", param);
            return RedirectToAction("Index");
        }


        public ActionResult Delete(int id)
        {
            DynamicParameters param = new DynamicParameters();

            param.Add("@Id", id);

            DapperORM.ExecuteWithoutReturn("Employees_Delete", param);
            return RedirectToAction("Index");
        }




        public ActionResult Restore(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", id);
            DapperORM.ExecuteWithoutReturn("soft_delete", param);
            return RedirectToAction("Index");
        }


        public ActionResult ViewDelete()
        {
            EmpModel emp = new EmpModel();
            return View(DapperORM.ReturnList<EmpModel>("restore_data"));
        }



        public void ExportToExcel()
        {
            DynamicParameters param = new DynamicParameters();
            var a = DapperORM.ReturnList<EmpModel>("Employees_View", param).ToList();
            List<EmpModel> data = new List<EmpModel>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                data = a;
            }
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(data, true);
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //here i have set filname as Students.xlsx
                Response.AddHeader("content-disposition", "attachment;  filename=Employees.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }


          














            //public ActionResult ConfirmDelete(int? id)
            //{
            //    if (id == 0)
            //    {
            //        return View(DapperORM.ReturnList<EmpModel>("EmployeeViewByID", null).FirstOrDefault<EmpModel>());
            //    }
            //    else
            //    {
            //        var a = DapperORM.ReturnList<EmpModel>("EmployeeViewAll",null);
            //        return View(a.Where(x => x.Empid == id).FirstOrDefault());
            //    }
            //}

            //public ActionResult Delete(int? id)
            //{
            //    DynamicParameters param = new DynamicParameters();
            //    param.Add("@Empid", id);
            //    DapperORM.ExecuteWithoutReturn("soft_delete", param);
            //    return RedirectToAction("Index");
            //}
        }
        public ActionResult PrintPDF()
        {
            EmpModel emp = new EmpModel();
            DynamicParameters param = new DynamicParameters();

            var Data = DapperORM.ReturnList<EmpModel>("Employees_View", param).ToList();

            return new PartialViewAsPdf("PrintPDF", Data)
            {
                PageOrientation = Orientation.Landscape,
                PageSize = Size.A3,
                CustomSwitches = "--footer-center \" [page] Page of [toPage] Pages\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"",
                FileName = "TestPartialViewAsPdf.pdf"
            };
        }


    }
}




   
