using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Collections;
using Rocket_Elevators_Customer_Portal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Rocket_Elevators_Customer_Portal
{
    public class ProductController : Controller
    {
        public async Task<IActionResult> Index()
        {
           await ProductAsync();
            return View();
        }


        public async Task<IActionResult> ProductAsync()
        {
           

            var currentUser = User.Identity.Name;
            var InterCtrl = new InterventionController();
            if (currentUser != null)
            {


                ViewBag.UserBag = $"<input type='hidden' value='{currentUser}' id='currentUser'>";
                var resBuilding = await InterCtrl.getBuidings();
                ViewBag.BuildingQuery = resBuilding;
                Console.WriteLine(resBuilding);

                for (var i = 0; i < resBuilding["data"]["buildingOfCustomer"].Count; i++)
                {

                    ViewBag.BuildingBag += $"<option value='{resBuilding["data"]["buildingOfCustomer"][i]["id"]}'>Building # {resBuilding["data"]["buildingOfCustomer"][i]["id"]} </option>";

                }

                var resBattery = await InterCtrl.getBattery();

                for (var i = 0; i < resBattery["data"]["batteries"].Count; i++)
                {
                    ViewBag.BatteryBag += $"<option value='{resBattery["data"]["batteries"][i]["id"]}' name='{resBattery["data"]["batteries"][i]["building_id"]}' >Battery # {resBattery["data"]["batteries"][i]["id"]} </option>";
                }

                var resColumn = await InterCtrl.getColumns();

                for (var i = 0; i < resColumn["data"]["columns"].Count; i++)
                {

                    ViewBag.ColumnBag += $"<option value='{resColumn["data"]["columns"][i]["id"]}'name='{resColumn["data"]["columns"][i]["battery_id"]}'>Column # {resColumn["data"]["columns"][i]["id"]}</option>";

                }

                var resElevator = await InterCtrl.getElevators();
                //ViewBag.ElevatorBag = "";

                for (var i = 0; i < resElevator["data"]["elevators"].Count; i++)
                {

                    ViewBag.ElevatorBag += $"<option value='{resElevator["data"]["elevators"][i]["id"]}'name='{resElevator["data"]["elevators"][i]["column_id"]}'>Elevator # {resElevator["data"]["elevators"][i]["id"]} </option>";

                }


                return View();

            }
            else 
            {
                return RedirectToAction("index", "Home");
            }
        

        }
    }


    

}