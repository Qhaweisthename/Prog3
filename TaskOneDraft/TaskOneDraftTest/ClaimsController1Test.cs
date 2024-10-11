
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskOneDraft.Areas.Identity.Data;
using TaskOneDraft.Controllers;
using TaskOneDraft.Models;

namespace TaskOneDraftTest
{
    public class ClaimsControllerTest
    {

        /*
         * Fact -- sets a method as unit test 
         * AAA -- > ARANGE, ACT, ASSERT
         */

        //pre setup 
        private readonly ClaimsController _controller;
        private readonly ApplicationDbContext _context;
        private readonly Mock<IWebHostEnvironment> _mockEnv;

        public ClaimsControllerTest()
        {
            //setup the in memory
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TaskOneDraft")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            _context.Database.EnsureCreated();
            //mock the ef environment 
            _mockEnv = new Mock<IWebHostEnvironment>();
            _mockEnv.Setup(env => env.WebRootPath).Returns("C:\\fakepath");

            //now in it the controller with the real context
            _controller = new ClaimsController(_context, _mockEnv.Object);

            //init -- the controller with the real contract 
           // _controller = new ClaimsController(_context, _mockEnv.Object);

            //Setup TempData 
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            _controller.TempData = tempData;



        }

        //set up the first 
        [Fact]
        public async void ClaimsPost_ValidModel_RedirectToList()
        {
            //AAA ---> ARRANGE, ACT, ASSERT 

            //arrange 
            var claims = new Claims
            {
                Id = 2,
                LecturerID = "123",
                FirstName = "John",
                LastName = "Smith",
                ClaimsPeriodStart = DateTime.Now.AddDays(-7),
                ClaimsPeriodEnd = DateTime.Now,
                HoursWorked = 5,
                RatePerHour = 100,
                DescriptionOfWork = "Testing db entry",
                SupportingDocuments = new List<IFormFile>(),
                Status = "Approved",

            };

            //act 
            var result = await _controller.Claims(claims);

            //assertions 
            var redirectResult = Assert.IsType<Microsoft.AspNetCore.Mvc.RedirectToActionResult>(result);
            Assert.Equal("claims", redirectResult.ActionName);
            //CHECKS THAT 1 VALUE GOT ADDED TO THE DB
            Assert.Equal(2,_context.Claims.Count());


        }

        //invalid claims 
        [Fact]
        public async Task ClaimsPost_InvalidModel_ReturnView()
        {
            //arrange 

            var claims = new Claims
            {
                LecturerID = "",
                FirstName = "",
                LastName = "",
                ClaimsPeriodStart = DateTime.Now.AddDays(-7),
                ClaimsPeriodEnd = DateTime.Now,
                HoursWorked = 10,
                RatePerHour = 50,
                DescriptionOfWork = "Testing",
                SupportingDocuments = new List<IFormFile>(),
                Status = "",
            };


            //act
            _controller.ModelState.AddModelError("FirstName", "Required");
                var result = await _controller.Claims(claims);

            //assert
            var ViewResult =Assert.IsType<ViewResult>(result);
            //ensures that the model returned back 
            Assert.IsType<Claims>(ViewResult.Model);
            Assert.False(_controller.ModelState.IsValid);   

        }

        [Fact]
        public void Approve_ClaimFound_RedirectToList()
        {
            //Arrange 
            var claim = new Claims
            {
                LecturerID = "2000",
                FirstName = "Test",
                LastName = "Testing",
                ClaimsPeriodStart = DateTime.Now.AddDays(7),
                ClaimsPeriodEnd = DateTime.Now,
                HoursWorked = 10,
                RatePerHour = 50,
                DescriptionOfWork = "Valid Claim",
              //  SupportingDocuments = new List<IFormFile>(),
                Status = "Pending",
            };
            _context.Claims.Add(claim);
            _context.SaveChanges();


            //Act
            var result = _controller.Approve(claim.Id);

            //Assert 
            var updatedClaim = _context.Claims.Find(claim.Id); //Fetch updated claim
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("List", redirectResult.ActionName);    
            Assert.Equal("Approved", updatedClaim.Status);
            Assert.Equal("Your claim has been approved", _controller.TempData["Message"]);
            Assert.Equal("success", _controller.TempData["MessageType"]);





        }
    }
}