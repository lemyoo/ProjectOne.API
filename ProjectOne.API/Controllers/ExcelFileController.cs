﻿using Microsoft.AspNetCore.Mvc;
using ProjectOne.API.Dtos.InDtos;
using ProjectOne.API.Models;
using ProjectOne.API.Repository.Ticket;

namespace ProjectOne.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExcelFileController : ControllerBase
    {
        private readonly ITicketRepository ticketRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ExcelFileController(ITicketRepository ticketRepository, IWebHostEnvironment webHostEnvironment)
        {
            this.ticketRepository = ticketRepository;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public IActionResult UploadFile(IFormFile formFile)
        {
            try 
            {
                if(formFile.Length == 0) { return NoContent(); }

                var filePath = SaveFile(formFile);

                var tickets = ExcelHelper.Import<Ticket>(filePath);

                var checkedagainstDbAndFile = ticketRepository.CheckAgainstDBAndFileAsync(tickets).Result;

                return Ok(checkedagainstDbAndFile);

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("FileUploadFailed");
            }
        }

        private string SaveFile(IFormFile formFile)
        {
            if (formFile.Length == 0)
            {
                throw new BadHttpRequestException("File is empty.");
            }

            var extension = Path.GetExtension(formFile.FileName);

            var webRootPath = webHostEnvironment.WebRootPath;
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var folderPath = Path.Combine(webRootPath, "uploads");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{Guid.NewGuid()}.{extension}";
            var filePath = Path.Combine(folderPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            formFile.CopyTo(stream);

            return filePath;
        }

        [HttpPost]
        public IActionResult UploadData(List<InExcelDto> inExcelDto) 
        {
            var data = ticketRepository.UploadDataToDbAsync(inExcelDto);

            if (data)
            {
                return Ok("Data saved successfully!");
            }
            
            return BadRequest("There's an issue with inserting data into the Database!");
        }
    }
}
