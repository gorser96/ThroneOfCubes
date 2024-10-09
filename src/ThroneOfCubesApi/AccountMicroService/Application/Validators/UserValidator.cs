using AccountMicroService.Application.Models;
using System.Text.RegularExpressions;

namespace AccountMicroService.Application.Validators;

public static class UserValidator
{
    public static void ValidateUser(RegisterModel registerModel)
    {
        ValidateUser(registerModel.Username, registerModel.Password);
    }

    private static void ValidateUser(string username, string password)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new BadHttpRequestException("Username is empty!");
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new BadHttpRequestException("Password is empty!");
        }

        if (username.Length < 3 || username.Length > 15)
        {
            throw new BadHttpRequestException("Username must be between 3 and 15 characters!");
        }

        if (password.Length < 4 || password.Length > 20)
        {
            throw new BadHttpRequestException("Password must be between 4 and 20 characters!");
        }

        if (Regex.IsMatch(username, "^[a-zA-Z0-9]+$"))
        {
            throw new BadHttpRequestException("The username must contain only letters and numbers!");
        }

        if (Regex.IsMatch(password, @"^[A-Za-z\d!@#$%^&*]+$"))
        {
            throw new BadHttpRequestException("You cannot use prohibited special characters in your password!");
        }
    }
}
