using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskOneDraft.Models;

namespace TaskOneDraftTest
{
    public class ClaimsTest
    {

        //fact --Arrange -- assert --- act
        [Fact]
        public void Claims_Properties_SetAndGetCorrectly()
        {
            //arange 
            var claim = new Claims
            {
                Id = 1,
                LecturerID = "200",
                FirstName = "Test",
                LastName = "Testing",
                ClaimsPeriodStart = new DateTime(2024, 1, 1),
                ClaimsPeriodEnd = new DateTime(2024, 1, 31),
                HoursWorked = 40,
                RatePerHour = 60,
                TotalAmount = 40,
                DescriptionOfWork = "Tut sessions with students ",
                SupportingDocuments = new List<IFormFile>(),
                Status = "Approved"
            };

            //assertions 
            Assert.Equal(1, claim.Id);
            Assert.Equal("200", claim.LecturerID);
            Assert.Equal("Test", claim.FirstName);
            Assert.Equal("Testing", claim.LastName);
            Assert.Equal(new DateTime(2024, 1, 1), claim.ClaimsPeriodStart);
            Assert.Equal(new DateTime(2024, 1, 31), claim.ClaimsPeriodEnd);
            Assert.Equal(40, claim.HoursWorked);
            Assert.Equal(60, claim.RatePerHour);
            Assert.Equal(40, claim.TotalAmount);
            Assert.Equal("Tut sessions with students ", claim.DescriptionOfWork);
            Assert.Empty(claim.SupportingDocuments);
            Assert.Equal("Approved", claim.Status);

        }//Method ends 
        [Fact]
        public void Status_Default()
        {
            var claim = new Claims();
            Assert.Equal("Approved", claim.Status);
        }


        //testing a test 
        [Fact]
        public void SupportingDocs_NotMapped() 
        {
            //arrange 
            var property = typeof(Claims).GetProperty(nameof(Claims.SupportingDocuments));
            //act
            var notMapped = property.GetCustomAttributes(typeof(NotMappedAttribute), false);
            //assert
            Assert.NotEmpty(notMapped);
        }



    }
}
