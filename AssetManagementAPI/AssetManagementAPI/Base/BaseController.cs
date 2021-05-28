using AssetManagementAPI.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repo;
        public BaseController(Repository repository)
        {
            this.repo = repository;
        }
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                IEnumerable<Entity> entities = repo.Get();
                return Ok(new { data = entities, status = "Ok" });
            }
            catch (Exception)
            {
                return StatusCode(404, new { status = "Not Found", errorMessage = "Failed to get the data" });
            }
        }
        [HttpPost]
        public ActionResult Insert(Entity entity)
        {
            try
            {
                var result = repo.Insert(entity);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, new { status = "Internal Server Error", errorMessage = "Failed to input the data" });
            }
        }
        [HttpPut]
        public ActionResult Update(Entity entity)
        {
            try
            {
                repo.Update(entity);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(400, new { status = "Bad Request", errorMessage = "Failed to update the data" });
            }
        }
        [HttpDelete("{key}")]
        public ActionResult Delete(Key key)
        {
            try
            {
                var del = repo.Delete(key);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(404, new { status = "Not Found", errorMessage = "Failed to delete the data" });
            }

        }

    }
}
