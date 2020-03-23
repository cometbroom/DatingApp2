using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		private readonly DataContext _context;
		public ValuesController(DataContext context)
		{
			_context = context;

		}
		// GET api/values
		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> GetValues()    //IActionResult lets you return an Http response instead of plain text
		{
            var     values = await _context.Values.ToListAsync();   //To list async is in MECore
                                                    //Most collection methods have async methods?

            return Ok(values);  //IActionResult needs the ok method which make an http response.
		}

		[AllowAnonymous]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetValue(int id)
		{
            var     value = await _context.Values.FirstOrDefaultAsync(x => x.Id == id);  //Default value is null. Firstordefault is a microsoft entity framework.  
			return Ok(value);
		}

		// POST api/values
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}