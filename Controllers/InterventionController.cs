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

        public async Task<ActionResult> InterventionFormAsync(int? building_id, int? battery_id, int? column_id, int? elevator_id, string? report, int customer_id) 
        {

            //variables have the same name so here's a quick fix
            var fcustomer_id = customer_id;
            var fbuilding_id = building_id;
            var fbattery_id = battery_id;
            var fcolumn_id = column_id;
            var felevator_id = elevator_id;
            var freport = report;
            try 
            {
                var query = @"mutation 
                    ( $building_id: Int, $battery_id: Int, $column_id: Int, $elevator_id: Int, $report: String, $customer_id: Int) {
                        intervention(report: $report column_id: $column_id battery_id: $battery_id building_id: $building_id elevator_id: $elevator_id, customer_id: $customer_id) {
                            id
                        }
                    }
                  ";
                  var queryObject = new
                  {
                      query,
                      variables = new {
                        customer_id = fcustomer_id,
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
                    var success = responseObj["data"]["intervention"]["id"];
                    if (success != null)
                    {
                        TempData["InterventionCreated"] = "Intervention successfully sent!"; 
                    }
                    else
                    {
                        TempData["InterventionError"] = "There was an error with your form, please try again later or contact us for support.";      //
                    }
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

                

                var resCustomer = await getCustomer();

                ViewBag.CustomerID = $"<input type='hidden' name='customer_id' value='{resCustomer["data"]["customers"]["id"]}' id='current_customerID'>";

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
        
        public async Task<dynamic> getCustomer()
        {
           var query = @" query ($email: String!) {
                customers(email: $email) {
                    email
                    id
                }
            }";

            var response = await ApiCall(query);

            return response;
        }
            
        
        public async Task<dynamic> getBuidings()
        {
            var query = @"
                  query ($email: String!) {
                        buildingOfCustomer(email: $email) {
                            id
                            customer_id
                            admin_full_name
                            admin_email
                            admin_phone
                            tech_contact_full_name
                            tech_contact_email
                            tech_contact_phone
                             
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
                            id, 
                            building_id, 
                            type_of_battery, 
                            status, 
                            date_of_comissioning, 
                            date_of_last_inspection, 
                            certificate_of_operation, 
                            information, 
                            notes 
                              
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
                            id,
                            battery_id,
                            type_of_column,
                            number_of_floors_served,
                            status,
                            information,
                            notes
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
                            id,
                            column_id,
                            type_of_building,
                            serial_number,
                            model,
                            status,
                            date_of_commissioning,
                            date_of_last_inspection,
                            certificate_of_operations,
                            information,
                            notes
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
                Console.WriteLine(resBuilding);

                for (var i = 0; i < resBuilding["data"]["buildingOfCustomer"].Count; i++)
                {
                    ViewBag.BuildingBag += $"<div>Building # {resBuilding["data"]["buildingOfCustomer"][i]["id"]} </div>";
                    ViewBag.BuildingBag += $"<div>Customer ID: {resBuilding["data"]["buildingOfCustomer"][i]["customer_id"]} </div>";
                    ViewBag.BuildingBag += $"<div>Admin Name: {resBuilding["data"]["buildingOfCustomer"][i]["admin_full_name"]} </div>";
                    ViewBag.BuildingBag += $"<div>Admin Email: {resBuilding["data"]["buildingOfCustomer"][i]["admin_email"]} </div>";
                    ViewBag.BuildingBag += $"<div>Admin Phone: {resBuilding["data"]["buildingOfCustomer"][i]["admin_phone"]} </div>";
                    ViewBag.BuildingBag += $"<div>Tech Contact: {resBuilding["data"]["buildingOfCustomer"][i]["tech_contact_full_name"]} </div>";
                    ViewBag.BuildingBag += $"<div>Tech Contact Email: {resBuilding["data"]["buildingOfCustomer"][i]["tech_contact_email"]} </div>";
                    ViewBag.BuildingBag += $"<div>Tech Contact Phone: {resBuilding["data"]["buildingOfCustomer"][i]["tech_contact_phone"]} </div>";
                    ViewBag.BuildingBag += $"<div>Address: {resBuilding["data"]["buildingOfCustomer"][i]["address"]} </div>";

                }

                var resBattery = await getBattery();

                for (var i = 0; i < resBattery["data"]["batteries"].Count; i++)
                {
                    ViewBag.BatteryBag += $"<div>Battery # {resBattery["data"]["batteries"][i]["id"]} </div>";
                    ViewBag.BatteryBag += $"<div>Battery # {resBattery["data"]["batteries"][i]["id"]} </div>";
                    ViewBag.BatteryBag += $"<div>Battery # {resBattery["data"]["batteries"][i]["id"]} </div>";
                    ViewBag.BatteryBag += $"<div>Battery # {resBattery["data"]["batteries"][i]["id"]} </div>";
                    ViewBag.BatteryBag += $"<div>Battery # {resBattery["data"]["batteries"][i]["id"]} </div>";
                    ViewBag.BatteryBag += $"<div>Battery # {resBattery["data"]["batteries"][i]["id"]} </div>";
                    ViewBag.BatteryBag += $"<div>Battery # {resBattery["data"]["batteries"][i]["id"]} </div>";
                    ViewBag.BatteryBag += $"<div>Battery # {resBattery["data"]["batteries"][i]["id"]} </div>";

                }

                var resColumn = await getColumns();

                for (var i = 0; i < resColumn["data"]["columns"].Count; i++)
                {

                    ViewBag.ColumnBag += $"<div>Column # {resColumn["data"]["columns"][i]["id"]}</div>";

                }

                var resElevator = await getElevators();
                //ViewBag.ElevatorBag = "";

                for (var i = 0; i < resElevator["data"]["elevators"].Count; i++)
                {

                    ViewBag.ElevatorBag += $"<div>Elevator # {resElevator["data"]["elevators"][i]["id"]} </div>";

                }
                Console.WriteLine(ViewBag.BuildingBag);
                Console.WriteLine(ViewBag.BatteryBag);
                Console.WriteLine(ViewBag.ColumnBag);
                Console.WriteLine(ViewBag.ElevatorBag);

                return View();

            }
            else 
            {
                return RedirectToAction("index", "Home");
            }
        }


    }
}