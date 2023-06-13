using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  public class AccountsController : BaseApiController
  {
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    
    public AccountsController(DataContext context , ITokenService tokenService, IMapper mapper)
    {
      this._mapper = mapper;
      this._context = context;
      this._tokenService = tokenService;
    }

    [HttpPost("register")]  // post : api/Accounts/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
      if (await UserExists(registerDto.UserName)) return BadRequest("User Name Is Taken");
      var user = _mapper.Map<AppUser>(registerDto);
      using var hmac = new HMACSHA512();

      
        user.UserName = registerDto.UserName.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        user.PasswordSalt = hmac.Key;
      
      _context.Users.Add(user);
      await _context.SaveChangesAsync();
      return new UserDto
      {
        UserName = user.UserName,
        Token = _tokenService.CreateToken(user),
        KnownAs = user.KnownAs,
        Gender  = user.Gender,
      };
    }

    [HttpPost("login")]  // post : api/Accounts/login
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
      var user = await _context.Users
      .Include(p => p.Photos)
      .SingleOrDefaultAsync(u => u.UserName == loginDto.UserName);
      if (user == null) return Unauthorized("Invalid Username");

      using var hmac = new HMACSHA512(user.PasswordSalt);

      var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

      for (int i = 0; i < computedHash.Length; i++)
      {
        if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
      }
      return new UserDto
      {
        UserName = user.UserName,
        Token = _tokenService.CreateToken(user),
        PhotoUrl = user.Photos.FirstOrDefault(img => img.IsMain)?.Url,
        KnownAs = user.KnownAs,
        Gender  = user.Gender,
      };
    }

    private async Task<bool> UserExists(string userName)
    {
      return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
    }
  }
}