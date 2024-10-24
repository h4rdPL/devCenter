﻿using DevCenter.Application.Common;
using DevCenter.Domain.Entieties;
using DevCenter.Domain.Enums.Users;
using DevCenter.Domain.Users;
using DevCenter.Infrastructure.Data;
using Google.Apis.Auth;

namespace DevCenter.Application.Users;

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ApplicationDbContext _context;

    public UserServices(IUserRepository userRepository, ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _context = context;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Register user using form
    /// </summary>
    /// <param name="username">user username</param>
    /// <param name="email">user email</param>
    /// <param name="password">user passowrd</param>
    /// <param name="role">user role</param>
    /// <returns></returns>
    public async Task<Result<User>> RegisterUser(string username, string email, string password, UserRoles role)
    {
        if (string.IsNullOrEmpty(username)) return Result<User>.Failure("Username cannot be empty.");
        if (string.IsNullOrEmpty(email)) return Result<User>.Failure("Email cannot be empty.");
        if (!IsValidEmail(email)) return Result<User>.Failure("Invalid email format.");
        if (string.IsNullOrEmpty(password)) return Result<User>.Failure("Password cannot be empty.");

        var existingUser = await _userRepository.GetUserByEmail(email);
        if (existingUser != null) return Result<User>.Failure("A user with this email already exists.");

        var user = new User
        {
            Username = username,
            Email = email,
            Password = _passwordHasher.Hash(password),
            Role = role
        };

        await _userRepository.Add(user);
        return Result<User>.Success(user);
    }

    /// <summary>
    /// Authenticate user using oAuth
    /// </summary>
    /// <param name="token">user jwt token</param>
    /// <returns>User data</returns>
    public async Task<Result<UserResponseDTO>> AuthenticateGoogleUser(string token)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(token);
            if (payload == null) return Result<UserResponseDTO>.Failure("Invalid Google token.");

            var user = await _userRepository.GetUserByEmail(payload.Email);
            var randomPassword = Guid.NewGuid().ToString();

            if (user == null)
            {
                user = new User
                {
                    Email = payload.Email,
                    Username = payload.Name,
                    Role = UserRoles.admin,
                    Password = _passwordHasher.Hash(randomPassword),
                    Company = null
                };
                await _userRepository.Add(user);
                await _context.SaveChangesAsync();
            }

            var userCompany = await _userRepository.GetCompanyByUser(user);
            user.Company = userCompany;

            var userDto = new UserResponseDTO(
                user.Username,
                user.Email,
                user.Role,
                user.Company != null ? new Company
                {
                    Id = user.Company.Id,
                    Name = user.Company.Name,
                    NIP = user.Company.NIP,
                    Country = user.Company.Country,
                    City = user.Company.City,
                    PostalCode = user.Company.PostalCode,
                    Street = user.Company.Street,
                    CompanyEmail = user.Company.CompanyEmail
                } : null
            );

            return Result<UserResponseDTO>.Success(userDto); 
        }
        catch (Exception ex)
        {
            return Result<UserResponseDTO>.Failure($"Error while authenticating Google user: {ex.Message}");
        }
    }



    /// <summary>
    /// Checking if email is valid
    /// </summary>
    /// <param name="email">email passed by user</param>
    /// <returns>true or false depend on validation</returns>
    private bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Add company to user
    /// </summary>
    /// <param name="userId">user id</param>
    /// <param name="company">company entity</param>
    /// <returns></returns>
    public async Task<Result> AddCompanyToUser(int userId, Company company)
    {
        if (string.IsNullOrEmpty(company.NIP) || string.IsNullOrEmpty(company.Name) ||
            string.IsNullOrEmpty(company.Country) || string.IsNullOrEmpty(company.City) ||
            string.IsNullOrEmpty(company.PostalCode) || string.IsNullOrEmpty(company.Street) ||
            string.IsNullOrEmpty(company.CompanyEmail))
        {
            return Result.Failure("Company details cannot be empty.");
        }

        if (company.NIP.Length != 10)
        {
            return Result.Failure("NIP must be a 10 digit number.");
        }

        if (!IsValidEmail(company.CompanyEmail))
        {
            return Result.Failure("Invalid company email.");
        }

        var user = await _userRepository.GetById(userId);

        if (user == null)
        {
            return Result.Failure("User not found.");
        }

        var newCompany = new Company
        {
            NIP = company.NIP,
            Name = company.Name,
            Country = company.Country,
            City = company.City,
            PostalCode = company.PostalCode,
            Street = company.Street,
            CompanyEmail = company.CompanyEmail
        };

        user.Company = newCompany;
        await _userRepository.Update(user);

        return Result.Success();
    }

    /// <summary>
    /// Get user company 
    /// </summary>
    /// <param name="user">user entity</param>
    /// <returns>company which belongs to the logged user</returns>
    public async Task<Company?> GetUserCompany(User user)
    {
        var company = await _userRepository.GetCompanyByUser(user);
        return company;
    }

    public Task<Result<TokenResponseDTO>> RefreshToken(string refreshToken)
    {
        throw new NotImplementedException();
    }
}