using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpiEyes.DAL;
using SpiEyes.Models;

namespace SpiEyes.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private DatabaseContext _databaseContext { get; set; }
    
    public UserController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    [HttpPost("Save")]
    public async Task<IActionResult> Save([FromBody] User user)
    {
        await _databaseContext.AddAsync(user);
        await _databaseContext.SaveChangesAsync();
        
        return Ok();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _databaseContext.Users.Where(x => x.Username == loginRequest.Username).FirstOrDefaultAsync();
        if (user is null)
            return new UnauthorizedResult();
        
        return new OkObjectResult(user);
    }
}