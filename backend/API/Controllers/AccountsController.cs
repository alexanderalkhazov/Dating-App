using API.Dtos;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountsController : BaseApiController
  {
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AccountsController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
    {
      this._mapper = mapper;
      this._userManager = userManager;
      this._tokenService = tokenService;
    }

    [HttpPost("register")]  // post : api/Accounts/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
      if (await UserExists(registerDto.UserName)) return BadRequest("User Name Is Taken");

      var user = _mapper.Map<AppUser>(registerDto);

      user.UserName = registerDto.UserName.ToLower();

      var result = await _userManager.CreateAsync(user, registerDto.Password);

      if (!result.Succeeded) return BadRequest(result.Errors);

      var roleResults = await _userManager.AddToRoleAsync(user,"Member");

      if (!roleResults.Succeeded) return BadRequest(roleResults.Errors);

      return new UserDto
      {
        UserName = user.UserName,
        Token = await _tokenService.CreateToken(user),
        KnownAs = user.KnownAs,
        Gender = user.Gender,
      };
    }

    [HttpPost("login")]  // post : api/Accounts/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
      var user = await _userManager.Users
      .Include(p => p.Photos)
      .SingleOrDefaultAsync(u => u.UserName == loginDto.UserName);
      if (user == null) return Unauthorized("Invalid Username");
      var result = await _userManager.CheckPasswordAsync(user,loginDto.Password);
      if (!result) return Unauthorized("Invalid password");
      return new UserDto
      {
        UserName = user.UserName,
        Token = await _tokenService.CreateToken(user),
        PhotoUrl = user.Photos.FirstOrDefault(img => img.IsMain)?.Url,
        KnownAs = user.KnownAs,
        Gender = user.Gender,
      };
    }

    private async Task<bool> UserExists(string userName)
    {
      return await _userManager.Users.AnyAsync(x => x.UserName == userName.ToLower());
    }
  }
}