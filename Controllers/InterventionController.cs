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
    public class InterventionController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;  

        public async Task<ActionResult> InterventionFormAsync(int? building_id, int? battery_id, int? column_id, int? elevator_id, string? report) {

            //variables have the same name so here's a quick fix
            var fbuilding_id = building_id;
            var fbattery_id = battery_id;
            var fcolumn_id = column_id;
            var felevator_id = elevator_id;
            var freport = report;
            try 
            {
                var query = @"mutation 
                    ( $building_id: Int, $battery_id: Int, $column_id: Int, $elevator_id: Int, $report: String) {
                        intervention(report: $report column_id: $column_id battery_id: $battery_id building_id: $building_id elevator_id: $elevator_id) {
                            id
                        }
                    }
                  ";
                  var queryObject = new
                  {
                      query,
                      variables = new {
                        building_id = fbuilding_id, 
                        battery_id = fbattery_id,
                        column_id = fcolumn_id,
                        elevator_id = felevator_id,
                        report = freport
                        }
                  };
                  //Console.WriteLine(query);
                  var request = new HttpRequestMessage
                  {
                      Method = HttpMethod.Post,
                      Content = new StringContent(JsonConvert.SerializeObject(queryObject), Encoding.UTF8, "application/json")

                  };

                  dynamic responseObj;

                using (var response = await Program.client.SendAsync(request))
                {
                  
                    response.EnsureSuccessStatusCode();
                    var responseString = await response.Content.ReadAsStringAsync();
                    responseObj = JsonConvert.DeserializeObject<dynamic>(responseString);
                    //if (responseObj["data"]["intervention"]["id"] != null && responseObj["data"]["intervention"]["id"] is Int16 || responseObj["data"]["intervention"]["id"] is Int32 )
                    //{
                    //    ViewBag.Message = "Intervention successfully sent!"; 
                    //}
                    //else
                    //{
                    //    ViewBag.Message = "There was an error with your form, please try again later or contact us for support.";      //
                    //}
                    Console.WriteLine(responseObj);
                    return RedirectToAction("Intervention");
                }
                  
                  
            }
            catch (System.Exception)
            {
                  
                throw;
            }
      }

        public async Task<IActionResult> InterventionAsync()
        {
            var currentUser = User.Identity.Name;
            if (currentUser != null){


            

                ViewBag.UserBag = $"<input type='hidden' value='{currentUser}' id='currentUser'>";
                var resBuilding = await getBuidings();
                ViewBag.BuildingQuery = resBuilding;

                for (var i = 0; i < resBuilding["data"]["buildingOfCustomer"].Count; i++)
                {

                    ViewBag.BuildingBag += $"<option value='{resBuilding["data"]["buildingOfCustomer"][i]["id"]}'>Building # {resBuilding["data"]["buildingOfCustomer"][i]["id"]} </option>";

                }

                var resBattery = await getBattery();

                for (var i = 0; i < resBattery["data"]["batteries"].Count; i++)
                {
                    ViewBag.BatteryBag += $"<option value='{resBattery["data"]["batteries"][i]["id"]}' name='{resBattery["data"]["batteries"][i]["building_id"]}' >Battery # {resBattery["data"]["batteries"][i]["id"]} </option>";
                }

                var resColumn = await getColumns();

                for (var i = 0; i < resColumn["data"]["columns"].Count; i++)
                {

                    ViewBag.ColumnBag += $"<option value='{resColumn["data"]["columns"][i]["id"]}'name='{resColumn["data"]["columns"][i]["battery_id"]}'>Column # {resColumn["data"]["columns"][i]["id"]}</option>";

                }

                var resElevator = await getElevators();
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

        public async Task<dynamic> ApiCall(String query)
        {

            var queryObject = new
            {
                query,
                variables = new { email = User.Identity.Name }

            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(queryObject), Encoding.UTF8, "application/json")

            };

            dynamic responseObj;

            using (var response = await Program.client.SendAsync(request))
            {

                //response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                responseObj = JsonConvert.DeserializeObject<dynamic>(responseString);

            }
            return responseObj;
        }

        public async Task<dynamic> ApiCallId(String query, Int32 queryId)
        {

            var queryObject = new
            {
                query,
                variables = new { queryId }

            };

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(queryObject), Encoding.UTF8, "application/json")

            };

            dynamic responseObj;

            using (var response = await Program.client.SendAsync(request))
            {

                //response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                responseObj = JsonConvert.DeserializeObject<dynamic>(responseString);

            }
            return responseObj;
        }
        public async Task<dynamic> getBuidings()
        {
            var query = @"
                  query ($email: String!) {
                        buildingOfCustomer(email: $email) {
                              id
                              
                        }
                  }";

            var response = await ApiCall(query);

            return response;
        }

        public async Task<dynamic> getBattery()
        {
              var query = @"
                  query ($email: String!) {
                        batteries(email: $email) {
                              building_id
                              id
                        }
                  }";

            var response = await ApiCall(query);

            return response;


        }

        public async Task<dynamic> getColumns()
        {
            var query = @"
                  query ($email: String!) {
                        columns (email: $email) {
                              battery_id
                              id
                        }
                  }";

            var response = await ApiCall(query);
            return response;
        }
        public async Task<dynamic> getElevators()
        {
            var query = @"
                  query ($email: String!) {
                        elevators(email: $email) {
                              column_id
                              id
                        }
                  }";

            var response = await ApiCall(query);

            return response;



        }

        public async Task<ActionResult> ProductsAsync()
        {
            var currentUser = User.Identity.Name;
            if (currentUser != null)
            {

                ViewBag.UserBag = $"<input type='hidden' value='{currentUser}' id='currentUser'>";
                var resBuilding = await getBuidings();
                ViewBag.BuildingQuery = resBuilding;

                for (var i = 0; i < resBuilding["data"]["buildingOfCustomer"].Count; i++)
                {

                    ViewBag.BuildingBag += $"<option value='{resBuilding["data"]["buildingOfCustomer"][i]["id"]}'>Building # {resBuilding["data"]["buildingOfCustomer"][i]["id"]} </option>";

                }

                var resBattery = await getBattery();

                for (var i = 0; i < resBattery["data"]["batteries"].Count; i++)
                {
                    ViewBag.BatteryBag += $"<option value='{resBattery["data"]["batteries"][i]["id"]}' name='{resBattery["data"]["batteries"][i]["building_id"]}' >Battery # {resBattery["data"]["batteries"][i]["id"]} </option>";
                }

                var resColumn = await getColumns();

                for (var i = 0; i < resColumn["data"]["columns"].Count; i++)
                {

                    ViewBag.ColumnBag += $"<option value='{resColumn["data"]["columns"][i]["id"]}'name='{resColumn["data"]["columns"][i]["battery_id"]}'>Column # {resColumn["data"]["columns"][i]["id"]}</option>";

                }

                var resElevator = await getElevators();
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