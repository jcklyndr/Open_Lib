using Microsoft.AspNetCore.Mvc;
using OopProject.Models;
using OopProject.Services;

namespace OopProject.Controllers
{
    public class RequestAdminController : AdminHeaderController
    {
        private readonly IRequestRepository _requestRepository;

        public RequestAdminController(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        // Action to display all requests
        public async Task<IActionResult> AllRequest()
        {
            // Fetch all requests with Book and Category details
            var allRequest = await _requestRepository.GetAllRequestsWithDetailsAsync();

            return View(allRequest);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRequest(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id);

            if (request == null)
            {
                TempData["ErrorMessage"] = "Request not found.";
                return RedirectToAction("AllRequest");
            }

            return View(request); // Pass a single request to the view
        }



        [HttpPost]
        public async Task<IActionResult> UpdateRequest(int id, string status)
        {
            // Retrieve the request from the database
            var request = await _requestRepository.GetByIdAsync(id);
            if (request == null)
            {
                TempData["ErrorMessage"] = "Request not found.";
                return RedirectToAction("AllRequest");
            }

            try
            {
                // Parse the incoming status string to the enum value
                if (Enum.TryParse(typeof(Request.RequestStatus), status, out var parsedStatus))
                {
                    request.Status = (Request.RequestStatus)parsedStatus;

                    // Update the request in the repository
                    await _requestRepository.UpdateAsync(request);

                    TempData["SuccessMessage"] = "Request updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid status selected.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating request: {ex.Message}";
            }

            return RedirectToAction("AllRequest");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteRequestConfirmed(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            if (request == null)
            {
                TempData["ErrorMessage"] = "Request not found.";
                return RedirectToAction("AllRequest"); // Redirect to the appropriate view to manage requests
            }

            try
            {
                // Delete the request record
                await _requestRepository.DeleteAsync(id);

                TempData["SuccessMessage"] = "Request deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting request: {ex.Message}";
            }

            return RedirectToAction("AllRequest"); // Or to any view you want after deletion
        }



    }
}
