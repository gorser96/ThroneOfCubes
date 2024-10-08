﻿namespace AccountMicroService.Application.Models;

public class UserViewModel
{
    public Guid Uid { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
