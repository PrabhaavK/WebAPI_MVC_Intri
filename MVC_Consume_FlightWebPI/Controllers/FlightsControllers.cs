using System.Net;
using MVC_Consume_FlightWebPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MVC_Consume_FlightWebPI.Controllers
{
    public class FlightsController : Controller
    {
        private readonly HttpClient _client;

        public FlightsController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:5043/api/");
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("Flights");
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                List<Flight> flightsList = JsonConvert.DeserializeObject<List<Flight>>(data);
                return View(flightsList);
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Flight model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return the view with the model to show validation errors
            }

            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync("Flights", content); // Await the PostAsync call
                if (response.IsSuccessStatusCode)
                {
                    TempData["successmsg"] = "New Flight Added";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errormsg"] = ex.Message;
                return View(model); // Return the view with the model to show the error
            }

            return View(model); // Return the view in case of failure
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            try 
            {
                Flight flight = new Flight();
                HttpResponseMessage response = await _client.GetAsync($"Flights/{id}"); // Await the GetAsync call
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync(); // Await the ReadAsStringAsync call
                    flight = JsonConvert.DeserializeObject<Flight>(data);
                }
                return View(flight);
            }
            catch (Exception ex)
            {
                TempData["errormsg"] = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                Flight flight = new Flight(); // Corrected: "new Flight()"
                HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "Flights/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    flight = JsonConvert.DeserializeObject<Flight>(data);
                }
                return View(flight);

            }
            catch (Exception ex)
            {
                TempData["errormsg"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, Flight model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PutAsync($"Flights/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Flight updated successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errormsg"] = ex.Message;
                return View();
            }
            return View();
        }
    }
}
