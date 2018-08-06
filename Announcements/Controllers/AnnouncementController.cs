using Announcements.Entities;
using Announcements.Helpers;
using Announcements.Models;
using Announcements.Services;
using AnnouncementsAPI.Helpers;
using AnnouncementsAPI.Helpers.Extensions;
using AnnouncementsAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Announcements.Controllers
{
    [Route("api/announcements")]
    [ApiController]
    public class AnnouncementController : Controller
    {   
        private IAnnouncementsRepository _announcementsRepository;
        private IUrlHelper _urlHelper;
        private ITypeHelperService _typeHelperService;

        public AnnouncementController(IAnnouncementsRepository announcementsRepository, IUrlHelper urlHelper, ITypeHelperService typeHelperService)
        {
            _announcementsRepository = announcementsRepository;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetAnnouncements")]
        public IActionResult GetAnnouncements([FromQuery] AnnouncementsResourceParameters announcementsResourceParameters)
        {
            if (!_typeHelperService.TypeHasProperties<AnnouncementForGetting>(announcementsResourceParameters.Fields))
                return BadRequest();

            var announcementsFromDb = _announcementsRepository.GetAnnouncements(announcementsResourceParameters);

            var previousPageLink = announcementsFromDb.HasPrevious ?
                CreateAnnouncementsResourceUri(announcementsResourceParameters, ResourceUriType.PreviousPage) : null;

            var nextPageLink = announcementsFromDb.HasNext ?
                CreateAnnouncementsResourceUri(announcementsResourceParameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = announcementsFromDb.TotalCount,
                pageSize = announcementsFromDb.PageSize,
                currentPage = announcementsFromDb.CurrentPage,
                totalPages = announcementsFromDb.TotalPages,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            var announcementsForGetting = Mapper.Map<IEnumerable<AnnouncementForGetting>>(announcementsFromDb);

            return Ok(announcementsForGetting.ShapeData(announcementsResourceParameters.Fields));
        }

        [NonAction]
        private string CreateAnnouncementsResourceUri(AnnouncementsResourceParameters announcementsResourceParameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetAnnouncements", new
                    {
                        fields = announcementsResourceParameters.Fields,
                        searchString = announcementsResourceParameters.SearchString,
                        pageNumber = announcementsResourceParameters.PageNumber - 1,
                        pageSize = announcementsResourceParameters.PageSize
                    });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetAnnouncements", new
                    {
                        fields = announcementsResourceParameters.Fields,
                        searchString = announcementsResourceParameters.SearchString,
                        pageNumber = announcementsResourceParameters.PageNumber + 1,
                        pageSize = announcementsResourceParameters.PageSize
                    });
                default:
                    return _urlHelper.Link("GetAnnouncements", new
                    {
                        fields = announcementsResourceParameters.Fields,
                        searchString = announcementsResourceParameters.SearchString,
                        pageNumber = announcementsResourceParameters.PageNumber,
                        pageSize = announcementsResourceParameters.PageSize
                    });
            }
        }

        [HttpGet("{id}", Name = "GetAnnouncement")]
        public IActionResult GetAnnouncement(Guid id, [FromQuery] string fields)
        {
            if (!_typeHelperService.TypeHasProperties<AnnouncementForGetting>(fields))
                return BadRequest();

            var announcementFromDb = _announcementsRepository.GetAnnouncement(id);
            if (announcementFromDb == null)
                return NotFound();

            var announcementForGetting = Mapper.Map<AnnouncementForGetting>(announcementFromDb);

            return Ok(announcementForGetting.ShapeData(fields));
        }

        [HttpPost]
        public IActionResult CreateAnnouncement([FromBody] AnnouncementForCreation announcement)
        {
            if (announcement == null)
                return BadRequest();

            var announcementForDb = Mapper.Map<Announcement>(announcement);

            _announcementsRepository.AddAnnouncement(announcementForDb);

            return SaveAndGetAnnouncement(announcementForDb, "Creating an announcement failed on save.");
        }

        [HttpPost("{id}")]
        public IActionResult BlockAnnouncementCreation(Guid id)
        {
            if (_announcementsRepository.AnnouncementExists(id))
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            return NotFound();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchAnnouncement(Guid id, [FromBody] JsonPatchDocument<AnnouncementForUpdate> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var announcementFromDb = _announcementsRepository.GetAnnouncement(id);
            if (announcementFromDb == null)
            {
                var announcementForUpdate = new AnnouncementForUpdate();
                patchDoc.ApplyTo(announcementForUpdate, ModelState);

                TryValidateModel(announcementForUpdate);

                if (!ModelState.IsValid)
                    return new UnprocessableEntityObjectResult(ModelState);

                var announcementToCreate = Mapper.Map<Announcement>(announcementForUpdate);
                announcementToCreate.ID = id;

                _announcementsRepository.AddAnnouncement(announcementToCreate);

                return SaveAndGetAnnouncement(announcementToCreate, "Announcement for patching not found and for alternate, createing an announcement failed on save.");
            }

            var announcementToPatch = Mapper.Map<AnnouncementForUpdate>(announcementFromDb);
            patchDoc.ApplyTo(announcementToPatch, ModelState);

            TryValidateModel(announcementToPatch);

            if (!ModelState.IsValid)
                return new UnprocessableEntityObjectResult(ModelState);

            Mapper.Map(announcementToPatch, announcementFromDb);

            _announcementsRepository.UpdateAnnouncement(announcementFromDb);

            return SaveAndGetAnnouncement(announcementFromDb, "Patching an announcement failed on save.");
        }

        [HttpPut("{id}")]
        public IActionResult PutAnnouncement(Guid id, [FromBody] AnnouncementForUpdate announcement)
        {
            if (announcement == null)
                return BadRequest();

            var announcementFromDb = _announcementsRepository.GetAnnouncement(id);
            if (announcementFromDb == null)
            {
                var announcementToCreate = Mapper.Map<Announcement>(announcement);
                announcementToCreate.ID = id;

                _announcementsRepository.AddAnnouncement(announcementToCreate);

                return SaveAndGetAnnouncement(announcementToCreate, "Announcement for putting not found and for alternate, createing an announcement failed on save.");
            }

            Mapper.Map(announcement, announcementFromDb);

            _announcementsRepository.UpdateAnnouncement(announcementFromDb);

            return SaveAndGetAnnouncement(announcementFromDb, "Putting an announcement failed on save.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAnnouncement(Guid id)
        {
            var announcementFromDb = _announcementsRepository.GetAnnouncement(id);
            if (announcementFromDb == null)
                return NotFound();

            _announcementsRepository.DeleteAnnouncement(announcementFromDb);

            if (!_announcementsRepository.Save())
                throw new Exception("Deleting an announcement failed on save.");

            return NoContent();
        }

        [NonAction]
        private CreatedAtRouteResult SaveAndGetAnnouncement(Announcement announcement, string exceptionMessage)
        {
            if (!_announcementsRepository.Save())
                throw new Exception(exceptionMessage);

            var announcementForGetting = Mapper.Map<AnnouncementForGetting>(announcement);

            return CreatedAtRoute("GetAnnouncement", new { id = announcementForGetting.ID }, announcementForGetting);
        }
    }
}
