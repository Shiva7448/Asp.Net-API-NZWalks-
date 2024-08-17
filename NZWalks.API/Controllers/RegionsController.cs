﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll ()
        {
            //Get data from database
            var regionsDomain = dbContext.Regions.ToList();

            //map domain models to DTO
            var regionsDto = new List<RegionDto>();
            foreach(var region in regionsDomain) 
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }
            //return
            return Ok(regionsDto);
        }

        //Get : https://localhost:portnumber/spi/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]

        public IActionResult GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            var regionDomain = dbContext.Regions.FirstOrDefault(x=> x.Id == id);

            if(regionDomain == null)
            {
                return NotFound();
            }

            var regionsDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };
                return Ok(regionsDto);
        }

        [HttpPost]

        public IActionResult Create([FromBody] AddRegionsDto addRegionDto)
        {
            var regionDomainModel = new Region
            {
                Code = addRegionDto.Code,
                Name = addRegionDto.Name,
                RegionImageUrl = addRegionDto.RegionImageUrl,
            };

            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            var regionDto = new RegionDto {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name =regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDto);
        }
    }
}